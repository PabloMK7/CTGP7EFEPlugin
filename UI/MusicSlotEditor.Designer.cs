namespace CTGP7.UI
{
    partial class MusicSlotViewer
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

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mainMenu1 = new LibEveryFileExplorer.UI.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.dataGrid = new System.Windows.Forms.DataGridView();
            this.separator1 = new System.Windows.Forms.Label();
            this.addButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.musicFileBox = new System.Windows.Forms.TextBox();
            this.musicModeBox = new System.Windows.Forms.ComboBox();
            this.bpmNormalBox = new System.Windows.Forms.TextBox();
            this.bpmFastBox = new System.Windows.Forms.TextBox();
            this.offsetNormalBox = new System.Windows.Forms.TextBox();
            this.offsetFastBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.nameInfo = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.courseSelector = new CTGP7.UI.CourseSelect();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem2});
            this.menuItem1.Text = "CTGP-7";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 0;
            this.menuItem2.Text = "Update Course Names";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // dataGrid
            // 
            this.dataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid.Location = new System.Drawing.Point(12, 12);
            this.dataGrid.MultiSelect = false;
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.ReadOnly = true;
            this.dataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGrid.Size = new System.Drawing.Size(690, 12);
            this.dataGrid.TabIndex = 1;
            this.dataGrid.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.DataGrid_RowStateChanged);
            // 
            // separator1
            // 
            this.separator1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.separator1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.separator1.Location = new System.Drawing.Point(12, 60);
            this.separator1.Name = "separator1";
            this.separator1.Size = new System.Drawing.Size(690, 2);
            this.separator1.TabIndex = 2;
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Location = new System.Drawing.Point(12, 32);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 3;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Location = new System.Drawing.Point(93, 32);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 23);
            this.deleteButton.TabIndex = 4;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // musicFileBox
            // 
            this.musicFileBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.musicFileBox.Location = new System.Drawing.Point(119, 95);
            this.musicFileBox.Name = "musicFileBox";
            this.musicFileBox.Size = new System.Drawing.Size(379, 20);
            this.musicFileBox.TabIndex = 6;
            this.musicFileBox.TextChanged += new System.EventHandler(this.MultipleControl_SelectionChanged);
            // 
            // musicModeBox
            // 
            this.musicModeBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.musicModeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.musicModeBox.FormattingEnabled = true;
            this.musicModeBox.Location = new System.Drawing.Point(119, 121);
            this.musicModeBox.Name = "musicModeBox";
            this.musicModeBox.Size = new System.Drawing.Size(379, 21);
            this.musicModeBox.TabIndex = 7;
            this.musicModeBox.SelectedIndexChanged += new System.EventHandler(this.MultipleControl_SelectionChanged);
            // 
            // bpmNormalBox
            // 
            this.bpmNormalBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bpmNormalBox.Location = new System.Drawing.Point(119, 148);
            this.bpmNormalBox.Name = "bpmNormalBox";
            this.bpmNormalBox.Size = new System.Drawing.Size(177, 20);
            this.bpmNormalBox.TabIndex = 8;
            this.bpmNormalBox.TextChanged += new System.EventHandler(this.MultipleControl_SelectionChanged);
            // 
            // bpmFastBox
            // 
            this.bpmFastBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bpmFastBox.Location = new System.Drawing.Point(338, 148);
            this.bpmFastBox.Name = "bpmFastBox";
            this.bpmFastBox.Size = new System.Drawing.Size(160, 20);
            this.bpmFastBox.TabIndex = 9;
            this.bpmFastBox.TextChanged += new System.EventHandler(this.MultipleControl_SelectionChanged);
            // 
            // offsetNormalBox
            // 
            this.offsetNormalBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.offsetNormalBox.Location = new System.Drawing.Point(119, 174);
            this.offsetNormalBox.Name = "offsetNormalBox";
            this.offsetNormalBox.Size = new System.Drawing.Size(177, 20);
            this.offsetNormalBox.TabIndex = 10;
            this.offsetNormalBox.TextChanged += new System.EventHandler(this.MultipleControl_SelectionChanged);
            // 
            // offsetFastBox
            // 
            this.offsetFastBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.offsetFastBox.Location = new System.Drawing.Point(338, 174);
            this.offsetFastBox.Name = "offsetFastBox";
            this.offsetFastBox.Size = new System.Drawing.Size(160, 20);
            this.offsetFastBox.TabIndex = 11;
            this.offsetFastBox.TextChanged += new System.EventHandler(this.MultipleControl_SelectionChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(70, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Course:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Music File Name:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Music Mode:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 151);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "BPM          Normal:";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 177);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Offset          Normal:";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(302, 151);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Fast:";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(302, 174);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Fast:";
            // 
            // nameInfo
            // 
            this.nameInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nameInfo.AutoSize = true;
            this.nameInfo.Location = new System.Drawing.Point(504, 98);
            this.nameInfo.Name = "nameInfo";
            this.nameInfo.Size = new System.Drawing.Size(0, 13);
            this.nameInfo.TabIndex = 19;
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(584, 177);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(118, 13);
            this.linkLabel1.TabIndex = 20;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "About Music Config File";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // courseSelector
            // 
            this.courseSelector.AllowedTypes = new CTGP7.CTGP7CourseList.NameEntry.CourseType[] {
        CTGP7.CTGP7CourseList.NameEntry.CourseType.OriginalRace,
        CTGP7.CTGP7CourseList.NameEntry.CourseType.CustomRace,
        CTGP7.CTGP7CourseList.NameEntry.CourseType.Unknown};
            this.courseSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.courseSelector.Location = new System.Drawing.Point(119, 68);
            this.courseSelector.Name = "courseSelector";
            this.courseSelector.Size = new System.Drawing.Size(379, 21);
            this.courseSelector.TabIndex = 21;
            this.courseSelector.SelectedTrackChanged += new System.EventHandler(this.MultipleControl_SelectionChanged);
            // 
            // MusicSlotViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 199);
            this.Controls.Add(this.courseSelector);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.nameInfo);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.offsetFastBox);
            this.Controls.Add(this.offsetNormalBox);
            this.Controls.Add(this.bpmFastBox);
            this.Controls.Add(this.bpmNormalBox);
            this.Controls.Add(this.musicModeBox);
            this.Controls.Add(this.musicFileBox);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.separator1);
            this.Controls.Add(this.dataGrid);
            this.Menu = this.mainMenu1;
            this.Name = "MusicSlotViewer";
            this.Load += new System.EventHandler(this.MusicSlotViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private LibEveryFileExplorer.UI.MainMenu mainMenu1;
        private System.Windows.Forms.DataGridView dataGrid;
        private System.Windows.Forms.Label separator1;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.TextBox musicFileBox;
        private System.Windows.Forms.ComboBox musicModeBox;
        private System.Windows.Forms.TextBox bpmNormalBox;
        private System.Windows.Forms.TextBox bpmFastBox;
        private System.Windows.Forms.TextBox offsetNormalBox;
        private System.Windows.Forms.TextBox offsetFastBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label nameInfo;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private CourseSelect courseSelector;
    }
}

