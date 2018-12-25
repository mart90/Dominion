using System;

namespace Dominion
{
    class CardEffect
    {
        public EffectPhase EffectPhase { get; set; }
        public Action<Player> Effect { get; set; }
    }
}
