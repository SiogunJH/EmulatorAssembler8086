namespace System
{
    class Algorithms
    {
        //Correct the result of addition of two ASCII values
        //If lower nibble of [AL] > 9 or [AF] is up, subtract 6h to [AL] and set [AF] to 1
        public static void AAA(string command)
        {
            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 0);

            //Read and test AL value
            int valueToWriteAL = Storage.Register["AL"];
            int valueToWriteAH = Storage.Register["AH"];
            if (valueToWriteAL % 16 > 9) //lower nibble
                Storage.Flags["AF"] = 1;

            //Adjust AL and AH values
            if (Storage.Flags["AF"] == 1)
            {
                valueToWriteAL += 6;
                valueToWriteAH += 1;
                Storage.Flags["CF"] = 1;
            }
            else
            {
                Storage.Flags["CF"] = 0;
            }

            //Determine and adjust final value(s)
            valueToWriteAL = Tools.AdjustValue(valueToWriteAL, "register");
            valueToWriteAH = Tools.AdjustValue(valueToWriteAH, "register");

            //Write value(s)
            Tools.WriteDataToOperand("AL", "register", valueToWriteAL);
            Tools.WriteDataToOperand("AH", "register", valueToWriteAH);

            //Modify flags
            Tools.UpdateParityFlag(valueToWriteAL); //For AAA, only AL is taken into account for PF
        }

        //Prepare the ASCII values of AL and AH to division
        //[AL] = [AH]*10 + [AL]
        public static void AAD(string command)
        {
            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 0);

            //Read value(s)
            int valueToWriteAL = Storage.Register["AL"];
            int valueToWriteAH = Storage.Register["AH"];

            //Update value(s)
            valueToWriteAL += valueToWriteAH * 10;
            valueToWriteAH = 0;

            //Adjust value(s)
            valueToWriteAL = Tools.AdjustValue(valueToWriteAL, "register");
            valueToWriteAH = Tools.AdjustValue(valueToWriteAH, "register");

            //Write value(s)
            Tools.WriteDataToOperand("AL", "register", valueToWriteAL);
            Tools.WriteDataToOperand("AH", "register", valueToWriteAH);

            //Modify flags
            Tools.UpdateParityFlag(valueToWriteAL); //For AAD, only AL is taken into account for PF
        }

        //Correct the result of multiplication of BCD values
        //[AL] = [AL] % 10
        //[AH] = [AL] / 10
        public static void AAM(string command)
        {
            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 0);

            //Read value(s)
            int valueToWriteAL = Storage.Register["AL"];
            int valueToWriteAH = Storage.Register["AH"];

            //Update value(s)
            valueToWriteAH = valueToWriteAL / 10;
            valueToWriteAL = valueToWriteAL % 10;

            //Adjust value(s)
            valueToWriteAL = Tools.AdjustValue(valueToWriteAL, "register");
            valueToWriteAH = Tools.AdjustValue(valueToWriteAH, "register");

            //Write value(s)
            Tools.WriteDataToOperand("AL", "register", valueToWriteAL);
            Tools.WriteDataToOperand("AH", "register", valueToWriteAH);

            //Modify flags
            Tools.UpdateParityFlag(valueToWriteAL); //For AAM, only AL is taken into account for PF
        }

        //Add [operand 2] to [operand 1] and save to [operand 1]
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

            //Determine and adjust final value(s)
            int valueToWrite = Tools.AdjustValue(operandValue[0] + operandValue[1] + Storage.Flags["CF"], operandType[0]);

            //Write operand1 value
            Tools.WriteDataToOperand(operand[0], operandType[0], valueToWrite);

            //Modify flags
            Tools.UpdateParityFlag(valueToWrite);
        }

        //Add [operand 2] to [operand 1] and save to [operand 1]
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

            //Determine and adjust final value(s)
            int valueToWrite = Tools.AdjustValue(operandValue[0] + operandValue[1], operandType[0]);

            //Write operand1 value
            Tools.WriteDataToOperand(operand[0], operandType[0], valueToWrite);

            //Modify flags
            Tools.UpdateParityFlag(valueToWrite);
        }

        //Correct the result of addition of two packed BCD values
        //IF higher nibble of [AL] > 9 or [CF] is up, add 60h to [AL] and set [CF] to 1
        //If lower nibble of [AL] > 9 or [AF] is up, add 6h to [AL] and set [AF] to 1
        public static void DAA(string command)
        {
            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 0);

            //Read and test AL value
            int valueToWrite = Storage.Register["AL"];
            if (valueToWrite / 16 > 9) //higher nibble
                Storage.Flags["CF"] = 1;
            if (valueToWrite % 16 > 9) //lower nibble
                Storage.Flags["AF"] = 1;

            //Adjust AL value
            if (Storage.Flags["CF"] == 1)
                valueToWrite += 96; // -=6*16
            if (Storage.Flags["AF"] == 1)
                valueToWrite += 6;

            //Determine and adjust final value(s)
            valueToWrite = Tools.AdjustValue(valueToWrite, "register");

            //Write operand1 value
            Tools.WriteDataToOperand("AL", "register", valueToWrite);

            //Modify flags
            Tools.UpdateParityFlag(valueToWrite);
        }

        //Correct the result of subtraction of two packed BCD values
        //IF higher nibble of [AL] > 9 or [CF] is up, subtract 60h to [AL] and set [CF] to 1
        //If lower nibble of [AL] > 9 or [AF] is up, subtract 6h to [AL] and set [AF] to 1
        public static void DAS(string command)
        {
            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 0);

            //Read and test AL value
            int valueToWrite = Storage.Register["AL"];
            if (valueToWrite / 16 > 9) //higher nibble
                Storage.Flags["CF"] = 1;
            if (valueToWrite % 16 > 9) //lower nibble
                Storage.Flags["AF"] = 1;

            //Adjust AL value
            if (Storage.Flags["CF"] == 1)
                valueToWrite -= 96; // -=6*16
            if (Storage.Flags["AF"] == 1)
                valueToWrite -= 6;

            //Determine and adjust final value(s)
            valueToWrite = Tools.AdjustValue(valueToWrite, "register");

            //Write operand1 value
            Tools.WriteDataToOperand("AL", "register", valueToWrite);

            //Modify flags
            Tools.UpdateParityFlag(valueToWrite);
        }

        //Decrement [operand 1] and save to [operand 1]
        public static void DEC(string command)
        {
            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 1);

            //Prepare operands
            string instruction = command.Split(' ')[0];
            command = command.Substring(command.Split(' ')[0].Length);
            string[] operand = command.Split(',');
            for (int i = 0; i < operand.Length; i++)
                operand[i] = operand[i].Trim();

            //Detect operand types
            string[] operandType = new string[operand.Length];
            for (int i = 0; i < operandType.Length; i++)
                operandType[i] = Tools.DetectOperandType(operand[i]);

            //Check if operation is not forbidden
            if (!"register;registerX;segment;pointer;memory".Contains(operandType[0])) throw new Exception($"Operand type for {instruction} instruction should only be 'register', 'registerX', 'pointer', 'segment' or 'memory' - recieved '{operandType[0]}'");

            //Read operand1 value
            int operandValue = Tools.ReadDataFromOperand(operand[0], operandType[0]);

            //Determine and adjust final value(s)
            int valueToWrite = Tools.AdjustValue(operandValue - 1, operandType[0]);

            //Write operand1 value
            Tools.WriteDataToOperand(operand[0], operandType[0], valueToWrite);

            //Modify flags
            Tools.UpdateParityFlag(valueToWrite);
        }

        //Divide [AX] by [operand 1]
        //[AL] or [AX] will contain results (/)
        //[AH] or [DX] will contain modulus (%)
        //Save location depends on wether diviser is of 'register' type (Small Division) or not (Big Division)
        public static void DIV(string command)
        {
            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 1);

            //Prepare operands
            string instruction = command.Split(' ')[0];
            command = command.Substring(command.Split(' ')[0].Length);
            string[] operand = command.Split(',');
            for (int i = 0; i < operand.Length; i++)
                operand[i] = operand[i].Trim();

            //Detect operand types
            string[] operandType = new string[operand.Length];
            for (int i = 0; i < operandType.Length; i++)
                operandType[i] = Tools.DetectOperandType(operand[i]);

            //Check if operation is not forbidden
            if (!"register;registerX;segment;pointer;memory".Contains(operandType[0])) throw new Exception($"Operand type for {instruction} instruction should only be 'register', 'registerX', 'pointer', 'segment' or 'memory' - recieved '{operandType[0]}'");

            //Read operand1 value
            int divident = Storage.Register["AH"] * 256 + Storage.Register["AL"];
            int divisor = Tools.ReadDataFromOperand(operand[0], operandType[0]);

            //Test if division is possible
            if (divisor == 0) throw new Exception("Dividing by 0 is forbidden");

            //Determine and adjust final value(s)
            int quotient = Tools.AdjustValue(divident / divisor, operandType[0]);
            int reminder = Tools.AdjustValue(divident % divisor, operandType[0]);

            //Distinguish Small and Big division, and act accordingly
            if (operandType[0] == "register") //SMALL DIVISION
            {
                //Write results value
                Storage.Register["AH"] = reminder;
                Storage.Register["AL"] = quotient;
            }
            else //BIG DIVISION
            {
                //Write results value
                Tools.WriteDataToOperand("DX", "registerX", reminder);
                Tools.WriteDataToOperand("AX", "registerX", quotient);
            }

            //Modify flags
        }

        //Increment [operand 1] and save to [operand 1]
        public static void INC(string command)
        {
            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 1);

            //Prepare operands
            string instruction = command.Split(' ')[0];
            command = command.Substring(command.Split(' ')[0].Length);
            string[] operand = command.Split(',');
            for (int i = 0; i < operand.Length; i++)
                operand[i] = operand[i].Trim();

            //Detect operand types
            string[] operandType = new string[operand.Length];
            for (int i = 0; i < operandType.Length; i++)
                operandType[i] = Tools.DetectOperandType(operand[i]);

            //Check if operation is not forbidden
            if (!"register;registerX;segment;pointer;memory".Contains(operandType[0])) throw new Exception($"Operand type for {instruction} instruction should only be 'register', 'registerX', 'pointer', 'segment' or 'memory' - recieved '{operandType[0]}'");

            //Read operand1 value
            int operandValue = Tools.ReadDataFromOperand(operand[0], operandType[0]);

            //Determine and adjust final value(s)
            int valueToWrite = Tools.AdjustValue(operandValue + 1, operandType[0]);

            //Write operand1 value
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

            //Determine and adjust final value(s)
            int valueToWrite = Tools.AdjustValue(operandValue, operandType[0]);

            //Write operand1 value
            Tools.WriteDataToOperand(operand[0], operandType[0], valueToWrite);

            //Modify flags
        }

        //Multiply [AL] by an [operand 1] and save to [AX]
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

            //Determine and adjust final value(s)
            int valueToWrite = Tools.AdjustValue(operandValue * Storage.Register["AL"], "registerX");

            //Write results value
            Tools.WriteDataToOperand("AX", "registerX", valueToWrite);

            //Modify flags
            Tools.UpdateParityFlag(valueToWrite); //PF
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

            //Determine and adjust final value(s)
            int valueToWrite = Tools.AdjustValue(operandValue[0] - operandValue[1] - Storage.Flags["CF"], operandType[0]);

            //Write operand1 value
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

            //Determine and adjust final value(s)
            int valueToWrite = Tools.AdjustValue(operandValue[0] - operandValue[1], operandType[0]);

            //Write operand1 value
            Tools.WriteDataToOperand(operand[0], operandType[0], valueToWrite);

            //Modify flags
            Tools.UpdateParityFlag(valueToWrite);
        }
    }
}