# FrostedGlassBehavior
This is a xaml behavior that can be added to a panel or a border element that changes its background to a transparent frosted glass effect using Win2d and XAML Behavior.

Required Package: 
Win2D.uwp 
Microsoft.Xaml.Behaviors.Uwp.Managed

Usage: 

```xml
<Border>
  <i:Interaction.Behaviors>
    <FGB:FrostedGlassBehavior />
  </i:Interaction.Behaviors>
    <TextBlock Text="Hello World" Foreground="White"/>
</Border>
```

##Update: Windows 10 Fall Creator's update now supports acrylic. 
[Acrylic Style](https://docs.microsoft.com/en-us/windows/uwp/style/acrylic)

![Image of Yaktocat](https://github.com/lancecontreras/FrostedGlassBehavior/blob/master/ScreenShot.PNG)
