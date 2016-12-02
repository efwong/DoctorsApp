using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsApp.Repository
{
    public interface IRepository<T>
    {
        IEnumerable<T> Get();
    }
}
