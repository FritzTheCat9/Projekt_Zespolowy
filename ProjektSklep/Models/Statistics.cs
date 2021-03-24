using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;

namespace ProjektSklep.Models
{
    public class Statistics
    {
        // Singleton przechowujacy dane statystyczne o sklepie.
        private static Statistics instance;
        public int visitors { get; set; }

        private Statistics()
        {
            string path = "Content/Resources/visit_counter.txt";
            FileStream fs = File.OpenRead(path);
            byte[] b = new byte[64];
            UTF8Encoding temp = new UTF8Encoding(true);
            int result = 0;
            while (fs.Read(b, 0, b.Length) > 0)
            {
                Int32.TryParse(temp.GetString(b), out result);
            }
            fs.Close();
            visitors = result;
        }

        public static Statistics GetInstance()
        {
            if (instance == null)
            {
                instance = new Statistics();
                return instance;
            }
            else
            {
                return instance;
            }
        }

        public void SetVisitors(int visitors)
        {
            this.visitors = visitors;
        }
    }
}
