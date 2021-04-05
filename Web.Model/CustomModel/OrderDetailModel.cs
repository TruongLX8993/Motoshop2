using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Model.CustomModel
{
    public class OrderDetailModel : Product
    {
        public int Quantity { get; set; }
    }
}
