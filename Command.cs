namespace System
{
    class Command
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

            switch (commandArray[0].ToUpper())
            {
                //DATA TRANSFER
                case "MOV": //TODO

                    break;
                case "LDS": //TODO

                    break;
                case "LES": //TODO

                    break;
                case "LEA": //TODO

                    break;
                case "LAHF": //TODO

                    break;
                case "SAHF": //TODO

                    break;
                case "XLAT": //TODO
                case "XLATB": //TODO

                    break;
                case "XCHG": //TODO

                    break;
                case "PUSH": //TODO

                    break;
                case "POP": //TODO

                    break;
                case "POPF": //TODO

                    break;
                case "IN": //TODO

                    break;
                case "OUT": //TODO

                    break;

                //ARITHMETIC
                case "ADD": //TODO

                    break;
                case "ADC": //TODO

                    break;
                case "SUB": //TODO

                    break;
                case "SBB": //TODO

                    break;
                case "MUL": //TODO

                    break;
                case "IMUL": //TODO

                    break;
                case "DIV": //TODO

                    break;
                case "IDIV": //TODO

                    break;
                case "INC": //TODO

                    break;
                case "DEC": //TODO

                    break;
                case "DAA": //TODO

                    break;
                case "DAS": //TODO

                    break;
                case "AAA": //TODO

                    break;
                case "AAD": //TODO

                    break;
                case "AAM": //TODO

                    break;
                case "AAS": //TODO

                    break;
                case "CBW": //TODO

                    break;
                case "CWD": //TODO

                    break;
                case "NEG": //TODO

                    break;
                case "CMP": //TODO

                    break;

                //LOGICAL
                case "AND": //TODO

                    break;
                case "OR": //TODO

                    break;
                case "XOR": //TODO

                    break;
                case "NOT": //TODO

                    break;
                case "TEST": //TODO

                    break;

                //ROTATE
                case "RCL": //TODO

                    break;
                case "RCR": //TODO

                    break;
                case "ROL": //TODO

                    break;
                case "ROR": //TODO

                    break;

                //SHIFT
                case "SAL": //TODO
                case "SHL": //TODO

                    break;
                case "SAR": //TODO

                    break;
                case "SHR": //TODO

                    break;

                //BRANCH
                case "JA": //TODO
                case "JNBE": //TODO

                    break;
                case "JAE": //TODO
                case "JNB": //TODO
                case "JNC": //TODO

                    break;
                case "JB": //TODO
                case "JNAE": //TODO
                case "JC": //TODO

                    break;
                case "JBE": //TODO
                case "JNA": //TODO

                    break;
                case "JCXZ": //TODO

                    break;
                case "JE": //TODO
                case "JZ": //TODO

                    break;
                case "JG": //TODO
                case "JNLE": //TODO

                    break;
                case "JGE": //TODO
                case "JNL": //TODO

                    break;
                case "JL": //TODO
                case "JNGE": //TODO

                    break;
                case "JLE": //TODO
                case "JNG": //TODO

                    break;
                case "JMP": //TODO

                    break;
                case "CALL": //TODO

                    break;
                case "RET": //TODO

                    break;
                case "IRET": //TODO

                    break;
                case "INT": //TODO

                    break;
                case "INTO": //TODO

                    break;
                case "LOOP": //TODO

                    break;
                case "LOOPZ": //TODO
                case "LOOPE": //TODO

                    break;
                case "LOOPNZ": //TODO
                case "LOOPNE": //TODO

                    break;

                //FLAGS
                case "CLC": //TODO

                    break;
                case "CLD": //TODO

                    break;
                case "CLI": //TODO

                    break;
                case "CMC": //TODO

                    break;
                case "STC": //TODO

                    break;
                case "STD": //TODO

                    break;
                case "STI": //TODO

                    break;
                case "HLT": //TODO

                    break;
                case "NOP": //TODO

                    break;
                case "ESC": //TODO

                    break;
                case "WAIT": //TODO

                    break;
                case "LOCK": //TODO

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
                case "~QUIT":
                    Console.WriteLine("Exiting...");
                    break;

                //ERROR
                default:
                    Console.WriteLine($"Command '{commandArray[0]}' was not recognized - make sure there are no typos and try again");
                    break;
            }
        }
    }

}