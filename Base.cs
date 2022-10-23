namespace System
{
    public class Base
    {
        public static bool continueSimulation = true;
        static void Main()
        {
            //Welcome message
            Console.WriteLine("Welcome to 8086 Simulator made by Hojda");
            Console.WriteLine("Here is a list of meta-commands available:\n");
            Console.WriteLine("~QUIT - exits the simulator, just as 'HLT' does");
            Console.WriteLine("~FLAGS - show all FLAG values");
            Console.WriteLine("~REGISTER - show all REGISTER values");
            Console.WriteLine("~SEGMENTS - show all SEGMENTS values");
            Console.WriteLine("~POINTERS - show all POINTERS values");
            Console.WriteLine("");

            Storage.RegisterInit();
            Storage.FlagsInit();
            Storage.SegmentsInit();
            Storage.PointersInit();

            //Program
            string userInput;
            do
            {
                Console.Write("> ");
                userInput = Console.ReadLine();

                Command.Recognize(userInput);

            } while (continueSimulation);
        }
    }
}