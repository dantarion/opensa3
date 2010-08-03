using System.Windows;
using System.Windows.Input;
using OpenSALib3;

namespace Tabuu.UI {
    /// <summary>
    /// Interaction logic for ScriptWindow.xaml
    /// </summary>
    public partial class ScriptWindow : Window {
        public ScriptWindow() {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e) {
            OutputTextBox.Text += ">>\n" + RunScript.FromString(ScriptTextBox.Text, null);
        }

        private void Button2Click(object sender, RoutedEventArgs e) {
            OutputTextBox.Text = "";
        }

        private void TextBoxKeyDown(object sender, KeyEventArgs e) {
            if (e.Key != Key.Return)
                return;
            OutputTextBox.Text += ">>" + CommandTextBox.Text + "\n" + RunScript.FromString(CommandTextBox.Text, null);
        }
    }
}
