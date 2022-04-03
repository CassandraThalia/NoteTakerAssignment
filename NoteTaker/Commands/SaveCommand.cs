using NoteTaker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                //First check if the name has been left blank
                if (snd.FileName == null)
                {
                    ContentDialog blankDialog = new ContentDialog()
                    {
                        Title = "Blank File Name",
                        Content = "File name cannot be blank",
                        PrimaryButtonText = "OK"
                    };
                    await blankDialog.ShowAsync();
                    Execute(null);
                }

                //Then check if the file name contains illegal characters
                else if (checkForInvalidChars(snd.FileName)){
                    ContentDialog invalidDialog = new ContentDialog()
                    {
                        Title = "Invalid File Characters",
                        Content = "File name cannot contain illegal characters",
                        PrimaryButtonText = "OK"
                    };
                    await invalidDialog.ShowAsync();
                    Execute(null);
                }

                //Then check if the name is a duplicate (if it is a new note, not an update)
                else if (checkForDuplicate(snd.FileName, nvm.Notes, nvm.SelectedNote))
                {
                    ContentDialog dupDialog = new ContentDialog()
                    {
                        Title = "Duplicate Filename Error",
                        Content = "Please enter an original name for your note",
                        PrimaryButtonText = "OK",
                    };
                    await dupDialog.ShowAsync();
                    Execute(null);
                }

                //Proceed to save
                else
                {
                    try
                    {
                        NoteModel nm;
                        //Call Save Notes function from NoteRepo
                        //Repositories.NoteRepo.SaveNotesToFile(nvm.NoteContent, snd.FileName, nvm.Notes, nvm.SelectedNote);
                        if (nvm.SelectedNote != null && snd.FileName == nvm.SelectedNote.Title)
                        {
                            nm = Repositories.DataRepo.UpdateData(nvm.NoteContent, snd.FileName);
                            if (nvm.Notes.Contains(nvm.SelectedNote))
                            {
                                nvm.Notes.Remove(nvm.SelectedNote);
                            }
                        }
                        else
                        {
                            Repositories.DataRepo.AddData(snd.FileName, nvm.NoteContent);
                            nm = new NoteModel(nvm.NoteContent, snd.FileName);
                        }

                        nvm.Notes.Add(nm);
                        nvm.RefreshSearchList();

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
                        Debug.Write("File Save Error" + e);
                    }
                }
                nvm.PrepFreshNote();
            }
        }

        //Moved the check for duplicate function here from the NoteRepo to be able to re-call the execute function if necessary
        public static bool checkForDuplicate(String fileName, ObservableCollection<NoteModel> notes, NoteModel selected)
        {
            bool dup = false;
            if (selected == null || fileName != selected.Title)
            {
                foreach (NoteModel note in notes)
                {
                    if (fileName == note.Title)
                    {
                        dup = true;
                    }
                }
            }
            return dup;
        }

        public static bool checkForInvalidChars(String fileName)
        {
            bool inv = false;
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                if (fileName.Contains(c))
                {
                    inv = true;
                }
            }
            return inv;
        }
    }
}

