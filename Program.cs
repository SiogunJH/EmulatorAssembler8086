namespace System
{
    class Hello
    {
        static void Main()
        {
            string userInput;
            do
            {
                userInput = Console.ReadLine();
                Console.WriteLine($"Recieved: {userInput}");
            } while (userInput != "~quit");
        }
    }
}