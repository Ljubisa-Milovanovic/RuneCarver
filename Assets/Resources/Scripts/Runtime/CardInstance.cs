using Assets.Resources.Scripts.Data;
using Assets.Resources.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Resources.Scripts.Runtime
{
    /*
     Ovde je bitno da napomenem, da sam ubacio za mehanizam da kartica moze da se menja
    samo tokom runtime-a, odnosno dokle god traje "borba", posle borbe sav progres se gubi.
    Odnosno ulazi se u borbu sa hp npr kolko je ostalo, mana se refreshuje, a hand koji se napravio
    tokom spajanja kartica ostaje samo unutar sesije - znaci ruka/spil ostaje samo unutar sesije, ostatak
    moze da se menja napolju tipa na nekom kampu ili nzm ni ja kod nekog forgera da napravi nove kartice
     */
    public class CardInstance
    {
        public readonly string InstanceID;
        public CardData BaseCardData { get; private set; }

        public List<RuneData> SocketedRunes { get; } = new List<RuneData>();

        public List<StatusEffect> ActiveStatuses { get; } = new List<StatusEffect>();

        public IReadOnlyList<GlyphType> Carving { get; }

        public CardInstance(CardData card) : this(card, null) { }

        public CardInstance(CardData card, IReadOnlyList<GlyphType> carvingOverride)
        {
            BaseCardData = card ?? throw new ArgumentNullException(nameof(card));
            InstanceID = Guid.NewGuid().ToString();

            var source = carvingOverride ?? (IReadOnlyList<GlyphType>)card.Carving;
            Carving = source != null ? new List<GlyphType>(source) : new List<GlyphType>();
        }

        private int MaxRuneSlots => BaseCardData.MaxRuneSlots;
        private int UsedRuneSlots => SocketedRunes.Count;
        public bool HasFreeRuneSlot => UsedRuneSlots < MaxRuneSlots;

        private int EffectiveManaCost
        {
            get
            {
                var cost = BaseCardData.ManaCost + SocketedRunes.Sum(rune => rune.StatBuffs.ManaCostDelta);
                return Math.Max(0, cost);
            }
        }
        private int EffectiveDamage
        {
            get
            {
                float damage = BaseCardData.BaseDamage;
                var multiplier = 1f;

                foreach(var rune in SocketedRunes)
                {
                    damage += rune.StatBuffs.BonusDamage;
                    multiplier += rune.StatBuffs.DamageMultiplier;
                }
                return Math.Max(0, (int)Math.Round(damage * multiplier));
            }
        }

        private int EffectiveBlock
        {
            get
            {
                var block = BaseCardData.BaseBlock + SocketedRunes.Sum(rune => rune.StatBuffs.BonusBlock);
                return Math.Max(0, block);
            }
        }
        public ElementType Element => BaseCardData.ElementType;

        public IEnumerable<(PassiveEffectType effect, int magnitude)> GetPassiveEffects()
        {
            return SocketedRunes
                .Where(r => r.PassiveEffect != PassiveEffectType.None)
                .Select(r => (r.PassiveEffect, r.PassiveMagnitude));
        }

        public override string ToString()
        {
            var runeSuffix = SocketedRunes.Count > 0
                ? $" [{string.Join(", ", SocketedRunes.Select(r => r.DisplayName))}]"
                : string.Empty;
            return $"{BaseCardData.DisplayName}{runeSuffix} (Dmg:{EffectiveDamage} Blk:{EffectiveBlock} Cost:{EffectiveManaCost})";
        }
    }
}
