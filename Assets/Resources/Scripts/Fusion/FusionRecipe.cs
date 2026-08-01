using Assets.Resources.Scripts.Data;
using Assets.Resources.Scripts.Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Resources.Scripts.Fusion
{
    [CreateAssetMenu(fileName = "NewFusionRecipe", menuName = "RuneCarver/Fusion Recipe")]
    public class FusionRecipe : ScriptableObject
    {
        [Header("Match Rule")]
        public FusionMatchMode MatchMode = FusionMatchMode.ByCarvingSequence;
        public List<GlyphType> RequiredSequence = new List<GlyphType>();
        [FormerlySerializedAs("ExactCardA")] public CardData FirstCard;
        [FormerlySerializedAs("ExactCardB")] public CardData SecondCard;

        [Header("Result")]
        public CardData ResultCard;

        public bool CollapseCarving;
        public string RecipeDisplayName;

        /// Kljuc pod kojim FusionDatabase indeksira ovaj recept u sequence mapi.
        public string CarvingKey => GlyphSequence.ToKey(RequiredSequence);

        public bool IsValid
        {
            get
            {
                if (ResultCard == null) return false;

                switch (MatchMode)
                {
                    case FusionMatchMode.ByCarvingSequence:
                        return GlyphSequence.IsCarved(RequiredSequence);

                    case FusionMatchMode.ByExactCards:
                        return FirstCard != null && SecondCard != null;

                    default:
                        return false;
                }
            }
        }
        public bool MatchesCards(CardData first, CardData second)
        {
            if (MatchMode != FusionMatchMode.ByExactCards) return false;
            if (first == null || second == null) return false;

            return first == FirstCard && second == SecondCard;
        }
        
        public bool MatchesCarving(IReadOnlyList<GlyphType> combinedCarving)
        {
            if (MatchMode != FusionMatchMode.ByCarvingSequence) return false;
            if (combinedCarving == null || RequiredSequence == null) return false;
            if (combinedCarving.Count != RequiredSequence.Count) return false;

            return !RequiredSequence.Where((t, i) => combinedCarving[i] != t).Any();
        }
    }

    public enum FusionMatchMode
    {
        ByCarvingSequence,
        ByExactCards
    }
}
