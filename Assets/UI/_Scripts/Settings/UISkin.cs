using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkin : MonoBehaviour
{
    
}

public static class UISkinHelper
{
    static UISkin uiSkin = null;

    public static UISkin Skin
    {
        get {
            if (uiSkin == null)
                uiSkin = GameObject.FindObjectOfType<UISkin>();

            return uiSkin;
        }
    }
}