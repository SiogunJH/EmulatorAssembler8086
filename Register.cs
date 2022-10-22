namespace System
{
    public struct Register
    {
        public Register()
        {
            AH = 0;
            BH = 0;
            CH = 0;
            DH = 0;
            AL = 0;
            BL = 0;
            CL = 0;
            DL = 0;
        }
        public void Display()
        {
            Console.WriteLine("\nAH BH CH DH AL BL CL DL");
            Console.Write($"{this.AH:X2} ");
            Console.Write($"{this.BH:X2} ");
            Console.Write($"{this.CH:X2} ");
            Console.Write($"{this.DH:X2} ");
            Console.Write($"{this.AL:X2} ");
            Console.Write($"{this.BL:X2} ");
            Console.Write($"{this.CL:X2} ");
            Console.Write($"{this.DL:X2}\n\n");
        }
        public int AH;
        public int BH;
        public int CH;
        public int DH;
        public int AL;
        public int BL;
        public int CL;
        public int DL;
    }

}