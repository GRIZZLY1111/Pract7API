using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Pract7.ValidationRules
{
    public class NecessarilyValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var name = (value ?? "").ToString().Trim();
            if (name == string.Empty)
            {
                return new ValidationResult(false, "Это поле ввода является обязательным");
            }
            return ValidationResult.ValidResult;
        }
    }
}
