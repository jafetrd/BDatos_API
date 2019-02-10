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

        public static ObservableCollection<T> IEnumerableToObservableCollection<T>(this IEnumerable<T> col)
        {
            return new ObservableCollection<T>(col);
        }

    }
}
