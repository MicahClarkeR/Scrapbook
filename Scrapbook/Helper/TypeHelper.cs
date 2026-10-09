using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Helper
{
    public static class TypeHelper
    {
        public static bool ConfirmType(object obj, Type type)
        {
            Type objType = obj.GetType();

            if (objType != type || !objType.IsSubclassOf(type))
                return false;

            return true;
        }
    }
}
