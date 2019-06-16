using System;

namespace Vocabulary_Practice
{
    class Program
    {
        static Words words = new Words("Words.csv");
        static void Main(string[] args)
        {
            Test();
        }

        static void Test()
        {
            if (words.WordsList.Count > 0)
            {
                while (true)
                {
                    Word word = words.GetNext();
                    Console.WriteLine("Type in Spanish: " + word.ToString(Word.ToStringOptions.TranslationWithNote));
                    string input = Console.ReadLine();
                    if (input.Length > 0 && (input[0] == '/' || input[0] == '-'))
                        HandleOptions(input.Substring(1));
                    else
                    {
                        if (input.Equals(word.Original, StringComparison.InvariantCultureIgnoreCase))
                        {
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.WriteLine("Correct. " + word.ToString());
                            word.AnsweredCorrectly(true);
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("Wrong. " + word.ToString());
                            Word similarWord = words.FindWord(input);
                            if (similarWord != null)
                            {
                                Console.BackgroundColor = ConsoleColor.Yellow;
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.WriteLine(similarWord);
                                similarWord.AnsweredCorrectly(false);
                            }
                            word.AnsweredCorrectly(false);
                        }
                        words.Save();
                        Console.ResetColor();
                        Console.ReadLine();
                    }
                    Console.Clear();
                }
            }
        }

        private static void HandleOptions(string option)
        {
            switch (option.ToLower())
            {
                case "add":
                    System.Diagnostics.Process.Start("Words.csv");
                    Environment.Exit(0);
                    break;
                case "learn":
                case "study":
                    Console.Clear();
                    Console.WriteLine(words.GetWordsToStudyAsString(0.7));
                    Console.ReadKey();
                    break;
                default:
                    Console.WriteLine("Available options: add or learn");
                    Console.ReadKey();
                    break;
            }
        }
    }
}