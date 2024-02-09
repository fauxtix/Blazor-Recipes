using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Receitas_API.Data
{
    public class SqlConnectionConfiguration : ISqlConnectionConfiguration
    {
        public SqlConnectionConfiguration(string value) => Value = value;
        public string Value { get; }
    }
}
