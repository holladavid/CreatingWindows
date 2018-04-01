using System;
using System.IO;
using AppKit;
using Foundation;

namespace CreatingWindows
{
    public class EditorWindowDelegate : NSWindowDelegate
    {
        #region Computed Properties
        public NSWindow Window { get; set; }
        #endregion

        #region constructors
        public EditorWindowDelegate(NSWindow window)
        {
            // Initialize
            this.Window = window;

        }
        #endregion

        #region Override Methods
        public override bool WindowShouldClose(NSObject sender)
        {
            // is the window dirty?
            if (Window.DocumentEdited)
            {
                var alert = new NSAlert()
                {
                    AlertStyle = NSAlertStyle.Critical,
                    InformativeText = "Save changes to document before closing window?",
                    MessageText = "Save Document",
                };
                alert.AddButton("Save");
                alert.AddButton("Lose Changes");
                alert.AddButton("Cancel");
                var result = alert.RunSheetModal(Window);

                // Take action based on resu;t
                switch (result)
                {
                    case 1000:
                        // Grab controller
                        var viewController = Window.ContentViewController as ViewController;

                        // Already saved?
                        if (Window.RepresentedUrl != null)
                        {
                            var path = Window.RepresentedUrl.Path;

                            // Save changes to file
                            // File.WriteAllText(path, viewController.Text);
                            return true;
                        }
                        else
                        {
                            var dlg = new NSSavePanel();
                            dlg.Title = "Save Document";
                            dlg.BeginSheet(Window, (rslt) => {
                                // File selected?
                                if (rslt == 1)
                                {
                                    var path = dlg.Url.Path;
                                    // File.WriteAllText(path, viewController.Text);
                                    Window.DocumentEdited = false;
                                    viewController.View.Window.SetTitleWithRepresentedFilename(Path.GetFileName(path));
                                    viewController.View.Window.RepresentedUrl = dlg.Url;
                                    Window.Close();
                                }
                            });
                            return true;
                        }
                    case 1001:
                        // Lose Changes
                        return true;
                    case 1002:
                        // Cancel
                        return false;
                }
            }

            return true;
        }
        #endregion
    }
}
