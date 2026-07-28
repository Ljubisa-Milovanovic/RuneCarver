using Assets.Resources.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Text;

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
        public readonly FusionResultCode Code;
        public readonly CardInstance ResultInstance;
        public readonly List<RuneData> DisplacedRunes;

        public FusionResult(FusionResultCode code, CardInstance resultInstance, List<RuneData> dispalcedRunes)
        {
            Code = code;
            ResultInstance = resultInstance;
            DisplacedRunes = dispalcedRunes;
        }

        public bool Success => Code == FusionResultCode.Success;

        public static FusionResult Failed(FusionResultCode code) => new FusionResult(code, null, null);
        
    }
    public class FusionService
    {
        private readonly FusionDatabase _db;

        public FusionService(FusionDatabase db)
        {
            _db = db;
        }

        public bool CanFuse(CardInstance a, CardInstance b)
        {
            if (a == null || b == null)
                return false;

            return _db != null && _db.CanFuse(a.BaseCardData, b.BaseCardData);
        }

        public FusionResult Fuse(CardInstance a, CardInstance b)
        {
            if (a == null || b == null)
                return FusionResult.Failed(FusionResultCode.Failed_NullInput);

            if (ReferenceEquals(a, b))
                return FusionResult.Failed(FusionResultCode.Failed_SameInstance);

            var recipe = _db?.FindRecipe(a.BaseCardData, b.BaseCardData);

            if (recipe == null || recipe.ResultCard == null)
                return FusionResult.Failed(FusionResultCode.Failed_NoRecipeFound);

            var result = new CardInstance(recipe.ResultCard);

            var displaced = new List<RuneData>();

            displaced.AddRange(RuneSocketingService.TransferCompatibleRunes(a, result));
            displaced.AddRange(RuneSocketingService.TransferCompatibleRunes(b, result));

            return new FusionResult(FusionResultCode.Success, result, displaced); 
        }
    }
}
