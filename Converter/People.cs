
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Converter
{
    public class People
    {
        public People()
        {

        }
        public People(string Name, int Rost, int Ves)
        {
            name = Name;
            rost = Rost;
            ves = Ves;
        }
        public string name;
        public int rost;
        public int ves;
    }
    public class Edit
    {
        public static string Editor(string data)
        {
            List<string> lines = data.Split("\n").ToList();
            bool TorF = false;
            int pos = 1;
            int maxPos = lines.Count;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Вы изменяете файл. Стрелка вправо (Редактирование). Enter (Замена текста). F1 (Сохранение)");
                foreach (string line in lines)
                {
                    Console.WriteLine("   " + line);
                }
                if (TorF)
                {
                    Console.SetCursorPosition(3, pos);
                    lines[pos - 1] = Console.ReadLine();
                    TorF = false;
                }
                Console.SetCursorPosition(0, pos);
                Console.Write("->");
                ConsoleKey key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if (pos > 1)
                        {
                            pos -= 1;
                        }
                        else
                        {
                            pos = maxPos;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (maxPos > pos)
                        {
                            pos += 1;
                        }
                        else
                        {
                            pos = 1;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        lines[pos - 1] = "";
                        TorF = true;
                        break;
                    case ConsoleKey.F1:
                        string result = "";
                        foreach (string elem in lines)
                        {
                            result += elem + "\n";
                        }
                        return result;
                }
            }
            return data;
        }
    }
    public class Converters
    {
        public static string SerTxt(List<People> data)
        {
            string result = "";
            foreach (People peopls in data)
            {
                result = result + peopls.name + "\n";
                result = result + peopls.rost + "\n";
                result = result + peopls.ves + "\n";
            }
            return result;
        }
        public static string SerJson(List<People> data)
        {
            return JsonConvert.SerializeObject(data);
        }
        public static string SerXml(List<People> data)
        {
            XmlSerializer xml = new XmlSerializer(typeof(List<People>));
            using (FileStream i = new FileStream("4ch.xml", FileMode.OpenOrCreate))
            {
                xml.Serialize(i, data);
            }
            string result = File.ReadAllText("4ch.xml");
            return result;
        }
        public static string Serialize(string pac, string data)
        {
            string result = "";
            List<People> des = DesTxt(data);
            switch (pac.Split(".")[^1])
            {
                case "json":
                    result = SerJson(des);
                    break;
                case "xml":
                    result = SerXml(des);
                    break;
                default:
                    result = SerTxt(des);
                    break;
            }
            return result;
        }
        public static List<People> Deserealise(string pac)
        {
            List<People> peopls = new List<People>();
            string data = File.ReadAllText(pac);
            switch (pac.Split(".")[^1])
            {
                case "json":
                    peopls = DesJson(data);
                    break;
                case "xml":
                    peopls = DesXml(data);
                    break;
                default:
                    peopls = DesTxt(data);
                    break;
            }
            return peopls;
        }
        private static List<People> DesTxt(string data)
        {
            List<People> result = new List<People>();
            List<string> lines = data.Split("\n").ToList();
            lines.RemoveAll(x => x == "");
            for (int i = 0; i < lines.Count; i += 3)
            {
                string name;
                int ves;
                int rost;
                try
                {
                    name = lines[i];
                    ves = Convert.ToInt32(lines[i + 1]);
                    rost = Convert.ToInt32(lines[i + 2]);
                }
                catch
                {
                    break;
                }
                People peopls = new People()
                {
                    name = name,
                    ves = ves,
                    rost = rost
                };
                result.Add(peopls);
            }
            return result;
        }
        private static List<People> DesJson(string data)
        {
            List<People> result;
            result = JsonConvert.DeserializeObject<List<People>>(data);
            return result;
        }
        private static List<People> DesXml(string pac)
        {
            List<People> result;
            XmlSerializer xml = new XmlSerializer(typeof(List<People>));
            using (FileStream i = new FileStream(pac, FileMode.Open))
            {
                result = (List<People>)xml.Deserialize(i);
            }
            return result;
        }
    }

}