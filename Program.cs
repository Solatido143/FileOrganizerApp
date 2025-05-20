// See https://aka.ms/new-console-template for more information

using System.Runtime.CompilerServices;

Console.Title = "File Organizer App";
Console.ForegroundColor = ConsoleColor.White;
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

// Car myObj = new();
// Console.WriteLine(myObj.color);
// Console.ReadKey();

// class Car
// {
//     public string color = "red";
// }

Console.WriteLine("Welcome to File Organizer App!\nPress any key to continue...");
Console.ReadKey();
Console.Clear();

Console.WriteLine("Please enter the folder path you want to organize");
string path = Console.ReadLine();
Console.Clear();

while (!Directory.Exists(path))
{
    Console.WriteLine("Error no existing file path");
    Console.WriteLine("Please enter a valid folder path you want to organize");
    path = Console.ReadLine();
    Console.Clear();
}

Console.WriteLine("You input an existing folder path, thank you!");

string[] files = Directory.GetFiles(path);

string extensions;


foreach (string file in files)
{
    extensions = Path.GetExtension(file);
    Console.WriteLine(extensions);
}

Console.ReadLine();
