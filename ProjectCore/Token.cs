using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore
{
    public class Token
    {
        public int tag;

        public Token(int tag)
        {
            this.tag = tag;
        }
    }

    public class Num : Token
    {
        public int value;
        public Num(int tag, int value) : base(tag)
        {
            this.value = value;
        }
    }

    //TODO
}
