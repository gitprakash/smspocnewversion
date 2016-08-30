using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
namespace DataServiceLibrary
{
    public static class ConcurrentBagExtension
    {
        public static  void AddRange<T>(this ConcurrentBag<T> cb,ConcurrentBag<T> cbinput)
        {
            foreach (var item in cbinput) cb.Add(item);
        }
    }
}
