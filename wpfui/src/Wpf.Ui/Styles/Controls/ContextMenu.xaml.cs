using System;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace Wpf.Ui.Styles.Controls
{
    partial class ContextMenu : ResourceDictionary
    {
        public ContextMenu()
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(OnResourceDictionaryLoaded));
        }

        private void OnResourceDictionaryLoaded()
        {
            var currentAssembly = typeof(Application).Assembly;

            AddEditorContextMenuDefaultStyle(currentAssembly);
        }

        private void AddEditorContextMenuDefaultStyle(Assembly currentAssembly)
        {
            var contextMenuStyle = this["UiContextMenu"] as Style;
            var editorContextMenuType = Type.GetType("System.Windows.Documents.TextEditorContextMenu+EditorContextMenu, " + currentAssembly);

            if (editorContextMenuType == null || contextMenuStyle == null)
                return;

            var editorContextMenuStyle = new Style(editorContextMenuType, contextMenuStyle);
            Add(editorContextMenuType, editorContextMenuStyle);
        }

    }
}
