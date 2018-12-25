using System;
using System.Collections.Generic;

namespace Dominion
{
    class Game
    {
        public Game(List<string> kingdom, List<string> playerNames)
        {
            int victoryCardCount = playerNames.Count == 2 ? 8 : 12;
            int curseCount = playerNames.Count == 2 ? 10 : playerNames.Count == 3 ? 20 : 30;

            AddBaseCards(victoryCardCount, curseCount);
            CardPiles.Find(e => e.Card.Name == "estate").Amount += playerNames.Count * 3;

            AddKingdom(kingdom);
            AddPlayers(playerNames);
        }

        public List<CardPile> CardPiles { get; set; } = new List<CardPile>();
        public List<Player> Players { get; set; } = new List<Player>();
        public List<Card> Trash { get; set; } = new List<Card>();

        public bool IsEnded()
        {
            return DepletedSupplyPiles().Count >= 3 || CardPiles.Find(e => e.Card.Name == "province").Amount == 0;
        }

        public void Play()
        {
            foreach (var player in Players)
            {
                player.DrawCards(5);
            }

            var activePlayer = 0;
            while (!IsEnded())
            {
                Players[activePlayer].DoTurn();
                activePlayer++;
                if (activePlayer == Players.Count)
                {
                    activePlayer = 0;
                }
            }
        }

        public void AddCardPile(string cardName, int amount)
        {
            CardPiles.Add(new CardPile(cardName, amount));
        }

        public void AddBaseCards(int victoryCardCount, int curseCount)
        {
            AddCardPile("copper", 60);
            AddCardPile("silver", 40);
            AddCardPile("gold", 30);

            AddCardPile("estate", victoryCardCount);
            AddCardPile("duchy", victoryCardCount);
            AddCardPile("province", victoryCardCount);

            AddCardPile("curse", curseCount);
        }

        public void AddKingdom(List<string> kingdom)
        {
            foreach (var cardName in kingdom)
            {
                AddCardPile(cardName, 10);
            }
        }

        public void AddPlayers(List<string> playerNames)
        {
            foreach (var playerName in playerNames)
            {
                var newPlayer = new Player(playerName, this);
                newPlayer.BuildStartingDeck();
                Players.Add(newPlayer);
            }
        }

        public Card GetSingleCardFromSupply(string cardName)
        {
            var cardPile = SupplyPiles().Find(e => e.Card.Name == cardName);
            return cardPile.GetSingleCard();
        }

        public List<Card> GetCardsFromSupply(string cardName, int count)
        {
            var cardPile = SupplyPiles().Find(e => e.Card.Name == cardName);
            return cardPile.GetMultipleCards(count);
        }

        public List<CardPile> SupplyPiles()
        {
            return CardPiles.FindAll(e => e.Card.IsSupply);
        }

        public List<CardPile> KingdomPiles()
        {
            return CardPiles.FindAll(e => e.Card.IsKingdom);
        }

        public List<CardPile> DepletedSupplyPiles()
        {
            return SupplyPiles().FindAll(e => e.Amount == 0);
        }

        public bool IsGainable(string cardName)
        {
            return SupplyPiles().Find(e => e.Card.Name == cardName && e.Amount > 0) != null;
        }

        public void ResetAttackImmunity()
        {
            foreach (var player in Players)
            {
                player.AttackImmunity = false;
            }
        }

        public int GetCoinCost(string cardName)
        {
            return CardPiles.Find(e => e.Card.Name == cardName).Card.CoinCost;
        }

        public void WriteSupply()
        {
            foreach (var cardPile in SupplyPiles())
            {
                Console.WriteLine($"{cardPile.Amount}x" + (cardPile.Amount - 10 < 0 ? "  " : " ") + $"({cardPile.Card.CoinCost}c) {cardPile.Card.Name}");
            }
        }

        public void WriteKingdom()
        {
            foreach (var cardPile in KingdomPiles())
            {
                Console.WriteLine($"{cardPile.Amount}x" + (cardPile.Amount - 10 < 0 ? "  " : " ") + $"({cardPile.Card.CoinCost}c) {cardPile.Card.Name}");
            }
        }
    }
}
