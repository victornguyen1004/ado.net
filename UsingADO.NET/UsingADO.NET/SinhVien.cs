using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace UsingADO.NET
{
    public class SinhVien
    {
        public int ID { get; set; }
        public string HoTen { get; set; }
        public string Lop { get; set; }

        public SinhVien(DataRow row)
        {
            ID = Convert.ToInt32(row["ID"]);
            HoTen = row["HoTen"].ToString();
            Lop = row["TenLop"].ToString();
        }

        public SinhVien() 
        { 
        
        }
    }
}
