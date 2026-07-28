using NUnit.Framework;
using Resources.Scripts.Data;
using Resources.Scripts.Enums;
using Resources.Scripts.Fusion;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FusionDatabase", menuName = "RuneCarver/FusionDatabase")]
public class FusionDatabase : ScriptableObject
{
    public List<FusionRecipe> Recipes = new List<FusionRecipe>();

    public FusionRecipe FindRecipe(CardData a, CardData b)
    {
        FusionRecipe exactMatch = null;
        FusionRecipe elementMatch = null;

        foreach(var recipe in Recipes)
        {
            if (recipe == null || !recipe.Matches(a, b)) continue;
            if (recipe.MatchMode == FusionMatchMode.ByExactCards)
            {
                exactMatch = recipe;
                break;
            }
            else if (elementMatch == null)
            {
                elementMatch = recipe;
            }
        }

        return exactMatch != null ? exactMatch : elementMatch;
    }

    public bool CanFuse(CardData a, CardData b) => FindRecipe(a, b) != null;

    public List<string> ValidateNoDuplicates()
    {
        var issues = new List<string>();
        var seenElementPairs = new HashSet<(ElementType, ElementType)>();

        foreach (var recipe in Recipes)
        {
            if (recipe == null) continue;
            if (recipe.MatchMode != FusionMatchMode.ByElementPair) continue;

            var a = recipe.ElementA; var b = recipe.ElementB;
            var key = a.CompareTo(b) <= 0 ? (a,b) : (b,a);

            if(!seenElementPairs.Add(key))
            {
                issues.Add($"Duplicate element-pair recipe for {a} + {b} (recipe: {recipe.name})");
            }
        }
        return issues;
    }
}
