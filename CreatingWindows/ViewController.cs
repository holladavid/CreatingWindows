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


        #region properties
        public bool DocumentEdited
        {
            get { return View.Window.DocumentEdited; }
            set { View.Window.DocumentEdited = value; }
        }

        public string Text
        {
            get => DocumentEditor.TextStorage.ToString();
            set => DocumentEditor.TextStorage.MutableString.SetString((Foundation.NSString)value);
        }

        public string FilePath
        {
            get;
            set;
        }
        #endregion

        public override void ViewWillAppear()
		{
            base.ViewWillAppear();

            // Set Window Title
            this.View.Window.Title = "untitled";

            //View.Window.WillClose += (sender, e) => {
            //    // is the window dirty?
            //    if (DocumentEdited)
            //    {
            //        var alert = new NSAlert()
            //        {
            //            AlertStyle = NSAlertStyle.Critical,
            //            InformativeText = "We need to give the user the ability to save the document here...",
            //            MessageText = "Save Document",
            //        };
            //        alert.RunModal();
            //    }
            //};



            // Just trying to find the text control
            // Should have made an outlet directly :-)
            //if (TextEditor.Subviews.Any())
            //{
            //    if (TextEditor.Subviews.First() is NSClipView clipView)
            //    {
            //        if (clipView.Subviews.Any())
            //        {
            //            if (clipView.Subviews.First() is NSTextView textView)
            //                textView.TextStorage.DidProcessEditing += (sender, e) => {
            //                View.Window.DocumentEdited = true;
            //            };
            //        }
            //    }
            //}       
		}

		public override void AwakeFromNib()
		{
            base.AwakeFromNib();

            DocumentEditor.TextDidChange += (sender, e) => {
                // Mark the document as dirty
                DocumentEdited = true;
            };

            // Overriding this delegate is required to monitor the TextDidChange event
            DocumentEditor.ShouldChangeTextInRanges += (NSTextView view, NSValue[] values, string[] replacements) => {
                return true;
            };
		}
	}
}
