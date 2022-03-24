using NoteTaker.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace NoteTaker.Commands
{
    public class SaveCommand : ICommand
    {
        private ViewModels.NotesViewModel nvm;
        public event EventHandler CanExecuteChanged;

        public SaveCommand(ViewModels.NotesViewModel nvm)
        {
            this.nvm = nvm;
        }

        public void Fire_CanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return !nvm.ReadOnly;
        }

        public async void Execute(object parameter)
        {
            //Create new Save Note Dialog
            SaveNoteDialog snd = new SaveNoteDialog(nvm.SelectedNote);
            ContentDialogResult result = await snd.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                try
                {
                    //Call Save Notes function from NoteRepo
                    Repositories.NoteRepo.SaveNotesToFile(nvm.NoteContent, snd.FileName, nvm.Notes, nvm.SelectedNote);
                    //Show confirm dialog if successful
                    ContentDialog saveD = new ContentDialog()
                    {
                        Title = "Note Saved",
                        Content = "You saved your note!",
                        PrimaryButtonText = "OK"
                    };
                    await saveD.ShowAsync();
                }
                catch (Exception e)
                {
                    Debug.Write("File Save Error");
                }
            }
            nvm.PrepFreshNote();
        }

    }
}

