using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MyControls
{
    public class CircularProgress : Shape
    {

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public Color FromColor
        {
            get { return (Color)GetValue(FromColorProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public Color ToColor
        {
            get { return (Color)GetValue(ToColorProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(CircularProgress),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault , OnUriChanged , null)
        );

        public static readonly DependencyProperty FromColorProperty = DependencyProperty.Register("FromColor", typeof(Color), typeof(CircularProgress),
            new FrameworkPropertyMetadata(Brushes.LightPink.Color , FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, null)
        );

        public static readonly DependencyProperty ToColorProperty = DependencyProperty.Register("ToColor", typeof(Color), typeof(CircularProgress),
            new FrameworkPropertyMetadata(Brushes.LightGreen.Color, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, null)
        );


        private static void OnUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CircularProgress)d).Value = (double)e.NewValue;
            ((CircularProgress)d).updateColor();
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                double endAngle = 90.0 - (trimValue() * 360.0);
                double startAngle = 90.0;

                double maxWidth = Math.Max(0.0, RenderSize.Width - StrokeThickness);
                double maxHeight = Math.Max(0.0, RenderSize.Height - StrokeThickness);

                double xStart = maxWidth / 2.0 * Math.Cos(startAngle * Math.PI / 180.0);
                double yStart = maxHeight / 2.0 * Math.Sin(startAngle * Math.PI / 180.0);

                double xEnd = maxWidth / 2.0 * Math.Cos(endAngle * Math.PI / 180.0);
                double yEnd = maxHeight / 2.0 * Math.Sin(endAngle * Math.PI / 180.0);

                StreamGeometry geom = new StreamGeometry();
                using (StreamGeometryContext ctx = geom.Open())
                {
                    ctx.BeginFigure(
                        new Point((RenderSize.Width / 2.0) + xStart,
                                  (RenderSize.Height / 2.0) - yStart),
                        false,   // Filled
                        false);  // Closed
                    ctx.ArcTo(
                        new Point((RenderSize.Width / 2.0) + xEnd,
                                  (RenderSize.Height / 2.0) - yEnd),
                        new Size(maxWidth / 2.0, maxHeight / 2),
                        0.0,     // rotationAngle
                        (startAngle - endAngle) > 180,   // greater than 180 deg?
                        SweepDirection.Clockwise,
                        true,    // isStroked
                        false);
                }
                return geom;
            }
        }

        private double trimValue()
        {
            if (Value < 0)
                return  0;
            else if (Value >= 0.99999)
                return 0.99999;
            else
                return Value;
        }

        private void updateColor()
        {
            Color actualColor = new Color();
            actualColor.R = Convert.ToByte(FromColor.R + (ToColor.R - FromColor.R) * trimValue());
            actualColor.G = Convert.ToByte(FromColor.G + (ToColor.G - FromColor.G) * trimValue());
            actualColor.B = Convert.ToByte(FromColor.B + (ToColor.B - FromColor.B) * trimValue());
            actualColor.ScA = 255;
            Stroke = new SolidColorBrush(actualColor);
        }
    }
}
