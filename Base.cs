namespace System
{
    public class Base
    {
        static void Main()
        {
            //Startup
            Console.Clear();
            Tools.StartupMessage();
            Tools.StorageInit();

            //Testfield

            //Simulation
            string userInput;
            do
            {
                //Await user input
                Console.Write("> ");
                userInput = Console.ReadLine();

                //Execute instruction
                Recognize.Init(userInput);

            } while (Storage.ContinueSimulation);

            //Output saved code
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nCode written:");
            Storage.CodeDisplay();
        }
    }
}