namespace System
{
    class Storage
    {
        public static Collections.Generic.Dictionary<String, Int32> Register = new Collections.Generic.Dictionary<String, Int32>();
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
            Console.WriteLine("\nAH BH CH DH AL BL CL DL");
            Console.Write($"{0:X2} ", Register["AH"]);
            Console.Write($"{0:X2} ", Register["BH"]);
            Console.Write($"{0:X2} ", Register["CH"]);
            Console.Write($"{0:X2} ", Register["DH"]);
            Console.Write($"{0:X2} ", Register["AL"]);
            Console.Write($"{0:X2} ", Register["BL"]);
            Console.Write($"{0:X2} ", Register["CL"]);
            Console.WriteLine($"{0:X2} \n", Register["DL"]);
        }
        public static Collections.Generic.Dictionary<String, Int32> Flags = new Collections.Generic.Dictionary<String, Int32>();
        public static void FlagsInit()
        {
            Flags.Add("OF", 0);
            Flags.Add("DF", 0);
            Flags.Add("IF", 0);
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
            Console.Write($" {0}  ", Flags["OF"]);
            Console.Write($"{0}  ", Flags["DF"]);
            Console.Write($"{0}  ", Flags["IF"]);
            Console.Write($"{0}  ", Flags["TF"]);
            Console.Write($"{0}  ", Flags["SF"]);
            Console.Write($"{0}  ", Flags["ZF"]);
            Console.Write($"{0}  ", Flags["AF"]);
            Console.Write($"{0}  ", Flags["PF"]);
            Console.WriteLine($"{0}\n", Flags["CF"]);
        }
        public static Collections.Generic.Dictionary<String, Int32> Segments = new Collections.Generic.Dictionary<String, Int32>();
        public static void SegmentsInit()
        {
            Segments.Add("SS", 0);
            Segments.Add("DS", 0);
            Segments.Add("ES", 0);
        }
        public static void SegmentsDisplay()
        {
            Console.WriteLine("\n SS   DS   ES");
            Console.Write($"{0:X4} ", Segments["SS"]);
            Console.Write($"{0:X4} ", Segments["DS"]);
            Console.WriteLine($"{0:X4}\n", Segments["ES"]);
        }
        public static Collections.Generic.Dictionary<String, Int32> Pointers = new Collections.Generic.Dictionary<String, Int32>();
        public static void PointersInit()
        {
            Pointers.Add("SP", 0);
            Pointers.Add("BP", 0);
            Pointers.Add("SI", 0);
            Pointers.Add("DI", 0);
        }
        public static void PointersDisplay()
        {
            Console.WriteLine("\n SP   BP   SI   DI");
            Console.Write($"{0:X4} ", Pointers["SP"]);
            Console.Write($"{0:X4} ", Pointers["BP"]);
            Console.Write($"{0:X4} ", Pointers["SI"]);
            Console.WriteLine($"{0:X4}\n", Pointers["DI"]);
        }
    }
}