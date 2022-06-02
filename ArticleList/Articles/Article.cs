using System;
using System.Collections.Generic;

namespace ArticleList.Articles
{
    public class Article
    {
        public string Title { get; set; }
        public HashSet<string> Authors { get; set; }
        public DateTime Published { get; set; }
        public DateTime LastUpdated { get; set; }
        public uint Likes { get; set; }
        public uint Dislikes { get; set; }
        public HashSet<string> Tags { get; set; }

        public override string ToString()
        {
            return "Article[" + Environment.NewLine +
                   $"\tTitle: {Title}" + Environment.NewLine +
                   $"\tAuthors: {{{string.Join(", ", Authors)}}}" + Environment.NewLine +
                   $"\tPublished: {Published:dd.MM.yyyy HH:mm:ss}" + Environment.NewLine +
                   $"\tLastUpdated: {LastUpdated:dd.MM.yyyy HH:mm:ss}" + Environment.NewLine +
                   $"\tLikes: {Likes}" + Environment.NewLine +
                   $"\tDislikes: {Dislikes}" + Environment.NewLine +
                   $"\tTags: {{{string.Join(", ", Tags)}}}" + Environment.NewLine +
                   "]";
        }
    }
}