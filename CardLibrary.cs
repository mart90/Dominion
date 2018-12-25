using System;
using System.Collections.Generic;

namespace Dominion
{
    class CardLibrary
    {
        public static Card GetCard(string cardName)
        {
            return Cards.Find(e => e.Name == cardName);
        }

        public static List<Card> Cards = new List<Card>
        {
            new Card
            {
                Name = "copper",
                Text = "",
                Expansion = Expansion.Base,
                Types = new List<CardType>
                {
                    CardType.Treasure
                },
                CoinCost = 0,
                IsSupply = true,
                IsKingdom = false,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnPlay,
                        Effect = player => player.Coins++
                    }
                }
            },

            new Card
            {
                Name = "silver",
                Text = "",
                Expansion = Expansion.Base,
                Types = new List<CardType>
                {
                    CardType.Treasure
                },
                CoinCost = 3,
                IsSupply = true,
                IsKingdom = false,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnPlay,
                        Effect = player => player.Coins += 2
                    }
                }
            },

            new Card
            {
                Name = "gold",
                Text = "",
                Expansion = Expansion.Base,
                Types = new List<CardType>
                {
                    CardType.Treasure
                },
                CoinCost = 6,
                IsSupply = true,
                IsKingdom = false,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnPlay,
                        Effect = player => player.Coins += 3
                    }
                }
            },

            new Card
            {
                Name = "estate",
                Text = "",
                Expansion = Expansion.Base,
                Types = new List<CardType>
                {
                    CardType.Victory
                },
                CoinCost = 2,
                IsSupply = true,
                IsKingdom = false,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnGain,
                        Effect = player => player.VictoryPoints++
                    },
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnTrash,
                        Effect = player => player.VictoryPoints--
                    }
                }
            },

            new Card
            {
                Name = "duchy",
                Text = "",
                Expansion = Expansion.Base,
                Types = new List<CardType>
                {
                    CardType.Victory
                },
                CoinCost = 5,
                IsSupply = true,
                IsKingdom = false,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnGain,
                        Effect = player => player.VictoryPoints += 3
                    },
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnTrash,
                        Effect = player => player.VictoryPoints -= 3
                    }
                }
            },

            new Card
            {
                Name = "province",
                Text = "",
                Expansion = Expansion.Base,
                Types = new List<CardType>
                {
                    CardType.Victory
                },
                CoinCost = 8,
                IsSupply = true,
                IsKingdom = false,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnGain,
                        Effect = player => player.VictoryPoints += 6
                    },
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnTrash,
                        Effect = player => player.VictoryPoints -= 6
                    }
                }
            },

            new Card
            {
                Name = "curse",
                Text = "",
                Expansion = Expansion.Base,
                Types = new List<CardType>
                {
                    CardType.Curse
                },
                CoinCost = 0,
                IsSupply = true,
                IsKingdom = false,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnGain,
                        Effect = player => player.VictoryPoints--
                    },
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnTrash,
                        Effect = player => player.VictoryPoints++
                    }
                }
            },

            new Card
            {
                Name = "artisan",
                Text = "Gain a card to your hand costing up to 5 Coins.\n" +
                       "Put a card from your hand onto your deck.",
                Expansion = Expansion.Base,
                Types = new List<CardType>
                {
                    CardType.Action
                },
                CoinCost = 6,
                IsSupply = true,
                IsKingdom = true,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnPlay,
                        Effect = player =>
                        {
                            Console.WriteLine("Choose a card to gain: ");
                            string cardName = Console.ReadLine();
                            player.GainCardToHand(cardName);

                            Console.WriteLine("Choose a card to put onto your deck: ");
                            cardName = Console.ReadLine();
                            var card = player.FindCardInHand(cardName);
                            player.CardsInHand.Remove(card);
                            player.Deck.Insert(0, card);
                        }
                    }
                }
            },

            new Card
            {
                Name = "bandit",
                Text = "Gain a Gold. Each other player reveals the top 2 cards of their deck, trashes a revealed Treasure other than Copper, and discards the rest.",
                Expansion = Expansion.Base,
                Types = new List<CardType>
                {
                    CardType.Action,
                    CardType.Attack
                },
                CoinCost = 5,
                IsSupply = true,
                IsKingdom = true,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnPlay,
                        Effect = player =>
                        {
                            player.GainCardToDiscardPile("gold");

                            foreach (var otherPlayer in player.GetOtherPlayersWithoutImmunity())
                            {
                                var revealedCards = otherPlayer.GetCardsFromDeck(2);
                                var revealedTreasuresOtherThanCopper = new List<Card>();

                                foreach (var revealedCard in revealedCards)
                                {
                                    if (revealedCard.Name != "copper" && revealedCard.Types.Contains(CardType.Treasure))
                                    {
                                        revealedTreasuresOtherThanCopper.Add(revealedCard);
                                    }
                                    else
                                    {
                                        otherPlayer.DiscardPile.Add(revealedCard);
                                    }
                                }

                                if (revealedTreasuresOtherThanCopper.Count > 0)
                                {
                                    Card cardToTrash;

                                    if (revealedTreasuresOtherThanCopper.Count == 1)
                                    {
                                        cardToTrash = revealedTreasuresOtherThanCopper[0];
                                        Console.WriteLine($"{otherPlayer.Name} trashes a {cardToTrash.Name}");
                                        otherPlayer.TrashCard(cardToTrash);
                                    }
                                    else
                                    {
                                        Console.WriteLine($"{otherPlayer.Name} reveals a {revealedTreasuresOtherThanCopper[0]} and a {revealedTreasuresOtherThanCopper[1]}. Pick one to trash: ");

                                        var cardToTrashString = Console.ReadLine();
                                        cardToTrash = revealedTreasuresOtherThanCopper.Find(e => e.Name == cardToTrashString);

                                        otherPlayer.TrashCard(cardToTrash);
                                        otherPlayer.DiscardPile.Add(revealedTreasuresOtherThanCopper.Find(e => e != cardToTrash));
                                    }
                                }
                            }
                        }
                    }
                }
            },

            new Card
            {
                Name = "bureaucrat",
                Text = "Gain a Silver onto your deck. Each other player reveals a Victory card from their hand and puts in onto their deck (or reveals a hand with no Victory cards).",
                Expansion = Expansion.Base,
                Types = new List<CardType>
                {
                    CardType.Action,
                    CardType.Attack
                },
                CoinCost = 4,
                IsSupply = true,
                IsKingdom = true,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnPlay,
                        Effect = player =>
                        {
                            player.GainCardToDiscardPile("silver");

                            foreach (var otherPlayer in player.GetOtherPlayersWithoutImmunity())
                            {
                                var victoryCardsInHand = otherPlayer.CardsInHand.FindAll(e => e.Types.Contains(CardType.Victory));
                                if (victoryCardsInHand.Count == 1)
                                {
                                    otherPlayer.CardsInHand.Remove(victoryCardsInHand[0]);
                                    otherPlayer.Deck.Insert(0, victoryCardsInHand[0]);
                                }
                                else if (victoryCardsInHand.Count > 1)
                                {
                                    Console.WriteLine($"{otherPlayer.Name} reveals:");
                                    foreach (var victoryCard in victoryCardsInHand)
                                    {
                                        Console.WriteLine(victoryCard.Name);
                                    }
                                    Console.WriteLine("Choose one to put onto their deck: ");

                                    var cardToPutOnDeckString = Console.ReadLine();
                                    var card = otherPlayer.FindCardInHand(cardToPutOnDeckString);

                                    otherPlayer.CardsInHand.Remove(card);
                                    otherPlayer.Deck.Insert(0, card);
                                }
                            }
                        }
                    }
                }
            },

            new Card
            {
                Name = "cellar",
                Text = "+1 Action\n" +
                       "Discard any number of cards, then draw that many.",
                Expansion = Expansion.Base,
                Types = new List<CardType>
                {
                    CardType.Action
                },
                CoinCost = 2,
                IsSupply = true,
                IsKingdom = true,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnPlay,
                        Effect = player =>
                        {
                            player.Actions++;
                            
                            while (true)
                            {
                                Console.WriteLine("Choose cards to discard: ");
                                var cardsToDiscardString = Console.ReadLine();
                                var cardsToDiscardArray = cardsToDiscardString.Split(' ');

                                foreach (var cardName in cardsToDiscardArray)
                                {
                                    var card = player.FindCardInHand(cardName);
                                    player.CardsInHand.Remove(card);
                                    player.DiscardPile.Add(card);
                                }

                                player.DrawCards(cardsToDiscardArray.Length);
                            }
                        }
                    }
                }
            },

            new Card
            {
                Name = "chapel",
                Text = "Trash up to 4 cards from your hand.",
                Expansion = Expansion.Base,
                Types = new List<CardType>
                {
                    CardType.Action
                },
                CoinCost = 2,
                IsSupply = true,
                IsKingdom = true,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnPlay,
                        Effect = player =>
                        {
                            Console.WriteLine("Choose cards to trash: ");
                            var cardsToTrashString = Console.ReadLine();
                            var cardsToTrashArray = cardsToTrashString.Split(' ');

                            foreach (var cardName in cardsToTrashArray)
                            {
                                var card = player.FindCardInHand(cardName);
                                player.CardsInHand.Remove(card);
                                player.TrashCard(card);
                            }
                        }
                    }
                }
            },

            new Card
            {
                Name = "council room",
                Text = "+4 Cards\n" +
                       "+1 Buy\n" +
                       "Each other player draws a card.",
                Expansion = Expansion.Base,
                Types = new List<CardType>
                {
                    CardType.Action
                },
                CoinCost = 5,
                IsSupply = true,
                IsKingdom = true,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnPlay,
                        Effect = player =>
                        {
                            player.DrawCards(4);
                            player.Buys++;

                            foreach (var otherPlayer in player.GetOtherPlayers())
                            {
                                otherPlayer.DrawCard();
                            }
                        }
                    }
                }
            },

            new Card
            {
                Name = "festival",
                Text = "+2 Actions\n" +
                       "+1 Buy\n" +
                       "+2 Coins",
                Expansion = Expansion.Base,
                Types = new List<CardType>
                {
                    CardType.Action
                },
                CoinCost = 5,
                IsSupply = true,
                IsKingdom = true,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnPlay,
                        Effect = player =>
                        {
                            player.Actions += 2;
                            player.Buys++;
                            player.Coins += 2;
                        }
                    }
                }
            },

            new Card
            {
                Name = "harbinger",
                Text = "+1 Card\n" +
                       "+1 Action\n" +
                       "Look through your discard pile. You may put a card from it onto your deck.",
                Expansion = Expansion.Base,
                Types = new List<CardType>
                {
                    CardType.Action
                },
                CoinCost = 3,
                IsSupply = true,
                IsKingdom = true,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnPlay,
                        Effect = player =>
                        {
                            player.DrawCard();
                            player.Actions++;

                            Console.WriteLine("Discard pile: ");
                            foreach (var card in player.DiscardPile)
                            {
                                Console.WriteLine(card.Name);
                            }
                            Console.WriteLine("Choose one to put onto your deck: ");

                            var cardToPutOnDeckString = Console.ReadLine();
                            var cardToPutOnDeck = player.FindCardInDiscardPile(cardToPutOnDeckString);

                            player.DiscardPile.Remove(cardToPutOnDeck);
                            player.Deck.Insert(0, cardToPutOnDeck);
                        }
                    }
                }
            },

            new Card
            {
                Name = "laboratory",
                Text = "+2 Cards\n" +
                       "+1 Action",
                Expansion = Expansion.Base,
                Types = new List<CardType>
                {
                    CardType.Action
                },
                CoinCost = 5,
                IsSupply = true,
                IsKingdom = true,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnPlay,
                        Effect = player =>
                        {
                            player.DrawCards(2);
                            player.Actions++;
                        }
                    }
                }
            },

            new Card
            {
                Name = "library",
                Text = "Draw until you have 7 cards in hand, skipping any Action cards you choose to; set those aside, discarding them afterwards.",
                Expansion = Expansion.Base,
                Types = new List<CardType>
                {
                    CardType.Action
                },
                CoinCost = 5,
                IsSupply = true,
                IsKingdom = true,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnPlay,
                        Effect = player =>
                        {
                            while (player.CardsInHand.Count < 7)
                            {
                                var card = player.GetCardsFromDeck(1)[0];
                                if (card.Types.Contains(CardType.Action))
                                {
                                    Console.WriteLine($"Drew {card.Name}. Skip? y/n");
                                    if (Console.ReadLine() == "y")
                                    {
                                        player.CardsInPlay.Add(card);
                                        continue;
                                    }
                                }
                                player.CardsInHand.Add(card);
                            }
                        }
                    }
                }
            },

            new Card
            {
                Name = "market",
                Text = "+1 Card\n" +
                       "+1 Action\n" +
                       "+1 Buy\n" +
                       "+1 Coin",
                Expansion = Expansion.Base,
                Types = new List<CardType>
                {
                    CardType.Action
                },
                CoinCost = 5,
                IsSupply = true,
                IsKingdom = true,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnPlay,
                        Effect = player =>
                        {
                            player.DrawCard();
                            player.Actions++;
                            player.Buys++;
                            player.Coins++;
                        }
                    }
                }
            },

            new Card
            {
                Name = "merchant",
                Text = "+1 Card\n" +
                       "+1 Action\n" +
                       "The first time you play a Silver this turn, +1 Coin.",
                Expansion = Expansion.Base,
                Types = new List<CardType>
                {
                    CardType.Action
                },
                CoinCost = 3,
                IsSupply = true,
                IsKingdom = true,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnPlay,
                        Effect = player =>
                        {
                            player.DrawCard();
                            player.Actions++;

                            // TODO dis shit is lazy af
                            if (player.FindCardInHand("silver") != null)
                            {
                                player.Coins++;
                            }
                        }
                    }
                }
            },

            new Card
            {
                Name = "militia",
                Text = "+2 Coins\n" +
                       "Each other player discards down to 3 cards in hand.",
                Expansion = Expansion.Base,
                Types = new List<CardType>
                {
                    CardType.Action
                },
                CoinCost = 4,
                IsSupply = true,
                IsKingdom = true,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnPlay,
                        Effect = player =>
                        {
                            player.Coins += 2;
                            
                            foreach (var otherPlayer in player.GetOtherPlayers())
                            {
                                Console.WriteLine($"{otherPlayer.Name} hand:");
                                foreach (var card in otherPlayer.CardsInHand)
                                {
                                    Console.WriteLine(card.Name);
                                }
                                Console.WriteLine($"Choose {otherPlayer.CardsInHand.Count - 3} to discard: ");

                                var cardsToDiscardString = Console.ReadLine();
                                var cardsToDiscardArray = cardsToDiscardString.Split(' ');

                                foreach (var cardName in cardsToDiscardArray)
                                {
                                    var card = otherPlayer.FindCardInHand(cardName);
                                    otherPlayer.CardsInHand.Remove(card);
                                    otherPlayer.DiscardPile.Add(card);
                                }
                            }
                        }
                    }
                }
            },

            new Card
            {
                Name = "mine",
                Text = "You may trash a Treasure from your hand. Gain a Treasure to your hand costing up to 3 Coins more than it.",
                Expansion = Expansion.Base,
                Types = new List<CardType>
                {
                    CardType.Action
                },
                CoinCost = 5,
                IsSupply = true,
                IsKingdom = true,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnPlay,
                        Effect = player =>
                        {                            
                            if (player.CardsInHand.Find(e => e.Types.Contains(CardType.Treasure)) == null)
                            {
                                return;
                            }
                            Console.WriteLine("Choose a treasure to trash: ");
                            var cardToTrashString = Console.ReadLine();

                            var cardToTrash = player.FindCardInHand(cardToTrashString);
                            player.CardsInHand.Remove(cardToTrash);
                            player.TrashCard(cardToTrash);

                            Console.WriteLine("Choose a treasure to gain to your hand costing up to 3 more:");
                            var cardToGainString = Console.ReadLine();
                            player.GainCardToHand(cardToGainString);
                        }
                    }
                }
            },

            new Card
            {
                Name = "moat",
                Text = "+2 Cards\n" +
                       "When another player plays an Attack card, you may first reveal this from your hand, to be unaffected by it.",
                Expansion = Expansion.Base,
                Types = new List<CardType>
                {
                    CardType.Action,
                    CardType.Reaction
                },
                CoinCost = 2,
                IsSupply = true,
                IsKingdom = true,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnPlay,
                        Effect = player =>
                        {
                            player.DrawCards(2);
                        }
                    },
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnReaction,
                        Effect = player =>
                        {
                            player.AttackImmunity = true;
                        }
                    }
                }
            },

            new Card
            {
                Name = "moneylender",
                Text = "You may trash a Copper from your hand for +3 Coins.",
                Expansion = Expansion.Base,
                Types = new List<CardType>
                {
                    CardType.Action
                },
                CoinCost = 4,
                IsSupply = true,
                IsKingdom = true,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnPlay,
                        Effect = player =>
                        {
                            var copper = player.FindCardInHand("copper");
                            if (copper != null)
                            {
                                Console.WriteLine("Trash a copper? y/n");
                                if (Console.ReadLine() == "y")
                                {
                                    player.TrashCard(copper);
                                    player.Coins += 3;
                                }
                            }
                        }
                    }
                }
            },

            new Card
            {
                Name = "poacher",
                Text = "+1 Card\n" +
                       "+1 Action\n" +
                       "+1 Coin\n" +
                       "Discard a card per empty Supply pile.",
                Expansion = Expansion.Base,
                Types = new List<CardType>
                {
                    CardType.Action
                },
                CoinCost = 4,
                IsSupply = true,
                IsKingdom = true,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnPlay,
                        Effect = player =>
                        {
                            player.DrawCard();
                            player.Actions++;
                            player.Coins++;

                            var emptySupplyPiles = player.Game.DepletedSupplyPiles().Count;
                            if (emptySupplyPiles == 0)
                            {
                                return;
                            }

                            Console.WriteLine($"Discard {emptySupplyPiles} cards: ");
                            var cardsToDiscardString = Console.ReadLine();
                            var cardsToDiscardArray = cardsToDiscardString.Split(' ');

                            foreach (var cardName in cardsToDiscardArray)
                            {
                                var card = player.FindCardInHand(cardName);
                                player.CardsInHand.Remove(card);
                                player.DiscardPile.Add(card);
                            }
                        }
                    }
                }
            },

            new Card
            {
                Name = "remodel",
                Text = "Trash a card from your hand. Gain a card costing up to 2 Coins more than it.",
                Expansion = Expansion.Base,
                Types = new List<CardType>
                {
                    CardType.Action
                },
                CoinCost = 4,
                IsSupply = true,
                IsKingdom = true,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnPlay,
                        Effect = player =>
                        {
                            Console.WriteLine("Choose a card to trash: ");

                            var cardToTrashString = Console.ReadLine();
                            var cardToTrash = player.FindCardInHand(cardToTrashString);
                            player.TrashCard(cardToTrash);

                            Console.WriteLine("Choose a card to gain costing up to 2 Coins more: ");

                            var cardToGainString = Console.ReadLine();
                            player.GainCardToDiscardPile(cardToGainString);
                        }
                    }
                }
            },

            new Card
            {
                Name = "sentry",
                Text = "+1 Card\n" +
                       "+1 Action\n" +
                       "Look at the top 2 cards of your deck. Trash and/or discard any number of them. Put the rest back on top in any order.",
                Expansion = Expansion.Base,
                Types = new List<CardType>
                {
                    CardType.Action
                },
                CoinCost = 5,
                IsSupply = true,
                IsKingdom = true,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnPlay,
                        Effect = player =>
                        {
                            player.DrawCard();
                            player.Actions++;

                            var revealedCards = player.GetCardsFromDeck(2);

                            Console.WriteLine($"Cards revealed: {revealedCards[0].Name}, {revealedCards[1].Name}. Choose which to trash: ");
                            var cardsToTrashString = Console.ReadLine();
                            var cardsToTrashArray = cardsToTrashString.Split(' ');

                            if (cardsToTrashArray.Length == 2)
                            {
                                player.TrashCard(revealedCards[0]);
                                player.TrashCard(revealedCards[1]);
                                return;
                            }
                            if (cardsToTrashArray.Length == 1)
                            {
                                var cardToTrash = revealedCards.Find(e => e.Name == cardsToTrashArray[0]);
                                revealedCards.Remove(cardToTrash);
                                player.TrashCard(cardToTrash);
                            }

                            Console.WriteLine("Choose which to discard: ");
                            var cardsToDiscardString = Console.ReadLine();
                            var cardsToDiscardArray = cardsToDiscardString.Split(' ');

                            if (cardsToDiscardArray.Length == 2)
                            {
                                player.DiscardPile.Add(revealedCards[0]);
                                player.DiscardPile.Add(revealedCards[1]);
                                return;
                            }
                            if (cardsToDiscardArray.Length == 1)
                            {
                                var cardToDiscard = revealedCards.Find(e => e.Name == cardsToDiscardArray[0]);
                                revealedCards.Remove(cardToDiscard);
                                player.DiscardPile.Add(cardToDiscard);
                            }

                            if (revealedCards.Count == 2)
                            {
                                Console.WriteLine("Choose which to put on top: ");
                                var cardToPutOnTopString = Console.ReadLine();
                                var cardToPutOnTop = revealedCards.Find(e => e.Name == cardToPutOnTopString);

                                player.Deck.Insert(0, revealedCards.Find(e => e != cardToPutOnTop));
                                player.Deck.Insert(0, cardToPutOnTop);
                            }
                            else if (revealedCards.Count == 1)
                            {
                                player.Deck.Insert(0, revealedCards[0]);
                            }
                        }
                    }
                }
            },

            new Card
            {
                Name = "smithy",
                Text = "+3 Cards",
                Expansion = Expansion.Base,
                Types = new List<CardType>
                {
                    CardType.Action
                },
                CoinCost = 4,
                IsSupply = true,
                IsKingdom = true,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnPlay,
                        Effect = player =>
                        {
                            player.DrawCards(3);
                        }
                    }
                }
            },

            new Card
            {
                Name = "throne room",
                Text = "You may play an Action card from your hand twice.",
                Expansion = Expansion.Base,
                Types = new List<CardType>
                {
                    CardType.Action
                },
                CoinCost = 4,
                IsSupply = true,
                IsKingdom = true,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnPlay,
                        Effect = player =>
                        {
                            Console.WriteLine("Choose an action card from your hand to play twice: ");
                            var actionToPlayTwiceString = Console.ReadLine();
                            var actionToPlayTwice = player.FindCardInHand(actionToPlayTwiceString);

                            player.CardsInHand.Remove(actionToPlayTwice);
                            player.CardsInPlay.Add(actionToPlayTwice);

                            player.ResolveCardEffects(actionToPlayTwice, EffectPhase.OnPlay);
                            player.ResolveCardEffects(actionToPlayTwice, EffectPhase.OnPlay);
                        }
                    }
                }
            },

            new Card
            {
                Name = "vassal",
                Text = "+2 Coins\n" +
                       "Discard the top card of your deck. If it's an Action card, you may play it.",
                Expansion = Expansion.Base,
                Types = new List<CardType>
                {
                    CardType.Action
                },
                CoinCost = 3,
                IsSupply = true,
                IsKingdom = true,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnPlay,
                        Effect = player =>
                        {
                            player.Coins += 2;

                            var card = player.GetCardsFromDeck(1)[0];
                            player.DiscardPile.Add(card);

                            if (card.Types.Contains(CardType.Action))
                            {
                                Console.WriteLine($"Discarded {card.Name}. Play it? y/n");
                                if (Console.ReadLine() == "y")
                                {
                                    player.DiscardPile.Remove(card);
                                    player.CardsInPlay.Add(card);
                                    player.ResolveCardEffects(card, EffectPhase.OnPlay);
                                }
                            }
                        }
                    }
                }
            },

            new Card
            {
                Name = "village",
                Text = "+1 Card\n" +
                       "+2 Actions",
                Expansion = Expansion.Base,
                Types = new List<CardType>
                {
                    CardType.Action
                },
                CoinCost = 3,
                IsSupply = true,
                IsKingdom = true,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnPlay,
                        Effect = player =>
                        {
                            player.DrawCard();
                            player.Actions += 2;
                        }
                    }
                }
            },

            new Card
            {
                Name = "witch",
                Text = "+2 Cards\n" +
                       "Each other player gains a Curse.",
                Expansion = Expansion.Base,
                Types = new List<CardType>
                {
                    CardType.Action,
                    CardType.Attack
                },
                CoinCost = 5,
                IsSupply = true,
                IsKingdom = true,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnPlay,
                        Effect = player =>
                        {
                            player.DrawCards(2);
                            foreach (var otherPlayer in player.GetOtherPlayersWithoutImmunity())
                            {
                                otherPlayer.GainCardToDiscardPile("curse");
                            }
                        }
                    }
                }
            },

            new Card
            {
                Name = "workshop",
                Text = "Gain a card costing up to 4 Coins.",
                Expansion = Expansion.Base,
                Types = new List<CardType>
                {
                    CardType.Action
                },
                CoinCost = 3,
                IsSupply = true,
                IsKingdom = true,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        EffectPhase = EffectPhase.OnPlay,
                        Effect = player =>
                        {
                            Console.WriteLine("Choose a card to gain: ");
                            var cardToGainString = Console.ReadLine();
                            player.GainCardToDiscardPile(cardToGainString);
                        }
                    }
                }
            }
        };
    }
}
