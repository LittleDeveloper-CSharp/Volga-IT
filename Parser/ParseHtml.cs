using System;
using System.IO;
using System.Net;

namespace Simbirsoft.Parser
{
    class ParseHtml
    {
        public Stream GetHtmlStream(string urlText)
        {
            var url = new Uri(urlText);
            var stream = new WebClient().OpenRead(url);
            return stream;
        }

        public string[] ParseHtmlOfStream(Stream file)
        {
            //new char[] { ',', '.', '!', '?', '"', ';', ':', '[', ']', '(', ')', '\n', '\r', '\t' }
            using (StreamReader reader = new StreamReader(file))
            {
                //{ ',', '.', '!', '?', '"', ';', ':', '[', ']', '(', ')', '\n', '\r', '\t'};
                string[] fileSeparators = new string[] { "<head>", "</head>" };
                string[] parsedHtml = reader.ReadToEnd()
                    .Split(fileSeparators, StringSplitOptions.RemoveEmptyEntries)[2]
                    .Split(new string[] { "<div", "\n", "\t", "\r", " ", "\"", "</div>", "</", "«", "<", ">", "script" }, StringSplitOptions.RemoveEmptyEntries);
                return parsedHtml;
            }
        }
    }
}
