namespace OrbisDebugger.Controls
{
    partial class ScratchPad
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
            this.ScratchPadTextBox = new DarkUI.Controls.DarkRichTextBox();
            this.SuspendLayout();
            // 
            // ScratchPadTextBox
            // 
            this.ScratchPadTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.ScratchPadTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ScratchPadTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScratchPadTextBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScratchPadTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ScratchPadTextBox.HScrollPos = 0;
            this.ScratchPadTextBox.Location = new System.Drawing.Point(0, 0);
            this.ScratchPadTextBox.Name = "ScratchPadTextBox";
            this.ScratchPadTextBox.Size = new System.Drawing.Size(940, 586);
            this.ScratchPadTextBox.TabIndex = 0;
            this.ScratchPadTextBox.Text = "";
            this.ScratchPadTextBox.VScrollPos = 0;
            // 
            // ScratchPad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ScratchPadTextBox);
            this.Name = "ScratchPad";
            this.SerializationKey = "ScratchPad";
            this.Size = new System.Drawing.Size(940, 586);
            this.ResumeLayout(false);

        }

        #endregion

        private DarkUI.Controls.DarkRichTextBox ScratchPadTextBox;
    }
}
