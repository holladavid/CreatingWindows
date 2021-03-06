using AppKit;
using Foundation;
using System.IO;

namespace CreatingWindows
{
    [Register("AppDelegate")]
    public partial class AppDelegate : NSApplicationDelegate
    {
        public int UntitledWindowCount { get; set; } = 1;

        public AppDelegate()
        {
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            // Insert code here to initialize your application
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }

        #region Menu items
        [Export("newDocument:")]
        void NewDocument(NSObject sender)
        {
            // Get new window
            var storyboard = NSStoryboard.FromName("Main", null);
            var controller = storyboard.InstantiateControllerWithIdentifier("MainWindow") as NSWindowController;

            // Display
            controller.ShowWindow(this);

            // Set the title
            controller.Window.Title = (++UntitledWindowCount == 1) ? "untitled" : string.Format("untitled {0}", UntitledWindowCount);
        }

        [Export("openDocument:")]
        void OpenDialog(NSObject sender)
        {
            var dlg = NSOpenPanel.OpenPanel;
            dlg.CanChooseFiles = true;
            dlg.CanChooseDirectories = false;

            if (dlg.RunModal() == 1)
            {
                // Nab the first file
                var url = dlg.Urls[0];

                if (url != null)
                {
                    var path = url.Path;

                    if (!IsFileOpenBringToFront(path))
                    {

                        // Get new window
                        var storyboard = NSStoryboard.FromName("Main", null);
                        var controller = storyboard.InstantiateControllerWithIdentifier("MainWindow") as NSWindowController;

                        // Display
                        controller.ShowWindow(this);

                        // Load the text into the window
                        var viewController = controller.Window.ContentViewController as ViewController;
                        viewController.Text = File.ReadAllText(path);
                        viewController.FilePath = path;
                        viewController.View.Window.SetTitleWithRepresentedFilename(Path.GetFileName(path));
                        viewController.View.Window.RepresentedUrl = url;
                    }

                }
            }
        }

        private bool IsFileOpenBringToFront(string path)
        {
            // Is the file already open?
            foreach (var window in NSApplication.SharedApplication.Windows)
            {
                if (window.ContentViewController is ViewController content && path == content.FilePath)
                {
                    // Bring window to front
                    window.MakeKeyAndOrderFront(this);
                    return true;
                }
            }
            return false;
        }
        #endregion



        public override NSApplicationTerminateReply ApplicationShouldTerminate(NSApplication sender)
		{
            // See if any window needs to be saved first
            foreach (NSWindow window in NSApplication.SharedApplication.Windows)
            {
                if (window.Delegate != null && !window.Delegate.WindowShouldClose(this))
                {
                    // Did the window terminate the close?
                    return NSApplicationTerminateReply.Cancel;
                }
            }

            // Allow normal termination
            return NSApplicationTerminateReply.Now;
		}
	}
}
