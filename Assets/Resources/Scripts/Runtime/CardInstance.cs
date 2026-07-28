using Assets.Resources.Scripts.Data;
using Assets.Resources.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor.Overlays;

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

        public CardInstance(CardData card)
        {
            BaseCardData = card ?? throw new ArgumentNullException(nameof(card));
            InstanceID = Guid.NewGuid().ToString();
        }

        public int MaxRuneSlots => BaseCardData.MaxRuneSlots;
        public int UsedRuneSlots => SocketedRunes.Count;
        public bool HasFreeRuneSlot => UsedRuneSlots < MaxRuneSlots;

        public int EffectiveManaCost
        {
            get
            {
                int cost = BaseCardData.ManaCost;
                foreach (var rune in SocketedRunes)
                {
                    cost += rune.StatBuffs.ManaCostDelta;
                }
                return Math.Max(0, cost);
            }
        }
        //ovo je u int zbog znakova, ako hocete moze i float da bude
        public int EffectiveDamage
        {
            get
            {
                float damage = BaseCardData.BaseDamage;
                float multiplier = 1f;

                foreach(var rune in SocketedRunes)
                {
                    damage += rune.StatBuffs.BonusDamage;
                    multiplier += rune.StatBuffs.DamageMultiplier;
                }
                return Math.Max(0, (int)Math.Round(damage * multiplier));
            }
        }

        public int EffectiveBlock
        {
            get
            {
                int block = BaseCardData.BaseBlock;
                foreach (var rune in SocketedRunes)
                    block += rune.StatBuffs.BonusBlock;
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
            string runeSuffix = SocketedRunes.Count > 0
                ? $" [{string.Join(", ", SocketedRunes.Select(r => r.DisplayName))}]"
                : string.Empty;
            return $"{BaseCardData.DisplayName}{runeSuffix} (Dmg:{EffectiveDamage} Blk:{EffectiveBlock} Cost:{EffectiveManaCost})";
        }
    }
}
