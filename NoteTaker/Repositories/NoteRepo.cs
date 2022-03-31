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
using NoteTaker.Commands;

namespace NoteTaker.Repositories
{
    public class NoteRepo
    {
        public static String createFileName(String fileName)
        {
            return fileName + ".txt";
        }

        public static void UpdateNotesOnSave(String fileName, ObservableCollection<NoteModel> notes, NoteModel selected, NoteModel newNote)
        {
            //If it is an update of an existing note, remove it first and replace it with new note (maybe cheating shhh)
            if (selected != null)
            {
                if (selected.Title == fileName)
                {
                    notes.Remove(selected);
                }
            }
            //Add new note to notes list
            notes.Add(newNote);
        }

        public async static void SaveNotesToFile(String noteContent, String fileName, ObservableCollection<NoteModel> notes, NoteModel selected)
        {
            string ffName = createFileName(fileName);

            StorageFolder _notesFolder = ApplicationData.Current.LocalFolder;

            try
            {
                //Create new file with note's properties
                StorageFile noteFile = await _notesFolder.CreateFileAsync(ffName, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(noteFile, noteContent);
                //Create new NoteModel with updated data
                NoteModel newNote = new NoteModel(noteContent, fileName);
                //Add it to notes observable collection
                UpdateNotesOnSave(fileName, notes, selected, newNote);
                
            }
            catch (Exception e)
            {
                Debug.Write("File save error" + e);
            }
               
        }

        private static void AddNotesToObservableCol(String fileName, ObservableCollection<NoteModel> notes, String noteContent)
        {
            if (fileName != ".txt")
            {
                NoteModel NM = new NoteModel(noteContent, Path.GetFileNameWithoutExtension(fileName));
                notes.Add(NM);
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
                    string noteContent = await FileIO.ReadTextAsync(file);
                    //It kept adding a blank note at the start for no reason, this is to account for that
                    AddNotesToObservableCol(file.Name, notes, noteContent);
                }
            }
            catch (Exception e)
            {
                Debug.Write("File load error" + e);
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
