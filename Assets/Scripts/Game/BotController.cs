using System.Collections.Generic;
using FlyingRush.Core;
using UnityEngine;

namespace FlyingRush.Game
{
    /// <summary>What the bots can see of the match: the bodies, the pads, the map and the guns.</summary>
    public sealed class Arena
    {
        public Match Match;
        public MapLayout Map;
        public WaypointGraph Graph;
        public Combat Combat;
        public MapView View;
        public readonly List<FighterBody> Bodies = new List<FighterBody>();
    }

    /// <summary>
    /// A bot: the core's brain decides, this follows waypoint paths (jumping up to platforms the
    /// way the graph was linked), looks for enemies with line-of-sight checks, turns its view at
    /// its profile's speed and pulls the trigger when its imperfect aim is on target.
    /// </summary>
    public sealed class BotController : MonoBehaviour
    {
        const float SenseInterval = 0.15f;
        const float ViewCone = 80f;
        const float HearingRange = 14f;
        const float ArriveDistance = 0.9f;

        FighterBody body;
        BotBrain brain;
        Arena arena;
        readonly List<int> path = new List<int>();
        int pathIndex;
        int pathGoalNode = -1;
        FighterBody target;
        FighterBody lastAttacker;
        Vector3 lastSeen;
        float sinceSeen = float.PositiveInfinity;
        float senseTimer;
        float aimTimer;
        (float yaw, float pitch) aimOffset;
        float stuckTimer;
        Vector3 lastProgressPosition;
        float repathTimer;
        Rng rng;
        int coverNode = -1;
        int roamGoal = -1;
        float coverTimer;

        public BotBrain Brain => brain;
        public FighterBody Body => body;
        public FighterBody Target => target;
        public BotGoal Goal => brain.Goal;

        public void Init(FighterBody fighterBody, BotProfile profile, Arena world, int seed)
        {
            body = fighterBody;
            arena = world;
            rng = new Rng(seed);
            brain = new BotBrain(profile, new Rng(seed * 7 + 3));
        }

        /// <summary>Forget everything after a respawn.</summary>
        public void Reset()
        {
            path.Clear();
            pathGoalNode = -1;
            target = null;
            lastAttacker = null;
            sinceSeen = float.PositiveInfinity;
            roamGoal = -1;
            coverNode = -1;
            stuckTimer = 0f;
            lastProgressPosition = body.transform.position;
        }

        /// <summary>Someone hurt this bot: it turns to them even if it did not see them.</summary>
        public void HurtBy(FighterBody attacker)
        {
            if (attacker != null && attacker != body)
            {
                lastAttacker = attacker;
                lastSeen = attacker.transform.position;
                sinceSeen = 0f;
            }
        }

        public void Tick(bool canAct, float deltaTime)
        {
            if (!body.Fighter.Alive || deltaTime <= 0f)
            {
                return;
            }
            if (!canAct)
            {
                body.Drive(Vector3.zero, false, false, deltaTime);
                return;
            }
            senseTimer -= deltaTime;
            if (senseTimer <= 0f)
            {
                senseTimer = SenseInterval;
                Sense();
            }
            sinceSeen += deltaTime;
            coverTimer -= deltaTime;
            if (target != null && !target.Fighter.Alive)
            {
                target = null;
            }

            PadView pad = NearestPad(out float padDistance);
            Gun gun = body.Loadout.Current;
            BotGoal goal = brain.Decide(new BotSenses
            {
                HealthFraction = body.Fighter.Vitals.Fraction,
                HasPowerWeapon = body.Loadout.HasPower,
                EnemyVisible = target != null,
                EnemyDistance = target != null ? Vector3.Distance(body.Eye, target.Chest) : float.PositiveInfinity,
                SinceEnemySeen = sinceSeen,
                NearestPadDistance = padDistance,
                SinceHurt = body.Fighter.Vitals.SinceHurt,
                WeaponRange = gun.Stats.Range,
            });

            Vector3 wish = Vector3.zero;
            bool jump = false;
            switch (goal)
            {
                case BotGoal.Engage:
                    wish = Fight(deltaTime, ref jump);
                    break;
                case BotGoal.Retreat:
                    wish = FollowTo(CoverNode(), deltaTime, ref jump);
                    LookAlongPathOr(target != null ? target.Chest : lastSeen, deltaTime, target != null);
                    break;
                case BotGoal.CollectWeapon:
                    wish = pad != null ? FollowTo(arena.Graph.Nearest(pad.Pad.Position), deltaTime, ref jump) : Vector3.zero;
                    LookAlongPathOr(Vector3.zero, deltaTime, false);
                    break;
                case BotGoal.Hunt:
                    wish = FollowTo(arena.Graph.Nearest(lastSeen.ToCore()), deltaTime, ref jump);
                    LookAlongPathOr(lastSeen + Vector3.up, deltaTime, sinceSeen < 1.5f);
                    break;
                default:
                    if (roamGoal < 0 || (pathGoalNode == roamGoal && pathIndex >= path.Count))
                    {
                        roamGoal = RoamNode();
                    }
                    wish = FollowTo(roamGoal, deltaTime, ref jump);
                    LookAlongPathOr(Vector3.zero, deltaTime, false);
                    break;
            }
            body.Drive(wish, jump, false, deltaTime);
            ChooseWeapon();
            if (gun.Magazine == 0 || (target == null && gun.Magazine < gun.Stats.Magazine / 3))
            {
                gun.StartReload();
            }
        }

