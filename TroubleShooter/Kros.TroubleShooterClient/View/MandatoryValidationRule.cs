using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Kros.TroubleShooterClient.View
{
    public class MandatoryValidationRule : ValidationRule
    {

        public ValidationCondition Mandatory { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return new ValidationResult(!Mandatory.ValidationEnabled || !string.IsNullOrWhiteSpace(value as string), "povinný údaj");
        }
    }


    public class ValidationCondition : DependencyObject
    {
        public static readonly DependencyProperty ValidationEnabledProperty =
                DependencyProperty.Register("ValidationEnabled", typeof(bool), typeof(ValidationCondition), new FrameworkPropertyMetadata(false));

        public bool ValidationEnabled
        {
            get { return (bool)GetValue(ValidationEnabledProperty); }
            set { SetValue(ValidationEnabledProperty, value); }
        }
    }

    public class BindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new PropertyMetadata(null));
    }

}
