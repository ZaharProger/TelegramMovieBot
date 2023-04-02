using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGBot
{
    public class Parser
    {
        private const string DELIMITER = "~~~";
        public async Task Parse(List<string> rawTags = null)
        {
            var fileNames = new string[] { };

            if (rawTags != null)
            {
                fileNames = new string[] { "TagCodes_MovieLens.csv", "TagScores_MovieLens.csv" };
            }

            var data = new StringBuilder("");

            foreach (var fileName in fileNames)
            {
                using var reader = new StreamReader(fileName);
                data.Append(await reader.ReadToEndAsync()).Append(DELIMITER);
            }
            
            var totalData = data.ToString()
                .Split(DELIMITER, System.StringSplitOptions.RemoveEmptyEntries)
                .ToList();
        }
    }
}
