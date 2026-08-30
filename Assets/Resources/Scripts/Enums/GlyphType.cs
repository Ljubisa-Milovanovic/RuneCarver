using System.Collections.Generic;

namespace Assets.Resources.Scripts.Enums
{
    /*
     Ovo su urezi na kartici. Bazna kartica ima jedan glif, a fuzionisana nosi
     spojen niz svojih operanada - prvi operand pa drugi. Zato je redosled bitan:
     Line + Circle i Circle + Line su dva razlicita ureza, pa i dva razlicita recepta.
     */
    public enum GlyphType
    {
        None = 0,
        Line,
        Circle,
        Triangle,
        Square,
        Arc,
        Dot,
        Cross
    }

    public static class GlyphSequence
    {
        // Ovde moze bilo kako da se postavi ali se postize ista stvar sad je npr:
        // line-circle-line, line-circle-triangle, line-circle-square, line-circle-arc, line-circle-dot, line-circle-cross
        private const string Separator = "-";

        public static string ToKey(IReadOnlyList<GlyphType> glyphs)
        {
            if (glyphs == null || glyphs.Count == 0) return string.Empty;

            return string.Join(Separator, glyphs);
        }

        public static List<GlyphType> Combine(IReadOnlyList<GlyphType> first, IReadOnlyList<GlyphType> second)
        {
            var combined = new List<GlyphType>((first?.Count ?? 0) + (second?.Count ?? 0));

            if (first != null) combined.AddRange(first);
            if (second != null) combined.AddRange(second);

            return combined;
        }

        public static bool IsCarved(IReadOnlyList<GlyphType> glyphs) => glyphs is { Count: > 0 };
    }
}