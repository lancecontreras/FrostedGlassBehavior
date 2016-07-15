using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace FrostedGlassBehavior
{
  public static class Extensions
  {
    public static Point GetCoordinates(this FrameworkElement element, UIElement parent = null, bool zero = false)
    {
      if (parent == null)
        parent = Window.Current.Content;

      var transform = element.TransformToVisual(parent);
      Point point;
      if (zero)
        point = transform.TransformPoint(new Point(0, 0));
      else
        point = transform.TransformPoint(new Point(element.ActualWidth / 2, element.ActualHeight / 2));
      return point;
    }

    public static async Task<IRandomAccessStream> RenderToRandomAccessStreamAsync(this UIElement element)
    {
      if (element == null) throw new NullReferenceException();

      RenderTargetBitmap rtb = new RenderTargetBitmap();
      await rtb.RenderAsync(element);

      var pixelBuffer = await rtb.GetPixelsAsync();
      var pixels = pixelBuffer.ToArray();

      var displayInformation = DisplayInformation.GetForCurrentView();

      var stream = new InMemoryRandomAccessStream();
      var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
      encoder.SetPixelData(BitmapPixelFormat.Bgra8,
                           BitmapAlphaMode.Premultiplied,
                           (uint)rtb.PixelWidth,
                           (uint)rtb.PixelHeight,
                           displayInformation.RawDpiX,
                           displayInformation.RawDpiY,
                           pixels);

      await encoder.FlushAsync();
      stream.Seek(0);

      return stream;
    }

    public static async Task<BitmapImage> GetBlurredBackground(this UIElement element, float blurrAmount = 5.0f)
    {
      using (var stream = await element.RenderToRandomAccessStreamAsync())
      {
        var device = new CanvasDevice();
        var bitmap = await CanvasBitmap.LoadAsync(device, stream);

        var renderer = new CanvasRenderTarget(device,
                                              bitmap.SizeInPixels.Width,
                                              bitmap.SizeInPixels.Height, bitmap.Dpi);

        using (var ds = renderer.CreateDrawingSession())
        {
          var blur = new GaussianBlurEffect();
          blur.BlurAmount = blurrAmount;
          blur.Source = bitmap;
          ds.DrawImage(blur);
        }

        stream.Seek(0);
        await renderer.SaveAsync(stream, CanvasBitmapFileFormat.Png);

        BitmapImage image = new BitmapImage();
        image.SetSource(stream);
        return image;
      }
    }

    public static async Task<BitmapImage> GetBlurredImageAsync(this IRandomAccessStream stream)
    {
      var device = new CanvasDevice();

      var bitmap = await CanvasBitmap.LoadAsync(device, stream);

      var renderer = new CanvasRenderTarget(device,
                                            bitmap.SizeInPixels.Width,
                                            bitmap.SizeInPixels.Height, bitmap.Dpi);

      using (var ds = renderer.CreateDrawingSession())
      {
        var blur = new GaussianBlurEffect();
        blur.BlurAmount = 10.0f;
        blur.Source = bitmap;
        ds.DrawImage(blur);
      }

      stream.Seek(0);
      await renderer.SaveAsync(stream, CanvasBitmapFileFormat.Png);

      BitmapImage image = new BitmapImage();
      image.SetSource(stream);
      return image;
    }

  }
}
