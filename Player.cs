using System;
using System.Collections.Generic;

namespace Dominion
{
    class Player
    {
        public Player(string name, Game game)
        {
            Name = name;
            Game = game;
        }

        public Game Game { get; set; }
        public string Name { get; set; }
        public List<Card> Deck { get; set; } = new List<Card>();
        public List<Card> DiscardPile { get; set; } = new List<Card>();
        public List<Card> CardsInPlay { get; set; } = new List<Card>();
        public List<Card> CardsInHand { get; set; } = new List<Card>();
        public int Actions { get; set; }
        public int Buys { get; set; }
        public int Coins { get; set; }
        public int VictoryPoints { get; set; }
        public bool AttackImmunity { get; set; }

        public void BuildStartingDeck()
        {
            for (int i = 0; i < 7; i++)
            {
                GainCardToDiscardPile("copper");
            }
            for (int i = 0; i < 3; i++)
            {
                GainCardToDiscardPile("estate");
            }
        }

        public void ResolveCardEffects(Card card, EffectPhase phase)
        {
            var effects = card.Effects.FindAll(e => e.EffectPhase == phase);
            foreach (var effect in effects)
            {
                effect.Effect(this);
            }
        }

        public Card GainCard(string cardName)
        {
            if (!Game.IsGainable(cardName))
            {
                return null;
            }
            var card = Game.GetSingleCardFromSupply(cardName);
            ResolveCardEffects(card, EffectPhase.OnGain);
            return card;
        }

        public List<Card> GetCardsFromDeck(int count)
        {
            var cards = new List<Card>();

            for (int i = 0; i < count; i++)
            {
                if (Deck.Count == 0)
                {
                    DiscardPile.Shuffle();
                    Deck.AddRange(DiscardPile);
                    DiscardPile.Clear();
                }
                cards.Add(Deck[0]);
                Deck.RemoveAt(0);
            }
            return cards;
        }

        public Card GetSingleCardFromDeck()
        {
            return GetCardsFromDeck(1)[0];
        }

        public void GainCardToHand(string cardName)
        {
            CardsInHand.Add(GainCard(cardName));
        }

        public void GainCardToDiscardPile(string cardName)
        {
            DiscardPile.Add(GainCard(cardName));
        }

        public void DrawCards(int amount)
        {
            CardsInHand.AddRange(GetCardsFromDeck(amount));
        }

        public void DrawCard()
        {
            CardsInHand.Add(GetCardsFromDeck(1)[0]);
        }

        public List<Player> GetOtherPlayers()
        {
            return Game.Players.FindAll(e => e != this);
        }

        public List<Player> GetOtherPlayersWithoutImmunity()
        {
            return Game.Players.FindAll(e => e != this && !e.AttackImmunity);
        }

        public Card FindCardInHand(string cardName)
        {
            return CardsInHand.Find(e => e.Name == cardName);
        }

        public Card FindCardInDiscardPile(string cardName)
        {
            return DiscardPile.Find(e => e.Name == cardName);
        }

        public void PlayCardFromHand(Card card)
        {
            if (card.Types.Contains(CardType.Action))
            {
                Actions--;
            }

            CardsInHand.Remove(card);
            CardsInPlay.Add(card);
            ResolveCardEffects(card, EffectPhase.OnPlay);
        }

        public void BuyCard(string cardName)
        {
            int coinCost = Game.GetCoinCost(cardName);
            if (coinCost <= Coins)
            {
                GainCardToDiscardPile(cardName);
            }
            Coins -= coinCost;
            Buys--;
        }

        public void DoTurn()
        {
            Game.ResetAttackImmunity();

            Console.WriteLine($"Turn: {Name}\n");

            Actions = 1;
            Buys = 1;
            Coins = 0;

            while (true)
            {
                WriteResources();

                try
                {
                    string input = Console.ReadLine();
                    if (input == "end")
                    {
                        break;
                    }
                    else if (input.StartsWith("play"))
                    {
                        var cardToPlayString = input.Split(' ')[1];
                        if (cardToPlayString == "treasures")
                        {
                            PlayAllTreasures();
                        }
                        else
                        {
                            var cardToPlay = FindCardInHand(cardToPlayString);
                            PlayCardFromHand(cardToPlay);
                        }
                    }
                    else if (input.StartsWith("buy"))
                    {
                        string cardToBuyString = input.Split(' ')[1];
                        BuyCard(cardToBuyString);
                    }
                    else if (input == "supply")
                    {
                        Game.WriteSupply();
                    }
                    else if (input == "kingdom")
                    {
                        Game.WriteKingdom();
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Nope. Try again");
                }
            }

            DiscardPile.AddRange(CardsInHand);
            DiscardPile.AddRange(CardsInPlay);
            CardsInHand.Clear();
            CardsInPlay.Clear();

            DrawCards(5);
        }

        public void WriteResources()
        {
            Console.Write("Cards in hand: ");
            foreach (var card in CardsInHand)
            {
                Console.Write($"{card.Name}, ");
            }

            Console.WriteLine($"\n\nActions: {Actions}, Buys: {Buys}, Coins: {Coins}");
        }

        public void MoveAllCardsInPlayAndHandToDiscardPile()
        {
            DiscardPile.AddRange(CardsInPlay);
            DiscardPile.AddRange(CardsInHand);

            CardsInPlay.Clear();
            CardsInHand.Clear();
        }

        public void TrashCard(Card card)
        {
            Game.Trash.Add(card);
        }

        public void Reaction()
        {
            foreach (var reactionCard in CardsInHand.FindAll(e => e.Types.Contains(CardType.Reaction)))
            {
                Console.WriteLine($"{Name} has {reactionCard.Name}. Use? y/n");
                if (Console.ReadLine() == "y")
                {
                    ResolveCardEffects(reactionCard, EffectPhase.OnReaction);
                }
            }
        }

        public void PlayAllTreasures()
        {
            foreach (var treasureCard in CardsInHand.FindAll(e => e.Types.Contains(CardType.Treasure)))
            {
                PlayCardFromHand(treasureCard);
            }
        }
    }
}
