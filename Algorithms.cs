namespace System
{
    class Algorithms
    {
        public static void MOV(string command)
        {
            if (!Tools.CheckForNumOfOperands(command, 2))
                return;

            command = command.Substring(3);
            string[] commandArray = command.Split(',');
            for (int i = 0; i < commandArray.Length; i++)
                commandArray[i] = commandArray[i].Trim();
        }
    }
}