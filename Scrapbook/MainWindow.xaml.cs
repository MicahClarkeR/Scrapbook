using Microsoft.Win32;
using Scrapbook.Core;
using Scrapbook.Editing;
using Scrapbook.Persistence;
using Scrapbook.UI.Selection;
using Scrapbook.UI.WPF;
using System.Windows;
using System.Windows.Controls;

namespace Scrapbook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BoardEditorSession _session => _boardSurface.Session;
        private BoardSurfaceControl _boardSurface;
        private SelectionPropertyInspectorControl _inspector;

        public MainWindow()
        {
            BoardEditorSession session = new BoardEditorSession();
            _boardSurface = new BoardSurfaceControl();

            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // Initialise Board Surface and place in the Main Grid.
            _boardSurface.BeginInit();
            MainGrid.Children.Add(_boardSurface);
            Grid.SetColumn(_boardSurface, 0);

            // Create Property Inspector once the Board Surface has been initiated.
            _inspector = new SelectionPropertyInspectorControl(PropertyGrid, _session, _boardSurface);

            // Set Window title.
            Title = GlobalDefaults.UI.WindowTitle;
        }

        private void SaveFileButtonClick(object sender, RoutedEventArgs e)
        {
            // Create Save File Dialog then, if a file is chosen, save it using the dedicated Board Saver.
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                AddExtension = true,
                DefaultExt = ".sbook",
                Filter = "Scrapbook Files (*.sbook)|*.sbook"
            };

            if (saveFileDialog.ShowDialog() ?? false)
            {
                BoardSaver saver = new BoardSaver(_session);
                saver.Save(saveFileDialog.FileName);
            }
        }

        #region Menu Interactions
        private void OpenFileButtonClick(object sender, RoutedEventArgs e)
        {
            _session.Editor.OpenFile(_session);
        }

        private async void EditUndoButtonClick(object sender, RoutedEventArgs e)
        {
            await _session.History.Undo(_session.Document);
        }

        private async void EditRedoButtonClick(object sender, RoutedEventArgs e)
        {
            await _session.History.Redo(_session.Document);
        }
        #endregion
    }
}