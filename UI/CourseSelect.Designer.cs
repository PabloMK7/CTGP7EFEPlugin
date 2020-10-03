namespace CTGP7.UI
{
    partial class CourseSelect
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
            this.trackTypeBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.trackNameBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // trackTypeBox
            // 
            this.trackTypeBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.trackTypeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.trackTypeBox.FormattingEnabled = true;
            this.trackTypeBox.Location = new System.Drawing.Point(43, 0);
            this.trackTypeBox.Name = "trackTypeBox";
            this.trackTypeBox.Size = new System.Drawing.Size(133, 21);
            this.trackTypeBox.TabIndex = 0;
            this.trackTypeBox.SelectedIndexChanged += new System.EventHandler(this.trackTypeBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Type:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(182, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Name:";
            // 
            // trackNameBox
            // 
            this.trackNameBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackNameBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.trackNameBox.FormattingEnabled = true;
            this.trackNameBox.Location = new System.Drawing.Point(226, 0);
            this.trackNameBox.Name = "trackNameBox";
            this.trackNameBox.Size = new System.Drawing.Size(152, 21);
            this.trackNameBox.Sorted = true;
            this.trackNameBox.TabIndex = 3;
            this.trackNameBox.SelectedIndexChanged += new System.EventHandler(this.trackNameBox_SelectedIndexChanged);
            this.trackNameBox.TextChanged += new System.EventHandler(this.trackNameBox_TextChanged);
            // 
            // CourseSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.trackNameBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackTypeBox);
            this.Name = "CourseSelect";
            this.Size = new System.Drawing.Size(378, 21);
            this.Load += new System.EventHandler(this.CourseSelect_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox trackTypeBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox trackNameBox;
    }
}
