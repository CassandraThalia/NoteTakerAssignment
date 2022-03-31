
using System;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoteTaker.Repositories;
using NoteTaker.Models;

namespace NoteTakerUnitTest_Files
{
    [TestClass]

    //As per your instructions in class, I did not even attempt to cover all possible errors
    //I just included enough tests to (hopefully) show you that I understand how they work

    public class NoteTakerUnitTests
    {
        [TestMethod]
        //Test if 
        public void Test_NoteRepo_CreateFileName()
        {
            String fileName = "nametest";
            String output = "nametest.txt";

            Assert.AreEqual(output, NoteRepo.createFileName(fileName));
        }

        [TestMethod]
        public void Test_NoteModel_AddingNoteContent()
        {
            String noteContent = "dummyNoteContent";
            String noteTitle = "dummyNoteTitle";
            NoteModel nm = new NoteModel(noteContent, noteTitle);

            Assert.AreEqual(nm.Note, noteContent);
        }

        [TestMethod]
        public void Test_NoteModel_AddingNoteTitle()
        {
            String noteContent = "dummyNoteContent";
            String noteTitle = "dummyNoteTitle";
            NoteModel nm = new NoteModel(noteContent, noteTitle);

            Assert.AreEqual(nm.Title, noteTitle);
        }

        [TestMethod]
        public void Test_NoteRepo_LoadingNotesToObservableCollection()
        {
            ObservableCollection<NoteModel> notes = new ObservableCollection<NoteModel>();
            NoteRepo.LoadNotesFromFile(notes);

            Assert.IsNotNull(notes);
        }

        [TestMethod]
        public void Test_NoteRepo_AddNewNoteToObservableCollectionOnSave()
        {
            String fileName = "dummyFileName";
            NoteModel selectedNote = new NoteModel("note content before", fileName);
            NoteModel changedNote = new NoteModel("note content after", fileName);
            ObservableCollection<NoteModel> notes = new ObservableCollection<NoteModel>();

            NoteRepo.UpdateNotesOnSave(fileName, notes, selectedNote, changedNote);

            Assert.IsTrue(notes.Contains(changedNote));
        }

        [TestMethod]
        public void Test_NoteRepo_RemoveNoteFromObservableCollectionOnSave()
        {
            String fileName = "dummyFileName";
            NoteModel selectedNote = new NoteModel("note content before", fileName);
            NoteModel changedNote = new NoteModel("note content after", fileName);
            ObservableCollection<NoteModel> notes = new ObservableCollection<NoteModel>();

            NoteRepo.UpdateNotesOnSave(fileName, notes, selectedNote, changedNote);

            Assert.IsFalse(notes.Contains(selectedNote));
        }
    }
}
