namespace System
{
    class Base
    {
        static void Main()
        {
            //Welcome message
            Console.WriteLine("Welcome to 8086 Simulator made by Hojda");
            Console.WriteLine("Here is a list of meta-commands available:\n");
            Console.WriteLine("~quit - exits the simulator");
            Console.WriteLine("");

            //Program
            string userInput;
            do
            {
                Console.Write("> ");
                userInput = Console.ReadLine();

                Command.Recognize(userInput);

            } while (userInput != "~quit");
        }
    }
}