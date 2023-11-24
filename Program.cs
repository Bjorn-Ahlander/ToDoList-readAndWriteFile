using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;

Found:

Console.WriteLine("ToDo-Lista");
Console.WriteLine();
Meny();
Val();

static void Meny()   // Visas när programmet startas
{
    Console.WriteLine("MENY");
    Console.WriteLine("1. Visa ToDo-lista");
    Console.WriteLine("2. Lägg till i listan"); 
    Console.WriteLine("3. Ta bort något på listan");
    Console.WriteLine("4. Ändra något som redan finns på listan");
    Console.WriteLine("5. Spara och Stäng");  // skickar till fil
}

static void Val()  //Här fångas användarens val med switch-case
{
    Console.WriteLine();
    Console.Write("Skriv en siffra 1-4 beroende på vad du väljer:");

    string valInput = Console.ReadLine();
    int val = Convert.ToInt32(valInput);

    switch (val)
    {
        case 1:
            Console.WriteLine("Visa ToDo-Lista");
            ReadFromFile();
            break;
        case 2:
            Console.WriteLine();
            Console.WriteLine("Lägg till i listan");
            WriteToFile();
            break;
        case 3:
            Console.WriteLine("Ta bort från listan");
            Change();
            break;
        case 4:
            Console.WriteLine("Ändra befintlig ToDo-uppgift"); 
            Edit();
            break;
        case 5:
            Console.WriteLine("Spara och Stäng listan"); //spara hela listan!
            Close();
            break;
    }
}   //Val() slutar här

static void ReadFromFile() //Detta är "Visa-lista" - val 1
{
    //Exceptions need to be handled. Otherwise if the file cannot be accesed our application will crash.
    try
    {
        //Path of the file to be read.
        string path = @"D:\\file.txt";

        //Initialize StreamReader by passing it the path of the file.
        StreamReader sr = new StreamReader(path);

        //Read from the file and save the string to a variable. 
        string message = sr.ReadToEnd(); //You could use the ReadLine() method inside a loop if you want to read the file line by line.

        //Don't forget to close the StreamReader. 
        sr.Close();

        //Write out the file contents to the console.
        Console.WriteLine(message + " was read from " + path);
    }
    catch (FileNotFoundException)
    {
        Console.WriteLine("No file with such name was found!");
    }
    catch (DirectoryNotFoundException)
    {
        Console.WriteLine("Directory doesn't exist!");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Something went wrong!" + ex.Data);
    }
} //Här slutar ReadFromFile - val 1

static void WriteToFile()  //Lägg till ny - val 2
{
    List<Device> devices = new List<Device>();
    
    while (true)
    {
        // Get Type of Uppgift
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Skriv in ny aktivitet (uppgift att utför) - tryck p för att se menyn: ");
        //Console.WriteLine("Enter p to show list");

        string devInput = Console.ReadLine();
        if (devInput.ToLower().Trim() == "p")
        {
            break;
        }

        // Get Ready Date
        Console.Write("Input Date of Doing yyyy-mm-dd: ");
        string dtInput = Console.ReadLine();
        if (dtInput.ToLower().Trim() == "p")
        {
            break;
        }

        bool isTrue = DateTime.TryParse(dtInput, out DateTime dt);
        if (isTrue)
        {
            Console.WriteLine("Datum korrekt");
        }

        // Get Kategori
        Console.Write("Kategori: ");
        string kategoriInput = Console.ReadLine();
        if (kategoriInput.ToLower().Trim() == "p")
        {
            break;
        }

        // Get Status
        Console.Write("Status: ");
        string status = Console.ReadLine();
        if (status.ToLower().Trim() == "p")
        {
            break;
        }

        string kategori = kategoriInput;
        Device device = new Device(devInput, dt, kategori, status);
        devices.Add(device);
        Console.WriteLine();

        List<Device> sorterad = devices.OrderBy(device => device.DT).ThenBy(device => device.DevInput).ToList();
        Console.WriteLine("Uppgift".PadRight(13) + "Färdigdatum".PadRight(10)
             + "   Kategori".PadRight(16) + "   Status");

        foreach (var item in sorterad)
        {
            string Nexx = (item.DevInput.PadRight(13) + item.DT.ToString("yyyy-MM-dd") +
            "    " + item.Kategori.PadRight(16) + item.Status);
            Console.WriteLine(item.DevInput.PadRight(13) + item.DT.ToString("yyyy-MM-dd")+
            "    " + item.Kategori.PadRight(16) + item.Status);

            string path = @"D:\\file.txt";
            
            File.AppendAllText(path, Nexx + Environment.NewLine);
            string[] lines = File.ReadAllLines(path);
            File.WriteAllLines(path, lines.Distinct().ToArray()); //Nu har vi tagit
                                                                  //bort dubletter
            Console.WriteLine("------------");
        }

        ////Exceptions need to be handled. Otherwise if the file cannot be accesed our application will crash.
        //try
        //{
        //    //Path of the file to be read.
        //    string path = "D:\\file.txt";

        //    //Check if the file exist ...
        //    if (!File.Exists(path))
        //    {
        //        //... if not create it.
        //        FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
        //        fs.Dispose();
        //    }

        //    //Initialize StreamWriter by passing it the path of the file.
        //    StreamWriter sw = File.CreateText(path);

        //    //Text to be written.
        //    string text = (device.DevInput + device.DT + device.Kategori + device.Status);

        //    //Write to the file.
        //    sw.Write(text); //You could use the WriteLine(); method if you want to write to the file line by line.

        //    Console.WriteLine(text + " was written to " + path);

        //    //Don't forget to close the StreamReader. 
        //    sw.Close();
        //}
        //catch (FileNotFoundException)
        //{
        //    Console.WriteLine("No file with such name was found!");
        //}
        //catch (DirectoryNotFoundException)
        //{
        //    Console.WriteLine("Directory doesn't exist!");
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine("Something went wrong!" + ex.Data);
        //}
    } //while slutar här
} //WriteToFile Slutar här - val 2

