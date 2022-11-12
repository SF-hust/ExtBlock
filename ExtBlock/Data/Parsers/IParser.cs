using System;
using System.Collections.Generic;
using System.Text;

namespace ExtBlock.Data.Parsers
{
    public interface IParser
    {
    }

    public interface IParser<I, O>
    {
        bool Parse(I input, out O output);
    }
}
