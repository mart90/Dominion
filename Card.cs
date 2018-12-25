using System;
using System.Collections.Generic;

namespace Dominion
{
    class Card
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public Expansion Expansion { get; set; }
        public List<CardType> Types { get; set; }
        public int CoinCost { get; set; }
        public bool IsSupply { get; set; }
        public bool IsKingdom { get; set; }
        public List<CardEffect> Effects { get; set; }

        public Card Copy()
        {
            return (Card)MemberwiseClone();
        }
    }
}
