using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfCalculator
{
    public partial class MainWindow : Window
    {
        private double _lastNumber;
        private string _lastOperation;
        private bool _newNumber;
        private bool _operationJustPressed;
        private bool _calculationPending;

        public MainWindow()
        {
            InitializeComponent();
            ClearCalculator();
        }

        private void ClearCalculator()
        {
            Display.Text = "0";
            _lastNumber = 0;
            _lastOperation = "";
            _newNumber = true;
            _operationJustPressed = false;
            _calculationPending = false;
        }

        private void Number_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var number = button.Content.ToString();

            if (_newNumber || Display.Text == "0")
            {
                Display.Text = number;
                _newNumber = false;
            }
            else
            {
                Display.Text += number;
            }

            _operationJustPressed = false;
        }

        private void Operation_Click(object sender, RoutedEventArgs e)
        {
            if (!_operationJustPressed && _calculationPending)
            {
                CalculateResult();
            }

            try
            {
                _lastNumber = Convert.ToDouble(Display.Text);
            }
            catch
            {
                Display.Text = "0";
                _lastNumber = 0;
            }

            var buttonPressed = (Button)sender;
            _lastOperation = buttonPressed.Content.ToString();
            _newNumber = true;
            _operationJustPressed = true;
            _calculationPending = true;
        }

        private void CalculateResult()
        {
            try
            {
                var currentNumber = Convert.ToDouble(Display.Text);
                double result = 0;

                switch (_lastOperation)
                {
                    case "+":
                        result = _lastNumber + currentNumber;
                        break;
                    case "-":
                        result = _lastNumber - currentNumber;
                        break;
                    case "*":
                        result = _lastNumber * currentNumber;
                        break;
                    case "/":
                        if (currentNumber == 0)
                        {
                            Display.Text = "Error";
                            //ClearCalculator();
                            return;
                        }
                        result = _lastNumber / currentNumber;
                        break;
                }

                Display.Text = result.ToString();
                _lastNumber = result;
            }
            catch
            {
                Display.Text = "Error";
                ClearCalculator();
            }
        }

        private void BtnEquals_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_lastOperation))
                return;

            CalculateResult();
            _lastOperation = "";
            _calculationPending = false;
            _operationJustPressed = false;
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearCalculator();
        }

        private void BtnDot_Click(object sender, RoutedEventArgs e)
        {
            if (_newNumber)
            {
                Display.Text = "0,";
                _newNumber = false;
            }

            if (!Display.Text.Contains(","))
            {
                Display.Text += ",";
            }

            _operationJustPressed = false;
        }

        private void BtnPlusMinus_Click(object sender, RoutedEventArgs e)
        {
            if (Display.Text == "0")
                return;

            if (Display.Text.StartsWith("-"))
            {
                Display.Text = Display.Text.Substring(1);
            }
            else
            {
                Display.Text = "-" + Display.Text;
            }

            _operationJustPressed = false;
        }
    }
}