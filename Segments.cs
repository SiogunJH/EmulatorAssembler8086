namespace System
{
    public struct Segments
    {
        public Segments()
        {
            SS = 0;
            ES = 0;
            DS = 0;
        }
        public void Display()
        {
            Console.WriteLine("\n SS  DS  ES");
            Console.Write($"{this.SS:X4} ");
            Console.Write($"{this.DS:X4} ");
            Console.Write($"{this.ES:X4}\n\n");
        }
        public int SS;
        public int DS;
        public int ES;
    }

}