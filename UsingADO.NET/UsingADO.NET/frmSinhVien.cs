using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace UsingADO.NET
{
    public partial class frmSinhVien : Form
    {
        public List<Lop> dsLop = new List<Lop>();
        public List<SinhVien> dsSV = new List<SinhVien>(); 
        public string connectionString = "Data Source=DESKTOP-I8J7HE7;Initial Catalog=QLSinhVien;Integrated Security=True";


        public frmSinhVien()
        {
            InitializeComponent();
        }

        private void frmSinhVien_Load(object sender, EventArgs e)
        {
            getAllLop();
            LoadComboBox(dsLop);
            GetAllSinhVien();
            LoadListView(dsSV);
            ShowDefaultSinhVien();
            
        }

        private void getAllLop()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * " +
                "FROM Lop";
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable lopTable = new DataTable();
            adapter.Fill(lopTable);
            connection.Close();
            foreach (DataRow row in lopTable.Rows)
            {
                Lop _lop = new Lop(row);
                dsLop.Add(_lop);
            }
        }

        private void GetAllSinhVien()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT a.ID, HoTen, TenLop " + "FROM SinhVien a, Lop b " + "WHERE a.MaLop = b.ID";
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable svTable = new DataTable();
            adapter.Fill(svTable);
            adapter.Dispose();
            connection.Close();
            foreach (DataRow row in svTable.Rows)
            {
                SinhVien sv = new SinhVien(row);
                dsSV.Add(sv);
            }
        }

        private void LoadComboBox(List<Lop> dslop)
        {
            foreach(Lop lop in dslop)
            {
                string _tenLop = lop.TenLop.ToString();
                cboLop.Items.Add(_tenLop);
            }
        }

        private void LoadListView(List<SinhVien> dssv)
        {
            lvSinhVien.Items.Clear();
            foreach(SinhVien sv in dssv)
            {
                AddSinhVienToLV(sv);
            }
        }

        private void AddSinhVienToLV(SinhVien sv)
        {
            ListViewItem lvItem = new ListViewItem(sv.ID.ToString());
            lvItem.SubItems.Add(sv.HoTen);
            lvItem.SubItems.Add(sv.Lop);
            lvSinhVien.Items.Add(lvItem);
        }

        private void btnMacDinh_Click(object sender, EventArgs e)
        {
            this.txtID.Text = "";
            this.txtTen.Text = "";
            this.cboLop.SelectedIndex = -1;
        }

        private SinhVien GetSinhVienFromLV(ListViewItem lvItem)
        {
            SinhVien kq = new SinhVien();
            kq.ID = Convert.ToInt32(lvItem.SubItems[0].Text);
            kq.HoTen = lvItem.SubItems[1].Text;
            kq.Lop = lvItem.SubItems[2].Text;
            return kq;
        }

        private void DisplaySinhVien(SinhVien sv)
        {
            this.txtID.Text = sv.ID.ToString();
            this.txtTen.Text = sv.HoTen;
            this.cboLop.Text = sv.Lop;
        }

        private void ShowDefaultSinhVien()
        {
            SinhVien sv = dsSV[0];
            this.txtID.Text = sv.ID.ToString();
            this.txtTen.Text = sv.HoTen;
            this.cboLop.Text = sv.Lop;
        }

        private void lvSinhVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lvSinhVien.SelectedItems.Count > 0)
            {
                DisplaySinhVien(GetSinhVienFromLV(lvSinhVien.SelectedItems[0]));
            }
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            GetAllSinhVien();
            LoadListView(dsSV);
        }

        private List<SinhVien> TimMaSinhVien(string id)
        {
            List<SinhVien> kqTim = new List<SinhVien>();
            kqTim = this.dsSV.FindAll(x => x.ID.ToString() == id);
            return kqTim;
        }

        private void HienThiKetQuaTim(List<SinhVien> list)
        {
            lvSinhVien.Items.Clear();
            foreach (SinhVien sv in list)
            {
                AddSinhVienToLV(sv);
            }
        }

        private void txtTim_TextChanged(object sender, EventArgs e)
        {
            maTim = txtTim.Text;
        }
        public string maTim;

        private void btnTim_Click(object sender, EventArgs e)
        {
            HienThiKetQuaTim(TimMaSinhVien(maTim));

        }

        private void txtTim_TextChanged_1(object sender, EventArgs e)
        {
            LoadListView(dsSV.FindAll(x => x.HoTen.Contains(txtTim.Text)));
        }

        // Hàm find trả về đối tượng đầu tiên đúng điều kiện, sau đó .ID để lấy mã lớp của đối tượng trả về hàm FindClassID
        private int FindLopID(string tenLop) => dsLop.Find(c => c.TenLop.CompareTo(tenLop) == 0).ID;

        private bool EditSinhVien()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE SinhVien" +
                                  " SET HoTen = N'" + this.txtTen.Text +
                                  "', MaLop = " + FindLopID(this.cboLop.Text) +
                                  " WHERE ID = " + this.txtID.Text;
           int rowsAffected = command.ExecuteNonQuery();
           connection.Close();
            if (rowsAffected > 0)
                return true;
            return false;
        }

        private bool AddSinhVien(SinhVien sv)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = connection.CreateCommand();
            connection.Open();
            command.CommandText = "INSERT INTO SinhVien VALUES (N'" + sv.HoTen + "', " +
                                     FindLopID(sv.Lop) + ")";
            int rowsAffected = command.ExecuteNonQuery();
            connection.Close();
            if (rowsAffected > 0)
                return true;
            return false;
        }

        private SinhVien GetSinhVien()
        {
            SinhVien sv = new SinhVien();
            sv.ID = Convert.ToInt32(this.txtID.Text);
            sv.HoTen = this.txtTen.Text;
            sv.Lop = this.cboLop.Text;
            return sv;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            SinhVien sv = GetSinhVien();
            if (sv != null)
            {
                if (this.txtID.Text != "")
                {
                    if (EditSinhVien(Convert.ToInt32(this.txtID.Text), sv))
                        MessageBox.Show("Đã cập nhật", "Thông báo", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                }
                else if (AddSinhVien(sv))
                    MessageBox.Show("Đã thêm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.lvSinhVien.Items.Clear();
                GetAllSinhVien();
                LoadListView(dsSV);
            }
            else
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private bool EditSinhVien(int id, SinhVien sv)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = "UPDATE SinhVien" +
                                     " SET HoTen = N'" + this.txtTen.Text +
                                     "', MaLop = " + FindLopID(this.cboLop.Text) +
                                     " WHERE ID = " + this.txtID.Text;
            sqlConnection.Open();
            int numberOfRowAffected = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            if (numberOfRowAffected != 0)
                return true;
            return false;
        }
    }
}
