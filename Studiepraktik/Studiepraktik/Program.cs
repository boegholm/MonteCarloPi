using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Studiepraktik
{
    class Program
    {


        static void Main(string[] args)
        {
            //Opgave1();
            //Opgave2();
            Console.ReadLine();
        }

        static void Opgave1()
        {
            Console.WriteLine("Hvad er dit navn?");
            string navn;
            navn = Console.ReadLine();
            Console.WriteLine("Hej " + navn + ", velkommen til studiepraktik!");
            Console.ReadLine();
        }

        static void Opgave2()
        {
            Console.WriteLine("Indtast tal1: ");
            int tal1 = ReadInt();
            Console.WriteLine("Indtast tal2: ");
            int tal2 = ReadInt();

            int resultat = tal1 + tal2;
            Console.WriteLine("tal1 + tal2 er: "+ resultat);
            resultat = tal1/tal2;
            Console.WriteLine("tal1 / tal2 er: " + resultat);
            Console.ReadLine();
        }

        static int ReadInt()//Læser strenge indtil input er et tal
        {
            int resultat; // her gemmer vi resultatet 

            // så længe vi læser noget, hvor TryParse 
            // returnerer false, skriv en fejlmeddelelse
            while (!int.TryParse(Console.ReadLine(), out resultat))
            // out er en lidt speciel ting i c#
            {
                Console.WriteLine("Fejl: Det indtastede var ikke et tal!");
            }
            return resultat; // returnér resultatet!
        }

    }
}
