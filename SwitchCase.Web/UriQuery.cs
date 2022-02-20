using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using SwitchCase.Core.Extensions;

namespace SwitchCase.Web
{
    public class UriQuery
    {
        private readonly Dictionary<string, StringValues> query;

        public UriQuery (Uri uri)
        {
            query = QueryHelpers.ParseQuery(uri.Query);
        }

        public IEnumerable<T> GetValues<T>(string key, T defaultValue)
        {
            return query.ContainsKey(key) ? query[key].Select(s=>s.Parse(defaultValue)) : Enumerable.Empty<T>();
        }

        public T GetValue<T>(string key, T defaultValue)
        {
            return query.ContainsKey(key) ? query[key].First().Parse(defaultValue) : defaultValue;
        }
    }
}
