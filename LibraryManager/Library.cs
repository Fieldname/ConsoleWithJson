using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LibraryManager
{
    class Library
    {
        public static void UI()
        {
            Console.Title = "Book Library";
            Console.WriteLine("Welcome to our book library!\n");
            int selection = 0;
            Book.JsonDeserialization();
            while (selection != 6)
            {
                Console.WriteLine("Please select your operation (1-6): ");
                Console.WriteLine("1. Add a book");
                Console.WriteLine("2. Check out a book");
                Console.WriteLine("3. Return a book");
                Console.WriteLine("4. List all books");
                Console.WriteLine("5. Delete a book");
                Console.WriteLine("6. Exit");
                if (Int32.TryParse(Console.ReadLine(), out selection) && selection <= 6 && selection > 0)
                {
                    if (selection < 6)
                    {
                        ChoiceSelector(selection);
                        Book.JsonSerialization();
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("Not a valid selection, please try again...");
                }
            }
        }
        public static void ChoiceSelector(int selection)
        {
            switch (selection)
            {
                case 1:
                    Book.AddBook();
                    break;
                case 2:
                    Book.CheckOutBook();
                    break;
                case 3:
                    Book.ReturnBook();
                    break;
                case 4:
                    Book.ListAllBooks();
                    break;
                case 5:
                    Book.DeleteBook();
                    break;
                default:
                    break;
            }
        }
    }
}
