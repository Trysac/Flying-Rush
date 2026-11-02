using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FlyingRush.Game
{
    /// <summary>One row of a menu: an action, or a setting with a value that left/right changes.</summary>
    public sealed class MenuItem
    {
        public string Id;
        public string Label;
        public string Value;
    }

    /// <summary>A scoreboard line.</summary>
    public struct BoardRow
    {
        public int Place;
        public string Name;
        public int Kills;
        public int Deaths;
        public int BestStreak;
        public Color Color;
        public bool You;
    }

    /// <summary>Everything a menu screen shows.</summary>
    public sealed class MenuModel
    {
        public bool Logo;
        public string Title;
        /// <summary>0 plain, 1 good (green), -1 danger (red).</summary>
        public int TitleTone;
        public string Subtitle;
        public readonly List<(string Label, string Value, bool Highlight)> Body = new List<(string, string, bool)>();
        public List<BoardRow> Board;
        public readonly List<MenuItem> Items = new List<MenuItem>();
        public int Selected;
        public string Hint;
        public bool LightDim;

        public MenuItem Add(string id, string label, string value = null)
        {
            var item = new MenuItem { Id = id, Label = label, Value = value };
            Items.Add(item);
            return item;
        }

        public int IndexOf(string id) => Items.FindIndex(item => item.Id == id);
    }

    /// <summary>
    /// The UI Toolkit HUD, built in code and styled by Hud.uss: crosshair, hit marker, sniper
    /// scope, damage indicators, health and jumps, weapon and ammunition, scores and timer, kill
    /// feed, banners, the scoreboard and a generic menu renderer.
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public sealed class Hud : MonoBehaviour
    {
        const int Indicators = 4;
        const float HitMarkerSeconds = 0.18f;

        VisualElement root;
        VisualElement play;
        VisualElement crosshair;
        readonly VisualElement[] crossBars = new VisualElement[4];
        VisualElement hitMarker;
        VisualElement scope;
        VisualElement vignette;
        readonly VisualElement[] indicators = new VisualElement[Indicators];
        readonly float[] indicatorAge = new float[Indicators];
        int nextIndicator;
        Label targetName;
        VisualElement markers;
        readonly List<VisualElement> markerPool = new List<VisualElement>();
        Label modeTag;
        Label leftLabel, leftValue, rightLabel, rightValue, timer, limit;
        VisualElement leftChip, rightChip;
        VisualElement feed;
        Label health;
        VisualElement healthFill;
        readonly List<VisualElement> pips = new List<VisualElement>();
        Label weaponName, ammo, weaponAlt;
        VisualElement reloadFill;
        Label banner, bannerSub, notice, killNote;
        VisualElement respawn;
        Label respawnTitle, respawnBy;
        VisualElement board;
        VisualElement menu;
        VisualElement menuDim;
        VisualElement logo;
        Label menuTitle, menuSubtitle, menuHint;
        VisualElement menuBody;
        VisualElement menuItems;
        Label credit;
        readonly List<VisualElement> rows = new List<VisualElement>();
        float hitAge = 1f;
        float bannerHideAt = float.MaxValue;
        float noticeHideAt;
        float killNoteHideAt;

        /// <summary>A menu row was clicked (its index).</summary>
        public event Action<int> ItemClicked;
        /// <summary>The pointer moved over a menu row (its index).</summary>
        public event Action<int> ItemHovered;

        public MenuModel Menu { get; private set; }
        public VisualElement Root => root;

        void OnEnable()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
            root.Clear();
            root.pickingMode = PickingMode.Ignore;
            Build();
        }

        static VisualElement E(VisualElement parent, string classes, string name = null)
        {
            var element = new VisualElement { name = name, pickingMode = PickingMode.Ignore };
            foreach (string c in classes.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                element.AddToClassList(c);
            }
            parent?.Add(element);
            return element;
        }

        static Label L(VisualElement parent, string classes, string text = "", string name = null)
        {
            var label = new Label(text) { name = name, pickingMode = PickingMode.Ignore };
            foreach (string c in classes.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                label.AddToClassList(c);
            }
            parent?.Add(label);
            return label;
        }

        void Build()
        {
            play = E(root, "layer", "play-hud");
            vignette = E(play, "layer vignette", "vignette");
            markers = E(play, "layer", "markers");

            VisualElement center = E(play, "center");
            scope = E(center, "scope hidden", "scope");
            Line(scope, 1000f - 320f, 999f, 640f, 2f);
            Line(scope, 999f, 1000f - 320f, 2f, 640f);
            crosshair = E(center, "", "crosshair");
            for (int i = 0; i < 4; i++)
            {
                crossBars[i] = E(crosshair, "cross-bar");
            }
            E(crosshair, "cross-dot");
            hitMarker = E(center, "hidden", "hit-marker");
            for (int i = 0; i < 4; i++)
            {
                VisualElement stroke = E(hitMarker, "hit-bar");
                float angle = 45f + i * 90f;
                Vector2 at = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), -Mathf.Cos(angle * Mathf.Deg2Rad)) * 12f;
                stroke.style.left = at.x - 1.5f;
                stroke.style.top = at.y - 6f;
                stroke.style.rotate = new Rotate(angle);
            }
            for (int i = 0; i < Indicators; i++)
            {
                indicators[i] = E(center, "indicator hidden");
                E(indicators[i], "indicator__arrow");
            }
            targetName = L(center, "target-name shadow", "", "target-name");

            modeTag = L(play, "mode-tag", "FREE FOR ALL", "mode-tag");
            VisualElement top = E(play, "top-center");
            VisualElement scoreRow = E(top, "score-row");
            leftChip = E(scoreRow, "score-chip");
            leftLabel = L(leftChip, "score-chip__label", "YOU");
            leftValue = L(leftChip, "score-chip__value display", "0", "left-score");
            timer = L(scoreRow, "timer display", "--:--", "timer");
            rightChip = E(scoreRow, "score-chip");
            rightLabel = L(rightChip, "score-chip__label", "LEADER");
            rightValue = L(rightChip, "score-chip__value display", "0", "right-score");
            limit = L(top, "limit", "FIRST TO 15", "limit");
            feed = E(play, "feed", "feed");

            VisualElement vitals = E(play, "vitals");
            VisualElement healthRow = E(vitals, "health-row");
            health = L(healthRow, "health-value display shadow", "150", "health");
            VisualElement bar = E(healthRow, "health-bar");
            healthFill = E(bar, "health-bar__fill");
            VisualElement jumps = E(vitals, "jumps");
            L(jumps, "jumps__label", "JUMPS");
            for (int i = 0; i < FlyingRush.Core.Jumps.MaxJumps; i++)
            {
                pips.Add(E(jumps, "jump-pip"));
            }

            VisualElement weapon = E(play, "weapon");
            weaponName = L(weapon, "weapon-name shadow", "ASSAULT RIFLE", "weapon-name");
            ammo = L(weapon, "ammo display shadow", "30 / --", "ammo");
            VisualElement reload = E(weapon, "reload-bar");
            reloadFill = E(reload, "reload-bar__fill");
            weaponAlt = L(weapon, "weapon-alt", "", "weapon-alt");

            banner = L(play, "banner display shadow hidden", "", "banner");
            bannerSub = L(play, "banner-sub shadow hidden", "", "banner-sub");
            notice = L(play, "notice shadow hidden", "", "notice");
            killNote = L(play, "kill-note shadow hidden", "", "kill-note");
            respawn = E(play, "respawn hidden", "respawn");
            respawnTitle = L(respawn, "respawn__title display", "RESPAWNING IN 3");
            respawnBy = L(respawn, "respawn__by", "");
            board = E(play, "board hidden", "board");

            menu = E(root, "layer hidden", "menu");
            menuDim = E(menu, "layer menu-dim");
            VisualElement panel = E(menu, "menu-panel");
            logo = E(panel, "logo", "logo");
            L(logo, "logo__title display", "FLYING RUSH");
            L(logo, "logo__tag", "SEVEN JUMPS. NO GROUND RULES.");
            menuTitle = L(panel, "menu-title display", "", "menu-title");
            menuSubtitle = L(panel, "menu-subtitle", "", "menu-subtitle");
            menuBody = E(panel, "menu-body", "menu-body");
            menuItems = E(panel, "menu-items", "menu-items");
            menuHint = L(menu, "menu-hint", "", "menu-hint");
            credit = L(menu, "credit", "Rebuilt in Unity 6 from a 2021 Core game", "credit");
        }

        static void Line(VisualElement parent, float left, float top, float width, float height)
        {
            VisualElement line = E(parent, "scope-line");
            line.style.left = left - 680f;
            line.style.top = top - 680f;
            line.style.width = width;
            line.style.height = height;
        }

        public static void SetVisible(VisualElement element, bool visible) => element.EnableInClassList("hidden", !visible);

        public static bool Visible(VisualElement element) => !element.ClassListContains("hidden");

        // ---- Play HUD ----

        public void ShowPlay(bool visible) => SetVisible(play, visible);

        /// <summary>The crosshair opens with the weapon's spread (in pixels from the centre).</summary>
        public void SetCrosshair(bool visible, float gap)
        {
            SetVisible(crosshair, visible);
            gap = Mathf.Clamp(gap, 4f, 60f);
            const float length = 9f, thick = 2f;
            Place(crossBars[0], -thick / 2f, -gap - length, thick, length);
            Place(crossBars[1], -thick / 2f, gap, thick, length);
            Place(crossBars[2], -gap - length, -thick / 2f, length, thick);
            Place(crossBars[3], gap, -thick / 2f, length, thick);
        }

        static void Place(VisualElement element, float left, float top, float width, float height)
        {
            element.style.left = left;
            element.style.top = top;
            element.style.width = width;
            element.style.height = height;
        }

        public void SetScope(bool visible) => SetVisible(scope, visible);

        public bool ScopeVisible => Visible(scope);

        public void HitMarker(bool kill, bool headshot)
        {
            hitAge = 0f;
            SetVisible(hitMarker, true);
            hitMarker.EnableInClassList("hit--kill", kill);
            hitMarker.EnableInClassList("hit--head", headshot && !kill);
        }

        public bool HitMarkerVisible => Visible(hitMarker);

        /// <summary>A red arrow round the crosshair pointing at where the damage came from (degrees, 0 ahead).</summary>
        public void DamageFrom(float angle)
        {
            VisualElement indicator = indicators[nextIndicator];
            indicatorAge[nextIndicator] = 0f;
            nextIndicator = (nextIndicator + 1) % Indicators;
            indicator.style.rotate = new Rotate(angle);
            SetVisible(indicator, true);
        }

        public int VisibleIndicators
        {
            get
            {
                int count = 0;
                foreach (VisualElement indicator in indicators)
                {
                    count += Visible(indicator) ? 1 : 0;
                }
                return count;
            }
        }

        public void SetVignette(float strength) => vignette.style.opacity = Mathf.Clamp01(strength);

        public void SetTargetName(string text, Color color)
        {
            targetName.text = text ?? string.Empty;
            targetName.style.color = color;
        }

        /// <summary>Hides the banner, the notice and the kill note.</summary>
        public void ClearMessages()
        {
            HideBanner();
            SetVisible(notice, false);
            SetVisible(killNote, false);
        }

        /// <summary>The HUD's size in its own (reference) pixels.</summary>
        public Vector2 PanelSize => new Vector2(root.resolvedStyle.width, root.resolvedStyle.height);

        /// <summary>Teammate markers at viewport positions (0..1 from the bottom left).</summary>
        public void SetMarkers(List<(Vector2 position, Color color)> points)
        {
            Vector2 size = PanelSize;
            while (markerPool.Count < points.Count)
            {
                markerPool.Add(E(markers, "marker"));
            }
            for (int i = 0; i < markerPool.Count; i++)
            {
                bool used = i < points.Count;
                SetVisible(markerPool[i], used);
                if (used)
                {
                    markerPool[i].style.left = points[i].position.x * size.x - 6f;
                    markerPool[i].style.top = (1f - points[i].position.y) * size.y - 6f;
                    markerPool[i].style.backgroundColor = points[i].color;
                }
            }
        }

        public void SetTopBar(string mode, string leftName, string left, Color leftColor, string rightName, string right, Color rightColor,
            string clock, string limitText)
        {
            modeTag.text = mode;
            leftLabel.text = leftName;
            leftValue.text = left;
            leftValue.style.color = leftColor;
            rightLabel.text = rightName;
            rightValue.text = right;
            rightValue.style.color = rightColor;
            timer.text = clock;
            limit.text = limitText;
        }

        public string LeftScore => leftValue.text;
        public string RightScore => rightValue.text;
        public string Clock => timer.text;

        public void SetFeed(List<(string killer, Color killerColor, string weapon, string victim, Color victimColor, bool you)> lines)
        {
            feed.Clear();
            foreach ((string killer, Color killerColor, string weapon, string victim, Color victimColor, bool you) in lines)
            {
                VisualElement line = E(feed, you ? "feed-line feed-line--you" : "feed-line");
                if (!string.IsNullOrEmpty(killer))
                {
                    L(line, "", killer).style.color = killerColor;
                }
                L(line, "feed-weapon", weapon);
                L(line, "", victim).style.color = victimColor;
            }
        }

        public int FeedLines => feed.childCount;

        public string FeedText(int index)
        {
            var parts = new List<string>();
            foreach (VisualElement child in feed[index].Children())
            {
                parts.Add(((Label)child).text);
            }
            return string.Join(" ", parts);
        }

        public void SetVitals(float hitPoints, float fraction, int jumpsLeft)
        {
            health.text = Mathf.CeilToInt(hitPoints).ToString();
            healthFill.style.width = Length.Percent(Mathf.Clamp01(fraction) * 100f);
            healthFill.style.backgroundColor = fraction > 0.5f ? Palette.Good : fraction > 0.25f ? Palette.Amber : Palette.Danger;
            for (int i = 0; i < pips.Count; i++)
            {
                pips[i].EnableInClassList("jump-pip--used", i >= jumpsLeft);
            }
        }

        public string HealthText => health.text;

        public int JumpPipsLit
        {
            get
            {
                int count = 0;
                foreach (VisualElement pip in pips)
                {
                    count += pip.ClassListContains("jump-pip--used") ? 0 : 1;
                }
                return count;
            }
        }

        public void SetWeapon(string name, Color color, string ammoText, bool low, float reload, string alt)
        {
            weaponName.text = name;
            weaponName.style.color = color;
            ammo.text = ammoText;
            ammo.EnableInClassList("ammo--low", low);
            reloadFill.style.width = Length.Percent(reload * 100f);
            weaponAlt.text = alt;
        }

        public string WeaponName => weaponName.text;
        public string AmmoText => ammo.text;

        public void Banner(string text, string sub, float seconds, Color color)
        {
            banner.text = text;
            banner.style.color = color;
            bannerSub.text = sub ?? string.Empty;
            SetVisible(banner, true);
            SetVisible(bannerSub, !string.IsNullOrEmpty(sub));
            banner.AddToClassList("banner--pop");
            banner.schedule.Execute(() => banner.RemoveFromClassList("banner--pop")).StartingIn(120);
            bannerHideAt = Time.unscaledTime + seconds;
        }

        public void HideBanner()
        {
            SetVisible(banner, false);
            SetVisible(bannerSub, false);
            bannerHideAt = float.MaxValue;
        }

        public string BannerText => Visible(banner) ? banner.text : string.Empty;

        public void Notice(string text, float seconds = 2f)
        {
            notice.text = text;
            SetVisible(notice, true);
            noticeHideAt = Time.unscaledTime + seconds;
        }

        public string NoticeText => Visible(notice) ? notice.text : string.Empty;

        public void KillNote(string text, Color color)
        {
            killNote.text = text;
            killNote.style.color = color;
            SetVisible(killNote, true);
            killNoteHideAt = Time.unscaledTime + 1.6f;
        }

        public string KillNoteText => Visible(killNote) ? killNote.text : string.Empty;

        public void SetRespawn(bool visible, string title, string by)
        {
            SetVisible(respawn, visible);
            respawnTitle.text = title;
            respawnBy.text = by;
        }

        public bool RespawnVisible => Visible(respawn);

        public void ShowBoard(bool visible, string title, List<BoardRow> lines)
        {
            SetVisible(board, visible);
            play.EnableInClassList("board-open", visible);
            if (visible)
            {
                FillBoard(board, title, lines);
            }
        }

        public bool BoardVisible => Visible(board);

        static void FillBoard(VisualElement target, string title, List<BoardRow> lines)
        {
            target.Clear();
            if (!string.IsNullOrEmpty(title))
            {
                L(target, "board-title display", title);
            }
            VisualElement head = E(target, "board-row board-row--head");
            L(head, "board-place", "#");
            L(head, "board-name", "FIGHTER");
            L(head, "board-num", "KILLS");
            L(head, "board-num", "DEATHS");
            L(head, "board-num", "STREAK");
            foreach (BoardRow line in lines)
            {
                VisualElement row = E(target, line.You ? "board-row board-row--you" : "board-row");
                L(row, "board-place", line.Place.ToString());
                VisualElement swatch = E(row, "board-swatch");
                swatch.style.backgroundColor = line.Color;
                L(row, "board-name", line.Name);
                L(row, "board-num display", line.Kills.ToString());
                L(row, "board-num", line.Deaths.ToString());
                L(row, "board-num", line.BestStreak.ToString());
            }
        }

        void Update()
        {
            float dt = Time.unscaledDeltaTime;
            if (Visible(hitMarker))
            {
                hitAge += dt;
                hitMarker.style.opacity = 1f - hitAge / HitMarkerSeconds;
                if (hitAge >= HitMarkerSeconds)
                {
                    SetVisible(hitMarker, false);
                }
            }
            for (int i = 0; i < Indicators; i++)
            {
                if (!Visible(indicators[i]))
                {
                    continue;
                }
                indicatorAge[i] += dt;
                indicators[i].style.opacity = Mathf.Clamp01(1.4f - indicatorAge[i]);
                if (indicatorAge[i] > 1.4f)
                {
                    SetVisible(indicators[i], false);
                }
            }
            if (Time.unscaledTime >= bannerHideAt)
            {
                HideBanner();
            }
            if (Visible(notice) && Time.unscaledTime >= noticeHideAt)
            {
                SetVisible(notice, false);
            }
            if (Visible(killNote) && Time.unscaledTime >= killNoteHideAt)
            {
                SetVisible(killNote, false);
            }
        }

        // ---- Menus ----

        public void ShowMenu(MenuModel model)
        {
            Menu = model;
            SetVisible(menu, true);
            menuDim.EnableInClassList("menu-dim--light", model.LightDim);
            SetVisible(logo, model.Logo);
            SetVisible(credit, model.Logo);
            menuTitle.text = model.Title ?? string.Empty;
            SetVisible(menuTitle, !string.IsNullOrEmpty(model.Title));
            menuTitle.EnableInClassList("menu-title--good", model.TitleTone > 0);
            menuTitle.EnableInClassList("menu-title--danger", model.TitleTone < 0);
            menuSubtitle.text = model.Subtitle ?? string.Empty;
            SetVisible(menuSubtitle, !string.IsNullOrEmpty(model.Subtitle));
            menuBody.Clear();
            foreach ((string label, string value, bool highlight) in model.Body)
            {
                VisualElement row = E(menuBody, highlight ? "body-row body-row--highlight" : "body-row");
                L(row, "body-row__label", label);
                L(row, "body-row__value", value);
            }
            if (model.Board != null)
            {
                FillBoard(E(menuBody, "board board--inline"), null, model.Board);
            }
            SetVisible(menuBody, model.Body.Count > 0 || model.Board != null);
            menuItems.Clear();
            rows.Clear();
            for (int i = 0; i < model.Items.Count; i++)
            {
                int index = i;
                var row = new VisualElement();
                row.AddToClassList("menu-item");
                var label = new Label(model.Items[i].Label) { pickingMode = PickingMode.Ignore };
                label.AddToClassList("menu-item__label");
                row.Add(label);
                var value = new Label(model.Items[i].Value ?? string.Empty) { pickingMode = PickingMode.Ignore };
                value.AddToClassList("menu-item__value");
                row.Add(value);
                row.RegisterCallback<ClickEvent>(_ => ItemClicked?.Invoke(index));
                row.RegisterCallback<PointerEnterEvent>(_ => ItemHovered?.Invoke(index));
                menuItems.Add(row);
                rows.Add(row);
            }
            menuHint.text = model.Hint ?? string.Empty;
            RefreshMenu();
        }

        /// <summary>Redraws the selection and the values after a change.</summary>
        public void RefreshMenu()
        {
            if (Menu == null)
            {
                return;
            }
            for (int i = 0; i < rows.Count; i++)
            {
                rows[i].EnableInClassList("menu-item--selected", i == Menu.Selected);
                ((Label)rows[i][1]).text = Menu.Items[i].Value ?? string.Empty;
            }
        }

        public void HideMenu()
        {
            Menu = null;
            SetVisible(menu, false);
        }

        public bool MenuVisible => Visible(menu);
    }
}
