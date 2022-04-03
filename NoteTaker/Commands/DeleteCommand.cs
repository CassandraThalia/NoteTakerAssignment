using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using NoteTaker.Repositories;
using System.Diagnostics;

namespace NoteTaker.Commands
{
    public class DeleteCommand : ICommand
    {
        private ViewModels.NotesViewModel _nvm;
        public event EventHandler CanExecuteChanged;

        public DeleteCommand(ViewModels.NotesViewModel nvm)
        {
            this._nvm = nvm;
        }

        public void Fire_CanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }


        public bool CanExecute(object parameter)
        {
            return _nvm.SelectedNote != null;
        }

        public async void Execute(object parameter)
        {
            ContentDialog confirmDelete = new ContentDialog()
            {
                Title = "Delete Note?",
                Content = "Are you sure you want to delete your note?",
                PrimaryButtonText = "Delete",
                SecondaryButtonText = "Cancel"
            };
            ContentDialogResult result = await confirmDelete.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                try
                {
                    //NoteRepo.DeleteNotes(_nvm.Notes, _nvm.SelectedNote);
                    DataRepo.DeleteData(_nvm.SelectedNote.Title);
                    _nvm.Notes.Remove(_nvm.SelectedNote);
                    _nvm.RefreshSearchList();

                    ContentDialog deledeD = new ContentDialog()
                    {
                        Title = "Note Deleted",
                        Content = "You successfully deleted your note!",
                        PrimaryButtonText = "OK"
                    };
                    await deledeD.ShowAsync();
                }
                catch (Exception e)
                {
                    Debug.Write("File delete error" + e);
                }
                _nvm.PrepFreshNote();
            }
            else if (result == ContentDialogResult.Secondary)
            {
                //Cancel delete
            }
        }
    }
}
