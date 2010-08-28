using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
//TODO: Finish translations. Bero, could you use English where possible?

namespace OpenSALib3.WPFControls {
    /// <summary>
    /// IntelListBox.xaml の相互作用ロジック
    /// </summary>
    /// 
    public partial class IntelliBox {
        private static readonly string[] Keywords = 
        { "and","as","assert","break","class","continue","def","elif","else","except","finally","for","from",
            "global","False","if","import","in","is","lambda","None","nonlocal","not","or","pass","raise","return",
            "Truetry","while","with","yeild"};
        private static readonly char[] Tokens = { '(', ')', ',', ';', ':', '[', ']', '{', '}', ' ', '\n', '\r', '\t' };
        private bool _completed;
        private bool _insertIndent;
        private string _currentIndent = "";
        private string _colonStack = "";
        private readonly Dictionary<string, string> _descriptions = new Dictionary<string, string>();

        private List<string> _targetSource = new List<string>();
        public List<string> TargetSource { get { return _targetSource; } set { _targetSource = value; } }
        public string Text { get { return CodeBox.Text; } set { CodeBox.Text = value; } }
        public bool UseIntelliSense { get; set; }

        public IntelliBox() {
            UseIntelliSense = true;
            InitializeComponent();
            GetMembers();
        }
        /// <summary>
        /// This gets current word.
        /// </summary>
        /// <param name="textBox"></param>
        /// <returns></returns>
        private static string GetCurrentWord(TextBox textBox) {
            if (string.IsNullOrEmpty(textBox.Text)) {
                return string.Empty;
            }
            if (textBox.CaretIndex == 0) {
                return string.Empty;
            }

            var index = textBox.CaretIndex - 1;
            var space = textBox.Text.LastIndexOfAny(Tokens, index) + 1;
            var plus = textBox.Text.IndexOfAny(Tokens, index);
            return textBox.Text.Substring(space, plus - space > 0 ? plus - space : textBox.CaretIndex - space);
        }

        private bool IsComment() {
            if (CodeBox.Text.Length > 0) {
                for (var i = CodeBox.CaretIndex - 1; i >= 0; i--)
                    switch (CodeBox.Text[i]) {
                        case '\n':
                            return false;
                        case '#':
                            return true;
                    }
            }

            return false;
        }
        /// <summary>
        /// Is this key a letter key?
        /// </summary>
        /// <param name="e">pressed key data</param>
        /// <returns>Letter or not</returns>
        private static bool IsLetter(Key e) {
            return (e >= Key.A && e <= Key.Z) || (e >= Key.NumPad0 && e <= Key.NumPad9) || (e >= Key.D0 && e <= Key.D9);
        }

        /// <summary>
        /// Generates ListViewItem
        /// </summary>
        private void GenerateItems() {
            ListView.Items.Clear();
            var currentWord = GetCurrentWord(CodeBox);
            var longestWord = "";
            if (IsComment()) return;
            if (!string.IsNullOrWhiteSpace(currentWord))
                foreach (var s in _targetSource.Where(x => x.StartsWith(currentWord) && x != currentWord)) {
                    var tip = new ToolTip { Content = _descriptions.ContainsKey(s) ? _descriptions[s] : null };
                    ListView.Items.Add(new ListViewItem { Content = s, ToolTip = tip });
                    if (s.Length > longestWord.Length)
                        longestWord = s;
                }
            if (ListView.Items.Count > 0) {
                ListView.SelectedIndex = 0;
                popup.IsOpen = true;
                ListView.Width = longestWord.Length * 15;
            } else
                popup.IsOpen = false;
        }

        private void SelectItem() {
            if (ListView.SelectedItems.Count <= 0) return;
            var textBox = (TextBox)popup.PlacementTarget;
            var caretIndex = textBox.CaretIndex;
            var currentWord = GetCurrentWord(textBox);
            var selectedText = ((ListViewItem)ListView.SelectedItem).Content as string;

            // 選択されたものを挿入 //Insert whatever's selected
            if (String.IsNullOrEmpty(selectedText)) return;
            var tmpText = textBox.Text.Remove(caretIndex - currentWord.Length, currentWord.Length);
            textBox.Text = tmpText.Insert(caretIndex - currentWord.Length, selectedText);
            textBox.CaretIndex = caretIndex + selectedText.Length;
            popup.IsOpen = false;
        }

        private void GetMembers() {
            //Get PSAName
            var type = typeof(CommandReceiver);
            var info = type.GetMethods();
            foreach (var i in info.Where(i => 
                    i.IsPublic && !i.Name.Contains("set_") && !i.Name.Contains("get_") &&
                    i.DeclaringType == typeof (CommandReceiver))) {
                _targetSource.Add(i.Name); _descriptions.Add(i.Name, i.Name + "(");
                var param = i.GetParameters();
                foreach (var paraminfo in param)
                    _descriptions[i.Name] += paraminfo.ParameterType.Name + " " + paraminfo.Name + ",";
                _descriptions[i.Name] += ")";
            }

            foreach (var s in Keywords)
                _targetSource.Add(s);
        }

