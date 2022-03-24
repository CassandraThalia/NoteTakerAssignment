using NoteTaker.Models;
using NoteTaker.ViewModels;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace NoteTaker.Repositories
{
    public class NoteRepo
    {
        //public StorageFolder _notesFolder = ApplicationData.Current.LocalFolder;

        public async static void SaveNotesToFile(String noteContent, String fileName, ObservableCollection<NoteModel> notes, NoteModel selected)
        {
            //First, check for duplicate file names
            bool dup = false;
            if (selected == null)
            {
                foreach (NoteModel note in notes)
                {
                    if (fileName == note.Title)
                    {
                        dup = true;
                    }
                }
            }
            if (dup)
            {
                ContentDialog dupDialog = new ContentDialog()
                {
                    Title = "Duplicate Filename Error",
                    Content = "Please enter an original name for your note",
                    PrimaryButtonText = "OK",
                };
                await dupDialog.ShowAsync();
            }
            //If not a duplicate, proceed with save
            else
            {
                string ffName = fileName + ".txt";

                //No idea why, but my computer really did not like it when I made this a private member variable, 
                //Would only work for write or read when I instantiated the StorageFolder within the function...
                StorageFolder _notesFolder = ApplicationData.Current.LocalFolder;

                try
                {
                    //Create new file with note's properties
                    StorageFile noteFile = await _notesFolder.CreateFileAsync(ffName, CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteTextAsync(noteFile, noteContent);
                    //Create new NoteModel with updated data
                    NoteModel nm = new NoteModel(noteContent, fileName);
                    //If it is an update of an existing note, remove it first
                    if (selected != null)
                    {
                        if (selected.Title == fileName)
                        {
                            notes.Remove(selected);
                        }
                    }
                    //Add new note to notes list
                    notes.Add(nm);
                }
                catch (Exception e)
                {
                    Debug.Write(e);
                }
            }    
        }

        public async static void LoadNotesFromFile(ObservableCollection<NoteModel> notes)
        {
            StorageFolder _notesFolder = ApplicationData.Current.LocalFolder;

            try
            {
                IReadOnlyList<StorageFile> fileList = await _notesFolder.GetFilesAsync();

                //Loop through each file and add note info to Note objects, then add them to notes list
                foreach (StorageFile file in fileList)
                {
                    string text = await FileIO.ReadTextAsync(file);
                    //It kept adding a blank note at the start for no reason, this is to account for that
                    if (file.Name != ".txt")
                    {
                        NoteModel NM = new NoteModel(text, Path.GetFileNameWithoutExtension(file.Name));
                        notes.Add(NM);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Write(e);
            }
        }

        public async static void DeleteNotes(ObservableCollection<NoteModel> notes, NoteModel selected)
        {
            StorageFolder _notesFolder = ApplicationData.Current.LocalFolder;
            try
            {
                //Find file and delete it from local folder
                StorageFile file = await _notesFolder.GetFileAsync(selected.Title + ".txt");
                await file.DeleteAsync();
                //Remove it from notes list
                notes.Remove(selected);

                //Show confirmation dialog
                ContentDialog remDialog = new ContentDialog()
                {
                    Title = "Note Deleted",
                    Content = "You deleted your note!",
                    PrimaryButtonText = "OK",
                };
                await remDialog.ShowAsync();
            }
            catch (Exception e)
            {
                Debug.Write(e);
            }
        }
    }
}
