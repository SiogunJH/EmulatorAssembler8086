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

            //Program
            string userInput;
            do
            {
                //USER INPUT
                Console.Write("> ");
                userInput = Console.ReadLine();

                //PROCESS COMMAND
                try
                {
                    Command.Recognize(userInput.ToUpper());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    if (Storage.DebugMode) Console.WriteLine(e.StackTrace);
                }

            } while (Storage.ContinueSimulation);
        }
    }
}