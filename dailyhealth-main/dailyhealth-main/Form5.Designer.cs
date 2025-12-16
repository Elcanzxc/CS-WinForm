namespace dailyhealth
{
    partial class Form5
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
            this.grpGoals = new System.Windows.Forms.GroupBox();
            this.txtWaterGoal = new System.Windows.Forms.TextBox();
            this.txtSleepGoal = new System.Windows.Forms.TextBox();
            this.txtWeightGoal = new System.Windows.Forms.TextBox();
            this.txtCaloriesGoal = new System.Windows.Forms.TextBox();
            this.txtStepsGoal = new System.Windows.Forms.TextBox();
            this.lblWaterGoal = new System.Windows.Forms.Label();
            this.lblSleepGoal = new System.Windows.Forms.Label();
            this.lblWeightGoal = new System.Windows.Forms.Label();
            this.lblCaloriesGoal = new System.Windows.Forms.Label();
            this.lblStepsGoal = new System.Windows.Forms.Label();
            this.lblWaterHint = new System.Windows.Forms.Label();
            this.lblSleepHint = new System.Windows.Forms.Label();
            this.btnCalculateIdealWeight = new System.Windows.Forms.Button();
            this.lblCurrentGoalsLabel = new System.Windows.Forms.Label();
            this.txtCurrentGoals = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.grpGoals.SuspendLayout();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblTitle.Location = new System.Drawing.Point(250, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(200, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Settings and Goals";

            // grpGoals
            this.grpGoals.Controls.Add(this.btnCalculateIdealWeight);
            this.grpGoals.Controls.Add(this.lblSleepHint);
            this.grpGoals.Controls.Add(this.lblWaterHint);
            this.grpGoals.Controls.Add(this.lblStepsGoal);
            this.grpGoals.Controls.Add(this.lblCaloriesGoal);
            this.grpGoals.Controls.Add(this.lblWeightGoal);
            this.grpGoals.Controls.Add(this.lblSleepGoal);
            this.grpGoals.Controls.Add(this.lblWaterGoal);
            this.grpGoals.Controls.Add(this.txtStepsGoal);
            this.grpGoals.Controls.Add(this.txtCaloriesGoal);
            this.grpGoals.Controls.Add(this.txtWeightGoal);
            this.grpGoals.Controls.Add(this.txtSleepGoal);
            this.grpGoals.Controls.Add(this.txtWaterGoal);
            this.grpGoals.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpGoals.Location = new System.Drawing.Point(20, 70);
            this.grpGoals.Name = "grpGoals";
            this.grpGoals.Size = new System.Drawing.Size(380, 240);
            this.grpGoals.TabIndex = 1;
            this.grpGoals.TabStop = false;
            this.grpGoals.Text = "Set Goals";

            // txtWaterGoal
            this.txtWaterGoal.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtWaterGoal.Location = new System.Drawing.Point(180, 30);
            this.txtWaterGoal.Name = "txtWaterGoal";
            this.txtWaterGoal.Size = new System.Drawing.Size(100, 23);
            this.txtWaterGoal.TabIndex = 0;
            this.txtWaterGoal.TextChanged += new System.EventHandler(this.txtWaterGoal_TextChanged);

            // txtSleepGoal
            this.txtSleepGoal.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtSleepGoal.Location = new System.Drawing.Point(180, 65);
            this.txtSleepGoal.Name = "txtSleepGoal";
            this.txtSleepGoal.Size = new System.Drawing.Size(100, 23);
            this.txtSleepGoal.TabIndex = 1;
            this.txtSleepGoal.TextChanged += new System.EventHandler(this.txtSleepGoal_TextChanged);

            // txtWeightGoal
            this.txtWeightGoal.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtWeightGoal.Location = new System.Drawing.Point(180, 100);
            this.txtWeightGoal.Name = "txtWeightGoal";
            this.txtWeightGoal.Size = new System.Drawing.Size(100, 23);
            this.txtWeightGoal.TabIndex = 2;

            // txtCaloriesGoal
            this.txtCaloriesGoal.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtCaloriesGoal.Location = new System.Drawing.Point(180, 135);
            this.txtCaloriesGoal.Name = "txtCaloriesGoal";
            this.txtCaloriesGoal.Size = new System.Drawing.Size(100, 23);
            this.txtCaloriesGoal.TabIndex = 3;

            // txtStepsGoal
            this.txtStepsGoal.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtStepsGoal.Location = new System.Drawing.Point(180, 170);
            this.txtStepsGoal.Name = "txtStepsGoal";
            this.txtStepsGoal.Size = new System.Drawing.Size(100, 23);
            this.txtStepsGoal.TabIndex = 4;

            // lblWaterGoal
            this.lblWaterGoal.AutoSize = true;
            this.lblWaterGoal.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblWaterGoal.Location = new System.Drawing.Point(20, 33);
            this.lblWaterGoal.Name = "lblWaterGoal";
            this.lblWaterGoal.Size = new System.Drawing.Size(120, 15);
            this.lblWaterGoal.TabIndex = 5;
            this.lblWaterGoal.Text = "Water (glasses/day):";

            // lblSleepGoal
            this.lblSleepGoal.AutoSize = true;
            this.lblSleepGoal.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSleepGoal.Location = new System.Drawing.Point(20, 68);
            this.lblSleepGoal.Name = "lblSleepGoal";
            this.lblSleepGoal.Size = new System.Drawing.Size(87, 15);
            this.lblSleepGoal.TabIndex = 6;
            this.lblSleepGoal.Text = "Sleep (hours/day):";

            // lblWeightGoal
            this.lblWeightGoal.AutoSize = true;
            this.lblWeightGoal.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblWeightGoal.Location = new System.Drawing.Point(20, 103);
            this.lblWeightGoal.Name = "lblWeightGoal";
            this.lblWeightGoal.Size = new System.Drawing.Size(90, 15);
            this.lblWeightGoal.TabIndex = 7;
            this.lblWeightGoal.Text = "Target weight (kg):";

            // lblCaloriesGoal
            this.lblCaloriesGoal.AutoSize = true;
            this.lblCaloriesGoal.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCaloriesGoal.Location = new System.Drawing.Point(20, 138);
            this.lblCaloriesGoal.Name = "lblCaloriesGoal";
            this.lblCaloriesGoal.Size = new System.Drawing.Size(110, 15);
            this.lblCaloriesGoal.TabIndex = 8;
            this.lblCaloriesGoal.Text = "Calories (kcal/day):";

            // lblStepsGoal
            this.lblStepsGoal.AutoSize = true;
            this.lblStepsGoal.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStepsGoal.Location = new System.Drawing.Point(20, 173);
            this.lblStepsGoal.Name = "lblStepsGoal";
            this.lblStepsGoal.Size = new System.Drawing.Size(90, 15);
            this.lblStepsGoal.TabIndex = 9;
            this.lblStepsGoal.Text = "Steps (steps/day):";

            // lblWaterHint
            this.lblWaterHint.AutoSize = true;
            this.lblWaterHint.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblWaterHint.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblWaterHint.Location = new System.Drawing.Point(290, 33);
            this.lblWaterHint.Name = "lblWaterHint";
            this.lblWaterHint.Size = new System.Drawing.Size(0, 13);
            this.lblWaterHint.TabIndex = 10;

            // lblSleepHint
            this.lblSleepHint.AutoSize = true;
            this.lblSleepHint.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblSleepHint.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblSleepHint.Location = new System.Drawing.Point(290, 68);
            this.lblSleepHint.Name = "lblSleepHint";
            this.lblSleepHint.Size = new System.Drawing.Size(0, 13);
            this.lblSleepHint.TabIndex = 11;

            // btnCalculateIdealWeight
            this.btnCalculateIdealWeight.BackColor = System.Drawing.Color.LightBlue;
            this.btnCalculateIdealWeight.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnCalculateIdealWeight.Location = new System.Drawing.Point(20, 205);
            this.btnCalculateIdealWeight.Name = "btnCalculateIdealWeight";
            this.btnCalculateIdealWeight.Size = new System.Drawing.Size(260, 25);
            this.btnCalculateIdealWeight.TabIndex = 12;
            this.btnCalculateIdealWeight.Text = "📊 Calculate Ideal Weight";
            this.btnCalculateIdealWeight.UseVisualStyleBackColor = false;
            this.btnCalculateIdealWeight.Click += new System.EventHandler(this.btnCalculateIdealWeight_Click);

            // lblCurrentGoalsLabel
            this.lblCurrentGoalsLabel.AutoSize = true;
            this.lblCurrentGoalsLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCurrentGoalsLabel.Location = new System.Drawing.Point(420, 70);
            this.lblCurrentGoalsLabel.Name = "lblCurrentGoalsLabel";
            this.lblCurrentGoalsLabel.Size = new System.Drawing.Size(115, 19);
            this.lblCurrentGoalsLabel.TabIndex = 2;
            this.lblCurrentGoalsLabel.Text = "Current Goals:";

            // txtCurrentGoals
            this.txtCurrentGoals.Font = new System.Drawing.Font("Courier New", 9F);
            this.txtCurrentGoals.Location = new System.Drawing.Point(420, 100);
            this.txtCurrentGoals.Multiline = true;
            this.txtCurrentGoals.Name = "txtCurrentGoals";
            this.txtCurrentGoals.ReadOnly = true;
            this.txtCurrentGoals.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCurrentGoals.Size = new System.Drawing.Size(300, 210);
            this.txtCurrentGoals.TabIndex = 3;

            // btnSave
            this.btnSave.BackColor = System.Drawing.Color.LightGreen;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(20, 330);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(180, 45);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // btnReset
            this.btnReset.BackColor = System.Drawing.Color.LightCoral;
            this.btnReset.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnReset.Location = new System.Drawing.Point(220, 330);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(180, 45);
            this.btnReset.TabIndex = 5;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);

            // btnClose
            this.btnClose.BackColor = System.Drawing.Color.LightGray;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(540, 330);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(180, 45);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

            // Form5
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 395);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtCurrentGoals);
            this.Controls.Add(this.lblCurrentGoalsLabel);
            this.Controls.Add(this.grpGoals);
            this.Controls.Add(this.lblTitle);
            this.Name = "Form5";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings and Goals";
            this.grpGoals.ResumeLayout(false);
            this.grpGoals.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox grpGoals;
        private System.Windows.Forms.TextBox txtWaterGoal;
        private System.Windows.Forms.TextBox txtSleepGoal;
        private System.Windows.Forms.TextBox txtWeightGoal;
        private System.Windows.Forms.TextBox txtCaloriesGoal;
        private System.Windows.Forms.TextBox txtStepsGoal;
        private System.Windows.Forms.Label lblWaterGoal;
        private System.Windows.Forms.Label lblSleepGoal;
        private System.Windows.Forms.Label lblWeightGoal;
        private System.Windows.Forms.Label lblCaloriesGoal;
        private System.Windows.Forms.Label lblStepsGoal;
        private System.Windows.Forms.Label lblWaterHint;
        private System.Windows.Forms.Label lblSleepHint;
        private System.Windows.Forms.Button btnCalculateIdealWeight;
        private System.Windows.Forms.Label lblCurrentGoalsLabel;
        private System.Windows.Forms.TextBox txtCurrentGoals;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnClose;
    }
}