        void Sense()
        {
            FighterBody best = null;
            float bestDistance = float.MaxValue;
            Vector3 eye = body.Eye;
            Vector3 forward = Quaternion.Euler(0f, body.Yaw, 0f) * Vector3.forward;
            foreach (FighterBody other in arena.Bodies)
            {
                if (other == body || other == null || !other.Fighter.Alive || !arena.Match.AreEnemies(body.Fighter, other.Fighter))
                {
                    continue;
                }
                Vector3 to = other.Chest - eye;
                float distance = to.magnitude;
                if (distance > brain.Profile.SightRange)
                {
                    continue;
                }
                bool inCone = Vector3.Angle(forward, new Vector3(to.x, 0f, to.z)) < ViewCone;
                bool noticed = inCone || distance < HearingRange || other == lastAttacker;
                if (!noticed || Physics.Linecast(eye, other.Chest, Layers.WorldMask, QueryTriggerInteraction.Ignore))
                {
                    continue;
                }
                // The one who just hurt us comes first, then the closest.
                float score = distance - (other == lastAttacker ? 20f : 0f);
                if (score < bestDistance)
                {
                    bestDistance = score;
                    best = other;
                }
            }
            if (best != target)
            {
                target = best;
                if (best != null)
                {
                    brain.Acquire();
                    aimTimer = 0f;
                }
            }
            if (target != null)
            {
                lastSeen = target.transform.position;
                sinceSeen = 0f;
            }
        }

        Vector3 Fight(float deltaTime, ref bool jump)
        {
            brain.Track(deltaTime);
            brain.UpdateStrafe(deltaTime);
            WeaponStats stats = body.Loadout.Current.Stats;
            Vector3 aimPoint = stats.Kind == WeaponKind.Rockets ? target.transform.position + Vector3.up * 0.3f : target.Chest;
            Vector3 targetVelocity = target.Velocity;
            Vector3 to = aimPoint - body.Eye;
            Vector3 side = Vector3.Cross(Vector3.up, to.normalized);
            float sideways = Vector3.Dot(targetVelocity, side);

            aimTimer -= deltaTime;
            if (aimTimer <= 0f)
            {
                aimTimer = 0.25f;
                aimOffset = brain.AimOffset(sideways);
            }
            float wantYaw = Mathf.Atan2(to.x, to.z) * Mathf.Rad2Deg + aimOffset.yaw;
            float wantPitch = Mathf.Atan2(to.y, new Vector2(to.x, to.z).magnitude) * Mathf.Rad2Deg + aimOffset.pitch;
            float yaw = brain.TurnTowards(body.Yaw, wantYaw, deltaTime);
            float pitch = Mathf.MoveTowards(body.Pitch, wantPitch, brain.Profile.TurnSpeed * 0.7f * deltaTime);
            body.Look(yaw, pitch);

            float off = Vector3.Angle(body.AimDirection, to);
            Gun gun = body.Loadout.Current;
            if (brain.ShouldFire(off, stats.Automatic, deltaTime))
            {
                arena.Combat.Fire(body, body.AimDirection, stats.Kind == WeaponKind.Sniper);
            }

            // Keep a distance that suits the weapon and sidestep.
            float distance = to.magnitude;
            float preferred = stats.Kind switch
            {
                WeaponKind.Shotgun => 6f,
                WeaponKind.Sniper => 30f,
                WeaponKind.Railgun => 22f,
                WeaponKind.Rockets => 14f,
                _ => 14f,
            };
            Vector3 flatTo = new Vector3(to.x, 0f, to.z).normalized;
            Vector3 strafe = Vector3.Cross(Vector3.up, flatTo) * brain.StrafeDirection;
            float approach = distance > preferred * 1.3f ? 1f : distance < preferred * 0.6f ? -0.8f : 0f;
            Vector3 wish = (flatTo * approach + strafe * 0.8f).normalized;
            if (gun.Reloading)
            {
                wish = (strafe - flatTo * 0.5f).normalized;
            }
            wish = AvoidEdges(wish);
            if (brain.WantsCombatJump() || (!body.Grounded && body.Velocity.y < -3f && body.Jumps.Left > 3 && GroundBelow() > 4f))
            {
                jump = true;
            }
            return wish;
        }

