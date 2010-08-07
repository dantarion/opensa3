using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Be.Windows.Forms.Design {
    /// <summary>
    ///   Display only fixed-piched fonts
    /// </summary>
    internal class HexFontEditor : FontEditor {
        private object _value;

        /// <summary>
        ///   Edits the value
        /// </summary>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider,
                                         object value) {
            _value = value;
            if (provider != null) {
                var service1 =
                    (IWindowsFormsEditorService) provider.GetService(typeof (IWindowsFormsEditorService));
                if (service1 != null) {
                    var fontDialog =
                        new FontDialog {
                                           ShowApply = false,
                                           ShowColor = false,
                                           AllowVerticalFonts = false,
                                           AllowScriptChange = false,
                                           FixedPitchOnly = true,
                                           ShowEffects = false,
                                           ShowHelp = false
                                       };
                    var font = value as Font;
                    if (font != null)
                        fontDialog.Font = font;
                    if (fontDialog.ShowDialog() == DialogResult.OK)
                        _value = fontDialog.Font;
                    fontDialog.Dispose();
                }
            }
            value = _value;
            _value = null;
            return value; // TODO wat??
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) {
            return UITypeEditorEditStyle.Modal;
        }
    }
}