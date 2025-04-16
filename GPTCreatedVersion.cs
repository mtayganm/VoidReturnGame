using System;
using System.Collections.Generic;

class GPTCreatedVersion
{
    static string[] rooms = { "Foyer", "Library", "Dining Hall", "Gallery", "Throne Room" };
    static bool[] ghostDefeated = new bool[5];
    static int currentRoom = 0;
    static bool lookBackUsed = false;
    static bool gameEnded = false;
    static Random rng = new Random();
    static int maxRoll = 6;

    static Dictionary<string, int> inventory = new Dictionary<string, int>();
    static string?[,] itemsInRooms = new string[5, 5]
    {
        {"key", "dice", "book", "candle", "coin"},
        {"key", "dice", "mirror", "map", "lantern"},
        {"key", "dice", "pendant", "bottle", "scroll"},
        {"key", "dice", "feather", "clock", "gem"},
        {"key", "dice", "skull", "dagger", "journal"}
    };

    static void Main()
    {
        ShowIntro();
        while (!gameEnded)
        {
            Console.WriteLine($"\nYou are now in the {rooms[currentRoom]}.");
            EncounterGhost();

            while (true)
            {
                Console.Write("\n> ");
                string? input = Console.ReadLine()?.ToLower();
                if (input == null) input="";
                switch (input)
                {
                    case "look":
                        DescribeRoom();
                        break;
                    case var s when s.StartsWith("take"):
                        HandleTake(s);
                        break;
                    case "move forward":
                        if (TryMove(1)) break;
                        continue;
                    case "move back":
                        if (TryMove(-1)) break;
                        continue;
                    case "fight":
                        TryFightGhost();
                        break;
                    case "look back":
                        lookBackUsed = true;
                        Console.WriteLine("You glance behind you... the shadows seem to shift.");
                        break;
                    case "inventory":
                        ShowInventory();
                        break;
                    case "help":
                        ShowHelp();
                        break;
                    case "open door":
                        if (currentRoom == rooms.Length - 1) CheckFinalDoor();
                        else Console.WriteLine("There is no door to open here.");
                        break;
                    default:
                        Console.WriteLine("Unknown command.");
                        break;
                }

                if (ghostDefeated[currentRoom]) break;
            }
        }
    }

    static void ShowIntro()
    {
        Console.WriteLine("=================================================");
        Console.WriteLine("       void Back() {return;}   ");
        Console.WriteLine("=================================================");
        Console.WriteLine("     A TEXT-BASED HORROR ADVENTURE GAME    ");
        Console.WriteLine("-------------------------------------------------");
        Console.WriteLine();
        Console.WriteLine("  You wake up in a dark room. A ghost looms over you.");
        Console.WriteLine("  The air is cold. Your head is spinning.");
        Console.WriteLine("  A whisper fills the silence...");
        Console.WriteLine();
        Console.WriteLine("  \"You are finally awake. Take the dice and face your fate...");
        Console.WriteLine("   Win, or stay here forever.\"");
        Console.WriteLine();
        Console.WriteLine("  >> Type \"help\" for a list of commands.");
    }

    static void ShowHelp()
    {
        Console.WriteLine("\nAvailable commands:");
        Console.WriteLine("look         - Observe the room.");
        Console.WriteLine("take [item]  - Pick up an item (e.g. take key).");
        Console.WriteLine("fight        - Roll dice and try to escape the ghost.");
        Console.WriteLine("move forward - Go to the next room (after defeating ghost).");
        Console.WriteLine("move back    - Return to the previous room.");
        Console.WriteLine("open door    - Try to open the final door (Throne Room only).");
        Console.WriteLine("look back    - (Mysterious command...)");
        Console.WriteLine("inventory    - Show your inventory.");
        Console.WriteLine("help         - Show this help message.");
    }

    static void DescribeRoom()
    {
        Console.WriteLine($"A ghost haunts the {rooms[currentRoom]}. Around you, scattered items glint in the dim light:");
        for (int i = 0; i < 5; i++)
        {
            if (!string.IsNullOrEmpty(itemsInRooms[currentRoom, i]))
                Console.WriteLine($"- {itemsInRooms[currentRoom, i]}");
        }
    }

