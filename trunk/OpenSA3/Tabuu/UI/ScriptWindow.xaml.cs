using System.Windows;
using System.Windows.Input;
using OpenSALib3;
using System.IO;
using System.Text;
namespace Tabuu.UI {
    /// <summary>
    ///   Interaction logic for ScriptWindow.xaml
    /// </summary>
    /// 
   
    public partial class ScriptWindow {
        public ScriptWindow() {
            InitializeComponent();
        }
        private StringBuilder sb = new StringBuilder();
        private void ButtonClick(object sender, RoutedEventArgs e) {
            sb.Append(">>\n");
            sb.Append(RunScript.FromString(ScriptTextBox.Text, null));
            OutputTextBox.Text = sb.ToString();
            OutputTextBox.ScrollToEnd();
        }

        private void Button2Click(object sender, RoutedEventArgs e) {
            sb.Clear();
            OutputTextBox.Text = "";
        }

        private void TextBoxKeyDown(object sender, KeyEventArgs e) {
            if (e.Key != Key.Return)
                return;
            sb.Append(">>");
            sb.Append(CommandTextBox.Text);
            sb.Append("\n");
            sb.Append(RunScript.FromString(CommandTextBox.Text, null));
            OutputTextBox.Text = sb.ToString();
            OutputTextBox.ScrollToEnd();
        }
    }
}