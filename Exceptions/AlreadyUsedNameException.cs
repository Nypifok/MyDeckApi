using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Exceptions
{
    public class AlreadyUsedNameException:Exception
    {
        public override string Message { get { return "Name already used"; } }
    }
}
