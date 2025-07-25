using System;

Random random = new Random();
Console.CursorVisible = false;
int height = Console.WindowHeight - 1;
int width = Console.WindowWidth - 5;
bool shouldExit = false;

// Console position of the player
int playerX = 0;
int playerY = 0;

// Console position of the food
int foodX = 0;
int foodY = 0;

// Available player and food strings
string[] states = {"('-')", "(^-^)", "(X_X)"};
string[] foods = {"@@@@@", "$$$$$", "#####"};

// Current player string displayed in the Console
string player = states[0];

// Index of the current food
int food = 0;

InitializeGame();
while (!shouldExit) 
{
    // if terminal was resized exit the game
    if (TerminalResized())
    {
        Console.Clear();
        Console.WriteLine("Console was resized. Program exiting.");
        Thread.Sleep(1000);
        break;
    }

    if (ShouldPlayerSpeedUp()) Move(exitGameWithOtherKeys: true, IncreasedMovementSpeed: true);
    else Move(exitGameWithOtherKeys: true);

    if (ConsumedFood())
    {
        ChangePlayer();
        if (ShouldPlayerFreeze()) FreezePlayer();
        ShowFood();
    }
}



// - Code to determine if the player has consumed the food displayed.
bool ConsumedFood()
{
    // Food consumed when player coordinates are of the same value as food coordinates
    // (consumed = true) when (player(X,Y) = food(X,Y))

    if (playerX == foodX)
    {
        if (playerY == foodY) return true;
    }

    return false;
}

// Returns true if the Terminal was resized 
bool TerminalResized() 
{
    return height != Console.WindowHeight - 1 || width != Console.WindowWidth - 5;
}

// Displays random food at a random location
void ShowFood() // local: food, foodX, foodY
{
    // Update food to a random index
    food = random.Next(0, foods.Length);

    // Update food position to a random location
    foodX = random.Next(0, width - player.Length);
    foodY = random.Next(0, height - 1);

    // Display the food at the location
    Console.SetCursorPosition(foodX, foodY);
    Console.Write(foods[food]);
}

// Changes the player to match the food consumed
void ChangePlayer() // local: player -- needs: food, playerX, playerY
{
    player = states[food];
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
}

bool ShouldPlayerSpeedUp()
{
    return (player == states[1]);
}

bool ShouldPlayerFreeze()
{
    return (player == states[2]) ? true : false;
}

// Temporarily stops the player from moving
void FreezePlayer() 
{
    System.Threading.Thread.Sleep(1000);
    player = states[0];
}

// Reads directional input from the Console and moves the player
void Move(bool exitGameWithOtherKeys = false, bool IncreasedMovementSpeed = false)
{
    int lastX = playerX;
    int lastY = playerY; 
    
    switch (Console.ReadKey(true).Key) 
    {
        case ConsoleKey.UpArrow:
            playerY--; 
            break;
		case ConsoleKey.DownArrow: 
            playerY++;            
            break;
		case ConsoleKey.LeftArrow:  
            playerX = (IncreasedMovementSpeed) ? (playerX - 4) : playerX--; 
            break;
		case ConsoleKey.RightArrow: 
            playerX = (IncreasedMovementSpeed) ? (playerX + 4) : playerX++; 
            break;
		case ConsoleKey.Escape:     
            shouldExit = true; 
            break;
        // optional parameter to close game with any input but directional keys
        default:
            if (exitGameWithOtherKeys)
                shouldExit = true;
            break;
    }

    // Clear the characters at the previous position
    Console.SetCursorPosition(lastX, lastY);
    for (int i = 0; i < player.Length; i++) 
    {
        Console.Write(" ");
    }

    // Keep player position within the bounds of the Terminal window
    playerX = (playerX < 0) ? 0 : (playerX >= width ? width : playerX);
    playerY = (playerY < 0) ? 0 : (playerY >= height ? height : playerY);

    // Draw the player at the new location
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
}

// Clears the console, displays the food and player
void InitializeGame() 
{
    Console.Clear();
    ShowFood();
    Console.SetCursorPosition(0, 0);
    Console.Write(player);
}
