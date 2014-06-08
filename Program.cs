using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionFilesExr
{
    class Program
    {
        static void Main(string[] args)
        {
            Person ivan = new Person();
            ivan.Name = "Ivan";
            ivan.Ages = 25;

            Building house = new Building();
            house.BuildingType = "House";
            house.Area = 254;

            string projectPath = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = "result2.txt";
            string filePath = projectPath + fileName;
            Save(house, filePath);
            Building object1 = (Building) Load(filePath);
            Console.WriteLine(object1.Area);

            Console.ReadLine();
        }

        public static void Save(object myObj, string filePath)
        {
            // get obj type
            string objType = myObj.GetType().ToString();
            // get the properties:
            List<string> lines = new List<string>();
            lines.Add(objType);
            foreach (var prop in myObj.GetType().GetProperties())
            {
                if (prop.CanWrite)
                {
                    lines.Add(prop.Name + " : " + prop.GetValue(myObj).ToString());
                }
            }
            
            // write the file:            
            System.IO.File.WriteAllLines(filePath, lines);
        }

        public static object Load(string fileName)
        {
            Object o = null;
            string line;
            using (System.IO.StreamReader file = new System.IO.StreamReader(fileName))
            {
                line = file.ReadLine();
                Type t = Type.GetType(line);
                o = Activator.CreateInstance(t);

                line = file.ReadLine();
                while (line != null)
                {
                    string[] arrProp = line.Split(':');
                    Console.WriteLine(line);
            
                    PropertyInfo prop = o.GetType().GetProperty(arrProp[0].Trim(), BindingFlags.Public | BindingFlags.Instance);
                    if (null != prop && prop.CanWrite)
                    {
                        var val = Convert.ChangeType(arrProp[1], prop.PropertyType);
                        prop.SetValue(o, val, null);
                    }

                    line = file.ReadLine();
                }
            }
            return o;           
        }
    } 

    class Person
    {
        public string Name { get; set; }
        public int Ages { get; set; }

        public Person()
        {
            Name = String.Empty;
        }

    }

    class Building
    {
        private string _BuildingType;
        public string BuildingType
        {            
            get
            {
                return _BuildingType ?? String.Empty;
            }
            set
            {
                this._BuildingType = value;
            }
        }
        public int Area
        {
            get;
            set;
        }

        public string Address
        {
            get
            {
                return String.Format("{0}-{1}", Area, BuildingType);
            }
           
        }
    }
}


