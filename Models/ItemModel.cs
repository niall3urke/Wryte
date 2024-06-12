namespace Wryte.Models
{
    public interface INovelItem
    {
        DateTime Created { get; set; }

        DateTime Updated { get; set; }

        bool Archived { get; set; }

        string Name { get; set; }

        int Index { get; set; }

        Guid Id { get; set; }
    }

    public class ItemModel : INovelItem
    {

        // Properties

        public DateTime OccursAt { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public Guid ParentId { get; set; }

        public Guid Id { get; set; }


        public bool Archived { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        public string Html { get; set; }

        public int Index { get; set; }

        // Constructors 

        public ItemModel()
        {
            OccursAt = DateTime.Now;
            Created = DateTime.Now;
            Updated = DateTime.Now;
            Id = Guid.NewGuid();
        }

        public ItemModel Clone()
        {
            return new ItemModel
            {
                Name = "Copy of " + Name,
                ParentId = ParentId,
                Index = Index,
                Text = Text,
                Html = Html,
            };
        }

    }
}
