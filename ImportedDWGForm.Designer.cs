namespace DynamoBundle
{
    partial class ImportedDWGForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportedDWGForm));
            this.impDataGridView = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deleteAllbtn = new System.Windows.Forms.Button();
            this.deleteSelectedbtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.impDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // impDataGridView
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.impDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.impDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.impDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.impDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.impDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.impDataGridView.Location = new System.Drawing.Point(12, 12);
            this.impDataGridView.Name = "impDataGridView";
            this.impDataGridView.RowHeadersVisible = false;
            this.impDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.impDataGridView.Size = new System.Drawing.Size(260, 200);
            this.impDataGridView.TabIndex = 0;
            this.impDataGridView.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.impDataGridView_CellContentDoubleClick);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.FillWeight = 150F;
            this.Column1.HeaderText = "Imported DWGs";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Id";
            this.Column2.Name = "Column2";
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column2.Visible = false;
            // 
            // deleteAllbtn
            // 
            this.deleteAllbtn.Location = new System.Drawing.Point(197, 226);
            this.deleteAllbtn.Name = "deleteAllbtn";
            this.deleteAllbtn.Size = new System.Drawing.Size(75, 23);
            this.deleteAllbtn.TabIndex = 1;
            this.deleteAllbtn.Text = "Delete All";
            this.deleteAllbtn.UseVisualStyleBackColor = true;
            this.deleteAllbtn.Click += new System.EventHandler(this.deleteAllbtn_Click);
            // 
            // deleteSelectedbtn
            // 
            this.deleteSelectedbtn.Location = new System.Drawing.Point(116, 226);
            this.deleteSelectedbtn.Name = "deleteSelectedbtn";
            this.deleteSelectedbtn.Size = new System.Drawing.Size(75, 23);
            this.deleteSelectedbtn.TabIndex = 2;
            this.deleteSelectedbtn.Text = "Delete";
            this.deleteSelectedbtn.UseVisualStyleBackColor = true;
            this.deleteSelectedbtn.Click += new System.EventHandler(this.deleteSelectedbtn_Click);
            // 
            // ImportedDWGForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.deleteSelectedbtn);
            this.Controls.Add(this.deleteAllbtn);
            this.Controls.Add(this.impDataGridView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(300, 300);
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "ImportedDWGForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Purge Imported DWG";
            ((System.ComponentModel.ISupportInitialize)(this.impDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView impDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.Button deleteAllbtn;
        private System.Windows.Forms.Button deleteSelectedbtn;
    }
}