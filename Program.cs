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

Console.WriteLine("Please enter the folder path you want to organize");
string? inputPath = Console.ReadLine();
Console.Clear();

while (!Directory.Exists(inputPath))
{
    Console.WriteLine("Error: no existing path");
    Console.WriteLine("Please enter a valid folder path you want to organize");
    inputPath = Console.ReadLine();
    Console.Clear();
}

FileOrganizer organizer = new(inputPath);
organizer.OrganizeFiles();

Console.WriteLine($"Found {organizer.filesCount} files.");
Console.WriteLine("Files organized successfully.");
Console.ReadLine();

class FileOrganizer
{
    // properties
    private string path;
    public int filesCount;
    private string[] files = Array.Empty<string>();

    // constructor
    public FileOrganizer(string _path)
    {
        path = _path;
    }

    // methods
    public void OrganizeFiles()
    {
        files = Directory.GetFiles(path);
        filesCount = files.Length;

        foreach (string file in files)
        {
            string extensions = Path.GetExtension(file).TrimStart('.');
            string targetFolder = Path.Combine(path, extensions);

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

            if (string.IsNullOrWhiteSpace(extensions))
            {
                extensions = "Unknown";
            }


            File.Move(file, destinationPath);
        }
    }
}