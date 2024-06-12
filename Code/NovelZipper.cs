using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Wryte.Models.DTO;

namespace Wryte.Code
{
    public class NovelZipper
    {

        // Constants

        private const string AppName = "Wryte";

        // Constructors 

        public NovelZipper() { }

        // Methods - public

        public string CreateZipString(NovelDTO novel, byte[] template)
        {
            CheckNovelPropertiesMeetLengthRequirements(ref novel);

            using var ms = new MemoryStream(template);

            using var zip = new MemoryStream();

            ms.CopyTo(zip);

            using (var archive = new ZipArchive(zip, ZipArchiveMode.Update, true))
            {
                // Do the files that need updating first. Then do the files
                // that need creating.
                for (int i = 0; i < archive.Entries.Count; i++)
                {
                    var entry = archive.Entries[i];

                    if (entry.Name.Contains("content.opf"))
                    {
                        GetContentOpf(ref entry, novel);
                    }

                    else if (entry.Name.Contains("toc.ncx"))
                    {
                        GetTocNcx(ref entry, novel);
                    }

                    else if (entry.Name.Contains("cover.xhtml"))
                    {
                        GetCoverXhtml(ref entry, novel);
                    }
                }

                // Do the chapters separately because they create entries in 
                // the zip, throwing out the count (causing us to potentially
                // miss files that need updating.
                for (int i = 0; i < archive.Entries.Count; i++)
                {
                    var entry = archive.Entries[i];

                    if (entry.Name.Contains("chapter.xhtml"))
                    {
                        CreateChapters(ref entry, novel);
                    }
                }
            }

            ms.Flush();
            ms.Close();

            zip.Seek(0, SeekOrigin.Begin);

            return Convert.ToBase64String(zip.ToArray());
        }

        // Methods - private

        private void CheckNovelPropertiesMeetLengthRequirements(ref NovelDTO novel)
        {
            novel.Subtitle = PadProperty("@Novel.Subtitle", novel.Subtitle);
            novel.Author = PadProperty("@Novel.Author", novel.Author);
            novel.Title = PadProperty("@Novel.Title", novel.Title);
        }

        private string PadProperty(string placeholder, string property)
        {
            // It's possible that the user doesn't input an item, or their
            // combined length is less that the length of the placeholders
            // In this case we need to add whitespace to the end of the 
            // content, otherwise we won't replace the all of the html in
            // the zip template.

            int diff = placeholder.Length - property.Length;

            if (diff > 0)
            {
                for (int i = 0; i < diff; i++)
                    property += " ";
            }
            return property;
        }

        private void GetTocNcx(ref ZipArchiveEntry entry, NovelDTO novel)
        {
            using var stream = entry.Open();
            using var sr = new StreamReader(stream);
            using var sw = new StreamWriter(stream);

            string text = sr.ReadToEnd();

            text = GetTOC(novel, text);

            stream.Seek(0, SeekOrigin.Begin);

            sw.Write(text);
        }

        private void GetContentOpf(ref ZipArchiveEntry entry, NovelDTO novel)
        {
            using var stream = entry.Open();
            using var sr = new StreamReader(stream);
            using var sw = new StreamWriter(stream);

            string text = sr.ReadToEnd();

            text = GetContent(novel, text);

            stream.Seek(0, SeekOrigin.Begin);

            sw.Write(text);
        }

        private void GetCoverXhtml(ref ZipArchiveEntry entry, NovelDTO novel)
        {
            using var stream = entry.Open();
            using var sr = new StreamReader(stream);
            using var sw = new StreamWriter(stream);

            string text = sr.ReadToEnd();

            text = GetCover(novel, text);

            stream.Seek(0, SeekOrigin.Begin);

            sw.Write(text);
        }

