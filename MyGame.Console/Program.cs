using System;
using System.Threading;
using System.Text;

class Program
{
    static void Main(String[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        GameManager.Instance.Run();
    }
}