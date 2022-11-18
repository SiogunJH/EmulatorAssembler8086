namespace System
{
    public class Base
    {
        static void Main()
        {
            //Startup
            Console.Clear();
            Tools.WelcomeMessage();
            Tools.StorageInit();

            //Testfield

            //Simulation
            string userInput;
            do
            {
                //User Input
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