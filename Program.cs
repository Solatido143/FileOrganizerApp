// See https://aka.ms/new-console-template for more information

// System.IO.Directory.GetFiles() – get all files
// Path.GetExtension() – determine file type
// Directory.CreateDirectory() – create folders
// File.Move() – move files to new folders

using System.Runtime.CompilerServices;

Console.Title = "File Organizer App";
Console.ForegroundColor = ConsoleColor.White;
// Console.WindowHeight = 10;

Console.WriteLine("Welcome to File Organizer App!\nPress any key to continue...");
Console.ReadKey();
Console.Clear();

// Console.WriteLine("How do you want your files to be organize?");
// Console.WriteLine("1 - by Date\n2 - by Type\n3 - by Size");

string validPath = FileOrganizer.GetValidPath();
FileOrganizer organizer = new(validPath);
organizer.OrganizeFiles();

Console.WriteLine($"Found {organizer.fileCount} files.\nFiles organized successfully.");
Console.ReadLine();

class FileOrganizer
{
    // properties
    private string path;
    public int fileCount;
    private string[] files = Array.Empty<string>();

    // constructor
    public FileOrganizer(string _path)
    {
        path = _path;
    }

    // methods
    public static string GetValidPath()
    {
        Console.WriteLine("Please enter the folder path you want to organize");
        string? path = Console.ReadLine();
        Console.Clear();

        while (!Directory.Exists(path))
        {
            Console.WriteLine("Error: no existing path\nPlease enter a valid folder path you want to organize");
            path = Console.ReadLine();
            Console.Clear();
        }

        Console.WriteLine($"{path}\nAre you sure you want to organize this path? (y/n)");
        string confirmation = Console.ReadLine();
        Console.Clear();

        while (confirmation != "y" && confirmation != "Y" && confirmation != "n" && confirmation != "N")
        {
            Console.WriteLine("Are you sure you want to organize this path? (y/n)");
            confirmation = Console.ReadLine();
            Console.Clear();
        }

        if (confirmation == "y" || confirmation == "Y")
        {
            return path!;
        }
        else 
        {
            return GetValidPath();
        }
    }
    public void OrganizeFiles()
    {
        files = Directory.GetFiles(path);
        fileCount = files.Length;

        if (fileCount < 1)
        {
            Console.WriteLine("No files to organize.");
            Console.ReadKey();
            Console.Clear();
            GetValidPath();
        }

        foreach (string file in files)
        {
            string extension = Path.GetExtension(file).TrimStart('.');

            if (string.IsNullOrWhiteSpace(extension))
            {
                extension = "Unknown";
            }

            string targetFolder = Path.Combine(path, extension);

            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            string fileName = Path.GetFileName(file);
            string destinationPath = Path.Combine(targetFolder, fileName);

            if (File.Exists(destinationPath))
            {
                Console.WriteLine($"Skipping {fileName} — already exists.");
                continue;
            }

            File.Move(file, destinationPath);

            Console.WriteLine($"Moved: {fileName} → {targetFolder}");
        }
    }
}