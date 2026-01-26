using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOAD_Project.Patterns
{
    public interface IMenuComponent
    {
        string Name { get; }
        decimal GetPrice();
    }
}
