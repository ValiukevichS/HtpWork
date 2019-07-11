using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace dz4
{
    class MainClass
    {
        static char[] WordsSplit = ",!?';:.-“„»«„”“”()	=*_][{}# \"\a\b\r\f\n\t\v //|~+<>".ToArray();
        const string DictionalryPath = "../Dictionary.txt";
        const string DictionalrySplit = " ";
        const string AnalyticsPath = "../AnalyticsData.txt";

        public static void Main()
        {
            Run();
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private static void Run()
        {
            // Create a new FileSystemWatcher and set its properties.
            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {
                string startupPath = System.IO.Directory.GetCurrentDirectory();
                Console.WriteLine($"Название папки за которой следим: {startupPath}");
                Console.WriteLine($"Название файла со словарем: {DictionalryPath}");


                watcher.Path = startupPath;

                // Watch for changes in LastAccess and LastWrite times, and
                // the renaming of files or directories.
                watcher.NotifyFilter = NotifyFilters.LastAccess
                                        | NotifyFilters.LastWrite
                                        | NotifyFilters.FileName
                                        | NotifyFilters.DirectoryName;

                // Only watch text files.
                watcher.Filter = "*.txt";

                // Add event handlers.
                watcher.Changed += OnChanged;
                watcher.Created += OnChanged;
                watcher.Deleted += OnChanged;
                watcher.Renamed += OnRenamed;

                // Begin watching.
                watcher.EnableRaisingEvents = true;

                // Wait for the user to quit the program.
                Console.WriteLine("нажмите [Escape] для выхода из приложения");

                
                while (Console.ReadKey(false).Key != ConsoleKey.Escape) ;

                Console.Write(" Enter для завершения работы");

                Console.ReadLine();
            }
        }

        
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");
            if(e.ChangeType == WatcherChangeTypes.Created)
            {
                var name = e.FullPath;
                Task.Run(() => {
                    try
                    {
                        UpdateDictionary(name);
                    }
                    catch(Exception ex)
                    {
                        Console.Write($"{ex}");
                    }
                });
            }
        }

        private static void OnRenamed(object source, RenamedEventArgs e) =>
            Console.WriteLine($"File: {e.OldFullPath} renamed to {e.FullPath}");
        


        public static void UpdateDictionary(string fileNameToRead)
        {
            var alltext = GetTextFromFile(fileNameToRead);

            if (alltext == null)
            {
                Console.WriteLine("Файл не найден");
            }
            else
            {
                Console.WriteLine("Файл найден, идет обнавление словаря");
                var dictionary = ReadDictionaly();
                WriteWordsToFile(DictionalryPath, alltext, dictionary);

                Console.WriteLine("Словарь Обнавлен");
            }
        }
       
        private static void WriteWordsToFile(string fileName, string text,Dictionary<string, int> dictionaryOld)
        {
            fileName = CheckFileName(fileName);

            var file = new FileInfo(fileName);

            var orderingWords = GetAllWords(text, dictionaryOld).OrderBy(x => x.Key).ToArray();

            if(file.Exists)
            {
                file.Delete();
            }

            using (Stream stream = file.Open(FileMode.CreateNew,FileAccess.Write))
            using (StreamWriter sr = new StreamWriter(stream, Encoding.UTF8))
            {
                foreach (var wordKey in orderingWords)
                {
                    sr.WriteLine($"{wordKey.Key}{DictionalrySplit}{wordKey.Value}");
                }
            }
        }

        private static Dictionary<string, int> ReadDictionaly()
        {
            string line = null;
            int count = 0;
            string[] words = null;
            Dictionary<string, int> directory = new Dictionary<string, int>();

            var file = new FileInfo(DictionalryPath);

            if (file.Exists)
            {
                using (Stream stream = file.OpenRead())
                using (StreamReader sr = new StreamReader(stream))
                {
                    while (!sr.EndOfStream)
                    {
                        line = sr.ReadLine().Trim();
                        if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                            continue;

                        words = line.Split(DictionalrySplit.ToArray());

                        if (words.Length == 2 && int.TryParse(words[1],out count))
                        {
                            if (string.IsNullOrEmpty(words[0]) || string.IsNullOrWhiteSpace(words[0]))
                                continue;

                            if (directory.ContainsKey(words[0]))
                            {
                                directory[words[0]]+= count;
                            }
                            else
                            {
                                directory.Add(words[0], count);
                            }
                        }
                    }
                }
            }
            return directory;
        }


        private static Dictionary<string, int> GetAllWords(string text, Dictionary<string, int> dictionaryOld = null)
        {
            uint countWords = 0;
            var words = text.Split(WordsSplit).Select(x=>x.Trim(WordsSplit));

            Dictionary<string, int> directory = dictionaryOld ?? new Dictionary<string, int>();
            
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
                countWords++;
            }
            AddAnalytics(countWords);
            return directory;
        }

        private static void AddAnalytics(uint newWords)
        {
            var file = new FileInfo(AnalyticsPath);

            if(!file.Exists)
            {
                file.Create();
            }

            using (Stream stream = file.Open(FileMode.Append, FileAccess.Write))
            using (StreamWriter sr = new StreamWriter(stream, Encoding.UTF8))
            {
                sr.WriteLine($"В словарь добавлено {newWords} слов (дата: {DateTime.Now.ToString("dd/MM/yyyy H:mm")})");
            }
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
