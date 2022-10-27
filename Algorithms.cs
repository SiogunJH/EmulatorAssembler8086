namespace System
{
    class Algorithms
    {
        public static void MOV(string command)
        {
            //Check for number of operands
            if (!Tools.CheckForNumOfOperands(command, 2))
                return;

            //Prepare operands
            command = command.Substring(3);
            string[] operand = command.Split(',');
            for (int i = 0; i < operand.Length; i++)
                operand[i] = operand[i].Trim();

            //Detect operand types
            string[] operandType = new string[operand.Length];
            for (int i = 0; i < operandType.Length; i++)
                operandType[i] = Tools.DetectOperandType(operand[i]);

            //Check for incorrect operands
            if (Array.IndexOf(operandType, "error") != -1)
                return;

            //Check if operation is allowed
            if (
                //NO WRITING TO NUMBER OR FLAG
                ("numberD;numberB;numberH;numberQ;flag".Contains(operandType[0])) ||
                //NO WRITING FROM FLAG
                ("flag".Contains(operandType[1])) ||
                //NO WRITING FROM REGISTER TO REGISTER H+L AND VICE VERSA
                ("register".Contains(operandType[0]) && "registerX".Contains(operandType[1])) ||
                ("registerX".Contains(operandType[0]) && "register".Contains(operandType[1])) ||
                //NO WRITING FROM MEMORY TO MEMORY
                ("memory".Contains(operandType[0]) && "memory".Contains(operandType[1])) ||
                //NO WRITING FROM NUMBER, SEGMENT, FLAG OR REGISTER H/L TO SEGMENT
                ("segment".Contains(operandType[0]) && "segment;numberD;numberB;numberH;numberQ;register".Contains(operandType[1]))
            )
            {
                Console.WriteLine($"Illegal operation: Cannot write '{operandType[1]}' to '{operandType[0]}'");
                return;
            }

            //Read operand2 value
            int operandValue = Tools.ReadDataFromOperand(operand[1], operandType[1]);
            if (operandValue == -1)
                return;

            //Write operand1 value
            bool result = Tools.WriteDataToOperand(operand[0], operandType[0], operandValue);
            return;
        }
    }
}