        private void CreateChapters(ref ZipArchiveEntry entry, NovelDTO novel)
        {
            using var stream = entry.Open();

            using var sr = new StreamReader(stream);

            string template = sr.ReadToEnd();

            for (int i = 0; i < novel.Chapters.Count; i++)
            {
                string content = CreateChapter(template, novel, novel.Chapters[i]);

                int id = i + 1;

                var chapter = entry.Archive.CreateEntry(@$"chapter{id}.xhtml");

                using (var sw = new StreamWriter(chapter.Open()))
                {
                    sw.Write(content);
                }
            }

            stream.Close();
            entry.Delete();
        }

        private string GetCover(NovelDTO novel, string content)
        {
            content = content.Replace("@Novel.Subtitle", novel.Subtitle);
            content = content.Replace("@Novel.Author", novel.Author);
            content = content.Replace("@Novel.Title", novel.Title);

            return content;
        }

        private string GetContent(NovelDTO novel, string content)
        {
            string tags = string.Join(", ", novel.Tags);

            string chapterItems = "";

            string spineItems = "";

            for (int i = 0; i < novel.Chapters.Count; i++)
            {
                int id = i + 1;
                chapterItems += $"<item href=\"chapter{id}.xhtml\" id=\"chapter{id}\" media-type=\"application/xhtml+xml\" />";
            }

            for (int i = 0; i < novel.Chapters.Count; i++)
            {
                int id = i + 1;
                spineItems += $"<itemref idref=\"chapter{id}\" />";
            }

            content = content.Replace("@Novel.Published", DateTime.Now.ToString("s"));
            content = content.Replace("@Novel.Created", novel.Created.ToString("s"));
            content = content.Replace("@ChapterItems", chapterItems);
            content = content.Replace("@Novel.Author", novel.Author);
            content = content.Replace("@Novel.Title", novel.Title);
            content = content.Replace("@Novel.Id", $"{novel.Id}");
            content = content.Replace("@SpineItems", spineItems);
            content = content.Replace("@Novel.Tags", tags);
            content = content.Replace("@AppName", AppName);

            return content;
        }

        private string CreateChapter(string template, NovelDTO novel, ChapterDTO chapter)
        {
            string content = template;

            content = content.Replace("@Novel.Title", novel.Title);
            content = content.Replace("@Chapter.Text", chapter.Text);
            content = content.Replace("@Chapter.Title", chapter.Title);
            content = content.Replace("@Chapter.Number", $"{chapter.Number}");
            content = content.Replace("<br>", "<br/>");

            return content;
        }

        private string GetTOC(NovelDTO novel, string content)
        {
            string navPoints = "";

            // Add the cover page nav point
            navPoints += GetCoverNavPoint();

            for (int i = 0; i < novel.Chapters.Count; i++)
            {
                navPoints += CreateNavPoint(i + 1, novel.Chapters[i]);
            }

            content = content.Replace("@Novel.Id", novel.Id.ToString());
            content = content.Replace("@Novel.Author", novel.Author);
            content = content.Replace("@Novel.Title", novel.Title);
            content = content.Replace("@NavPoints", navPoints);

            return content;
        }

        private string CreateNavPoint(int id, ChapterDTO chapter)
        {
            string txt = chapter.Title;
            string nav = "";

            if (chapter.Number > 0)
                txt = $"{chapter.Number}: {txt}";

            nav += $"<navPoint id=\"navpoint-{id}\" playOrder=\"{id}\">";
            nav += $"<navLabel>";
            nav += $"<text>{txt}</text>";
            nav += $"</navLabel>";
            nav += $"<content src=\"chapter{id}.xhtml\" />";
            nav += "</navPoint>";

            return nav;
        }

        private string GetCoverNavPoint()
        {
            string nav = "";

            nav += $"<navPoint id=\"navpoint-cover\" playOrder=\"0\">";
            nav += $"<navLabel>";
            nav += $"<text>Cover</text>";
            nav += $"</navLabel>";
            nav += $"<content src=\"cover.xhtml\" />";
            nav += "</navPoint>";

            return nav;
        }


    }
}
