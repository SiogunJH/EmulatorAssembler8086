namespace System
{
    class Tools
    {
        public static void WelcomeMessage()
        {
            Console.WriteLine("\nWelcome to 8086 Simulator made by Hojda");
            Console.WriteLine("Here is a list of meta-commands available:\n");
            Console.WriteLine("!QUIT - exit the simulator");
            Console.WriteLine("!FLAGS - show all FLAG values");
            Console.WriteLine("!REGISTER - show all REGISTER values");
            Console.WriteLine("!SEGMENTS - show all SEGMENTS values");
            Console.WriteLine("!POINTERS - show all POINTERS values");
            Console.WriteLine("!STORAGE - show all data");
            Console.WriteLine("");
        }
        public static void StorageInit()
        {
            Storage.RegisterInit();
            Storage.FlagsInit();
            Storage.SegmentsInit();
            Storage.PointersInit();
        }
        public static void StorageDisplay()
        {
            Storage.RegisterDisplay();
            Storage.FlagsDisplay();
            Storage.SegmentsDisplay();
            Storage.PointersDisplay();
        }
        public static bool CheckForNumOfOperands(string command, int expectedNumOfOperands)
        {
            command = command.Substring(command.Split(' ')[0].Length);
            string[] commandArray = command.Split(",");
            if (commandArray.Length != expectedNumOfOperands)
            {
                Console.WriteLine("Incorrect number of Operands!");
                return false;
            }
            return true;
        }
        public static string DetectOperandType(string operand)
        {
            if (operand.EndsWith('L') || operand.EndsWith('X') || operand.EndsWith('H')) //REGISTER OPERAND
                return "R";
            else if (operand.EndsWith('F')) //FLAG OPERAND
                return "F";
            else if (operand.EndsWith('S')) //SEGMENT OPERAND
                return "S";
            else if (operand.EndsWith('P') || operand.EndsWith('I')) //POINTER OPERAND
                return "P";
            else //NUMBER OPERAND
                return "N";
        }
        public static int ReadDataFromOperand(string operand)
        {
            string operandType = DetectOperandType(operand);
            switch (operandType)
            {
                case "R":
                    return Storage.Register[operand];
                default:
                    return -1;
            }
        }
    }
}