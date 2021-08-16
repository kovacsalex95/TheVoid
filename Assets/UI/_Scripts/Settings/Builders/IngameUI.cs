using Assets.UI._Scripts.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameUI : UIBuilder
{
    UISkin Skin = null;

    protected override void BuildUI()
    {
        Skin = UIUtil.Skin;

        UIPanel testPanel = new UIPanel();
        testPanel.HOrientation = HorizontalOrientation.Left;
        testPanel.Offsets.Left = testPanel.Offsets.Top = testPanel.Offsets.Bottom = 20;
        testPanel.Offsets.Width = 0.2f * UIWidth;
        testPanel.Build(transform);

        UIButton testButton = new UIButton("Test button");
        testButton.VOrientation = VerticalOrientation.Top;
        testButton.Offsets.Left = testButton.Offsets.Top = testButton.Offsets.Right = 20;
        testButton.Offsets.Height = 60;
        testButton.Build(testPanel.Transform.transform);
        testButton.TextAlign = HorizontalOrientation.Left;
        testButton.Clicked += TestButtonClicked1;

        UIButton testButton2 = new UIButton("Test button 2", Skin.GetIcon("TestIcon"));
        testButton2.VOrientation = VerticalOrientation.Top;
        testButton2.Offsets.Left = testButton2.Offsets.Right = 20;
        testButton2.Offsets.Top = 20 * 2 + 60;
        testButton2.Offsets.Height = 60;
        testButton2.Build(testPanel.Transform.transform);
        testButton2.TextAlign = HorizontalOrientation.Left;
        testButton2.Clicked += TestButtonClicked2;
    }

    private void TestButtonClicked1(object sender, EventArgs e)
    {
        Debug.Log("Test button clicked");
    }

    private void TestButtonClicked2(object sender, EventArgs e)
    {
        Debug.Log("Test button 2 clicked");
    }
}
