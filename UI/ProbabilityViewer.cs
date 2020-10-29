using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CTGP7.UI
{
    public partial class ProbabilityViewer<T> : UserControl
    {
        private DataTable mainTable;
        private List<string> RowNames;
        private List<string> ColumnNames;
        private T[][] Data;
        public ProbabilityViewer()
        {
            InitializeComponent();
        }

        public void PopulateData(List<string> rowNames, List<string> columnNames, T[][] data)
        {
            if (data.Length == 0 || rowNames.Count != data.Length || data[0].Length == 0 || columnNames.Count != data[0].Length)
            {
                throw new ArgumentException("Data size is zero or data and names size mismatch.");
            }

            RowNames = rowNames.Clone();
            ColumnNames = columnNames.Clone();

            int size = data[0].Length;
            Data = new T[data.Length][];
            for (int i = 0; i < data.Length; i++)
            {
                if (size != data[i].Length) throw new ArgumentException("Data column size not constant for each row.");
                Data[i] = new T[data[i].Length];
                for (int j = 0; j < data[i].Length; j++)
                {
                    Data[i][j] = (T)data[i][j];
                }
            }

            populateTable();
        }

        private void populateTable()
        {
            mainDataView.DataSource = null;
            mainTable = new DataTable();

            mainDataView.RowTemplate.DefaultHeaderCellType = typeof(CustomHeaderCell);
            mainDataView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            mainDataView.RowHeadersDefaultCellStyle.Padding = new Padding(2);

            Type columnType = typeof(T);
            for (int i = 0; i < ColumnNames.Count; i++)
            {
                mainTable.Columns.Add(ColumnNames[i], columnType);
            }
            for (int i = 0; i < RowNames.Count; i++)
            {
                object[] currRow = new object[ColumnNames.Count];
                for (int j = 0; j < ColumnNames.Count; j++)
                {
                    currRow[j] = Data[i][j];
                }
                mainTable.Rows.Add(currRow);
            }
            mainDataView.DataSource = mainTable;
            for (int i = 0; i < RowNames.Count; i++)
            {
                mainDataView.Rows[i].HeaderCell.Value = RowNames[i];
            }
        }
        public T[][] Values { 
            get
            {
                T[][] ret = new T[Data.Length][];
                for (int i = 0; i < ret.Length; i++)
                {
                    ret[i] = new T[Data[i].Length];
                    for (int j = 0; j < ret[i].Length; j++)
                    {
                        ret[i][j] = Data[i][j];
                    }
                }
                return ret;
            }
        }
        public T Get(int rowID, int columnID)
        {
            mainDataView.EndEdit();
            return Data[rowID][columnID];
        }

        public void Set(int rowID, int columnID, T data)
        {
            mainDataView.EndEdit();
            Data[rowID][columnID] = data;
            mainDataView.Rows[rowID].Cells[columnID].Value = Data[rowID][columnID];
        }
        public void SetRowNames(List<string> newNames)
        {
            if (RowNames.Count != newNames.Count) throw new ArgumentException("Names size mismatch.");
            for (int i = 0; i < newNames.Count; i++)
            {
                RowNames[i] = (string)newNames[i].Clone();
            }
            populateTable();
        }
        public void SetRowName(int rowID, string name)
        {
            RowNames[rowID] = (string)name.Clone();
        }
        public void SetColumnName(int columnID, string name)
        {
            ColumnNames[columnID] = (string)name.Clone();
        }
        public void UpdateNames()
        {
            populateTable();
        }

        public class CustomHeaderCell : DataGridViewRowHeaderCell //Really...
        {
            protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
            {
                var size1 = base.GetPreferredSize(graphics, cellStyle, rowIndex, constraintSize);
                var value = string.Format("{0}", this.DataGridView.Rows[rowIndex].HeaderCell.FormattedValue);
                var size2 = TextRenderer.MeasureText(value, cellStyle.Font);
                var padding = cellStyle.Padding;
                return new Size(size2.Width + padding.Left + padding.Right, size1.Height);
            }
            protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
            {
                base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, DataGridViewPaintParts.Background);
                base.PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
                TextRenderer.DrawText(graphics, string.Format("{0}", formattedValue), cellStyle.Font, cellBounds, cellStyle.ForeColor);
            }
        }

        private void MainDataView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Data[e.RowIndex][e.ColumnIndex] = (T)mainDataView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
        }
    }

    public class ByteProbabilityViewer : ProbabilityViewer<byte> { }

    static class CloneExtension
    {
        public static List<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}
