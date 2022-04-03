using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;
using System.Collections.ObjectModel;
using NoteTaker.Models;

namespace NoteTaker.Repositories
{
    public class DataRepo
    {
        public async static void InitializeDatabase()
        {
            //Create the db file in windows storage.
            await ApplicationData.Current.LocalFolder.CreateFileAsync("LocalNote.db", CreationCollisionOption.OpenIfExists);
            //Get full path to db
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "LocalNote.db");

            //open connection to db
            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))//Establish connection to a database
            {
                conn.Open(); //Open connection
                String tableCommand = "CREATE TABLE IF NOT EXISTS NotesTable " + //Define a SQL command (Cretea table statement)
                "(NoteID INTEGER PRIMARY KEY, " +
                "NoteTitle nvarchar(100) NOT NULL," +
                "NoteContent nvarchar(1000) NOT NULL);";
                SqliteCommand cmd = new SqliteCommand(tableCommand, conn); //Create a command object (running our string SQL commnd)
                cmd.ExecuteReader(); //Execute the sql command
            }
        }

        public static void AddData(string noteTitle, string noteContent)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "LocalNote.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;
                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO NotesTable (NoteTitle, NoteContent) VALUES (@NoteTitle, @NoteContent);";
                insertCommand.Parameters.AddWithValue("@NoteTitle", noteTitle);
                insertCommand.Parameters.AddWithValue("@NoteContent", noteContent);
                insertCommand.ExecuteReader();
                db.Close();
            }
        }

        public static ObservableCollection<NoteModel> GetData()
        {
            ObservableCollection<NoteModel> notes = new ObservableCollection<NoteModel>();

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "LocalNote.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand getNoteData = new SqliteCommand
                ("SELECT NoteTitle, NoteContent from NotesTable", db);
                SqliteDataReader notesQuery = getNoteData.ExecuteReader();

                while (notesQuery.Read())
                {
                    NoteModel nm = new NoteModel(notesQuery[1].ToString(), notesQuery[0].ToString());
                    notes.Add(nm); 
                }
                db.Close();
            }

            return notes;
        }

        public static NoteModel UpdateData(string noteContent, string noteTitle)
        {
            NoteModel nm;
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "LocalNote.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand updateNote = new SqliteCommand
                ($"UPDATE NotesTable SET NoteContent='" + noteContent + "' WHERE NoteTitle='" + noteTitle + "'", db);
                updateNote.ExecuteNonQuery();
                nm = new NoteModel(noteContent, noteTitle);
                db.Close();
            }
            return nm;
        }

        public static void DeleteData(string noteTitle)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "LocalNote.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand deleteNote = new SqliteCommand
                ($"DELETE FROM NotesTable WHERE NoteTitle='" + noteTitle + "'", db);
                deleteNote.ExecuteNonQuery();
                db.Close();
            }
        }
    }
}
