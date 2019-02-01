using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BDatos_API
{


    public static class CollectionUtils
    {
        public static Collection<T> IEnumerableToCollection<T>(this IEnumerable<T> data)
        {
            return new Collection<T>(data.ToList());
        }
    }
}
