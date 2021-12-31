using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Enumeration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace LibraryManager
{
    class Book
    {
        private static int CheckOutTimeLimitInMonths = 2;
        private static int CheckOutBookLimit = 3;
        private static string directoryIN = Directory.GetCurrentDirectory() + $"{Path.DirectorySeparatorChar}Books.json";
        private static string directoryOUT = Directory.GetCurrentDirectory() + $"{Path.DirectorySeparatorChar}Books.json";
        private static List<Book> JsonBookList = new List<Book>();
        public int bookID { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public string Language { get; set; }
        public string PublicationDate { get; set; }
        public string ISBN { get; set; }
        public int? UserID { get; set; }
        public string DueDate { get; set; }
        public Book()
        {
        }
        public static void AddBook()
        {
            var _bookID = 0;
            if (File.Exists(directoryIN))
            {
                _bookID = JsonBookList.LastOrDefault().bookID;
                _bookID++;
            }
            Console.WriteLine("Please enter the name: ");
            var name = Console.ReadLine();
            Console.WriteLine("Please enter the author: ");
            var author = Console.ReadLine();
            Console.WriteLine("Please enter the category: ");
            var category = Console.ReadLine();
            Console.WriteLine("Please enter the language: ");
            var language = Console.ReadLine();
            Console.WriteLine("Please enter the publication date: ");
            var publicationDate = Console.ReadLine();
            Console.WriteLine("Please enter the ISBN: ");
            var _ISBN = Console.ReadLine();
            JsonBookList.Add(new Book
            {
                bookID = _bookID,
                Name = name,
                Author = author,
                Category = category,
                Language = language,
                PublicationDate = publicationDate,
                ISBN = _ISBN
            });
            Console.WriteLine("Book " + name + " was succesfully added. Press any key to continue...");
            Console.ReadKey();
        }
        public static void CheckOutBook()
        {
            int _userID, _bookID;
            Console.WriteLine("Please enter users ID: ");
            while (!Int32.TryParse(Console.ReadLine(), out _userID))
            {
                Console.WriteLine("Invalid user ID, please try again: ");
            }

            var userBookCount = JsonBookList.FindAll(item => item.UserID == _userID).Count();

            if (!(userBookCount >= CheckOutBookLimit))
            {
                do
                {
                    Console.WriteLine("Please enter the book ID you wish to check out: ");
                    while (!Int32.TryParse(Console.ReadLine(), out _bookID))
                    {
                        Console.WriteLine("Invalid book ID, please try again: ");
                    }
                    if (JsonBookList.Contains(JsonBookList[_bookID]))
                    {
                        JsonBookList[_bookID].UserID = _userID;
                    }
                    else
                    {
                        Console.WriteLine("No such book exists: ");
                    }
                } while (!JsonBookList.Contains(JsonBookList[_bookID]));

                int TimeInMonths = 0;
                Console.WriteLine("Please enter the check out length in months: ");
                while (TimeInMonths > CheckOutTimeLimitInMonths || TimeInMonths <= 0)
                {
                    if (Int32.TryParse(Console.ReadLine(), out TimeInMonths))
                    {
                        if (TimeInMonths > CheckOutTimeLimitInMonths || TimeInMonths <= 0)
                        {
                            Console.WriteLine($"A book can only be checked out for up to {CheckOutTimeLimitInMonths} months, please try again...");
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input format, please try again...");
                    }
                }
                JsonBookList[_bookID].DueDate = DateTime.Now.AddMonths(TimeInMonths).ToString("yyyy-MM-dd");
                Console.WriteLine("Book succesfully checked out. Press any key to continue...");
            }
            else
            {
                Console.WriteLine("User has reached the limit of books he can simultaneously have. Press any key to continue...");
            }
            Console.ReadKey();
        }
        public static void ReturnBook()
        {
            int _userID;
            Console.WriteLine("Please enter users ID: ");
            while (!Int32.TryParse(Console.ReadLine(), out _userID))
            {
                Console.WriteLine("Invalid user ID, please try again: ");
            }
            Console.WriteLine("Users books: ");
            List<int> userBookID = new List<int>();
            foreach (var book in JsonBookList)
            {
                if (book.UserID == _userID)
                {
                    userBookID.Add(book.bookID);
                    Console.WriteLine("Book ID: " + book.bookID);
                    Console.WriteLine("Name: " + book.Name);
                    Console.WriteLine("Due date: " + book.DueDate);
                }
            }
            if (userBookID.Any())
            {
                int _bookID;
                Console.WriteLine("Enter the book ID of which you wish to return: ");
                while (!(Int32.TryParse(Console.ReadLine(), out _bookID) && userBookID.Contains(_bookID)))
                {
                    Console.WriteLine("Invalid book ID, please try again");
                    if (!userBookID.Contains(_bookID))
                    {
                        Console.WriteLine("Book ID match was not found, please try again: ");
                    }
                }
                if (DateTime.Parse(JsonBookList[_bookID].DueDate) < DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")))
                {
                    Console.WriteLine("Book is overdue. Time flies when having fun with a book in hand!");
                }
                JsonBookList[_bookID].UserID = null;
                JsonBookList[_bookID].DueDate = null;
                Console.WriteLine("Book succesfully returned. Press any key to continue...");
            }
            else
            {
                Console.WriteLine("No books found. Press any key to continue...");
            }
            Console.ReadKey();
        }
        public static void ListAllBooks()
        {

            foreach (var book in JsonBookList)
            {
                    Console.Write("ID: " + book.bookID + " | ");
                    Console.Write("Name: " + book.Name + " | ");
                    Console.Write("Author: " + book.Author + " | ");
                    Console.Write("Category: " + book.Category + " | ");
                    Console.Write("Language: " + book.Language + " | ");
                    Console.Write("Publication date: " + book.PublicationDate + " | ");
                    Console.Write("ISBN: " + book.ISBN + " | ");
                    Console.WriteLine(book.UserID == null ? "Available" : "Taken");
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        public static void DeleteBook()
        {
            int _bookID;
            Console.WriteLine("Enter the book ID of which you wish to delete: ");
            while (!(Int32.TryParse(Console.ReadLine(), out _bookID) && JsonBookList.Contains(JsonBookList[_bookID])))
            {
                Console.WriteLine("Invalid book ID, please try again");
                if (!JsonBookList.Contains(JsonBookList[_bookID]))
                {
                    Console.WriteLine("Book ID match was not found, please try again: ");
                }
            }
            JsonBookList.RemoveAt(_bookID);
            Console.WriteLine("Book succesfully removed. Press any key to continue...");
            Console.ReadKey();
        }
        public static void JsonDeserialization()
        {
            if (File.Exists(directoryIN))
            {
                string BookListRead = File.ReadAllText(directoryIN);
                JsonBookList = JsonConvert.DeserializeObject<List<Book>>(BookListRead);
            }
        }
        public static void JsonSerialization()
        {
            string JsonBookListString = JsonConvert.SerializeObject(JsonBookList);
            File.WriteAllText(directoryOUT, JsonBookListString);
        }
    }
}