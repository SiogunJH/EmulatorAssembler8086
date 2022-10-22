namespace System
{
    public struct Pointers
    {
        public Pointers()
        {
            SP = 0;
            BP = 0;
            SI = 0;
            DI = 0;
        }
        public void Display()
        {
            Console.WriteLine("\n SP   BP   SI   DI");
            Console.Write($"{this.SP:X4} ");
            Console.Write($"{this.BP:X4} ");
            Console.Write($"{this.SI:X4} ");
            Console.Write($"{this.DI:X4}\n\n");
        }
        public int SP;
        public int BP;
        public int SI;
        public int DI;
    }

}