        private void TextBoxPreviewKeyDown(object sender, KeyEventArgs e) {
            if (!UseIntelliSense) return;
            Action upKeyPressed = () => {
                                      if (!popup.IsOpen) return;
                                      if (ListView.SelectedIndex >= 1)
                                          ListView.SelectedIndex = ListView.SelectedIndex - 1;
                                      e.Handled = true; ((ListViewItem)ListView.SelectedItem).Focus();
                                  };
            Action downKeyPressed = () => {
                                        if (!popup.IsOpen) return;
                                        if (ListView.SelectedIndex < 0 ||
                                            ListView.SelectedIndex >= ListView.Items.Count - 1) return;
                                        ListView.SelectedIndex = ListView.SelectedIndex + 1;
                                        e.Handled = true; ((ListViewItem)ListView.SelectedItem).Focus();
                                    };
            Action enterKeyPressed = () => {
                                         if (popup.IsOpen) {
                                             //アイテムを選んだ、ということなので行変えを無効にする
                                             CodeBox.AcceptsReturn = false;
                                             SelectItem();
                                             ListView.Items.Clear();
                                             _completed = true;
                                         } else {
                                             //アイテムを選んで無いときは行変えを有効にする
                                             CodeBox.AcceptsReturn = true;
                                             //こっから先、インデントの文字数を取得
                                             _insertIndent = true;
                                             _currentIndent = "";
                                             var i = CodeBox.SelectionStart - 1;
                                             if (i >= 0 && CodeBox.Text.Length != 0) {
                                                 while (CodeBox.Text[i] != '\n')//search \n
                                                 {
                                                     i--;
                                                     if (i <= 0) {
                                                         i = 0; break;
                                                     }
                                                     if (CodeBox.Text[i] != '\n') continue;
                                                     i++; break;
                                                 }
                                                 while (CodeBox.Text[i] == ' ') {
                                                     _currentIndent += " "; i++;
                                                     if (i == CodeBox.Text.Length || CodeBox.Text[i] != ' ')
                                                         break;
                                                 }
                                             } else
                                                 _currentIndent = "";

                                         }
                                     };
            Action spaceKeyPressed = () => {
                                         if (!popup.IsOpen) return;
                                         CodeBox.AcceptsReturn = false;
                                         SelectItem();
                                         ListView.Items.Clear();
                                         _completed = true;
                                     };
            Action defaultCase = () => {
                                 };
            Action backKeyPressed = () => {
                                    };
            Action colonKeyPressed = () => {
                                         _colonStack += "    ";
                                     };
            switch (e.Key) {
                case Key.OemSemicolon: colonKeyPressed(); break;
                case Key.Space: popup.IsOpen = false; spaceKeyPressed(); break;
                case Key.Up: upKeyPressed(); break;
                case Key.Down: downKeyPressed(); break;
                case Key.Back: backKeyPressed(); break;
                case Key.Enter: enterKeyPressed(); break;
                default: defaultCase(); break;
            }
        }


        private void ListViewPreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Return || e.Key == Key.Space) {
                SelectItem();
                ListView.Items.Clear();
                popup.IsOpen = false;
                CodeBox.Focus();
                CodeBox.AcceptsReturn = false;
                _completed = true;
            } else if (IsLetter(e.Key) || e.Key == Key.Back) {
                CodeBox.Focus();
            }
        }

        private void ListViewPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e) {
            SelectItem();
            ListView.Items.Clear();
            popup.IsOpen = false;
            CodeBox.Focus();
        }

        private void CodeBoxPreviewKeyUp(object sender, KeyEventArgs e) {
            if (!UseIntelliSense) return;
            if (_insertIndent) {
                //インデント挿入するよ //Insert indentation
               var caretIndex = CodeBox.CaretIndex;
                CodeBox.Text = CodeBox.Text.Insert(CodeBox.SelectionStart, _currentIndent + _colonStack);
                CodeBox.CaretIndex = caretIndex + _currentIndent.Length + _colonStack.Length;
                _colonStack = "";
                _insertIndent = false;
            }
            if (!_completed && IsLetter(e.Key)) {
                GenerateItems();
                popup.PlacementTarget = CodeBox;
                popup.PlacementRectangle = CodeBox.GetRectFromCharacterIndex(CodeBox.CaretIndex);
            } else
                _completed = false;
        }

        private void CodeBoxSelectionChanged(object sender, RoutedEventArgs e) {
            var temp = new List<char>();
            var text = CodeBox.Text;
            var parenthesesIndex = CodeBox.CaretIndex - 1; //括弧インデックス //parentheses index
            while (parenthesesIndex > 0)
                if (text[parenthesesIndex--] == '(')//括弧見つけた！ //Found parentheses
                {
                    for (; parenthesesIndex > -1; parenthesesIndex--)
                        if (Tokens.Contains(text[parenthesesIndex]))
                            break;
                        else
                            temp.Add(text[parenthesesIndex]);//一個一個後ろに進みつつ、文字を手に入れる
                    break;
                } else if (text[parenthesesIndex] == ' ' || text[parenthesesIndex] == '\n' || text[parenthesesIndex] == '\r' || text[parenthesesIndex] == '\t' || text[parenthesesIndex] == ')')
                    //もし行のはじめ、閉じ括弧、空白に到達したら走査終了
                    break;
            //文字は逆向きなので反対に直す //The characters are backwards so reverse them to fix it
            temp.Reverse();
            var ca = new String(temp.ToArray());

            //テキストボックス頭の文字列でなければ走査したものを、そうでなければ現在の文字列を得る
            var word = ca != "clr.AddReference" && !String.IsNullOrEmpty(ca) ? ca : GetCurrentWord(CodeBox);
            if (!string.IsNullOrEmpty(word))
                foreach (var pair in _descriptions.Where(pair => pair.Key.Contains(word))) {
                    descriptionLabel.Content = pair.Value;
                    return;
                }
            descriptionLabel.Content = null;
        }

        private void UserControlLoaded(object sender, RoutedEventArgs e) {
            //ステータスバーのラベルの大きさを調整 //Status bar size adjustment
            descriptionLabel.Width = Width;
        }
    }
}

