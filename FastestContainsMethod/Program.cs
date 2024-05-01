using BenchmarkDotNet.Running;

namespace FastestContainsMethod
{
    public class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<ContainsMethodBenchmark>();
        }
    }
}
