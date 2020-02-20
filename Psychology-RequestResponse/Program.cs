using System;

namespace Psychology_RequestResponse
{
    class Program
    {
        static void Main(string[] args)
        {
            Rabbit.Rabbit rabbit = new Rabbit.Rabbit();
            rabbit.Received();
        }
    }
}
