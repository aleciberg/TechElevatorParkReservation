using System;
using System.Collections.Generic;
using System.Text;
using Capstone.DAL;
using Capstone.Models;

namespace Capstone
{
    public class ParkReservationCLI
    {
        public void RunCLI()
        {
            string input = "";
            do
            {
                PrintTitleScreen("View Parks Interface");
                Console.WriteLine("Select Park For Further Details");

                ParkSqlDAL parkSqlDAL = new ParkSqlDAL();
                List<string> parks = parkSqlDAL.GetParkName();

                for (int i = 0; i < parks.Count; i++)
                {
                    Console.WriteLine($"{i + 1}) {parks[i]}");
                }
                Console.WriteLine("Q) Quit");

                bool validInput = false;
                int parkSelection = 0;
                do
                {
                    try
                    {
                        Console.Write("\n Park Selection (enter Q to cancel): ");
                        string userInput = Console.ReadLine();
                        if (userInput.ToUpper()  == "Q")
                        {
                            input = "Q";
                            return;
                        }
                        parkSelection = int.Parse(userInput);
                        {
                            if (parkSelection <= parks.Count && parkSelection > 0) { validInput = true; }
                        }
                        if (validInput == false) { Console.WriteLine("Invalid Park Number\n"); }
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine("Invalid Park Number\n");
                    }
                } while (validInput == false);

                ParkMenu(parks[parkSelection - 1]);

            } while (input != "Q");
           
            Console.ReadLine();
        }

        public void ParkMenu(string name)
        {
            string input = "";
            do
            {
                DisplayParkInformation(name);
                Console.WriteLine("\nSelect a Command");
                Console.WriteLine("1) View Campgrounds");
                //Console.WriteLine("2) Search for Reservation");
                Console.WriteLine("2) View Reservation for the next 30 Days");
                Console.WriteLine("3) Return to Previous Screen");


                bool validInput = false;

                do
                {
                    Console.Write("\nEnter choice (1, 2, 3): ");
                    input = Console.ReadLine();

                    if (input == "1" || input == "2" || input == "3")
                    {
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Not a valid input. Please try again.");
                    }
                } while (validInput == false);
                switch (input)
                {
                    case "1":
                        ViewCampgroundsMenu(name);
                        break;

                    //case "2":
                    //    SearchForUpcomingParkReservations(name);
                    //    break;

                    case "2":
                        SearchForParkReservations(name);
                        break;

                    case "3":
                        Console.Write("\n");
                        break;
                }
            } while (input != "3");
        }

        public void DisplayParkInformation(string name)
        {
            ParkSqlDAL parkSqlDAL = new ParkSqlDAL();
            Park park = parkSqlDAL.GetParkInfo(name);

            Console.WriteLine();
            PrintTitleScreen("Park Information Screen");
            Console.WriteLine("Name:".PadRight(17) + park.Name);
            Console.WriteLine("Location:".PadRight(17) + park.Location);
            Console.WriteLine("Established:".PadRight(17) + park.EstablishDate.ToString("yyyy-MM-dd"));
            Console.WriteLine("Area:".PadRight(17) + park.Area.ToString("N0") + (" acres"));
            Console.WriteLine("Annual Visitors:".PadRight(17) + park.AnnualVisitorCount.ToString("N0"));
            Console.WriteLine("Description:".PadRight(17));
            List<string> description = ReformatLargeText(park.Description);
            for (int i = 0; i < description.Count; i++) {
                if (i == 0)
                {
                    Console.WriteLine(description[i]);
                }
                Console.WriteLine(description[i]);
            }
        }

