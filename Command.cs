namespace System
{
    public class Command
    {
        public static void Recognize(string command)
        {
            //Prepare command for analyze
            command = command.Split(";")[0]; //ignore comment
            command = command.Trim();

            //No response nor action
            if (command == "" || command == null)
                return;

            //Detect command
            string[] commandArray = command.Split(" ", StringSplitOptions.RemoveEmptyEntries); // commandArray[] = {Instruction, Operand1, Operand2}

            switch (commandArray[0])
            {
                //DATA TRANSFER
                case "MOV":
                    Algorithms.MOV(command);
                    break;
                case "LDS": //TODO

                    break;
                case "LES": //TODO

                    break;
                case "LEA": //TODO MUST

                    break;
                case "LAHF": //TODO MUST

                    break;
                case "SAHF": //TODO MUST

                    break;
                case "XLAT": //TODO
                case "XLATB": //TODO

                    break;
                case "XCHG":
                    Algorithms.XCHG(command);
                    break;
                case "PUSH": //TODO MUST

                    break;
                case "PUSHF": //TODO MUST

                    break;
                case "POP": //TODO MUST

                    break;
                case "POPF": //TODO MUST

                    break;
                case "IN": //TODO MUST

                    break;
                case "OUT": //TODO MUST

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
                case "IMUL": //TODO MUST

                    break;
                case "DIV":
                    Algorithms.DIV(command);
                    break;
                case "IDIV": //TODO MUST

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
                case "CMP": //TODO

                    break;

                //LOGICAL
                case "AND": //TODO MUST

                    break;
                case "OR": //TODO MUST

                    break;
                case "XOR": //TODO MUST

                    break;
                case "NOT": //TODO MUST
                    Algorithms.NOT(command);

                    break;
                case "TEST": //TODO MUST

                    break;

                //ROTATE
                case "RCL": //TODO MUST

                    break;
                case "RCR": //TODO MUST

                    break;
                case "ROL": //TODO MUST

                    break;
                case "ROR": //TODO MUST

                    break;

                //SHIFT
                case "SAL": //TODO MUST
                case "SHL": //TODO MUST

                    break;
                case "SAR": //TODO MUST

                    break;
                case "SHR": //TODO MUST

                    break;

                //BRANCH
                case "JA": //TODO MUST
                case "JNBE": //TODO MUST

                    break;
                case "JAE": //TODO MUST
                case "JNB": //TODO MUST
                case "JNC": //TODO MUST

                    break;
                case "JB": //TODO MUST
                case "JNAE": //TODO MUST
                case "JC": //TODO MUST

                    break;
                case "JBE": //TODO MUST
                case "JNA": //TODO MUST

                    break;
                case "JCXZ": //TODO MUST

                    break;
                case "JE": //TODO MUST
                case "JZ": //TODO MUST

                    break;
                case "JG": //TODO MUST
                case "JNLE": //TODO MUST

                    break;
                case "JGE": //TODO MUST
                case "JNL": //TODO MUST

                    break;
                case "JL": //TODO MUST
                case "JNGE": //TODO MUST

                    break;
                case "JLE": //TODO MUST
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
                case "INTO": //TODO

                    break;
                case "LOOP": //TODO MUST

                    break;
                case "LOOPZ": //TODO MUST
                case "LOOPE": //TODO MUST

                    break;
                case "LOOPNZ": //TODO MUST
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
                case "MOVS": //TODO
                case "MOVSB": //TODO
                case "MOVSW": //TODO

                    break;
                case "CMPS": //TODO
                case "CMPSB": //TODO
                case "CMPSW": //TODO

                    break;
                case "SCAS": //TODO
                case "SCASB": //TODO
                case "SCASW": //TODO

                    break;
                case "LODS": //TODO
                case "LODSB": //TODO
                case "LODSW": //TODO

                    break;
                case "STOS": //TODO
                case "STOSB": //TODO
                case "STOSW": //TODO

                    break;
                case "REP": //TODO

                    break;
                case "REPE": //TODO
                case "REPZ": //TODO

                    break;
                case "REPNE": //TODO
                case "REPNZ": //TODO

                    break;

                //META
                case "QUIT":
                    Storage.ContinueSimulation = false;
                    break;
                case "FLAGS":
                    Storage.FlagsDisplay();
                    break;
                case "REGISTER":
                    Storage.RegisterDisplay();
                    break;
                case "SEGMENTS":
                    Storage.SegmentsDisplay();
                    break;
                case "POINTERS":
                    Storage.PointersDisplay();
                    break;
                case "MEMORY":
                    Storage.MemoryDisplay();
                    break;
                case "STORAGE":
                    Tools.StorageDisplay();
                    break;
                case "DEBUG":
                    Algorithms.DEBUG();
                    break;
                case "CLEAR":
                case "CLS":
                case "CLEANSE":
                    Console.Clear();
                    break;

                //ERROR
                default:
                    throw new Exception($"Command '{commandArray[0]}' was not recognized - make sure there are no typos, champ");
            }
        }
    }

}