using Assets.UI._Scripts.Misc;
using Assets.UI._Scripts.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameUI : UIBuilder
{
    public UIPanel[] GameScreens { get; private set; }

    protected override void BuildUI()
    {
        /*UIPanel testPanel = new UIPanel();
        testPanel.VOrientation = VerticalOrientation.Bottom;
        testPanel.Offsets.Left = testPanel.Offsets.Right = testPanel.Offsets.Bottom = 20;
        testPanel.Offsets.Height = 300;
        testPanel.MouseInteraction = true;
        testPanel.Build(transform);

        UIGroupPanel groupPanel = new UIGroupPanel();
        groupPanel.HOrientation = HorizontalOrientation.Left;
        groupPanel.Offsets.Left = groupPanel.Offsets.Top = 20;
        groupPanel.Offsets.Bottom = 340;
        groupPanel.Offsets.Width = 0.2f * UIWidth;
        groupPanel.Orientation = GroupOrientation.Vertical;
        groupPanel.Spacing = 20;
        groupPanel.MouseInteraction = true;
        groupPanel.Build(transform);

        for (int i = 0; i < 30; i++)
        {
            UIButton testButton = new UIButton(i, "Test button " + i.ToString(), "[S " + UIIcons.SolidIconNames[i] + "]");
            testButton.VOrientation = VerticalOrientation.Center;
            testButton.Offsets.Left = testButton.Offsets.Top = testButton.Offsets.Right = 30;
            testButton.Offsets.Height = 50;
            testButton.Build(groupPanel.Container);
            testButton.Clicked += ButtonClicked;
        }*/

        GameScreens = new UIPanel[5];

        GameScreens[0] = BuildWorldSelectionUI();
        GameScreens[1] = BuildStartingPointUI();
        GameScreens[2] = BuildIngameManagementUI();
        GameScreens[3] = BuildIngameBuildingUI();
        GameScreens[4] = BuildIngameMissionUI();
    }

    float GapSmall => UIWidth * 0.01f;
    float GapNormal => UIWidth * 0.025f;
    float GapLarge => UIWidth * 0.05f;

    UIPanel BuildPasePanel()
    {
        UIPanel panel = new UIPanel();
        panel.Offsets.Left = panel.Offsets.Top = panel.Offsets.Right = panel.Offsets.Bottom = 0;
        panel.Build(transform);
        panel.BackColor = new Color(0, 0, 0, 0);
        return panel;
    }

    UIPanel BuildWorldSelectionUI()
    {
        UIPanel panel = BuildPasePanel();
        panel.Container.gameObject.name = "WorldSelection";

        UIButton randomizePlanetButton = new UIButton(0, "Randomize planet", UIIcons.Icon("dice-five", UIIcons.IconStyle.Solid));
        randomizePlanetButton.HOrientation = HorizontalOrientation.Center;
        randomizePlanetButton.VOrientation = VerticalOrientation.Bottom;
        randomizePlanetButton.Offsets.Top = -GapNormal;
        randomizePlanetButton.Offsets.Width = 250;
        randomizePlanetButton.Offsets.Left = -randomizePlanetButton.Offsets.Width / 2f - GapSmall / 2f;
        randomizePlanetButton.Offsets.Height = 50;
        randomizePlanetButton.MouseInteraction = true;
        randomizePlanetButton.Build(panel.Container);

        UIButton nextStepButton = new UIButton(1, "Select start point", UIIcons.Icon("flag", UIIcons.IconStyle.Solid));
        nextStepButton.HOrientation = HorizontalOrientation.Center;
        nextStepButton.VOrientation = VerticalOrientation.Bottom;
        nextStepButton.Offsets.Top = -GapNormal;
        nextStepButton.Offsets.Width = 250;
        nextStepButton.Offsets.Left = nextStepButton.Offsets.Width / 2f + GapSmall / 2f;
        nextStepButton.Offsets.Height = 50;
        nextStepButton.MouseInteraction = true;
        nextStepButton.Build(panel.Container);

        return panel;
    }

    UIPanel BuildStartingPointUI()
    {
        UIPanel panel = BuildPasePanel();
        panel.Container.gameObject.name = "StartingPoint";
        return panel;
    }

    UIPanel BuildIngameManagementUI()
    {
        UIPanel panel = BuildPasePanel();
        panel.Container.gameObject.name = "Ingame Management";
        return panel;
    }

    UIPanel BuildIngameBuildingUI()
    {
        UIPanel panel = BuildPasePanel();
        panel.Container.gameObject.name = "Ingame Building";
        return panel;
    }

    UIPanel BuildIngameMissionUI()
    {
        UIPanel panel = BuildPasePanel();
        panel.Container.gameObject.name = "Ingame Mission";
        return panel;
    }
}
