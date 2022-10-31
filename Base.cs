namespace System
{
    public class Base
    {
        public static bool ContinueSimulation = true;
        public static bool DebugMode = false;
        static void Main()
        {
            //Startup
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
                    if (DebugMode) Console.WriteLine(e.StackTrace);
                }

            } while (ContinueSimulation);
        }
    }
}