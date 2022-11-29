namespace System
{
    class Tools
    {
        //Message that will disply on startup
        //It contains all meta commands and info needed to use this simulator
        public static void StartupMessage()
        {
            //Welcome message with basic information
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Welcome to 8086 Simulator made by Hojda");
            Console.WriteLine("Here is a list of meta-commands available:");
            Console.WriteLine();

            //Data display
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("FLAGS    - show all FLAG values");
            Console.WriteLine("REGISTER - show all REGISTER values");
            Console.WriteLine("SEGMENTS - show all SEGMENTS values");
            Console.WriteLine("POINTERS - show all POINTERS values");
            Console.WriteLine("MEMORY   - show all MEMORY values");
            Console.WriteLine("STACK    - show all STACK values");
            Console.WriteLine("PORTS    - show all PORTS values");
            Console.WriteLine("STORAGE  - show all data");
            Console.WriteLine("DUMP     - reset all data");
            Console.WriteLine();

            //Debug display
            Console.WriteLine("DEBUG    - toggle additional information (off by default)");
            Console.WriteLine();

            //Save/Load data
            Console.WriteLine("CODE             - display written/loaded code");
            Console.WriteLine("DROP             - drop all written/loaded code (start over)");
            Console.WriteLine("SAVE <FileName>  - save written code to a named .txt file");
            Console.WriteLine("LOAD <FileName>  - load code from a named .txt file");
            Console.WriteLine("RUN              - dump current storage data and run written/loaded code");
            Console.WriteLine();

            //Clear and quit
            Console.WriteLine("CLEAR    - clear console buffer");
            Console.WriteLine("QUIT     - exit the simulator");
            Console.WriteLine();

            //Additional information
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Additional Information");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("- JUMP and LOOP type instructions will only work via use of 'RUN' meta command");
            //Console.WriteLine("- ...");
            Console.WriteLine();

            //Reset color
            Console.ForegroundColor = ConsoleColor.White;
        }

        //Prepare storage for use
        public static void StorageInit()
        {
            Storage.RegisterInit();
            Storage.FlagsInit();
            Storage.SegmentsInit();
            Storage.PointersInit();
            Storage.MemoryInit();
            Storage.StackInit();
            Storage.PortInit();
        }

        //Display all data within storage
        public static void StorageDisplay()
        {
            Storage.RegisterDisplay();
            Storage.FlagsDisplay();
            Storage.SegmentsDisplay();
            Storage.PointersDisplay();
            Storage.MemoryDisplay();
            Storage.StackDisplay();
            Storage.PortDisplay();
        }

        //Dump all data within storage and reinitiate its base values
        public static void StorageDump()
        {
            Storage.RegisterDump();
            Storage.FlagsDump();
            Storage.SegmentsDump();
            Storage.PointersDump();
            Storage.MemoryDump();
            Storage.StackDump();
            Storage.PortDump();
        }

        ///<summary> 
        ///Funkcja <c>Parse</c> konwertuje liczbę w formie <c>string</c> z dowolnego systemu liczbowego w liczbę w formie <c>int</c> systemu dziesiętnego
        ///</summary>
        ///<param name="number">Liczba w formacie <c>string</c></param>
        ///<param name="systemSize">Rozmiar systemu, z którego konwertowana jest liczba (np. <c>16</c> dla systemu szesnastkowego i <c>10</c> dla systemu dziesiętnego)</param>
        ///<returns>Wartość <c>int</c> odpowiadająca wartością przyjętego parametru</returns>
        public static long Parse(string number, int systemSize)
        {
            //Define what characters are allowed in a system
            string systemNumbers = "0123456789ABCDEF".Substring(0, systemSize);

            //Check if all characters are correct for a number system
            char[] numberArray = number.ToCharArray();
            for (long i = 0; i < numberArray.Length; i++)
            {
                if (!(systemNumbers.Contains(numberArray[i])))
                    throw new Exception($"Niepoprawna wartość numeryczna '{number}' dla systemu o rozmiarze '{systemSize}'");
            }
            return unchecked((long)Convert.ToInt64(number, systemSize));
        }

        //Test if number is equal to 0 and update Zero Flag (ZF) accordingly
        public static void UpdateZeroFlag(long number)
        {
            //Check if number is equal to zero
            if (number == 0)
                Storage.Flags["ZF"] = 1;
            else
                Storage.Flags["ZF"] = 0;
        }

        //Test number for Bit Parity and adjust the Parity Flag (PF) accordingly
        public static void UpdateParityFlag(long number)
        {
            //[long] to [binary char array]
            char[] binary = Convert.ToString(number, 2).ToCharArray();

            //Count 1s and set Parity Flag accordingly
            long length = Array.FindAll(binary, element => element == '1').Length;
            if (length % 2 == 0)
                Storage.Flags["PF"] = 1;
            else
                Storage.Flags["PF"] = 0;
        }

        //Check for High Bit value and adjust the Sign Flag (SF) accordingly
        public static void UpdateSignFlag(long number, string operandType)
        {
            //[long] to [binary char array]
            char[] binary = Convert.ToString(number, 2).ToCharArray();

            //Determine number of bits needed to save a specific data type
            long numOfBits;
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

        //Modify value, so it may fit to operand
        public static long AdjustValue(long operandValue, string operandType, bool modifyFlags)
        {
            //DEBUG Display
            if (Storage.DebugMode) Console.WriteLine("Adjust Value:");
            if (Storage.DebugMode) Console.WriteLine("\tOperand Value: {0}", operandValue);
            if (Storage.DebugMode) Console.WriteLine("\tOperand Type: {0}", operandType);

            //Determine max possible value
            long maxValue;
            if (operandType == "regHL" || operandType == "memory") maxValue = 256;
            else if (operandType == "flag") maxValue = 1;
            else maxValue = 65536;
            if (Storage.DebugMode) Console.WriteLine("\tMax Value: {0}", maxValue);

            //Check if negative
            if (operandValue < 0)
            {
                operandValue += maxValue;
                if (Storage.DebugMode) Console.WriteLine("\tNegation Adjust: {0}", operandValue);
            }

            //Check if above limit
            if (operandValue >= maxValue)
            {
                operandValue %= maxValue;
                if (Storage.DebugMode) Console.WriteLine("\tOverflow Adjust: {0}", operandValue);
            }

            //Return new value
            if (Storage.DebugMode) Console.WriteLine("\t---");
            return operandValue;
        }

        //Check if correct number of operand were used
        //Throw error if incorrect
        public static void CheckForNumOfOperands(string command, long expectedNumOfOperands)
        {
            //Save instruction name
            string instruction = command.Split(' ')[0];

            //Split command into array of operands
            command = command.Substring(instruction.Length);
            string[] commandArray = command.Split(",");
            long recievedNumOfOperands = commandArray.Length;

            //Check for no operands
            if (commandArray.Length == 1 && commandArray[0].Trim() == "") recievedNumOfOperands = 0;

            //Throw exception if expected number of operand is not equal to the number of operands recieved
            if (recievedNumOfOperands != expectedNumOfOperands)
            {
                throw new Exception($"Incorrect number of operands for '{instruction}' - recieved {recievedNumOfOperands}, expected {expectedNumOfOperands}!");
            }
        }

        //Detect operand type
        //Throw error if no match is found
        public static string DetectOperandType(string operand)
        {
            if ("AH;BH;CH;DH;AL;BL;CL;DL".Contains(operand)) //REGISTER OPERAND
                return "regHL";
            if ("AX;BX;CX;DX".Contains(operand)) //REGISTER H+L OPERAND
                return "regX";
            if ("OF;DF;IF;TF;SF;ZF;AF;PF;CF".Contains(operand)) //FLAG OPERAND
                return "flag";
            if ("SS;DS;ES".Contains(operand)) //SEGMENT OPERAND
                return "segment";
            if ("SP;BP;SI;DI".Contains(operand)) //POINTER OPERAND
                return "pointer";
            if (long.TryParse(operand, out long temp)) //NUMBER OPERAND DECIMAL
                return "numberD";
            if (operand.EndsWith("H")) //NUMBER OPERAND HEXADECIMAL
                return "numberH";
            if (operand.EndsWith("Q") || operand.EndsWith("O")) //NUMBER OPERAND OCTAL
                return "numberQ";
            if (operand.EndsWith("B")) //NUMBER OPERAND BINARY
                return "numberB";
            if (operand.StartsWith('[') && operand.EndsWith(']')) //MEMORY
                return "memory";
            // if (operand.Contains('+') || operand.Contains('-') || operand.Contains('*') || operand.Contains('/')) //TODO
            //     return "equation";

            throw new Exception($"Operand '{operand}' was not recognized!");
        }

        //Read data from operand
        //Throw error if cannot reach address
        public static long ReadDataFromOperand(string operand, string operandType)
        {
            switch (operandType)
            {
                case "regHL": //REGISTER
                    return Storage.Register[operand];
                case "regX": //REGISTER H+L
                    return Storage.Register[String.Format("{0}{1}", operand.Substring(0, 1), "H")] * 256 + Storage.Register[String.Format("{0}{1}", operand.Substring(0, 1), "L")];
                case "flag": //FLAG
                    return Storage.Flags[operand];
                case "segment": //SEGMENT
                    return Storage.Segments[operand];
                case "pointer": //POINTER
                    return Storage.Pointers[operand];
                case "numberD": //NUMBER DECIMAL
                    return long.Parse(operand);
                case "numberH": //NUMBER HEXA
                    return Tools.Parse(operand.Substring(0, operand.Length - 1), 16);
                case "numberQ": //NUMBER OCTAL
                    return Tools.Parse(operand.Substring(0, operand.Length - 1), 8);
                case "numberB": //NUMBER BINARY
                    return Tools.Parse(operand.Substring(0, operand.Length - 1), 2);
                case "memory":
                    return Tools.ReadDataFromMemory(operand);
                case "equation":
                    return 0;//Tools.ReadDataFromEquation(operand);
            }
            throw new Exception($"Incorrect operand type of '{operandType}' for operand '{operand}'");
        }

        //Determines the memory address and reads data stored withing that address
        public static long ReadDataFromMemory(string operand)
        {
            //Przygotuj zmienne
            long address;
            operand = operand.Substring(1, operand.Length - 2); //Usuń nawiasy kwadratowe

            //Określ adres
            address = Tools.ReadDataFromOperand(operand, Tools.DetectOperandType(operand));

            //Odczytaj zawartość konkretnego adresu
            long addressValue = 0;
            bool results = Storage.Memory.TryGetValue(address, out addressValue);
            if (Storage.DebugMode) Console.WriteLine("Read Data From Memory:\n\tAddress: {0:X4}\n\tValue: {1:X2}\n", address, addressValue);

            //Zwróć wynik
            return addressValue;
        }

        ///<summary>
        ///Funkcja znajduje odpowiedni operand opdowiadający przesłanym danym i zapisuje nową wartość w jego miejscu
        ///</summary>
        public static void WriteDataToOperand(string operand, string operandType, long operandValue)
        {
            //DEBUG Display
            if (Storage.DebugMode) Console.WriteLine("Write Data to Operand:");
            if (Storage.DebugMode) Console.WriteLine("\tOperand: {0}", operand);
            if (Storage.DebugMode) Console.WriteLine("\tOperand Type: {0}", operandType);
            if (Storage.DebugMode) Console.WriteLine("\tOperand Value: {0}", operandValue);
            if (Storage.DebugMode) Console.WriteLine("\t---");

            switch (operandType)
            {
                case "regHL": //REGISTER
                    if (!(operandValue >= 0 && operandValue <= 255))
                        break;
                    Storage.Register[operand] = operandValue;
                    return;

                case "regX": //REGISTER H+L
                    if (!(operandValue >= 0 && operandValue <= 65535))
                        break;
                    Storage.Register[String.Format("{0}{1}", operand.Substring(0, 1), "H")] = operandValue / 256;
                    Storage.Register[String.Format("{0}{1}", operand.Substring(0, 1), "L")] = operandValue % 256;
                    return;

                case "flag": //FLAG
                    if (!(operandValue >= 0 && operandValue <= 1))
                        break;
                    Storage.Flags[operand] = operandValue;
                    return;

                case "segment": //SEGMENT
                    if (!(operandValue >= 0 && operandValue <= 65535))
                        break;
                    Storage.Segments[operand] = operandValue;
                    return;

                case "pointer": //POINTER
                    if (!(operandValue >= 0 && operandValue <= 65535))
                        break;
                    Storage.Pointers[operand] = operandValue;
                    return;

                case "memory": //MEMORY
                    if (!(operandValue >= 0 && operandValue <= 255))
                        break;

                    //Define address
                    string addressString = operand.Substring(1, operand.Length - 2);
                    long address = Tools.ReadDataFromOperand(addressString, Tools.DetectOperandType(addressString));

                    //Save data to address
                    bool results = Storage.Memory.ContainsKey(address);
                    if (results) //Memory address is in use
                        Storage.Memory[address] = operandValue;
                    else //New memory address
                        Storage.Memory.Add(address, operandValue);
                    return;
            }
            throw new Exception($"Couldn't write '{operandValue}' to operand named '{operand}' with type of '{operandType}'");
        }
    }
}