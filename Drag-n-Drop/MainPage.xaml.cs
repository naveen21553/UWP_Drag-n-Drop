using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Drag_n_Drop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void StackPanel_DragOver(object sender, DragEventArgs e)
        {
            //using Windows.ApplicationModel.DataTransfer;
            e.AcceptedOperation = DataPackageOperation.Copy;

            e.DragUIOverride.Caption = "Drop here";
            e.DragUIOverride.IsCaptionVisible = true;
            e.DragUIOverride.IsContentVisible = true;
            e.DragUIOverride.IsGlyphVisible = true;
        }

        private async void StackPanel_Drop(object sender, DragEventArgs e)
        {
            if(e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var items = await e.DataView.GetStorageItemsAsync();
                if (items.Any())
                {
                    var storageFile = items[0] as StorageFile;
                    var contentType = storageFile.ContentType;

                    StorageFolder folder = ApplicationData.Current.LocalCacheFolder;

                    PathTextBlock.Text = folder.Path;

                    if(contentType == "image/png" || contentType == "image/jpeg")
                    {
                        StorageFile newFile = await storageFile.CopyAsync(folder, storageFile.Name, NameCollisionOption.GenerateUniqueName);
                        var bitmapImage = new BitmapImage();
                        bitmapImage.SetSource(await storageFile.OpenAsync(FileAccessMode.Read));
                        ImageViewer.Source = bitmapImage;
                    }
                    else if(contentType == "audio/wav" || contentType == "audio/mpeg" || contentType == "audio/mp3")
                    {
                        StorageFile newFile = await storageFile.CopyAsync(folder, storageFile.Name, NameCollisionOption.GenerateUniqueName);
                        MediaPlayer.SetSource(await storageFile.OpenAsync(FileAccessMode.Read), contentType);
                        MediaPlayerStoryBoard.Begin();
                    }
                }
            }
        }
    }
}
