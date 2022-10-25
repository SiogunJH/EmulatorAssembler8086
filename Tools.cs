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
        public static int Parse(string number, string numberType)
        {
            //int temp = unchecked((int)Convert.ToInt64(hexValue, 16));
            return 0;
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
        public static string DetectOperandType(string operand) //TODO: FIX DETECTION
        {
            if ("AH;BH;CH;DH;AL;BL;CL;DL;AX;BX;CX;DX".Contains(operand)) //REGISTER OPERAND
                return "register";
            if ("OF;DF;IF;TF;SF;ZF;AF;PF;CF".Contains(operand)) //FLAG OPERAND
                return "flag";
            if ("SS;DS;ES".Contains(operand)) //SEGMENT OPERAND
                return "segment";
            if ("SP;BP;SI;DI".Contains(operand)) //POINTER OPERAND
                return "pointer";
            if (int.TryParse(operand, out int temp))//NUMBER OPERAND
                return "numberD";
            if (operand.EndsWith("H"))
                //TODO: TEST IF CORRECT
                return "numberH";
            if (operand.EndsWith("Q") || operand.EndsWith("O"))
                //TODO: TEST IF CORRECT
                return "numberQ";
            if (operand.EndsWith("B"))
                //TODO: TEST IF CORRECT
                return "numberB";
            /*if (false) //MEMORY
                return "memory";*/

            Console.WriteLine($"Operand {operand} was not recognized!");
            return "error";

        }
        public static int ReadDataFromOperand(string operand, string operandType) //TODO: FIX THIS SO IT READS HEX NUMBERS
        {
            switch (operandType)
            {
                case "register": //REGISTER
                    operand = operand.Replace('X', 'L');
                    return Storage.Register[operand];
                case "flag": //FLAG
                    return Storage.Flags[operand];
                case "segment": //SEGMENT
                    return Storage.Segments[operand];
                case "pointer": //POINTER
                    return Storage.Pointers[operand];
                case "numberD": //NUMBER DECIMAL
                    return int.Parse(operand);
                case "numberH":
                    return Tools.Parse(operand.Substring(0, operand.Length - 1), "H");
                case "numberQ":
                    return Tools.Parse(operand.Substring(0, operand.Length - 1), "Q");
                case "numberB":
                    return Tools.Parse(operand.Substring(0, operand.Length - 1), "B");


            }
            //ERROR
            Console.WriteLine($"Incorrect operand type of '{operandType}' for operand '{operand}'");
            return -1;
        }
        public static bool WriteDataToOperand(string operand, string operandType, int operandValue)
        {
            //Console.WriteLine($"Writing {operandValue} to {operandType} operand, named {operand}");
            switch (operandType)
            {
                case "register": //REGISTER
                    operand = operand.Replace('X', 'L');
                    if (!(operandValue >= 0 && operandValue <= 255))
                        break;
                    Storage.Register[operand] = operandValue;
                    return true;
                case "flag": //FLAG
                    if (!(operandValue >= 0 && operandValue <= 1))
                        break;
                    Storage.Flags[operand] = operandValue;
                    return true;
                case "segment": //SEGMENT
                    if (!(operandValue >= 0 && operandValue <= 255))
                        break;
                    Storage.Segments[operand] = operandValue;
                    return true;
                case "pointer": //POINTER
                    if (!(operandValue >= 0 && operandValue <= 255))
                        break;
                    Storage.Pointers[operand] = operandValue;
                    return true;
            }
            Console.WriteLine($"Couldn't write '{operandValue}' to '{operandType}' operand named '{operand}'");
            return false;
        }
    }
}