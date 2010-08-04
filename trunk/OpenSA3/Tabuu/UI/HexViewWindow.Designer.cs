namespace Tabuu.UI {
    namespace UI {
        sealed partial class HexViewWindow {
            /// <summary>
            /// Required designer variable.
            /// </summary>
            private System.ComponentModel.IContainer components = null;

            /// <summary>
            /// Clean up any resources being used.
            /// </summary>
            /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
            protected override void Dispose(bool disposing) {
                if (disposing && (components != null)) {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }

            #region Windows Form Designer generated code

            /// <summary>
            /// Required method for Designer support - do not modify
            /// the contents of this method with the code editor.
            /// </summary>
            private void InitializeComponent() {
                this.hexBox1 = new Be.Windows.Forms.HexBox();
                this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
                this.SuspendLayout();
                // 
                // hexBox1
                // 
                this.hexBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                this.hexBox1.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.hexBox1.LineInfoForeColor = System.Drawing.Color.Empty;
                this.hexBox1.LineInfoVisible = true;
                this.hexBox1.Location = new System.Drawing.Point(0, 0);
                this.hexBox1.Name = "hexBox1";
                this.hexBox1.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
                this.hexBox1.Size = new System.Drawing.Size(630, 345);
                this.hexBox1.TabIndex = 0;
                this.hexBox1.UseFixedBytesPerLine = true;
                this.hexBox1.VScrollBarVisible = true;
                // 
                // flowLayoutPanel1
                // 
                this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
                this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 351);
                this.flowLayoutPanel1.Name = "flowLayoutPanel1";
                this.flowLayoutPanel1.Size = new System.Drawing.Size(630, 20);
                this.flowLayoutPanel1.TabIndex = 1;
                // 
                // HexViewWindow
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.ClientSize = new System.Drawing.Size(630, 371);
                this.Controls.Add(this.flowLayoutPanel1);
                this.Controls.Add(this.hexBox1);
                this.Name = "HexViewWindow";
                this.Text = "Hex View - ";
                this.ResumeLayout(false);

            }

            #endregion

            private Be.Windows.Forms.HexBox hexBox1;
            private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        }
    }
}