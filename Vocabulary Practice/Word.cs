namespace Vocabulary_Practice
{
    class Word
    {
        public string Original { get; }
        public string Translation { get; }
        public string Note { get; }
        public uint TimesCorrectlyAnswered { get; private set; }
        public uint TimesIncorrectlyAnswered { get; private set; }
        public enum ToStringOptions
        {
            Simple, CommaSeparated, TranslationWithNote, SimpleWithSuccessRate
        }

        public Word(string original, string translation, string note, uint correctlyAnsweredCount, uint incorrectlyAnsweredCount)
        {
            Original = original;
            Translation = translation;
            Note = note;
            TimesCorrectlyAnswered = correctlyAnsweredCount;
            TimesIncorrectlyAnswered = incorrectlyAnsweredCount;
        }

        public uint GetTimesShowed()
        {
            return TimesCorrectlyAnswered + TimesIncorrectlyAnswered;
        }

        public double GetAnsweredCorrectlyRate()
        {
            if (GetTimesShowed() == 0)
                return 0;
            return (double)TimesCorrectlyAnswered / GetTimesShowed();
        }

        public void AnsweredCorrectly(bool isCorrect)
        {
            if (isCorrect)
                TimesCorrectlyAnswered++;
            else
                TimesIncorrectlyAnswered++;
        }

        public override string ToString()
        {
            return ToString(ToStringOptions.Simple);
        }

        public string ToString(ToStringOptions options)
        {
            string output = "";
            switch (options)
            {
                case ToStringOptions.Simple:
                    output = $"{Original} = {Translation}" + (Note.Equals("") ? "" : $" ({Note})");
                    break;
                case ToStringOptions.CommaSeparated:
                    output = $"{Original},{Translation},{Note},{TimesCorrectlyAnswered},{TimesIncorrectlyAnswered}";
                    break;
                case ToStringOptions.TranslationWithNote:
                    output = Translation + (Note.Equals("") ? "" : $" ({Note})");
                    break;
                case ToStringOptions.SimpleWithSuccessRate:
                    output = ToString() + " [" + (GetAnsweredCorrectlyRate() * 100).ToString("0") + "%]";
                    break;
            }
            return output;
        }
    }
}