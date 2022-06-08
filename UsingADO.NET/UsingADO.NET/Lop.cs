using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsingADO.NET
{
    public class Lop
    {
        public int ID { get; set; }
        public string TenLop { get; set; }

        public Lop(DataRow row)
        {
            this.ID = Convert.ToInt32(row["ID"]);
            this.TenLop = row["TenLop"].ToString();
        }
            
    }
}
