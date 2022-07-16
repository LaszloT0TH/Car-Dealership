using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace InputForms
{
    internal class DispalyTable : InputField
    {
        DataGridView dataGridView;

        PrintPreviewDialog printPreviewDialog;

        PrintDocument printDocument;

        ComponentResourceManager resources;

        Bitmap bitmap;

        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(bitmap, 0, 0);
        }

        public void Print()
        {
            resources = new ComponentResourceManager(typeof(Car_Dealership.Query));

            printPreviewDialog = new PrintPreviewDialog();

            printDocument = new PrintDocument();

            printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);

            printPreviewDialog.AutoScrollMargin = new Size(0, 0);
            printPreviewDialog.AutoScrollMinSize = new Size(0, 0);
            printPreviewDialog.ClientSize = new Size(400, 300);
            printPreviewDialog.Document = printDocument;
            printPreviewDialog.Enabled = true;
            printPreviewDialog.Icon = ((Icon)(resources.GetObject("printPreviewDialog.Icon")));
            printPreviewDialog.Name = "printPreviewDialog";
            printPreviewDialog.Visible = false;

            int height = dataGridView.Height;
            dataGridView.Height = dataGridView.RowCount * dataGridView.RowTemplate.Height * 2;

            bitmap = new Bitmap(dataGridView.Width, dataGridView.Height);
            dataGridView.DrawToBitmap(bitmap, new Rectangle(0, 0, dataGridView.Width, dataGridView.Height));

            printPreviewDialog.PrintPreviewControl.Zoom = 1;
            printPreviewDialog.ShowDialog();
            dataGridView.Height = height;
        }

        public DispalyTable(string text, string[] options, Control parent = null) : base(text, parent)
        {
            Items(options);
            (input as DataGridView).TabIndex = 1;
        }

        public DispalyTable Add(object[] rows)
        {
            foreach (string[] rowArray in rows)
            {
                dataGridView.Rows.Add(rowArray);
            }
            return this;
        }

        private void Items(string[] options)
        {
            for (int i = 1; i < options.Length + 1; i++)
            {
                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();

                dataGridView.Columns.AddRange(new DataGridViewColumn[] { column });

                column.HeaderText = options[i - 1];
                column.Name = "column" + i.ToString();
                column.MinimumWidth = 6;
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                column.Resizable = DataGridViewTriState.True;
                column.ReadOnly = true;
            }
        }

        protected override Control CreateField()
        {
            dataGridView = new DataGridView();
            dataGridView.Size = new Size(1126, 643);
            dataGridView.Location = new Point(10, 10);
            dataGridView.Margin = new Padding(4, 4, 4, 4);
            dataGridView.ReadOnly = true;
            dataGridView.RowHeadersWidth = 51;
            dataGridView.ScrollBars = ScrollBars.Both;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            return dataGridView;
        }
    }
}
