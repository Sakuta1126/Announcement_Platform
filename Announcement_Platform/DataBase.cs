using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLitePCL;
using System.IO;
using System.Windows;
using System.Windows.Controls;


namespace Announcement_Platform
{
    public class DataBase
    {

        public static async void Connection()
        {
            string dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Announcement.db");
            SqliteConnection conn = new SqliteConnection($"Filename={dbpath}");
            conn.Open();
            var Task = new SqliteCommand("CREATE TABLE IF NOT EXISTS User(Id INTEGER PRIMARY KEY AUTOINCREMENT, Name VARCHAR(25), Surname VARCHAR(50),BirthDate DATE,Email VARCHAR(50),PhoneNumber INT(9),PFP VARCHAR(60),ResidenceAdd VARCHAR(60)", conn);
            Task.ExecuteReader();
            Task = new SqliteCommand("CREATE TABLE IF NOT EXISTS Occupation(Id INTEGER PRIMARY KEY AUTOINCREMENT, Working BOOL, Work Description VARCHAR(200)", conn);
            Task.ExecuteReader();
            Task = new SqliteCommand("CREATE TABLE IF NOT EXISTS WorkingExperience(Id INTEGER PRIMARY KEY AUTOINCREMENT,Position VARCHAR(30),CompanyName VARCHAR(60),Localization VARCHAR(60),WorkingPeriodStart DATE,WorkingPeriodEnd DATE,Duties VARCHAR(200) ", conn);
            Task.ExecuteReader();
            Task = new SqliteCommand("CREATE TABLE IF NOT EXISTS WorkingSummary(Id INTEGER PRIMARY KEY AUTOINCREMENT,Description VARCHAR(300)", conn);
            Task.ExecuteReader();
            Task = new SqliteCommand("CREATE TABLE IF NOT EXISTS Education(Id INTEGER PRIMARY KEY AUTOINCREMENT,SchoolName VARCHAR(60),VARCHAR Localization(60),EducationLevel Varchar(50),Course VARHCAR(60),Education_Start DATE,Education_End DATE,IsStillEducating BOOL", conn);
            Task.ExecuteReader();
            Task = new SqliteCommand("CREATE TABLE IF NOT EXISTS Langueage(Id INTEGER PRIMARY KEY AUTOINCREMENT,Lang VARCHAR(30),LevelofLang VARCHAR(2)", conn);
            Task.ExecuteReader();
            Task = new SqliteCommand("CREATE TABLE IF NOT EXISTS Skills(Id INTEGER PRIMARY KEY AUTOINCREMENT,Ability VARCHAR(100)", conn);
            Task.ExecuteReader();
            Task = new SqliteCommand("CREATE TABLE IF NOT EXISTS Links(Id INTEGER PRIMARY KEY AUTOINCREMENT,Link VARCHAR(100)", conn);
            Task.ExecuteReader();

        }
    }
}
