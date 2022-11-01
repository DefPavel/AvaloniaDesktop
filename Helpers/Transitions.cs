using Avalonia.Animation;
using System;

namespace AvaloniaDesktop.Helpers;

public static class Transitions
{
    public static IPageTransition Fade => new CrossFade(TimeSpan.FromMilliseconds(200));

    public static IPageTransition Slide => new PageSlide(TimeSpan.FromMilliseconds(200));
}