static void Change() // Ta bort aktivitet - val 3
{
    ReadFromFile();
    Console.WriteLine("Skriv namnet på uppgiften du vill ändra på");
    string path = @"D:\\file.txt";
    string andring = Console.ReadLine();

    List<string> lines = new List<string>(File.ReadAllLines(path));
    for (int i = 0; i < lines.Count; i++)
    {
        if (lines[i].Contains(andring))
        {
            lines.RemoveAt(i);
            i--;
        }
    }
    File.WriteAllLines(path, lines);
    ReadFromFile();
    Console.WriteLine("din ändring är utförd");     
}

static void Edit() // Val 4, ändra befintlig
{
    ReadFromFile();

    Console.WriteLine("Skriv namnet på uppgiften du vill ändra på");
    //string andring = Console.ReadLine();
    string lineToEdit = Console.ReadLine();
    string path = @"D:\\file.txt"; 
    List<string> lines = new List<string>(File.ReadAllLines(path));
    bool modified= false;

    for (int i = 0; i < lines.Count; i++)
    {
        if (lines[i].Contains(lineToEdit))
        {
            Console.WriteLine(lines.Count);
            Console.Write("Edit Uppgift (lägg till din Akktivitet): ");
            string newDevInput = Console.ReadLine();

            Console.Write("Enter new Date: ");
            string newDate = Console.ReadLine();

            Console.Write("Edit - (lägg till din kategori): ");
            string newCat = Console.ReadLine();

            Console.Write("Enter new Status: ");
            string newStatus = Console.ReadLine();

                modified = true;
                Console.WriteLine("Din ändring är gjord");
            //lines.[i] = string.Join(",", parts);
            lines[i] = newDevInput.PadRight(13) + newDate.PadRight(14) + 
                newCat.PadRight(16) + newStatus.PadRight(13);
            //}           
        }           
    }

        File.WriteAllLines(path, lines);
        Console.WriteLine("");
}

goto Found;

static void Close()
    {
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Dina ändringar är sparade och programmet stängs nu!");
    Environment.Exit(0);
    }

    Console.WriteLine();
    Console.WriteLine("---------------");

//Console.WriteLine();
//Console.ForegroundColor = ConsoleColor.Green;
//Console.WriteLine("Vill registrera mer?: j/n");
//string xx = Console.ReadLine();

//if (xx == "j")
//{
//    goto Found;
//}

//else
//{
//    Console.ForegroundColor = ConsoleColor.Green;
//    Console.WriteLine("hejdå! - programmet avslutas");
//}

class Device
{
    public Device(string devInput, DateTime dt, string kategori, string status)
    {
        DevInput = devInput;
        DT = dt;
        Kategori = kategori;
        Status = status;
    }

    public string DevInput { get; set; }
    public DateTime DT { get; set; }
    public string Kategori { get; set; }
    public string Status { get; set; }
}