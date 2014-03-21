using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Graphs.lib.DataStructure;
namespace Graphs.lib.Algorithms
{
    public  interface IAlgorithm<T>
        where T:IComparable<T>
    {
        Graph<T> Graph { get; set; }
        void Run();
    }
}
