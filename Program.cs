using System;
using Simbirsoft.Parser;
using Simbirsoft.CounterWords;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace Simbirsoft
{
    class Program
    {
        static void Main(string[] args)
        {
            ParseHtml parse = new ParseHtml();

            Console.Write("Введите URL: ");
            var url = Console.ReadLine();

            Console.WriteLine();

            Console.Write("Введите кол-во вхождений слова: ");
            var countText = Console.ReadLine();

            try
            {
                if (int.TryParse(countText, out int count))
                {
                    //"https://www.simbirsoft.com/"
                    var streamHtml = parse.GetHtmlStream(url);

                    var parsedHtml = parse.ParseHtmlOfStream(streamHtml);

                    UniqueWords uniqueWords = new UniqueWords();

                    var clearArray = uniqueWords.ClearArrayWithWord(parsedHtml);

                    var dictionaryUniqueWord = uniqueWords.CountUniqueWord(clearArray);

                    foreach (var keyValue in dictionaryUniqueWord)
                    {
                        if(keyValue.Value >= count)
                            Console.WriteLine($"{keyValue.Key} - {keyValue.Value}");
                    }
                }
                else
                    throw new Exception("Некорректное число");
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("\nНажмите любую кнопку...");
                Console.ReadKey();
                Process.Start(Assembly.GetExecutingAssembly().Location);
                Environment.Exit(0);
            }
        }
    }
}