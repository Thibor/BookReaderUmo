namespace NSProgram
{
	partial class FormBook
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tbBook = new System.Windows.Forms.TextBox();
			this.butLoad = new System.Windows.Forms.Button();
			this.butSave = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.butClear = new System.Windows.Forms.Button();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.tsslRecords = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tbBook
			// 
			this.tbBook.Dock = System.Windows.Forms.DockStyle.Top;
			this.tbBook.Location = new System.Drawing.Point(0, 0);
			this.tbBook.Name = "tbBook";
			this.tbBook.Size = new System.Drawing.Size(565, 20);
			this.tbBook.TabIndex = 1;
			// 
			// butLoad
			// 
			this.butLoad.Location = new System.Drawing.Point(349, 39);
			this.butLoad.Name = "butLoad";
			this.butLoad.Size = new System.Drawing.Size(94, 21);
			this.butLoad.TabIndex = 2;
			this.butLoad.Text = "Add";
			this.butLoad.UseVisualStyleBackColor = true;
			this.butLoad.Click += new System.EventHandler(this.ButLoad_Click);
			// 
			// butSave
			// 
			this.butSave.Location = new System.Drawing.Point(449, 39);
			this.butSave.Name = "butSave";
			this.butSave.Size = new System.Drawing.Size(94, 21);
			this.butSave.TabIndex = 3;
			this.butSave.Text = "Save";
			this.butSave.UseVisualStyleBackColor = true;
			this.butSave.Click += new System.EventHandler(this.ButSave_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.DefaultExt = "umo";
			this.openFileDialog1.Filter = "BookReaderUmo|*.umo|Universal Chess Interface|*.uci|Portable Game Notation|*.pgn";
			this.openFileDialog1.RestoreDirectory = true;
			// 
			// saveFileDialog1
			// 
			this.saveFileDialog1.CreatePrompt = true;
			this.saveFileDialog1.DefaultExt = "umo";
			this.saveFileDialog1.Filter = "BookReaderUmo|*.umo|Universal Chess Interface|*.uci|Portable Game Notation|*.pgn";
			// 
			// butClear
			// 
			this.butClear.Location = new System.Drawing.Point(249, 39);
			this.butClear.Name = "butClear";
			this.butClear.Size = new System.Drawing.Size(94, 21);
			this.butClear.TabIndex = 4;
			this.butClear.Text = "Clear";
			this.butClear.UseVisualStyleBackColor = true;
			this.butClear.Click += new System.EventHandler(this.ButClear_Click);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslRecords});
			this.statusStrip1.Location = new System.Drawing.Point(0, 67);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(565, 22);
			this.statusStrip1.TabIndex = 6;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// tsslRecords
			// 
			this.tsslRecords.Name = "tsslRecords";
			this.tsslRecords.Size = new System.Drawing.Size(58, 17);
			this.tsslRecords.Text = "Records 0";
			// 
			// FormBook
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(565, 89);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.butClear);
			this.Controls.Add(this.butSave);
			this.Controls.Add(this.butLoad);
			this.Controls.Add(this.tbBook);
			this.Name = "FormBook";
			this.Text = "BookReaderUmo";
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tbBook;
		private System.Windows.Forms.Button butLoad;
		private System.Windows.Forms.Button butSave;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.Button butClear;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel tsslRecords;
	}
}

