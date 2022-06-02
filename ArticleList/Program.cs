using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using ArticleList.Articles;

namespace ArticleList
{
    internal class Program
    {
        private static readonly Random Random = new Random();
        
        public static void Main()
        {
            var articles = CreateDefaultArticles();
            Console.WriteLine("Default Generated Articles:");
            Console.WriteLine(string.Join(", " + Environment.NewLine, articles.Select(article => article.ToString())));
            Console.WriteLine();

            var someDate = DateTime.ParseExact("01.01.2000", "dd.MM.yyyy", CultureInfo.InvariantCulture);

            var articlesByAuthor = FilterAuthor("author1", articles);
            var articlesByTag = FilterTag("tag3", articles);
            var articleBefore = FilterPublishedBefore(someDate, articles);
            var articleAfter = FilterPublishedAfter(someDate, articles);
            var articleDuringWeekend = FilterPublishedOverTheWeekend(articles);
            var articleUpdateBefore = FilterUpdatedBefore(someDate, articles);
            var articleUpdateAfter = FilterUpdatedAfter(someDate, articles);
            var articleUpdateBeforeByAuthor = FilterUpdatedBefore(someDate, articlesByAuthor);
            
            var sortByPublishDate = SortArticlesBy(articles, (article1, article2) => DateTime.Compare(article1.Published, article2.Published) > 0);
            var sortedByLikes = SortArticlesBy(articles, (article1, article2) => article1.Likes > article2.Likes);
            var sortedByDislikes = SortArticlesBy(articles, (article1, article2) => article1.Dislikes > article2.Dislikes);
            var authorsSortedByNumberOfArticles = GetAuthorsSortedByMostArticles(articles);

            
            Console.WriteLine("Articles by author 'author1':");
            Console.WriteLine(string.Join(", " + Environment.NewLine, articlesByAuthor.Select(article => article.ToString())));
            Console.WriteLine();
            Thread.Sleep(10);
            Console.WriteLine("Articles by tag 'tag3':");
            Console.WriteLine(string.Join(", " + Environment.NewLine, articlesByTag.Select(article => article.ToString())));
            Console.WriteLine();
            Thread.Sleep(10);
            Console.WriteLine("Articles created before01.01.2000:");
            Console.WriteLine(string.Join(", " + Environment.NewLine, articleBefore.Select(article => article.ToString())));
            Console.WriteLine();
            Thread.Sleep(10);
            Console.WriteLine("Articles creaded after 01.01.2000:");
            Console.WriteLine(string.Join(", " + Environment.NewLine, articleAfter.Select(article => article.ToString())));
            Console.WriteLine();
            Thread.Sleep(10);
            Console.WriteLine("Articles during weekend:");
            Console.WriteLine(string.Join(", " + Environment.NewLine, articleDuringWeekend.Select(article => article.ToString())));
            Console.WriteLine();
            Thread.Sleep(10);
            Console.WriteLine("Articles updated before 01.01.2000:");
            Console.WriteLine(string.Join(", " + Environment.NewLine, articleUpdateBefore.Select(article => article.ToString())));
            Console.WriteLine();
            Thread.Sleep(10);
            Console.WriteLine("Articles updated after 01.01.2000:");
            Console.WriteLine(string.Join(", " + Environment.NewLine, articleUpdateAfter.Select(article => article.ToString())));
            Console.WriteLine();
            Thread.Sleep(10);
            Console.WriteLine("Articles updated before 01.01.2000 by author 'author1':");
            Console.WriteLine(string.Join(", " + Environment.NewLine, articleUpdateBeforeByAuthor.Select(article => article.ToString())));
            Console.WriteLine();
            Thread.Sleep(10);
            Console.WriteLine("Articles by publish date:");
            Console.WriteLine(string.Join(", " + Environment.NewLine, sortByPublishDate.Select(article => article.ToString())));
            Console.WriteLine();
            Thread.Sleep(10);
            Console.WriteLine("Articles by likes:");
            Console.WriteLine(string.Join(", " + Environment.NewLine, sortedByLikes.Select(article => article.ToString())));
            Console.WriteLine();
            Thread.Sleep(10);
            Console.WriteLine("Articles by dislikes:");
            Console.WriteLine(string.Join(", " + Environment.NewLine, sortedByDislikes.Select(article => article.ToString())));
            Console.WriteLine();
            Thread.Sleep(10);
            Console.WriteLine("Authors by number of articles:");
            Console.WriteLine(string.Join(", ",
                authorsSortedByNumberOfArticles.Select(author =>
                    author + ": " + FilterAuthor(author, articles).Length)));
            Console.WriteLine();
            Thread.Sleep(10);
            Console.WriteLine("Number of articles between 01.01.1980 and 01.01.2000: " + 
                              FilterPublishedAfter(
                                  new DateTime(1980, 1, 1),
                                  FilterPublishedBefore(new DateTime(2000, 1, 1), articles)).Length);
        }

        private static Article[] FilterAuthor(string author, IEnumerable<Article> articles)
        {
            return articles.Where(article => article.Authors.Contains(author)).ToArray();
        }
        
        private static Article[] FilterTag(string tag, IEnumerable<Article> articles)
        {
            return articles.Where(article => article.Tags.Contains(tag)).ToArray();
        }     
        
