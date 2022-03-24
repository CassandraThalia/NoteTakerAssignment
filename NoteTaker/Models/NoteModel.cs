using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteTaker.Models
{
    public class NoteModel
    {
        public string Note { get; }
        public string Title { get; }

        public NoteModel(string p_note, string p_title)
        {
            Note = p_note;
            Title = p_title;
        }

        public string TitlesAsString
        {
            get { return string.Join(",", Title); }
        }
    }
}