    static void HandleTake(string input)
    {
        if (input == "take")
        {
            Console.WriteLine("What would you like to take?");
            Console.Write(" > ");
            input = "take " + Console.ReadLine()?.ToLower();
        }

        string item = input.Substring(5);
        bool found = false;

        for (int i = 0; i < 5; i++)
        {
            if (itemsInRooms[currentRoom, i] == item)
            {
                if (inventory.ContainsKey(item))
                    inventory[item]++;
                else
                    inventory[item] = 1;

                itemsInRooms[currentRoom, i] = null;
                Console.WriteLine($"You took the {item}.");

                if (item == "dice")
                {
                    maxRoll += 3;
                    Console.WriteLine("You feel luckier. Your max dice roll increased!");
                }

                found = true;
                break;
            }
        }

        if (!found)
            Console.WriteLine("There's nothing like that here.");
    }

    static void EncounterGhost()
    {
        if (!ghostDefeated[currentRoom])
        {
            Console.WriteLine("A ghost blocks your path. You must fight it to move on.");
        }
    }

    static void TryFightGhost()
    {
        if (ghostDefeated[currentRoom])
        {
            Console.WriteLine("You've already dealt with this ghost.");
            return;
        }

        int roll = rng.Next(1, maxRoll + 1);
        Console.WriteLine($"You rolled a {roll}.");

        if (roll >= 5)
        {
            ghostDefeated[currentRoom] = true;
            Console.WriteLine("You dodged the ghost successfully!");
        }
        else
        {
            Console.WriteLine("You failed to escape. Try again or find a way to increase your odds.");
        }
    }

    static bool TryMove(int direction)
    {
        if (!ghostDefeated[currentRoom])
        {
            Console.WriteLine("You can't leave until you've faced the ghost.");
            return false;
        }

        int nextRoom = currentRoom + direction;
        if (nextRoom >= 0 && nextRoom < rooms.Length)
        {
            currentRoom = nextRoom;
            return true;
        }
        else
        {
            Console.WriteLine("You can't move in that direction.");
            return false;
        }
    }

    static void ShowInventory()
    {
        Console.WriteLine("Inventory:");
        foreach (var item in inventory)
        {
            Console.WriteLine($"- {item.Key} x{item.Value}");
        }
    }

    static void CheckFinalDoor()
    {
        Console.WriteLine("You stand before the final door of the chateau...");

        if (!inventory.ContainsKey("key") || inventory["key"] < 5)
        {
            Console.WriteLine("You reach for the lock... but you're missing some keys.");
            Console.WriteLine("The spirits laugh as the door remains sealed.");
            return;
        }

        Console.WriteLine("You use all 5 keys to open it.");

        if (!lookBackUsed)
        {
            Console.WriteLine("\nBut something doesn't feel right...");
            int fate = rng.Next(0, 2);
            if (fate == 0)
            {
                Console.WriteLine("The spirits claim you. You are lost forever.");
                Console.WriteLine(">> GAME OVER <<");
                gameEnded = true;
                return;
            }
        }

        Console.WriteLine("=================================================");
        Console.WriteLine("       void Back() {return;}   ");
        Console.WriteLine("=================================================");
        Console.WriteLine("            ESCAPE SUCCESSFUL    ");
        Console.WriteLine("-------------------------------------------------");
        Console.WriteLine();
        Console.WriteLine("  The final door creaks open. Cold air rushes in.");
        Console.WriteLine("  You step into a garden, surrounded by barbed wire.");
        Console.WriteLine("  As you push through, sharp thorns cut into your skin.");
        Console.WriteLine("  Blood drips onto the ground. A scream echoes behind you...");
        Console.WriteLine();
        Console.WriteLine("  But you do not look back.");
        Console.WriteLine("  The gray sky begins to clear.");
        Console.WriteLine("  The nightmare is over. Or is it?");
        Console.WriteLine();
        Console.WriteLine("  >> GAME OVER <<");

        gameEnded = true;
    }
}
