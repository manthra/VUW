using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace VUW
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory();

            DirectoryInfo diRoot = new DirectoryInfo(path);

            FileInfo[] flXMLs = diRoot.GetFiles("*.xml");


            ConvertDate cd = new ConvertDate();

            foreach (FileInfo flXML in flXMLs)
            {

                Console.WriteLine(flXML.FullName.ToString());

                cd.Convert(flXML); 


            }



            //Console.WriteLine("The current directory is {0}", path);
           
        }
    }
}
