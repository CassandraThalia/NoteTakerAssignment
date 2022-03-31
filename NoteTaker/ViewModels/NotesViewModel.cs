using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteTaker.Commands;
using NoteTaker.Models;
using Windows.Storage;

namespace NoteTaker.ViewModels
{
    public class NotesViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<NoteModel> Notes { get; set; }

        private List<NoteModel> _allNotes = new List<NoteModel>();

        public string NoteContent { get; set; }
        public string NoteTitle { get; set; }
        public bool ReadOnly { get; set; }
        public bool EditMode { get; set; }

        private NoteModel _selectedNote;

        public EditCommand EditCommand;
        public SaveCommand SaveCommand;
        public AddCommand AddCommand;
        public DeleteCommand DeleteCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        public NoteModel SelectedNote 
        { 
            get { return _selectedNote; } 
            set 
            {
                _selectedNote = value;

                //If selecting new note, set it first as ReadOnly
                ChangeReadOnly(true);

                if (value == null)
                {
                    NoteContent = "";
                    NoteTitle = "";
                }
                else
                {
                    NoteTitle = value.Title;
                    NoteContent = value.Note;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NoteContent"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NoteTitle"));
                EditCommand.Fire_CanExecuteChanged();
                DeleteCommand.Fire_CanExecuteChanged();
            }
        }

        //Filter functionality from class notes
        private string _filter;

        public string Filter
        {
            get { return _filter; }
            set
            {
                if (value == _filter) { return; }
                _filter = value;
                PerformFiltering();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Filter"));
            }
        }

        //Constructor
        public NotesViewModel()
        {
            EditCommand = new EditCommand(this);
            SaveCommand = new SaveCommand(this);
            AddCommand = new AddCommand(this);
            DeleteCommand = new DeleteCommand(this);

            EditMode = true;

            Notes = new ObservableCollection<NoteModel>();

            Repositories.NoteRepo.LoadNotesFromFile(Notes);

            RefreshSearchList(); 
        }

        //Function to add all notes from Notes ObservableCollection to _allNotes list
        public void RefreshSearchList()
        {
            foreach(NoteModel note in Notes)
            {
                if (!_allNotes.Contains(note))
                {
                    _allNotes.Add(note);
                }
            }
        }

        //This function is hooked up to ReadOnly property of textbox
        public void ChangeReadOnly(bool b)
        {
            ReadOnly = b;
            //Set edit mode for edit button
            if (b == false)
            {
                EditMode = true;
            }
            else
            {
                EditMode = false;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ReadOnly"));
            SaveCommand.Fire_CanExecuteChanged();
            EditCommand.Fire_CanExecuteChanged();
        }

        //Function to reset textbox and buttons for new entry
        public void PrepFreshNote()
        {
            if (SelectedNote != null)
            {
                SelectedNote = null;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedNote"));
            ChangeReadOnly(false);
        }

        //Filtering functionality from class notes
        private void PerformFiltering()
        {
            RefreshSearchList();

            if (_filter == null)
            {
                _filter = "";
            }
            //If _filter has a value (ie. user entered something in Filter textbox)
            //Lower-case and trim string
            var lowerCaseFilter = Filter.ToLowerInvariant().Trim();

            //Use LINQ query to get all personmodel names that match filter text, as a list
            var result =
                _allNotes.Where(d => d.TitlesAsString.ToLowerInvariant()
                .Contains(lowerCaseFilter))
                .ToList();

            //Get list of values in current filtered list that we want to remove
            //(ie. don't meet new filter criteria)
            var toRemove = Notes.Except(result).ToList();

            //Loop to remove items that fail filter
            foreach (var x in toRemove)
            {
                Notes.Remove(x);
            }

            var resultCount = result.Count;
            // Add back in correct order.
            for (int i = 0; i < resultCount; i++)
            {
                var resultItem = result[i];
                if (i + 1 > Notes.Count || !Notes[i].Equals(resultItem))
                {
                    Notes.Insert(i, resultItem);
                }
            }
        }
    }
}
