using DesignPatternsManagerW8;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace DesignPatternsBuilder
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class DesignPatternDetailPage : DesignPatternsBuilder.Common.LayoutAwarePage
    {
        private XDocument designPatternXML;
        public DesignPatternDetailPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override async void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            if (pageState != null && pageState.ContainsKey("SelectedItem"))
            {
                navigationParameter = pageState["SelectedItem"];
            }

            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            var item = DesignPatternDataSource.GetItem((String)navigationParameter);
            this.DefaultViewModel["Item"] = item;

            await LoadControls(item.Path);
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }
        private async Task LoadControls(String patternPath)
        {
            DesignPattensFileManager fileManager = new DesignPattensFileManager();
            var readFile = await fileManager.ReadFile(patternPath, await fileManager.GetDesignPatternsTemplatesPath());
            designPatternXML = XDocument.Parse(readFile);

            int i = 0;
            foreach (var param in designPatternXML.Descendants("Parameter"))
            {
                var multipleAttribute = param.Attribute("multiple");
                var multiple = multipleAttribute != null? Boolean.Parse(multipleAttribute.Value) : false;
                var name = param.Attribute("description").Value;
                var paramName = param.Attribute("name").Value;
                var rowDef = new RowDefinition();
                rowDef.Height = GridLength.Auto;

                ControlsGridView.RowDefinitions.Add(rowDef);
                var label = new TextBlock();
                label.Text = name + ":";
                label.Margin = new Thickness(0, 15, 0, 15);
                label.Style = App.Current.Resources["Label"] as Style;
                Grid.SetRow(label, i);
                Grid.SetColumn(label, 0);

                if (!multiple)
                {
                    var text = new TextBox();
                    text.Name = paramName;
                    text.Margin = new Thickness(15, 15, 75, 15);
                    Grid.SetRow(text, i);
                    Grid.SetColumn(text, 1);
                    ControlsGridView.Children.Add(text);
                    
                }
                else
                {
                    var outerGrid = new Grid();
                    outerGrid.Name = paramName;
                    outerGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    outerGrid.RowDefinitions.Add(new RowDefinition());
                    outerGrid.RowDefinitions.Add(new RowDefinition());

                    var innerGrid = new Grid();
                    innerGrid.Name = paramName + "list";
                    innerGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    innerGrid.RowDefinitions.Add(new RowDefinition());

                    Grid.SetColumn(innerGrid, 0);
                    Grid.SetRow(innerGrid, 0);
                    
                    var text = new TextBox();
                    text.Name = paramName + "_1";
                    text.Margin = new Thickness(15, 15, 75, 15);
                    Grid.SetRow(text, 0);
                    Grid.SetColumn(text, 0);

                    innerGrid.Children.Add(text);

                    var buttonAdd = new Button();
                    buttonAdd.Content = "Add";
                    buttonAdd.Name = paramName + "add_button";
                    buttonAdd.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Right;
                    buttonAdd.Margin = new Thickness(15, 15, 75, 15);
                    buttonAdd.Click += buttonAdd_Click;
                    Grid.SetColumn(buttonAdd, 0);
                    Grid.SetRow(buttonAdd, 1);

                    outerGrid.Children.Add(innerGrid);
                    outerGrid.Children.Add(buttonAdd);

                    Grid.SetRow(outerGrid, i);
                    Grid.SetColumn(outerGrid, 1);

                    ControlsGridView.Children.Add(outerGrid);
                }
                ControlsGridView.Children.Add(label);
                i++;
            }

        }

        void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            var buttonSender = (Button)sender;
            var gridSender = (Grid)buttonSender.Parent;

            var gridSenderChilds = from i in gridSender.Children
                    where i is Grid
                    select (Grid)i;

            var textboxGrid = gridSenderChilds.FirstOrDefault(n => n.Name == gridSender.Name + "list");

            var textboxes = from t in textboxGrid.Children
                            where t is TextBox
                            select (TextBox)t;

            var textboxesNumbers = textboxes.Select(p => Int16.Parse(p.Name.Replace(gridSender.Name+"_", "")));

            var latsNumber = (from t in textboxesNumbers
                              orderby t
                              select t).LastOrDefault();

            var text = new TextBox();
            text.Name = gridSender.Name + "_" + (latsNumber + 1).ToString();
            text.Margin = new Thickness(15, 15, 75, 15);
            Grid.SetRow(text, latsNumber);
            Grid.SetColumn(text, 0);

            textboxGrid.RowDefinitions.Add(new RowDefinition());
            textboxGrid.Children.Add(text);
        }

        private async void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            var textBoxes = from c in ControlsGridView.Children
                            where c is TextBox
                            select (TextBox)c;

            var parameters = designPatternXML.Descendants("Parameter");

            var parametersControls = from p in parameters
                                     join t in textBoxes on p.Attribute("name").Value equals t.Name
                                     select new Tuple<TextBox, String>(t, p.Attribute("description").Value);

            var isValid = await ValidateTextBoxes(parametersControls);

            if (isValid)
            {
                Dictionary<String, String> dpParameters = new Dictionary<string, string>();
                var designPattern = (DesignPatternDataItem)this.DefaultViewModel["Item"];
                foreach (var item in textBoxes)
                {
                    dpParameters.Add(item.Name, item.Text);
                }
                var _dpFileManager = new DesignPattensFileManager();
                var designPatternBuilder = new DesignPatternBuilder(_dpFileManager);
                var classInformation = await designPatternBuilder.BuildFromXml(designPattern.Path, dpParameters, null);

                FolderPicker folderPicker = new FolderPicker();
                folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
                folderPicker.FileTypeFilter.Add(".cs");

                StorageFolder folder = await folderPicker.PickSingleFolderAsync();
                if (folder != null)
                {
                    foreach (var item in classInformation)
                    {
                        await _dpFileManager.CreateFile(item.FileName, folder, item.Content);
                    }

                    MessageDialog finished = new MessageDialog("Files created successfully", "Files Ready");
                    await finished.ShowAsync();
                }
            }
        }
        private async Task<bool> ValidateTextBoxes(IEnumerable<Tuple<TextBox, String>> textBoxes)
        {
            var errorBuilder = new StringBuilder();
            var hasErrors = false;
            foreach (var item in textBoxes)
            {
                if (String.IsNullOrEmpty(item.Item1.Text))
                {
                    errorBuilder.Append(item.Item2 + " could not be empty");
                    errorBuilder.Append(Environment.NewLine);
                    hasErrors = true;
                }
            }

            if (hasErrors)
            {
                MessageDialog error = new MessageDialog(errorBuilder.ToString());
                await error.ShowAsync();
            }
            return !hasErrors;
        }
    }
}
