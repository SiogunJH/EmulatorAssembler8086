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
            do
            {
                //Await user input and execute instruction
                Console.Write("> ");
                Recognize.Init(Console.ReadLine());

            } while (Storage.ContinueSimulation);

            //Output written code
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nCode saved to LATEST.txt file:");
            Storage.CodeDisplay();

            //Save code
            Algorithms.SAVE("SAVE LATEST");
        }
    }
}