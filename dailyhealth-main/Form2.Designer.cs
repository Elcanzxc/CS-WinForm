namespace dailyhealth
{
    partial class Form2
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lvHistory = new System.Windows.Forms.ListView();
            this.colDate = new System.Windows.Forms.ColumnHeader();
            this.colTime = new System.Windows.Forms.ColumnHeader();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.colWeight = new System.Windows.Forms.ColumnHeader();
            this.colBMI = new System.Windows.Forms.ColumnHeader();
            this.colCategory = new System.Windows.Forms.ColumnHeader();
            this.colWater = new System.Windows.Forms.ColumnHeader();
            this.colSleep = new System.Windows.Forms.ColumnHeader();
            this.colActivity = new System.Windows.Forms.ColumnHeader();
            this.colScore = new System.Windows.Forms.ColumnHeader();
            this.txtStats = new System.Windows.Forms.TextBox();
            this.lblStats = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblTitle.Location = new System.Drawing.Point(300, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(250, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "История и статистика";

            // lvHistory
            this.lvHistory.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                this.colDate, this.colTime, this.colName, this.colWeight,
                this.colBMI, this.colCategory, this.colWater, this.colSleep,
                this.colActivity, this.colScore});
            this.lvHistory.FullRowSelect = true;
            this.lvHistory.GridLines = true;
            this.lvHistory.Location = new System.Drawing.Point(20, 70);
            this.lvHistory.Name = "lvHistory";
            this.lvHistory.Size = new System.Drawing.Size(850, 300);
            this.lvHistory.TabIndex = 1;
            this.lvHistory.View = System.Windows.Forms.View.Details;
            this.lvHistory.DoubleClick += new System.EventHandler(this.lvHistory_DoubleClick);

            // Columns
            this.colDate.Text = "Дата";
            this.colDate.Width = 90;
            this.colTime.Text = "Время";
            this.colTime.Width = 60;
            this.colName.Text = "Имя";
            this.colName.Width = 100;
            this.colWeight.Text = "Вес";
            this.colWeight.Width = 60;
            this.colBMI.Text = "ИМТ";
            this.colBMI.Width = 60;
            this.colCategory.Text = "Категория";
            this.colCategory.Width = 120;
            this.colWater.Text = "Вода";
            this.colWater.Width = 60;
            this.colSleep.Text = "Сон";
            this.colSleep.Width = 60;
            this.colActivity.Text = "Активность";
            this.colActivity.Width = 100;
            this.colScore.Text = "Балл";
            this.colScore.Width = 60;

            // lblStats
            this.lblStats.AutoSize = true;
            this.lblStats.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblStats.Location = new System.Drawing.Point(20, 385);
            this.lblStats.Name = "lblStats";
            this.lblStats.Size = new System.Drawing.Size(90, 19);
            this.lblStats.TabIndex = 2;
            this.lblStats.Text = "Статистика:";

            // txtStats
            this.txtStats.Font = new System.Drawing.Font("Courier New", 9F);
            this.txtStats.Location = new System.Drawing.Point(20, 415);
            this.txtStats.Multiline = true;
            this.txtStats.Name = "txtStats";
            this.txtStats.ReadOnly = true;
            this.txtStats.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStats.Size = new System.Drawing.Size(850, 200);
            this.txtStats.TabIndex = 3;

            // btnRefresh
            this.btnRefresh.BackColor = System.Drawing.Color.LightBlue;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Location = new System.Drawing.Point(20, 630);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(150, 40);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Обновить";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            // btnDelete
            this.btnDelete.BackColor = System.Drawing.Color.LightCoral;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Location = new System.Drawing.Point(190, 630);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(150, 40);
            this.btnDelete.TabIndex = 5;
            this.btnDelete.Text = "Удалить";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            // btnExport
            this.btnExport.BackColor = System.Drawing.Color.LightGreen;
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnExport.Location = new System.Drawing.Point(360, 630);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(150, 40);
            this.btnExport.TabIndex = 6;
            this.btnExport.Text = "Экспорт в CSV";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);

            // btnClose
            this.btnClose.BackColor = System.Drawing.Color.LightGray;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(720, 630);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(150, 40);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

            // Form2
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(890, 690);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.txtStats);
            this.Controls.Add(this.lblStats);
            this.Controls.Add(this.lvHistory);
            this.Controls.Add(this.lblTitle);
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "История и статистика";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ListView lvHistory;
        private System.Windows.Forms.ColumnHeader colDate;
        private System.Windows.Forms.ColumnHeader colTime;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colWeight;
        private System.Windows.Forms.ColumnHeader colBMI;
        private System.Windows.Forms.ColumnHeader colCategory;
        private System.Windows.Forms.ColumnHeader colWater;
        private System.Windows.Forms.ColumnHeader colSleep;
        private System.Windows.Forms.ColumnHeader colActivity;
        private System.Windows.Forms.ColumnHeader colScore;
        private System.Windows.Forms.TextBox txtStats;
        private System.Windows.Forms.Label lblStats;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnClose;
    }
}