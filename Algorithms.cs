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
            long valueToWriteAL = Storage.Register["AL"];
            long valueToWriteAH = Storage.Register["AH"];
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
            long valueToWriteAL = Storage.Register["AL"];
            long valueToWriteAH = Storage.Register["AH"];

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
            long valueToWriteAL = Storage.Register["AL"];
            long valueToWriteAH = Storage.Register["AH"];

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
            long valueToWriteAL = Storage.Register["AL"];
            long valueToWriteAH = Storage.Register["AH"];
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
            for (long i = 0; i < operand.Length; i++)
                operand[i] = operand[i].Trim();

            //Detect operand types
            string[] operandType = new string[operand.Length];
            for (long i = 0; i < operandType.Length; i++)
                operandType[i] = Tools.DetectOperandType(operand[i]);

            //Read operand1 and operand2 value
            long[] operandValue = new long[2];
            for (long i = 0; i < operand.Length; i++)
                operandValue[i] = Tools.ReadDataFromOperand(operand[i], operandType[i]);

            //Determine and adjust final value(s)
            long valueToWrite = Tools.AdjustValue(operandValue[0] + operandValue[1] + Storage.Flags["CF"], operandType[0], false);

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
            for (long i = 0; i < operand.Length; i++)
                operand[i] = operand[i].Trim();

            //Detect operand types
            string[] operandType = new string[operand.Length];
            for (long i = 0; i < operandType.Length; i++)
                operandType[i] = Tools.DetectOperandType(operand[i]);

            //Read operand1 and operand2 value
            long[] operandValue = new long[2];
            for (long i = 0; i < operand.Length; i++)
                operandValue[i] = Tools.ReadDataFromOperand(operand[i], operandType[i]);

            //Determine and adjust final value(s)
            long valueToWrite = Tools.AdjustValue(operandValue[0] + operandValue[1], operandType[0], false);

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
            long valueToWrite = Storage.Register["AL"];
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
            long valueToWrite = Storage.Register["AL"];
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
            for (long i = 0; i < operand.Length; i++)
                operand[i] = operand[i].Trim();

            //Detect operand types
            string[] operandType = new string[operand.Length];
            for (long i = 0; i < operandType.Length; i++)
                operandType[i] = Tools.DetectOperandType(operand[i]);

            //Check if operation is not forbidden
            if (!"regHL;regX;segment;pointer;memory".Contains(operandType[0])) throw new Exception($"Operand type for {instruction} instruction should only be 'regHL', 'regX', 'pointer', 'segment' or 'memory' - recieved '{operandType[0]}'");

            //Read operand1 value
            long operandValue = Tools.ReadDataFromOperand(operand[0], operandType[0]);

            //Determine and adjust final value(s)
            long valueToWrite = Tools.AdjustValue(operandValue - 1, operandType[0], false);

            //Write operand1 value
            Tools.WriteDataToOperand(operand[0], operandType[0], valueToWrite);

            //Modify flags
            Tools.UpdateParityFlag(valueToWrite);
            Tools.UpdateSignFlag(valueToWrite, operandType[0]);
        }

        //IF [operand 1] is 8-bit, divide [AX] by [operand 1]
        //IF [operand 1] is 16-bit, divide [DX AX] by [operand 1]
        //[AL] or [AX] will contain results (/)
        //[AH] or [DX] will contain modulus (%)
        //Save location depends on wether diviser is of 'regHL/memory' type (Small Division) or not (Big Division)
        public static void DIV(string command)
        {
            //DEBUG Display
            if (Storage.DebugMode) Console.WriteLine("DIV:");

            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 1);

            //Prepare operands
            string instruction = command.Split(' ')[0];
            command = command.Substring(command.Split(' ')[0].Length);
            string operand = command.Trim();
            if (Storage.DebugMode) Console.WriteLine("\tOperand Name: {0}", operand);

            //Detect operand types
            string operandType = Tools.DetectOperandType(operand);
            if (Storage.DebugMode) Console.WriteLine("\tOperand Type: {0}", operandType);

            //Check if operation is not forbidden
            if (!"regHL;regX;segment;pointer;memory".Contains(operandType))
                throw new Exception($"Operand type for {instruction} instruction should only be 'regHL', 'regX', 'pointer', 'segment' or 'memory' - recieved '{operandType}'");

            //Read value(s)
            long divident = Storage.Register["AH"] * 256 + Storage.Register["AL"]; //SMALL DIVISION
            if (operandType != "regHL " && operandType != "memory") //BIG DIVISION
                divident += Storage.Register["DH"] * 256 * 256 * 256 + Storage.Register["DL"] * 256 * 256;
            long divisor = Tools.ReadDataFromOperand(operand, operandType); //BIG/SMALL DIVISION

            if (Storage.DebugMode) Console.WriteLine("\tOperand Value (Divisor): {0}", divisor);
            if (Storage.DebugMode) Console.WriteLine("\tDivident: {0}", divident);

            //Test if division is possible
            if (divisor == 0) throw new Exception("Dividing by 0 is forbidden");

            //Determine if division does not generate overflow
            long quotient = divident / divisor;
            long reminder = divident % divisor;

            if (Storage.DebugMode) Console.WriteLine("\tQuotient Raw Value: {0}", quotient);
            if (Storage.DebugMode) Console.WriteLine("\tReminder Raw Value: {0}", reminder);

            if ((operandType == "regHL" || operandType == "memory") && quotient > 255) //SMALL DIVISION
                throw new Exception(String.Format("Quotient cannot exceed FF (255). Your quotient was {0:X} ({0})", quotient));
            else if (quotient > 65535)//BIG DIVISION
                throw new Exception(String.Format("Quotient cannot exceed FFFF (65535). Your quotient was {0:X} ({0})", quotient));

            //Determine and adjust final value(s)
            quotient = Tools.AdjustValue(quotient, operandType, false);
            reminder = Tools.AdjustValue(reminder, operandType, false);

            if (Storage.DebugMode) Console.WriteLine("\tQuotient Value: {0}", quotient);
            if (Storage.DebugMode) Console.WriteLine("\tReminder Value: {0}", reminder);

            //Distinguish Small and Big division, and act accordingly
            if (operandType == "regHL" || operandType == "memory") //SMALL DIVISION
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

        //Moves (copies) data from [port] with the address of an [operand 2] to [operand 1]
        //[Operand 1] must be AX or AL
        public static void IN(string command)
        {
            //DEBUG Display
            if (Storage.DebugMode) Console.WriteLine("IN:");

            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 2);

            //Prepare operands
            string instruction = command.Split(' ')[0];
            command = command.Substring(command.Split(' ')[0].Length);
            string[] operand = command.Split(',');
            for (long i = 0; i < operand.Length; i++)
                operand[i] = operand[i].Trim();
            if (Storage.DebugMode) Console.WriteLine("\tOperand 1 Name: {0}", operand[0]);
            if (Storage.DebugMode) Console.WriteLine("\tOperand 2 (Port Address) Name: {0}", operand[1]);

            //Detect operand types
            string[] operandType = new string[operand.Length];
            for (long i = 0; i < operandType.Length; i++)
                operandType[i] = Tools.DetectOperandType(operand[i]);
            if (Storage.DebugMode) Console.WriteLine("\tOperand 1 Type: {0}", operandType[0]);
            if (Storage.DebugMode) Console.WriteLine("\tOperand 2 (Port Address) Type: {0}", operandType[1]);

            //Check if operation is not forbidden
            if (!"AX;AH".Contains(operand[0]))
                throw new Exception($"Operand 1 name for {instruction} instruction should only be 'AH' or 'AX' - recieved '{operand[0]}'");

            //Read operand value(s)
            long operandValue = Tools.ReadDataFromOperand(operand[1], operandType[1]);
            if (Storage.DebugMode) Console.WriteLine("\tOperand 2 (Port Address) Value: {0}", operandValue);

            //Get value from port
            long portValue;
            bool results = Storage.Port.ContainsKey(operandValue);
            if (results) //Port address is in use
                portValue = Storage.Port[operandValue];
            else //New port address
                portValue = 0;

            //Write value(s)
            Tools.WriteDataToOperand(operand[0], operandType[0], portValue);

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
            for (long i = 0; i < operand.Length; i++)
                operand[i] = operand[i].Trim();

            //Detect operand types
            string[] operandType = new string[operand.Length];
            for (long i = 0; i < operandType.Length; i++)
                operandType[i] = Tools.DetectOperandType(operand[i]);

            //Check if operation is not forbidden
            if (!"regHL;regX;segment;pointer;memory".Contains(operandType[0])) throw new Exception($"Operand type for {instruction} instruction should only be 'regHL', 'regX', 'pointer', 'segment' or 'memory' - recieved '{operandType[0]}'");

            //Read operand1 value
            long operandValue = Tools.ReadDataFromOperand(operand[0], operandType[0]);

            //Determine and adjust final value(s)
            long valueToWrite = Tools.AdjustValue(operandValue + 1, operandType[0], false);

            //Write operand1 value
            Tools.WriteDataToOperand(operand[0], operandType[0], valueToWrite);

            //Modify flags
            Tools.UpdateParityFlag(valueToWrite);
            Tools.UpdateSignFlag(valueToWrite, operandType[0]);
        }

        //IF [operand 1] is 8-bit, divide [AX] by [operand 1]
        //IF [operand 1] is 16-bit, divide [DX AX] by [operand 1]
        //[AL] or [AX] will contain results (/)
        //[AH] or [DX] will contain modulus (%)
        //Save location depends on wether diviser is of 'regHL/memory' type (Small Division) or not (Big Division)
        public static void IDIV(string command)
        {
            //DEBUG Display
            if (Storage.DebugMode) Console.WriteLine("DIV:");

            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 1);

            //Prepare operands
            string instruction = command.Split(' ')[0];
            command = command.Substring(command.Split(' ')[0].Length);
            string operand = command.Trim();
            if (Storage.DebugMode) Console.WriteLine("\tOperand Name: {0}", operand);

            //Detect operand types
            string operandType = Tools.DetectOperandType(operand);
            if (Storage.DebugMode) Console.WriteLine("\tOperand Type: {0}", operandType);

            //Check if operation is not forbidden
            if (!"regHL;regX;segment;pointer;memory".Contains(operandType))
                throw new Exception($"Operand type for {instruction} instruction should only be 'regHL', 'regX', 'pointer', 'segment' or 'memory' - recieved '{operandType}'");

            //Read value(s)

            long divisor = Tools.ReadDataFromOperand(operand, operandType);
            long divident = Storage.Register["AH"] * 256 + Storage.Register["AL"]; //SMALL DIVISION
            if (operandType != "regHL" && operandType != "memory") //BIG DIVISION
                divident += Storage.Register["DH"] * (256 * 256 * 256) + Storage.Register["DL"] * 256 * 256;

            if (Storage.DebugMode) Console.WriteLine("\tOperand Value (Divisor): {0}", divisor);
            if (Storage.DebugMode) Console.WriteLine("\tDivident: {0}", divident);

            //Translate unsigned to signed value(s)
            if ((operandType == "regHL" || operandType == "memory") && (Convert.ToString(divident, 2).Length == 16 && Convert.ToString(divident, 2)[0] == '1')) //SMALL DIVISION
                divident = divident - (256 * 256);
            else if (operandType != "regHL" && operandType != "memory" && (Convert.ToString(divident, 2).Length == 32 && Convert.ToString(divident, 2)[0] == '1')) //BIG DIVISION
            {
                long temp = 256 * 256 * 256;
                temp *= 256;
                divident = divident - (temp);
            }

            if ((operandType == "regHL" || operandType == "memory") && (Convert.ToString(divisor, 2).Length == 8 && Convert.ToString(divisor, 2)[0] == '1'))
                divisor = divisor - 256;
            else if (operandType != "regHL" && operandType != "memory" && (Convert.ToString(divisor, 2).Length == 16 && Convert.ToString(divisor, 2)[0] == '1'))
                divisor = divisor - (256 * 256);

            //Test if division is possible
            if (divisor == 0) throw new Exception("Dividing by 0 is forbidden");

            //Determine if division does not generate overflow
            long quotient = divident / divisor;
            long reminder = divident % divisor;

            if (Storage.DebugMode) Console.WriteLine("\tQuotient Raw Value: {0}", quotient);
            if (Storage.DebugMode) Console.WriteLine("\tReminder Raw Value: {0}", reminder);

            if ((operandType == "regHL" || operandType == "memory") && quotient > 255) //SMALL DIVISION
                throw new Exception(String.Format("Quotient cannot exceed FF (255). Your quotient was {0:X} ({0})", quotient));
            else if (quotient > 65535)//BIG DIVISION
                throw new Exception(String.Format("Quotient cannot exceed FFFF (65535). Your quotient was {0:X} ({0})", quotient));

            //Determine and adjust final value(s)
            quotient = Tools.AdjustValue(quotient, operandType, false);
            reminder = Tools.AdjustValue(reminder, operandType, false);

            if (Storage.DebugMode) Console.WriteLine("\tQuotient Value: {0}", quotient);
            if (Storage.DebugMode) Console.WriteLine("\tReminder Value: {0}", reminder);

            //Distinguish Small and Big division, and act accordingly
            if (operandType == "regHL" || operandType == "memory") //SMALL DIVISION
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

        //IF [operand 1] is 8-bit:
        //Multiply [AL] by an [operand 1] and save to [AX]
        //IF [operand 1] is 16-bit:
        //Multiply [AX] by an [operand 1] and save to [DX AX]
        public static void IMUL(string command)
        {
            //DEBUG Display
            if (Storage.DebugMode) Console.WriteLine("MUL:");

            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 1);

            //Prepare operands
            string instruction = command.Split(' ')[0];
            command = command.Substring(command.Split(' ')[0].Length);
            string operand = command.Split(',')[0].Trim();
            if (Storage.DebugMode) Console.WriteLine("\tOperand Name: {0}", operand);

            //Detect operand types
            string operandType = Tools.DetectOperandType(operand);
            if (Storage.DebugMode) Console.WriteLine("\tOperand Type: {0}", operandType);

            //Check if operation is not forbidden
            if (!"regHL;regX;segment;pointer;memory".Contains(operandType[0]))
                throw new Exception($"Operand type for {instruction} instruction should only be 'regHL', 'regX', 'pointer', 'segment' or 'memory' - recieved '{operandType[0]}'");

            //Read value(s)
            long multiplicator = Tools.ReadDataFromOperand(operand, operandType);
            if (Storage.DebugMode) Console.WriteLine("\tOperand Value (Multiplicator): {0}", multiplicator);
            if ((operandType == "regHL" || operandType == "memory") && (Convert.ToString(multiplicator, 2).Length == 8 && Convert.ToString(multiplicator, 2)[0] == '1'))
                multiplicator = multiplicator - 256;
            else if (operandType != "regHL" && operandType != "memory" && (Convert.ToString(multiplicator, 2).Length == 16 && Convert.ToString(multiplicator, 2)[0] == '1'))
                multiplicator = multiplicator - 65536;
            if (Storage.DebugMode) Console.WriteLine("\tOperand Signed Value: {0}", multiplicator);

            long multiplicand;
            if (operandType == "regHL" || operandType == "memory")
            {
                multiplicand = Tools.ReadDataFromOperand("AL", "regHL");
                if (Storage.DebugMode) Console.WriteLine("\tMultiplicand Type: {0}", "regHL");
                if (Storage.DebugMode) Console.WriteLine("\tMultiplicand Value: {0}", multiplicand);
                if (Convert.ToString(multiplicand, 2).Length == 8 && Convert.ToString(multiplicand, 2)[0] == '1')
                    multiplicand = multiplicand - 256;
            }
            else
            {
                multiplicand = Tools.ReadDataFromOperand("AX", "regX");
                if (Storage.DebugMode) Console.WriteLine("\tMultiplicand Type: {0}", "regX");
                if (Storage.DebugMode) Console.WriteLine("\tMultiplicand Value: {0}", multiplicand);
                if (Convert.ToString(multiplicand, 2).Length == 16 && Convert.ToString(multiplicand, 2)[0] == '1')
                    multiplicand = multiplicand - 65536;
            }
            if (Storage.DebugMode) Console.WriteLine("\tMultiplicand Signed Value: {0}", multiplicand);

            //Determine value(s)
            long product = multiplicand * multiplicator;
            if (Storage.DebugMode) Console.WriteLine("\tProduct Raw Value: {0}", product);

            //Adjust and write result value(s)
            if (operandType == "regHL" || operandType == "memory")
            {
                product = Tools.AdjustValue(product, "regX", false);
                if (Storage.DebugMode) Console.WriteLine("\tProduct Final Value: {0}", product);
                Tools.WriteDataToOperand("AX", "regX", product);
            }
            else
            {
                long productOG = product;
                product = Tools.AdjustValue(productOG / (256 * 256), "regX", false);
                if (product == 0 && productOG < 0) product = Tools.AdjustValue(-1, "regX", false);
                if (Storage.DebugMode) Console.WriteLine("\tProduct Final Value (Upper): {0}", product);
                Tools.WriteDataToOperand("DX", "regX", product);
                product = Tools.AdjustValue(productOG % (256 * 256), "regX", false);
                if (Storage.DebugMode) Console.WriteLine("\tProduct Final Value (Lower): {0}", product);
                Tools.WriteDataToOperand("AX", "regX", product);
            }

            //Modify flag(s)
            Tools.UpdateParityFlag(product % (256 * 256));
            Tools.UpdateSignFlag(product % (256 * 256), "regX");
        }

        //Sets AH value accordingly to flag values, as follows:
        //AH Bits: 7[SF], 6[ZF], 5[0], 4[AF], 3[0], 2[PF], 1[1], 0[CF]
        public static void LAHF(string command)
        {
            //DEBUG Display
            if (Storage.DebugMode) Console.WriteLine("LAHF:");

            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 0);

            //Create binary string
            string binaryValue = String.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
            Convert.ToString(Storage.Flags["SF"]),
            Convert.ToString(Storage.Flags["ZF"]),
            "0",
            Convert.ToString(Storage.Flags["AF"]),
            "0",
            Convert.ToString(Storage.Flags["PF"]),
            "1",
            Convert.ToString(Storage.Flags["CF"]));
            if (Storage.DebugMode) Console.WriteLine("\tBinary Value: {0}", binaryValue);

            //Determine and adjust final value(s)
            long valueToWrite = Tools.Parse(binaryValue, 2);
            if (Storage.DebugMode) Console.WriteLine("\tDecimal Value: {0}", valueToWrite);

            //Write value(s)
            Tools.WriteDataToOperand("AH", "regHL", valueToWrite);

            //Modify flags
        }

        //Moves (copies) data from [operand 2] (must be 'regX', 'pointer' or 'memory') to [operand 1] (must be 'pointer')
        //IF [operand 2] is of 'memory' type, move (copy) the memory address instead of memory value
        public static void LEA(string command)
        {
            //DEBUG Display
            if (Storage.DebugMode) Console.WriteLine("LEA:");

            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 2);

            //Prepare operands
            string instruction = command.Split(' ')[0];
            command = command.Substring(command.Split(' ')[0].Length);
            string[] operand = command.Split(',');
            for (long i = 0; i < operand.Length; i++)
            {
                operand[i] = operand[i].Trim();
                if (Storage.DebugMode) Console.WriteLine("\tOperand {0} Name: {1}", i, operand[i]);
            }

            //Detect operand types
            string[] operandType = new string[operand.Length];
            for (long i = 0; i < operandType.Length; i++)
            {
                operandType[i] = Tools.DetectOperandType(operand[i]);
                if (Storage.DebugMode) Console.WriteLine("\tOperand {0} Type: {1}", i, operand[i]);
            }

            //Check if operation is not forbidden
            if (operandType[0] != "pointer" || !"regX;pointer;memory".Contains(operandType[1]))
                throw new Exception($"Operand types for {instruction} instruction should only be 'pointer' and 'regX', 'pointer' or 'memory' - recieved '{operandType[0]}' and '{operandType[1]}'");

            //Translate memory to number (memory address)
            if (operandType[1] == "memory")
            {
                operand[1] = operand[1].Substring(1, operand[1].Length - 2);
                operandType[1] = Tools.DetectOperandType(operand[1]);
                if (Storage.DebugMode) Console.WriteLine("\tMemory to Address Translation (Name): {0}", operand[1]);
                if (Storage.DebugMode) Console.WriteLine("\tMemory to Address Translation (Type): {0}", operandType[1]);
            }

            //Read value(s)
            long operandValue = Tools.ReadDataFromOperand(operand[1], operandType[1]);
            if (Storage.DebugMode) Console.WriteLine("\tOperand Raw Value: {0}", operandValue);

            //Determine and adjust final value(s)
            operandValue = Tools.AdjustValue(operandValue, operandType[0], false);
            if (Storage.DebugMode) Console.WriteLine("\tOperand Value: {0}", operandValue);

            //Write operand1 value
            Tools.WriteDataToOperand(operand[0], operandType[0], operandValue);

            //Modify flags
        }

        //Moves (copies) data from [operand 2] to [operand 1]
        public static void MOV(string command)
        {
            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 2);

            //Prepare operands
            command = command.Substring(command.Split(' ')[0].Length);
            string[] operand = command.Split(',');
            for (long i = 0; i < operand.Length; i++)
                operand[i] = operand[i].Trim();

            //Detect operand types
            string[] operandType = new string[operand.Length];
            for (long i = 0; i < operandType.Length; i++)
                operandType[i] = Tools.DetectOperandType(operand[i]);

            //Read operand2 value
            long operandValue = Tools.ReadDataFromOperand(operand[1], operandType[1]);

            //Determine and adjust final value(s)
            long valueToWrite = Tools.AdjustValue(operandValue, operandType[0], false);

            //Write operand1 value
            Tools.WriteDataToOperand(operand[0], operandType[0], valueToWrite);

            //Modify flags
        }

        //IF [operand 1] is 8-bit:
        //Multiply [AL] by an [operand 1] and save to [AX]
        //IF [operand 1] is 16-bit:
        //Multiply [AX] by an [operand 1] and save to [DX AX]
        public static void MUL(string command)
        {
            //DEBUG Display
            if (Storage.DebugMode) Console.WriteLine("MUL:");

            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 1);

            //Prepare operands
            string instruction = command.Split(' ')[0];
            command = command.Substring(command.Split(' ')[0].Length);
            string operand = command.Split(',')[0].Trim();
            if (Storage.DebugMode) Console.WriteLine("\tOperand Name: {0}", operand);

            //Detect operand types
            string operandType = Tools.DetectOperandType(operand);
            if (Storage.DebugMode) Console.WriteLine("\tOperand Type: {0}", operandType);

            //Check if operation is not forbidden
            if (!"regHL;regX;segment;pointer;memory".Contains(operandType[0]))
                throw new Exception($"Operand type for {instruction} instruction should only be 'regHL', 'regX', 'pointer', 'segment' or 'memory' - recieved '{operandType[0]}'");

            //Read value(s)
            long multiplicator = Tools.ReadDataFromOperand(operand, operandType);
            if (Storage.DebugMode) Console.WriteLine("\tOperand Value: {0}", multiplicator);

            long multiplicand;
            if (operandType == "regHL" || operandType == "memory")
                multiplicand = Tools.ReadDataFromOperand("AL", "regHL");
            else
                multiplicand = Tools.ReadDataFromOperand("AX", "regX");
            if (Storage.DebugMode) Console.WriteLine("\tMultiplicand Value: {0}", multiplicand);

            //Determine value(s)
            long product = multiplicand * multiplicator;
            if (Storage.DebugMode) Console.WriteLine("\tProduct Raw Value: {0}", product);

            //Adjust and write result value(s)
            if (operandType == "regHL" || operandType == "memory")
            {
                product = Tools.AdjustValue(product, "regX", false);
                if (Storage.DebugMode) Console.WriteLine("\tProduct Final Value: {0}", product);
                Tools.WriteDataToOperand("AX", "regX", product);
            }
            else
            {
                long productOG = product;
                product = Tools.AdjustValue(productOG / (256 * 256), "regX", false);
                if (Storage.DebugMode) Console.WriteLine("\tProduct Final Value (Upper): {0}", product);
                Tools.WriteDataToOperand("DX", "regX", product);
                product = Tools.AdjustValue(productOG % (256 * 256), "regX", false);
                if (Storage.DebugMode) Console.WriteLine("\tProduct Final Value (Lower): {0}", product);
                Tools.WriteDataToOperand("AX", "regX", product);
            }

            //Modify flag(s)
            Tools.UpdateParityFlag(product % (256 * 256));
            Tools.UpdateSignFlag(product % (256 * 256), "regX");
        }

        //Negate the value of [operand1] and save to [operand1]
        public static void NEG(string command)
        {
            //DEBUG Display
            if (Storage.DebugMode) Console.WriteLine("NEG:");

            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 1);

            //Save instruction
            string instruction = command.Split(' ')[0];

            //Prepare operand(s)
            command = command.Substring(command.Split(' ')[0].Length);
            string operand = command.Trim();
            if (Storage.DebugMode) Console.WriteLine("\tOperand Name: {0}", operand);

            //Detect operand type(s)
            string operandType = Tools.DetectOperandType(operand);
            if (Storage.DebugMode) Console.WriteLine("\tOperand Type: {0}", operandType);

            //Check if operation is not forbidden
            if (!"regHL;regX;segment;pointer;memory".Contains(operandType)) throw new Exception($"Operand type for {instruction} instruction should only be 'regHL', 'regX', 'pointer', 'segment' or 'memory' - recieved '{operandType[0]}'");

            //Read value(s)
            long operandValue = Tools.ReadDataFromOperand(operand, operandType);
            if (Storage.DebugMode) Console.WriteLine("\tOperand Value: {0}", operandValue);

            //Update value(s)
            long maxValue;
            if (operandType == "regHL" || operandType == "memory") maxValue = 256;
            else maxValue = 65536;
            if (Storage.DebugMode) Console.WriteLine("\tMax Value: {0}", maxValue);

            //Determine and adjust final value(s)
            long valueToWrite = maxValue - operandValue;
            if (Storage.DebugMode) Console.WriteLine("\tNegated Value: {0}", valueToWrite);

            //Adjust value(s)
            valueToWrite = Tools.AdjustValue(valueToWrite, operandType, false);

            //Write operand1 value
            Tools.WriteDataToOperand(operand, operandType, valueToWrite);

            //Modify flags
            Tools.UpdateParityFlag(valueToWrite);
            Tools.UpdateSignFlag(valueToWrite, operandType);
        }

        //Negate all bits of [operand 1]
        public static void NOT(string command)
        {
            //DEBUG Display
            if (Storage.DebugMode) Console.WriteLine("NOT:");

            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 1);

            //Save instruction
            string instruction = command.Split(' ')[0];

            //Prepare operand(s)
            command = command.Substring(command.Split(' ')[0].Length);
            string operand = command.Trim();
            if (Storage.DebugMode) Console.WriteLine("\tOperand Name: {0}", operand);

            //Detect operand type(s)
            string operandType = Tools.DetectOperandType(operand);
            if (Storage.DebugMode) Console.WriteLine("\tOperand Type: {0}", operandType);

            //Check if operation is not forbidden
            if (!"regHL;regX;segment;pointer;memory".Contains(operandType)) throw new Exception($"Operand type for {instruction} instruction should only be 'regHL', 'regX', 'pointer', 'segment' or 'memory' - recieved '{operandType[0]}'");

            //Read value(s)
            long operandValue = Tools.ReadDataFromOperand(operand, operandType);
            if (Storage.DebugMode) Console.WriteLine("\tOperand Value: {0}", operandValue);

            //Negate all bits
            long valueToWrite;
            if (operandType == "regHL" || operandType == "memory")
                valueToWrite = Tools.Parse(Convert.ToString(operandValue, 2).PadLeft(8, '0').Replace('0', '2').Replace('1', '0').Replace('2', '1'), 2); //8-bit
            else
                valueToWrite = Tools.Parse(Convert.ToString(operandValue, 2).PadLeft(16, '0').Replace('0', '2').Replace('1', '0').Replace('2', '1'), 2); //16-bit
            if (Storage.DebugMode) Console.WriteLine("\tNegated Value: {0}", valueToWrite);

            //Determine and adjust final value(s)
            valueToWrite = Tools.AdjustValue(valueToWrite, operandType, false);

            //Write operand1 value
            Tools.WriteDataToOperand(operand, operandType, valueToWrite);

            //Modify flags
            Tools.UpdateParityFlag(valueToWrite);
            Tools.UpdateSignFlag(valueToWrite, operandType);
        }

        //Moves (copies) data from [operand 2] to [port] with the address of an [operand 1]
        //[Operand 2] must be AX or AL
        public static void OUT(string command)
        {
            //DEBUG Display
            if (Storage.DebugMode) Console.WriteLine("OUT:");

            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 2);

            //Prepare operands
            string instruction = command.Split(' ')[0];
            command = command.Substring(command.Split(' ')[0].Length);
            string[] operand = command.Split(',');
            for (long i = 0; i < operand.Length; i++)
                operand[i] = operand[i].Trim();
            if (Storage.DebugMode) Console.WriteLine("\tOperand 1 (Port Address) Name: {0}", operand[0]);
            if (Storage.DebugMode) Console.WriteLine("\tOperand 2 Name: {0}", operand[1]);

            //Detect operand types
            string[] operandType = new string[operand.Length];
            for (long i = 0; i < operandType.Length; i++)
                operandType[i] = Tools.DetectOperandType(operand[i]);
            if (Storage.DebugMode) Console.WriteLine("\tOperand 1 (Port Address) Type: {0}", operandType[0]);
            if (Storage.DebugMode) Console.WriteLine("\tOperand 2 Type: {0}", operandType[1]);

            //Check if operation is not forbidden
            if (!"AX;AH".Contains(operand[1]))
                throw new Exception($"Operand 2 name for {instruction} instruction should only be 'AH' or 'AX' - recieved '{operand[1]}'");

            //Read operand value(s)
            long portAddress = Tools.ReadDataFromOperand(operand[0], operandType[0]);
            long operandValue = Tools.ReadDataFromOperand(operand[1], operandType[1]);
            if (Storage.DebugMode) Console.WriteLine("\tOperand 1 (Port Address) Value: {0}", portAddress);
            if (Storage.DebugMode) Console.WriteLine("\tOperand 2 Value: {0}", operandValue);

            //Set value to port
            bool results = Storage.Port.ContainsKey(portAddress);
            if (results) //Port address is in use
                Storage.Port[portAddress] = operandValue;
            else //New port address
                Storage.Port.Add(portAddress, operandValue);

            //Modify flags
        }

        //Get value from the stack
        //Get said value from [SP] and then set [SP] to [SP+2]
        public static void POP(string command)
        {
            //DEBUG Display
            if (Storage.DebugMode) Console.WriteLine("POP:");

            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 1);

            //Prepare operand(s)
            string instruction = command.Split(' ')[0];
            command = command.Substring(command.Split(' ')[0].Length);
            string operand = command.Trim();
            if (Storage.DebugMode) Console.WriteLine("\tOperand Name: {0}", operand);

            //Detect operand type(s)
            string operandType = Tools.DetectOperandType(operand);
            if (Storage.DebugMode) Console.WriteLine("\tOperand Type: {0}", operandType);

            //Check if operation is not forbidden
            if (!"regX;segment;pointer;memory".Contains(operandType))
                throw new Exception($"Operand type for {instruction} instruction should only be 'regX', 'pointer', 'segment' or 'memory' - recieved '{operandType}'");

            //Get stack address
            long stackAddress = Storage.Pointers["SP"];
            if (Storage.DebugMode) Console.WriteLine("\tStack Address: {0}", stackAddress);

            //Get value from stack
            bool results = Storage.Stack.ContainsKey(stackAddress);
            if (results) //Stack address is in use
                Tools.WriteDataToOperand(operand, operandType, Storage.Stack[stackAddress]);
            else //New stack address
                Tools.WriteDataToOperand(operand, operandType, 0);

            //Update Stack Pointer
            Storage.Pointers["SP"] = stackAddress + 2;

            //Modify flags
        }

        //Sets flags according to a value from stack
        //Get said value from [SP] and then set [SP] to [SP+2]
        //The value is as follows (in binary): 7[SF], 6[ZF], 5[0], 4[AF], 3[0], 2[PF], 1[1], 0[CF]
        public static void POPF(string command)
        {
            //DEBUG Display
            if (Storage.DebugMode) Console.WriteLine("POP:");

            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 0);

            //Get stack address
            long stackAddress = Storage.Pointers["SP"];
            if (Storage.DebugMode) Console.WriteLine("\tStack Address: {0}", stackAddress);

            //Get value from stack
            long stackValue;
            bool results = Storage.Stack.ContainsKey(stackAddress);
            if (results) //Stack address is in use
                stackValue = Storage.Stack[stackAddress];
            else //New stack address
                stackValue = 0;

            //Update Stack Pointer
            Storage.Pointers["SP"] = stackAddress + 2;

            //Create binary string
            string binaryValue = Convert.ToString(stackValue, 2);
            for (long i = binaryValue.Length; i < 8; i++)
                binaryValue = String.Format("{0}{1}", "0", binaryValue);
            if (Storage.DebugMode) Console.WriteLine("\tBinary Value: {0}", binaryValue);

            //Write value(s)
            Storage.Flags["SF"] = (long)Convert.ToDouble(binaryValue[0].ToString());
            Storage.Flags["ZF"] = (long)Convert.ToDouble(binaryValue[1].ToString());
            Storage.Flags["AF"] = (long)Convert.ToDouble(binaryValue[3].ToString());
            Storage.Flags["PF"] = (long)Convert.ToDouble(binaryValue[5].ToString());
            Storage.Flags["CF"] = (long)Convert.ToDouble(binaryValue[7].ToString());

            //Modify flags
        }

        //Stores value in the stack
        //Set said value at [SP-2] and then set [SP] to [SP-2]
        public static void PUSH(string command)
        {
            //DEBUG Display
            if (Storage.DebugMode) Console.WriteLine("PUSH:");

            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 1);

            //Prepare operand(s)
            string instruction = command.Split(' ')[0];
            command = command.Substring(command.Split(' ')[0].Length);
            string operand = command.Trim();
            if (Storage.DebugMode) Console.WriteLine("\tOperand Name: {0}", operand);

            //Detect operand type(s)
            string operandType = Tools.DetectOperandType(operand);
            if (Storage.DebugMode) Console.WriteLine("\tOperand Type: {0}", operandType);

            //Check if operation is not forbidden
            if (!"regX;segment;pointer;memory".Contains(operandType))
                throw new Exception($"Operand type for {instruction} instruction should only be 'regX', 'pointer', 'segment' or 'memory' - recieved '{operandType}'");

            //Read operand value(s)
            long operandValue = Tools.ReadDataFromOperand(operand, operandType);
            if (Storage.DebugMode) Console.WriteLine("\tOperand Value: {0}", operandValue);

            //Get stack address
            long stackAddress = Storage.Pointers["SP"] - 2;
            if (Storage.DebugMode) Console.WriteLine("\tStack Address: {0}", stackAddress);
            stackAddress = Tools.AdjustValue(stackAddress, "segment", false);
            if (Storage.DebugMode) Console.WriteLine("\tStack Address Adjusted: {0}", stackAddress);

            //Put value on stack
            bool results = Storage.Stack.ContainsKey(stackAddress);
            if (results) //Stack address is in use
                Storage.Stack[stackAddress] = operandValue;
            else //New stack address
                Storage.Stack.Add(stackAddress, operandValue);

            //Update Stack Pointer
            Storage.Pointers["SP"] = stackAddress;

            //Modify flags
        }

        //Stores value made out of flags in the stack
        //Set said value at [SP-2] and then set [SP] to [SP-2]
        //The value is as follows (in binary): 7[SF], 6[ZF], 5[0], 4[AF], 3[0], 2[PF], 1[1], 0[CF]
        public static void PUSHF(string command)
        {
            //DEBUG Display
            if (Storage.DebugMode) Console.WriteLine("PUSHF:");

            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 0);

            //Create binary string
            string binaryValue = String.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
            Convert.ToString(Storage.Flags["SF"]),
            Convert.ToString(Storage.Flags["ZF"]),
            "0",
            Convert.ToString(Storage.Flags["AF"]),
            "0",
            Convert.ToString(Storage.Flags["PF"]),
            "1",
            Convert.ToString(Storage.Flags["CF"]));
            if (Storage.DebugMode) Console.WriteLine("\tBinary Value: {0}", binaryValue);

            //Determine and adjust final value(s)
            long valueToWrite = Tools.Parse(binaryValue, 2);
            if (Storage.DebugMode) Console.WriteLine("\tDecimal Value: {0}", valueToWrite);

            //Get stack address
            long stackAddress = Storage.Pointers["SP"] - 2;
            if (Storage.DebugMode) Console.WriteLine("\tStack Address: {0}", stackAddress);
            stackAddress = Tools.AdjustValue(stackAddress, "segment", false);
            if (Storage.DebugMode) Console.WriteLine("\tStack Address Adjusted: {0}", stackAddress);

            //Put value on stack
            bool results = Storage.Stack.ContainsKey(stackAddress);
            if (results) //Stack address is in use
                Storage.Stack[stackAddress] = valueToWrite;
            else //New stack address
                Storage.Stack.Add(stackAddress, valueToWrite);

            //Update Stack Pointer
            Storage.Pointers["SP"] = stackAddress;

            //Modify flags
        }

        //Sets flag values according to AH binary value, as follows:
        //AH Bits: 7[SF], 6[ZF], 5[-], 4[AF], 3[-], 2[PF], 1[-], 0[CF]
        public static void SAHF(string command)
        {
            //DEBUG Display
            if (Storage.DebugMode) Console.WriteLine("LAHF:");

            //Check for number of operands
            Tools.CheckForNumOfOperands(command, 0);

            //Create binary string
            string binaryValue = Convert.ToString(Storage.Register["AH"], 2);
            for (long i = binaryValue.Length; i < 8; i++)
                binaryValue = String.Format("{0}{1}", "0", binaryValue);
            if (Storage.DebugMode) Console.WriteLine("\tBinary Value: {0}", binaryValue);

            //Write value(s)
            Storage.Flags["SF"] = (long)Convert.ToDouble(binaryValue[0].ToString());
            Storage.Flags["ZF"] = (long)Convert.ToDouble(binaryValue[1].ToString());
            Storage.Flags["AF"] = (long)Convert.ToDouble(binaryValue[3].ToString());
            Storage.Flags["PF"] = (long)Convert.ToDouble(binaryValue[5].ToString());
            Storage.Flags["CF"] = (long)Convert.ToDouble(binaryValue[7].ToString());

            //Modify flags
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
            for (long i = 0; i < operand.Length; i++)
                operand[i] = operand[i].Trim();

            //Detect operand types
            string[] operandType = new string[operand.Length];
            for (long i = 0; i < operandType.Length; i++)
                operandType[i] = Tools.DetectOperandType(operand[i]);

            //Read operand1 and operand2 value
            long[] operandValue = new long[2];
            operandValue[0] = Tools.ReadDataFromOperand(operand[0], operandType[0]);
            operandValue[1] = Tools.ReadDataFromOperand(operand[1], operandType[1]);

            //Determine and adjust final value(s)
            long valueToWrite = Tools.AdjustValue(operandValue[0] - operandValue[1] - Storage.Flags["CF"], operandType[0], false);

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
            for (long i = 0; i < operand.Length; i++)
                operand[i] = operand[i].Trim();

            //Detect operand types
            string[] operandType = new string[operand.Length];
            for (long i = 0; i < operandType.Length; i++)
                operandType[i] = Tools.DetectOperandType(operand[i]);

            //Read operand1 and operand2 value
            long[] operandValue = new long[2];
            operandValue[0] = Tools.ReadDataFromOperand(operand[0], operandType[0]);
            operandValue[1] = Tools.ReadDataFromOperand(operand[1], operandType[1]);

            //Determine and adjust final value(s)
            long valueToWrite = Tools.AdjustValue(operandValue[0] - operandValue[1], operandType[0], false);

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
            long[] operandValue = new long[2];
            command = command.Substring(command.Split(' ')[0].Length);
            string[] operand = command.Split(',');
            for (long i = 0; i < operand.Length; i++)
                operand[i] = operand[i].Trim();

            //Detect operand types
            string[] operandType = new string[operand.Length];
            for (long i = 0; i < operandType.Length; i++)
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