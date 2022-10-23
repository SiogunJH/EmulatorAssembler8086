namespace System
{
    public class Base
    {
        public static bool continueSimulation = true;
        static void Main()
        {
            Tools.WelcomeMessage();
            Tools.StorageInit();

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