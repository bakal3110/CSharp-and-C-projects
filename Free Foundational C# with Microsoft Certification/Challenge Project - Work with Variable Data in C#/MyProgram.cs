using System;
using System.Threading;

// Globalization
using System.Globalization;
CultureInfo.CurrentCulture = new CultureInfo("en-US");

const int secondInMiliseconds = 1000;

// ourAnimals array will store the following: 
string animalSpecies = "";
string animalID = "";
string animalAge = "";
string animalPhysicalDescription = "";
string animalPersonalityDescription = "";
string animalNickname = "";
string suggestedDonation = "";

// variables that support data entry
int maxPets = 8;
string? readResult;
string menuSelection = "";
decimal decimalDonation = 0.00m;

// array used to store runtime data
string[,] ourAnimals = new string[maxPets, 7];

// sample data ourAnimals array entries
for (int i = 0; i < maxPets; i++)
{
    switch (i)
    {
        case 0:
            animalSpecies = "dog";
            animalID = "d1";
            animalAge = "2";
            animalPhysicalDescription = "medium sized cream colored female golden retriever weighing about 45 pounds. housebroken.";
            animalPersonalityDescription = "loves to have her belly rubbed and likes to chase her tail. gives lots of kisses.";
            animalNickname = "lola";
            suggestedDonation = "85.00";
            break;

        case 1:
            animalSpecies = "dog";
            animalID = "d2";
            animalAge = "9";
            animalPhysicalDescription = "large reddish-brown male golden retriever weighing about 85 pounds. housebroken.";
            animalPersonalityDescription = "loves to have his ears rubbed when he greets you at the door, or at any time! loves to lean-in and give doggy hugs.";
            animalNickname = "gus";
            suggestedDonation = "49.99";
            break;

        case 2:
            animalSpecies = "cat";
            animalID = "c3";
            animalAge = "1";
            animalPhysicalDescription = "small white female weighing about 8 pounds. litter box trained.";
            animalPersonalityDescription = "friendly";
            animalNickname = "snow";
            suggestedDonation = "40.00";
            break;

        case 3:
            animalSpecies = "cat";
            animalID = "c4";
            animalAge = "";
            animalPhysicalDescription = "";
            animalPersonalityDescription = "";
            animalNickname = "lion";
            suggestedDonation = "";

            break;

        default:
            animalSpecies = "";
            animalID = "";
            animalAge = "";
            animalPhysicalDescription = "";
            animalPersonalityDescription = "";
            animalNickname = "";
            suggestedDonation = "";
            break;

    }

    ourAnimals[i, 0] = "ID #: " + animalID;
    ourAnimals[i, 1] = "Species: " + animalSpecies;
    ourAnimals[i, 2] = "Age: " + animalAge;
    ourAnimals[i, 3] = "Nickname: " + animalNickname;
    ourAnimals[i, 4] = "Physical description: " + animalPhysicalDescription;
    ourAnimals[i, 5] = "Personality: " + animalPersonalityDescription;
    
    if (!decimal.TryParse(suggestedDonation, out decimalDonation)){
        decimalDonation = 45.00m; // if suggestedDonation NOT a number, default to 45.00
    }
    ourAnimals[i, 6] = $"Suggested Donation: {decimalDonation:C2}";
}

// top-level menu options
do
{
    // NOTE: the Console.Clear method is throwing an exception in debug sessions
    Console.Clear();

    Console.WriteLine("Welcome to the Contoso PetFriends app. Your main menu options are:");
    Console.WriteLine(" 1. List all of our current pet information");
    Console.WriteLine(" 2. Display all dogs with a specified characteristic");
    Console.WriteLine();
    Console.WriteLine("Enter your selection number (or type Exit to exit the program)");

    readResult = Console.ReadLine();
    if (readResult != null)
    {
        menuSelection = readResult.ToLower();
    }

    // switch-case to process the selected menu option
    switch (menuSelection)
    {
        case "1":
            // list all pet info
            for (int i = 0; i < maxPets; i++)
            {
                if (ourAnimals[i, 0] != "ID #: ")
                {
                    Console.WriteLine();
                    for (int j = 0; j < 7; j++)
                    {
                        Console.WriteLine(ourAnimals[i, j].ToString());
                    }
                }
            }

            Console.WriteLine("\r\nPress the Enter key to continue");
            readResult = Console.ReadLine();

            break;

        case "2":
            // #1 Display all dogs with a multiple search characteristics

            string dogCharacteristic = "";

            while (dogCharacteristic == "")
            {
                // #2 have user enter multiple comma separated characteristics to search for
                Console.WriteLine($"\r\nEnter one desired dog characteristics to search for, separate with commas (',')");
                readResult = Console.ReadLine();
                if (readResult != null)
                {
                    dogCharacteristic = readResult.ToLower().Trim();
                    Console.WriteLine();
                }
            }
            
            // this splits searches into words, but it saves any spaces before a word, I need to get rid of them, maybe try trimming each word in foreach loop
            string[] dogSearches = dogCharacteristic.Split(',');
            Array.Sort(dogSearches);

            bool noMatchesDog = true;
            string dogDescription = "";
            
            // #4 update to "rotating" animation with countdown
            string[] searchingIcons = {"| ", "/ ", "--", "\\ "};

            // I want the loading animation cycle to last a set amound of time
            int amountOfIconsToAnimate = searchingIcons.Length;
            int animationCountdown = 3;
            float animationTimeInSeconds = 1.0f;
            float threadSleepTime = ( (float)secondInMiliseconds * animationTimeInSeconds ) / amountOfIconsToAnimate;

            // loop ourAnimals array to search for matching animals
            for (int i = 0; i < maxPets; i++)
            {
                bool dogMatched = false; 

                if (ourAnimals[i, 1].Contains("dog"))
                {
                    
                    // Search combined descriptions and report results
                    dogDescription = ourAnimals[i, 4] + "\r\n" + ourAnimals[i, 5];
                    foreach (string searchWord in dogSearches)
                    {
                        // counter for loading icon
                        for (int j = animationCountdown; j > -1 ; j--)
                        {
                        // #5 update "searching" message to show countdown 
                            foreach (string icon in searchingIcons)
                            {
                                Console.Write($"\rsearching our dog {ourAnimals[i, 3]} for {searchWord} {icon} {j}");
                                Thread.Sleep(Convert.ToInt32(threadSleepTime));
                            }
                            
                            Console.Write($"\r{new String(' ', Console.BufferWidth)}");
                        }
                        
                        // #3a iterate submitted characteristic terms and search description for each term
                        
                        if (dogDescription.Contains(searchWord))
                        {
                            // #3b update message to reflect term 
                            // #3c set a flag "this dog" is a match
                            Console.WriteLine($"\rOur dog {ourAnimals[i, 3].Substring(10)} is a match for {searchWord}!");

                            dogMatched = true;
                            noMatchesDog = false;
                        }
                    }
                        // #3d if "this dog" is match write match message + dog description
                        if (dogMatched)
                        {
                            Console.WriteLine("\n{0} ({1})", ourAnimals[i, 3], ourAnimals[i, 0]);
                            Console.WriteLine(dogDescription);
                            Console.WriteLine();
                        }
                }
            }

            if (noMatchesDog)
            {
                Console.WriteLine("\rNone of our dogs are a match found for: " + dogCharacteristic);
            }

            Console.WriteLine("\n\rPress the Enter key to continue");
            readResult = Console.ReadLine();

            break;

        default:
            break;
    }

} while (menuSelection != "exit");
