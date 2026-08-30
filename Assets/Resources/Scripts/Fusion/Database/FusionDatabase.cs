using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Resources.Scripts.Data;
using Assets.Resources.Scripts.Enums;
using Assets.Resources.Scripts.Fusion;
using UnityEngine;

namespace Resources.Scripts.Fusion.Database
{
    [CreateAssetMenu(fileName = "FusionDatabase", menuName = "RuneCarver/FusionDatabase")]
    public class FusionDatabase : ScriptableObject
    {
        public List<FusionRecipe> Recipes = new();
    
        /*
         * This is the data structure we are going to use for getting new 'carved' cards
         * from the database. 
        */
        private readonly Dictionary<(CardData first, CardData second), FusionRecipe> _byCardPair = new();

        private readonly Dictionary<string, FusionRecipe> _byCarving = new();

        private bool _indexBuilt;

        private void OnEnable() => RebuildIndex();

        private void OnValidate() => _indexBuilt = false;

        private void RebuildIndex()
        {
            _byCardPair.Clear();
            _byCarving.Clear();

            foreach (var recipe in Recipes.Where(recipe => recipe != null && recipe.IsValid))
            {
                switch (recipe.MatchMode)
                {
                    case FusionMatchMode.ByExactCards:
                        var pairKey = (recipe.FirstCard, recipe.SecondCard);
                        if (!_byCardPair.ContainsKey(pairKey)) _byCardPair.Add(pairKey, recipe);
                        break;

                    case FusionMatchMode.ByCarvingSequence:
                        var carvingKey = recipe.CarvingKey;
                        _byCarving.TryAdd(carvingKey, recipe);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            _indexBuilt = true;
        }

        private void EnsureIndex()
        {
            if (!_indexBuilt) RebuildIndex();
        }
        
        private FusionRecipe FindByExactCards(CardData first, CardData second)
        {
            if (first == null || second == null) return null;

            EnsureIndex();
            return _byCardPair.GetValueOrDefault((first, second));
        }
        
        private FusionRecipe FindByCarving(IReadOnlyList<GlyphType> combinedCarving)
        {
            if (!GlyphSequence.IsCarved(combinedCarving)) return null;

            EnsureIndex();
            return _byCarving.GetValueOrDefault(GlyphSequence.ToKey(combinedCarving));
        }
        
        public FusionRecipe FindRecipe(CardData first, CardData second, IReadOnlyList<GlyphType> combinedCarving)
        {
            var exact = FindByExactCards(first, second);
            return exact != null ? exact : FindByCarving(combinedCarving);
        }

        public bool CanFuse(CardData first, CardData second, IReadOnlyList<GlyphType> combinedCarving)
            => FindRecipe(first, second, combinedCarving) != null;

        public List<string> ValidateNoDuplicates()
        {
            var issues = new List<string>();
            var seenCardPairs = new HashSet<(CardData, CardData)>();
            var seenCarvings = new HashSet<string>();

            foreach (var recipe in Recipes.Where(recipe => recipe != null))
            {
                if (!recipe.IsValid)
                {
                    issues.Add($"Recipe '{recipe.name}' is incomplete for mode {recipe.MatchMode}.");
                    continue;
                }

                switch (recipe.MatchMode)
                {
                    case FusionMatchMode.ByExactCards:
                        if (!seenCardPairs.Add((recipe.FirstCard, recipe.SecondCard)))
                        {
                            issues.Add($"Duplicate ordered card-pair recipe for " +
                                       $"{recipe.FirstCard.name} -> {recipe.SecondCard.name} (recipe: {recipe.name})");
                        }
                        break;

                    case FusionMatchMode.ByCarvingSequence:
                        if (!seenCarvings.Add(recipe.CarvingKey))
                        {
                            issues.Add($"Duplicate carving-sequence recipe for [{recipe.CarvingKey}] (recipe: {recipe.name})");
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return issues;
        }
    }
}
