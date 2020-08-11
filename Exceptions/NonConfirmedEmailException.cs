using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Exceptions
{
    public class NonConfirmedEmailException:Exception
    {
        public override string Message { get { return "Email is not confirmed";} }
    }
}