        public void ViewCampgroundsMenu(string park)
        {
            CampgroundSqlDAL campgroundSqlDAL = new CampgroundSqlDAL();

            List<Campground> campgrounds = campgroundSqlDAL.GetCampgrounds(park);
            Console.WriteLine();
            PrintTitleScreen(park + " Campgrounds");
            Console.WriteLine("".PadRight(4) + "Name".PadRight(32) + "Open".PadRight(9) + "Close".PadRight(11) + "Daily fee");

            for (int i = 0; i < campgrounds.Count; i++)
            {
                Console.WriteLine(i + 1 + ")".PadRight(3) +
                    campgrounds[i].Name.PadRight(32) +
                    ReturnMonthName(campgrounds[i].OpenMonth).PadRight(9) +
                    ReturnMonthName(campgrounds[i].ClosingMonth).PadRight(11) +
                    campgrounds[i].DailyFee.ToString("C2"));
            }
            
            int campgroundSelection = 0;
            bool validinput = false;
            do
            {
                Console.Write("\nWhich campground (enter 0 to cancel): ");
                string userInput = Console.ReadLine();

                if (int.TryParse(userInput, out campgroundSelection))
                {
                    if (campgroundSelection >= 0 && campgroundSelection <= campgrounds.Count)
                    {
                        validinput = true;
                    }
                }

                if (validinput == false) { Console.WriteLine("Not a valid input. Please try again."); }
                
            } while (validinput == false);

            if (campgroundSelection == 0) { return; }

            else { SearchCampgroundReservations(park, campgrounds[campgroundSelection - 1].Name); }
                    
        }

        public void SearchForUpcomingParkReservations(string name)
        {

        }

        public void SearchForParkReservations(string park)
        {
            DateTime today = System.DateTime.Now;
            DateTime futureMonthDate = today.AddMonths(1);
            ReservationSqlDAL reservationSqlDAL = new ReservationSqlDAL();

            List<Reservation> reservations = new List<Reservation>();
            reservations = reservationSqlDAL.GetParkReversations(park, today, futureMonthDate);

            if (reservations.Count != 0) {
                Console.WriteLine($"\nAll Reservations for {park}:");
                Console.WriteLine("ID".PadRight(4) + "Site ID".PadRight(9) + "Name".PadRight(32) + "From Date".PadRight(12) + "To Date");
                foreach (Reservation reservation in reservations)
                {
                    Console.WriteLine(
                        reservation.ReservationId.ToString().PadRight(4) +
                        reservation.SiteID.ToString().PadRight(9)+
                        reservation.Name.ToString().PadRight(32) +
                        reservation.FromDate.ToString("d").PadRight(12) +
                        reservation.ToDate.Date.ToString("d")
                        );
                }

                Console.WriteLine("\nPress <Enter> to continue!");
                Console.ReadLine();
            }
            else { Console.WriteLine($"No Reservations for {park}"); }
        }

