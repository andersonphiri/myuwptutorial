using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using DragAndDrop1.Models;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DragAndDrop1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<BitmapItem> Items = new ObservableCollection<BitmapItem>();
        private ObservableCollection<string> Items2 = new ObservableCollection<string>();
        public MainPage()
        {
            this.InitializeComponent();
            BitmapImage bi = new BitmapImage();
            //InitializeList();
        }

        private async void InitializeList()
        {
            Uri fileUri = new Uri(@"ms-appx:///Assets/forDragAndDrop.jpg");
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(fileUri);
            BitmapImage bi = new BitmapImage();
            await bi.SetSourceAsync(await file.OpenReadAsync());
            Items.Add(new BitmapItem() { Source = bi });
        }

        private async void ImageForDragAndDrop_DragStarting(UIElement sender, DragStartingEventArgs args)
        {
            var ImageSender = sender as Image;
            List<IStorageItem> files = new List<IStorageItem>();
            Uri fileUri = new Uri(@"ms-appx:///Assets/forDragAndDrop.jpg");
            //var testUri = ImageSender.BaseUri.AbsolutePath;
            //var baseUri = ImageSender.BaseUri;
            //var name = ImageSender.Source.;
            //Uri fileUri = new Uri(testUri);
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(fileUri);
            files.Add(file);
            args.DragUI.SetContentFromDataPackage();
            args.Data.RequestedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy; //| Windows.ApplicationModel.DataTransfer.DataPackageOperation.Move;
            args.Data.SetStorageItems(files);
        }

        private async void Grid_Drop(object sender, DragEventArgs e)
        {
            var files = await e.DataView.GetStorageItemsAsync();
            foreach (StorageFile file in files)
            {
                try
                {
                    BitmapImage bi = new BitmapImage();
                   await  bi.SetSourceAsync(await file.OpenReadAsync());
                    Items.Add(new BitmapItem() { Source=bi});
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private void Grid_DragEnter(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy; //| Windows.ApplicationModel.DataTransfer.DataPackageOperation.Move;
        }
    }
}
