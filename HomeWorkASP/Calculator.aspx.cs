using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HomeWorkASP
{
    public partial class Calculator : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void btnOperation_Click(object sender, EventArgs e)
        {
            try
            {
                double a = double.Parse(num1.Text);
                double b = double.Parse(num2.Text);
                string operation = (sender as Button).Text;
                double result = 0;

          
                switch (operation)
                {
                    case "+":
                        result = a + b;
                        break;
                    case "-":
                        result = a - b;
                        break;
                    case "*":
                        result = a * b;
                        break;
                    case "/":
                        if (b != 0)
                            result = a / b;
                        else
                            lblResult.Text = "Cannot divide by zero!";
                        return;
                }

                lblResult.Text = result.ToString();
            }
            catch
            {
                lblResult.Text = "Input error";
            }
        }

    
        protected void btnCalc_Click(object sender, EventArgs e)
        {
            
        }
    }
}