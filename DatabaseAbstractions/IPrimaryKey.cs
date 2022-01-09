using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAbstractions
{
    public interface IPrimaryKey
    {
        public int Id { get; set; }
    }

    public abstract class PrimaryKey : IPrimaryKey
    {
        public int Id { get; set; }
    }
}
