namespace System
{
    public class Base
    {
        public static bool continueSimulation = true;
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
                Console.Write("> ");
                userInput = Console.ReadLine();

                Command.Recognize(userInput.ToUpper());

            } while (continueSimulation);
        }
    }
}