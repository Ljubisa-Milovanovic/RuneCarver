using Resources.Scripts.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace Resources.Scripts.Enums.Data
{
    public enum PassiveEffectType
    {
        None,
        OnPlayBonusDamage,
        OnPlayLifesteal,
        ReduceCostOverTime,
        ApplyStatusOnHit,
        ThornsOnBlock,
    }

    public class RuneData : ScriptableObject
    {
        [Header("Identity")]
        public string RuneID;
        public string DisplayName;
        [TextArea] public string Description;
        public Sprite Artwork;

        [Header("Compatibility")]
        public List<ElementType> CompatibleElements = new List<ElementType>();

        [Header("Stat Buffs (additive to host card, applied on card)")]
        public StatBuffs StatBuffs;

        [Header("Passive Effect (evaluated during combat resolution)")]
        public PassiveEffectType PassiveEffect = PassiveEffectType.None;
        public int PassiveMagnitude;

        public bool IsCompatibleWith(ElementType hostElement)
        {
            return CompatibleElements.Count == 0 || CompatibleElements.Contains(hostElement);
        }
    }

    [System.Serializable]
    public struct StatBuffs
    {
        public int BonusDamage;
        public int BonusBlock;
        public int ManaCostDelta;
        public float DamageMultiplier; // 0 = nema; 0.2 = +20%
    }
}