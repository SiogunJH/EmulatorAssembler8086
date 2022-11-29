namespace System
{
    public class Recognize
    {
        ///<summary>
        ///Funkcja przyjmuje instrukcję wpisaną przez użytkownika, formatuje ją, interpretuje i zapisuje w pamięci tymczasowej
        ///</summary>
        public static void Init(string command)
        {
            //Reset variable(s)
            Storage.DoNotSaveToCode = false;

            //Prepare command for analyze
            command = command
                .Split(";")[0] //Remove comment
                .Trim() //Remove whitespaces
                .ToUpper(); //Go all caps

            //Check for empty command
            if (command == "" || command == null)
            {
                Storage.DoNotSaveToCode = true;
                return;
            }

            //Check if command is a label
            else if (command.Split(' ').Length == 1 && command.EndsWith(':'))
            {
                if (Storage.DebugMode) Console.WriteLine("Label detected: {0}", command.Substring(0, command.Length - 1));
            }

            //Detect and execute a command
            else
            {
                try
                {
                    Recognize.Command(command.Split(' ')[0], command);
                }
                catch (Exception e)
                {
                    //Send error message
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);

                    //Send additional debug data
                    if (Storage.DebugMode) Console.WriteLine(e.StackTrace);
                    Console.ForegroundColor = ConsoleColor.White;

                    //Do not save to code
                    Storage.DoNotSaveToCode = true;
                }
            }

            //Add instruction to saved code
            if (!Storage.DoNotSaveToCode)
                Storage.SavedCode.Add(command);
        }

        ///<summary>
        ///Funkcja przetwarza odpowiednio przygotowany i sformatowany kod, linijka po linijce z uwzględnieniem pętli i skoków
        ///</summary>
        public static void AutoRun(int interval)
        {
            //Storage dump
            Tools.StorageDump();

            //Set variables
            Storage.AutoRun = true;

            //Run loaded code
            try
            {
                while (Storage.ContinueSimulation)
                {
                    //Store old index for later use
                    int oldIndex = (int)Storage.Pointers["IP"];

                    //Skip labels
                    if (Storage.SavedCode[oldIndex].Split(' ').Length == 1 && Storage.SavedCode[oldIndex].EndsWith(':'))
                        Storage.Pointers["IP"]++;

                    //Recognize and execute command
                    else
                        Recognize.Command(Storage.SavedCode[oldIndex].Split(' ')[0], Storage.SavedCode[oldIndex]);

                    //Display instruction
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(Storage.SavedCode[oldIndex]);
                    Console.ForegroundColor = ConsoleColor.White;

                    //Interval
                    System.Threading.Thread.Sleep(interval);
                }
            }
            catch (Exception e)
            {
                //Display instruction
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(Storage.SavedCode[(int)Storage.Pointers["IP"]]);
                Console.ForegroundColor = ConsoleColor.White;

                //Send error message
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);

                //Send additional debug data
                if (Storage.DebugMode) Console.WriteLine(e.StackTrace);
                Console.ForegroundColor = ConsoleColor.White;
            }

            //Reset variables
            Storage.AutoRun = false;
            Storage.ContinueSimulation = true;
        }

        ///<summary>
        ///Funkcja rozpoznaje odpowiednio sfotmaowaną instrukcję i uruchamia jej algorytm
        ///</summary>
        public static void Command(string instruction, string command)
        {
            switch (instruction)
            {
                //DATA TRANSFER
                case "MOV":
                    Algorithms.MOV(command);
                    break;
                case "LDS":
                    throw new NotImplementedException("This instruction is not implemented");
                case "LES":
                    throw new NotImplementedException("This instruction is not implemented");
                case "LEA":
                    Algorithms.LEA(command);
                    break;
                case "LAHF":
                    Algorithms.LAHF(command);
                    break;
                case "SAHF":
                    Algorithms.SAHF(command);
                    break;
                case "XLAT":
                case "XLATB":
                    throw new NotImplementedException("This instruction is not implemented");
                case "XCHG":
                    Algorithms.XCHG(command);
                    break;
                case "PUSH":
                    Algorithms.PUSH(command);
                    break;
                case "PUSHF":
                    Algorithms.PUSHF(command);
                    break;
                case "POP":
                    Algorithms.POP(command);
                    break;
                case "POPF":
                    Algorithms.POPF(command);
                    break;
                case "IN":
                    Algorithms.IN(command);
                    break;
                case "OUT":
                    Algorithms.OUT(command);
                    break;

                //ARITHMETIC
                case "ADD":
                    Algorithms.ADD(command);
                    break;
                case "ADC":
                    Algorithms.ADC(command);
                    break;
                case "SUB":
                    Algorithms.SUB(command);
                    break;
                case "SBB":
                    Algorithms.SBB(command);
                    break;
                case "MUL":
                    Algorithms.MUL(command);
                    break;
                case "IMUL":
                    Algorithms.IMUL(command);
                    break;
                case "DIV":
                    Algorithms.DIV(command);
                    break;
                case "IDIV":
                    Algorithms.IDIV(command);
                    break;
                case "INC":
                    Algorithms.INC(command);
                    break;
                case "DEC":
                    Algorithms.DEC(command);
                    break;
                case "DAA":
                    Algorithms.DAA(command);
                    break;
                case "DAS":
                    Algorithms.DAS(command);
                    break;
                case "AAA":
                    Algorithms.AAA(command);
                    break;
                case "AAD":
                    Algorithms.AAD(command);
                    break;
                case "AAM":
                    Algorithms.AAM(command);
                    break;
                case "AAS":
                    Algorithms.AAS(command);
                    break;
                case "CBW":
                    Algorithms.CBW(command);
                    break;
                case "CWD":
                    Algorithms.CWD(command);
                    break;
                case "NEG":
                    Algorithms.NEG(command);
                    break;
                case "CMP":
                    throw new NotImplementedException("This instruction is not implemented");

                //LOGICAL
                case "AND":
                    Algorithms.AND(command);
                    break;
                case "OR":
                    Algorithms.OR(command);
                    break;
                case "XOR":
                    Algorithms.XOR(command);
                    break;
                case "NOT":
                    Algorithms.NOT(command);
                    break;
                case "TEST":
                    Algorithms.TEST(command);
                    break;

                //ROTATE
                case "RCL":
                    Algorithms.RCL(command);
                    break;
                case "RCR":
                    Algorithms.RCR(command);
                    break;
                case "ROL":
                    Algorithms.ROL(command);
                    break;
                case "ROR":
                    Algorithms.ROR(command);
                    break;

                //SHIFT
                case "SAL":
                case "SHL":
                    Algorithms.SHL(command);
                    break;
                case "SAR":
                    Algorithms.SAR(command);
                    break;
                case "SHR":
                    Algorithms.SHR(command);
                    break;

                //BRANCH
                case "JA":
                case "JNBE":
                    Algorithms.JA(command);
                    break;
                case "JAE":
                case "JNB":
                case "JNC":
                    Algorithms.JAE(command);
                    break;
                case "JB":
                case "JNAE":
                case "JC":
                    Algorithms.JB(command);
                    break;
                case "JBE":
                case "JNA":
                    Algorithms.JBE(command);
                    break;
                case "JCXZ":
                    Algorithms.JCXZ(command);
                    break;
                case "JE":
                case "JZ":
                    Algorithms.JE(command);
                    break;
                case "JG":
                case "JNLE":
                    Algorithms.JG(command);
                    break;
                case "JGE":
                case "JNL":
                    Algorithms.JGE(command);
                    break;
                case "JL":
                case "JNGE":
                    Algorithms.JL(command);
                    break;
                case "JLE":
                case "JNG":
                    Algorithms.JLE(command);
                    break;
                case "JMP":
                    Algorithms.JMP(command);
                    break;
                case "CALL": //TODO MUST
                    throw new NotImplementedException("This instruction is not implemented");
                case "RET":
                    if (Storage.AutoRun) Storage.ContinueSimulation = false;
                    break;
                case "IRET": //TODO MUST
                    throw new NotImplementedException("This instruction is not implemented");
                case "INT": //TODO MUST
                    throw new NotImplementedException("This instruction is not implemented");
                case "INTO":
                    throw new NotImplementedException("This instruction is not implemented");
                case "LOOP":
                    Algorithms.LOOP(command);
                    break;
                case "LOOPZ":
                case "LOOPE":
                    Algorithms.LOOPZ(command);
                    break;
                case "LOOPNZ":
                case "LOOPNE":
                    Algorithms.LOOPNZ(command);
                    break;

                //FLAGS
                case "CLC":
                    Algorithms.CLC(command);
                    break;
                case "CLD":
                    Algorithms.CLD(command);
                    break;
                case "CLI":
                    Algorithms.CLI(command);
                    break;
                case "CMC":
                    Algorithms.CMC(command);
                    break;
                case "STC":
                    Algorithms.STC(command);
                    break;
                case "STD":
                    Algorithms.STD(command);
                    break;
                case "STI":
                    Algorithms.STI(command);
                    break;

                //CONTROL
                case "HLT":
                    if (Storage.AutoRun) Storage.ContinueSimulation = false;
                    break;
                case "NOP":
                    throw new NotImplementedException("This instruction is not implemented");
                case "ESC":
                    throw new NotImplementedException("This instruction is not implemented");
                case "WAIT":
                    throw new NotImplementedException("This instruction is not implemented");
                case "LOCK":
                    throw new NotImplementedException("This instruction is not implemented");

                //STRING
                case "MOVS":
                case "MOVSB":
                case "MOVSW":
                    throw new NotImplementedException("This instruction is not implemented");
                case "CMPS":
                case "CMPSB":
                case "CMPSW":
                    throw new NotImplementedException("This instruction is not implemented");
                case "SCAS":
                case "SCASB":
                case "SCASW":
                    throw new NotImplementedException("This instruction is not implemented");
                case "LODS":
                case "LODSB":
                case "LODSW":
                    throw new NotImplementedException("This instruction is not implemented");
                case "STOS":
                case "STOSB":
                case "STOSW":
                    throw new NotImplementedException("This instruction is not implemented");
                case "REP":
                    throw new NotImplementedException("This instruction is not implemented");
                case "REPE":
                case "REPZ":
                    throw new NotImplementedException("This instruction is not implemented");
                case "REPNE":
                case "REPNZ":
                    throw new NotImplementedException("This instruction is not implemented");

                //META
                case "QUIT":
                    Storage.ContinueSimulation = false;
                    Storage.DoNotSaveToCode = true;
                    break;
                case "FLAGS":
                    Storage.FlagsDisplay();
                    Storage.DoNotSaveToCode = true;
                    break;
                case "REGISTER":
                    Storage.RegisterDisplay();
                    Storage.DoNotSaveToCode = true;
                    break;
                case "SEGMENTS":
                    Storage.SegmentsDisplay();
                    Storage.DoNotSaveToCode = true;
                    break;
                case "POINTERS":
                    Storage.PointersDisplay();
                    Storage.DoNotSaveToCode = true;
                    break;
                case "MEMORY":
                    Storage.MemoryDisplay();
                    Storage.DoNotSaveToCode = true;
                    break;
                case "STACK":
                    Storage.StackDisplay();
                    Storage.DoNotSaveToCode = true;
                    break;
                case "PORTS":
                    Storage.PortDisplay();
                    Storage.DoNotSaveToCode = true;
                    break;
                case "CODE":
                    Storage.CodeDisplay();
                    Storage.DoNotSaveToCode = true;
                    break;
                case "DROP":
                    Storage.CodeDump();
                    Storage.DoNotSaveToCode = true;
                    break;
                case "STORAGE":
                    Tools.StorageDisplay();
                    Storage.DoNotSaveToCode = true;
                    break;
                case "DUMP":
                    Tools.StorageDump();
                    Storage.DoNotSaveToCode = true;
                    break;
                case "DEBUG":
                    Algorithms.DEBUG();
                    Storage.DoNotSaveToCode = true;
                    break;
                case "CLEAR":
                case "CLS":
                case "CLEANSE":
                    Console.Clear();
                    Storage.DoNotSaveToCode = true;
                    break;
                case "SAVE":
                    Algorithms.SAVE(command);
                    Storage.DoNotSaveToCode = true;
                    break;
                case "LOAD":
                    Algorithms.LOAD(command);
                    Storage.DoNotSaveToCode = true;
                    break;
                case "RUN":
                    Algorithms.RUN(command);
                    Storage.DoNotSaveToCode = true;
                    break;
                case "AUTHOR":
                    Algorithms.AUTHOR();
                    Storage.DoNotSaveToCode = true;
                    break;

                //ERROR
                default:
                    throw new Exception($"Command '{instruction}' was not recognized - make sure there are no typos, champ");
            }
        }
    }
}