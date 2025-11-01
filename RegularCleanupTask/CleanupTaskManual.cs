namespace RegularCleanupTask
{
    public static class CleanupTaskManual
    {
        static string targetPath = @"D:/Work";
        static int daysThreshold = 30;
        static int scanIntervalDays = 1;

        static void test()
        {
            Helper.GetInfo(targetPath, daysThreshold, scanIntervalDays);

            Console.WriteLine("\nPress 'S' start the App. Press ANY KEY to exit the App.\n");
            var key = Console.ReadKey();

            if (key.Key != ConsoleKey.S)
                return;

            Console.WriteLine("\nAre you sure to start the cleanup task? Press 'Y' to confirm, or ANY KEY to exit.\n");
            if (Console.ReadKey().Key != ConsoleKey.Y)
                return;


        }
    }
}
