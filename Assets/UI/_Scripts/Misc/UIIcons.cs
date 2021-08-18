using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.UI._Scripts.Misc
{
    class UIIcons
    {
        public static void UpdateTMText(TMPro.TextMeshProUGUI textField)
        {
            string text = textField.text;

            bool hasEmoji = false;

            foreach (KeyValuePair<string, string> icon in Icons)
            {
                if (text.Contains(string.Format("[{0}]", icon.Key)))
                {
                    hasEmoji = true;
                    break;
                }
            }

            textField.font = hasEmoji ? UIUtil.Skin.EmojiFonts : UIUtil.Skin.DefaultFonts;
            textField.UpdateFontAsset();

            if (!hasEmoji)
                return;

            foreach (KeyValuePair<string, string> icon in Icons)
            {
                text = text.Replace(string.Format("[{0}]", icon.Key), icon.Value);
            }

            textField.text = text;
        }

        public static void UpdateAllTMText()
        {
            TMPro.TextMeshProUGUI[] textFields = GameObject.FindObjectsOfType<TMPro.TextMeshProUGUI>();

            foreach (TMPro.TextMeshProUGUI textField in textFields)
                UpdateTMText(textField);
        }

        public static Dictionary<string, string> Icons = new Dictionary<string, string>
        {
            ["address-book"]                = "\uf2b9",
            ["address-card"]                = "\uf2bb",
            ["angry"]                       = "\uf556",
            ["arrow-alt-circle-down"]       = "\uf358",
            ["arrow-alt-circle-left"]       = "\uf359",
            ["arrow-alt-circle-right"]      = "\uf35a",
            ["arrow-alt-circle-up"]         = "\uf35b",
            ["bell"]                        = "\uf0f3",
            ["bell-slash"]                  = "\uf1f6",
            ["bookmark"]                    = "\uf02e",
            ["building"]                    = "\uf1ad",
            ["calendar"]                    = "\uf133",
            ["calendar-alt"]                = "\uf073",
            ["calendar-check"]              = "\uf274",
            ["calendar-minus"]              = "\uf272",
            ["calendar-plus"]               = "\uf271",
            ["calendar-times"]              = "\uf273",
            ["caret-square-down"]           = "\uf150",
            ["caret-square-left"]           = "\uf191",
            ["caret-square-right"]          = "\uf152",
            ["caret-square-up"]             = "\uf151",
            ["chart-bar"]                   = "\uf080",
            ["check-circle"]                = "\uf058",
            ["check-square"]                = "\uf14a",
            ["circle"]                      = "\uf111",
            ["clipboard"]                   = "\uf328",
            ["clock"]                       = "\uf017",
            ["clone"]                       = "\uf24d",
            ["closed-captioning"]           = "\uf20a",
            ["comment"]                     = "\uf075",
            ["comment-alt"]                 = "\uf27a",
            ["comment-dots"]                = "\uf4ad",
            ["comments"]                    = "\uf086",
            ["compass"]                     = "\uf14e",
            ["copy"]                        = "\uf0c5",
            ["copyright"]                   = "\uf1f9",
            ["credit-card"]                 = "\uf09d",
            ["dizzy"]                       = "\uf567",
            ["dot-circle"]                  = "\uf192",
            ["edit"]                        = "\uf044",
            ["envelope"]                    = "\uf0e0",
            ["envelope-open"]               = "\uf2b6",
            ["eye"]                         = "\uf06e",
            ["eye-slash"]                   = "\uf070",
            ["file"]                        = "\uf15b",
            ["file-alt"]                    = "\uf15c",
            ["file-archive"]                = "\uf1c6",
            ["file-audio"]                  = "\uf1c7",
            ["file-code"]                   = "\uf1c9",
            ["file-excel"]                  = "\uf1c3",
            ["file-image"]                  = "\uf1c5",
            ["file-pdf"]                    = "\uf1c1",
            ["file-powerpoint"]             = "\uf1c4",
            ["file-video"]                  = "\uf1c8",
            ["file-word"]                   = "\uf1c2",
            ["flag"]                        = "\uf024",
            ["flushed"]                     = "\uf579",
            ["folder"]                      = "\uf07b",
            ["folder-open"]                 = "\uf07c",
            ["frown"]                       = "\uf119",
            ["frown-open"]                  = "\uf57a",
            ["futbol"]                      = "\uf1e3",
            ["gem"]                         = "\uf3a5",
            ["grimace"]                     = "\uf57f",
            ["grin"]                        = "\uf580",
            ["grin-alt"]                    = "\uf581",
            ["grin-beam"]                   = "\uf582",
            ["grin-beam-sweat"]             = "\uf583",
            ["grin-hearts"]                 = "\uf584",
            ["grin-squint"]                 = "\uf585",
            ["grin-squint-tears"]           = "\uf586",
            ["grin-stars"]                  = "\uf587",
            ["grin-tears"]                  = "\uf588",
            ["grin-tongue"]                 = "\uf589",
            ["grin-tongue-squint"]          = "\uf58a",
            ["grin-tongue-wink"]            = "\uf58b",
            ["grin-wink"]                   = "\uf58c",
            ["hand-lizard"]                 = "\uf258",
            ["hand-paper"]                  = "\uf256",
            ["hand-peace"]                  = "\uf25b",
            ["hand-point-down"]             = "\uf0a7",
            ["hand-point-left"]             = "\uf0a5",
            ["hand-point-right"]            = "\uf0a4",
            ["hand-point-up"]               = "\uf0a6",
            ["hand-pointer"]                = "\uf25a",
            ["hand-rock"]                   = "\uf255",
            ["hand-scissors"]               = "\uf257",
            ["hand-spock"]                  = "\uf259",
            ["handshake"]                   = "\uf2b5",
            ["hdd"]                         = "\uf0a0",
            ["heart"]                       = "\uf004",
            ["hospital"]                    = "\uf0f8",
            ["hourglass"]                   = "\uf254",
            ["id-badge"]                    = "\uf2c1",
            ["id-card"]                     = "\uf2c2",
            ["image"]                       = "\uf03e",
            ["images"]                      = "\uf302",
            ["keyboard"]                    = "\uf11c",
            ["kiss"]                        = "\uf596",
            ["kiss-beam"]                   = "\uf597",
            ["kiss-wink-heart"]             = "\uf598",
            ["laugh"]                       = "\uf599",
            ["laugh-beam"]                  = "\uf59a",
            ["laugh-squint"]                = "\uf59b",
            ["laugh-wink"]                  = "\uf59c",
            ["lemon"]                       = "\uf094",
            ["life-ring"]                   = "\uf1cd",
            ["lightbulb"]                   = "\uf0eb",
            ["list-alt"]                    = "\uf022",
            ["map"]                         = "\uf279",
            ["meh"]                         = "\uf11a",
            ["meh-blank"]                   = "\uf5a4",
            ["meh-rolling-eyes"]            = "\uf5a5",
            ["minus-square"]                = "\uf146",
            ["money-bill-alt"]              = "\uf3d1",
            ["moon"]                        = "\uf186",
            ["newspaper"]                   = "\uf1ea",
            ["object-group"]                = "\uf247",
            ["object-ungroup"]              = "\uf248",
            ["paper-plane"]                 = "\uf1d8",
            ["pause-circle"]                = "\uf28b",
            ["play-circle"]                 = "\uf144",
            ["plus-square"]                 = "\uf0fe",
            ["question-circle"]             = "\uf059",
            ["registered"]                  = "\uf25d",
            ["sad-cry"]                     = "\uf5b3",
            ["sad-tear"]                    = "\uf5b4",
            ["save"]                        = "\uf0c7",
            ["share-square"]                = "\uf14d",
            ["smile"]                       = "\uf118",
            ["smile-beam"]                  = "\uf5b8",
            ["smile-wink"]                  = "\uf4da",
            ["snowflake"]                   = "\uf2dc",
            ["square"]                      = "\uf0c8",
            ["star"]                        = "\uf005",
            ["star-half"]                   = "\uf089",
            ["sticky-note"]                 = "\uf249",
            ["stop-circle"]                 = "\uf28d",
            ["sun"]                         = "\uf185",
            ["surprise"]                    = "\uf5c2",
            ["thumbs-down"]                 = "\uf165",
            ["thumbs-up"]                   = "\uf164",
            ["times-circle"]                = "\uf057",
            ["tired"]                       = "\uf5c8",
            ["trash-alt"]                   = "\uf2ed",
            ["user"]                        = "\uf007",
            ["user-circle"]                 = "\uf2bd",
            ["window-close"]                = "\uf410",
            ["window-maximize"]             = "\uf2d0",
            ["window-minimize"]             = "\uf2d1",
            ["window-restore"]              = "\uf2d2"
            // font atlas codes:
            // f2b9,f2bb,f556,f358,f359,f35a,f35b,f0f3,f1f6,f02e,f1ad,f133,f073,f274,f272,f271,f273,f150,f191,f152,f151,f080,f058,f14a,f111,f328,f017,f24d,f20a,f075,f27a,f4ad,f086,f14e,f0c5,f1f9,f09d,f567,f192,f044,f0e0,f2b6,f06e,f070,f15b,f15c,f1c6,f1c7,f1c9,f1c3,f1c5,f1c1,f1c4,f1c8,f1c2,f024,f579,f07b,f07c,f119,f57a,f1e3,f3a5,f57f,f580,f581,f582,f583,f584,f585,f586,f587,f588,f589,f58a,f58b,f58c,f258,f256,f25b,f0a7,f0a5,f0a4,f0a6,f25a,f255,f257,f259,f2b5,f0a0,f004,f0f8,f254,f2c1,f2c2,f03e,f302,f11c,f596,f597,f598,f599,f59a,f59b,f59c,f094,f1cd,f0eb,f022,f279,f11a,f5a4,f5a5,f146,f3d1,f186,f1ea,f247,f248,f1d8,f28b,f144,f0fe,f059,f25d,f5b3,f5b4,f0c7,f14d,f118,f5b8,f4da,f2dc,f0c8,f005,f089,f249,f28d,f185,f5c2,f165,f164,f057,f5c8,f2ed,f007,f2bd,f410,f2d0,f2d1,f2d2
        };

        static string[] iconNames = null;
        public static string[] IconNames
        {
            get
            {
                if (iconNames == null)
                {
                    iconNames = new string[Icons.Keys.Count];
                    int i = 0;
                    foreach (KeyValuePair<string, string> icon in Icons)
                    {
                        iconNames[i] = icon.Key;
                        i++;
                    }
                }
                return iconNames;
            }
        }
    }
}
