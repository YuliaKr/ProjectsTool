using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectsTool.Classes
{
    public static class MapReduce
    {
        public static IEnumerable<T2> Map<T1, T2>(IEnumerable<T1> collection, Func<T1, T2> transformation)
        {
            T2[] result = new T2[collection.Count()];
            for (int i = 0; i < collection.Count(); i++)
            {
                result[i] = transformation(collection.ElementAt(i));
            }
            return result;
        }

        public static IEnumerable<T> Where<T>(IEnumerable<T> collection, Func<T, bool> condition)
        {
            List<T> result = new List<T>();

            for (int i = 0; i < collection.Count(); i++)
            {
                if (condition(collection.ElementAt(i)))
                {
                    result.Add(collection.ElementAt(i));
                }
            }
            return result;
        }

        public static T2 Reduce<T1,T2>(IEnumerable<T1> collection, T2 init, Func<T2,T1,T2> operation)
        {
            T2 result = init;
            for (int i = 0; i < collection.Count(); i++)
            {
                result = operation(result, collection.ElementAt(i));
            }
            return result;
        }

        public static IEnumerable<Tuple<T1,T2>> Join<T1,T2>(IEnumerable<T1> table1,IEnumerable<T2> table2, Func<Tuple<T1,T2>,bool> condition)
        {
            return
                Reduce(table1, new List<Tuple<T1,T2>>(), (queryResult, x) =>
                {
                    List<Tuple<T1, T2>> combination =
                    Reduce(table2, new List<Tuple<T1, T2>>(), (c, y) =>
                     {
                         Tuple<T1, T2> row = new Tuple<T1, T2>(x, y);
                         if (condition(row))
                         {
                             c.Add(row);
                         }
                         return c;
                     });
                    queryResult.AddRange(combination);
                    return queryResult;
                });
        }
    }
}