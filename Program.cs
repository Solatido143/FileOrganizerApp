// System.IO.Directory.GetFiles() – get all files
// Path.GetExtension() – determine file type
// Directory.CreateDirectory() – create folders
// File.Move() – move files to new folders

Console.Title = "File Organizer App";
Console.ForegroundColor = ConsoleColor.White;
// Console.WindowHeight = 10;

Console.WriteLine("Welcome to File Organizer App!\nPress any key to continue...");
Console.ReadKey();
Console.Clear();

Console.WriteLine("How do you want your files to be organize?");
Console.WriteLine("1 - by Date\nPress any key for default(By File Type)");

ConsoleKeyInfo choice = Console.ReadKey();
Console.Clear();

string validPath = FileOrganizer.GetConfirmedPath();
FileOrganizer organizer = new(validPath);

if (choice.KeyChar == '1')
{
    organizer.OrganizeByDate();
}
else
{
    organizer.OrganizeByType();
}

if (organizer.totalFiles == 0)
{
    Console.WriteLine("No files were organized.");
}
else
{
    Console.WriteLine($"Organized {organizer.totalFiles} file(s).");
}

Console.WriteLine("\nDo you want to undo the operation? (y/n)");
Console.WriteLine("Note: Undo only works for this session.");
string? undoChoice = Console.ReadLine()?.ToLower();
if (undoChoice == "y")
{
    organizer.UndoLastOperation();
}


Console.ReadKey();





class FileOrganizer
{
    // properties
    private string path;
    public int totalFiles;
    private string[] files = Array.Empty<string>();
    private List<(string from, string to)> moveLog = new();


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

    public void OrganizeByType()
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
            moveLog.Add((file, destinationPath));
            moved++;
            Console.WriteLine($"Moved: {fileName} → {targetFolder}");
        }
        totalFiles = moved;
        if (skipped > 0)
        {
            Console.WriteLine($"{skipped} files(s) were skipped because they already exist in their destination.");
        }
    }

    public void OrganizeByDate()
    {
        files = Directory.GetFiles(path);
        totalFiles = files.Length;
        int skipped = 0;
        int moved = 0;

        foreach (string file in files)
        {
            string dateFolder = File.GetLastWriteTime(file).ToString("yyyy-MM");
            string targetFolder = Path.Combine(path, dateFolder);
            if (!Directory.Exists(targetFolder)) Directory.CreateDirectory(targetFolder);

            string fileName = Path.GetFileName(file);
            string destinationPath = Path.Combine(targetFolder, fileName);

            if (File.Exists(destinationPath))
            {
                skipped++;
                Console.WriteLine($"Skipping {fileName} — already exists.");
                continue;
            }
            File.Move(file, destinationPath);
            moveLog.Add((file, destinationPath));
            moved++;
            Console.WriteLine($"Moved: {fileName} → {targetFolder}");
        }

        totalFiles = moved;
        if (skipped > 0)
        {
            Console.WriteLine($"{skipped} file(s) were skipped because they already exist.");
        }
    }

    public void UndoLastOperation()
    {
        Console.WriteLine("Undoing operation...");
        int undone = 0;
        foreach (var move in moveLog)
        {
            try
            {
                string fileName = Path.GetFileName(move.from);
                if (File.Exists(move.to))
                {
                    if (File.Exists(move.from))
                    {
                        Console.WriteLine($"Skipping undo for {fileName} — original file already exists.");
                        continue;
                    }

                    File.Move(move.to, move.from);
                    undone++;
                    Console.WriteLine($"Restored: {fileName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error restoring {move.to} → {move.from}: {ex.Message}");
            }
        }

        Console.WriteLine($"Undo complete! {undone} file(s) restored.");
    }

}