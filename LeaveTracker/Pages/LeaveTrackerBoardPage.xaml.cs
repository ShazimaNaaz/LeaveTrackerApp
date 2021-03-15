using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using LeaveTracker.Controls;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LeaveTracker.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeaveTrackerBoardPage : ContentPage
    {
        SKPaintSurfaceEventArgs args;
        TrackerUtils progressUtils = new TrackerUtils();
        //int dailyWorkout = 25;
        //int monthlyWorkout = 340;
        //int goal = 900;

        int leaveBalance = 25;
        int goal = 360*40;

        public LeaveTrackerBoardPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            // Drawing the Radial Gauge
            initiateProgressUpdate();


        }

        // Event Handler for Switch Toggle
        void switchToggledAsync(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            initiateProgressUpdate();
        }


        void sliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            if (canvas != null)
            {
                // Invalidating surface due to data change
                canvas.InvalidateSurface();
            }
        }


        // Animating the Progress of Radial Gauge
        async void animateProgress(int progress)
        {

            // Looping at data interval of 5
            for (int i = 0; i < progress; i = i + 5)
            {
                sweepAngleSlider.Value = i;
                await Task.Delay(3);
            }
        }

        void initiateProgressUpdate()
        {
            //animateProgress(progressUtils.getSweepAngle(goal / 30, leaveBalance));
            animateProgress(62);
        }

        public async Task drawGaugeAsync()
        {
            // Radial Gauge Constants
            //int uPadding = 150;
            //int side = 500;
            int uPadding = 120;
            int side = 450;
            int radialGaugeWidth = 12;

            // Line TextSize inside Radial Gauge
            int lineSize1 = 450;
            int lineSize2 = 70;
            int lineSize3 = 80;

            // Line Y Coordinate inside Radial Gauge
            int lineHeight1 = 250;
            int lineHeight2 = 400;
            int lineHeight3 = 500;

            // Start & End Angle for Radial Gauge
            float startAngle = -210;
            float sweepAngle = 240;

            try
            {
                // Getting Canvas Info 
                SKImageInfo info = args.Info;
                SKSurface surface = args.Surface;
                SKCanvas canvas = surface.Canvas;
                progressUtils.setDevice(info.Height, info.Width);
                canvas.Clear();

                // Getting Device Specific Screen Values
                // -------------------------------------------------

                // Top Padding for Radial Gauge
                float upperPading = progressUtils.getFactoredHeight(uPadding);

                /* Coordinate Plotting for Radial Gauge
                *
                *    (X1,Y1) ------------
                *           |   (XC,YC)  |
                *           |      .     |
                *         Y |            |
                *           |            |
                *            ------------ (X2,Y2))
                *                  X
                *   
                *To fit a perfect Circle inside --> X==Y
                *       i.e It should be a Square
                */

                // Xc & Yc are center of the Circle
                int Xc = info.Width / 2;
                float Yc = progressUtils.getFactoredHeight(side);

                // X1 Y1 are lefttop cordiates of rectange
                int X1 = (int)(Xc - Yc);
                int Y1 = (int)(Yc - Yc + upperPading);

                // X2 Y2 are rightbottom cordiates of rectange
                int X2 = (int)(Xc + Yc);
                int Y2 = (int)(Yc + Yc + upperPading);

                //Loggig Screen Specific Calculated Values
                Debug.WriteLine("INFO " + info.Width + " - " + info.Height);
                Debug.WriteLine(" C : " + upperPading + "  " + info.Height);
                Debug.WriteLine(" C : " + Xc + "  " + Yc);
                Debug.WriteLine("XY : " + X1 + "  " + Y1);
                Debug.WriteLine("XY : " + X2 + "  " + Y2);

                //  Empty Gauge Styling
                SKPaint paint1 = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = Color.FromHex("#ffffff").ToSKColor(),                   // Colour of Radial Gauge
                    StrokeWidth = progressUtils.getFactoredWidth(radialGaugeWidth), // Width of Radial Gauge
                    StrokeCap = SKStrokeCap.Round                                   // Round Corners for Radial Gauge
                };

                // Filled Gauge Styling
                SKPaint paint2 = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = Color.FromHex("#FCBC45").ToSKColor(),                   // Overlay Colour of Radial Gauge
                    StrokeWidth = progressUtils.getFactoredWidth(radialGaugeWidth), // Overlay Width of Radial Gauge
                    StrokeCap = SKStrokeCap.Round                                   // Round Corners for Radial Gauge
                };

                // Defining boundaries for Gauge
                SKRect rect = new SKRect(X1, Y1, X2, Y2);

                // Rendering Empty Gauge
                SKPath path1 = new SKPath();
                path1.AddArc(rect, startAngle, sweepAngle);
                canvas.DrawPath(path1, paint1);

                // Rendering Filled Gauge
                SKPath path2 = new SKPath();
                path2.AddArc(rect, startAngle, (float)sweepAngleSlider.Value);
                canvas.DrawPath(path2, paint2);

                //---------------- Drawing Text Over Gauge ---------------------------

                // Achieved Leave count
                using (SKPaint skPaint = new SKPaint())
                {
                    skPaint.Style = SKPaintStyle.Fill;
                    skPaint.IsAntialias = true;
                    skPaint.Color = SKColor.Parse("#FFFFFF");
                    skPaint.TextAlign = SKTextAlign.Center;
                    skPaint.TextSize = progressUtils.getFactoredHeight(lineSize1);
                    skPaint.Typeface = SKTypeface.FromFamilyName(
                                        "Arial",
                                        SKFontStyleWeight.Bold,
                                        SKFontStyleWidth.SemiCondensed,
                                        SKFontStyleSlant.Upright);
                        canvas.DrawText(leaveBalance + "", Xc, Yc + progressUtils.getFactoredHeight(lineHeight1), skPaint);
                }

                // Achieved LEAVES AVAILABLE Text Styling
                using (SKPaint skPaint = new SKPaint())
                {
                    skPaint.Style = SKPaintStyle.Fill;
                    skPaint.IsAntialias = true;
                    skPaint.Color = SKColor.Parse("#FFFFFF");
                    skPaint.TextAlign = SKTextAlign.Center;
                    skPaint.TextSize = progressUtils.getFactoredHeight(lineSize2);
                    skPaint.Typeface = SKTypeface.FromFamilyName(
                                        "Arial",
                                        SKFontStyleWeight.Bold,
                                        SKFontStyleWidth.Normal,
                                        SKFontStyleSlant.Upright);
                    canvas.DrawText("LEAVES AVAILABLE", Xc, Yc + progressUtils.getFactoredHeight(lineHeight2), skPaint);
                }

                // Goal Total leaves Text Styling
                using (SKPaint skPaint = new SKPaint())
                {
                    skPaint.Style = SKPaintStyle.Fill;
                    skPaint.IsAntialias = true;
                    skPaint.Color = SKColor.Parse("#FFFFFF");
                    skPaint.TextAlign = SKTextAlign.Center;
                    Padding = 6;
                    skPaint.TextSize = progressUtils.getFactoredHeight(lineSize3);
                    skPaint.Typeface = SKTypeface.FromFamilyName(
                                        "Arial",
                                        SKFontStyleWeight.Bold,
                                        SKFontStyleWidth.Normal,
                                        SKFontStyleSlant.Upright);

                    // Drawing Text Over Radial Gauge

                    canvas.DrawText("OUT OF 40 ANNUAL LEAVES", Xc, Yc + progressUtils.getFactoredHeight(lineHeight3), skPaint);
                    
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }

        async void canvas_PaintSurface(System.Object sender, SKPaintSurfaceEventArgs e)
        {
            args = e;
            await drawGaugeAsync();
        }
    }
}
