# FrostedGlassBehavior
## With FadeInAnimation
This is a xaml behavior that can be added to a panel or a border element that changes its background to a transparent frosted glass effect using Win2d and XAML Behavior.

This branch comes in with a nice FadeInAnimation. The storyboard was coded in the behavior so no changes needed externally.

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

![Image of Yaktocat](https://github.com/lancecontreras/FrostedGlassBehavior/blob/master/ScreenShot.PNG)