        private static Article[] FilterPublishedBefore(DateTime before, IEnumerable<Article> articles)
        {
            return articles.Where(article => DateTime.Compare(article.Published, before) < 0).ToArray();
        }
        
        
        private static Article[] FilterPublishedAfter(DateTime after, IEnumerable<Article> articles)
        {
            return articles.Where(article => DateTime.Compare(article.Published, after) > 0).ToArray();
        }
        
        private static Article[] FilterPublishedOverTheWeekend(IEnumerable<Article> articles)
        {
            return articles.Where(article => article.Published.DayOfWeek == DayOfWeek.Saturday || article.Published.DayOfWeek == DayOfWeek.Sunday).ToArray();
        }
        
        private static Article[] FilterUpdatedBefore(DateTime before, IEnumerable<Article> articles)
        {
            return articles.Where(article => DateTime.Compare(article.LastUpdated, before) < 0).ToArray();
        }
        
        private static Article[] FilterUpdatedAfter(DateTime after, IEnumerable<Article> articles)
        {
            return articles.Where(article => DateTime.Compare(article.LastUpdated, after) > 0).ToArray();
        }

        private static string[] GetAuthorsSortedByMostArticles(Article[] articles)
        {
            var authors = articles.SelectMany(article => article.Authors).ToHashSet().ToArray();

            return authors.Length == 0
                ? Array.Empty<string>()
                : authors.OrderByDescending(author => FilterAuthor(author, articles).Length).ToArray();
        }

        private static Article[] SortArticlesBy(Article[] articles, Func<Article, Article, bool> func)
        {
            var articlesCopy = new Article[articles.Length];
            Array.Copy(articles, articlesCopy, articles.Length);
            
            for (var i = 1; i < articlesCopy.Length; i++)
            {
                var element = articlesCopy[i];
                var j = i - 1;
                
                while (j >= 0 && func(articlesCopy[j], element))
                {
                    articlesCopy[j + 1] = articlesCopy[j];
                    j--;
                }

                articlesCopy[j + 1] = element;
            }

            return articlesCopy;
        }

        private static Article[] CreateDefaultArticles()
        {
            var amount = Random.Next(3, 6);
            var articles = new Article[amount];
            
            for (var i = 0; i < amount; i++)
            {
                var published = GenerateRandomPublishDate();
                
                var article = new Article
                {
                    Title = GenerateRandomArticleTitle(),
                    Authors = GenerateRandomAuthors(),
                    Published = published,
                    LastUpdated = GenerateLastUpdatedDate(published),
                    Likes = GenerateRandomLikesAmount(),
                    Dislikes = GenerateRandomDislikesAmount(),
                    Tags = GenerateRandomTags()
                };
                articles[i] = article;
            }

            return articles;
        }

        private static HashSet<string> GenerateRandomTags()
        {
            var tags = new List<string> { "tag1", "tag2", "tag3", "tag4", "tag5", "tag6", "tag7", "tag8", "tag9", "tag10" };
            var shuffledTags = Shuffle(tags);
            
            return shuffledTags.Take(Random.Next(shuffledTags.Count + 1)).ToHashSet();
        }

        private static uint GenerateRandomDislikesAmount()
        {
            return (uint) Random.Next(1000);
        }

        private static uint GenerateRandomLikesAmount()
        {
            return (uint) Random.Next(1000);
        }

        private static DateTime GenerateLastUpdatedDate(DateTime from)
        {
            var to = DateTime.Now;
            var range = to - from;

            var randTimeSpan = new TimeSpan((long)(Random.NextDouble() * range.Ticks)); 

            return from + randTimeSpan;
        }

        private static DateTime GenerateRandomPublishDate()
        {
            var from = DateTime.ParseExact("01.01.1700", "dd.MM.yyyy", CultureInfo.InvariantCulture);
            var to = DateTime.Now - TimeSpan.FromDays(2);
            var range = to - from;

            var randTimeSpan = new TimeSpan((long)(Random.NextDouble() * range.Ticks)); 

            return from + randTimeSpan;
        }

        private static string GenerateRandomArticleTitle()
        {
            var titles = new List<string> { "title1", "title2", "title3", "title4", "title5", "title6", "title7", "title8", "title9", "title10" };
            
            return titles[Random.Next(0, titles.Count - 1)];
        }

        private static HashSet<string> GenerateRandomAuthors()
        {
            var authors = new List<string> { "author1", "author2", "author3", "author4", "author5", "author6", "author7", "author8", "author9", "author10" };
            var shuffledAuthors = Shuffle(authors);
            
            return shuffledAuthors.Take(Random.Next(1, shuffledAuthors.Count + 1)).ToHashSet();
        }

        private static List<T> Shuffle<T>(IEnumerable<T> set)
        {
            var list = set.ToList();
            var n = list.Count;  
            while (n > 1) {  
                n--;  
                var k = Random.Next(n + 1);  
                (list[k], list[n]) = (list[n], list[k]);
            }

            return list;
        }

        // Change the Console Output to a file
        private static void SetOutputToFile()
        {
            StreamWriter writer;
            try
            {
                writer = new StreamWriter(new FileStream ("./output.txt", FileMode.OpenOrCreate, FileAccess.Write));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            Console.SetOut(writer);
        }
    }
}