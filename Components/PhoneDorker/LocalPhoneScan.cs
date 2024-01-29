using System;
using System.Collections.Generic;
using PhoneNumbers;
using Console = Colorful.Console;
using System.Linq;
using Color = System.Drawing.Color;

namespace Dox.Components.PhoneDorker
{
    public class LocalPhoneScan
    {
        private static string _displayDash = "[NUMBER_LOG] *-";

        internal struct PhoneStorage
        {
            internal static PhoneNumberKind NumberKind;
            internal static List<string> sig_nums_prefixes = new List<string>().ToList();
        }

        public static void Start(string num)
        {
            PhoneNumber.TryParse($"{num}", out IEnumerable<PhoneNumber> phoneNumbers);
            Console.Write($"[{DateTime.Now:h:mm:ss tt}] ", Color.Magenta);
            var enumerable = phoneNumbers.ToList();
            Console.Write($" Found {enumerable.Count()} possible calling codes for ", Color.DarkMagenta); Console.Write($"{num}\n\n-------------------\n", Color.Green);
            foreach (var line in enumerable)
            {
                Console.Write(_displayDash, Color.Magenta); Console.Write($" {line}\t {line.Country.Name}\n", Color.DarkMagenta);
                _displayDash += "-";
            }
            Console.WriteLine("-------------------\n", Color.Green);
            Console.Write($"[+] Would you like to choose a certain number listed above [0 being 1 etc] (0-{enumerable.Count()}) or ALL?: ", Color.Orange);
            string input = Console.ReadLine().ToLower();
            if (int.TryParse(input, out int value))
            {
                /*
                 * Single inputted number
                 */
                Console.WriteLine($"[!] Selected {enumerable.ElementAt(value)} as the number\n", Color.Magenta);
                Scan(enumerable.ElementAt(value).ToString(), enumerable.ElementAt(value));
            }
            else if (input.Equals("all"))
            {
                /*
                 * For all numbers found
                 */
                Console.WriteLine($"[!] Selected everything (0-{enumerable.Count()})", Color.DarkMagenta);
                foreach(var line in enumerable)
                {
                    Console.Write($"[{DateTime.Now:h:mm:ss tt}] ", Color.Magenta); Console.Write($" Getting information on {line}...\n\n", Color.DarkMagenta);
                    Scan(line.ToString(), line);
                }
            }
            else { Console.WriteLine("[!] Invalid Input", Color.Red); }
        }
        private static void Scan(string num, PhoneNumber phone)
        {
            string[] ci = CompileInfo(num);
            foreach (string line in ci)
            {
                Console.WriteLine(line, Color.DarkMagenta);
            }
            string[] ci2 = CompileInfo2(phone);
            foreach(string line in ci2)
            {
                Console.WriteLine(line, Color.DarkMagenta);
            }
        }
        private static string[] CompileInfo(string num)
        {
            PhoneNumber.TryParse($"{num}", out PhoneNumber? phoneNumber);
            bool allowsLocalGeographicDialling = phoneNumber!.Country.AllowsLocalGeographicDialling;
            string countryCode = phoneNumber.Country.CallingCode;
            string continent = phoneNumber.Country.Continent;
            bool hasNationalDestCode = phoneNumber.Country.HasNationalDestinationCodes;
            bool hasTrunkPrefix = phoneNumber.Country.HasTrunkPrefix;
            bool isEuropeanUnionMember = phoneNumber.Country.IsEuropeanUnionMember;
            string isoCode = phoneNumber.Country.Iso3166Code; 
            string country = phoneNumber.Country.Name; 
            bool sharesCallingCode = phoneNumber.Country.SharesCallingCode;
            PhoneNumberKind pnk = phoneNumber.Kind;
            string nationalDestCode = phoneNumber.NationalDestinationCode ?? "Unknown";
            string nationalSigNumber = phoneNumber.NationalSignificantNumber;
            string subscriberNum = phoneNumber.SubscriberNumber;

            // Give vars to internal struct to store and re-use
            PhoneStorage.NumberKind = pnk;
            PhoneStorage.sig_nums_prefixes.Add(nationalSigNumber + " | " + countryCode);
            //

            var ss = new[]
            {
                $"[---------------- [{num}] ----------------]\n",
                $" [+] AllowsLocalGeographicDialling: {allowsLocalGeographicDialling}",
                $" [+] Country Code: {countryCode}",
                $" [+] Continent: {continent}",
                $" [+] HasNationalDestintionCode: {hasNationalDestCode}",
                $" [+] HasTrunkPrefix: {hasTrunkPrefix}",
                $" [+] IsEuropeanUnionMember: {isEuropeanUnionMember}",
                $" [+] ISO Code: {isoCode}",
                $" [+] Country: {country}",
                $" [+] SharesCallingCode: {sharesCallingCode}",
                $" [+] Phone Number Kind: {pnk.ToString()}",
                $" [+] NationalDestinationCod: {nationalDestCode}",
                $" [+] NationalSignificantNumber: {nationalSigNumber}",
                $" [+] SubscriberNumber: {subscriberNum}\n",
            };
            return ss;
        }
        private static string[] CompileInfo2(PhoneNumber num)
        {
            List<string> list = new List<string>();

            if (PhoneStorage.NumberKind == PhoneNumberKind.GeographicPhoneNumber)
            {
                list.Add("[---------------- [Geographical Location] ----------------]\n");
                var geographicPhoneNumber = (GeographicPhoneNumber)num;
                string geographicalArea = geographicPhoneNumber.GeographicArea;
                list.Add(" [+] Geographical Location: " + geographicalArea + "\n");
            }
            else if (PhoneStorage.NumberKind == PhoneNumberKind.MobilePhoneNumber)
            {
                list.Add("[---------------- [Miscellaneous (Mobile)] ----------------]\n");
                var mobilePhoneNumber = (MobilePhoneNumber)num;
                bool isPager = mobilePhoneNumber.IsPager;
                bool isVirtualSim = mobilePhoneNumber.IsVirtual;
                list.Add(" [+] IsPager: " + isPager); list.Add(" [+] IsVirtualSim: " + isVirtualSim + "\n");
            }
            else if (PhoneStorage.NumberKind == PhoneNumberKind.NonGeographicPhoneNumber)
            {
                list.Add("[---------------- [Miscellaneous (NonGeoNumber)] ----------------]\n");
                var nonGeographicPhoneNumber = (NonGeographicPhoneNumber)num;
                bool isFreephone =  nonGeographicPhoneNumber.IsFreephone;              // true/false
                bool isMachineToMachine = nonGeographicPhoneNumber.IsMachineToMachine;       // true/false
                bool isPremiumRate = nonGeographicPhoneNumber.IsPremiumRate;            // true/false
                bool isSharedCost = nonGeographicPhoneNumber.IsSharedCost;             // true/false
                list.Add(" [+] Freephone: " + isFreephone); list.Add(" [+] IsMachineToMachine: " + isMachineToMachine);
                list.Add(" [+] IsPremiumRate: " + isPremiumRate); list.Add(" [+] IsSharedCost: " + isSharedCost + "\n");
            }
            string[] compiledInfo = list.ToArray();
            return compiledInfo;
        }
    }
}
