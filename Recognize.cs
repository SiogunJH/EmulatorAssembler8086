namespace System
{
    public class Recognize
    {
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
            if (command.Split(' ').Length == 1 && command.EndsWith(':'))
            {
                if (Storage.DebugMode) Console.WriteLine("LABEL DETECTED");
                return;
            }

            //Detect and execute a command
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

            //Add instruction to saved code
            if (!Storage.DoNotSaveToCode)
                Storage.SavedCode.Add(command);
        }

        public static void Command(string instruction, string command)
        {
            switch (instruction)
            {
                //DATA TRANSFER
                case "MOV":
                    Algorithms.MOV(command);
                    break;
                case "LDS":
                    Tools.NotImplemented();
                    break;
                case "LES":
                    Tools.NotImplemented();
                    break;
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
                    Tools.NotImplemented();
                    break;
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
                    Tools.NotImplemented();
                    break;

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
                case "TEST": //TODO MUST

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
                case "JNBE": //TODO MUST

                    break;
                case "JAE":
                case "JNB":
                case "JNC": //TODO MUST

                    break;
                case "JB":
                case "JNAE":
                case "JC": //TODO MUST

                    break;
                case "JBE":
                case "JNA": //TODO MUST

                    break;
                case "JCXZ": //TODO MUST

                    break;
                case "JE":
                case "JZ": //TODO MUST

                    break;
                case "JG":
                case "JNLE": //TODO MUST

                    break;
                case "JGE":
                case "JNL": //TODO MUST

                    break;
                case "JL":
                case "JNGE": //TODO MUST

                    break;
                case "JLE":
                case "JNG": //TODO MUST

                    break;
                case "JMP": //TODO MUST

                    break;
                case "CALL": //TODO MUST

                    break;
                case "RET": //TODO MUST

                    break;
                case "IRET": //TODO MUST

                    break;
                case "INT": //TODO MUST

                    break;
                case "INTO":
                    Tools.NotImplemented();
                    break;
                case "LOOP": //TODO MUST

                    break;
                case "LOOPZ":
                case "LOOPE": //TODO MUST

                    break;
                case "LOOPNZ":
                case "LOOPNE": //TODO MUST

                    break;

                //FLAGS
                case "CLC":
                    //This instruction resets the carry flag CF to 0.
                    Storage.Flags["CF"] = 0;
                    break;
                case "CLD":
                    //This instruction resets the direction flag DF to 0.
                    Storage.Flags["DF"] = 0;
                    break;
                case "CLI":
                    //This instruction resets the interrupt flag IF to 0.
                    Storage.Flags["IF"] = 0;
                    break;
                case "CMC":
                    //Inverts value of CF.
                    if (Storage.Flags["CF"] == 1)
                    {
                        //Console.WriteLine("CF: 1->0");
                        Storage.Flags["CF"] = 0;
                    }
                    else
                    {
                        //Console.WriteLine("CF: 0->1");
                        Storage.Flags["CF"] = 1;
                    }
                    break;
                case "STC":
                    //Set carry flag CF to 1.
                    Storage.Flags["CF"] = 1;
                    break;
                case "STD":
                    //Set direction flag DF to 1.
                    Storage.Flags["DF"] = 1;
                    break;
                case "STI":
                    //Set interrupt flag IF to 1.
                    Storage.Flags["IF"] = 1;
                    break;

                //CONTROL
                case "HLT":
                    //Halt processing. It stops program execution
                    Storage.ContinueSimulation = false;
                    break;
                case "NOP":
                    //Do nothing for one tick
                    ;
                    break;
                case "ESC": //VERIFY
                    //Provides access to the data bus for other resident processors. The CPU treats it as a NOP but places memory operand on bus.
                    ;
                    break;
                case "WAIT": //VERIFY
                    //Just... waits? I guess?
                    ;
                    break;
                case "LOCK": //VERIFY
                    //I dunno
                    break;

                //STRING
                case "MOVS":
                case "MOVSB":
                case "MOVSW":
                    Tools.NotImplemented();
                    break;
                case "CMPS":
                case "CMPSB":
                case "CMPSW":
                    Tools.NotImplemented();
                    break;
                case "SCAS":
                case "SCASB":
                case "SCASW":
                    Tools.NotImplemented();
                    break;
                case "LODS":
                case "LODSB":
                case "LODSW":
                    Tools.NotImplemented();
                    break;
                case "STOS":
                case "STOSB":
                case "STOSW":
                    Tools.NotImplemented();
                    break;
                case "REP":
                    Tools.NotImplemented();
                    break;
                case "REPE":
                case "REPZ":
                    Tools.NotImplemented();
                    break;
                case "REPNE":
                case "REPNZ":
                    Tools.NotImplemented();
                    break;

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
                case "STORAGE":
                    Tools.StorageDisplay();
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

                //ERROR
                default:
                    throw new Exception($"Command '{instruction}' was not recognized - make sure there are no typos, champ");
            }
        }
    }
}