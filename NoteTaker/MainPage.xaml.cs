using System;
using System.Collections.Generic;
using System.Diagnostics;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace NoteTaker
{
    public sealed partial class MainPage : Page
    {
        public ViewModels.NotesViewModel NotesViewModel { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            this.NotesViewModel = new ViewModels.NotesViewModel();
        }

        private void aboutBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Frame.Navigate(typeof(AboutPage), null);
            }
            catch (Exception ex)
            {
                Debug.Write("Error opening About Page: " + ex);
            }
        }

        private async void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog confirmExit = new ContentDialog()
            {
                Title = "Confirm Exit",
                Content = "Are you sure you want to exit?",
                PrimaryButtonText = "Exit",
                SecondaryButtonText = "Cancel"
            };
            ContentDialogResult result = await confirmExit.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                Application.Current.Exit();
            }
            else
            {
                //Cancel exit
            }
        }
    }
}


//Resources:
//https://docs.microsoft.com/en-us/windows/apps/design/controls/command-bar
//https://stackoverflow.com/questions/32677597/how-to-exit-or-close-an-uwp-app-programmatically-windows-10
//https://stackoverflow.com/questions/3825433/c-sharp-remove-invalid-characters-from-filename
//Logo sourced from Canva.com
