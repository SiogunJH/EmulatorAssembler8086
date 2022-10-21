namespace System
{
    class Command
    {
        public static void Recognize(string command)
        {
            //Prepare command for analyze
            command = command.Trim();
            command = command.Split(";")[0];

            //No response nor action
            if (command == "" || command == null)
                return;

            //Detect command
            string[] commandArray = command.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            switch (commandArray[0])
            {
                case "~quit":
                    Console.WriteLine("Exiting the Simulator...");
                    break;
                default:
                    Console.WriteLine($"Command '{commandArray[0]}' was not recognized - make sure there are no typos and try again");
                    break;
            }
        }
    }

}