using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;

namespace LibraryManager
{
    class Book
    {
        private static readonly int CheckOutTimeLimitInMonths = 2;
        private static readonly int CheckOutBookLimit = 3;
        private static readonly string directoryIN = Directory.GetCurrentDirectory().Split(new string[] { $"{Path.DirectorySeparatorChar}bin" }, StringSplitOptions.None)[0] + $"{Path.DirectorySeparatorChar}Books.json";
        private static readonly string directoryOUT = Directory.GetCurrentDirectory().Split(new string[] { $"{Path.DirectorySeparatorChar}bin" }, StringSplitOptions.None)[0] + $"{Path.DirectorySeparatorChar}Books.json";
        private static List<Book> JsonBookList = new List<Book>();
        public int BookID { get; set; }
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
                _bookID = JsonBookList.LastOrDefault().BookID;
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
                BookID = _bookID,
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
                    userBookID.Add(book.BookID);
                    Console.WriteLine("Book ID: " + book.BookID);
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
            Console.WriteLine("Please select a filter (1-6): ");
            Console.WriteLine("1. Author");
            Console.WriteLine("2. Category");
            Console.WriteLine("3. Language");
            Console.WriteLine("4. ISBN");
            Console.WriteLine("5. Name");
            Console.WriteLine("6. Status");
            int selection;
            string filterData = "";
            if (Int32.TryParse(Console.ReadLine(), out selection) && selection <= 6 && selection > 0)
            {
                Console.WriteLine("Please enter filter data: ");
                filterData = Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Not a valid selection, please try again...");
            }
            BookFilter(filterData, selection);
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
        public static void BookFilter(string filterData, int selection)
        {
            foreach (var book in JsonBookList)
            {
                if (selection == 1 && FileSystemName.MatchesSimpleExpression($"*{filterData}*", book.Author) ||
                   selection == 2 && FileSystemName.MatchesSimpleExpression($"*{filterData}*", book.Category) ||
                   selection == 3 && FileSystemName.MatchesSimpleExpression($"*{filterData}*", book.Language) ||
                   selection == 4 && FileSystemName.MatchesSimpleExpression($"*{filterData}*", book.ISBN) ||
                   selection == 5 && FileSystemName.MatchesSimpleExpression($"*{filterData}*", book.Name) ||
                   selection == 6 && FileSystemName.MatchesSimpleExpression($"*{filterData}*", book.UserID == null ? "Available" : "Taken"))
                {
                    Console.Write("ID: " + book.BookID + " | ");
                    Console.Write("Name: " + book.Name + " | ");
                    Console.Write("Author: " + book.Author + " | ");
                    Console.Write("Category: " + book.Category + " | ");
                    Console.Write("Language: " + book.Language + " | ");
                    Console.Write("Publication date: " + book.PublicationDate + " | ");
                    Console.Write("ISBN: " + book.ISBN + " | ");
                    Console.WriteLine(book.UserID == null ? "Available" : "Taken");
                }
            }
        }
    }
}