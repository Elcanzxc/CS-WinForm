namespace dailyhealth
{
    partial class Form3
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
            this.grpInput = new System.Windows.Forms.GroupBox();
            this.txtAge = new System.Windows.Forms.TextBox();
            this.txtWeight = new System.Windows.Forms.TextBox();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.cmbGender = new System.Windows.Forms.ComboBox();
            this.cmbActivityLevel = new System.Windows.Forms.ComboBox();
            this.cmbGoal = new System.Windows.Forms.ComboBox();
            this.lblAge = new System.Windows.Forms.Label();
            this.lblWeight = new System.Windows.Forms.Label();
            this.lblHeight = new System.Windows.Forms.Label();
            this.lblGender = new System.Windows.Forms.Label();
            this.lblActivityLevel = new System.Windows.Forms.Label();
            this.lblGoal = new System.Windows.Forms.Label();
            this.grpResults = new System.Windows.Forms.GroupBox();
            this.lblBMR = new System.Windows.Forms.Label();
            this.lblTDEE = new System.Windows.Forms.Label();
            this.lblRecommended = new System.Windows.Forms.Label();
            this.txtMacros = new System.Windows.Forms.TextBox();
            this.txtRecommendations = new System.Windows.Forms.TextBox();
            this.lblMacros = new System.Windows.Forms.Label();
            this.lblRecommendations = new System.Windows.Forms.Label();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.grpInput.SuspendLayout();
            this.grpResults.SuspendLayout();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblTitle.Location = new System.Drawing.Point(250, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(250, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Calorie Calculator";

            // grpInput
            this.grpInput.Controls.Add(this.lblGoal);
            this.grpInput.Controls.Add(this.lblActivityLevel);
            this.grpInput.Controls.Add(this.lblGender);
            this.grpInput.Controls.Add(this.lblHeight);
            this.grpInput.Controls.Add(this.lblWeight);
            this.grpInput.Controls.Add(this.lblAge);
            this.grpInput.Controls.Add(this.cmbGoal);
            this.grpInput.Controls.Add(this.cmbActivityLevel);
            this.grpInput.Controls.Add(this.cmbGender);
            this.grpInput.Controls.Add(this.txtHeight);
            this.grpInput.Controls.Add(this.txtWeight);
            this.grpInput.Controls.Add(this.txtAge);
            this.grpInput.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpInput.Location = new System.Drawing.Point(20, 70);
            this.grpInput.Name = "grpInput";
            this.grpInput.Size = new System.Drawing.Size(350, 280);
            this.grpInput.TabIndex = 1;
            this.grpInput.TabStop = false;
            this.grpInput.Text = "Your Data";

            // Input fields
            this.txtAge.Location = new System.Drawing.Point(150, 30);
            this.txtAge.Size = new System.Drawing.Size(180, 23);
            this.txtWeight.Location = new System.Drawing.Point(150, 65);
            this.txtWeight.Size = new System.Drawing.Size(180, 23);
            this.txtHeight.Location = new System.Drawing.Point(150, 100);
            this.txtHeight.Size = new System.Drawing.Size(180, 23);

            this.cmbGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGender.Items.AddRange(new object[] { "Male", "Female" });
            this.cmbGender.Location = new System.Drawing.Point(150, 135);
            this.cmbGender.Size = new System.Drawing.Size(180, 23);

            this.cmbActivityLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbActivityLevel.Items.AddRange(new object[] {
                "Sedentary",
                "Lightly Active",
                "Moderately Active",
                "Very Active",
                "Extremely Active"
            });
            this.cmbActivityLevel.Location = new System.Drawing.Point(150, 170);
            this.cmbActivityLevel.Size = new System.Drawing.Size(180, 23);

            this.cmbGoal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGoal.Items.AddRange(new object[] {
                "Weight Loss",
                "Weight Maintenance",
                "Muscle Gain"
            });
            this.cmbGoal.Location = new System.Drawing.Point(150, 205);
            this.cmbGoal.Size = new System.Drawing.Size(180, 23);
            this.cmbGoal.SelectedIndexChanged += new System.EventHandler(this.cmbGoal_SelectedIndexChanged);

            // Labels
            this.lblAge.AutoSize = true;
            this.lblAge.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAge.Location = new System.Drawing.Point(20, 33);
            this.lblAge.Text = "Age:";

            this.lblWeight.AutoSize = true;
            this.lblWeight.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblWeight.Location = new System.Drawing.Point(20, 68);
            this.lblWeight.Text = "Weight (kg):";

            this.lblHeight.AutoSize = true;
            this.lblHeight.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblHeight.Location = new System.Drawing.Point(20, 103);
            this.lblHeight.Text = "Height (cm):";

            this.lblGender.AutoSize = true;
            this.lblGender.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblGender.Location = new System.Drawing.Point(20, 138);
            this.lblGender.Text = "Gender:";

            this.lblActivityLevel.AutoSize = true;
            this.lblActivityLevel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblActivityLevel.Location = new System.Drawing.Point(20, 173);
            this.lblActivityLevel.Text = "Activity Level:";

            this.lblGoal.AutoSize = true;
            this.lblGoal.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblGoal.Location = new System.Drawing.Point(20, 208);
            this.lblGoal.Text = "Goal:";

            // grpResults
            this.grpResults.Controls.Add(this.lblRecommended);
            this.grpResults.Controls.Add(this.lblTDEE);
            this.grpResults.Controls.Add(this.lblBMR);
            this.grpResults.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpResults.Location = new System.Drawing.Point(390, 70);
            this.grpResults.Name = "grpResults";
            this.grpResults.Size = new System.Drawing.Size(380, 120);
            this.grpResults.TabIndex = 2;
            this.grpResults.TabStop = false;
            this.grpResults.Text = "Results";

            this.lblBMR.AutoSize = true;
            this.lblBMR.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBMR.Location = new System.Drawing.Point(20, 30);
            this.lblBMR.Text = "Basal Metabolic Rate (BMR): --";

            this.lblTDEE.AutoSize = true;
            this.lblTDEE.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTDEE.Location = new System.Drawing.Point(20, 55);
            this.lblTDEE.Text = "Total Daily Energy Expenditure (TDEE): --";

            this.lblRecommended.AutoSize = true;
            this.lblRecommended.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblRecommended.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblRecommended.Location = new System.Drawing.Point(20, 85);
            this.lblRecommended.Text = "Recommended Intake: --";

            // Macros
            this.lblMacros.AutoSize = true;
            this.lblMacros.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMacros.Location = new System.Drawing.Point(390, 200);
            this.lblMacros.Text = "Macronutrients:";

            this.txtMacros.Font = new System.Drawing.Font("Courier New", 9F);
            this.txtMacros.Location = new System.Drawing.Point(390, 230);
            this.txtMacros.Multiline = true;
            this.txtMacros.ReadOnly = true;
            this.txtMacros.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMacros.Size = new System.Drawing.Size(380, 120);
            this.txtMacros.TabIndex = 4;

            // Recommendations
            this.lblRecommendations.AutoSize = true;
            this.lblRecommendations.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblRecommendations.Location = new System.Drawing.Point(20, 360);
            this.lblRecommendations.Text = "Recommendations:";

            this.txtRecommendations.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtRecommendations.Location = new System.Drawing.Point(20, 390);
            this.txtRecommendations.Multiline = true;
            this.txtRecommendations.ReadOnly = true;
            this.txtRecommendations.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRecommendations.Size = new System.Drawing.Size(750, 180);
            this.txtRecommendations.TabIndex = 6;

            // Buttons
            this.btnCalculate.BackColor = System.Drawing.Color.LightGreen;
            this.btnCalculate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCalculate.Location = new System.Drawing.Point(20, 590);
            this.btnCalculate.Size = new System.Drawing.Size(200, 45);
            this.btnCalculate.Text = "Calculate";
            this.btnCalculate.UseVisualStyleBackColor = false;
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);

            this.btnClose.BackColor = System.Drawing.Color.LightGray;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(570, 590);
            this.btnClose.Size = new System.Drawing.Size(200, 45);
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

            // Form3
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 650);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCalculate);
            this.Controls.Add(this.txtRecommendations);
            this.Controls.Add(this.lblRecommendations);
            this.Controls.Add(this.txtMacros);
            this.Controls.Add(this.lblMacros);
            this.Controls.Add(this.grpResults);
            this.Controls.Add(this.grpInput);
            this.Controls.Add(this.lblTitle);
            this.Name = "Form3";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Calorie Calculator";
            this.grpInput.ResumeLayout(false);
            this.grpInput.PerformLayout();
            this.grpResults.ResumeLayout(false);
            this.grpResults.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox grpInput;
        private System.Windows.Forms.TextBox txtAge;
        private System.Windows.Forms.TextBox txtWeight;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.ComboBox cmbGender;
        private System.Windows.Forms.ComboBox cmbActivityLevel;
        private System.Windows.Forms.ComboBox cmbGoal;
        private System.Windows.Forms.Label lblAge;
        private System.Windows.Forms.Label lblWeight;
        private System.Windows.Forms.Label lblHeight;
        private System.Windows.Forms.Label lblGender;
        private System.Windows.Forms.Label lblActivityLevel;
        private System.Windows.Forms.Label lblGoal;
        private System.Windows.Forms.GroupBox grpResults;
        private System.Windows.Forms.Label lblBMR;
        private System.Windows.Forms.Label lblTDEE;
        private System.Windows.Forms.Label lblRecommended;
        private System.Windows.Forms.TextBox txtMacros;
        private System.Windows.Forms.TextBox txtRecommendations;
        private System.Windows.Forms.Label lblMacros;
        private System.Windows.Forms.Label lblRecommendations;
        private System.Windows.Forms.Button btnCalculate;
        private System.Windows.Forms.Button btnClose;
    }
}
