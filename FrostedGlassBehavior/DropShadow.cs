using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace FrostedGlassBehavior
{
  public class DropShadow : Behavior<TextBlock>
  {
    public DropShadow()
    {
    }

    private void Watch_PropertyChanged(object sender, EventArgs e)
    {
    }

    protected async override void OnAttached()
    {
      TextBlock tb = this.AssociatedObject as TextBlock;
      Panel p = tb.Parent as Panel;
      TextBlock sh = new TextBlock() { Text = tb.Text, Foreground = new SolidColorBrush(Colors.Black), FontWeight = FontWeights.Black};
      //BitmapImage bm = await sh.GetBlurredBackground();
//      Image im = new Image() { Source = bm };

      p.Children.Insert(p.Children.IndexOf(tb) - 1, sh);

      base.OnAttached();
    }


  }
}
