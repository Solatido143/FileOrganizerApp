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

string validPath = FileOrganizer.GetValidPath();
if (!FileOrganizer.ConfirmPath(validPath))
{
    Console.WriteLine("Operation cancelled.");
    Console.ReadKey();
    return;
}

FileOrganizer organizer = new(validPath);
organizer.OrganizeFiles();
if (organizer.fileCount < 0)
{
    Console.WriteLine("No files to organize.");
}
else
{
    Console.WriteLine($"Found {organizer.fileCount} file/s.");
    Console.WriteLine($"Files organized successfully.");
}

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

        return path!;
    }
    public void OrganizeFiles()
    {
        files = Directory.GetFiles(path);
        fileCount = files.Length;

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

    public static bool ConfirmPath(string path)
    {
        Console.WriteLine($"{path}\nAre you sure you want to organize this path? (y/n)");
        string? input = Console.ReadLine();
        Console.Clear();

        while (input?.ToLower() != "y" && input?.ToLower() != "n")
        {
            Console.WriteLine("Are you sure you want to organize this path? (y/n)");
            input = Console.ReadLine();
            Console.Clear();
        }

        return input?.ToLower() == "y";
    }

}