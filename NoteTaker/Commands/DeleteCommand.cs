using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

        public void Execute(object parameter)
        {
            Repositories.NoteRepo.DeleteNotes(_nvm.Notes, _nvm.SelectedNote);
            _nvm.PrepFreshNote();
        }
    }
}
