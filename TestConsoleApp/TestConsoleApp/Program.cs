using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;


namespace TestConsoleApp
{
    class Program
    {
        public class Employee
        {
            public string name;
            public int id;
            public List<double> Mn = new List<double>();
            public List<double> Td = new List<double>();
            public List<double> Wd = new List<double>();
            public List<double> Th = new List<double>();
            public List<double> Fr = new List<double>();
            public List<double> Sd = new List<double>();
            public List<double> Sn = new List<double>();
            public double mn;
            public double td;
            public double wd;
            public double th;
            public double fr;
            public double sd;
            public double sn;
        }
        public class dayOfWeak
        {
            public Dictionary<int, double> emp = new Dictionary<int, double>();
        }
        static void Main(string[] args)
        {
            string connectionString = @"Data Source=ASUS;Initial Catalog=TestDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            Dictionary<int, Employee> employees = new Dictionary<int, Employee>();
            Dictionary<int, dayOfWeak> days = new Dictionary<int, dayOfWeak>();
            for (int i = 1; i < 8; i++)
            {
                dayOfWeak day = new dayOfWeak();
                days.Add(i, day);
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataAdapter line1 = new SqlDataAdapter("SELECT * FROM employees", connection);
                DataSet dataset1 = new DataSet();
                int rows1 = line1.Fill(dataset1, "employees");

                foreach (DataRow row in dataset1.Tables["employees"].Rows)
                {
                    int id = (int)row["id"];
                    string name = (string)row["name"];
                    Employee emp = new Employee();
                    emp.name = name;
                    employees.Add(id, emp);
                }   

                SqlDataAdapter line2 = new SqlDataAdapter("SELECT id, employee_id, hours, date FROM time_reports", connection);
                DataSet dataset2 = new DataSet();
                int rows2 = line2.Fill(dataset2, "time_reports");

                foreach (DataRow row in dataset2.Tables["time_reports"].Rows)
                {
                    int id = (int)row["id"];
                    int employee_id = (int)row["employee_id"];
                    double hours = (double)row["hours"];
                    DateTime date = (DateTime)row["date"]; 
                    string s = date.ToString("dddd");
                    switch (s)
                    {
                        case "понедельник":
                            employees[employee_id].Mn.Add(hours);
                            employees[employee_id].id = employee_id;
                            break;
                       case "вторник":
                            employees[employee_id].Td.Add(hours);
                            employees[employee_id].id = employee_id;
                            break;
                        case "среда":
                            employees[employee_id].Wd.Add(hours);
                            employees[employee_id].id = employee_id;
                            break;
                        case "четверг":
                            employees[employee_id].Th.Add(hours);
                            employees[employee_id].id = employee_id;
                            break;
                        case "пятница":
                            employees[employee_id].Fr.Add(hours);
                            employees[employee_id].id = employee_id;
                            break;
                        case "суббота":
                            employees[employee_id].Sd.Add(hours);
                            employees[employee_id].id = employee_id;
                            break;
                        case "воскресенье":
                            employees[employee_id].Sn.Add(hours);
                            employees[employee_id].id = employee_id;
                            break;
                        default:
                            break;
                    }
                }
            }
            foreach (Employee e in employees.Values)
            {
                if (e.Mn.Count() != 0) { e.mn = e.Mn.Average(); days[1].emp.Add(e.id, e.mn); } else e.mn = 0;
                if (e.Td.Count() != 0) { e.td = e.Td.Average(); days[2].emp.Add(e.id, e.td); } else e.td = 0;
                if (e.Wd.Count() != 0) { e.wd = e.Wd.Average(); days[3].emp.Add(e.id, e.wd); } else e.wd = 0;
                if (e.Th.Count() != 0) { e.th = e.Th.Average(); days[4].emp.Add(e.id, e.th); } else e.th = 0;
                if (e.Fr.Count() != 0) { e.fr = e.Fr.Average(); days[5].emp.Add(e.id, e.fr); } else e.fr = 0;
                if (e.Sd.Count() != 0) { e.sd = e.Sd.Average(); days[6].emp.Add(e.id, e.sd); } else e.sd = 0;
                if (e.Sn.Count() != 0) { e.sn = e.Sn.Average(); days[7].emp.Add(e.id, e.sn); } else e.sn = 0;
            }
            for (int i = 1; i < 8; i++)
            {
                switch (i)
                {
                    case 1:
                        Console.Write("| Monday    |");
                        break;
                    case 2:
                        Console.Write("| Tuesday   |");
                        break;
                    case 3:
                        Console.Write("| Wednesday |");
                        break;
                    case 4:
                        Console.Write("| Thursday  |");
                        break;
                    case 5:
                        Console.Write("| Friday    |");
                        break;
                    case 6:
                        Console.Write("| Saturday  |");
                        break;
                    case 7:
                        Console.Write("| Sunday    |");
                        break;
                    default:
                        break;
                }
                days[i].emp = days[i].emp.OrderBy(x => x.Value).Reverse().ToDictionary(x => x.Key, x => x.Value);
                foreach (int s in days[i].emp.Keys)
                    Console.Write($" {employees[s].name}, ({Math.Round(days[i].emp[s], 2)} hours), ");
                Console.WriteLine();
            }
        }
    }
}
