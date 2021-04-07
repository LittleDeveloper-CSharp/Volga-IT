using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Simbirsoft
{
    class Program
    {
        static void Main(string[] args)
        {
            char[] arr_ru = new char[] { 'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ь', 'ы', 'ъ', 'э', 'ю', 'я' };
            var url = new Uri("https://www.simbirsoft.com/");
            string fileName = "html.txt";
            new WebClient().DownloadFile(url, fileName);
            var item = new Dictionary<string, int>();
            
            using(StreamReader reader = new StreamReader(fileName))
            {
                string[] file = reader.ReadToEnd().Split(new char[] { ',', '.', '!', '?', '"', ';', ':', '[', ']', '(', ')', '\n', '\r', '\t' }); 
                foreach(var i in file)
                {
                    var lower = string.Concat(i.Where(x=>arr_ru.Contains(char.ToLower(x))).Select(x => char.ToLower(x)));

                    if (item.ContainsKey(lower))
                        item[lower]++;
                    else
                        item.Add(lower, 1);

                }
                StreamWriter writer = new StreamWriter("test.txt");
                foreach (var a in item)
                {
                    writer.Write($"{a.Key} - {a.Value}\n");
                }
                
            }
        }
    }
}
