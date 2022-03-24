using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NoteTaker.Commands
{
    public class EditCommand : ICommand
    {

        private ViewModels.NotesViewModel _nvm;

        public event EventHandler CanExecuteChanged;


        public EditCommand(ViewModels.NotesViewModel NVM)
        {
            _nvm = NVM;
        }

        public void Fire_CanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return !_nvm.EditMode;
        }

        public void Execute(object parameter)
        {
            _nvm.ChangeReadOnly(false);
        }
    }
}
