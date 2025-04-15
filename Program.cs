//** Opening screen write game name and start.
// *Dice mechanic to win against enemy. If we lose game over, if we win we continue.
// *Look mechanic we can search the current room. 
// **Use Array include room name, item name.
// *Take mechanic we can take item to our inventory.
// Open door mechanic to open door.
// look back for easter egg. at the last room.
// Each room we roll dice game to the new enemy.
// Dice item for increase dice randoms. [start 7, increase 3, final 22]
// Final screen write game over message.

// Update 1.2: arrays added, inventory and current room declared, look commands included.
// Update 1.3: take command added, there is no enemy added, restart game when lose first fight.
// Update 1.4: take command if added to commands method, keyword added to take method.(if clock or book written you can take Ancient book and Broken Clock.)
// Update 1.5: 
// added git - desktop update test.
// case updated. in dice method shouldExit get commented temporary for fast-fix *******
// areWeLost,isKeyTaken,isDiceTaken added.
// Spagetti code continues.
// gameui deleted. final stage added.

Random random = new Random();
Console.CursorVisible = true;
bool shouldExit = false;
bool isGhostAhead = true;
bool isKeyTaken = false;
bool isDiceTaken = false;
bool areWeLost = false;

// Items And Room names
string[] rooms = ["Starting Room", "Old Study", "Mirror Hallway", "Basement", "Door to the Garden"];
string[] items = ["Dice", "Key", "Ancient Book", "Candle", "Broken Clock"];


string[] playerInventory = [];
int currentRoom = 0;
// Opening screen write game name and start.
InitializeGame();
while (!shouldExit)
{

    if (isGhostAhead)
        Battle();
    else
        Commands();
}
if (!areWeLost)
{
    //Final Stage:
    FinalStage();

}


// Reads input from the Console and moves the player
void Commands()
{
    WriteAnimation();
    string? input = Console.ReadLine();
    if (input != null)
        input = input.ToLower().Trim();
    else return;

    switch (input)
    {
        case string roll when input.StartsWith("roll"):
            Console.WriteLine("There is no enemy.");
            break;
        case "look":
            Look();
            break;
        case string open when input.StartsWith("open"):
            if (isKeyTaken)
            {
                currentRoom++;
                if (currentRoom == 5)
                {
                    shouldExit = true;
                    break;
                }
                Console.WriteLine($"You unlock the next door and move forward into the {rooms[currentRoom]}.");

                isKeyTaken = false;
                isDiceTaken = false;
                isGhostAhead = true;
                Console.WriteLine("Another ghost prepares to attack. Roll your dice!.");

                break;

            }
            Console.WriteLine("The door is locked. You must have the key.");
            break;

        case "help":
            Console.WriteLine($"You are at: {rooms[currentRoom]}");
            Help();
            break;
        case string take when input.StartsWith("take"): //chatgpt
            Take(input);
            break;
        case string inventory when input.StartsWith("inv"):
            if (playerInventory.Length == 0)
                Console.WriteLine("You carry nothing but your will.");
            else
                Console.WriteLine("Inventory: " + string.Join(", ", playerInventory));
            break;
        case "exit":
            shouldExit = true;
            break;
        default:
            if (input != "")
                Console.WriteLine($"I dont that command. \"{input}\" ");
            shouldExit = false;
            break;
    }

}

void Take(string? input)
{
    string[] keywords = ["dice", "key", "book", "candle", "clock"];
    if (input != null)
    {
        input = input.Remove(0, 4).Trim();
        if (!(input.Contains("key") || input.Contains("dice") || input.Contains("book") || input.Contains("candle") || input.Contains("clock")))
        {
            Console.WriteLine("What do you want to take?");
            input = Console.ReadLine();

            if (input != null)
                foreach (string keyword in keywords)
                {
                    if (input.Contains(keyword))
                        input = keyword;
                }

        }
        switch (input)
        {
            case "key":
                if (!isKeyTaken)
                {
                    playerInventory = playerInventory.Append($"Key {currentRoom + 1}").ToArray();
                    Console.WriteLine("Key taken. You can unlock the door.");
                    isKeyTaken = true;
                    break;
                }
                Console.WriteLine("You already took that key!");
                break;
            case "dice":
                if (!isDiceTaken)
                {
                    playerInventory = playerInventory.Append($"Dice {currentRoom + 1}").ToArray();
                    Console.WriteLine("Dice taken. Your power increased!");
                    isDiceTaken = true;
                    break;
                }
                Console.WriteLine("You already took it!");
                break;

            case "book":
                break;
            case "candle":
                break;
            case "clock":
                break;
            default:
                break;
        }
    }
}

