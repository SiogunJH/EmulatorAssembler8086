namespace System
{
    class Tools
    {
        public static bool CheckForNumOfOperands(string command, int expectedNumOfOperands)
        {
            command = command.Substring(command.Split(' ')[0].Length);
            string[] commandArray = command.Split(",");
            if (commandArray.Length != expectedNumOfOperands)
            {
                Console.WriteLine("Incorrect Syntax!");
                return false;
            }
            return true;
        }
    }
}