        public void SearchCampgroundReservations(string park, string campground)
        {
            List<Campsite> avaiablereservations = new List<Campsite>();
            bool validInput = false;
            DateTime arrivalDate = DateTime.MinValue;
            DateTime departureDate = DateTime.MinValue;

            bool SearchReservations = true;
            do
            {
                do
                {
                    Console.Write("What is the arrival date? (mm/dd/yyyy): ");
                    string input = Console.ReadLine();
                    try
                    {
                        arrivalDate = DateTime.Parse(input);
                        validInput = true;
                    }
                    catch
                    {
                        Console.WriteLine("Invalid Input (mm/dd/yyyy): ");
                    }
                } while (validInput == false);

                validInput = false;
                do
                {
                    Console.Write("What is the departure date? (mm/dd/yyyy): ");
                    string input = Console.ReadLine();
                    try
                    {
                        departureDate = DateTime.Parse(input);
                        validInput = true;
                    }
                    catch
                    {
                        Console.WriteLine("Invalid Input (mm/dd/yyyy): ");
                    }
                } while (validInput == false);

                ReservationSqlDAL reservationSqlDAL = new ReservationSqlDAL();
                avaiablereservations = reservationSqlDAL.SearchForReservation(park, campground, arrivalDate, departureDate);
                if (avaiablereservations.Count == 0)
                {
                    Console.WriteLine("No available sites\n");
                    validInput = false;
                    do
                    {
                        try
                        {
                            Console.Write("Would you like to enter another date (Y/N): ");
                            char anotherSearch;
                            anotherSearch = Convert.ToChar(Console.ReadLine());
                            if (char.ToUpper(anotherSearch) == 'Y')
                            {
                                validInput = true;
                                SearchReservations = true;
                            }
                            else if (char.ToUpper(anotherSearch) == 'N')
                            {
                                validInput = true;
                                SearchReservations = false;
                                return;
                            }
                        }
                        catch
                        {
                            Console.WriteLine("\nPlease Enter (Y/N)");
                        }
                    } while (validInput == false);
                }
                else
                {
                    SearchReservations = false;
                }
            } while (SearchReservations == true);


            Console.WriteLine("\nResults Matching Your Search Criteria");
            Console.WriteLine("Site No.".PadRight(11) + "Max Occup.".PadRight(11) + "Accessible".PadRight(13) + "Max RV Length".PadRight(15) + "Utility".PadRight(9) + "Cost");

            foreach (Campsite campsite in avaiablereservations)
            {
                TimeSpan difference = departureDate - arrivalDate;

                Console.WriteLine(
                    Convert.ToString(campsite.SiteNumber).PadRight(11) +
                    Convert.ToString(campsite.MaxOccupancy).PadRight(11) +
                    Convert.ToString(campsite.Accessible).PadRight(13) +
                    Convert.ToString(campsite.MaxRvLength).PadRight(15) +
                    Convert.ToString(campsite.Utilities).PadRight(9) +
                    (campsite.DailyFee * (difference.Days + 1)).ToString("C2")
                    );
            }

            validInput = false;
            Campsite selectedCampsite = new Campsite();
            Console.WriteLine("");
            do
            {
                int userinput = 0;
                try
                {
                    Console.Write("Which site should be reserved (enter 0 to cancel): ");
                    userinput = int.Parse(Console.ReadLine());
                    if (userinput == 0) { return; }
                    foreach (Campsite campsite in avaiablereservations)
                    {
                        if (campsite.SiteNumber == userinput)
                        {
                            validInput = true;
                            selectedCampsite = campsite;
                        }
                    }
                    if (validInput == false) { Console.WriteLine("Invalid Site Number\n"); }
                }
                catch (FormatException e)
                {
                    Console.WriteLine("Invalid Input\n");
                }
            } while (validInput == false);
            Console.Write("What name should the reservation be made under): ");
            string name = Console.ReadLine();
            MakeNewReservation(name, selectedCampsite.SiteId, arrivalDate, departureDate);
            Console.WriteLine("");
            
        }

        public void MakeNewReservation(string name, int campsiteId, DateTime arrivalDate, DateTime departureDate)
        {
            ReservationSqlDAL reservationSqlDAl = new ReservationSqlDAL();
            int reservationId = reservationSqlDAl.MakeReservation(name, campsiteId, arrivalDate, departureDate);

            Console.WriteLine("The reservation has been made and the confirmation id is {0}", reservationId);
        }

        public void PrintTitleScreen(string name)
                {
                    string dashes = new string('-', name.Length + 2);
                    Console.WriteLine(dashes);
                    Console.WriteLine(" " + name);
                    Console.WriteLine(dashes);
                }

        public List<string> ReformatLargeText(string orignalText)
        {
            List<string> parts = new List<string>();
            int partLength = 35;

            string[] pieces = orignalText.Split(' ');
            StringBuilder tempString = new StringBuilder("");

            foreach (var piece in pieces)
            {
                if (piece.Length + tempString.Length + 1 > partLength)
                {
                    parts.Add(tempString.ToString());
                    tempString.Clear();
                }

                tempString.Append(piece + " ");
            }

            return parts;
        }

        public string ReturnMonthName(int monthNumber)
        {
            Dictionary<int, string> month = new Dictionary<int, string>();
            month.Add(1, "January");
            month.Add(2, "February");
            month.Add(3, "March");
            month.Add(4, "April");
            month.Add(5, "May");
            month.Add(6, "June");
            month.Add(7, "July");
            month.Add(8, "August");
            month.Add(9, "September");
            month.Add(10, "October");
            month.Add(11, "November");
            month.Add(12, "December");

            return month[monthNumber];
        }
    }
}