using System.Collections.Generic;
using UnityEngine;


namespace Assets.Resources.Scripts.Enums
{
	public enum ElementType
	{
		None = 0,
		Fire,
		Water,
		Earth,
		Air,
		Lightning,

        /*
		 Vatra + Voda
		 Vatra + Zemlja
		 Vatra + Vazduh
		 Vatra + Munja
		 */
		Steam,
		Lava,
		Flame, // Ovo je kao jaca vatra
		Explosion, //?????
        /*
		 Voda + Zemlja
		 Voda + Vazduh
		 Voda + Munja
		 */
		Mud,
        Fog,
		Thunderstorm,
		/*
		 Zemlja + vazduh
		 Zemlja + munja
		 */
		Sandstorm,
		Metal,
        Storm
    }

	public static class ElementMatrix
	{
		private const float COUNTER_MULTIPLIER = 1.5f;
		private static readonly Dictionary<ElementType, ElementType> CounterMap = new Dictionary<ElementType, ElementType>
		{
			{ElementType.Water, ElementType.Fire },
			{ElementType.Fire, ElementType.Earth },
			{ElementType.Earth, ElementType.Lightning },
			{ElementType.Lightning, ElementType.Water },
			{ElementType.Air, ElementType.Earth  },
		};
		
		public static bool IsCounter(ElementType attackerElement, ElementType defenderElement)
		{
			return CounterMap.TryGetValue(attackerElement, out var countered) && countered == defenderElement;
		}

		public static float GetDamageMultiplier(ElementType attackerElement, ElementType defenderElement)
		{
			return IsCounter(attackerElement, defenderElement) ? COUNTER_MULTIPLIER : 1;
		}

		public static bool IsHybrid(ElementType element)
		{
			switch (element)
			{
				case ElementType.Sandstorm:
				case ElementType.Steam:
				case ElementType.Explosion:
				case ElementType.Flame:
                case ElementType.Lava:
				case ElementType.Mud:
				case ElementType.Thunderstorm:
				case ElementType.Metal:
				case ElementType.Storm:
                case ElementType.Fog:
					return true;
				default:
					return false;
            }
        }
	}
}
