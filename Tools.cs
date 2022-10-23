namespace System
{
    class Tools
    {
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