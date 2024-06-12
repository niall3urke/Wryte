using System;
using System.Collections.Generic;
using System.Text;

namespace Wryte.Models
{
    public class ItemTagModel
    {

        // Properties

        public Guid ItemId { get; set; }

        public Guid TagId { get; set; }

        public Guid Id { get; set; }

        // Constructors

        public ItemTagModel()
        {
            Id = Guid.NewGuid();
        }


    }
}
