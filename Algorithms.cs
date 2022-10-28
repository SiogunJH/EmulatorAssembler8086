namespace System
{
    class Algorithms
    {
        //Adds specified operands and the carry status
        public static void ADC(string command)
        {
            //Check for number of operands
            if (!Tools.CheckForNumOfOperands(command, 2))
                return;

            //Prepare operands
            command = command.Substring(command.Split(' ')[0].Length);
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

            //Read operand1 and operand2 value
            int[] operandValue = new int[2];
            operandValue[0] = Tools.ReadDataFromOperand(operand[0], operandType[0]);
            operandValue[1] = Tools.ReadDataFromOperand(operand[1], operandType[1]);
            if (Array.IndexOf(operandValue, -1) != -1)
                return;

            //Write operand1 value
            bool result = Tools.WriteDataToOperand(operand[0], operandType[0], operandValue[0] + operandValue[1] + Storage.Flags["CF"]);
            return;
        }

        //Adds specified operands
        public static void ADD(string command)
        {
            //Check for number of operands
            if (!Tools.CheckForNumOfOperands(command, 2))
                return;

            //Prepare operands
            command = command.Substring(command.Split(' ')[0].Length);
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

            //Read operand1 and operand2 value
            int[] operandValue = new int[2];
            operandValue[0] = Tools.ReadDataFromOperand(operand[0], operandType[0]);
            operandValue[1] = Tools.ReadDataFromOperand(operand[1], operandType[1]);
            if (Array.IndexOf(operandValue, -1) != -1)
                return;

            //Write operand1 value
            bool result = Tools.WriteDataToOperand(operand[0], operandType[0], operandValue[0] + operandValue[1]);
            return;
        }

        //Moves data from register to register, register to memory, memory to register, memory to accumulator, accumulator to memory, etc.
        public static void MOV(string command)
        {
            //Check for number of operands
            if (!Tools.CheckForNumOfOperands(command, 2))
                return;

            //Prepare operands
            command = command.Substring(command.Split(' ')[0].Length);
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

            //Read operand2 value
            int operandValue = Tools.ReadDataFromOperand(operand[1], operandType[1]);
            if (operandValue == -1)
                return;

            //Write operand1 value
            bool result = Tools.WriteDataToOperand(operand[0], operandType[0], operandValue);
            return;
        }

        //Multiplies AL by an operand, and saves the result in AX; if AH is empty afterwards, set OF and CF to 0; if not, set OF and CF to 1
        public static void MUL(string command)
        {
            //Check for number of operands
            if (!Tools.CheckForNumOfOperands(command, 1))
                return;

            //Prepare operands
            command = command.Substring(command.Split(' ')[0].Length);
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

            //Read operand1 value
            int operandValue = Tools.ReadDataFromOperand(operand[0], operandType[0]);
            if (operandValue == -1)
                return;

            //Write results value
            bool result = Tools.WriteDataToOperand("AX", "registerX", operandValue * Storage.Register["AL"]);

            //Modify flags
            if (Storage.Register["AH"] == 0)
            {
                Storage.Flags["OF"] = 0;
                Storage.Flags["CF"] = 0;
            }
            else
            {
                Storage.Flags["OF"] = 1;
                Storage.Flags["CF"] = 1;
            }
            return;
        }

        //Substract specified operands and substract one extra if the carry flag is up
        public static void SBB(string command)
        {
            //Check for number of operands
            if (!Tools.CheckForNumOfOperands(command, 2))
                return;

            //Prepare operands
            command = command.Substring(command.Split(' ')[0].Length);
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

            //Read operand1 and operand2 value
            int[] operandValue = new int[2];
            operandValue[0] = Tools.ReadDataFromOperand(operand[0], operandType[0]);
            operandValue[1] = Tools.ReadDataFromOperand(operand[1], operandType[1]);
            if (Array.IndexOf(operandValue, -1) != -1)
                return;

            //Write operand1 value
            bool result = Tools.WriteDataToOperand(operand[0], operandType[0], operandValue[0] - operandValue[1] - Storage.Flags["CF"]);
            return;
        }

        //Adds specified operands and the carry status
        public static void SUB(string command)
        {
            //Check for number of operands
            if (!Tools.CheckForNumOfOperands(command, 2))
                return;

            //Prepare operands
            command = command.Substring(command.Split(' ')[0].Length);
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

            //Read operand1 and operand2 value
            int[] operandValue = new int[2];
            operandValue[0] = Tools.ReadDataFromOperand(operand[0], operandType[0]);
            operandValue[1] = Tools.ReadDataFromOperand(operand[1], operandType[1]);
            if (Array.IndexOf(operandValue, -1) != -1)
                return;

            //Write operand1 value
            bool result = Tools.WriteDataToOperand(operand[0], operandType[0], operandValue[0] - operandValue[1]);
            return;
        }
    }
}