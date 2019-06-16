using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Vocabulary_Practice
{
    class Words
    {
        public string PathToWordsFile { get; set; }
        public List<Word> WordsList { get; set; }
        public Word LastShowedWord { get; private set; }
        public enum ToStringOptions
        {
            Simple, CommaSeparated, SimpleWithSuccessRate
        }

        public Words(string pathToWordsFile)
        {
            PathToWordsFile = pathToWordsFile;
            WordsList = new List<Word>();
            Load();
        }

        public Words(IEnumerable<Word> words)
        {
            WordsList = new List<Word>(words);
        }

        public void Load()
        {
            string[] lines = File.ReadAllLines(PathToWordsFile);
            if (lines.Length == 0)
                throw new IOException("Empty file");
            int currentLineNumber = 1;
            try
            {
                foreach (string line in lines)
                {
                    string[] columns = line.Split(',');
                    string original = columns[0];
                    string translation = columns[1];
                    string note = columns.Length > 2 ? columns[2] : "";
                    uint correctCount = columns.Length > 3 ? uint.Parse(columns[3]) : 0;
                    uint incorrectCount = columns.Length > 4 ? uint.Parse(columns[4]) : 0;
                    WordsList.Add(new Word(original, translation, note, correctCount, incorrectCount));
                    currentLineNumber++;
                }
            }
            catch (Exception)
            {
                throw new FormatException("Bad input file format on line " + currentLineNumber);
            }
        }

        public void Save()
        {
            File.WriteAllText(PathToWordsFile, ToString(ToStringOptions.CommaSeparated));
        }

        public static Word GetRandomWord(Word[] words)
        {
            return words[new Random().Next(words.Length)];
        }

        public Word[] GetLeastShowedWords()
        {
            uint minTimesShowed = WordsList.Min(word => word.GetTimesShowed());
            return WordsList.Where(word => word.GetTimesShowed() == minTimesShowed).ToArray();
        }

        public Word[] GetLeastAnsweredCorrectlyWords(double threshold)
        {
            return WordsList.Where(word => word.GetAnsweredCorrectlyRate() < threshold).ToArray();
        }

        public Word GetNext()
        {
            Word word;
            if (WordsList.Count > 1)
            {
                do
                {
                    double chance = new Random().NextDouble();
                    if (chance < .1)
                        word = GetRandomWord(WordsList.ToArray());
                    else if (chance < .2)
                        word = GetRandomWord(GetLeastShowedWords());
                    else
                        word = GetRandomWord(GetLeastAnsweredCorrectlyWords(.7));
                } while (LastShowedWord == word);
            }
            else
                word = WordsList[0];
            LastShowedWord = word;
            return word;
        }

        public string GetWordsToStudyAsString(double threshold)
        {
            return new Words(GetLeastAnsweredCorrectlyWords(threshold).OrderBy(word => word.GetAnsweredCorrectlyRate())).ToString(ToStringOptions.SimpleWithSuccessRate);
        }

        public Word FindWord(string original)
        {
            return WordsList.Find(word => word.Original.Equals(original));
        }

        public override string ToString()
        {
            return ToString(ToStringOptions.Simple);
        }

        public string ToString(ToStringOptions options)
        {
            string output = "";
            foreach (Word word in WordsList)
                switch (options)
                {
                    case ToStringOptions.Simple:
                        output += word.ToString() + Environment.NewLine;
                        break;
                    case ToStringOptions.CommaSeparated:
                        output += word.ToString(Word.ToStringOptions.CommaSeparated) + Environment.NewLine;
                        break;
                    case ToStringOptions.SimpleWithSuccessRate:
                        output += word.ToString(Word.ToStringOptions.SimpleWithSuccessRate) + Environment.NewLine;
                        break;
                }
            return output;
        }
    }
}