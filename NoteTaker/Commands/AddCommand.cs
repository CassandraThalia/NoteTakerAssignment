using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NoteTaker.Commands
{
    public class AddCommand : ICommand
    {
        private ViewModels.NotesViewModel nvm;
        public event EventHandler CanExecuteChanged;

        public AddCommand(ViewModels.NotesViewModel nvm)
        {
            this.nvm = nvm;
        }

        public void Fire_CanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            nvm.PrepFreshNote();
        }
    }
}
