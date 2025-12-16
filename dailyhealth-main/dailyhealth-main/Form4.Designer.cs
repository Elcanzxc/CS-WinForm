namespace dailyhealth
{
    partial class Form4
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
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.cmbLevel = new System.Windows.Forms.ComboBox();
            this.cmbGoal = new System.Windows.Forms.ComboBox();
            this.lblLevel = new System.Windows.Forms.Label();
            this.lblGoal = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.lblPlan = new System.Windows.Forms.Label();
            this.txtWorkoutPlan = new System.Windows.Forms.TextBox();
            this.lblTips = new System.Windows.Forms.Label();
            this.txtTips = new System.Windows.Forms.TextBox();
            this.btnSavePlan = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.grpOptions.SuspendLayout();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblTitle.Location = new System.Drawing.Point(300, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(200, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Workout Plan";

            // grpOptions
            this.grpOptions.Controls.Add(this.btnGenerate);
            this.grpOptions.Controls.Add(this.lblGoal);
            this.grpOptions.Controls.Add(this.lblLevel);
            this.grpOptions.Controls.Add(this.cmbGoal);
            this.grpOptions.Controls.Add(this.cmbLevel);
            this.grpOptions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpOptions.Location = new System.Drawing.Point(20, 70);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(780, 100);
            this.grpOptions.TabIndex = 1;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";

            // cmbLevel
            this.cmbLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLevel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbLevel.FormattingEnabled = true;
            this.cmbLevel.Items.AddRange(new object[] { "Beginner", "Intermediate" });
            this.cmbLevel.Location = new System.Drawing.Point(120, 30);
            this.cmbLevel.Name = "cmbLevel";
            this.cmbLevel.Size = new System.Drawing.Size(200, 23);
            this.cmbLevel.TabIndex = 0;

            // cmbGoal
            this.cmbGoal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGoal.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbGoal.FormattingEnabled = true;
            this.cmbGoal.Items.AddRange(new object[] {
                "Weight Loss",
                "Muscle Gain",
                "Maintenance"
            });
            this.cmbGoal.Location = new System.Drawing.Point(450, 30);
            this.cmbGoal.Name = "cmbGoal";
            this.cmbGoal.Size = new System.Drawing.Size(200, 23);
            this.cmbGoal.TabIndex = 1;

            // lblLevel
            this.lblLevel.AutoSize = true;
            this.lblLevel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblLevel.Location = new System.Drawing.Point(20, 33);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(45, 15);
            this.lblLevel.TabIndex = 2;
            this.lblLevel.Text = "Level:";

            // lblGoal
            this.lblGoal.AutoSize = true;
            this.lblGoal.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblGoal.Location = new System.Drawing.Point(350, 33);
            this.lblGoal.Name = "lblGoal";
            this.lblGoal.Size = new System.Drawing.Size(35, 15);
            this.lblGoal.TabIndex = 3;
            this.lblGoal.Text = "Goal:";

            // btnGenerate
            this.btnGenerate.BackColor = System.Drawing.Color.LightGreen;
            this.btnGenerate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnGenerate.Location = new System.Drawing.Point(300, 65);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(180, 30);
            this.btnGenerate.TabIndex = 4;
            this.btnGenerate.Text = "Generate Plan";
            this.btnGenerate.UseVisualStyleBackColor = false;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);

            // lblPlan
            this.lblPlan.AutoSize = true;
            this.lblPlan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblPlan.Location = new System.Drawing.Point(20, 185);
            this.lblPlan.Name = "lblPlan";
            this.lblPlan.Size = new System.Drawing.Size(130, 19);
            this.lblPlan.TabIndex = 2;
            this.lblPlan.Text = "Workout Plan:";

            // txtWorkoutPlan
            this.txtWorkoutPlan.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtWorkoutPlan.Location = new System.Drawing.Point(20, 215);
            this.txtWorkoutPlan.Multiline = true;
            this.txtWorkoutPlan.Name = "txtWorkoutPlan";
            this.txtWorkoutPlan.ReadOnly = true;
            this.txtWorkoutPlan.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtWorkoutPlan.Size = new System.Drawing.Size(450, 340);
            this.txtWorkoutPlan.TabIndex = 3;

            // lblTips
            this.lblTips.AutoSize = true;
            this.lblTips.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTips.Location = new System.Drawing.Point(490, 185);
            this.lblTips.Name = "lblTips";
            this.lblTips.Size = new System.Drawing.Size(169, 19);
            this.lblTips.TabIndex = 4;
            this.lblTips.Text = "Exercise Tips:";

            // txtTips
            this.txtTips.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTips.Location = new System.Drawing.Point(490, 215);
            this.txtTips.Multiline = true;
            this.txtTips.Name = "txtTips";
            this.txtTips.ReadOnly = true;
            this.txtTips.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTips.Size = new System.Drawing.Size(310, 340);
            this.txtTips.TabIndex = 5;

            // btnSavePlan
            this.btnSavePlan.BackColor = System.Drawing.Color.LightBlue;
            this.btnSavePlan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSavePlan.Location = new System.Drawing.Point(20, 570);
            this.btnSavePlan.Name = "btnSavePlan";
            this.btnSavePlan.Size = new System.Drawing.Size(180, 40);
            this.btnSavePlan.TabIndex = 6;
            this.btnSavePlan.Text = "Save Plan";
            this.btnSavePlan.UseVisualStyleBackColor = false;
            this.btnSavePlan.Click += new System.EventHandler(this.btnSavePlan_Click);

            // btnClose
            this.btnClose.BackColor = System.Drawing.Color.LightGray;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(620, 570);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(180, 40);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

            // Form4
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 625);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSavePlan);
            this.Controls.Add(this.txtTips);
            this.Controls.Add(this.lblTips);
            this.Controls.Add(this.txtWorkoutPlan);
            this.Controls.Add(this.lblPlan);
            this.Controls.Add(this.grpOptions);
            this.Controls.Add(this.lblTitle);
            this.Name = "Form4";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Workout Plan";
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.ComboBox cmbLevel;
        private System.Windows.Forms.ComboBox cmbGoal;
        private System.Windows.Forms.Label lblLevel;
        private System.Windows.Forms.Label lblGoal;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Label lblPlan;
        private System.Windows.Forms.TextBox txtWorkoutPlan;
        private System.Windows.Forms.Label lblTips;
        private System.Windows.Forms.TextBox txtTips;
        private System.Windows.Forms.Button btnSavePlan;
        private System.Windows.Forms.Button btnClose;
    }
}
