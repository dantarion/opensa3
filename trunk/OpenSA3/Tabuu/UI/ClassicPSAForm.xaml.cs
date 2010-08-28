using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OpenSALib3;
using OpenSALib3.Utility;
using OpenSALib3.PSA;
namespace Tabuu.UI
{

    struct DataHolder
    {
        public string value;
        public ParameterType type;
    }

    /// <summary>
    /// ClassicPSAForm.xaml の相互作用ロジック
    /// </summary>
    public partial class ClassicPSAForm : Window
    {
        private Dictionary<string, PSANames.EventData> EventNames;
        public static Dictionary<int, string> ReqNames;
        bool initialized;
        private string _result=null;
        public string Result { get { return _result; } }

        public ClassicPSAForm()
        {
            InitializeComponent();
            initialized = true;
            PSANames.LoadData();
            EventNames = PSANames.EventNames;
            ReqNames = PSANames.ReqNames;
            Initialize();
        }

        private void Initialize()
        {
            List<string> eventNameList = new List<string>();
            List<string> keys = new List<string>();
            foreach (var pair in EventNames.OrderBy(x => x.Key))
            {
                eventNameList.Add(pair.Value.Name);
                keys.Add(pair.Key);
            }
            for(int i=0;i<eventNameList.Count;i++)
            {
                    CommandsBox.Items.Add(new ComboBoxItem() { Content=eventNameList[i],Tag=EventNames[keys[i]]});
            }
        }

        private void CommandsBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ParamBox.Items.Clear();
            var data = CommandsBox.SelectedItem as ComboBoxItem;
            foreach (var d in ((PSANames.EventData)data.Tag).ParamNames)
                ParamBox.Items.Add(new ListBoxItem() { Content=d});
            NameBox.Text = data.Content as string;
            DesBox.Text = ((PSANames.EventData)data.Tag).Description;
            foreach (var pair in EventNames)
                if (pair.Value == data.Tag)
                    RawValueBox.Text = pair.Key;
        }

        private void ValueTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (initialized)
            {
                Action AllInvisible = () =>
                    {
                        ValueLabel.Visibility = Visibility.Hidden;
                        BooleanBox.Visibility = Visibility.Hidden;
                        ValueBox.Visibility = Visibility.Hidden;
                        LongBox.Visibility = Visibility.Hidden;
                        LongLabel.Visibility = Visibility.Hidden;
                        Type2Label.Visibility = Visibility.Hidden;
                        Type2Box.Visibility = Visibility.Hidden;
                        varLabel.Visibility = Visibility.Hidden;
                        varBox.Visibility = Visibility.Hidden;
                        NotCheck.Visibility = Visibility.Hidden;
                    };

                Action ValueSelected = () =>
                    {
                        ValueLabel.Visibility = Visibility.Visible;
                        ValueBox.Visibility = Visibility.Visible;
                        ValueLabel.Text = "Value:";
                    };
                Action ScalarSelected = () =>
                    {
                        ValueSelected();
                    };
                Action PointerSelected = () =>
                    {
                        ValueSelected();
                        ValueLabel.Text = "Address:";
                    };
                Action BooleanSelected = () =>
                    {
                        ValueLabel.Visibility = Visibility.Visible;
                        BooleanBox.Visibility = Visibility.Visible;
                    };
                Action FourSelected = () =>
                    {
                        ValueSelected();
                    };
                Action VariableSelected = () =>
                    {
                        LongLabel.Visibility = Visibility.Visible;
                        LongBox.Visibility = Visibility.Visible;
                        Type2Box.Visibility = Visibility.Visible;
                        Type2Label.Visibility = Visibility.Visible;
                        varLabel.Visibility = Visibility.Visible;
                        varBox.Visibility = Visibility.Visible;

                        LongLabel.Text = "Memory Type:";
                        LongBox.Items.Clear();
                        LongBox.Items.Add(new ComboBoxItem() { Content = "Internal Constant" });
                        LongBox.Items.Add(new ComboBoxItem() { Content = "Runtime Longterm" });
                        LongBox.Items.Add(new ComboBoxItem() { Content = "Random Access" });
                    };
                Action RequirementSelected = () =>
                    {
                        LongBox.Visibility = Visibility.Visible;
                        LongLabel.Visibility = Visibility.Visible;
                        NotCheck.Visibility = Visibility.Visible;

                        LongLabel.Text = "Requirement:";
                        LongBox.Items.Clear();
                        foreach (var item in ReqNames)
                            LongBox.Items.Add(new ComboBoxItem() { Content = item.Value });
                    };

                AllInvisible();
                switch (((ComboBoxItem)ValueTypeBox.SelectedItem).Content as string)
                {
                    case "Value": ValueSelected(); break;
                    case "Scalar": ScalarSelected(); break;
                    case "Pointer": PointerSelected(); break;
                    case "Boolean": BooleanSelected(); break;
                    case "(4)": FourSelected(); break;
                    case "Variable": VariableSelected(); break;
                    case "Requirement": RequirementSelected(); break;
                }
            }
        }

        private void DoneBtn_Click(object sender, RoutedEventArgs e)
        {
            var list=new List<object>();
            foreach(ListBoxItem n in ParamBox.Items)
                list.Add(((DataHolder)n.Tag).value);
            _result = String.Format(((PSANames.EventData)((ComboBoxItem)CommandsBox.SelectedItem).Tag).FormatString, list.ToArray());
        }

        

        private void ParamBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ParamBox.SelectedIndex >= 0)
            {
                DataHolder holder = new DataHolder();
                switch (((ComboBoxItem)ValueTypeBox.SelectedItem).Content as string)
                {
                    case "Value": holder.value = ValueBox.Text; holder.type = ParameterType.Value; break;
                    case "Scalar": holder.value = ValueBox.Text; holder.type = ParameterType.Scalar; break;
                    case "Boolean": holder.value = ((ComboBoxItem)BooleanBox.SelectedItem).Content as string; holder.type = ParameterType.Boolean; break;
                    case "(4)": holder.value = ValueBox.Text; break;
                    case "Pointer": holder.value = ValueBox.Text; holder.type = ParameterType.Offset; break;
                    case "Variable": holder.type = ParameterType.Variable; break;
                    case "Requirement":
                        {
                            holder.value = LongBox.SelectedIndex.ToString("X"); holder.type = ParameterType.Requirement;
                        }
                        break;
                }
                ((ListBoxItem)ParamBox.SelectedItem).Tag = holder;
            }
        }

        private void ParamBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ParamBox.SelectedIndex >= 0)
            {
                if (((ListBoxItem)ParamBox.SelectedItem).Tag != null)
                {
                    DataHolder holder = (DataHolder)((ListBoxItem)ParamBox.SelectedItem).Tag;
                    switch (holder.type)
                    {
                        case ParameterType.Value: ValueBox.Text = holder.value; ValueTypeBox.SelectedIndex = 0; break;
                        case ParameterType.Scalar: ValueBox.Text = holder.value; ValueTypeBox.SelectedIndex = 1; break;
                        case ParameterType.Offset: ValueBox.Text = holder.value; ValueTypeBox.SelectedIndex = 2; break;
                        case ParameterType.Boolean: BooleanBox.SelectedIndex = holder.value == "True" ? 0 : 1; ValueTypeBox.SelectedIndex = 3; break;
                        case ParameterType.Variable: ValueTypeBox.SelectedIndex = 5; break;
                        case ParameterType.Requirement: LongBox.SelectedIndex = Convert.ToInt32(holder.value,16); ValueTypeBox.SelectedIndex = 6; break;
                    }
                }
            }
        }


    }
}
