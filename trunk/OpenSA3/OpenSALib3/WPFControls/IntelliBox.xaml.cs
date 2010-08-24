using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using OpenSALib3;
using System.Reflection;
using System.Runtime.InteropServices;
namespace System.Windows.Controls
{
    /// <summary>
    /// IntelListBox.xaml の相互作用ロジック
    /// </summary>
    /// 
    public partial class IntelliBox:System.Windows.Controls.UserControl
    {
        private static readonly string[] keywords = 
        { "and","as","assert","break","class","continue","def","elif","else","except","finally","for","from",
            "global","False","if","import","in","is","lambda","None","nonlocal","not","or","pass","raise","return",
            "Truetry","while","with","yeild"};
        private static readonly char[] tokens = {'(',')',',',';',':','[',']','{','}',' ','\n','\r','\t'};
        private bool completed = false;
        private bool insertIndent = false;
        private string currentIndent = "";
        private string colonStack = "";
        private Dictionary<string, string> descriptions = new Dictionary<string, string>();

        private List<string> _targetSource=new List<string>();
        public List<string> TargetSource { get { return _targetSource; } set { _targetSource = value; } }
        public string Text { get { return CodeBox.Text; } set { CodeBox.Text = value; } }
        private bool useIntelliSense = true;
        public bool UseIntelliSense { get { return useIntelliSense; } set { useIntelliSense = value; } }

        public IntelliBox()
        {
            InitializeComponent();
            GetMembers();
        }
        /// <summary>
        /// This gets current word.
        /// </summary>
        /// <param name="textBox"></param>
        /// <returns></returns>
        private static string GetCurrentWord(TextBox textBox)
        {
            if (string.IsNullOrEmpty(textBox.Text))
            {
                return string.Empty;
            }
            if (textBox.CaretIndex == 0)
            {
                return string.Empty;
            }

            int index = textBox.CaretIndex - 1;
            int space = textBox.Text.LastIndexOfAny(tokens, index)+1;
            int plus = textBox.Text.IndexOfAny(tokens, index);
            return textBox.Text.Substring(space,plus-space>0? plus - space:textBox.CaretIndex-space);
        }

        private bool IsComment()
        {
            if (CodeBox.Text.Length > 0)
            {
                for (int i = CodeBox.CaretIndex-1; i >= 0; i--)
                    if (CodeBox.Text[i] == '\n')
                        return false;
                    else if (CodeBox.Text[i] == '#')
                        return true;
            }

            return false;
        }
        /// <summary>
        /// Is this key a letter key?
        /// </summary>
        /// <param name="e">pressed key data</param>
        /// <returns>Letter or not</returns>
        private bool IsLetter(Key e)
        {
            if ((e>= Key.A && e<= Key.Z) || (e>= Key.NumPad0 && e<= Key.NumPad9) || (e>= Key.D0 && e<= Key.D9))
                return true;
            return false;
        }

        /// <summary>
        /// Generates ListViewItem
        /// </summary>
        private void GenerateItems()
        {
            ListView.Items.Clear();
            string currentWord = GetCurrentWord(CodeBox);
            string longestWord="";
            if (!IsComment())
            {
                if (!string.IsNullOrWhiteSpace(currentWord))
                    foreach (string s in _targetSource.Where(x => x.StartsWith(currentWord)&&x!=currentWord))
                    {
                        ToolTip tip = new Controls.ToolTip() { Content = descriptions.ContainsKey(s) ? descriptions[s] : null };
                        ListView.Items.Add(new ListViewItem() { Content = s, ToolTip = tip });
                        if (s.Length > longestWord.Length)
                            longestWord = s;
                    }
                if (ListView.Items.Count > 0)
                {
                    ListView.SelectedIndex = 0;
                    popup.IsOpen = true;
                    ListView.Width = longestWord.Length * 15;
                }
                else
                    popup.IsOpen = false;
            }
        }

        private void SelectItem()
        {
            if (ListView.SelectedItems.Count > 0)
            {
                var textBox = popup.PlacementTarget as TextBox;
                var caretIndex = textBox.CaretIndex;
                var currentWord = GetCurrentWord(textBox);
                var selectedText = ((ListViewItem)ListView.SelectedItem).Content as string;

                // 選択されたものを挿入
                var tmpText = textBox.Text.Remove(caretIndex - currentWord.Length, currentWord.Length);
                textBox.Text = tmpText.Insert(caretIndex - currentWord.Length, selectedText);
                textBox.CaretIndex = caretIndex + selectedText.Length;
                popup.IsOpen = false;
            }
        }

        private void GetMembers()
        {
            //Get PSAName
            Type t = typeof(CommandReceiver);
            MethodInfo[] info = t.GetMethods();
            foreach (MethodInfo i in info)
                if (i.IsPublic && !i.Name.Contains("set_") && !i.Name.Contains("get_") && i.DeclaringType == typeof(CommandReceiver))
                {
                    _targetSource.Add(i.Name); descriptions.Add(i.Name, i.Name + "(");
                    ParameterInfo[] param = i.GetParameters();
                    foreach (ParameterInfo paraminfo in param)
                        descriptions[i.Name] += paraminfo.ParameterType.Name+ " " + paraminfo.Name+",";
                    descriptions[i.Name] += ")";
                }

            foreach (string s in keywords)
                _targetSource.Add(s);
        }