        /// <summary>Cancels a move that would walk off a platform's edge.</summary>
        Vector3 AvoidEdges(Vector3 wish)
        {
            if (!body.Grounded || body.transform.position.y < 0.5f || wish.sqrMagnitude < 0.01f)
            {
                return wish;
            }
            Vector3 probe = body.transform.position + wish.normalized * 1.2f + Vector3.up * 0.5f;
            return Physics.Raycast(probe, Vector3.down, 1.6f, Layers.WorldMask, QueryTriggerInteraction.Ignore) ? wish : Vector3.zero;
        }

        float GroundBelow()
        {
            return Physics.Raycast(body.transform.position + Vector3.up * 0.2f, Vector3.down, out RaycastHit hit, 30f, Layers.WorldMask)
                ? hit.distance
                : 30f;
        }

        /// <summary>Walks a path to a waypoint, re-planning when the goal changes or the bot is stuck.</summary>
        Vector3 FollowTo(int goalNode, float deltaTime, ref bool jump)
        {
            if (goalNode < 0)
            {
                return Vector3.zero;
            }
            repathTimer -= deltaTime;
            if (goalNode != pathGoalNode || repathTimer <= 0f)
            {
                Plan(goalNode);
            }
            if (pathIndex >= path.Count)
            {
                return Vector3.zero;
            }
            Vector3 feet = body.transform.position;
            Vector3 next = arena.Graph.Nodes[path[pathIndex]].ToUnity();
            Vector3 previous = pathIndex > 0 ? arena.Graph.Nodes[path[pathIndex - 1]].ToUnity() : feet;
            Vector3 flat = new Vector3(next.x - feet.x, 0f, next.z - feet.z);
            float rise = next.y - feet.y;

            if (flat.magnitude < ArriveDistance && rise > -1.5f && rise < 1.2f)
            {
                pathIndex++;
                stuckTimer = 0f;
                lastProgressPosition = feet;
                return flat.normalized * 0.3f;
            }

            Vector3 wish;
            if (rise > 0.6f && feet.y < next.y + 0.25f)
            {
                // Climb as the link was checked: straight up over the lower waypoint, then across.
                Vector3 column = new Vector3(previous.x - feet.x, 0f, previous.z - feet.z);
                wish = column.magnitude > 0.5f && pathIndex > 0 ? column.normalized * 0.8f : Vector3.zero;
                bool nearApex = body.Velocity.y < 1.5f;
                jump = body.Grounded || (nearApex && body.Jumps.Left > 0);
                // Once high enough, start drifting over.
                if (feet.y > next.y - 0.6f)
                {
                    wish = flat.normalized;
                }
            }
            else
            {
                wish = flat.normalized;
                // Fell short of a platform: fly back up with the spare jumps.
                if (!body.Grounded && rise > 0.3f && body.Velocity.y < 0f && body.Jumps.Left > 0)
                {
                    jump = true;
                }
            }

            stuckTimer += deltaTime;
            if (stuckTimer > 1.2f)
            {
                if (Vector3.Distance(feet, lastProgressPosition) < 1f)
                {
                    jump = true;
                    if (stuckTimer > 2.6f)
                    {
                        // Give up on this route: plan again from here, or roam somewhere else.
                        path.Clear();
                        roamGoal = RoamNode();
                        repathTimer = 0f;
                        stuckTimer = 0f;
                    }
                }
                else
                {
                    stuckTimer = 0f;
                    lastProgressPosition = feet;
                }
            }
            return wish;
        }

