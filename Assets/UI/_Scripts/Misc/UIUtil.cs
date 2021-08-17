﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.UI._Scripts.Misc
{
    public static class UIUtil
    {
        static UIPointerHandler pointerHandler = null;
        static UISkin uiSkin = null;

        public static UIPointerHandler Pointers
        {
            get
            {
                if (pointerHandler == null)
                    pointerHandler = Skin.gameObject.AddComponent<UIPointerHandler>();

                return pointerHandler;
            }
        }

        public static UISkin Skin
        {
            get
            {
                if (uiSkin == null)
                    uiSkin = GameObject.FindObjectOfType<UISkin>();

                return uiSkin;
            }
        }

        public static Color ColorOpacity(Color source, float opacity = 1)
        {
            return new Color(source.r, source.g, source.b, opacity);
        }

        public static RectOffset Padding(float left, float right, float top, float bottom)
        {
            return new RectOffset(Mathf.RoundToInt(left), Mathf.RoundToInt(right), Mathf.RoundToInt(top), Mathf.RoundToInt(bottom));
        }
        public static RectOffset Padding(float all = 0)
        {
            return Padding(all, all, all, all);
        }
    }
}
