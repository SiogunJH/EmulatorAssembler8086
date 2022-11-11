namespace System
{
    class Algorithms
    {
        //Correct the result of addition of two ASCII values
        //If lower nibble of [AL] > 9 or [AF] is up
        //      [AL]+=6, [AH]+=1, [AF]=1, [CF]=1
        //else:
        //      AF=0, CF=0
        //In both the cases clear the Higher Nibble of AL
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
            valueToWriteAL = Tools.AdjustValue(valueToWriteAL, "regHL", false);
            valueToWriteAH = Tools.AdjustValue(valueToWriteAH, "regHL", false);

            //Write value(s)
            Tools.WriteDataToOperand("AL", "regHL", valueToWriteAL);
            Tools.WriteDataToOperand("AH", "regHL", valueToWriteAH);

            //Modify flags
            Tools.UpdateParityFlag(valueToWriteAL);
            Tools.UpdateSignFlag(valueToWriteAL, "regHL");
        }

        //Prepare the ASCII values of AL and AH to division
        //[AL] = [AH]*10 + [AL]
        //[AH] = 0
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
            valueToWriteAL = Tools.AdjustValue(valueToWriteAL, "regHL", false);
            valueToWriteAH = Tools.AdjustValue(valueToWriteAH, "regHL", false);

            //Write value(s)
            Tools.WriteDataToOperand("AL", "regHL", valueToWriteAL);
            Tools.WriteDataToOperand("AH", "regHL", valueToWriteAH);

            //Modify flags
            Tools.UpdateParityFlag(valueToWriteAL);
            Tools.UpdateSignFlag(valueToWriteAL, "regHL");
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
            valueToWriteAL = Tools.AdjustValue(valueToWriteAL, "regHL", false);
            valueToWriteAL %= 16; //Clear higher nibble
            valueToWriteAH = Tools.AdjustValue(valueToWriteAH, "regHL", false);

            //Write value(s)
            Tools.WriteDataToOperand("AL", "regHL", valueToWriteAL);
            Tools.WriteDataToOperand("AH", "regHL", valueToWriteAH);

            //Modify flags
            Tools.UpdateParityFlag(valueToWriteAL);
            Tools.UpdateSignFlag(valueToWriteAL, "regHL");
        }

        //Correct the result of addition of two ASCII values
        //If lower nibble of [AL] > 9 or [AF] is up:
        //      [AL]-=6, [AH]-=1, [AF]=1, [CF]=1
        //else:
        //      AF=0, CF=0
        //In both the cases clear the Higher Nibble of AL
        public static void AAS(string command)
        {
            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 0);

            //Read and test value(s)
            int valueToWriteAL = Storage.Register["AL"];
            int valueToWriteAH = Storage.Register["AH"];
            if (valueToWriteAL % 16 > 9) //lower nibble
                Storage.Flags["AF"] = 1;

            //Adjust value(s)
            if (Storage.Flags["AF"] == 1)
            {
                valueToWriteAL -= 6;
                valueToWriteAH -= 1;
                Storage.Flags["CF"] = 1;
            }
            else
            {
                Storage.Flags["CF"] = 0;
            }

            //Determine and adjust final value(s)
            valueToWriteAL = Tools.AdjustValue(valueToWriteAL, "regHL", false);
            valueToWriteAL %= 16; //Clear higher nibble
            valueToWriteAH = Tools.AdjustValue(valueToWriteAH, "regHL", false);

            //Write value(s)
            Tools.WriteDataToOperand("AL", "regHL", valueToWriteAL);
            Tools.WriteDataToOperand("AH", "regHL", valueToWriteAH);

            //Modify flags
            Tools.UpdateParityFlag(valueToWriteAL);
            Tools.UpdateSignFlag(valueToWriteAL, "regHL");
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
            for (int i = 0; i < operand.Length; i++)
                operandValue[i] = Tools.ReadDataFromOperand(operand[i], operandType[i]);

            //Determine and adjust final value(s)
            int valueToWrite = Tools.AdjustValue(operandValue[0] + operandValue[1] + Storage.Flags["CF"], operandType[0], false);

            //Write operand1 value
            Tools.WriteDataToOperand(operand[0], operandType[0], valueToWrite);

            //Modify flags
            Tools.UpdateParityFlag(valueToWrite);
            Tools.UpdateSignFlag(valueToWrite, operandType[0]);
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
            for (int i = 0; i < operand.Length; i++)
                operandValue[i] = Tools.ReadDataFromOperand(operand[i], operandType[i]);

            //Determine and adjust final value(s)
            int valueToWrite = Tools.AdjustValue(operandValue[0] + operandValue[1], operandType[0], false);

            //Write operand1 value
            Tools.WriteDataToOperand(operand[0], operandType[0], valueToWrite);

            //Modify flags
            Tools.UpdateParityFlag(valueToWrite);
            Tools.UpdateSignFlag(valueToWrite, operandType[0]);
        }

        //Convert byte into signed word
        //If high bit of [AL] is 1:
        //      [AH]=255
        //else:
        //      [AH]=0
        public static void CBW(string command)
        {
            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 0);

            //Read and test value(s)

            //Adjust value(s)
            if (Storage.Register["AL"] / 128 == 1) //Check if high bit is 1
                Storage.Register["AH"] = 255;
            else
                Storage.Register["AH"] = 0;

            //Determine and adjust final value(s)

            //Write value(s)

            //Modify flags
        }

        //Convert word into double word
        //If high bit of [AX] is 1:
        //      [DX]=255
        //else:
        //      [DX]=0
        public static void CWD(string command)
        {
            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 0);

            //Read and test value(s)

            //Adjust value(s)
            if (Storage.Register["AH"] / 128 == 1) //Check if high bit is 1
            {
                Storage.Register["DH"] = 255;
                Storage.Register["DL"] = 255;
            }
            else
            {
                Storage.Register["DH"] = 0;
                Storage.Register["DL"] = 0;
            }

            //Determine and adjust final value(s)

            //Write value(s)

            //Modify flags
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
            valueToWrite = Tools.AdjustValue(valueToWrite, "regHL", false);

            //Write operand1 value
            Tools.WriteDataToOperand("AL", "regHL", valueToWrite);

            //Modify flags
            Tools.UpdateParityFlag(valueToWrite);
            Tools.UpdateSignFlag(valueToWrite, "regHL");
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
            valueToWrite = Tools.AdjustValue(valueToWrite, "regHL", false);

            //Write operand1 value
            Tools.WriteDataToOperand("AL", "regHL", valueToWrite);

            //Modify flags
            Tools.UpdateParityFlag(valueToWrite);
            Tools.UpdateSignFlag(valueToWrite, "regHL");
        }

        //META COMMAND
        //Toggles between Normal Mode and Debug Mode
        public static void DEBUG()
        {
            if (Storage.DebugMode)
            {
                Storage.DebugMode = false;
                Console.WriteLine("Debug Mode is Off");
            }
            else
            {
                Storage.DebugMode = true;
                Console.WriteLine("Debug Mode is On");
            }
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
            if (!"regHL;regX;segment;pointer;memory".Contains(operandType[0])) throw new Exception($"Operand type for {instruction} instruction should only be 'regHL', 'regX', 'pointer', 'segment' or 'memory' - recieved '{operandType[0]}'");

            //Read operand1 value
            int operandValue = Tools.ReadDataFromOperand(operand[0], operandType[0]);

            //Determine and adjust final value(s)
            int valueToWrite = Tools.AdjustValue(operandValue - 1, operandType[0], false);

            //Write operand1 value
            Tools.WriteDataToOperand(operand[0], operandType[0], valueToWrite);

            //Modify flags
            Tools.UpdateParityFlag(valueToWrite);
            Tools.UpdateSignFlag(valueToWrite, operandType[0]);
        }

        //Divide [AX] by [operand 1]
        //[AL] or [AX] will contain results (/)
        //[AH] or [DX] will contain modulus (%)
        //Save location depends on wether diviser is of 'regHL/memory' type (Small Division) or not (Big Division)
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
            if (!"regHL;regX;segment;pointer;memory".Contains(operandType[0]))
                throw new Exception($"Operand type for {instruction} instruction should only be 'regHL', 'regX', 'pointer', 'segment' or 'memory' - recieved '{operandType[0]}'");

            //Read value(s)
            int divident = Storage.Register["AH"] * 256 + Storage.Register["AL"]; //SMALL DIVISION
            if (operandType[0] != "regX" && operandType[0] != "memory") //BIG DIVISION
                divident += Storage.Register["DH"] * 256 * 256 * 256 + Storage.Register["DL"] * 256 * 256;
            int divisor = Tools.ReadDataFromOperand(operand[0], operandType[0]); //BIG/SMALL DIVISION

            //Test if division is possible
            if (divisor == 0) throw new Exception("Dividing by 0 is forbidden");

            //Determine if division does not generate overflow
            int quotient = divident / divisor;
            int reminder = divident % divisor;
            if ((operandType[0] == "regHL" || operandType[0] == "memory") && quotient > 255) //SMALL DIVISION
                throw new Exception(String.Format("Quotient cannot exceed FF (255). Your quotient was {0:X} ({0})", quotient));
            else if (operandType[0] != "regHL" && operandType[0] != "memory" && quotient > 65535)//BIG DIVISION
                throw new Exception(String.Format("Quotient cannot exceed FFFF (65535). Your quotient was {0:X} ({0})", quotient));

            //Determine and adjust final value(s)
            quotient = Tools.AdjustValue(quotient, operandType[0], false);
            reminder = Tools.AdjustValue(reminder, operandType[0], false);

            //Distinguish Small and Big division, and act accordingly
            if (operandType[0] == "regHL" || operandType[0] == "memory") //SMALL DIVISION
            {
                //Write results value
                Storage.Register["AH"] = reminder;
                Storage.Register["AL"] = quotient;
            }
            else //BIG DIVISION
            {
                //Write results value
                Tools.WriteDataToOperand("DX", "regX", reminder);
                Tools.WriteDataToOperand("AX", "regX", quotient);
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
            if (!"regHL;regX;segment;pointer;memory".Contains(operandType[0])) throw new Exception($"Operand type for {instruction} instruction should only be 'regHL', 'regX', 'pointer', 'segment' or 'memory' - recieved '{operandType[0]}'");

            //Read operand1 value
            int operandValue = Tools.ReadDataFromOperand(operand[0], operandType[0]);

            //Determine and adjust final value(s)
            int valueToWrite = Tools.AdjustValue(operandValue + 1, operandType[0], false);

            //Write operand1 value
            Tools.WriteDataToOperand(operand[0], operandType[0], valueToWrite);

            //Modify flags
            Tools.UpdateParityFlag(valueToWrite);
            Tools.UpdateSignFlag(valueToWrite, operandType[0]);
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
            int valueToWrite = Tools.AdjustValue(operandValue, operandType[0], false);

            //Write operand1 value
            Tools.WriteDataToOperand(operand[0], operandType[0], valueToWrite);

            //Modify flags
        }

        //Negate the value of [operand1] and save to [operand1]
        public static void NEG(string command)
        {
            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 1);

            //Save instruction
            string instruction = command.Split(' ')[0];

            //Prepare operand(s)
            command = command.Substring(command.Split(' ')[0].Length);
            string operand = command.Trim();

            //Detect operand type(s)
            string operandType = Tools.DetectOperandType(operand);

            //Check if operation is not forbidden
            if (!"regHL;regX;segment;pointer;memory".Contains(operandType)) throw new Exception($"Operand type for {instruction} instruction should only be 'regHL', 'regX', 'pointer', 'segment' or 'memory' - recieved '{operandType[0]}'");

            //Read value(s)
            int operandValue = Tools.ReadDataFromOperand(operand, operandType);
            int maxValue;

            //Update value(s)
            if (operandType == "regHL" || operandType == "memory") maxValue = 255;
            else maxValue = 65535;

            //Determine and adjust final value(s)
            int valueToWrite = maxValue - operandValue;
            if (Storage.DebugMode) Console.WriteLine("NEG:\n\tOperand: {0:X4}\n\tValue: {1:X4}\n\tNegated Value: {2:X4}\n\tMax Value: {3:X4}\n", operand, operandValue, valueToWrite, maxValue);

            //Write operand1 value
            Tools.WriteDataToOperand(operand, operandType, valueToWrite);

            //Modify flags
            Tools.UpdateParityFlag(valueToWrite);
            Tools.UpdateSignFlag(valueToWrite, operandType);
        }

        //Multiply [AL] by an [operand 1] and save to [AX]
        //Update [Parity Flag] afterwards
        public static void MUL(string command)
        {
            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 1);

            //Prepare operands
            command = command.Substring(command.Split(' ')[0].Length);
            string operand = command.Split(',')[0].Trim();

            //Detect operand types
            string operandType = Tools.DetectOperandType(operand);

            //Read operand1 value
            int operandValue = Tools.ReadDataFromOperand(operand, operandType);

            //Determine and adjust final value(s)
            int valueToWrite = Tools.AdjustValue(operandValue * Storage.Register["AL"], "regX", false);

            //Write results value
            Tools.WriteDataToOperand("AX", "regX", valueToWrite);

            //Modify flags
            Tools.UpdateParityFlag(valueToWrite);
            Tools.UpdateSignFlag(valueToWrite, "regX");
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
            int valueToWrite = Tools.AdjustValue(operandValue[0] - operandValue[1] - Storage.Flags["CF"], operandType[0], false);

            //Write operand1 value
            Tools.WriteDataToOperand(operand[0], operandType[0], valueToWrite);

            //Modify flags
            Tools.UpdateParityFlag(valueToWrite);
            Tools.UpdateSignFlag(valueToWrite, operandType[0]);
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
            int valueToWrite = Tools.AdjustValue(operandValue[0] - operandValue[1], operandType[0], false);

            //Write operand1 value
            Tools.WriteDataToOperand(operand[0], operandType[0], valueToWrite);

            //Modify flags
            Tools.UpdateParityFlag(valueToWrite);
            Tools.UpdateSignFlag(valueToWrite, operandType[0]);
        }

        //Exchange data between [operand 1] and [operand 2]
        public static void XCHG(string command)
        {
            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 2);

            //Prepare operands
            string instruction = command.Split(' ')[0];
            int[] operandValue = new int[2];
            command = command.Substring(command.Split(' ')[0].Length);
            string[] operand = command.Split(',');
            for (int i = 0; i < operand.Length; i++)
                operand[i] = operand[i].Trim();

            //Detect operand types
            string[] operandType = new string[operand.Length];
            for (int i = 0; i < operandType.Length; i++)
                operandType[i] = Tools.DetectOperandType(operand[i]);

            //Check if operation is allowed
            if (!(
                ("regHL;memory".Contains(operandType[0]) && ("regHL;memory".Contains(operandType[1]))) ||
                ("regX;segment;pointer".Contains(operandType[0]) && ("regX;segment;pointer".Contains(operandType[1])))
                ))
                throw new Exception($"Operand type for {instruction} instruction should only be 'regHL/memory' to 'regHL/memory' or 'regX/pointer/segment' to 'regX/pointer/segment' - recieved '{operandType[0]}' to '{operandType[1]}'");


            //Read operand2 value
            operandValue[0] = Tools.ReadDataFromOperand(operand[0], operandType[0]);

            //Read operand2 value
            operandValue[1] = Tools.ReadDataFromOperand(operand[1], operandType[1]);

            //Determine and adjust final value(s)

            //Write value(s)
            Tools.WriteDataToOperand(operand[0], operandType[0], operandValue[1]);
            Tools.WriteDataToOperand(operand[1], operandType[1], operandValue[0]);

            //Modify flags
        }
    }
}