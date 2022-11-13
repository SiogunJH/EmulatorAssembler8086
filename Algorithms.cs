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
            int divident = Storage.Register["AH"] * 256 + Storage.Register["AL"]; //SMALL DIVISION
            if (operandType != "regX" && operandType != "memory") //BIG DIVISION
                divident += Storage.Register["DH"] * 256 * 256 * 256 + Storage.Register["DL"] * 256 * 256;
            int divisor = Tools.ReadDataFromOperand(operand, operandType); //BIG/SMALL DIVISION

            if (Storage.DebugMode) Console.WriteLine("\tOperand Value (Divisor): {0}", divisor);
            if (Storage.DebugMode) Console.WriteLine("\tDivident: {0}", divident);

            //Test if division is possible
            if (divisor == 0) throw new Exception("Dividing by 0 is forbidden");

            //Determine if division does not generate overflow
            int quotient = divident / divisor;
            int reminder = divident % divisor;

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
            if (Storage.DebugMode) Console.WriteLine("\tOperand Name: ", operand);

            //Detect operand types
            string operandType = Tools.DetectOperandType(operand);
            if (Storage.DebugMode) Console.WriteLine("\tOperand Type: ", operandType);

            //Check if operation is not forbidden
            if (!"regHL;regX;segment;pointer;memory".Contains(operandType))
                throw new Exception($"Operand type for {instruction} instruction should only be 'regHL', 'regX', 'pointer', 'segment' or 'memory' - recieved '{operandType}'");

            //Read value(s)
            int divident = Storage.Register["AH"] * 256 + Storage.Register["AL"]; //SMALL DIVISION
            if (operandType != "regX" && operandType != "memory") //BIG DIVISION
                divident += Storage.Register["DH"] * 256 * 256 * 256 + Storage.Register["DL"] * 256 * 256;
            int divisor = Tools.ReadDataFromOperand(operand, operandType); //BIG/SMALL DIVISION

            if (Storage.DebugMode) Console.WriteLine("\tOperand Value (Divisor): ", divisor);
            if (Storage.DebugMode) Console.WriteLine("\tDivident: ", divident);

            //Test if division is possible
            if (divisor == 0) throw new Exception("Dividing by 0 is forbidden");

            //Determine if division does not generate overflow
            int quotient = divident / divisor;
            int reminder = divident % divisor;

            if (Storage.DebugMode) Console.WriteLine("\tQuotient Raw Value: ", quotient);
            if (Storage.DebugMode) Console.WriteLine("\tReminder Raw Value: ", reminder);

            if ((operandType == "regHL" || operandType == "memory") && quotient > 255) //SMALL DIVISION
                throw new Exception(String.Format("Quotient cannot exceed FF (255). Your quotient was {0:X} ({0})", quotient));
            else if (quotient > 65535)//BIG DIVISION
                throw new Exception(String.Format("Quotient cannot exceed FFFF (65535). Your quotient was {0:X} ({0})", quotient));

            //Determine and adjust final value(s)
            quotient = Tools.AdjustValue(quotient, operandType, false);
            reminder = Tools.AdjustValue(reminder, operandType, false);

            if (Storage.DebugMode) Console.WriteLine("\tQuotient Value: ", quotient);
            if (Storage.DebugMode) Console.WriteLine("\tReminder Value: ", reminder);

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
            int multiplicator = Tools.ReadDataFromOperand(operand, operandType);
            if (Storage.DebugMode) Console.WriteLine("\tOperand Value: {0}", multiplicator);
            if ((operandType == "regHL" || operandType == "memory") && multiplicator >= 128)
                multiplicator = multiplicator - 256;
            else if (multiplicator >= 32768)
                multiplicator = multiplicator - 65536;
            if (Storage.DebugMode) Console.WriteLine("\tOperand Signed Value: {0}", multiplicator);

            int multiplicand;
            if (operandType == "regHL" || operandType == "memory")
            {
                multiplicand = Tools.ReadDataFromOperand("AL", "regHL");
                if (Storage.DebugMode) Console.WriteLine("\tMultiplicand Type: {0}", "regHL");
                if (Storage.DebugMode) Console.WriteLine("\tMultiplicand Value: {0}", multiplicand);
                if (multiplicand >= 128) multiplicand = multiplicand - 256;
            }
            else
            {
                multiplicand = Tools.ReadDataFromOperand("AX", "regX");
                if (Storage.DebugMode) Console.WriteLine("\tMultiplicand Type: {0}", "regX");
                if (Storage.DebugMode) Console.WriteLine("\tMultiplicand Value: {0}", multiplicand);
                if (multiplicand >= 32768) multiplicand = multiplicand - 65536;
            }
            if (Storage.DebugMode) Console.WriteLine("\tMultiplicand Signed Value: {0}", multiplicand);

            //Determine value(s)
            int product = multiplicand * multiplicator;
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
                int productOG = product;
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
            int operandValue = Tools.ReadDataFromOperand(operand, operandType);
            if (Storage.DebugMode) Console.WriteLine("\tOperand Value: {0}", operandValue);

            //Update value(s)
            int maxValue;
            if (operandType == "regHL" || operandType == "memory") maxValue = 256;
            else maxValue = 65536;
            if (Storage.DebugMode) Console.WriteLine("\tMax Value: {0}", maxValue);

            //Determine and adjust final value(s)
            int valueToWrite = maxValue - operandValue;
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
            int operandValue = Tools.ReadDataFromOperand(operand, operandType);
            if (Storage.DebugMode) Console.WriteLine("\tOperand Value: {0}", operandValue);

            //Negate all bits
            int valueToWrite;
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
            int multiplicator = Tools.ReadDataFromOperand(operand, operandType);
            if (Storage.DebugMode) Console.WriteLine("\tOperand Value: {0}", multiplicator);

            int multiplicand;
            if (operandType == "regHL" || operandType == "memory")
                multiplicand = Tools.ReadDataFromOperand("AL", "regHL");
            else
                multiplicand = Tools.ReadDataFromOperand("AX", "regX");
            if (Storage.DebugMode) Console.WriteLine("\tMultiplicand Value: {0}", multiplicand);

            //Determine value(s)
            int product = multiplicand * multiplicator;
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
                int productOG = product;
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