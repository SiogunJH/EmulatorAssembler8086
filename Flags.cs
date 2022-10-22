namespace System
{
    public struct Flags
    {
        public Flags()
        {
            OF = false;
            DF = false;
            IF = false;
            TF = false;
            SF = false;
            ZF = false;
            AF = false;
            PF = false;
            CF = false;
        }
        public void Display()
        {
            Console.WriteLine("\nOF DF IF TF SF ZF AF PF CF");
            Console.Write($" {(this.OF ? '1' : '0')}");
            Console.Write($"  {(this.DF ? '1' : '0')}");
            Console.Write($"  {(this.IF ? '1' : '0')}");
            Console.Write($"  {(this.TF ? '1' : '0')}");
            Console.Write($"  {(this.SF ? '1' : '0')}");
            Console.Write($"  {(this.ZF ? '1' : '0')}");
            Console.Write($"  {(this.AF ? '1' : '0')}");
            Console.Write($"  {(this.PF ? '1' : '0')}");
            Console.Write($"  {(this.CF ? '1' : '0')}\n\n");
        }
        public bool OF;
        public bool DF;
        public bool IF;
        public bool TF;
        public bool SF;
        public bool ZF;
        public bool AF;
        public bool PF;
        public bool CF;
    }

}