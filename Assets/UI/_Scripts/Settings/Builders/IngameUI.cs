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
        testPanel.VOrientation = VerticalOrientation.Bottom;
        testPanel.Offsets.Left = testPanel.Offsets.Right = testPanel.Offsets.Bottom = 20;
        testPanel.Offsets.Height = 300;
        testPanel.Build(transform);

        UIGroupPanel groupPanel = new UIGroupPanel();
        groupPanel.HOrientation = HorizontalOrientation.Left;
        groupPanel.Offsets.Left = groupPanel.Offsets.Top = 20;
        groupPanel.Offsets.Bottom = 340;
        groupPanel.Offsets.Width = 0.2f * UIWidth;
        groupPanel.GroupOrientation = GroupOrientation.Vertical;
        groupPanel.GroupSpacing = 20;
        groupPanel.Build(transform);

        for (int i = 0; i < 30; i++)
        {
            UIButton testButton = new UIButton(i, "Test button " + i.ToString(), Skin.GetIcon("TestIcon"));
            testButton.VOrientation = VerticalOrientation.Center;
            testButton.Offsets.Left = testButton.Offsets.Top = testButton.Offsets.Right = 30;
            testButton.Offsets.Height = 50;
            testButton.Build(groupPanel.Container);
            testButton.TextAlign = HorizontalOrientation.Left;
            testButton.Clicked += ButtonClicked;
        }
    }

    void ButtonClicked(object sender, UIButtonClickedEventArgs e)
    {
        Debug.Log("Test button " + e.ID.ToString() + " clicked");
    }
}
