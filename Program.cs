using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

class Book
{
    public int Id {get; set;}
    public string Title {get; set;}
    public string Author {get; set;}
    public bool IsBorrowed {get; set;}
    public int? BorrowedByUserId {get; set;}
}

class User
{
    public int Id {get; set;}
    public string Name {get; set;}
}

class LibraryData
{
    public List<Book> Books {get; set;}
    public List<User> Users {get; set;}
}

class LibraryService
{
    List<Book> books = [];
    List<User> users = [];

    int nextBookId = 0;
    int nextUserId = 0;

    public void AddBook()
    {
        Console.Write("Write the book title: ");
        string title = Console.ReadLine();

        Console.Write("Write the book author: ");
        string author = Console.ReadLine();

        books.Add(new Book
        {
            Id = nextBookId,
            Title = title,
            Author = author,
            IsBorrowed = false
        });

        nextBookId++;
    }

    public void BorrowBook()
    {
        int bookId = ReadInt("Write the book id: ");

        if (bookId < 0 || bookId >= books.Count)
        {
            Console.WriteLine("Invalid book id. Try again.");
            return;
        }
        
        int userId = ReadInt("Write the user id: ");

        if (userId < 0 || userId >= users.Count)
        {
            Console.WriteLine("Invalid user id. Try again.");
            return;
        }

        if (books[bookId].IsBorrowed == false) 
        {
            books[bookId].IsBorrowed = true;
            books[bookId].BorrowedByUserId = userId;
            Console.WriteLine("Book successfully borrowed.");
        }
        else
        {
            Console.WriteLine("Book is already borrowed, choose another book.");
            return;
        }
        
    }

    public void ReturnBook()
    {
        int bookId = ReadInt("Write the book id: ");

        if (bookId < 0 || bookId >= books.Count)
        {
            Console.WriteLine("Invalid book id. Try again.");
            return;
        }

        if (!books[bookId].IsBorrowed)
        {
            Console.WriteLine("Book is not borrowed.");
            return;
        }

        books[bookId].IsBorrowed = false;
        books[bookId].BorrowedByUserId = null;

        Console.WriteLine("Book successfully returned.");
    }

    public void ShowBooks()
    {
        Console.WriteLine("Books list: ");
        for (int i = 0; i < books.Count; i++)
        {
            var book = books[i];

            string status = "Available";

            if (book.IsBorrowed)
            {
                var user = users.FirstOrDefault(u => u.Id == book.BorrowedByUserId);
                status = user != null 
                    ? $"Borrowed by {user.Name}" 
                    : "Borrowed (unknown user)";
            }
            Console.WriteLine($"{book.Id}. {book.Title} by {book.Author} - {status}");
        }
        Console.WriteLine();
    }

    public void AddUser()
    {
        Console.Write("Write the user name: ");
        string name = Console.ReadLine();

        users.Add(new User
        {
            Id = nextUserId,
            Name = name
        });

        nextUserId++;
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

    public void SaveToFile()
    {
        var data = new LibraryData
        {
            Books = books,
            Users = users
        };

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
            Converters = { new JsonStringEnumConverter() }
        };

        string jsonString = JsonSerializer.Serialize(data, options);
        File.WriteAllText("data.json", jsonString);

        Console.WriteLine("Data saved.");
    }

    public void LoadFromFile()
    {
        string jsonPath = "data.json";

        if (!File.Exists(jsonPath))
        {
            Console.WriteLine("No data in file");
            return;
        }

        try
        {
            string jsonString = File.ReadAllText(jsonPath);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
                Converters = { new JsonStringEnumConverter() }
            };

            var data = JsonSerializer.Deserialize<LibraryData>(jsonString, options);
            books = data?.Books ?? [];
            users = data?.Users ?? [];
            nextBookId = books.Count > 0 ? books.Max(b => b.Id) + 1 : 0;
            nextUserId = users.Count > 0 ? users.Max(b => b.Id) + 1 : 0;

            Console.WriteLine("Data loaded.");
        }
        catch
        {
            Console.WriteLine();
            books = [];
            users = [];
            nextBookId = books.Count > 0 ? books.Max(b => b.Id) + 1 : 0;
            nextUserId = users.Count > 0 ? users.Max(b => b.Id) + 1 : 0;
        }
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
        Console.Clear();
        Program program = new Program();
        while (true)
        {
            program.ShowMenu();
            program.Choice();
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
            "6. Return book.\n"+
            "7. Save books list to file.\n"+
            "8. Load books list from file.\n"+
            "0. Exit program.\n"
        );
    }

    public void Choice()
    {
        int choise = library.ReadInt("Write your choice: ");

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
            case 7:
                Console.Clear();
                library.SaveToFile();
                break;
            case 8:
                Console.Clear();
                library.LoadFromFile();
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