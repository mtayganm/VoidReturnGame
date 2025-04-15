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

Random random = new Random();
Console.CursorVisible = true;
bool shouldExit = false;
bool isGhostAhead = true;

// Items And Room names
string[] rooms = ["Starting Room", "Old Study", "Mirror Hallway", "Basement", "Door to the Garden"];
string[] items = ["Dice", "Key", "Ancient Book", "Candle", "Broken Clock"];


string[] playerInventory = ["Inventory :"];
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

// Reads input from the Console and moves the player
void Commands()
{
    WriteAnimation();
    string? input = Console.ReadLine();
    if (input != null)
        input = input.ToLower().Trim();
    else return;

    if (input.StartsWith("take"))
    {
        Take(input);
        return;
    }

    switch (input)
    {
        case "roll dice":
            Console.WriteLine("There is no enemy.");
            break;
        case "look":
            Look();
            break;
        case "pick":
            Console.WriteLine("pick up key");
            break;
        case "open":
            Console.WriteLine("opened door");
            isGhostAhead = true;
            break;
        case "help":
            Help();
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
                break;
            case "dice":
                playerInventory.Append("Dice").ToArray();
                Console.WriteLine("Dice taken. Your power increased!");
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
    for(int i=0;i<playerInventory.Length;i++)
    {
        if (playerInventory[i].Contains("Dice")) 
        {
            inventoryDiceCount+=5;
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
        //shouldExit = true;
        return false; //we lose
    }
}
// Help commands
void Help()
{
    Console.WriteLine("look, pick {item}, open door, help, roll, exit, look back");
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