void Look()
{
    Console.WriteLine("When you look around the room you see bunch of items");
    bool hasComma = false;
    foreach (string item in items)
    {
        if (hasComma)
            Console.Write(", ");
        Console.Write($"{item}");
        hasComma = true;
    }
    Console.WriteLine();
}

//Battle against ghost.
void Battle()
{
    do
    {
        WriteAnimation();
        string? roll = Console.ReadLine();
        if (roll != null)
        {
            if (roll == "roll" || roll == "roll dice" || roll == "r")
            {
                Dice(InventoryDiceCount());
                break;
            }
            else if (roll == "help")
            {
                Help();
            }
            else
            {
                Console.WriteLine("Roll dice to fight.");
            }
        }

    } while (true);
}

//Inventory dice calculator
int InventoryDiceCount()
{
    int inventoryDiceCount = 7;
    for (int i = 0; i < playerInventory.Length; i++)
    {
        if (playerInventory[i].Contains("Dice"))
        {
            inventoryDiceCount += 5;
        }
    }
    return inventoryDiceCount;
}
// Dice mechanic. If we lose game over, if we win we continue.
bool Dice(int diceCount = 7)
{
    int enemyDice = random.Next(0, 7);
    int playerDice = random.Next(0, diceCount);
    if (playerDice >= enemyDice)
    {
        Console.WriteLine("You win againsts ghost. Fade is from your side. ");
        isGhostAhead = false;
        return true; //we win
    }
    else
    {
        Console.WriteLine("You LOST! You are going to trap here forever.");
        Console.WriteLine("Press Enter to exit.");
        Console.ReadLine();
        shouldExit = true;
        areWeLost = true;
        return false; //we lose
    }
}
// Help commands
void Help()
{
    Console.WriteLine("look, take {item}, open door, help, roll, exit,inventory, look back");
}

void FinalStage()
{
    Console.Clear();
    Console.WriteLine("=======================================");
    Console.WriteLine("\tvoid Back() {return;}");
    Console.WriteLine("=======================================");
    Console.WriteLine("  \tESCAPE SUCCESSFUL");
    Console.WriteLine("---------------------------------------\n");

    Console.WriteLine("  The final door creaks open. Cold air rushes in.");
    Console.WriteLine("  You step into a garden, surrounded by barbed wire.\n");
    Console.WriteLine("  As you push through, sharp thorns cut into your skin.\n");
    Console.WriteLine("  Blood drips onto the ground. A scream echoes behind you...\n");
    Console.WriteLine("  But you do not look back.\n  The gray sky begins to clear.\n  The nightmare is over.\n  Or is it?");

    Console.WriteLine("\nPress Enter to Exit. But... something whispers behind you.");
    WriteAnimation();

    string? input = Console.ReadLine()?.ToLower();

    if (input == "look back")
    {
        Console.Write("You turn, but see nothing. The whisper stops. You walk into the light. You are free.");
    }
    else
    {
        int chance = random.Next(2);
        if (chance == 0)
        {
            Console.Write("You ignore the whisper. A claw tears through the veil. You vanish into the dark.");
            shouldExit = true;
        }
        else
        {
            Console.Write("You charge forward without looking. The whisper fades. You made it out—barely.");
        }
    }

    Console.WriteLine("\n\n  >> GAME OVER <<");
}


// Clears the console, displays the first message.
void InitializeGame()
{

    Console.Clear();
    Console.WriteLine("=======================================");
    Console.WriteLine("\tvoid Back() {return;}");
    Console.WriteLine("=======================================");
    Console.WriteLine("  A TEXT-BASED HORROR ADVENTURE GAME");
    Console.WriteLine("---------------------------------------\n");

    Console.WriteLine("  You wake up in a dark room. The air is cold. Your head is spinning.");
    Console.WriteLine("  A  ghost appears from the darkness and whispers coldly:\n");
    Console.WriteLine("  Hey, you. You're finally awake. Take the dice and face your fate...\n  Win, or trapped here forever.\n");
    Console.WriteLine("Dice taken.");
    //Console.WriteLine("  >> Type \"roll dice\" to fight. ");
    //Console.WriteLine("  >> Type \"help\" for a list of commands. ");
}
// >> and setcursorposition for better input view.
void WriteAnimation()
{
    Console.Write(">>");
    Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
}
