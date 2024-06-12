using System;
using System.Collections.Generic;
using System.Text;

namespace Wryte.Models.DTO
{
    public class NovelDTO
    {

        public List<ChapterDTO> Chapters { get; set; }

        public List<string> Tags { get; set; }

        public DateTime Created { get; set; }

        public string Subtitle { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }

        public Guid Id { get; set; }

        // Constructors

        public NovelDTO()
        {
            Chapters = new List<ChapterDTO>();
            Tags = new List<string>();
            Subtitle = "";
            Author = "";
            Title = "";
        }


    }
}
