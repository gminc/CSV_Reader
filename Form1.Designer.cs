namespace ReadCSV
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer Components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (Components != null))
            {
                Components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.StockSelectComboBox = new System.Windows.Forms.ComboBox();
            this.ReadFileButton = new System.Windows.Forms.Button();
            this.SearchStockButton = new System.Windows.Forms.Button();
            this.FilePathTextBox = new System.Windows.Forms.TextBox();
            this.CostTimeTextBox = new System.Windows.Forms.TextBox();
            this.Top50Button = new System.Windows.Forms.Button();
            this.StockDataGridView = new System.Windows.Forms.DataGridView();
            this.ReadFileState = new System.Windows.Forms.Label();
            this.StatisticsDataGridView = new System.Windows.Forms.DataGridView();
            this.Top50DataGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.StockDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StatisticsDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Top50DataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // StockSelectComboBox
            // 
            this.StockSelectComboBox.FormattingEnabled = true;
            this.StockSelectComboBox.Location = new System.Drawing.Point(21, 73);
            this.StockSelectComboBox.Name = "StockSelectComboBox";
            this.StockSelectComboBox.Size = new System.Drawing.Size(554, 20);
            this.StockSelectComboBox.TabIndex = 0;
            // 
            // ReadFileButton
            // 
            this.ReadFileButton.Location = new System.Drawing.Point(586, 9);
            this.ReadFileButton.Name = "ReadFileButton";
            this.ReadFileButton.Size = new System.Drawing.Size(87, 25);
            this.ReadFileButton.TabIndex = 1;
            this.ReadFileButton.Text = "讀取檔案";
            this.ReadFileButton.UseVisualStyleBackColor = true;
            this.ReadFileButton.Click += new System.EventHandler(this.ReadFielButton_Click);
            // 
            // SearchStockButton
            // 
            this.SearchStockButton.Location = new System.Drawing.Point(586, 73);
            this.SearchStockButton.Name = "SearchStockButton";
            this.SearchStockButton.Size = new System.Drawing.Size(87, 20);
            this.SearchStockButton.TabIndex = 2;
            this.SearchStockButton.Text = "股票查詢";
            this.SearchStockButton.UseVisualStyleBackColor = true;
            this.SearchStockButton.Click += new System.EventHandler(this.SearchStockButton_Click);
            // 
            // FilePathTextBox
            // 
            this.FilePathTextBox.Location = new System.Drawing.Point(21, 12);
            this.FilePathTextBox.Name = "FilePathTextBox";
            this.FilePathTextBox.Size = new System.Drawing.Size(554, 22);
            this.FilePathTextBox.TabIndex = 3;
            // 
            // CostTimeTextBox
            // 
            this.CostTimeTextBox.Location = new System.Drawing.Point(783, 12);
            this.CostTimeTextBox.Multiline = true;
            this.CostTimeTextBox.Name = "CostTimeTextBox";
            this.CostTimeTextBox.ReadOnly = true;
            this.CostTimeTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.CostTimeTextBox.Size = new System.Drawing.Size(371, 81);
            this.CostTimeTextBox.TabIndex = 4;
            // 
            // Top50Button
            // 
            this.Top50Button.Location = new System.Drawing.Point(690, 73);
            this.Top50Button.Name = "Top50Button";
            this.Top50Button.Size = new System.Drawing.Size(87, 20);
            this.Top50Button.TabIndex = 5;
            this.Top50Button.Text = "買賣超TOP50";
            this.Top50Button.UseVisualStyleBackColor = true;
            this.Top50Button.Click += new System.EventHandler(this.Top50Button_Click);
            // 
            // StockDataGridView
            // 
            this.StockDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.StockDataGridView.Location = new System.Drawing.Point(21, 109);
            this.StockDataGridView.Name = "StockDataGridView";
            this.StockDataGridView.RowTemplate.Height = 24;
            this.StockDataGridView.Size = new System.Drawing.Size(756, 269);
            this.StockDataGridView.TabIndex = 10;
            // 
            // ReadFileState
            // 
            this.ReadFileState.AutoSize = true;
            this.ReadFileState.Location = new System.Drawing.Point(688, 11);
            this.ReadFileState.MinimumSize = new System.Drawing.Size(87, 20);
            this.ReadFileState.Name = "ReadFileState";
            this.ReadFileState.Size = new System.Drawing.Size(87, 20);
            this.ReadFileState.TabIndex = 11;
            this.ReadFileState.Text = "讀取狀態";
            this.ReadFileState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // StatisticsDataGridView
            // 
            this.StatisticsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.StatisticsDataGridView.Location = new System.Drawing.Point(21, 384);
            this.StatisticsDataGridView.Name = "StatisticsDataGridView";
            this.StatisticsDataGridView.RowTemplate.Height = 24;
            this.StatisticsDataGridView.Size = new System.Drawing.Size(756, 269);
            this.StatisticsDataGridView.TabIndex = 12;
            // 
            // Top50DataGridView
            // 
            this.Top50DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Top50DataGridView.Location = new System.Drawing.Point(783, 109);
            this.Top50DataGridView.Name = "Top50DataGridView";
            this.Top50DataGridView.RowTemplate.Height = 24;
            this.Top50DataGridView.Size = new System.Drawing.Size(370, 543);
            this.Top50DataGridView.TabIndex = 13;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1166, 702);
            this.Controls.Add(this.Top50DataGridView);
            this.Controls.Add(this.StatisticsDataGridView);
            this.Controls.Add(this.ReadFileState);
            this.Controls.Add(this.StockDataGridView);
            this.Controls.Add(this.Top50Button);
            this.Controls.Add(this.CostTimeTextBox);
            this.Controls.Add(this.FilePathTextBox);
            this.Controls.Add(this.SearchStockButton);
            this.Controls.Add(this.ReadFileButton);
            this.Controls.Add(this.StockSelectComboBox);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.StockDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StatisticsDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Top50DataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox StockSelectComboBox;
        private System.Windows.Forms.Button ReadFileButton;
        private System.Windows.Forms.Button SearchStockButton;
        private System.Windows.Forms.TextBox FilePathTextBox;
        private System.Windows.Forms.TextBox CostTimeTextBox;
        private System.Windows.Forms.Button Top50Button;
        private System.Windows.Forms.DataGridView StockDataGridView;
        private System.Windows.Forms.Label ReadFileState;
        private System.Windows.Forms.DataGridView StatisticsDataGridView;
        private System.Windows.Forms.DataGridView Top50DataGridView;
    }
}

