using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpolatedParser.Utilities
{
    public static class UnsafeUtils
    {
        public static unsafe ref T AsRef<T>(in T value)
        {
#pragma warning disable CS8500 // 这会获取托管类型的地址、获取其大小或声明指向它的指针
            fixed (T* p = &value)
            {
                return ref *p;
            }
#pragma warning restore CS8500 // 这会获取托管类型的地址、获取其大小或声明指向它的指针
        }
    }
}
