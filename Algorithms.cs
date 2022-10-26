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
            //TODO
            //VERIFY: SYNTAX OF MEMORY
            //SREG: DS, ES, SS, and only as second operand: CS.
            //REG: AX = AHAL
            // MOV REG, memory
            // MOV REG, REG
            // MOV REG, immediate
            // MOV REG, SREG
            // MOV memory, REG
            // MOV memory, immediate
            // MOV memory, SREG
            // MOV SREG, memory
            // MOV SREG, REG

            //Read operand2 value
            int operandValue = Tools.ReadDataFromOperand(operand[1], operandType[1]);
            if (operandValue == -1)
                return;

            //Write operand1 value
            //TODO: USE BOOL FOR FILE VERIFICATION
            bool result = Tools.WriteDataToOperand(operand[0], operandType[0], operandValue);
            return;
        }
    }
}