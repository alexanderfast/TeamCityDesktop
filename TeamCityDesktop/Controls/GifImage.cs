using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace TeamCityDesktop.Controls
{
    internal class GifImage : Image
    {
        public static readonly DependencyProperty FrameIndexProperty =
            DependencyProperty.Register(
                "FrameIndex",
                typeof(int),
                typeof(GifImage),
                new UIPropertyMetadata(0, FrameIndexChanged));

        public static readonly DependencyProperty AutoStartProperty =
            DependencyProperty.Register(
                "AutoStart",
                typeof(bool),
                typeof(GifImage),
                new UIPropertyMetadata(true, AutoStartChanged));

        public static readonly DependencyProperty GifSourceProperty =
            DependencyProperty.Register(
                "GifSource",
                typeof(string),
                typeof(GifImage),
                new UIPropertyMetadata(string.Empty, GifSourceChanged));

        private Int32Animation animation;
        private GifBitmapDecoder decoder;
        private bool isInitialized;

        static GifImage()
        {
            VisibilityProperty.OverrideMetadata(
                typeof(GifImage),
                new FrameworkPropertyMetadata(VisibilityChanged));
        }

        public int FrameIndex
        {
            get { return (int)GetValue(FrameIndexProperty); }
            set { SetValue(FrameIndexProperty, value); }
        }

        public bool AutoStart
        {
            get { return (bool)GetValue(AutoStartProperty); }
            set { SetValue(AutoStartProperty, value); }
        }

        public string GifSource
        {
            get { return (string)GetValue(GifSourceProperty); }
            set { SetValue(GifSourceProperty, value); }
        }

        private void Initialize()
        {
            decoder = new GifBitmapDecoder(
                new Uri("pack://application:,,," + GifSource),
                BitmapCreateOptions.PreservePixelFormat,
                BitmapCacheOption.Default);
            int frameCount = decoder.Frames.Count;
            animation = new Int32Animation(
                0,
                decoder.Frames.Count - 1,
                new Duration(new TimeSpan(0, 0, 0, 0, frameCount * 100)))
                {RepeatBehavior = RepeatBehavior.Forever};
            Source = decoder.Frames[0];
            isInitialized = true;
            if (AutoStart && Visibility == Visibility.Visible)
            {
                StartAnimation();
            }
        }

        private static void VisibilityChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if ((Visibility)e.NewValue == Visibility.Visible)
            {
                ((GifImage)sender).StartAnimation();
            }
            else
            {
                ((GifImage)sender).StopAnimation();
            }
        }

        private static void FrameIndexChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var gifImage = sender as GifImage;
            if (gifImage != null)
            {
                gifImage.Source = gifImage.decoder.Frames[(int)e.NewValue];
            }
        }

        private static void AutoStartChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
                ((GifImage)sender).StartAnimation();
        }

        private static void GifSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((GifImage)sender).Initialize();
        }

        public void StartAnimation()
        {
            if (!isInitialized)
                Initialize();
            BeginAnimation(FrameIndexProperty, animation);
        }

        public void StopAnimation()
        {
            BeginAnimation(FrameIndexProperty, null);
        }
    }
}
