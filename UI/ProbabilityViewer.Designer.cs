using System;
using System.Windows.Forms;

namespace CTGP7.UI
{
    partial class ProbabilityViewer<T>
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainDataView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.mainDataView)).BeginInit();
            this.SuspendLayout();
            // 
            // mainDataView
            // 
            this.mainDataView.AllowUserToAddRows = false;
            this.mainDataView.AllowUserToDeleteRows = false;
            this.mainDataView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainDataView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.mainDataView.Location = new System.Drawing.Point(0, 0);
            this.mainDataView.Name = "mainDataView";
            this.mainDataView.Size = new System.Drawing.Size(150, 150);
            this.mainDataView.TabIndex = 0;
            this.mainDataView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.MainDataView_CellEndEdit);
            this.mainDataView.Paint += new System.Windows.Forms.PaintEventHandler(this.mainDataView_Paint);
            // 
            // ProbabilityViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainDataView);
            this.Name = "ProbabilityViewer";
            ((System.ComponentModel.ISupportInitialize)(this.mainDataView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView mainDataView;
    }
}
