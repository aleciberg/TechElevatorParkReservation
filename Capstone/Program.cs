using Capstone;
using System;

namespace capstone
{
    class Program
    {
        static void Main(string[] args)
        {
            ParkReservationCLI cli = new ParkReservationCLI();
            cli.RunCLI();
        }
    }
}
