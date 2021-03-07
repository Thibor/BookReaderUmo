using System;
using RapBookUci;
using System.Windows.Forms;

namespace NSProgram
{
	public partial class FormBook : Form
	{
		readonly CBookUmo Book = new CBookUmo();

		public FormBook()
		{
			InitializeComponent();
		}

		void ShowRecords()
		{
			tsslRecords.Text = $"Records {Book.moves.Count}";
		}

		private void ButLoad_Click(object sender, EventArgs e)
		{
			openFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
			DialogResult result = openFileDialog1.ShowDialog();
			if (result == DialogResult.OK)
			{
				tbBook.Text = openFileDialog1.FileName;
				Book.FileAdd(tbBook.Text);
				ShowRecords();
				MessageBox.Show("The book has been loaded");
			}
		}

		private void ButSave_Click(object sender, EventArgs e)
		{
			saveFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
			DialogResult result = saveFileDialog1.ShowDialog();
			if (result == DialogResult.OK)
			{
				tbBook.Text = saveFileDialog1.FileName;
				Book.Save(tbBook.Text);
				MessageBox.Show("The book has been saved");
			}
		}

		private void ButClear_Click(object sender, EventArgs e)
		{
			Book.Clear();
			ShowRecords();
		}
	}
}
