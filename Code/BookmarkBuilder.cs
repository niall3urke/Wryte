namespace Wryte.Code
{
    public class BookmarkBuilder
    {

        // Fields

        private List<BookmarkItem> _bookmarks;

        // Constructors

        public BookmarkBuilder()
        {
            _bookmarks = new List<BookmarkItem>();
        }

        // Methods

        public void Add(string name, string href)
        {
            string compiledHref = "";
            
            _bookmarks.ForEach(x => compiledHref += $"{x.Href}/");

            href = compiledHref + href;

            _bookmarks.Add(new BookmarkItem(name, href));
        }

        public List<BookmarkItem> Get() => _bookmarks;
    }

    public record BookmarkItem(string Name, string Href);

}
