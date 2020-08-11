using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Exceptions
{
    public class NonValidatedGoogleIdTokenException:Exception
    {
        public override string Message { get { return "IdToken is not valide"; } }
    }
}
