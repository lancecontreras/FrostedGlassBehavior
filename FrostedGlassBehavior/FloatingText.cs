using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace FrostedGlassBehavior
{
  public class FloatingText : ContentControl
  {
    DispatcherTimer dt = new DispatcherTimer();
    public FloatingText()
    {
      DefaultStyleKey = typeof(FloatingText);
      dt.Interval = new TimeSpan(0, 0, 0, 0, 1000);
      dt.Tick += Dt_Tick;
      dt.Start();
    }

    private void Dt_Tick(object sender, object e)
    {
      //throw new NotImplementedException();Refresh
      Refresh();
    }

    public string Text
    {
      get { return (string)GetValue(TextProperty); }
      set { SetValue(TextProperty, value); }
    }

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(FloatingText), new PropertyMetadata(string.Empty, textChanged));

    protected override void OnApplyTemplate()
    {
      //Refresh();
      dt.Start(); 
      base.OnApplyTemplate();
    }


    async void Refresh()
    {
      FloatingText ft = this as FloatingText;
      TextBlock tb = ft.FindDescendantByName("PART_SHADOWTEXT") as TextBlock;
      Grid pb = ft.FindDescendantByName("PART_BORDER") as Grid;


      if (tb != null && pb != null)
      {
        if (pb.Background is SolidColorBrush)
        {
          var bb = await pb.GetBlurredBackground(3.0f);
          pb.Background = new ImageBrush() { ImageSource = bb, Opacity = .2, Stretch = Stretch.None, AlignmentX = AlignmentX.Left, AlignmentY = AlignmentY.Top };
          dt.Stop();
        }
        else
        {
          pb.Background = new SolidColorBrush(Colors.Black);
        }
      }
    }

    private async static void textChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      FloatingText ft = d as FloatingText;
      ft.dt.Start(); 
    }
  }
}
