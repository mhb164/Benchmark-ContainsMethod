using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace FastestContainsMethod
{
    [SimpleJob(runtimeMoniker: RuntimeMoniker.Net80, baseline: true)]
    [SimpleJob(runtimeMoniker: RuntimeMoniker.Net481)]
    [Config(typeof(Config))]
    public class ContainsMethodBenchmark
    {
        private class Config : ManualConfig
        {
            public Config()
            {
                HideColumns("Job", "Error", "StdDev", "RatioSD");
            }
        }

        [Params(100, 100_000)]
        public int ItemCount { get; set; }

        private static readonly int[] Initial = new int[] { 10, 1, 4, 5, 2, 3, 9, 7, 8, 6 };
        private int[] _Array;
        private List<int> _List;
        private HashSet<int> _HashSet;
        private ImmutableHashSet<int> _ImmutableHashSet;
        private SortedSet<int> _SortedSet;
        private ImmutableArray<int> _ImmutableArray;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _Array = new int[ItemCount];

            var i = 0;
            for (; i < _Array.Length - Initial.Length; i++)
                _Array[i] = _Array.Length - i;

            for (var j = 0; j < Initial.Length; i++, j++)
                _Array[i] = Initial[j];

            _List = new List<int>(_Array);
            _HashSet = new HashSet<int>(_Array);
            _ImmutableHashSet = ImmutableHashSet.Create(_Array);
            _SortedSet = new SortedSet<int>(_Array);

            _ImmutableArray = ImmutableArray.Create(_Array);
        }

        private static bool DoJob(ICollection<int> collection) 
            => collection.Contains(0) && collection.Contains(6);

        [Benchmark(Baseline = true)]
        public bool With_HashSet() => DoJob(_HashSet);

        [Benchmark]
        public bool With_ImmutableHashSet() => DoJob(_ImmutableHashSet);

        [Benchmark]
        public bool With_SortedSet() => DoJob(_SortedSet);

        [Benchmark]
        public bool With_List() => DoJob(_List);

        [Benchmark]
        public bool With_Array() => DoJob(_Array);

        [Benchmark]
        public bool With_ImmutableArray() => DoJob(_ImmutableArray);
    }
}
