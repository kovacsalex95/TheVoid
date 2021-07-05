using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRotation : MonoBehaviour
{
    [Range(0f, 360f)]
    public float startAngle = 315;
    float angle;

    public float dayDuration = 1f; // minutes

    public float updateInterval = 1f;
    float currentFrameTime = 0;

    private void Awake()
    {
        angle = startAngle;
        UpdateAngle(false);
    }

    void Update()
    {
        currentFrameTime += Time.deltaTime;

        if (currentFrameTime >= updateInterval)
        {
            currentFrameTime = 0f;
            UpdateAngle();
        }
    }

    void UpdateAngle(bool advance = true)
    {
        while (angle >= 360f)
            angle -= 360f;

        if (advance && dayDuration > 0)
        {
            float dayDurationSeconds = dayDuration * 60f;
            angle += 360f * (updateInterval / dayDurationSeconds);
        }

        Vector3 eulerAngles = this.transform.localEulerAngles;
        eulerAngles.y = angle;
        this.transform.localEulerAngles = eulerAngles;
    }
}
