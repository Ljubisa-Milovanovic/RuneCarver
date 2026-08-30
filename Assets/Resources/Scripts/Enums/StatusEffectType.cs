using System;
using System.Collections;
using UnityEngine;

namespace Assets.Resources.Scripts.Enums
{
    public enum StatusEffectType
    {
        None = 0,
        Burn,
        Freeze,
        Chain,
        Weaken,
        Vulnerable,
        Shielded,
    }

    [Serializable]
    public struct StatusEffect
    {
        public StatusEffectType Type;
        public int Magnitude;
        public int Duration;
        public StatusEffect(StatusEffectType type, int magnitude, int duration)
        {
            Type = type;
            Magnitude = magnitude;
            Duration = duration;
        }

        public bool IsExpired => Duration == 0;
    }

    public static class StatusStackingRules
    {
        public enum StackMode
        {
            RefreshDuration,
            AddMagnitude,
            AddDuration,
            Ignore
        };

        public static StackMode GetStackMode(StatusEffectType type)
        {
            switch (type)
            {
                case StatusEffectType.Burn:
                    return StackMode.AddMagnitude;
                case StatusEffectType.Freeze:
                    return StackMode.RefreshDuration;
                case StatusEffectType.Weaken:
                case StatusEffectType.Vulnerable:
                    return StackMode.AddDuration;
                case StatusEffectType.Chain:
                    return StackMode.Ignore;
                case StatusEffectType.Shielded:
                    return StackMode.AddMagnitude;
                default:
                    return StackMode.RefreshDuration;
            }
        }
    }
}