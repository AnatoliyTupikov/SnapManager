using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapManager.Models.WPFModels.Hierarchy
{
    public interface ICopy<T> where T : ICopy<T>
    {
         public T Clone();

        public void CopyValueFrom(T source);
    }
}
