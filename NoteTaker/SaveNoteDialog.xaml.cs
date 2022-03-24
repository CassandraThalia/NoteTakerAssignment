using NoteTaker.Models;
using NoteTaker.ViewModels;
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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NoteTaker
{
    public sealed partial class SaveNoteDialog : ContentDialog
    { 
        public string FileName { get; set; }
        public SaveNoteDialog(NoteModel selected)
        {
            this.InitializeComponent();
            if (selected != null)
            {
                saveTB.Text = selected.Title;
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (saveTB.Text != null)
            {
                FileName = saveTB.Text;
            }
            else
            {
                saveTB.PlaceholderText = "Please enter a valid file name";
            }
            
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            //Exit button
        }
    }
}
