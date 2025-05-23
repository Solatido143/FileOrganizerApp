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

string validPath = FileOrganizer.GetConfirmedPath();
FileOrganizer organizer = new(validPath);
organizer.OrganizeFiles();

if (organizer.totalFiles == 0)
{
    Console.WriteLine("No files were organized.");
}
else
{
    Console.WriteLine($"Organized {organizer.totalFiles} file(s).");
}

Console.ReadLine();


class FileOrganizer
{
    // properties
    private string path;
    public int totalFiles;
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
    public static string GetConfirmedPath()
    {
        string validPath;

        do
        {
            validPath = FileOrganizer.GetValidPath();
            if (!FileOrganizer.ConfirmPath(validPath))
            {
                Console.WriteLine("You cancelled the operation. Press any key to try a different folder...");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                break;
            }
        } while (true);

        return validPath;
    }

    public void OrganizeFiles()
    {
        files = Directory.GetFiles(path);
        totalFiles = files.Length;
        int skipped = 0;
        int moved = 0;

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
                skipped++;
                Console.WriteLine($"Skipping {fileName} — already exists.");
                continue;
            }

            File.Move(file, destinationPath);
            moved++;
            Console.WriteLine($"Moved: {fileName} → {targetFolder}");
        }
        totalFiles = moved;
        if (skipped > 0)
        {
            Console.WriteLine($"{skipped} files(s) were skipped because they already exist in their destination.");
        }
    }

}