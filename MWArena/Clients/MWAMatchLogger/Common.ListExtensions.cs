using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public static class ListExtenstions
    {
        /// <summary>for each item in the list it appends a formatted string with the item in the argsIndex.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objList">The obj list.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.ArgumentNullException">format</exception>
        public static string FormatList<T>(this IList<T> objList, string format, params object[] args)
        {
            object[] argsClone = args.Select(a => a).ToArray();
            var sb = new StringBuilder();
            format = format + Environment.NewLine;
            foreach (var o in objList)
            {
                int i = 0;
                foreach (var o1 in args)
                {
                    if (o1.ToString().StartsWith("."))
                    {
                        var pname = o1.ToString().Remove(0, 1);
                        if (pname == "TypeName")
                        {
                            argsClone[i] = o.GetType().AssemblyQualifiedName;
                        }
                        else
                        {
                            if (o.GetType().GetProperty(o1.ToString().Remove(0, 1)) != null)
                            {
                                argsClone[i] = o.GetType().GetProperty(o1.ToString().Remove(0, 1)).GetValue(o, null);
                            }
                        }
                    }
                    i++;
                }
                sb.Append(String.Format(format, argsClone));
            }

            return sb.ToString();
        }
    }
}