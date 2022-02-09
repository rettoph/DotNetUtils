using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minnow.System.Helpers
{
    public static class MethodHelper
    {
        public static Boolean FailResponse<T>(out T instance)
        {
            instance = default;
            return false;
        }
    }
}
