using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.Specialized;
using System.Threading.Tasks;

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
            _uniqueId = uniqueId;
            _designPatternName = designPatterName;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return _uniqueId; }
            set { SetProperty(ref _uniqueId, value); }
        }

        private string _designPatternName = string.Empty;
        public string DesignPatternName
        {
            get { return _designPatternName; }
            set { SetProperty(ref _designPatternName, value); }
        }

        public override string ToString()
        {
            return _designPatternName;
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class DesignPatternDataItem : DesignPatternDataCommon
    {
        private Uri _baseUri;
        public DesignPatternDataItem(String uniqueId, String designPatterName, String description, String path, String imagePath, DesignPatternDataGroup group)
            : base(uniqueId, designPatterName)
        {
            _group = group;
            _description = description;
            _path = path;
            _imagePath = imagePath;
            _baseUri = new Uri("ms-appx:///DesignPatternsManagerW8/DesignPatternsTemplates/");
        }

        private DesignPatternDataGroup _group;
        public DesignPatternDataGroup Group
        {
            get { return _group; }
            set { SetProperty(ref _group, value); }
        }
        private string _description = string.Empty;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private string _path = string.Empty;
        public string Path
        {
            get { return _path; }
            set { SetProperty(ref _path, value); }
        }
        private ImageSource _image;
        private String _imagePath;
        public ImageSource Image
        {
            get
            {
                if (_image == null && _imagePath != null)
                {
                    _image = new BitmapImage(new Uri(_baseUri, _imagePath));
                }
                return _image;
            }

            set
            {
                _imagePath = null;
                SetProperty(ref _image, value);
            }
        }

        public void SetImage(String path)
        {
            _image = null;
            _imagePath = path;
            OnPropertyChanged("Image");
        }
        
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class DesignPatternDataGroup : DesignPatternDataCommon
    {
        
        public DesignPatternDataGroup(String uniqueId, String designPatterName)
            : base(uniqueId, designPatterName)
        {
            Items.CollectionChanged += ItemsCollectionChanged;
            
        }
        
        private void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
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
            get { return _items; }
        }

        private ObservableCollection<DesignPatternDataItem> _topItem = new ObservableCollection<DesignPatternDataItem>();
        public ObservableCollection<DesignPatternDataItem> TopItems
        {
            get {return _topItem; }
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
            get { return _allGroups; }
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
            var fileManager = new FileManager();
            var designPatternUpdater = new DesignPatternsReader(fileManager);

            var designpatternList = await designPatternUpdater.UpdateDesignPatterns(true);
            var designPatternTypesList = from p in designpatternList
                                         group p by p.DesignPatternType
                                         into t
                                         select t;

            foreach (var designPatternType in designPatternTypesList)
            {
                var group = new DesignPatternDataGroup(designPatternType.Key, designPatternType.Key);
                foreach (var designPattern in designPatternType)
                {
                    var item = new DesignPatternDataItem(designPattern.Id.ToString(), designPattern.DesignPatternName,
                                                         designPattern.Description, designPattern.Path, designPattern.Path.Replace(".xml", ".png"), group);
                    group.Items.Add(item);
                }
                _allGroups.Add(group);
            }
        }
    }
}
