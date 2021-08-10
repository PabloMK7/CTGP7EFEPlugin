
namespace CTGP7.UI
{
    partial class MissionRichText
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
            this.changeColorButton = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.boldCheck = new System.Windows.Forms.CheckBox();
            this.italicCheck = new System.Windows.Forms.CheckBox();
            this.underlineCheck = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // changeColorButton
            // 
            this.changeColorButton.BackColor = System.Drawing.Color.White;
            this.changeColorButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.changeColorButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.changeColorButton.Location = new System.Drawing.Point(238, 4);
            this.changeColorButton.Name = "changeColorButton";
            this.changeColorButton.Size = new System.Drawing.Size(23, 23);
            this.changeColorButton.TabIndex = 19;
            this.changeColorButton.UseVisualStyleBackColor = false;
            this.changeColorButton.Click += new System.EventHandler(this.changeColorButton_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.ForeColor = System.Drawing.Color.White;
            this.richTextBox1.Location = new System.Drawing.Point(3, 33);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(270, 30);
            this.richTextBox1.TabIndex = 14;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // colorDialog1
            // 
            this.colorDialog1.Color = System.Drawing.Color.White;
            // 
            // boldCheck
            // 
            this.boldCheck.AutoSize = true;
            this.boldCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boldCheck.Location = new System.Drawing.Point(3, 8);
            this.boldCheck.Name = "boldCheck";
            this.boldCheck.Size = new System.Drawing.Size(51, 17);
            this.boldCheck.TabIndex = 20;
            this.boldCheck.Text = "Bold";
            this.boldCheck.UseVisualStyleBackColor = true;
            this.boldCheck.CheckedChanged += new System.EventHandler(this.boldCheck_CheckedChanged);
            // 
            // italicCheck
            // 
            this.italicCheck.AutoSize = true;
            this.italicCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.italicCheck.Location = new System.Drawing.Point(60, 8);
            this.italicCheck.Name = "italicCheck";
            this.italicCheck.Size = new System.Drawing.Size(48, 17);
            this.italicCheck.TabIndex = 21;
            this.italicCheck.Text = "Italic";
            this.italicCheck.UseVisualStyleBackColor = true;
            this.italicCheck.CheckedChanged += new System.EventHandler(this.italicCheck_CheckedChanged);
            // 
            // underlineCheck
            // 
            this.underlineCheck.AutoSize = true;
            this.underlineCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.underlineCheck.Location = new System.Drawing.Point(114, 8);
            this.underlineCheck.Name = "underlineCheck";
            this.underlineCheck.Size = new System.Drawing.Size(71, 17);
            this.underlineCheck.TabIndex = 22;
            this.underlineCheck.Text = "Underline";
            this.underlineCheck.UseVisualStyleBackColor = true;
            this.underlineCheck.CheckedChanged += new System.EventHandler(this.underlineCheck_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(197, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Color:";
            // 
            // MissionRichText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.underlineCheck);
            this.Controls.Add(this.italicCheck);
            this.Controls.Add(this.boldCheck);
            this.Controls.Add(this.changeColorButton);
            this.Controls.Add(this.richTextBox1);
            this.Name = "MissionRichText";
            this.Size = new System.Drawing.Size(276, 66);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button changeColorButton;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.CheckBox boldCheck;
        private System.Windows.Forms.CheckBox italicCheck;
        private System.Windows.Forms.CheckBox underlineCheck;
        private System.Windows.Forms.Label label1;
    }
}
