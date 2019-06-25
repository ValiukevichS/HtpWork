using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dz3
{
    class MainClass
    {
        static char[] WordsSplit = ",!?';:.-“„»«„”“”()	=*_][{}# \"\a\b\r\f\n\t\v //|~+<>".ToArray();
        static char[] SentencesSplit = "!?.\n".ToArray();

        public static void Main(string[] args)
        {
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            Console.WriteLine($"Название папки где должены лежать файлы: {startupPath}");
            Console.Write("Введите имя файла для чтения: ");
            var fileName = Console.ReadLine();

            var alltext = GetTextFromFile(fileName);

            if(alltext == null)
            {
                Console.Write("Файл не найден");
            }

            Console.Write("Введите имя файла для записи слов: ");
            fileName = Console.ReadLine();

            WriteWordsToFile(fileName, alltext);

            Console.Write("Введите имя файла для записи длинного предложения и самую повторяющейся букву: ");
            fileName = Console.ReadLine();

            WriteAnalyticToFile(fileName, alltext);


            Console.Write("Файлы записаны, проверяйте их");

            Console.ReadLine();
        }

        private static void WriteAnalyticToFile(string fileName, string text)
        {
            fileName = CheckFileName(fileName);

            var file = new FileInfo(fileName);

            if (!file.Exists)
            {
                file.Create();
            }

            var topSentences = GetTopSentences(text);

            topSentences.Add(GetTopChar(text));

            using (Stream stream = file.OpenWrite())
            using (StreamWriter sr = new StreamWriter(stream, Encoding.UTF8))
            {
                foreach (var str in topSentences)
                {
                    sr.WriteLine(str);
                }
            }
        }

        private static string GetTopChar(string text)
        {
            Dictionary<char, int> directory = new Dictionary<char, int>();

            foreach (var charOne in text.ToArray())
            {
                if (char.IsControl(charOne)
                    || char.IsPunctuation(charOne)
                    || char.IsSeparator(charOne)
                    || char.IsWhiteSpace(charOne))
                    continue;
                
                if (directory.ContainsKey(char.ToLower(charOne)))
                {
                    directory[char.ToLower(charOne)]++;
                }
                else
                {
                    directory.Add(char.ToLower(charOne), 1);
                }
            }

            var topChar = directory.OrderByDescending(x => x.Value).First();

            return $"символ '{topChar.Key}' повторяется {topChar.Value} раз";
        }

        private static List<string> GetTopSentences(string text)
        {
            var sentences = text.Split(SentencesSplit).Select(x => x.Trim(WordsSplit));

            int countWords = -1;
            string topWords = string.Empty;

            int countChars = -1;
            string topChars = string.Empty;

            foreach (var sentence in sentences)
            {
                var count = sentence.Split(WordsSplit).Where(x=> !(string.IsNullOrEmpty(x) || string.IsNullOrWhiteSpace(x))).Count();
                if (countWords < count)
                {
                    topWords = sentence;
                    countWords = count;
                }

                count = sentence.ToArray().Length;
                if(countChars < count)
                {
                    topChars = sentence;
                    countChars = count;
                }
            }

            var list = new List<string>(2);

            list.Add($"{topWords} => количество слов {countWords}");
            list.Add($"{topChars} => количество символов {countChars}");

            return list;
        }

        private static void WriteWordsToFile(string fileName, string text)
        {
            fileName = CheckFileName(fileName);

            var file = new FileInfo(fileName);

            if (!file.Exists)
            {
                file.Create();
            }

            var orderingWords = GetAllWords(text).OrderBy(x => x.Key).ToArray();

            using (Stream stream = file.OpenWrite())
            using (StreamWriter sr = new StreamWriter(stream, Encoding.UTF8))
            {
                foreach (var wordKey in orderingWords)
                {
                    sr.WriteLine($"{wordKey.Key} {wordKey.Value}");
                }
            }
        }

        private static Dictionary<string, int> GetAllWords(string text)
        {
            var words = text.Split(WordsSplit).Select(x=>x.Trim(WordsSplit));

            Dictionary<string, int> directory = new Dictionary<string, int>();

            foreach (var word in words)
            {
                if (string.IsNullOrEmpty(word) || string.IsNullOrWhiteSpace(word))
                    continue;

                if (directory.ContainsKey(word))
                {
                    directory[word]++;
                }
                else
                {
                    directory.Add(word, 1);
                }
            }
            return directory;
        }

        private static string GetTextFromFile(string fileName)
        {
            string text = null;

            fileName = CheckFileName(fileName);

            var file = new FileInfo(fileName);

            if (file.Exists)
            {
                using (Stream stream = file.OpenRead())
                using (StreamReader sr = new StreamReader(stream))
                {
                    text = sr.ReadToEnd();
                }
            }

            return text;
        }

        private static string CheckFileName(string fileName)
        {
            if (!fileName.Contains("."))
            {
                fileName += ".txt";
            }
            return fileName;
        }
    }
}
