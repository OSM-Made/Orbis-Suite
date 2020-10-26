namespace OrbisConsoleOutput.Controls
{
    partial class OutputControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Output = new DarkUI.Controls.DarkRichTextBox();
            this.darkScrollBar1 = new DarkUI.Controls.DarkScrollBar();
            this.SuspendLayout();
            // 
            // Output
            // 
            this.Output.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.Output.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Output.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Output.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Output.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.Output.HScrollPos = 0;
            this.Output.Location = new System.Drawing.Point(0, 0);
            this.Output.Name = "Output";
            this.Output.Size = new System.Drawing.Size(1042, 722);
            this.Output.TabIndex = 0;
            this.Output.Text = "";
            this.Output.VScrollPos = 0;
            this.Output.Scroll += new System.EventHandler<DarkUI.Controls.MyScrollEvent>(this.Output_Scroll);
            this.Output.TextChanged += new System.EventHandler(this.Output_TextChanged);
            this.Output.Resize += new System.EventHandler(this.Output_Resize);
            // 
            // darkScrollBar1
            // 
            this.darkScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.darkScrollBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.darkScrollBar1.Location = new System.Drawing.Point(1025, 0);
            this.darkScrollBar1.Name = "darkScrollBar1";
            this.darkScrollBar1.Size = new System.Drawing.Size(17, 722);
            this.darkScrollBar1.TabIndex = 1;
            this.darkScrollBar1.Text = "darkScrollBar1";
            this.darkScrollBar1.ValueChanged += new System.EventHandler<DarkUI.Controls.ScrollValueEventArgs>(this.darkScrollBar1_ValueChanged);
            // 
            // OutputControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.darkScrollBar1);
            this.Controls.Add(this.Output);
            this.Name = "OutputControl";
            this.Size = new System.Drawing.Size(1042, 722);
            this.ResumeLayout(false);

        }

        #endregion

        private DarkUI.Controls.DarkRichTextBox Output;
        private DarkUI.Controls.DarkScrollBar darkScrollBar1;
    }
}
