using System;
using System.Linq;
using AppKit;
using Foundation;

namespace CreatingWindows
{
    public partial class ViewController : NSViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Do any additional setup after loading the view.
        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }

		public override void ViewWillAppear()
		{
            base.ViewWillAppear();

            // Set Window Title
            this.View.Window.Title = "untitled";

            // Just trying to find the text control
            // Should have made an outlet directly :-)
            if (TextEditor.Subviews.Any())
            {
                if (TextEditor.Subviews.First() is NSClipView clipView)
                {
                    if (clipView.Subviews.Any())
                    {
                        if (clipView.Subviews.First() is NSTextView textView)
                            textView.TextStorage.DidProcessEditing += (sender, e) => {
                            View.Window.DocumentEdited = true;
                        };
                    }
                }
            }       
		}
	}
}
