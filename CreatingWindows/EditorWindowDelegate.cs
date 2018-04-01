using System;
using AppKit;
using Foundation;

namespace CreatingWindows
{
    public class EditorWindowDelegate : NSWindowDelegate
    {
		public override bool WindowShouldClose(NSObject sender)
		{
            return base.WindowShouldClose(sender);
		}
	}
}
