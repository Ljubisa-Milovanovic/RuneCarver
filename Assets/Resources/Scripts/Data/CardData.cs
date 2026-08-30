using Assets.Resources.Scripts.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Resources.Scripts.Data
{
    public enum CardCategory
    {
        Attack,
        Skill,
        Power
    }
    [CreateAssetMenu(fileName = "NewCard", menuName = "RuneCarver/Main Card")]
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

        public List<GlyphType> Carving = new List<GlyphType>();

        [Header("Base Stats")]
        public int ManaCost;
        public int BaseDamage;
        public int BaseBlock;
        [Min(0)] public int MaxRuneSlots = 1;

        [Header("On-Play Effects")]
        public List<StatusEffectApplication> AppliedStatusEffects = new List<StatusEffectApplication>();
        public TargetType Target = TargetType.SingleEnemy;

        public bool IsAttack => Category == CardCategory.Attack;

        public bool IsHybrid => Carving is { Count: > 1 };
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