        void Plan(int goalNode)
        {
            repathTimer = 4f;
            pathGoalNode = goalNode;
            path.Clear();
            int start = arena.Graph.Nearest(body.transform.position.ToCore());
            path.AddRange(arena.Graph.Path(start, goalNode));
            pathIndex = path.Count > 1 ? 1 : 0;
            stuckTimer = 0f;
            lastProgressPosition = body.transform.position;
        }

        /// <summary>Looks where it is going, or at a point of interest.</summary>
        void LookAlongPathOr(Vector3 point, float deltaTime, bool usePoint)
        {
            Vector3 direction;
            if (usePoint)
            {
                direction = point - body.Eye;
            }
            else if (pathIndex < path.Count)
            {
                direction = arena.Graph.Nodes[path[pathIndex]].ToUnity() + Vector3.up * 1.5f - body.Eye;
            }
            else
            {
                return;
            }
            if (new Vector2(direction.x, direction.z).sqrMagnitude < 0.04f)
            {
                return;
            }
            float wantYaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float wantPitch = Mathf.Clamp(Mathf.Atan2(direction.y, new Vector2(direction.x, direction.z).magnitude) * Mathf.Rad2Deg, -30f, 30f);
            body.Look(brain.TurnTowards(body.Yaw, wantYaw, deltaTime * 0.6f),
                Mathf.MoveTowards(body.Pitch, wantPitch, 90f * deltaTime));
        }

        int RoamNode()
        {
            IReadOnlyList<Vec3> nodes = arena.Graph.Nodes;
            // Prefer somewhere within 35 m, with a pull toward the high ground.
            for (int attempt = 0; attempt < 8; attempt++)
            {
                int node = brain.PickRoamTarget(nodes.Count);
                float distance = Vector3.Distance(nodes[node].ToUnity(), body.transform.position);
                if (distance > 6f && distance < 35f && (nodes[node].Y > 2f || rng.Value() < 0.6f))
                {
                    return node;
                }
            }
            return brain.PickRoamTarget(nodes.Count);
        }

        /// <summary>A waypoint near the bot that the threat cannot see.</summary>
        int CoverNode()
        {
            if (coverNode >= 0 && coverTimer > 0f)
            {
                return coverNode;
            }
            coverTimer = 1.5f;
            coverNode = FindCover();
            return coverNode;
        }

        int FindCover()
        {
            Vector3 threat = (target != null ? target.Eye : lastSeen + Vector3.up * 1.6f);
            IReadOnlyList<Vec3> nodes = arena.Graph.Nodes;
            int best = -1;
            float bestDistance = float.MaxValue;
            for (int i = 0; i < nodes.Count; i++)
            {
                Vector3 node = nodes[i].ToUnity();
                float distance = Vector3.Distance(node, body.transform.position);
                if (distance > 25f || distance >= bestDistance)
                {
                    continue;
                }
                if (Physics.Linecast(threat, node + Vector3.up * 1.2f, Layers.WorldMask, QueryTriggerInteraction.Ignore))
                {
                    best = i;
                    bestDistance = distance;
                }
            }
            return best >= 0 ? best : RoamNode();
        }

        PadView NearestPad(out float distance)
        {
            PadView best = null;
            distance = float.PositiveInfinity;
            if (arena.View == null)
            {
                return null;
            }
            foreach (PadView pad in arena.View.Pads)
            {
                if (!pad.Pad.Available)
                {
                    continue;
                }
                float d = Vector3.Distance(pad.transform.position, body.transform.position);
                if (d < distance)
                {
                    distance = d;
                    best = pad;
                }
            }
            return best;
        }

        /// <summary>The power weapon for its range, the rifle otherwise.</summary>
        void ChooseWeapon()
        {
            Loadout loadout = body.Loadout;
            if (!loadout.HasPower)
            {
                return;
            }
            float distance = target != null ? Vector3.Distance(body.Eye, target.Chest) : 20f;
            bool power = loadout.Power.Kind switch
            {
                WeaponKind.Shotgun => distance < 16f,
                WeaponKind.Sniper => distance > 9f,
                _ => true,
            };
            bool holdingPower = loadout.Current == loadout.Power;
            if (power != holdingPower && !loadout.Current.Reloading)
            {
                loadout.Switch();
            }
        }
    }
}
