using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace BasicProgrammingDay4.Exercises
{
    internal class Hangman
    {
        public static int numberOfGuesses = 7;
        public static char[] possibleLetters = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'æ', 'ø', 'å'];
        public static void RunGame()
        {
            string secretWord = SetupGame();
            bool isPlayerWinning = IsPlayerWinningGamePhase(secretWord);
            if (isPlayerWinning)
            {
                Console.WriteLine("Tillykke, du vandt!!");
            }
            else
            {
                Console.WriteLine("Øv, du tabte!!");
            }
        }
        public static string SetupGame()
        {
            string secretWord = "";
            bool isSecretWordUseableForProgram = false;

            while (!isSecretWordUseableForProgram)
            {
                Console.Clear();
                Console.Write("Til game master: Vælg et hemmeligt ord: ");
                secretWord = Console.ReadLine().ToLower();
                if (string.IsNullOrEmpty(secretWord))
                {
                    Console.WriteLine("Det skal være et rigtigt ord!!");
                    Console.ReadKey();
                    continue;
                }
                isSecretWordUseableForProgram = isSecretWordUseable(secretWord);
            }
            return secretWord;
        }
        public static bool isSecretWordUseable(string secretWord)
        {
            bool isSecretWordUseable = true;
            foreach (char letter in secretWord)
            {
                if (!possibleLetters.Contains(letter))
                {
                    isSecretWordUseable = false;
                    Console.WriteLine("Det skal være et rigtigt ord!!");
                    Console.ReadKey();
                    break;
                }
            }
            return isSecretWordUseable;
        }
        public static bool isLetterUseable(char guessedChar)
        {
            bool isGuessedUseable = true;
            if (!possibleLetters.Contains(guessedChar))
            {
                isGuessedUseable = false;
            }
            return isGuessedUseable;
        }

        public static bool IsPlayerWinningGamePhase(string secretWord)
        {
            List <char> lettersGuessed = new List <char>();
            lettersGuessed = ['-', '-', '>', ' '];
            int numberOfGuessesLeft = numberOfGuesses;
            bool isNoGuessesLeft = false;
            bool isWinning = false;
            bool isPlayerWantingToGuessALetter;
            while (!isWinning && !isNoGuessesLeft)
            {
                isPlayerWantingToGuessALetter = IsPlayerWantingToGuessALetter(numberOfGuessesLeft, lettersGuessed, secretWord);

                if (isPlayerWantingToGuessALetter)
                {
                    char letterGuess = GetPlayersGuessingLetter(numberOfGuessesLeft, lettersGuessed, secretWord);
                    lettersGuessed.Add(letterGuess);
                    if (!secretWord.Contains(letterGuess))
                    {
                        Console.WriteLine($"Desværre! Det hemmelige ord indeholder ikke {letterGuess.ToString().ToUpper()}.");
                        Console.ReadKey();
                        numberOfGuessesLeft--;
                    }
                    else
                    {
                        Console.WriteLine($"Tillykke! Det hemmelige ord indeholder {letterGuess.ToString().ToUpper()}.");
                        Console.ReadKey();
                    }
                }
                else
                {
                    isWinning = IsSecretWordGuessed(GetPlayersGuessingWord(numberOfGuessesLeft, lettersGuessed, secretWord), secretWord);
                    if (isWinning)
                    {
                        break;
                    }
                    Console.WriteLine("Det var desværre forkert. Prøv igen!!");
                    Console.ReadKey();
                    numberOfGuessesLeft--;
                }

                foreach (char letter in secretWord)
                {
                    if (!lettersGuessed.Contains(letter))
                    {
                        isWinning = false;
                        break;
                    }
                    isWinning = true;
                }
                if (numberOfGuessesLeft == 0)
                {
                    isNoGuessesLeft = true;
                }
            }
            return isWinning;
        }
        public static void GuessingPhase()
        {

        }
        public static bool IsPlayerWantingToGuessALetter(int numberOfGuessesLeft, List<char> lettersGuessed, string secretWord)
        {
            bool isPlayerWantingToGuessALetter = false;
            string userInput;
            while (true)
            {
                Console.Clear();
                PlayerOverview(numberOfGuessesLeft, lettersGuessed, secretWord);
                Console.Write("Du skal indtaste [B] for at vælge bogstav eller [H] for gæt hemmeligt ord?: ");
                userInput = Console.ReadLine().ToLower();
                if (userInput != "b" && userInput != "h")
                {
                    Console.WriteLine("Du skal indtaste [B] eller [H]. Prøv igen!!");
                    Console.ReadKey();
                    continue;
                }
                break;
            }
            if (userInput == "b")
            {
                isPlayerWantingToGuessALetter = true;
                return isPlayerWantingToGuessALetter;
            }
            return isPlayerWantingToGuessALetter;
        }
        public static void PlayerOverview(int numberOfGuessesLeft, List<char> lettersGuessed, string secretWord)
        {
            Console.WriteLine($"Gæt tilbage: {numberOfGuessesLeft}");
            Console.WriteLine();
            Console.Write("Bogstaver gættet på: ");
            Console.WriteLine();
            Console.WriteLine(" ");
            foreach (char letter in lettersGuessed)
            {
                Console.Write(letter.ToString().ToUpper() + " ");
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("Hemmeligt ord: ");
            for (int i = 0; i < secretWord.Length; i++)
            {
                if (lettersGuessed.Contains(secretWord[i]))
                {
                    Console.Write(secretWord[i].ToString().ToUpper() + " ");
                    continue;
                }
                Console.Write("_ ");
            }
            Console.WriteLine();
            Console.WriteLine();
        }
        public static char GetPlayersGuessingLetter(int numberOfGuessesLeft, List<char> lettersGuessed, string secretWord)
        {
            char guessedLetter;
            string stringGuessedLetter;
            while (true)
            {
                Console.Clear();
                PlayerOverview(numberOfGuessesLeft, lettersGuessed, secretWord);
                Console.Write("Gæt på et bogstav: ");
                stringGuessedLetter = Console.ReadLine().ToLower();
                if (!char.TryParse(stringGuessedLetter, out guessedLetter))
                {
                    Console.WriteLine("Det må kun være en karakter. Prøv igen!!");
                    Console.ReadKey();
                    continue;
                }
                if (!possibleLetters.Contains(guessedLetter))
                {
                    Console.WriteLine("Dette er ikke et bogstav. Prøv igen!!");
                    Console.ReadKey();
                    continue;
                }
                if (lettersGuessed.Contains(guessedLetter))
                {
                    Console.WriteLine("Dette bogstav er allerede gættet. Prøv igen!!");
                    Console.ReadKey();
                    continue;
                }
                break;
            }
            return guessedLetter;
        }
        public static string GetPlayersGuessingWord(int numberOfGuessesLeft, List<char> lettersGuessed, string secretWord)
        {
            Console.Clear();
            PlayerOverview(numberOfGuessesLeft, lettersGuessed, secretWord);
            Console.Write("Gæt på det hemmelige ord: ");
            string GuessingWord = Console.ReadLine().ToLower();
            return GuessingWord;
        }
        public static bool IsSecretWordGuessed(string guessingWord, string secretWord)
        {
            bool isSecretWordGuessed = false;
            if (guessingWord == secretWord)
            {
                isSecretWordGuessed = true;
            }
            return isSecretWordGuessed;
        }
    }
}