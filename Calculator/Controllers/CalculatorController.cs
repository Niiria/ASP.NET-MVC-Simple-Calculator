using Microsoft.AspNetCore.Mvc;
using System;

namespace Calculator.Controllers
{
    public class CalculatorController : Controller
    {
        public class CalcModel
        {
            public string Formula { get; set; }
            public string DisplayedNumber { get; set; } = null;
            public double ResultNumber { get; set; } = 0;
            public string Action { get; set; } = null;
            public bool ActionSignControl { get; set; }

        }

        private static readonly CalcModel Calc = new();


        public IActionResult Index()
        {
            return View(Calc);
        }

        public IActionResult SetNumber(string number)
        {
            if (Calc.ActionSignControl)
            {
                Calc.DisplayedNumber = "";
                Calc.ActionSignControl = false;
            }

            Calc.Formula += number;
            Calc.DisplayedNumber += number;


            return RedirectToAction("Index");
        }

        public IActionResult SetFunction(string function)
        {

            switch (function)
            {
                case "clear":
                    {
                        Calc.ResultNumber = 0;
                        Calc.DisplayedNumber = null;
                        Calc.Formula = null;
                        Calc.Action = null;
                        Calc.ActionSignControl = false;
                        break;
                    }

                case "clearLast":
                    {
                        if (Calc.DisplayedNumber != null && Calc.DisplayedNumber.Length > 0 && !Calc.ActionSignControl)
                        {
                            Calc.DisplayedNumber = Calc.DisplayedNumber.Substring(0, Calc.DisplayedNumber.Length - 1);
                            Calc.Formula = Calc.Formula.Substring(0, Calc.Formula.Length - 1);
                        }

                        break;
                    }

                case "equals":
                    {
                        Calculate();
                        Calc.Action = "save";
                        Calc.Formula = Calc.ResultNumber.ToString();
                        Calc.ActionSignControl = false;
                        break;
                    }

                case "changeSign":
                    {
                        if (Calc.DisplayedNumber != null && Calc.DisplayedNumber.Length > 0 && !Calc.ActionSignControl)
                        {
                            Calc.Formula = Calc.Formula.Substring(0, Calc.Formula.Length - Calc.DisplayedNumber.Length);
                            Calc.DisplayedNumber = (-Convert.ToDouble(Calc.DisplayedNumber)).ToString();
                            Calc.Formula += Calc.DisplayedNumber;
                        }
                        break;
                    }

                case "add":
                    {
                        Calculate();
                        Calc.Action = "add";
                        Calc.Formula += "+";
                        Calc.ActionSignControl = true;
                        break;
                    }

                case "substract":
                    {
                        Calculate();
                        Calc.Action = "substract";
                        Calc.Formula += "-";
                        Calc.ActionSignControl = true;
                        break;
                    }


                case "multiply":
                    {
                        Calculate();
                        Calc.Formula += "*";
                        Calc.Action = "multiply";
                        Calc.ActionSignControl = true;
                        break;
                    }

                case "divide":
                    {
                        Calculate();
                        Calc.Formula += "÷";
                        Calc.Action = "divide";
                        Calc.ActionSignControl = true;
                        break;
                    }
                case "percentage":
                    {
                        Calc.DisplayedNumber = (Convert.ToDouble(Calc.ResultNumber) * (Convert.ToDouble(Calc.DisplayedNumber) / 100)).ToString();
                        Calc.Formula += "%";
                        if (Calc.Action != "multiply")
                            Calculate();
                        Calc.Action = "save";
                        break;
                    }
                case "fraction":
                    {
                        if (Calc.DisplayedNumber != null && Calc.DisplayedNumber.Length > 0 && !Calc.ActionSignControl)
                        {
                            Calc.Formula = Calc.Formula.Substring(0, Calc.Formula.Length - Calc.DisplayedNumber.Length);
                            Calc.Formula += "1/" + Calc.DisplayedNumber;
                        }
                        Calc.DisplayedNumber = (1 / Convert.ToDouble(Calc.DisplayedNumber)).ToString();

                        break;
                    }
                case "power":
                    {
                        Calculate();
                        Calc.Formula += "^";
                        Calc.Action = "power";
                        Calc.ActionSignControl = true;
                        break;
                    }
                case "root":
                    {

                        if (Calc.DisplayedNumber != null && Calc.DisplayedNumber.Length > 0 && !Calc.ActionSignControl)
                        {
                            Calc.Formula = Calc.Formula.Substring(0, Calc.Formula.Length - Calc.DisplayedNumber.Length);
                            Calc.Formula += "√" + Calc.DisplayedNumber;
                        }
                        Calc.DisplayedNumber = Math.Sqrt(Convert.ToDouble(Calc.DisplayedNumber)).ToString();
                        break;
                    }

                default:
                    break;
            }



            return RedirectToAction("Index");
        }

        public void Calculate()
        {
            if (Calc.Formula != null && Calc.ActionSignControl && Calc.Formula.Length > 0)
            {
                Calc.Formula = Calc.Formula.Substring(0, Calc.Formula.Length - 1);
            }
            else if (Calc.DisplayedNumber != null && Calc.DisplayedNumber.Length > 0)
            {
                Calc.ActionSignControl = false;
                switch (Calc.Action)
                {
                    case "add":
                        Calc.ResultNumber += Convert.ToDouble(Calc.DisplayedNumber);

                        break;
                    case "substract":
                        Calc.ResultNumber -= Convert.ToDouble(Calc.DisplayedNumber);

                        break;
                    case "multiply":
                        Calc.ResultNumber *= Convert.ToDouble(Calc.DisplayedNumber);

                        break;
                    case "divide":
                        Calc.ResultNumber = Calc.ResultNumber / Convert.ToDouble(Calc.DisplayedNumber);
                        break;


                    case "power":
                        Calc.ResultNumber = Math.Pow(Calc.ResultNumber, Convert.ToDouble(Calc.DisplayedNumber));
                        break;


                    default:
                        Calc.ResultNumber = Convert.ToDouble(Calc.DisplayedNumber);
                        return;
                }
                Calc.DisplayedNumber = Calc.ResultNumber.ToString();
            }
        }
    }
}
