using FrostedGlassBehavior;
using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace FrostedGlassBehavior
{
  public class ValidateInputBehavior : Behavior<TextBox>
  {
    protected override void OnAttached()
    {
      DependencyPropertyWatcher<string> watcher = new DependencyPropertyWatcher<string>(this.AssociatedObject, "Text");
      watcher.PropertyChanged += Watcher_PropertyChanged;
      base.OnAttached();
    }

    private void Watcher_PropertyChanged(object sender, EventArgs e)
    {
      this.AssociatedObject.TextChanging += AssociatedObject_TextChanging;
    }

    private void AssociatedObject_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
    {
      
    }
  }

  public class DependencyPropertyWatcher<T> : DependencyObject, IDisposable
  {
    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register(
            "Value",
            typeof(object),
            typeof(DependencyPropertyWatcher<T>),
            new PropertyMetadata(null, OnPropertyChanged));

    public event EventHandler PropertyChanged;

    public DependencyPropertyWatcher(DependencyObject target, string propertyPath)
    {
      this.Target = target;
      BindingOperations.SetBinding(
          this,
          ValueProperty,
          new Binding() { Source = target, Path = new PropertyPath(propertyPath), Mode = BindingMode.TwoWay });
    }

    public DependencyObject Target { get; private set; }

    public T Value
    {
      get { return (T)this.GetValue(ValueProperty); }
    }

    public static void OnPropertyChanged(object sender, DependencyPropertyChangedEventArgs args)
    {
      DependencyPropertyWatcher<T> source = (DependencyPropertyWatcher<T>)sender;

      if (source.PropertyChanged != null)
      {
        source.PropertyChanged(source, EventArgs.Empty);
      }
    }

    public void Dispose()
    {
      this.ClearValue(ValueProperty);
    }
  }
}
