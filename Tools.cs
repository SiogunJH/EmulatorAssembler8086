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
        public static int Parse(string number, int systemSize)
        {

            //Define what characters are allowed in a system
            string systemNumbers = "0123456789ABCDEF";
            systemNumbers = systemNumbers.Substring(0, systemSize);

            //Check if all characters are correct for a number system
            char[] numberArray = number.ToCharArray();
            for (int i = 0; i < numberArray.Length; i++)
            {
                if (!(systemNumbers.Contains(numberArray[i])))
                {
                    Console.WriteLine($"Niepoprawna wartość numeryczna '{number}'");
                    return -1;
                }
            }
            return unchecked((int)Convert.ToInt64(number, systemSize));
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
            if ("AH;BH;CH;DH;AL;BL;CL;DL".Contains(operand)) //REGISTER OPERAND
                return "register";
            if ("AX;BX;CX;DX".Contains(operand)) //REGISTER H+L OPERAND
                return "registerX";
            if ("OF;DF;IF;TF;SF;ZF;AF;PF;CF".Contains(operand)) //FLAG OPERAND
                return "flag";
            if ("SS;DS;ES".Contains(operand)) //SEGMENT OPERAND
                return "segment";
            if ("SP;BP;SI;DI".Contains(operand)) //POINTER OPERAND
                return "pointer";
            if (int.TryParse(operand, out int temp))//NUMBER OPERAND
                return "numberD";
            if (operand.EndsWith("H"))
                return "numberH";
            if (operand.EndsWith("Q") || operand.EndsWith("O"))
                return "numberQ";
            if (operand.EndsWith("B"))
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
                    return Storage.Register[operand];
                case "registerX": //REGISTER H+L
                    return 0;
                case "flag": //FLAG
                    return Storage.Flags[operand];
                case "segment": //SEGMENT
                    return Storage.Segments[operand];
                case "pointer": //POINTER
                    return Storage.Pointers[operand];
                case "numberD": //NUMBER DECIMAL
                    return int.Parse(operand);
                case "numberH": //NUMBER HEXA
                    return Tools.Parse(operand.Substring(0, operand.Length - 1), 16);
                case "numberQ": //NUMBER OCTAL
                    return Tools.Parse(operand.Substring(0, operand.Length - 1), 8);
                case "numberB": //NUMBER BINARY
                    return Tools.Parse(operand.Substring(0, operand.Length - 1), 2);


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
                    if (!(operandValue >= 0 && operandValue <= 255))
                        break;
                    Storage.Register[operand] = operandValue;
                    return true;
                case "registerX":
                    if (!(operandValue >= 0 && operandValue <= 65535))
                        break;
                    Storage.Register[String.Format("{0}{1}", operand.Substring(0,1), "H")] = operandValue/256;
                    Storage.Register[String.Format("{0}{1}", operand.Substring(0,1), "L")] = operandValue%256;
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