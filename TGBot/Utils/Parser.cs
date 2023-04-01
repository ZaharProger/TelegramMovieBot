using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TGBot.Utils
{
    public class Parser
    {
        private const string DELIMITER = "\n\n";

        public async Task<string> Parse(int batchSize, List<string> rawTags)
        {
            var fileNames = new string[] { "TagCodes_MovieLens", "TagScores_MovieLens" };

            var data = new StringBuilder("");

            foreach (var fileName in fileNames)
            {
                using var reader = new StreamReader(@$"C:\Users\domol_000\Desktop\Мое\ml-latest\{fileName}.csv");
                var line = "";
                var isHeader = true;

                while ((line = await reader.ReadLineAsync()) != null && batchSize > 0)
                {
                    if (!isHeader)
                    {
                        data.AppendLine(line).Append(DELIMITER);
                        --batchSize;
                    }

                    isHeader = false;
                }
            }

            return data.ToString();
        }
    }
}
