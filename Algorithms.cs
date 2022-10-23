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
            string[] operands = command.Split(',');
            string operandTypes = "";
            for (int i = 0; i < operands.Length; i++)
            {
                operands[i] = operands[i].Trim();
                operandTypes += $"{Tools.DetectOperandType(operands[i])} ";
            }
            Console.WriteLine($"MOV operands: {operandTypes}");



            // MOV REG, memory
            // MOV REG, REG
            // MOV REG, immediate
            // MOV REG, SREG
            // MOV memory, REG
            // MOV memory, immediate
            // MOV memory, SREG
            // MOV SREG, memory
            // MOV SREG, REG

        }
    }
}