using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Wryte.Models
{
    public class DisplayModel
    {

        // Actions

        [JsonIgnore]
        public Action Update { get; set; }

        // Fields

        private ListBy _listBy;

        private bool _listEventsCondensed;

        private bool _listEventsDesc;

        private bool _listNumberDesc;

        private bool _listNameDesc;

        private bool _showArchived;

        private bool _listNovels;

        private bool _listItems;

        private Guid _lastView;

        // Properties

        public ListBy ListBy
        {
            get => _listBy;
            set => SetField(ref _listBy, value);
        }

        public Guid LastView
        {
            get => _lastView;
            set => SetField(ref _lastView, value);
        }

        public bool ShowArchived
        {
            get => _showArchived;
            set => SetField(ref _showArchived, value);
        }

        public bool ListEventsCondensed
        {
            get => _listEventsCondensed;
            set => SetField(ref _listEventsCondensed, value);
        }

        public bool ListEventsDesc
        {
            get => _listEventsDesc;
            set => SetField(ref _listEventsDesc, value);
        }

        public bool ListNumberDesc
        {
            get => _listNumberDesc;
            set => SetField(ref _listNumberDesc, value);
        }

        public bool ListNameDesc
        {
            get => _listNameDesc;
            set => SetField(ref _listNameDesc, value);
        }

        public bool ListItems
        {
            get => _listItems;
            set => SetField(ref _listItems, value);
        }

        public bool ListNovels
        {
            get => _listNovels;
            set => SetField(ref _listNovels, value);
        }

        // Methods

        private void SetField<T>(ref T field, T value)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;

                Update?.Invoke();
            }
        }


    }

    // Enums

    public enum ListBy
    {
        Number,
        Name,
    }

}
