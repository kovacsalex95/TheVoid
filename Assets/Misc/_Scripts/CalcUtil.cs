using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Misc._Scripts
{
    class CalcUtil
    {
        public static float SmoothDamp(float current, float target, float time = 0.1f, float thresholdRatio = 0.01f)
        {
            if (Mathf.Abs(target - current) <= target * thresholdRatio)
                return target;

            float vel = 0;
            return Mathf.SmoothDamp(current, target, ref vel, time);
        }
        public static Vector3 SmoothDamp(Vector3 current, Vector3 target, float time = 0.1f, float absoluteThreshold = 0.01f)
        {
            if ((current - target).magnitude <= absoluteThreshold)
                return target;

            Vector3 vel = Vector3.zero;
            return Vector3.SmoothDamp(current, target, ref vel, time);
        }

        public static float Normalize(float value)
        {
            return value == 0f ? 0f : (value > 0f ? 1f : -1f);
        }
    }
}
