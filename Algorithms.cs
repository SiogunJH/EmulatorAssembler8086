namespace System
{
    class Algorithms
    {
        //Adds [operand 2] to [operand 1] and save to [operand 1]
        //Add 1 extra, if [Carry Flag] is 1
        //Update [Parity Flag] afterwards
        public static void ADC(string command)
        {
            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 2);

            //Prepare operands
            command = command.Substring(command.Split(' ')[0].Length);
            string[] operand = command.Split(',');
            for (int i = 0; i < operand.Length; i++)
                operand[i] = operand[i].Trim();

            //Detect operand types
            string[] operandType = new string[operand.Length];
            for (int i = 0; i < operandType.Length; i++)
                operandType[i] = Tools.DetectOperandType(operand[i]);

            //Read operand1 and operand2 value
            int[] operandValue = new int[2];
            operandValue[0] = Tools.ReadDataFromOperand(operand[0], operandType[0]);
            operandValue[1] = Tools.ReadDataFromOperand(operand[1], operandType[1]);

            //Write operand1 value
            int valueToWrite = operandValue[0] + operandValue[1] + Storage.Flags["CF"];
            Tools.WriteDataToOperand(operand[0], operandType[0], valueToWrite);

            //Modify flags
            Tools.UpdateParityFlag(valueToWrite);
        }

        //Adds [operand 2] to [operand 1] and save to [operand 1]
        //Update [Parity Flag] afterwards
        public static void ADD(string command)
        {
            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 2);

            //Prepare operands
            command = command.Substring(command.Split(' ')[0].Length);
            string[] operand = command.Split(',');
            for (int i = 0; i < operand.Length; i++)
                operand[i] = operand[i].Trim();

            //Detect operand types
            string[] operandType = new string[operand.Length];
            for (int i = 0; i < operandType.Length; i++)
                operandType[i] = Tools.DetectOperandType(operand[i]);

            //Read operand1 and operand2 value
            int[] operandValue = new int[2];
            operandValue[0] = Tools.ReadDataFromOperand(operand[0], operandType[0]);
            operandValue[1] = Tools.ReadDataFromOperand(operand[1], operandType[1]);

            //Write operand1 value
            int valueToWrite = operandValue[0] + operandValue[1];
            Tools.WriteDataToOperand(operand[0], operandType[0], valueToWrite);

            //Modify flags
            Tools.UpdateParityFlag(valueToWrite);
        }

        //Moves (copies) data from [operand 2] to [operand 1]
        public static void MOV(string command)
        {
            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 2);

            //Prepare operands
            command = command.Substring(command.Split(' ')[0].Length);
            string[] operand = command.Split(',');
            for (int i = 0; i < operand.Length; i++)
                operand[i] = operand[i].Trim();

            //Detect operand types
            string[] operandType = new string[operand.Length];
            for (int i = 0; i < operandType.Length; i++)
                operandType[i] = Tools.DetectOperandType(operand[i]);

            //Read operand2 value
            int operandValue = Tools.ReadDataFromOperand(operand[1], operandType[1]);

            //Adjust value to write
            int valueToWrite = operandValue;

            //Write operand1 value
            Tools.WriteDataToOperand(operand[0], operandType[0], valueToWrite);

            //Modify flags
        }

        //Multiply [AL] by an [operand 1] and save to [AX]
        //If [AH] is empty afterwards, set [Overflow Flag] and [Carry Flag] to 0 - otherwise, set both to 1
        //Update [Parity Flag] afterwards
        public static void MUL(string command)
        {
            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 1);

            //Prepare operands
            command = command.Substring(command.Split(' ')[0].Length);
            string[] operand = command.Split(',');
            for (int i = 0; i < operand.Length; i++)
                operand[i] = operand[i].Trim();

            //Detect operand types
            string[] operandType = new string[operand.Length];
            for (int i = 0; i < operandType.Length; i++)
                operandType[i] = Tools.DetectOperandType(operand[i]);

            //Read operand1 value
            int operandValue = Tools.ReadDataFromOperand(operand[0], operandType[0]);


            //Write results value
            int valueToWrite = operandValue * Storage.Register["AL"];
            Tools.WriteDataToOperand("AX", "registerX", valueToWrite);

            //Modify flags
            Tools.UpdateParityFlag(valueToWrite); //PF
            if (Storage.Register["AH"] == 0) //OF i CF
            {
                Storage.Flags["OF"] = 0;
                Storage.Flags["CF"] = 0;
            }
            else
            {
                Storage.Flags["OF"] = 1;
                Storage.Flags["CF"] = 1;
            }
        }

        //Substract [operand 2] from [operand 1] and save to [operand 1]
        //Substract 1 extra, if [Carry Flag] is 1
        //Update [Parity Flag] afterwards
        public static void SBB(string command)
        {
            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 2);

            //Prepare operands
            command = command.Substring(command.Split(' ')[0].Length);
            string[] operand = command.Split(',');
            for (int i = 0; i < operand.Length; i++)
                operand[i] = operand[i].Trim();

            //Detect operand types
            string[] operandType = new string[operand.Length];
            for (int i = 0; i < operandType.Length; i++)
                operandType[i] = Tools.DetectOperandType(operand[i]);



            //Read operand1 and operand2 value
            int[] operandValue = new int[2];
            operandValue[0] = Tools.ReadDataFromOperand(operand[0], operandType[0]);
            operandValue[1] = Tools.ReadDataFromOperand(operand[1], operandType[1]);


            //Write operand1 value
            int valueToWrite = operandValue[0] - operandValue[1] - Storage.Flags["CF"];
            Tools.WriteDataToOperand(operand[0], operandType[0], valueToWrite);

            //Modify flags
            Tools.UpdateParityFlag(valueToWrite);
        }

        //Substract [operand 2] from [operand 1] and save to [operand 1]
        //Update [Parity Flag] afterwards
        public static void SUB(string command)
        {
            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 2);

            //Prepare operands
            command = command.Substring(command.Split(' ')[0].Length);
            string[] operand = command.Split(',');
            for (int i = 0; i < operand.Length; i++)
                operand[i] = operand[i].Trim();

            //Detect operand types
            string[] operandType = new string[operand.Length];
            for (int i = 0; i < operandType.Length; i++)
                operandType[i] = Tools.DetectOperandType(operand[i]);



            //Read operand1 and operand2 value
            int[] operandValue = new int[2];
            operandValue[0] = Tools.ReadDataFromOperand(operand[0], operandType[0]);
            operandValue[1] = Tools.ReadDataFromOperand(operand[1], operandType[1]);


            //Write operand1 value
            int valueToWrite = operandValue[0] - operandValue[1];
            Tools.WriteDataToOperand(operand[0], operandType[0], valueToWrite);

            //Modify flags
            Tools.UpdateParityFlag(valueToWrite);
        }
    }
}