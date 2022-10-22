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

            var temp = new Collections.Generic.Dictionary<String, Int32>();
            temp.Add("AH", 0);
            temp.Add("BH", 1);
            temp.Add("CH", 2);
            temp.Add("DH", 3);
            Console.WriteLine(temp["AH"]);
            Console.WriteLine(temp["BH"]);
            var t = "AH";
            temp[t] = 10;
            Console.WriteLine(temp["AH"]);

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