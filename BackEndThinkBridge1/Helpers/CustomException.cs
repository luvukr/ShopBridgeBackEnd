using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopBridgeBackend.Helpers
{
    public class CustomException
    {
        public class  NotFoundException <T> :Exception where T: class
        {
            public NotFoundException() : base(typeof(T).Name + " not found.")
            {
            }

        }

        public class GeneralErrorMessage : Exception 
        {
            public GeneralErrorMessage() : base("Something wrong happened. It's us not you."+Environment.NewLine+" Please visit us after some time. We are on it.")
            {
            }
            public GeneralErrorMessage(string message) : base(message)
            {
            }

        }

    }
}