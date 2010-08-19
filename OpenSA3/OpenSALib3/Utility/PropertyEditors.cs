using System.Activities.Presentation.PropertyEditing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Markup;

namespace OpenSALib3.Utility
{
    public class HexPropertyEditor : PropertyValueEditor 
    {
        public HexPropertyEditor() {
            const string template1 = @"
                        <DataTemplate
                    xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'
                    xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'
                    xmlns:pe='clr-namespace:System.Activities.Presentation.PropertyEditing;assembly=System.Activities.Presentation'>
                    <DockPanel LastChildFill='True'>
                    <TextBox IsReadOnly='True' Text='{Binding StringFormat=X,Path=Value}' />
                    </DockPanel>
                    </DataTemplate>";
            using (var sr = new MemoryStream(Encoding.UTF8.GetBytes(template1)))
            {
                InlineEditorTemplate = XamlReader.Load(sr) as DataTemplate;
            }
        }
    }
}
