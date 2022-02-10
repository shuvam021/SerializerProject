using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using CsvHelper;

namespace SerializerProject
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"Employee: {Id}, {Name}";
        }
    }
    public class EmployeeDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public long Phone { get; set; }
        public string Country { get; set; }
        public DateTime JDate { get; set; }

        public override string ToString()
        {
            return $"Employee: {Id}, {Name}, {Email}, {Phone}, {Country}, {JDate}";
        }
    }
    public static class ApiOperations
    {
        public static void JsonSerialize(string path, List<Employee> list)
        {
            string res = JsonConvert.SerializeObject(list);
            File.WriteAllText(path, res);
        }
        public static void JsonDeSerialize(string path)
        {
            string res = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<Employee>>(res);
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }
        public static void XmlSerialize(string path, List<Employee> list)
        {
            XmlSerializer xml = new XmlSerializer(typeof(Employee));
            FileStream st = new FileStream(path, FileMode.OpenOrCreate);
            foreach (var item in list)
            {
                xml.Serialize(st, item);
                break;
            }
        }
        public static void XmlDeSerialize(string path)
        {
            XmlSerializer xml = new XmlSerializer(typeof(Employee));
            FileStream st = new FileStream(path, FileMode.Open);
            Employee res = (Employee)xml.Deserialize(st);
            Console.WriteLine(res);
        }
        public static void CsvSerialize<T>(string path, IEnumerable<T> list)
        {
            try
            {
                var wr = new StreamWriter(path);
                var csvWr = new CsvWriter(wr, CultureInfo.InvariantCulture);
                csvWr.WriteRecords(list);
                wr.Close();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File Not Found!!!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void CsvDeSerialize<T>(string path)
        {
            try
            {
                var read = new StreamReader(path);
                var csvRead = new CsvReader(read, CultureInfo.InvariantCulture);
                var result = csvRead.GetRecords<T>().ToList();
                if (result.Count == 0)
                {
                    Console.WriteLine("File is Empty");
                    return;
                }
                foreach (var item in result)
                {
                    Console.WriteLine(item);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File Not Found!!!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    class Program
    {
        private const string JsonPath = @"./File/Something.json";
        private const string XmlPath = @"./File/Something.xml";
        private const string CsvPath = @"./File/Something.csv";
        private static readonly List<Employee> EmployeeList = new List<Employee>()
        {
            new Employee(){Id=1, Name = "John"},
            new Employee(){Id=2, Name = "Jane"}
        };
        private static readonly List<EmployeeDetails> EmployeeDetailsList = new List<EmployeeDetails>()
        {
            new EmployeeDetails(){Id=1, Name = "John", Email = "john.some@thing.com", Phone = 9876543210, Country = "Knowhere", JDate = DateTime.Now},
            new EmployeeDetails(){Id=2, Name = "Jane", Email = "jane.some@thing.com", Phone = 1234567890, Country = "Knowhere", JDate = DateTime.Now},
        };

        static void Main(string[] args)
        {
            // Json
            Console.WriteLine("data from Json file");
            ApiOperations.JsonSerialize(JsonPath, EmployeeList);
            ApiOperations.JsonDeSerialize(JsonPath);

            // Xml
            Console.WriteLine("data from Xml file");
            ApiOperations.XmlSerialize(XmlPath, EmployeeList);
            ApiOperations.XmlDeSerialize(XmlPath);

            // Csv
            Console.WriteLine("data from CSV file");
            ApiOperations.CsvSerialize<Employee>(CsvPath, EmployeeList);
            ApiOperations.CsvDeSerialize<Employee>(CsvPath);

            ApiOperations.CsvSerialize<EmployeeDetails>(CsvPath, EmployeeDetailsList);
            ApiOperations.CsvDeSerialize<EmployeeDetails>(CsvPath);
        }
    }
}
