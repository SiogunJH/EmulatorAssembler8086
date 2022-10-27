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
            if (!(
                (operandType[0] == "register" && "memory;register;numberB,numberQ;numberD;numberH".Contains(operandType[1])) ||
                (operandType[0] == "registerX" && "memory;registerX;pointer;numberB,numberQ;numberD;numberH".Contains(operandType[1])) ||
                (operandType[0] == "pointer" && "memory;registerX;pointer;numberB,numberQ;numberD;numberH".Contains(operandType[1])) ||
                (operandType[0] == "memory" && "register;registerX;pointer;numberB,numberQ;numberD;numberH".Contains(operandType[1]))
            ))
            {
                Console.WriteLine($"Illegal operation: Cannot add '{operandType[1]}' to '{operandType[0]}'");
                return;
            }

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
            if (!(
                (operandType[0] == "register" && "memory;register;numberB,numberQ;numberD;numberH".Contains(operandType[1])) ||
                (operandType[0] == "registerX" && "memory;registerX;pointer;numberB,numberQ;numberD;numberH".Contains(operandType[1])) ||
                (operandType[0] == "pointer" && "memory;registerX;pointer;numberB,numberQ;numberD;numberH".Contains(operandType[1])) ||
                (operandType[0] == "memory" && "register;registerX;pointer;numberB,numberQ;numberD;numberH".Contains(operandType[1]))
            ))
            {
                Console.WriteLine($"Illegal operation: Cannot add '{operandType[1]}' to '{operandType[0]}'");
                return;
            }

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
            if (!(
                (operandType[0] == "register" && "memory;register;pointer;numberB,numberQ;numberD;numberH".Contains(operandType[1])) ||
                (operandType[0] == "registerX" && "memory;registerX;pointer;numberB,numberQ;numberD;numberH;segment".Contains(operandType[1])) ||
                (operandType[0] == "pointer" && "memory;registerX;pointer;numberB,numberQ;numberD;numberH;segment".Contains(operandType[1])) ||
                (operandType[0] == "memory" && "register;registerX;pointer;numberB,numberQ;numberD;numberH;segment".Contains(operandType[1])) ||
                (operandType[0] == "segment" && "memory;registerX;pointer".Contains(operandType[1]))
            ))
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