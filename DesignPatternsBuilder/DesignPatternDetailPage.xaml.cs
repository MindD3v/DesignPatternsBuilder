using System.Collections.ObjectModel;
using DesignPatternsManagerW8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace DesignPatternsBuilder
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class DesignPatternDetailPage : DesignPatternsBuilder.Common.LayoutAwarePage
    {
        private List<DesignPatternParameter> _designPatternParameters;
        public DesignPatternDetailPage()
        {
            InitializeComponent();
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

            var item = DesignPatternDataSource.GetItem((String)navigationParameter);
            DefaultViewModel["Item"] = item;

            var designPatternsReader = new DesignPatternsReader(new FileManager());
            _designPatternParameters = (await designPatternsReader.GetParametersFromDesignPattern(item.Path)).ToList();

            //imgPattern.Source = item.Image;

            LoadControls();
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
        private void LoadControls()
        {
            int i = 0;
            foreach (var param in _designPatternParameters)
            {
                var rowDef = new RowDefinition {Height = GridLength.Auto};

                ControlsGridView.RowDefinitions.Add(rowDef);
                var label = new TextBlock
                    {
                        Text = param.Description + ":",
                        Margin = new Thickness(0, 15, 0, 15),
                        Style = Application.Current.Resources["Label"] as Style
                    };
                label.LayoutUpdated += (sender, o) =>
                    {
                        if (ApplicationViewStates.CurrentState.Name == "Snapped")
                        {
                            label.Style = Application.Current.Resources["LabelSnapped"] as Style;
                        }
                        else
                        {
                            label.Style = Application.Current.Resources["Label"] as Style;
                        }
                    };
                Grid.SetRow(label, i);
                Grid.SetColumn(label, 0);

                if (!param.IsMultiple)
                {
                    var text = new TextBox
                        {
                            Name = param.Name,
                            Margin = new Thickness(15, 15, 75, 15)
                        };
                    text.LayoutUpdated += (sender, o) =>
                        {
                            text.Margin = ApplicationViewStates.CurrentState.Name == "Snapped"
                                              ? new Thickness(15, 15, 15, 15)
                                              : new Thickness(15, 15, 75, 15);
                        };
                    Grid.SetRow(text, i);
                    Grid.SetColumn(text, 1);
                    ControlsGridView.Children.Add(text);
                    
                }
                else
                {
                    CreateMultipleParameterGrid(param,i);
                }
                ControlsGridView.Children.Add(label);
                i++;
            }

        }

        private void CreateMultipleParameterGrid(DesignPatternParameter param, int rowNumber)
        {
            //Create the outer Grid to hold the Grid that will contain the list and the buttons
            var parameterGrid = new Grid
            {
                Name = param.Name
            };
            parameterGrid.ColumnDefinitions.Add(new ColumnDefinition());
            parameterGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star)});
            parameterGrid.RowDefinitions.Add(new RowDefinition{Height = new GridLength(1,GridUnitType.Star)});
            parameterGrid.Loaded += (sender, args) =>
                {
                    var g = (Grid)sender;
                    var p = (Grid)g.Parent;
                    g.MaxHeight = p.ActualHeight;
                };
            parameterGrid.LayoutUpdated += (sender, o) =>
                {
                    var g = parameterGrid;
                    var p = (Grid)g.Parent;
                    g.MaxHeight = p.ActualHeight;
                };

            var parameterListGrid = new StackPanel()
                {
                    
                    Name = param.Name + "list"
                };
            var firstParameterInList = new TextBox
            {
                Name = param.Name + "_1"
            };

            firstParameterInList.LayoutUpdated += (sender, o) => UpdateTextBoxLayout(firstParameterInList);

            
            parameterListGrid.Children.Add(firstParameterInList);

            var addParameterButton = new Button
            {
                Content = "Add",
                Name = param.Name + "_add_button",
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(15, 15, 75, 15)
            };
            addParameterButton.Click += buttonAdd_Click;
            Grid.SetColumn(addParameterButton, 0);
            Grid.SetRow(addParameterButton, 1);

            parameterGrid.Children.Add(parameterListGrid);
            parameterGrid.Children.Add(addParameterButton);

            Grid.SetRow(parameterGrid, rowNumber);
            Grid.SetColumn(parameterGrid, 1);

            ControlsGridView.Children.Add(parameterGrid);
        }

        void UpdateTextBoxLayout(TextBox sender)
        {
            sender.Margin = ApplicationViewStates.CurrentState.Name == "Snapped"
                                                      ? new Thickness(15, 15, 15, 15)
                                                      : new Thickness(15, 15, 80, 15);
            var g = (StackPanel)sender.Parent;
            sender.Width = g.ActualWidth;
        }
        void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            var buttonSender = (Button)sender;
            var gridSender = (Grid)buttonSender.Parent;

            var gridSenderChilds = from i in gridSender.Children
                                   where i is StackPanel
                                   select (StackPanel) i;

            var textboxGrid = gridSenderChilds.FirstOrDefault(n => n.Name == gridSender.Name + "list");

            var textboxes = from t in textboxGrid.Children
                            where t is TextBox
                            select (TextBox)t;

            var textboxesNumbers = textboxes.Select(p => Int16.Parse(p.Name.Replace(gridSender.Name+"_", "")));

            var latsNumber = (from t in textboxesNumbers
                              orderby t
                              select t).LastOrDefault();

            var text = new TextBox
                {
                    Name = gridSender.Name + "_" + (latsNumber + 1).ToString()
                };


            text.LayoutUpdated += (o, o1) => UpdateTextBoxLayout(text);
            
            Grid.SetRow(text, latsNumber);
            Grid.SetColumn(text, 0);

            textboxGrid.Children.Add(text);
        }
        private Tuple<Grid, DesignPatternParameter> GetParameterGrid(DesignPatternParameter dpParameter)
        {
            var grids = (from p in ControlsGridView.Children
                         where p is Grid
                         select p as Grid).ToList();

            var parameterGrid = (from g in grids
                                where g.Name == dpParameter.Name
                                select new Tuple<Grid, DesignPatternParameter>(g, dpParameter)).FirstOrDefault();

            return parameterGrid;

        }
        private async void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            var textBoxes = from c in ControlsGridView.Children
                            where c is TextBox
                            select (TextBox)c;


            var textBoxs = textBoxes as IList<TextBox> ?? textBoxes.ToList();

            var parametersControls = from p in _designPatternParameters
                                     join t in textBoxs on p.Name equals t.Name
                                     select new Tuple<TextBox, DesignPatternParameter>(t, p);

            var isValid = await ValidateTextBoxes(parametersControls);


            var multipleObjects = (from p in _designPatternParameters
                                   where p.IsMultiple
                                   select p).ToList();

            bool isValidMultiple = true;
            var objects = new Dictionary<String, List<String>>();
            if (multipleObjects.Any())
            {
                var grids = from g in ControlsGridView.Children
                            where g is Grid
                            select (Grid)g;

                var gridParameters = from g in grids
                                     join p in multipleObjects on g.Name equals p.Name
                                     select new Tuple<Grid, DesignPatternParameter>(g, p);
                
                foreach (var g in gridParameters)
                {
                    var list = (from i in g.Item1.Children
                                where i is StackPanel
                                select (StackPanel)i).FirstOrDefault();

                    var textBoxesFromList = (from t in list.Children
                              where t is TextBox
                              select (TextBox)t).ToList();

                    var txtValuesList = (from t in textBoxesFromList
                                         select t.Text).ToList();

                    objects.Add(g.Item2.Name, txtValuesList);

                    isValidMultiple = await ValidateTextBoxesMultiParameter(textBoxesFromList, g.Item2);
                }
            }


            if (isValid && isValidMultiple)
            {
                var designPattern = (DesignPatternDataItem)DefaultViewModel["Item"];
                var dpParameters = textBoxs.ToDictionary(item => item.Name, item => item.Text);
                var dpFileManager = new FileManager();
                var designPatternBuilder = new DesignPatternBuilder(dpFileManager);
                var classInformation = await designPatternBuilder.BuildFromXml(designPattern.Path, dpParameters, objects);

                var folderPicker = new FolderPicker {SuggestedStartLocation = PickerLocationId.Desktop};
                folderPicker.FileTypeFilter.Add(".cs");

                StorageFolder folder = await folderPicker.PickSingleFolderAsync();
                if (folder != null)
                {
                    foreach (var item in classInformation)
                    {
                        await dpFileManager.CreateFile(item.FileName, folder, item.Content);
                    }

                    var finished = new MessageDialog("Files created successfully", "Files Ready");
                    await finished.ShowAsync();
                }
            }
            Frame.GoBack();
        }
        #region Validation
        private async Task<bool> ValidateTextBoxes(IEnumerable<Tuple<TextBox, DesignPatternParameter>> textBoxes)
        {
            var errorBuilder = new StringBuilder();
            var hasErrors = false;
            foreach (var item in textBoxes)
            {
                if (String.IsNullOrEmpty(item.Item1.Text))
                {
                    errorBuilder.Append(item.Item2.Name + " could not be empty");
                    errorBuilder.Append(Environment.NewLine);
                    hasErrors = true;
                }
            }

            if (hasErrors)
            {
                var error = new MessageDialog(errorBuilder.ToString());
                await error.ShowAsync();
            }
            return !hasErrors;
        }
        private async Task<bool> ValidateTextBoxesMultiParameter(IEnumerable<TextBox> textBoxes, DesignPatternParameter parameter)
        {
            var errorBuilder = new StringBuilder();
            var hasErrors = false;
            foreach (var item in textBoxes)
            {
                if (String.IsNullOrEmpty(item.Text))
                {

                    errorBuilder.Append(parameter.Description+ " " + item.Name.Replace(parameter.Name +"_","") + " could not be empty");
                    errorBuilder.Append(Environment.NewLine);
                    hasErrors = true;
                }
            }

            if (hasErrors)
            {
                var error = new MessageDialog(errorBuilder.ToString());
                await error.ShowAsync();
            }
            return !hasErrors;
        }
        #endregion
    }
}
