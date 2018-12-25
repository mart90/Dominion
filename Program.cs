using System;
using System.Collections.Generic;

namespace Dominion
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game(
                new List<string>
                {
                    "cellar",
                    "market",
                    "merchant",
                    "militia",
                    "mine",
                    "moat",
                    "remodel",
                    "smithy",
                    "village",
                    "workshop"
                },
                new List<string>
                {
                    "test1",
                    "test2"
                });

            game.Play();
            Console.WriteLine("Game ended. Final score:");
            foreach (var player in game.Players)
            {
                Console.WriteLine($"{player.Name}: {player.VictoryPoints}");
            }

            Console.ReadLine();
        }
    }
}
