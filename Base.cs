namespace System
{
    class Base
    {
        public static int Register;
        public static int Segments;
        public static int Pointers;
        public static int Flags;
        public static int Memory;
        static void Main()
        {
            //Welcome message
            Console.WriteLine("Welcome to 8086 Simulator made by Hojda");
            Console.WriteLine("Here is a list of meta-commands available:\n");
            Console.WriteLine("~QUIT - exits the simulator");
            Console.WriteLine("");

            //Memory, Flags, Register, Segments and Pointers

            //Program
            string userInput;
            do
            {
                Console.Write("> ");
                userInput = Console.ReadLine();

                Command.Recognize(userInput);

            } while (userInput.ToUpper() != "~QUIT");
        }
    }
}