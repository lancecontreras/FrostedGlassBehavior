using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace FrostedGlassBehavior
{
  public class FrostedGlassBehavior : Behavior<FrameworkElement>
  {
    static BitmapImage _blurredBackground = null;
    static int _tickCtr = 0;
    static bool _isBlurred = false;

    Frame _layoutRoot;
    DispatcherTimer _timer = new DispatcherTimer();
    FrameworkElement _panel;

    protected override void OnAttached()
    {
      _panel = this.AssociatedObject as FrameworkElement;
      _layoutRoot = (Window.Current.Content as Frame);
      _panel.Loaded += Panel_Loaded;
    }

    private void Layoutroot_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      _layoutRoot.SizeChanged -= Layoutroot_SizeChanged;
      Refresh();
    }

    private void _layoutRoot_Navigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
    {
      _layoutRoot.Navigated -= _layoutRoot_Navigated;
      Refresh();
    }


    void Refresh()
    {
      _layoutRoot.SizeChanged -= Layoutroot_SizeChanged;
      _blurredBackground = null;
      _tickCtr = 0;
      _isBlurred = false;
      _timer.Start();
    }


    private async void _timer_Tick(object sender, object e)
    {
      if (_panel.Visibility == Visibility.Visible)
        _panel.Visibility = Visibility.Collapsed;
      if (_isBlurred == false && _panel.Visibility == Visibility.Collapsed)
      {
        if (_blurredBackground == null)
        {
          _blurredBackground = await _layoutRoot.GetBlurredBackground(8.0f);
          _isBlurred = true;
        }
      }
      if (_isBlurred)
      {
        Point panelPoint = _panel.GetCoordinates(_layoutRoot, false);
        Point layoutRootPoint = _layoutRoot.GetCoordinates(_layoutRoot, false);

        var distance = GetDistance(panelPoint, layoutRootPoint);
        Transform moveTransform = new TranslateTransform() { X = distance.X, Y = distance.Y };
        Brush backgroundBrush = new ImageBrush() { ImageSource = _blurredBackground, Stretch = Stretch.None, Transform = moveTransform, AlignmentX = AlignmentX.Center, AlignmentY = AlignmentY.Center };

        if (_panel is Border)
          (_panel as Border).Background = backgroundBrush;
        if (_panel is Panel)
          (_panel as Panel).Background = backgroundBrush;

        _panel.Visibility = Visibility.Visible;
        (sender as DispatcherTimer).Stop();
        _layoutRoot.SizeChanged += Layoutroot_SizeChanged;
        _layoutRoot.Navigated += _layoutRoot_Navigated; ;
      }
      _tickCtr++;
    }


    private void Panel_Loaded(object sender, RoutedEventArgs e)
    {
      _panel.Loaded -= Panel_Loaded;
      _timer.Interval = new TimeSpan(10);
      _timer.Tick += _timer_Tick;
      _timer.Start();
    }

    protected override void OnDetaching()
    {
      //base.OnDetaching();
      _timer.Stop();
    }
    private Point GetDistance(Point p, Point p2)
    {
      return new Point(((-1 * p2.X) + p.X) * -1, ((-1 * p2.Y) + p.Y) * -1);
    }
  }
}
