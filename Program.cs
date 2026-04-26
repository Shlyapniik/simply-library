class Book
{
    public int Id {get; set;}
    public string Title {get; set;}
    public string Author {get; set;}
    public bool IsBorrowed {get; set;}
}

class User
{
    public int Id {get; set;}
    public string Name {get; set;}
}

class LibraryService
{
    List<Book> books = new List<Book>();
    List<User> users = new List<User>();

    public void AddBook()
    {
        Console.Write("Write the book title: ");
        string title = Console.ReadLine();

        Console.Write("Write the book author: ");
        string author = Console.ReadLine();

        books.Add(new Book
        {
            Id = books.Count,
            Title = title,
            Author = author,
            IsBorrowed = false
        });
    }

    public void BorrowBook()
    {
        int id = ReadInt("Write the book id: ");

        if (id > books.Count)
        {
            Console.WriteLine("Invalid index. Try again.");
            return;
        }

        if (books[id].IsBorrowed == false) books[id].IsBorrowed = true;
        else
        {
            Console.WriteLine("The book is already borrowed< choose another book.");
            return;
        }
        
    }

    public void ReturnBook()
    {
        
    }

    public void ShowBooks()
    {
        Console.WriteLine("Books list: ");
        for (int i = 0; i < books.Count; i++)
        {
            Console.WriteLine($"{books[i].Id}. {books[i].Title} by {books[i].Author}. Is borrowed: {books[i].IsBorrowed}");
        }
        Console.WriteLine();
    }

    public void AddUser()
    {
        Console.Write("Write the user name: ");
        string name = Console.ReadLine();

        users.Add(new User
        {
            Id = users.Count,
            Name = name
        });
    }

    public void ShowUsers()
    {
        Console.WriteLine("Users list: ");
        for (int i = 0; i < users.Count; i++)
        {
            Console.WriteLine($"{users[i].Id}. {users[i].Name}");
        }
        Console.WriteLine();
    }

    public int ReadInt(string message)
    {
        int number;
        Console.Write(message);
        
        while (!int.TryParse(Console.ReadLine(), out number))
        {
            Console.WriteLine("Invalid input. Try again: ");
        }

        return number;
    }
}

class Program
{
    LibraryService library = new LibraryService();
    public static void Main()
    {
        Program program = new Program();
        while (true)
        {
            program.ShowMenu();
            program.Choise();
        }    
    }

    public void ShowMenu()
    {
        Console.WriteLine(
            "This is console library manager. What I can:\n"+
            "1. Add book.\n"+
            "2. Show all books.\n"+
            "3. Add user.\n"+
            "4. Show all users.\n"+
            "5. Borrow book.\n"+
            "6. Return bool.\n"+
            "0. Exit program.\n"
        );
    }

    public void Choise()
    {
        int choise = library.ReadInt("Write your choise: ");

        switch (choise)
        {
            case 1:
                Console.Clear();
                library.AddBook();
                break;
            case 2:
                Console.Clear();
                library.ShowBooks();
                break;
            case 3:
                Console.Clear();
                library.AddUser();
                break;
            case 4:
                Console.Clear();
                library.ShowUsers();
                break;
            case 5:
                Console.Clear();
                library.BorrowBook();
                break;
            case 6:
                Console.Clear();
                library.ReturnBook();
                break;
            case 0:
                Console.Clear();
                Environment.Exit(0);
                break;
            default:
                break;
        }
    }

    
}