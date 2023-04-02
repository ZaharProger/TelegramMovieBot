using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using TGBot.Models;
using System;

namespace TGBot.Utils
{
    public class Parser
    {
        private static readonly string path = @"C:\Users\domol_000\Desktop\Мое\ml-latest\";

        private static readonly string[] fileNames = new string[] { "TagCodes_MovieLens", "TagScores_MovieLens", "links_IMDB_MovieLens", "MovieCodes_IMDB" };

        public static async Task<List<Movie>> Parse(List<string> rawTags)
        {
            List<Tag>  tags = new List<Tag>();
            List<Movie> movie = new List<Movie>();

            var data = new StringBuilder("");

            using var readerTag = new StreamReader($"{path}{fileNames[0]}.csv");
            using var readerTagScore = new StreamReader($"{path}{fileNames[1]}.csv");
            using var readerLinks = new StreamReader($"{path}{fileNames[2]}.csv");
            using var readerMovie = new StreamReader($"{path}{fileNames[3]}.tsv");

            string regTagCodes = @"(?'id'\d+).(?'tag'" + rawTags[0];

            var tagline = "";
            var tagScoreline = "";
            var linkline = "";
            var movieline = "";
            var firstMatch = false;

            if (rawTags.Count > 1)
            {
                foreach (var tag in rawTags)
                {
                    regTagCodes += @"|" + tag;
                }
            }
                
            regTagCodes += @")";

            while ((tagline = await readerTag.ReadLineAsync()) != null)
            {
                Match parseTagLine = Regex.Match(tagline, regTagCodes);

                if (parseTagLine.Success)
                {
                    string regTagScore = @"(?'movieId'\d+),(?'tagId'" + parseTagLine.Groups[1] + @"),(?'relevant'\d.\d+)";

                    while ((tagScoreline = await readerTagScore.ReadLineAsync()) != null && !firstMatch)
                    {
                        firstMatch = false;
                        Match parseTagScoreLine = Regex.Match(tagScoreline, regTagScore);

                        if (parseTagScoreLine.Success && (double)Convert.ToDouble(parseTagScoreLine.Groups[3].ToString().Replace(".", ",")) > 0.5)
                        {
                            tags.Add(new Tag(Convert.ToInt32(parseTagLine.Groups[1].ToString()), parseTagLine.Groups[2].ToString(), Convert.ToDouble(parseTagScoreLine.Groups[3].ToString().Replace(".", ","))));
                            string regLink = @"(?'movieId'" + parseTagScoreLine.Groups[1] + @"),(?'imdbId'\d+),";
                            
                            while ((linkline = await readerLinks.ReadLineAsync()) != null && !firstMatch)
                            {
                                firstMatch = false;
                                Match parseLinkLine = Regex.Match(linkline, regLink);

                                if (parseLinkLine.Success)
                                {
                                    string parseLinkLineConvert = parseLinkLine.Groups[2].ToString().Length switch
                                    {
                                        1 => "000000" + parseLinkLine.Groups[2].ToString(),
                                        2 => "00000" + parseLinkLine.Groups[2].ToString(),
                                        3 => "0000" + parseLinkLine.Groups[2].ToString(),
                                        4 => "000" + parseLinkLine.Groups[2].ToString(),
                                        5 => "00" + parseLinkLine.Groups[2].ToString(),
                                        6 => "0" + parseLinkLine.Groups[2].ToString(),
                                        7 => parseLinkLine.Groups[2].ToString(),
                                    } ;

                                    string regMovie = @"(?'titleId'tt" + parseLinkLineConvert + @").\d+.(?'title'.*?).(?'region'RU|US|GB)";
                                    
                                    while ((movieline = await readerMovie.ReadLineAsync()) != null && !firstMatch)
                                    {
                                        firstMatch = false;

                                        Match parseMovieLine = Regex.Match(movieline, regMovie);
                                        if (parseMovieLine.Success)
                                        {
                                            movie.Add(new Movie(parseMovieLine.Groups[1].ToString(), parseMovieLine.Groups[2].ToString(), parseMovieLine.Groups[3].ToString()));
                                            firstMatch = true;
                                        }
                                    }

                                    firstMatch = true;
                                }
                            }

                            firstMatch = true;
                        }

                    }
                } 
            }

            return movie;
        }

