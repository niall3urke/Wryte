using System;
using System.Collections.Generic;
using System.Text;

namespace Wryte.Models.DTO
{
    public class ChapterDTO
    {

        public bool Include { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public int Number { get; set; }

        // Constructors 

        public ChapterDTO()
        {
            Include = true;
        }

    }
}
