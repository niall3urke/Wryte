using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Wryte.Models
{
    public class SettingsModel
    {

        // Actions

        [JsonIgnore]
        public Action Update { get; set; }

        // Fields

        private ToolbarModel _toolbar;

        private DisplayModel _display;

        private string _pageWidth;

        private bool _typewriter;

        private bool _showHeader;

        private bool _autoFade;

        private bool _autoSave;

        private bool _darkMode;

        private Guid _id;

        // Constructors

        public SettingsModel()
        {
            _toolbar = new ToolbarModel();
            _display = new DisplayModel();
            _pageWidth = "is-normal";
            _typewriter = false;
            _showHeader = true;
            _autoFade = false;
            _autoSave = true;

            _id = Guid.NewGuid();
        }

        // Properties

        public Guid Id
        {
            get => _id;
            set => SetField(ref _id, value);
        }

        public DisplayModel Display
        {
            get => _display;
            set => SetField(ref _display, value);
        }

        public ToolbarModel Toolbar
        {
            get => _toolbar;
            set => SetField(ref _toolbar, value);
        }

        public string PageWidth
        {
            get => _pageWidth;
            set => SetField(ref _pageWidth, value);
        }

        public bool Typewriter
        {
            get => _typewriter;
            set => SetField(ref _typewriter, value);
        }

        public bool ShowHeader
        {
            get => _showHeader;
            set => SetField(ref _showHeader, value);
        }

        public bool AutoFade
        {
            get => _autoFade;
            set => SetField(ref _autoFade, value);
        }

        public bool AutoSave
        {
            get => _autoSave;
            set => SetField(ref _autoSave, value);
        }

        public bool DarkMode
        {
            get => _darkMode;
            set => SetField(ref _darkMode, value);
        }

        // Methods 

        private void SetField<T>(ref T field, T value, bool update = true)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;

                if (update)
                {
                    Update?.Invoke();
                }
            }
        }

        public SettingsModel Clone()
        {
            var settings = MemberwiseClone() as SettingsModel;

            settings.Toolbar = Toolbar.Clone();

            return settings;
        }

        public bool RefreshRequired(SettingsModel settings)
        {
            if (AutoFade != settings.AutoFade)
                return true;

            if (!Toolbar.CompareTo(settings.Toolbar))
                return true;

            return false;
        }

    }

    public enum EditorWidth
    {
        Narrow,
        Normal, 
        Wide
    }
}
