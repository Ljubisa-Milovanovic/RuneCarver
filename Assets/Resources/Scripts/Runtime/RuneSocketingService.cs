using Assets.Resources.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Resources.Scripts.Runtime
{
    public enum SocketResult
    {
        Success,
        Failed_NoFreeSlots,
        Failed_IncompatibleElement,
        Failed_NullInput
    }

    public enum UnsocketResult
    {
        Success,
        Failed_RuneNotSocketed,
        Failed_NullInput
    }
    public static class RuneSocketingService
    {
        public static SocketResult TrySocket(CardInstance host, RuneData rune)
        {
            if (host == null || rune == null) {
                return SocketResult.Failed_NullInput;
            }
            if (!host.HasFreeRuneSlot) return SocketResult.Failed_NoFreeSlots;
            if (!rune.IsCompatibleWith(host.Element)) return SocketResult.Failed_IncompatibleElement;

            host.SocketedRunes.Add(rune);
            return SocketResult.Success;
        }

        // Evo i ova mehanika ovde ako hocete da moze da skida rune sa kartica, ali meni se to ne svidja...
        public static UnsocketResult TryUnsocket(CardInstance host, RuneData rune)
        {
            if (host == null || rune == null)
                return UnsocketResult.Failed_NullInput;

            bool removed = host.SocketedRunes.Remove(rune);
            return removed ? UnsocketResult.Success : UnsocketResult.Failed_RuneNotSocketed;
        }

        // Ovo mozda buni sad, ali pogledajte Fusion Service. Ovo sluzi da kad se spoje dve karte
        /*
        Kad se spoje dve karte koje su imale rune na sebi, da se prenesu i rune na njima koje su kompatibilne sa tim karticama
        Npr ako spajamo Vatru i Kamen dobijemo lavu, a na vatru smo stavili neku runu za jacanje a na kamen za odbranu
        Dobijamo lavu sa tim istim runama koje su bile na kartama.
         */
        public static List<RuneData> TransferCompatibleRunes(
            CardInstance source, CardInstance destination)
        {
            var leftover = new List<RuneData>();
            if (source == null || destination == null)
                return leftover;

            
            var runesToMove = new List<RuneData>(source.SocketedRunes);

            foreach (var rune in runesToMove)
            {
                var result = TrySocket(destination, rune);
                if (result == SocketResult.Success)
                {
                    source.SocketedRunes.Remove(rune);
                }
                else
                {
                    leftover.Add(rune);
                }
            }

            return leftover;
        }
    }
}
