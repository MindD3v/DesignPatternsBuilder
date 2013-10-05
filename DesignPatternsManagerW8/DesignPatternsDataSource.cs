using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.Specialized;
using System.Threading.Tasks;
using DesignPatternsCommonLibrary;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace DesignPatternsManagerW8
{
    /// <summary>
    /// Base class for <see cref="DesignPatternDataItem"/> and <see cref="DesignPatternDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class DesignPatternDataCommon : BindableBase
    {
        public DesignPatternDataCommon(String uniqueId, String designPatterName)
        {
            this._uniqueId = uniqueId;
            this._designPatternName = designPatterName;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _designPatternName = string.Empty;
        public string DesignPatternName
        {
            get { return this._designPatternName; }
            set { this.SetProperty(ref this._designPatternName, value); }
        }

        public override string ToString()
        {
            return this.DesignPatternName;
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class DesignPatternDataItem : DesignPatternDataCommon
    {
        public DesignPatternDataItem(String uniqueId, String designPatterName, String description, DesignPatternDataGroup group)
            : base(uniqueId, designPatterName)
        {
            this._group = group;
            this._description = description;
        }

        private DesignPatternDataGroup _group;
        public DesignPatternDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class DesignPatternDataGroup : DesignPatternDataCommon
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(DesignPatternDataGroup._baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }

        public DesignPatternDataGroup(String uniqueId, String designPatterName, String imagePath)
            : base(uniqueId, designPatterName)
        {
            Items.CollectionChanged += ItemsCollectionChanged;
            _imagePath = imagePath;
        }
        
        private void ItemsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex < 12)
                    {
                        TopItems.Insert(e.NewStartingIndex,Items[e.NewStartingIndex]);
                        if (TopItems.Count > 12)
                        {
                            TopItems.RemoveAt(12);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    if (e.OldStartingIndex < 12 && e.NewStartingIndex < 12)
                    {
                        TopItems.Move(e.OldStartingIndex, e.NewStartingIndex);
                    }
                    else if (e.OldStartingIndex < 12)
                    {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        TopItems.Add(Items[11]);
                    }
                    else if (e.NewStartingIndex < 12)
                    {
                        TopItems.Insert(e.NewStartingIndex, Items[e.NewStartingIndex]);
                        TopItems.RemoveAt(12);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex < 12)
                    {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        if (Items.Count >= 12)
                        {
                            TopItems.Add(Items[11]);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.OldStartingIndex < 12)
                    {
                        TopItems[e.OldStartingIndex] = Items[e.OldStartingIndex];
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    TopItems.Clear();
                    while (TopItems.Count < Items.Count && TopItems.Count < 12)
                    {
                        TopItems.Add(Items[TopItems.Count]);
                    }
                    break;
            }

        }
        
        private ObservableCollection<DesignPatternDataItem> _items = new ObservableCollection<DesignPatternDataItem>();
        public ObservableCollection<DesignPatternDataItem> Items
        {
            get { return this._items; }
        }

        private ObservableCollection<DesignPatternDataItem> _topItem = new ObservableCollection<DesignPatternDataItem>();
        public ObservableCollection<DesignPatternDataItem> TopItems
        {
            get {return this._topItem; }
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// 
    /// SampleDataSource initializes with placeholder data rather than live production
    /// data so that sample data is provided at both design-time and run-time.
    /// </summary>
    public sealed class DesignPatternDataSource
    {
        private static DesignPatternDataSource _sampleDataSource = new DesignPatternDataSource();

        private ObservableCollection<DesignPatternDataGroup> _allGroups = new ObservableCollection<DesignPatternDataGroup>();
        public ObservableCollection<DesignPatternDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<DesignPatternDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            
            return _sampleDataSource.AllGroups;
        }

        public static DesignPatternDataGroup GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static DesignPatternDataItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public DesignPatternDataSource()
        {
            PopulateDesignPatterns().ConfigureAwait(false);
        }
        public async Task PopulateDesignPatterns()
        {
            var fileManager = new DesignPattensFileManager();
            var designPatternUpdater = new DesignPatternsUpdater(fileManager);

            var designpatternList = await designPatternUpdater.UpdateDesignPatterns();
            var designPatternTypesList = from p in designpatternList
                                         group p by p.DesignPatternType
                                         into t
                                         select t;

            foreach (var designPatternType in designPatternTypesList)
            {
                var group = new DesignPatternDataGroup(designPatternType.Key.Type, designPatternType.Key.Type, 
                    fileManager.GetDesignPatternsTemplatesPath() + "\\" + designPatternType.Key.ImagePath);
                foreach (var designPattern in designPatternType)
                {
                    var item = new DesignPatternDataItem(designPattern.Id.ToString(), designPattern.DesignPatternName, designPattern.Description, group);
                    group.Items.Add(item);
                }
                _allGroups.Add(group);
            }
        }
    }
}
