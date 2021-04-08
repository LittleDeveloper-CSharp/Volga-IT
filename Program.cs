using System;
using Simbirsoft.Parser;
using Simbirsoft.CounterWords;
using System.Diagnostics;
using System.Reflection;
using Simbirsoft.Models;
using System.Linq;
using System.Data.Entity;

namespace Simbirsoft
{
    class Program
    {
        static void Main(string[] args)
        {
            ApplicationContext context = new ApplicationContext();
            context.UniqueWords.Load();

            Console.Write("Что сделать? (1-вывести из базы, 2-ввести URL): ");
            var answer = int.Parse(Console.ReadLine());

            if (answer == 1)
            {
                Console.WriteLine("\n\nСписок слов:\n");
                foreach (var word in context.UniqueWords.Local.ToList())
                    Console.WriteLine($"{word.Word} - {word.Count}");
            }
            else if(answer == 2)
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
                            if (keyValue.Value >= count)
                            {
                                Console.WriteLine($"{keyValue.Key} - {keyValue.Value}");
                                var item = context.UniqueWords.Local.FirstOrDefault(x => x.Word == keyValue.Key && x.Url == url);
                                if (item == null)
                                {
                                    var lastItem = context.UniqueWords.Local.LastOrDefault();
                                    var lastIndex = lastItem == null ? 1 : lastItem.IdWord + 1;
                                    context.UniqueWords.Add(new UniqueWord { IdWord = lastIndex, Url = url, Word = keyValue.Key, Count = keyValue.Value });
                                }
                                else
                                    item.Count += keyValue.Value;

                            }
                        }
                        context.SaveChanges();
                        Console.WriteLine("\nНажмите на любую кнопку");
                    }
                    else
                        throw new Exception("Некорректное число");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.WriteLine("\nНажмите любую кнопку...");
            Console.ReadKey();
            Process.Start(Assembly.GetExecutingAssembly().Location);
            Environment.Exit(0);
        }
    }
}