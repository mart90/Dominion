using System.Collections.Generic;

namespace Dominion
{
    class CardPile
    {
        public CardPile(string cardName, int amount)
        {
            Card = CardLibrary.GetCard(cardName);
            Amount = amount;
        }

        public Card Card { get; set; }
        public int Amount { get; set; }

        public Card GetSingleCard()
        {
            if (Amount == 0)
            {
                return null;
            }
            Amount--;
            return Card.Copy();
        }

        public List<Card> GetMultipleCards(int count)
        {
            List<Card> cards = new List<Card>();
            for (int i = 0; i < count; i++)
            {
                cards.Add(Card.Copy());
            }
            Amount -= count;
            return cards;
        }
    }
}