        public static async Task<List<string>> GetTagIdByTag(List<string> tags)
        {
            List<string> tagId = new List<string>();
            using var readerLinks = new StreamReader($"{path}{fileNames[0]}.csv");

            string regTag = @"(?'id'\d+).(?'tag'" + tags[0];
            if (tags.Count > 1)
            {
                foreach (var tag in tags)
                {
                    regTag += @"|" + tag;
                }
            }
            regTag += @")";

            var file = await readerLinks.ReadToEndAsync();

            foreach (Match match in Regex.Matches(file, regTag))
                tagId.Add(match.Groups[1].ToString());

            return tagId;
        }

        public static async Task<List<string>> GetMovieIdByTagId(List<string> tags)
        {
            List<string> movieId = new List<string>();
            using var readerLinks = new StreamReader($"{path}{fileNames[1]}.csv");
            int cycle = 50;

            string regScore = @"(?'movieId'\d+),(?'tagId'" + tags[0];

            if (tags.Count > 1)
                tags.ForEach(t => regScore += @"|" + t);

            regScore += @"),(?'relevant'\d.\d+)";

            var file = await readerLinks.ReadToEndAsync();

            foreach (Match match in Regex.Matches(file, regScore))
            {
                if (cycle < 0)
                    break;

                if ((double)Convert.ToDouble(match.Groups[3].ToString().Replace('.', ',')) > 0.5)
                {
                    movieId.Add(match.Groups[1].ToString());
                    cycle--;
                }
            }

            return movieId;
        }

        public static async Task<List<string>> GetImdbIdByMoviceId(List<string> movieId)
        {
            using var readerLinks = new StreamReader($"{path}{fileNames[2]}.csv");
            int cycle = 50;

            string regLink = @"(?'movieId'" + movieId[0] ;

            if (movieId.Count > 1)
                movieId.ForEach(t => regLink += @"|" + t);

            regLink += @").(?'imdbId'\d+),";

            var file = await readerLinks.ReadToEndAsync();

            foreach (Match match in Regex.Matches(file, regLink))
            {
                if (cycle < 0)
                    break;

                else
                    cycle--;

                string parseLinkLineConvert = match.Groups[2].ToString().Length switch
                {
                    1 => "000000" + match.Groups[2].ToString(),
                    2 => "00000" + match.Groups[2].ToString(),
                    3 => "0000" + match.Groups[2].ToString(),
                    4 => "000" + match.Groups[2].ToString(),
                    5 => "00" + match.Groups[2].ToString(),
                    6 => "0" + match.Groups[2].ToString(),
                    7 => match.Groups[2].ToString(),
                };

                movieId.Add(parseLinkLineConvert);
            }

            return movieId;
        }

        public static async Task<List<Movie>> GetMoviebByID(List<string> imdbId)
        {
            List<Movie> movieList = new List<Movie>();
            using var readerMovie = new StreamReader($"{path}{fileNames[3]}.csv");
            int cycle = 50;
            string regMovie = @"(?'titleId'tt" + imdbId[0] ;

            if (imdbId.Count > 1)
                imdbId.ForEach(t => regMovie += @"|" + t);

            regMovie += @").\d+.(?'title'.*?).(?'region'RU|US|GB)";

            var file = await readerMovie.ReadToEndAsync();

            foreach (Match match in Regex.Matches(file, regMovie))
            {
                if (cycle < 0)
                    break;

                else
                    cycle--;

                movieList.Add(new Movie(match.Groups[1].ToString(), match.Groups[2].ToString(), match.Groups[3].ToString()));
            }

            return movieList;
        }
    }
}
