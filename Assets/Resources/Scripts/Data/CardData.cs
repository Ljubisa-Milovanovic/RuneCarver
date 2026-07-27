using Assets.Resources.Scripts.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Resources.Scripts.Enums.Data
{
    public enum CardCategory
    {
        Attack,
        Skill,
        Power
    }

    public class CardData : ScriptableObject
    {
        [Header("Identity")]
        public string CardID;
        public string DisplayName;
        [TextArea] public string Description;
        public Sprite Artwork;

        [Header("Classification")]
        public CardCategory Category = CardCategory.Attack;
        public ElementType ElementType = ElementType.None;
        public bool IsHybrid;

        [Header("Base Stats")]
        public int ManaCost;
        public int BaseDamage;
        public int BaseBlock;
        [Min(0)] public int MaxRuneSlots = 1;

        [Header("On-Play Effects")]
        public List<StatusEffectApplication> AppliedStatusEffects = new List<StatusEffectApplication>();
        public TargetType Target = TargetType.SingleEnemy;

        public bool IsAttack => Category == CardCategory.Attack;
    }

    public enum TargetType
    {
        SingleEnemy,
        AllEnemies,
        Self,
        RandomEnemy
    }

    [Serializable]
    public struct StatusEffectApplication
    {
        public StatusEffectType Type;
        public int Magnitude;
        public int Duration;
    }
}