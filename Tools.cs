namespace System
{
    class Tools
    {
        public static void WelcomeMessage()
        {
            Console.WriteLine("Welcome to 8086 Simulator made by Hojda");
            Console.WriteLine("Here is a list of meta-commands available:\n");
            Console.WriteLine("QUIT - exit the simulator");
            Console.WriteLine("FLAGS - show all FLAG values");
            Console.WriteLine("REGISTER - show all REGISTER values");
            Console.WriteLine("SEGMENTS - show all SEGMENTS values");
            Console.WriteLine("POINTERS - show all POINTERS values");
            Console.WriteLine("MEMORY - show all MEMORY values");
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
            Storage.MemoryInit();
        }
        public static void StorageDisplay()
        {
            Storage.RegisterDisplay();
            Storage.FlagsDisplay();
            Storage.SegmentsDisplay();
            Storage.PointersDisplay();
            Storage.MemoryDisplay();
        }

        //Parse string number to a decimal
        //number - value to be parsed
        //systemSize - system size of a number that will be parsed
        public static int Parse(string number, int systemSize)
        {
            //Define what characters are allowed in a system
            string systemNumbers = "0123456789ABCDEF".Substring(0, systemSize);

            //Check if all characters are correct for a number system
            char[] numberArray = number.ToCharArray();
            for (int i = 0; i < numberArray.Length; i++)
            {
                if (!(systemNumbers.Contains(numberArray[i])))
                    throw new Exception($"Niepoprawna wartość numeryczna '{number}' dla systemu o rozmiarze '{systemSize}'");
            }
            return unchecked((int)Convert.ToInt64(number, systemSize));
        }

        //Test number for Bit Parity and adjust the Parity Flag (PF) accordingly
        public static void UpdateParityFlag(int number)
        {
            //[int] to [binary char array]
            char[] binary = Convert.ToString(number, 2).ToCharArray();

            //Count 1s and set Parity Flag accordingly
            int length = Array.FindAll(binary, element => element == '1').Length;
            if (length % 2 == 0)
                Storage.Flags["PF"] = 1;
            else
                Storage.Flags["PF"] = 0;
        }

        //Check for High Bit value and adjust the Sign Flag (SF) accordingly
        public static void UpdateSignFlag(int number, string operandType)
        {
            //[int] to [binary char array]
            char[] binary = Convert.ToString(number, 2).ToCharArray();

            //Determine number of bits needed to save a specific data type
            int numOfBits;
            if (operandType == "register" || operandType == "memory")
                numOfBits = 8;
            else
                numOfBits = 16;

            //Check if binary length of a number is equal to number of bits used to write a specific data type; if so, then high bit is 1 and the number is negative
            if (binary.Length == numOfBits)
                Storage.Flags["SF"] = 1;
            else
                Storage.Flags["SF"] = 0;
        }

        public static int AdjustValue(int operandValue, string operandType, bool modifyFlags)
        {
            //Determine max possible value
            int maxValue;
            if (operandType == "register" || operandType == "memory") maxValue = 256;
            else if (operandType == "flag") maxValue = 1;
            else maxValue = 65536;

            //Check if negative
            while (operandValue < 0)
            {
                operandValue += maxValue;
            }

            //Check if above limit
            if (operandValue >= maxValue)
            {
                operandValue %= maxValue;
            }

            //Return new value
            return operandValue;
        }

        public static void CheckForNumOfOperands(string command, int expectedNumOfOperands)
        {
            //Save instruction name
            string instruction = command.Split(' ')[0];

            //Split command into array of operands
            command = command.Substring(instruction.Length);
            string[] commandArray = command.Split(",");
            int recievedNumOfOperands = commandArray.Length;

            //Check for no operands
            if (commandArray.Length == 1 && commandArray[0].Trim() == "") recievedNumOfOperands = 0;

            //Throw exception if expected number of operand is not equal to the number of operands recieved
            if (recievedNumOfOperands != expectedNumOfOperands)
            {
                throw new Exception($"Incorrect number of operands for '{instruction}' - recieved {recievedNumOfOperands}, expected {expectedNumOfOperands}!");
            }
        }

        public static string DetectOperandType(string operand)
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
            if (operand.StartsWith('[') && operand.EndsWith(']')) //MEMORY
                return "memory";
            if (operand.Contains('+') || operand.Contains('-') || operand.Contains('*') || operand.Contains('/')) //TODO
                return "equation";

            throw new Exception($"Operand '{operand}' was not recognized!");
        }

        public static int ReadDataFromOperand(string operand, string operandType)
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
                case "memory":
                    return Tools.ReadDataFromMemory(operand);
                case "equation":
                    return Tools.ReadDataFromEquation(operand);
            }
            throw new Exception($"Incorrect operand type of '{operandType}' for operand '{operand}'");
        }

        //Determines the memory address and reads data stored withing that address
        public static int ReadDataFromMemory(string operand)
        {
            //Przygotuj zmienne
            int results = 0;
            if (operand.StartsWith('[') && operand.EndsWith(']'))
                operand = operand.Substring(1, operand.Length - 2);

            //Rekurencyjne rozbij i wykonaj działania złożone
            ; //TODO
            results = Tools.ReadDataFromOperand(operand, Tools.DetectOperandType(operand));

            //Odczytaj konkretny adres

            //Zwróć wynik
            return results;
        }

        public static int ReadDataFromEquation(string operand)
        {
            //Przygotuj zmienne
            int results;
            string[] arguments;
            string[] operations;

            //Split equation into arguments and operations
            while (operand.Length != 0)
            {

            }

            //Rekurencyjne rozbij i wykonaj działania złożone
            ; //TODO
            results = Tools.ReadDataFromOperand(operand, Tools.DetectOperandType(operand));

            //Odczytaj konkretny adres

            //Zwróć wynik
            return results;
        }

        public static void WriteDataToOperand(string operand, string operandType, int operandValue)
        {
            if (Storage.DebugMode) Console.WriteLine($"Write Data To Operand:\n\tOperand: {operand}\n\tOperand Type: {operandType}\n\tValue: {operandValue}\n");
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

                case "memory": //MEMORY

                    return;
            }
            throw new Exception($"Couldn't write '{operandValue}' to operand named '{operand}' with type of '{operandType}'");
        }
    }
}