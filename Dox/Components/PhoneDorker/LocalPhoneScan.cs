using System;
using System.Collections.Generic;
using System.Text;
using Colorful;
using System.Drawing;
using PhoneNumbers;
using Spectre;
using Console = Colorful.Console;
using System.Linq;
using Spectre.Console;
using Color = System.Drawing.Color;

namespace Dox.Components.PhoneDorker
{
    public class LocalPhoneScan
    {
        private static string displayDash = "[NUMBER_LOG] *-";
        private static string Selected_num;
        private static List<string> Num_list = new List<string>();

        internal struct PhoneStorage
        {
            public static PhoneNumberKind number_kind;
            public static List<string> sig_nums_prefixes = new List<string>().ToList<string>();
        }

        public static void Start(string num)
        {
            PhoneNumber.TryParse($"{num}", out IEnumerable<PhoneNumber> phoneNumbers);
            Console.Write($"[{DateTime.Now.ToString("h:mm:ss tt")}] ", Color.Magenta); Console.Write($" Found {phoneNumbers.Count()} possible calling codes for ", Color.DarkMagenta); Console.Write($"{num}\n\n-------------------\n", Color.Green);
            foreach (var line in phoneNumbers)
            {
                Console.Write(displayDash, Color.Magenta); Console.Write($" {line}\t {line.Country.Name}\n", Color.DarkMagenta);
                displayDash += "-";
            }
            Console.WriteLine("-------------------\n", Color.Green);
            Console.Write($"[+] Would you like to choose a certain number listed above [0 being 1 etc] (0-{phoneNumbers.Count()}) or ALL?: ", Color.Orange);
            string input = Console.ReadLine().ToLower();
            if (int.TryParse(input, out int value))
            {
                /*
                 * Single inputted number
                 */
                Console.WriteLine($"[!] Selected {phoneNumbers.ElementAt(value)} as the number\n", Color.Magenta);
                Scan(phoneNumbers.ElementAt(value).ToString(), phoneNumbers.ElementAt(value));
            }
            else if (input.Equals("all"))
            {
                /*
                 * For all numbers found
                 */
                Console.WriteLine($"[!] Selected everything (0-{phoneNumbers.Count()})", Color.DarkMagenta);
                foreach(var line in phoneNumbers)
                {
                    Console.Write($"[{DateTime.Now.ToString("h:mm:ss tt")}] ", Color.Magenta); Console.Write($" Getting information on {line.ToString()}...\n\n", Color.DarkMagenta);
                    Scan(line.ToString(), line);
                }
            }
            else { Console.WriteLine("[!] Invalid Input", Color.Red); }
        }
        private static void Scan(string num, PhoneNumber phone)
        {
            string[] CI = CompileInfo(num);
            foreach (string line in CI)
            {
                Console.WriteLine(line, Color.DarkMagenta);
            }
            string[] CI2 = CompileInfo2(phone);
            foreach(string line in CI2)
            {
                Console.WriteLine(line, Color.DarkMagenta);
            }
        }
        private static string[] CompileInfo(string num)
        {
            PhoneNumber.TryParse($"{num}", out PhoneNumber phoneNumber);
            bool AllowsLocalGeographicDialling = phoneNumber.Country.AllowsLocalGeographicDialling;
            string country_code = phoneNumber.Country.CallingCode;
            string continent = phoneNumber.Country.Continent;
            bool HasNationalDestCode = phoneNumber.Country.HasNationalDestinationCodes;
            bool HasTrunkPrefix = phoneNumber.Country.HasTrunkPrefix;
            bool IsEuropeanUnionMember = phoneNumber.Country.IsEuropeanUnionMember;
            string IsoCode = phoneNumber.Country.Iso3166Code; 
            string country = phoneNumber.Country.Name; 
            bool SharesCallingCode = phoneNumber.Country.SharesCallingCode;
            PhoneNumberKind PNK = phoneNumber.Kind;
            string NationalDestCode = phoneNumber.NationalDestinationCode;
            string NationalSigNumber = phoneNumber.NationalSignificantNumber;
            string SubscriberNum = phoneNumber.SubscriberNumber;

            // Give vars to internal struct to store and re-use
            PhoneStorage.number_kind = PNK;
            PhoneStorage.sig_nums_prefixes.Add(NationalSigNumber + " | " + country_code);
            //

            var ss = new string[]
            {
                $"[---------------- [{num}] ----------------]\n",
                $" [+] AllowsLocalGeographicDialling: {AllowsLocalGeographicDialling}",
                $" [+] Country Code: {country_code}",
                $" [+] Continent: {continent}",
                $" [+] HasNationalDestintionCode: {HasNationalDestCode}",
                $" [+] HasTrunkPrefix: {HasTrunkPrefix}",
                $" [+] IsEuropeanUnionMember: {IsEuropeanUnionMember}",
                $" [+] ISO Code: {IsoCode}",
                $" [+] Country: {country}",
                $" [+] SharesCallingCode: {SharesCallingCode}",
                $" [+] Phone Number Kind: {PNK.ToString()}",
                $" [+] NationalDestinationCod: {NationalDestCode}",
                $" [+] NationalSignificantNumber: {NationalSigNumber}",
                $" [+] SubscriberNumber: {SubscriberNum}\n",
            };
            return ss;
        }
        private static string[] CompileInfo2(PhoneNumber num)
        {
            List<string> list = new List<string>();

            if (PhoneStorage.number_kind == PhoneNumberKind.GeographicPhoneNumber)
            {
                list.Add("[---------------- [Geographical Location] ----------------]\n");
                var geographicPhoneNumber = (GeographicPhoneNumber)num;
                string geographical_Area = geographicPhoneNumber.GeographicArea;
                list.Add(" [+] Geographical Location: " + geographical_Area + "\n");
            }
            else if (PhoneStorage.number_kind == PhoneNumberKind.MobilePhoneNumber)
            {
                list.Add("[---------------- [Miscellaneous (Mobile)] ----------------]\n");
                var mobilePhoneNumber = (MobilePhoneNumber)num;
                bool IsPager = mobilePhoneNumber.IsPager;
                bool IsVirtualSim = mobilePhoneNumber.IsVirtual;
                list.Add(" [+] IsPager: " + IsPager); list.Add(" [+] IsVirtualSim: " + IsVirtualSim + "\n");
            }
            else if (PhoneStorage.number_kind == PhoneNumberKind.NonGeographicPhoneNumber)
            {
                list.Add("[---------------- [Miscellaneous (NonGeoNumber)] ----------------]\n");
                var nonGeographicPhoneNumber = (NonGeographicPhoneNumber)num;
                bool IsFreephone =  nonGeographicPhoneNumber.IsFreephone;              // true/false
                bool IsMachineToMachine = nonGeographicPhoneNumber.IsMachineToMachine;       // true/false
                bool IsPremiumRate = nonGeographicPhoneNumber.IsPremiumRate;            // true/false
                bool IsSharedCost = nonGeographicPhoneNumber.IsSharedCost;             // true/false
                list.Add(" [+] Freephone: " + IsFreephone); list.Add(" [+] IsMachineToMachine: " + IsMachineToMachine);
                list.Add(" [+] IsPremiumRate: " + IsPremiumRate); list.Add(" [+] IsSharedCost: " + IsSharedCost + "\n");
            }
            string[] CompiledInfo = list.ToArray();
            return CompiledInfo;
        }
    }
}
