# The 2021 version

Flying Rush was first made in March 2021 in **Core**, the game creation platform by Manticore
Games, on its "Deathmatch" framework. It was published on Core as "Death Cage". Core is no
longer available to build or run that project, so the game was rebuilt from scratch in Unity 6
(see the [main README](../../README.md)). Nothing from the Core project is used by the Unity
version.

## What it was

Its published description called it a Free For All shooter where the most advanced weapons are
scarce, and where you and your enemies jump so much that you seem to fly across the map.
The project data says how:

| Setting | Value in the Core project |
|---|---|
| Mode | Free For All (team mode `ffa`), up to 10 players |
| Round rule | First to **15 kills** wins the round; no round time limit; 5 s round end, then a new round |
| Round message | "Eliminate the Enemies" |
| Health | 150 hit points |
| Jumps | **7 jumps** before touching the ground again: the "flying" |
| Starting weapon | An assault rifle, given on the first spawn |
| Other weapons | Placed high on the map, about 40 m up: sniper rifles, shotguns, a submachine gun, rocket and grenade launchers, crossbows, gatling guns, a laser cannon and a community railgun |
| Respawn | Round-robin over two sets of spawn points |

The map was a closed cage about 150 by 145 m: stacked shipping containers, cover walls, tall
walls with ledges to climb with the extra jumps, a sniper area, a low crouching area, two team
bases, doors, stairs, floating platforms and a kill zone under the floor.

## Where the files are

The Core project (`Data/`, `CommunityContent/`, `Meta.pbt`, `PublishData/`, the editor
screenshots and user settings) was removed from the working tree when the Unity version
started; it stays in the git history, for example at commit `c0225a7`:

```bash
git checkout c0225a7 -- Data Meta.pbt
```

Its Lua scripts are Core's own framework and community content made for Core; they are not
reused here.
