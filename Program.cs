// See https://aka.ms/new-console-template for more information

Console.Title = "File Organizer App";
Console.ForegroundColor = ConsoleColor.Green;
// Console.WindowHeight = 10;

// System.IO.Directory.GetFiles() – get all files
// Path.GetExtension() – determine file type
// Directory.CreateDirectory() – create folders
// File.Move() – move files to new folders

// Console.WriteLine("This is my first console C# project!\nPress any key to continue...");
// Console.ReadKey();
// Console.WriteLine("What is your name?");
// string userName = Console.ReadLine();
// Console.WriteLine("Hello, " + userName + "\nHow are you?");
// Console.ReadLine();
// Console.WriteLine("I see");

Car myObj = new();
Console.WriteLine(myObj.color);
Console.ReadKey();

class Car
{
    public string color = "red";
}