        private void TextBox_PreviewKeyDown(object sender, Input.KeyEventArgs e)
        {
            if (UseIntelliSense)
            {
                Action UpKeyPressed = () =>
                {
                    if (popup.IsOpen)
                    {
                        if (ListView.SelectedIndex >= 1)
                            ListView.SelectedIndex = ListView.SelectedIndex - 1;
                        e.Handled = true; ((ListViewItem)ListView.SelectedItem).Focus();
                    }
                };
                Action DownKeyPressed = () =>
                {
                    if (popup.IsOpen)
                    {
                        if (ListView.SelectedIndex >= 0 && ListView.SelectedIndex < ListView.Items.Count - 1)
                        {
                            ListView.SelectedIndex = ListView.SelectedIndex + 1;
                            e.Handled = true; ((ListViewItem)ListView.SelectedItem).Focus();
                        }
                    }
                };
                Action EnterKeyPressed = () =>
                {
                    if (popup.IsOpen)
                    {
                        //アイテムを選んだ、ということなので行変えを無効にする
                        CodeBox.AcceptsReturn = false;
                        SelectItem();
                        ListView.Items.Clear();
                        completed = true;
                    }
                    else
                    {
                        //アイテムを選んで無いときは行変えを有効にする
                        CodeBox.AcceptsReturn = true;
                        //こっから先、インデントの文字数を取得
                        insertIndent = true;
                        currentIndent = "";
                        int i = CodeBox.SelectionStart - 1;
                        if (i >= 0 && CodeBox.Text.Length != 0)
                        {
                            while (CodeBox.Text[i] != '\n')//search \n
                            {
                                i--;
                                if (i <= 0)
                                {
                                    i = 0; break;
                                }
                                if (CodeBox.Text[i] == '\n')
                                {
                                    i++; break;
                                }
                            }
                           while (CodeBox.Text[i] == ' ')
                           {
                               currentIndent += " "; i++;
                               if (i == CodeBox.Text.Length || CodeBox.Text[i] != ' ')
                                   break;
                           }
                        }
                        else
                            currentIndent = "";

                    }
                };
                Action SpaceKeyPressed = () =>
                {
                    if (popup.IsOpen)
                    {
                        CodeBox.AcceptsReturn = false;
                        SelectItem();
                        ListView.Items.Clear();
                        completed = true;
                    }
                };
                Action Defaultcase = () =>
                {
                };
                Action BackKeyPressed = () =>
                {
                };
                Action ColonKeyPressed = () =>
                    {
                        colonStack += "    ";
                    };
                switch (e.Key)
                {
                    case Key.OemSemicolon: ColonKeyPressed(); break;
                    case Key.Space: popup.IsOpen = false; SpaceKeyPressed(); break;
                    case Key.Up: UpKeyPressed(); break;
                    case Key.Down: DownKeyPressed(); break;
                    case Key.Back: BackKeyPressed(); break;
                    case Key.Enter: EnterKeyPressed(); break;
                    default: Defaultcase(); break;
                }
            }
        }


        private void ListView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Space)
            {
                SelectItem();
                ListView.Items.Clear();
                popup.IsOpen = false;
                CodeBox.Focus();
                CodeBox.AcceptsReturn = false;
                completed = true;
            }
            else if (IsLetter(e.Key)||e.Key==Key.Back)
            {
                CodeBox.Focus();
            }
        }

        private void ListView_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectItem();
            ListView.Items.Clear();
            popup.IsOpen = false;
            CodeBox.Focus();
        }

        private void CodeBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (UseIntelliSense)
            {
                if (insertIndent)
                {
                    //インデント挿入するよ
                    int caretIndex = CodeBox.CaretIndex;
                    CodeBox.Text = CodeBox.Text.Insert(CodeBox.SelectionStart, currentIndent+colonStack);
                    CodeBox.CaretIndex = caretIndex + currentIndent.Length+colonStack.Length;
                    colonStack = "";
                    insertIndent = false;
                }
                if (!completed&&IsLetter(e.Key))
                {
                    GenerateItems();
                    popup.PlacementTarget = CodeBox;
                    popup.PlacementRectangle = CodeBox.GetRectFromCharacterIndex(CodeBox.CaretIndex);
                }
                else
                    completed = false;
            }
        }

        private void CodeBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            string word;
            List<char> temp = new List<char>();
            string ca;
            string text=CodeBox.Text;
            int kakkoIndex=CodeBox.CaretIndex-1;
            while (kakkoIndex > 0)
                if (text[kakkoIndex--] == '(')//括弧見つけた！
                {
                    for (; kakkoIndex > -1; kakkoIndex--)
                        if (tokens.Contains(text[kakkoIndex]))
                            break;
                        else
                            temp.Add(text[kakkoIndex]);//一個一個後ろに進みつつ、文字を手に入れる
                    break;
                }
                else if (text[kakkoIndex] == ' ' || text[kakkoIndex] == '\n' || text[kakkoIndex] == '\r' || text[kakkoIndex] == '\t' || text[kakkoIndex] == ')')
                    //もし行のはじめ、閉じ括弧、空白に到達したら走査終了
                    break;
            //文字は逆向きなので反対に直す
            temp.Reverse();
            ca = new String(temp.ToArray());

            //テキストボックス頭の文字列でなければ走査したものを、そうでなければ現在の文字列を得る
            word = ca!="clr.AddReference"&&!String.IsNullOrEmpty(ca)? ca : GetCurrentWord(CodeBox);
                if (!string.IsNullOrEmpty(word))
                    foreach (var pair in descriptions)
                        if (pair.Key.Contains(word))
                        {
                            descriptionLabel.Content = pair.Value;
                            return;
                        }
                descriptionLabel.Content = null;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //ステータスバーのラベルの大きさを調整
            descriptionLabel.Width = this.Width;
        }
    }
}

