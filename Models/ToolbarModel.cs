using System;
using System.Collections.Generic;
using System.Text;

namespace Wryte.Models
{
    public class ToolbarModel
    {

        // Properties 

        public string Theme { get; set; }

        // Group 

        public bool ShowBold { get; set; }

        public bool ShowItalic { get; set; }

        public bool ShowUnderline { get; set; }

        public bool ShowStrike { get; set; }

        // Group 

        public bool ShowBlockquote { get; set; }

        public bool ShowCodeblock { get; set; }

        // Group 

        public bool ShowLink { get; set; }

        public bool ShowImage { get; set; }

        // Group

        public bool ShowHeading1 { get; set; }

        public bool ShowHeading2 { get; set; }

        // Group

        public bool ShowUnorderedList { get; set; }

        public bool ShowOrderedList { get; set; }

        // Group

        public bool ShowSuperscript { get; set; }

        public bool ShowSubscript { get; set; }

        // Group

        public bool ShowIndentForward { get; set; }

        public bool ShowIndentBackward { get; set; }

        // Group

        public bool ShowRightToLeft { get; set; }

        // Group

        public bool ShowFontSizes { get; set; }

        // Group

        public bool ShowFontColor { get; set; }

        public bool ShowFontBackgroundColor { get; set; }

        // Group

        public bool ShowFonts { get; set; }

        // Group

        public bool ShowAlignments { get; set; }

        // Group 

        public bool ShowClean { get; set; }

        // Constructors

        public ToolbarModel()
        {
            ShowBlockquote = true;
            ShowHeading2 = true;
            ShowHeading1 = true;
            ShowItalic = true;
            ShowBold = true;
            Theme = "bubble";
        }

        // Methods

        public bool CompareTo(ToolbarModel toolbar)
        {
            if (Theme != toolbar.Theme)
                return false;

            if (ShowBold != toolbar.ShowBold)
                return true;

            if (ShowItalic != toolbar.ShowItalic)
                return true;

            if (ShowUnderline != toolbar.ShowUnderline)
                return false;

            if (ShowStrike != toolbar.ShowStrike)
                return false;

            if (ShowBlockquote != toolbar.ShowBlockquote)
                return false;

            if (ShowCodeblock != toolbar.ShowCodeblock)
                return false;

            if (ShowHeading1 != toolbar.ShowHeading1)
                return false;

            if (ShowHeading2 != toolbar.ShowHeading2)
                return false;

            if (ShowUnorderedList != toolbar.ShowUnorderedList)
                return false;

            if (ShowOrderedList != toolbar.ShowOrderedList)
                return false;

            if (ShowSuperscript != toolbar.ShowSuperscript)
                return false;

            if (ShowSubscript != toolbar.ShowSubscript)
                return false;

            if (ShowIndentForward != toolbar.ShowIndentForward)
                return false;

            if (ShowIndentBackward != toolbar.ShowIndentBackward)
                return false;

            if (ShowRightToLeft != toolbar.ShowRightToLeft)
                return false;

            if (ShowFontSizes != toolbar.ShowFontSizes)
                return false;

            if (ShowFonts != toolbar.ShowFonts)
                return false;

            if (ShowFontBackgroundColor != toolbar.ShowFontBackgroundColor)
                return false;

            if (ShowFontColor != toolbar.ShowFontColor)
                return false;

            if (ShowAlignments != toolbar.ShowAlignments)
                return false;

            if (ShowClean != toolbar.ShowClean)
                return false;

            return true;
        }

        public ToolbarModel Clone()
        {
            return MemberwiseClone() as ToolbarModel;
        }


    }
}
