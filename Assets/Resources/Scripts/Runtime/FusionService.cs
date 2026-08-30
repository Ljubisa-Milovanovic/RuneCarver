using Assets.Resources.Scripts.Data;
using Assets.Resources.Scripts.Enums;
using System.Collections.Generic;
using Resources.Scripts.Fusion.Database;

namespace Assets.Resources.Scripts.Runtime
{
    public enum FusionResultCode
    {
        Success,
        Failed_NoRecipeFound,
        Failed_NullInput,
        Failed_SameInstance
    }
    public readonly struct FusionResult
    {
        private readonly FusionResultCode _code;
        public readonly CardInstance ResultInstance;
        public readonly List<RuneData> DisplacedRunes;

        public FusionResult(FusionResultCode code, CardInstance resultInstance, List<RuneData> dispalcedRunes)
        {
            _code = code;
            ResultInstance = resultInstance;
            DisplacedRunes = dispalcedRunes;
        }

        public bool Success => _code == FusionResultCode.Success;

        public static FusionResult Failed(FusionResultCode code) => new FusionResult(code, null, null);
        
    }
    public class FusionService
    {
        private readonly FusionDatabase _db;

        public FusionService(FusionDatabase db)
        {
            _db = db;
        }
        
        public bool CanFuse(CardInstance first, CardInstance second)
        {
            if (first == null || second == null || ReferenceEquals(first, second))
                return false;

            return _db != null && _db.CanFuse(
                first.BaseCardData,
                second.BaseCardData,
                GlyphSequence.Combine(first.Carving, second.Carving));
        }

        public FusionResult Fuse(CardInstance first, CardInstance second)
        {
            if (first == null || second == null)
                return FusionResult.Failed(FusionResultCode.Failed_NullInput);

            if (ReferenceEquals(first, second))
                return FusionResult.Failed(FusionResultCode.Failed_SameInstance);

            var combinedCarving = GlyphSequence.Combine(first.Carving, second.Carving);
            var recipe = _db?.FindRecipe(first.BaseCardData, second.BaseCardData, combinedCarving);

            if (recipe == null || recipe.ResultCard == null)
                return FusionResult.Failed(FusionResultCode.Failed_NoRecipeFound);

            IReadOnlyList<GlyphType> resultCarving = recipe.CollapseCarving
                ? recipe.ResultCard.Carving
                : combinedCarving;

            var result = new CardInstance(recipe.ResultCard, resultCarving);

            var displaced = new List<RuneData>();

            displaced.AddRange(RuneSocketingService.TransferCompatibleRunes(first, result));
            displaced.AddRange(RuneSocketingService.TransferCompatibleRunes(second, result));

            return new FusionResult(FusionResultCode.Success, result, displaced);
        }
    }
}
