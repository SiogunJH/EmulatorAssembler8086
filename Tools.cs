namespace System
{
    class Tools
    {
        public static void WelcomeMessage()
        {
            Console.WriteLine("\nWelcome to 8086 Simulator made by Hojda");
            Console.WriteLine("Here is a list of meta-commands available:\n");
            Console.WriteLine("QUIT - exit the simulator");
            Console.WriteLine("FLAGS - show all FLAG values");
            Console.WriteLine("REGISTER - show all REGISTER values");
            Console.WriteLine("SEGMENTS - show all SEGMENTS values");
            Console.WriteLine("POINTERS - show all POINTERS values");
            Console.WriteLine("STORAGE - show all data");
            Console.WriteLine("CLEAR - clear console buffer");
            Console.WriteLine("DEBUG - toggle additional information (off by default)");
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

        //Parse string number to a decimal
        //number - value to be parsed
        //systemSize - system size of a number that will be parsed
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
                    throw new Exception($"Niepoprawna wartość numeryczna '{number}' dla systemu o rozmiarze '{systemSize}'");
            }
            return unchecked((int)Convert.ToInt64(number, systemSize));
        }
        //Test number for bit parity and adjust the Parity Flag (PF) accordingly
        //number - the decimal value, that will be tested 
        public static void UpdateParityFlag(int number)
        {
            char[] binary = Convert.ToString(number, 2).ToCharArray(); //[int] to [binary char array]
            int length = Array.FindAll(binary, element => element == '1').Length;
            if (length % 2 == 0)
                Storage.Flags["PF"] = 1;
            else
                Storage.Flags["PF"] = 0;
            return;
        }

        public static void CheckForNumOfOperands(string command, int expectedNumOfOperands)
        {
            string instruction = command.Split(' ')[0];
            command = command.Substring(instruction.Length);
            string[] commandArray = command.Split(",");
            if (commandArray.Length != expectedNumOfOperands)
            {
                throw new Exception($"Incorrect number of operands for '{instruction}' - recieved {commandArray.Length}, expected {expectedNumOfOperands}!");
            }
        }
        public static string DetectOperandType(string operand) //TODO: ADD MEMORY
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
            if (int.TryParse(operand, out int temp)) //NUMBER OPERAND DECIMAL
                return "numberD";
            if (operand.EndsWith("H")) //NUMBER OPERAND HEXADECIMAL
                return "numberH";
            if (operand.EndsWith("Q") || operand.EndsWith("O")) //NUMBER OPERAND OCTAL
                return "numberQ";
            if (operand.EndsWith("B")) //NUMBER OPERAND BINARY
                return "numberB";
            /*if (false) //MEMORY
                return "memory";*/

            throw new Exception($"Operand '{operand}' was not recognized!");

        }
        public static int ReadDataFromOperand(string operand, string operandType) //TODO: FIX THIS SO IT READS HEX NUMBERS
        {
            switch (operandType)
            {
                case "register": //REGISTER
                    return Storage.Register[operand];
                case "registerX": //REGISTER H+L
                    return Storage.Register[String.Format("{0}{1}", operand.Substring(0, 1), "H")] * 256 + Storage.Register[String.Format("{0}{1}", operand.Substring(0, 1), "L")];
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
            throw new Exception($"Incorrect operand type of '{operandType}' for operand '{operand}'");
        }
        public static void WriteDataToOperand(string operand, string operandType, int operandValue)
        {
            if (Base.DebugMode) Console.WriteLine($"Write Data To Operand:\n\tOperand: {operand}\n\tOperand Type: {operandType}\n\tValue: {operandValue}\n");
            switch (operandType)
            {
                case "register": //REGISTER
                    if (!(operandValue >= -128 && operandValue <= 255))
                        break;
                    if (operandValue < 0)
                        operandValue += 256;
                    Storage.Register[operand] = operandValue;
                    return;

                case "registerX": //REGISTER H+L
                    if (!(operandValue >= -32768 && operandValue <= 65535))
                        break;
                    if (operandValue < 0)
                        operandValue += 65536;
                    Storage.Register[String.Format("{0}{1}", operand.Substring(0, 1), "H")] = operandValue / 256;
                    Storage.Register[String.Format("{0}{1}", operand.Substring(0, 1), "L")] = operandValue % 256;
                    return;

                case "flag": //FLAG
                    if (!(operandValue >= 0 && operandValue <= 1))
                        break;
                    Storage.Flags[operand] = operandValue;
                    return;

                case "segment": //SEGMENT
                    if (!(operandValue >= -32768 && operandValue <= 65535))
                        break;
                    if (operandValue < 0)
                        operandValue += 65536;
                    Storage.Segments[operand] = operandValue;
                    return;

                case "pointer": //POINTER
                    if (!(operandValue >= -32768 && operandValue <= 65535))
                        break;
                    if (operandValue < 0)
                        operandValue += 65536;
                    Storage.Pointers[operand] = operandValue;
                    return;
            }
            throw new Exception($"Couldn't write '{operandValue}' to operand named '{operand}' with type of '{operandType}'");
        }
    }
}