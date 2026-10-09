using Scrapbook.Editing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Scrapbook.UI.WPF
{
    // WPF-facing commands and bindable UIValues state
    public sealed class BoardEditorViewModel
    {
        public BoardEditorSession Session { get; }

        // Operations below to bind to user interface XAML.
        public ICommand ImportImageCommand { get; }
        public ICommand PasteCommand { get; }
        public ICommand AddTextCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand LoadCommand { get; }
        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }
    }
}
