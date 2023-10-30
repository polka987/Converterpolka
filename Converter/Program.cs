
using System;

namespace Converter
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Напишите путь к файлу, который нужно открыть");
            Console.WriteLine("--------------------------------------------");
            string opened = Console.ReadLine();
            List<People> peopls = Converters.Deserealise(opened);
            string edited = Edit.Editor(Converters.SerTxt(peopls));
            Console.Clear();
            Console.WriteLine("Напишите путь куда сохранить");
            Console.WriteLine("--------------------------------------------");
            string saved = Console.ReadLine();
            string ser = Converters.Serialize(saved, edited);
            File.WriteAllText(saved, ser);
        }
    }
}