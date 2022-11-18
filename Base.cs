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
                //USER INPUT
                Console.Write("> ");
                userInput = Console.ReadLine();

                //PROCESS COMMAND
                try
                {
                    //Execute instruction
                    Recognize.Init(userInput);

                    //Add instruction to saved code
                    if (!Storage.DoNotSaveToCode)
                        Storage.SavedCode.Add(userInput.ToUpper());
                }
                //SEND ERROR MESSAGE
                catch (Exception e)
                {
                    //Send error message
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);

                    //Send additional debug data
                    if (Storage.DebugMode) Console.WriteLine(e.StackTrace);
                    Console.ForegroundColor = ConsoleColor.White;
                }

            } while (Storage.ContinueSimulation);

            //Output saved code
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nCode written:");
            Storage.CodeDisplay();
        }
    }
}