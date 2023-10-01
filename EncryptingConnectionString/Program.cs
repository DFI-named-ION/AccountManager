using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptingConnectionString
{
    public class Program
    {
        static void Main()
        {
            try
            {
                Console.Write("Enter file name: ");
                string fileName = Console.ReadLine();
                Configuration objectConfig = ConfigurationManager.OpenExeConfiguration(fileName);
                ConnectionStringsSection objectSection = (ConnectionStringsSection)objectConfig.GetSection("connectionStrings");
                if (objectSection.SectionInformation.IsProtected)
                {
                    objectSection.SectionInformation.UnprotectSection();
                }
                else
                {
                    objectSection.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                }
                objectConfig.Save();
                Console.WriteLine($"File: {objectSection.SectionInformation.IsProtected}");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }
    }
}
