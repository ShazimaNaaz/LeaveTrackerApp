using System;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace LeaveTracker.Controls
{
    public class CircularProgressRing : SKCanvasView
    {
        public static readonly BindableProperty StrokeWidthProperty =
            BindableProperty.Create(nameof(StrokeWidth), typeof(float), typeof(CircularProgressRing), 10f, propertyChanged: OnPropertyChanged);

        public static readonly BindableProperty ProgressProperty =
            BindableProperty.Create(nameof(Progress), typeof(float), typeof(CircularProgressRing), 0f, propertyChanged: OnPropertyChanged);

        public static readonly BindableProperty ProgressStartColorProperty =
            BindableProperty.Create(nameof(ProgressStartColor), typeof(Color), typeof(CircularProgressRing), Color.Blue, propertyChanged: OnPropertyChanged);

        public static readonly BindableProperty ProgressEndColorProperty =
            BindableProperty.Create(nameof(ProgressEndColor), typeof(Color), typeof(CircularProgressRing), Color.Red, propertyChanged: OnPropertyChanged);

        private const float StartAngle = -90;
        private const float SweepAngle = 360;

        public float StrokeWidth
        {
            get { return (float)GetValue(StrokeWidthProperty); }
            set { SetValue(StrokeWidthProperty, value); }
        }

        public float Progress
        {
            get { return (float)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }

        public Color ProgressStartColor
        {
            get { return (Color)GetValue(ProgressStartColorProperty); }
            set { SetValue(ProgressStartColorProperty, value); }
        }

        public Color ProgressEndColor
        {
            get { return (Color)GetValue(ProgressEndColorProperty); }
            set { SetValue(ProgressEndColorProperty, value); }
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            int size = Math.Min(info.Width, info.Height);
            int max = Math.Max(info.Width, info.Height);

            canvas.Translate((max - size) / 2, 0);

            canvas.Clear();
            canvas.Save();
            canvas.RotateDegrees(0, size / 2, size / 2);
            DrawProgressCircle(info, canvas);

            canvas.Restore();
        }

        private static void OnPropertyChanged(BindableObject bindable, object oldVal, object newVal)
        {
            var circleProgress = bindable as CircularProgressRing;
            circleProgress?.InvalidateSurface();
        }

        private void DrawProgressCircle(SKImageInfo info, SKCanvas canvas)
        {
            float progressAngle = SweepAngle * Progress;
            int size = Math.Min(info.Width, info.Height);

            var shader = SKShader.CreateSweepGradient(
                new SKPoint(size / 2, size / 2),
                new[]
                {
                    ProgressStartColor.ToSKColor(),
                    ProgressEndColor.ToSKColor(),
                    ProgressStartColor.ToSKColor()
                },
                new[]
                {
                    StartAngle / 360,
                    (StartAngle + progressAngle + 1) / 360,
                    (StartAngle + progressAngle + 2) / 360
                });

            var paint = new SKPaint
            {
                Shader = shader,
                StrokeWidth = StrokeWidth,
                IsStroke = true,
                IsAntialias = true,
                StrokeCap = SKStrokeCap.Round
            };

            DrawCircle(info, canvas, paint, progressAngle);

            // Line TextSize inside Radial Gauge
            int lineSize1 = 220;
            int lineSize2 = 70;
            int lineSize3 = 80;
            using (SKPaint skPaint = new SKPaint())
            {
                skPaint.Style = SKPaintStyle.Fill;
                skPaint.IsAntialias = true;
                skPaint.Color = SKColor.Parse("#676a69");
                skPaint.TextAlign = SKTextAlign.Center;
                skPaint.TextSize = getFactoredHeight(lineSize1);
                skPaint.Typeface = SKTypeface.FromFamilyName(
                                    "Arial",
                                    SKFontStyleWeight.Bold,
                                    SKFontStyleWidth.Normal,
                                    SKFontStyleSlant.Upright);
            }


            // Radial Gauge Constants
            int uPadding = 150;
            int side = 500;
            int radialGaugeWidth = 25;

            // Line Y Coordinate inside Radial Gauge
            int lineHeight1 = 100;
            int lineHeight2 = 200;
            int lineHeight3 = 300;

            // Start & End Angle for Radial Gauge
            float startAngle = -220;
            float sweepAngle = 260;
            int Xc = info.Width / 2;
            float Yc = getFactoredHeight(side);
            float upperPading = getFactoredHeight(uPadding);

            // X1 Y1 are lefttop cordiates of rectange
            int X1 = (int)(Xc - Yc);
            int Y1 = (int)(Yc - Yc + upperPading);

            // X2 Y2 are rightbottom cordiates of rectange
            int X2 = (int)(Xc + Yc);
            int Y2 = (int)(Yc + Yc + upperPading);

            // Achieved Minutes Text Styling
            using (SKPaint skPaint = new SKPaint())
            {
                skPaint.Style = SKPaintStyle.Fill;
                skPaint.IsAntialias = true;
                skPaint.Color = SKColor.Parse("#676a69");
                skPaint.TextAlign = SKTextAlign.Center;
                skPaint.TextSize = getFactoredHeight(lineSize2);
                canvas.DrawText("Minutes", Xc, Yc + getFactoredHeight(lineHeight2), skPaint);
            }

            // Goal Minutes Text Styling
            using (SKPaint skPaint = new SKPaint())
            {
                skPaint.Style = SKPaintStyle.Fill;
                skPaint.IsAntialias = true;
                skPaint.Color = SKColor.Parse("#e2797a");
                skPaint.TextAlign = SKTextAlign.Center;
                skPaint.TextSize = getFactoredHeight(lineSize3);

                    canvas.DrawText("Goal "  + " Min", Xc, Yc + getFactoredHeight(lineHeight3), skPaint);
            }
        }

        private void DrawCircle(SKImageInfo info, SKCanvas canvas, SKPaint paint, float angle)
        {
            int size = Math.Min(info.Width, info.Height);
            float halfWidth = size / 2;
            float halfHeight = size / 2;

            using (SKPath path = new SKPath())
            {
                SKRect rect = new SKRect(
                    StrokeWidth,
                    StrokeWidth,
                    size - StrokeWidth,
                    size - StrokeWidth);

                path.AddArc(rect, StartAngle, angle);

                canvas.DrawPath(path, paint);
            }
        }

        // Reference Values(Standard Pixel 1 Device)
        private const float refHeight = 1080;//1677;
        private const float refWidth = 632;//940;

        // Derived Proportinate Values
        private float deviceHeight = 1; // Initializing to 1
        private float deviceWidth = 1;  // Initializing to 1

        // Deriving Proportinate Height
        public float getFactoredHeight(int value)
        {
            return (float)((value / refHeight) * deviceHeight);
        }
    }
}
