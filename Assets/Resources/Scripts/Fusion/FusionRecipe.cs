using Assets.Resources.Scripts.Data;
using Assets.Resources.Scripts.Enums;
using UnityEngine;

namespace Assets.Resources.Scripts.Fusion
{
    [CreateAssetMenu(fileName = "NewFusionRecipe", menuName = "RuneCarver/Fusion Recipe")]
    public class FusionRecipe : ScriptableObject
    {
        [Header("Match Rule")]
        public FusionMatchMode MatchMode = FusionMatchMode.ByElementPair;

        [Tooltip("Used when MatchMode = ByElementPair. Order doesn't matter — Fire+Earth matches Earth+Fire.")]
        public ElementType ElementA;
        public ElementType ElementB;

        [Tooltip("Used when MatchMode = ByExactCards.")]
        public CardData ExactCardA;
        public CardData ExactCardB;

        [Header("Result")]
        [Tooltip("Hand-authored CardData produced by this fusion. Should have IsHybrid = true.")]
        public CardData ResultCard;

        [Tooltip("Optional flavor name shown in the fusion confirmation UI, e.g. 'Lava'.")]
        public string RecipeDisplayName;

        public bool Matches(CardData a, CardData b)
        {
            if (a == null || b == null) return false;

            switch (MatchMode)
            {
                case FusionMatchMode.ByElementPair:
                    if (ElementMatrix.IsHybrid(ElementA) || ElementMatrix.IsHybrid(ElementB))
                        return false;
                    if (ElementMatrix.IsHybrid(a.ElementType) || ElementMatrix.IsHybrid(b.ElementType))
                        return false;

                    return MatchesElementPair(a.ElementType, b.ElementType);

                case FusionMatchMode.ByExactCards:
                    return (a == ExactCardA && b == ExactCardB) ||
                           (a == ExactCardB && b == ExactCardA);

                default:
                    return false;
            }
        }

        private bool MatchesElementPair(ElementType a, ElementType b)
        {
            return (a == ElementA && b == ElementB) ||
                   (a == ElementB && b == ElementA);
        }
    }

    public enum FusionMatchMode
    {
        ByElementPair,
        ByExactCards
    }
}
