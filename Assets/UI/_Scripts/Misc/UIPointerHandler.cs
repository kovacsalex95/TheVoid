using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIPointerHandler : MonoBehaviour
{
    public bool MouseOverUI { get; private set; }

    Dictionary<int, UIElement> uiElements = null;
    int newElementIndex = 0;

    PlayerInput input;

    void Awake()
    {
        MouseOverUI = false;
        input = Util.gameController().input;
    }

    void LateUpdate()
    {
        Vector2 mousePosition = input.actions["Mouse Position"].ReadValue<Vector2>();

        MouseOverUI = false;

        foreach (KeyValuePair<int, UIElement> element in uiElements)
        {
            if (!element.Value.MouseInteraction)
                continue;

            Vector2 localMousePosition = element.Value.Transform.InverseTransformPoint(mousePosition);
            if (element.Value.Transform.rect.Contains(localMousePosition))
            {
                MouseOverUI = true;
                break;
            }
        }
    }

    public int RegisterElement(UIElement element)
    {
        if (uiElements == null)
            uiElements = new Dictionary<int, UIElement>();

        uiElements.Add(newElementIndex, element);
        
        int result = newElementIndex; newElementIndex++; return result;
    }

    public void DeleteElement(int elementIndex)
    {
        if (uiElements == null) {
            uiElements = new Dictionary<int, UIElement>();
            return;
        }

        if (!uiElements.ContainsKey(elementIndex))
            return;

        uiElements.Remove(elementIndex);
    }
}
