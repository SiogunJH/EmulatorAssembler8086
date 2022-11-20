namespace System
{
    class Storage
    {
        //GLOBAL VARIABLES
        public static bool ContinueSimulation = true;
        public static bool DebugMode = false;
        public static bool DoNotSaveToCode = false;
        public static bool CodeFromFile = false;

        //SAVED CODE
        public static Collections.Generic.List<string> SavedCode = new Collections.Generic.List<string>();

        public static void CodeDisplay()
        {
            Console.WriteLine();
            foreach (var line in SavedCode)
            {
                //DEFUALT
                Console.ForegroundColor = ConsoleColor.Yellow;

                //LABEL
                if (line.Split(";")[0].Trim().EndsWith(':'))
                    Console.ForegroundColor = ConsoleColor.DarkYellow;

                //DISPLAY
                Console.WriteLine(line);
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }

        //MEMORY
        public static Collections.Generic.SortedDictionary<Int64, Int64> Memory = new Collections.Generic.SortedDictionary<Int64, Int64>();

        public static void MemoryInit()
        {
            //Some sample memory
            Memory.Add(0, 0);
            Memory.Add(3, 23);
            Memory.Add(7, 55);
            Memory.Add(13, 123);
            Memory.Add(5, 5);
        }

        public static void MemoryDisplay()
        {
            Console.WriteLine();
            long i = 0;
            foreach (long key in Memory.Keys)
            {
                Console.Write("[{0:X4} -> {1:X2}]\t", key, Memory[key]);
                if (i == 3)
                {
                    Console.WriteLine();
                    i = 0;
                }
                else
                {
                    i++;
                }
            }
            Console.WriteLine("\n");
        }

        //PORTS
        public static Collections.Generic.SortedDictionary<Int64, Int64> Port = new Collections.Generic.SortedDictionary<Int64, Int64>();

        public static void PortInit()
        {
            //Some sample ports
            Port.Add(0, 0);
            Port.Add(14, 157);
            Port.Add(4, 222);
            Port.Add(78, 757);
            Port.Add(1, 15);
        }

        public static void PortDisplay()
        {
            Console.WriteLine();
            long i = 0;
            foreach (long key in Port.Keys)
            {
                Console.Write("[{0:X4} -> {1:X4}]\t", key, Port[key]);
                if (i == 3)
                {
                    Console.WriteLine();
                    i = 0;
                }
                else
                {
                    i++;
                }
            }
            Console.WriteLine("\n");
        }

        //STACK
        public static Collections.Generic.SortedDictionary<Int64, Int64> Stack = new Collections.Generic.SortedDictionary<Int64, Int64>();

        public static void StackInit()
        {
            //Default Stack
            Stack.Add(Tools.Parse("FFFE", 16), Tools.Parse("FF00", 16));
        }

        public static void StackDisplay()
        {
            Console.WriteLine();
            long i = 0;
            foreach (long key in Stack.Keys)
            {
                Console.Write("[{0:X4} -> {1:X4}]\t", key, Stack[key]);
                if (i == 3)
                {
                    Console.WriteLine();
                    i = 0;
                }
                else
                {
                    i++;
                }
            }
            Console.WriteLine("\n");
        }

        //REGISTER
        public static Collections.Generic.Dictionary<String, Int64> Register = new Collections.Generic.Dictionary<String, Int64>();
        public static void RegisterInit()
        {
            Register.Add("AH", 0);
            Register.Add("BH", 0);
            Register.Add("CH", 0);
            Register.Add("DH", 0);
            Register.Add("AL", 0);
            Register.Add("BL", 0);
            Register.Add("CL", 0);
            Register.Add("DL", 0);
        }
        public static void RegisterDisplay()
        {
            Console.WriteLine("\nAH AL BH BL CH CL DH DL");
            Console.Write("{0:X2} ", Register["AH"]);
            Console.Write("{0:X2} ", Register["AL"]);
            Console.Write("{0:X2} ", Register["BH"]);
            Console.Write("{0:X2} ", Register["BL"]);
            Console.Write("{0:X2} ", Register["CH"]);
            Console.Write("{0:X2} ", Register["CL"]);
            Console.Write("{0:X2} ", Register["DH"]);
            Console.Write("{0:X2} \n\n", Register["DL"]);
        }

        //FLAGS
        public static Collections.Generic.Dictionary<String, Int64> Flags = new Collections.Generic.Dictionary<String, Int64>();
        public static void FlagsInit()
        {
            Flags.Add("OF", 0);
            Flags.Add("DF", 0);
            Flags.Add("IF", 1);
            Flags.Add("TF", 0);
            Flags.Add("SF", 0);
            Flags.Add("ZF", 0);
            Flags.Add("AF", 0);
            Flags.Add("PF", 0);
            Flags.Add("CF", 0);
        }
        public static void FlagsDisplay()
        {
            Console.WriteLine("\nOF DF IF TF SF ZF AF PF CF");
            Console.Write(" {0}  ", Flags["OF"]);
            Console.Write("{0}  ", Flags["DF"]);
            Console.Write("{0}  ", Flags["IF"]);
            Console.Write("{0}  ", Flags["TF"]);
            Console.Write("{0}  ", Flags["SF"]);
            Console.Write("{0}  ", Flags["ZF"]);
            Console.Write("{0}  ", Flags["AF"]);
            Console.Write("{0}  ", Flags["PF"]);
            Console.WriteLine("{0}\n", Flags["CF"]);
        }

        //SEGMENTS
        public static Collections.Generic.Dictionary<String, Int64> Segments = new Collections.Generic.Dictionary<String, Int64>();
        public static void SegmentsInit()
        {
            Segments.Add("SS", 0);
            Segments.Add("DS", 0);
            Segments.Add("ES", 0);
        }
        public static void SegmentsDisplay()
        {
            Console.WriteLine("\n SS   DS   ES");
            Console.Write("{0:X4} ", Segments["SS"]);
            Console.Write("{0:X4} ", Segments["DS"]);
            Console.WriteLine("{0:X4}\n", Segments["ES"]);
        }

        //POINTERS
        public static Collections.Generic.Dictionary<String, Int64> Pointers = new Collections.Generic.Dictionary<String, Int64>();
        public static void PointersInit()
        {
            Pointers.Add("SP", Tools.Parse("FFFE", 16));
            Pointers.Add("BP", 0);
            Pointers.Add("SI", 0);
            Pointers.Add("DI", 0);
        }
        public static void PointersDisplay()
        {
            Console.WriteLine("\n SP   BP   SI   DI");
            Console.Write("{0:X4} ", Pointers["SP"]);
            Console.Write("{0:X4} ", Pointers["BP"]);
            Console.Write("{0:X4} ", Pointers["SI"]);
            Console.WriteLine("{0:X4}\n", Pointers["DI"]);
        }
    }
}