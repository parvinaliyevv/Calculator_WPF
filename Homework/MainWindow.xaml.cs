using System;
using System.Windows;
using System.Windows.Controls;

namespace Homework
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public decimal Operand { get; set; } = default;
        public string Operator { get; set; } = null;

        public decimal CacheOperand { get; set; } = default;
        public string CacheOperator { get; set; } = null;


        private string _number = null;
        public byte queue { get; set; } = default;


        // Control Properties

        public string ControlResultTextBlock
        {
            get { return Result.Text; }
            set { Result.Text = (value.Length > 16) ? value.Substring(0, 16) : value; }
        }
        public string ControlNumber
        {
            get { return _number; }
            set { _number = (value.Length > 16) ? value.Substring(0, 16) : value; }
        }


        // Constructors

        public MainWindow()
        {
            InitializeComponent();
            Reset();
        }


        // Methods

        private void Calculate()
        {
            switch (Operator)
            {
                case "+":
                    ControlResultTextBlock = (Operand + Convert.ToDecimal(Result.Text)).ToString();
                    break;

                case "-":
                    ControlResultTextBlock = (Operand - Convert.ToDecimal(Result.Text)).ToString();
                    break;

                case "x":
                    ControlResultTextBlock = (Operand * Convert.ToDecimal(Result.Text)).ToString();
                    break;

                case "÷":
                    if (Convert.ToDecimal(Result.Text) == 0)
                        MessageBox.Show("You can't divide by zero!", "Divide Zero Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                        ControlResultTextBlock = (Operand / Convert.ToDecimal(Result.Text)).ToString();
                    break;
            }
        }

        private void Reset()
        {
            Result.Text = "0";
            View.Text = "0";
            ControlNumber = "0";
            Operand = 0;
            Operator = null;
        }


        // UIElement events

        private void Button_Click_Cache_Clear(object sender, RoutedEventArgs e)
        {
            CacheOperand = default;
            CacheOperator = null;
        }

        private void Button_Click_Cache_Result(object sender, RoutedEventArgs e)
        {
            Button_Click_Cache_Operand(sender, e);

            ControlResultTextBlock = CacheOperand.ToString();

            Button_Click_Cache_Clear(sender, e);
        }

        private void Button_Click_Cache_Operand(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                if (CacheOperator is null) CacheOperand = Convert.ToDecimal(ControlNumber);
                else
                {
                    if (CacheOperator == "M+") CacheOperand += Convert.ToDecimal(ControlNumber);
                    else if (CacheOperator == "M-") CacheOperand -= Convert.ToDecimal(ControlNumber);
                }

                CacheOperator = btn.Content.ToString();
            }
        }


        private void Button_Click_Number(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                var symbol = btn.Content.ToString();

                if (Result.Text == "0" || ControlNumber == "0") ControlNumber = symbol;
                else ControlNumber += symbol;

                ControlResultTextBlock = ControlNumber;

                if (queue % 2 == 0) { queue++; }
            }
        }

        private void Button_Click_Other(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                var symbol = btn.Content.ToString();

                if (symbol == "+/-" & ControlNumber != "0") ControlNumber = (ControlNumber[0] == '-') ? ControlNumber.Substring(1, ControlNumber.Length - 1) : "-" + ControlNumber;
                else if (symbol == "." & !ControlNumber.Contains(".")) ControlNumber += ".";

                ControlResultTextBlock = ControlNumber;
            }
        }

        private void Button_Click_Basic_Operation(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                if (Operator != null & queue % 2 != 0)
                {
                    Calculate();
                    queue--;
                }

                Operator = btn.Content.ToString();
                Operand = Convert.ToDecimal(Result.Text);

                ControlNumber = "0";
                ControlResultTextBlock = Operand.ToString();
                View.Text = string.Format("{0} {1}", Operand, Operator);
            }
        }

        private void Button_Click_Advanced_Operation(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                var symbol = btn.Content.ToString();
                var number = Convert.ToDecimal(ControlNumber);

                if (symbol == "x2")
                {
                    try { ControlNumber = (number * number).ToString(); }
                    catch (Exception) { }
                }
                else if (symbol == "1/X")
                {
                    try { ControlNumber = (1 / number).ToString();  }
                    catch (Exception) { MessageBox.Show("You can't divide by zero!", "Divide Zero Exception", MessageBoxButton.OK, MessageBoxImage.Error); }
                }
                else if (symbol == "√x") ControlNumber = (Math.Sqrt(Convert.ToDouble(number))).ToString();

                ControlResultTextBlock = ControlNumber;
            }
        }

        private void Button_Click_Clear(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                var symbol = btn.Content.ToString();

                if (symbol == "C") Reset();
                else if (symbol == "CE")
                {
                    if (View.Text.Contains("="))
                    {
                        Reset();
                        return;
                    }
                    ControlNumber = "0";
                    Result.Text = "0";
                }
                else
                {
                    ControlNumber = ControlNumber.Remove(ControlNumber.Length - 1, 1);
                    ControlResultTextBlock = ControlNumber;

                    if (string.IsNullOrWhiteSpace(Result.Text)) Reset();
                }
            }
        }

        private void Button_Click_Result(object sender, RoutedEventArgs e)
        {
            if (Operator is null) return;

            Calculate();

            View.Text = string.Format("{0} {1} {2} = ", Operand, Operator, ControlNumber);
            ControlNumber = Result.Text;

            Operand = decimal.Parse(ControlNumber);
            Operator = null;
        }
    }
}
