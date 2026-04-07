using System.Data;
using Microsoft.Data.SqlClient;

public class CtrlKelolaUser: UserControl
{
    DbConn db = new DbConn();

    int? id = null;

    ComboBox cmbTipe = new ComboBox{Anchor = AnchorStyles.Left | AnchorStyles.Right, Items = {"Gudang", "Kasir"}, SelectedIndex = 0};
    TextBox tbUsername = new TextBox{Anchor = AnchorStyles.Left | AnchorStyles.Right, PlaceholderText = "Username"};
    TextBox tbnNamaUser = new TextBox{Anchor = AnchorStyles.Left | AnchorStyles.Right, PlaceholderText = "Nama User"};
    TextBox tbPassword = new TextBox{Anchor = AnchorStyles.Left | AnchorStyles.Right, PlaceholderText = "Password"};
    TextBox tTelepon = new TextBox{Anchor = AnchorStyles.Left | AnchorStyles.Right, PlaceholderText = "Telepon"};

    Button btnTambah = new Button{Text = "Tambah", Height = 40, Width = 90 };
    Button btnEdit = new Button{Text = "Edit", Height = 40, Width = 80 };
    Button btnHapus = new Button{Text = "Hapus", Height = 40, Width = 80 };

    RichTextBox rtbAlamat = new RichTextBox{Dock = DockStyle.Fill,};


    DataGridView dgvUsers = new DataGridView{Dock = DockStyle.Fill, AllowUserToAddRows = false, RowHeadersVisible = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
    SelectionMode = DataGridViewSelectionMode.FullRowSelect};
    public CtrlKelolaUser()
    {
        btnHapus.Click += btnHapus_CLick;
        btnTambah.Click += btnTambah_CLick;
        dgvUsers.CellClick += dgvUsers_CellClick;

        var rootLabel = new TableLayoutPanel{Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 2, 
        RowStyles = {new RowStyle(SizeType.Percent, 20), new RowStyle(SizeType.Percent, 80)},
            ColumnStyles = {
                new ColumnStyle(SizeType.Percent, 50),
                new ColumnStyle(SizeType.Percent, 50)
            },
            Controls = {new Label {Dock = DockStyle.Fill, Text = "Pages/Dashboard"}, new Panel(),
            new Label{Dock = DockStyle.Fill, Text = "Log Activity", Font = new Font("FontFamily", 18)}}
        };

        var rootForm = new TableLayoutPanel{Dock = DockStyle.Fill, RowCount = 4, ColumnCount = 2,
            RowStyles = {
                new RowStyle(SizeType.Percent, 20),
                new RowStyle(SizeType.Percent, 20),
                new RowStyle(SizeType.Percent, 20),
                new RowStyle(SizeType.Percent, 40),
            },
            ColumnStyles =
            {
                new ColumnStyle(SizeType.Percent, 40), new ColumnStyle(SizeType.Percent, 40),
            },
            Controls =
            {
                cmbTipe, tbUsername, tbnNamaUser, tbPassword, tTelepon,
                new FlowLayoutPanel{Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight,
                 Controls =
                    {
                        btnTambah, btnEdit, btnHapus
                    }
                }, rtbAlamat
            }
        };  
        rootForm.SetColumnSpan(rtbAlamat, 2);

        var root = new TableLayoutPanel{Dock = DockStyle.Fill, RowCount = 3, ColumnCount = 1,
        ColumnStyles = {new ColumnStyle(SizeType.Percent, 100)},
        RowStyles = {new RowStyle(SizeType.Percent, 20), new RowStyle(SizeType.Percent, 40), new RowStyle(SizeType.Percent, 40)},
        Controls =
            {
                rootLabel, rootForm, dgvUsers
            }
        };

        this.Controls.Add(root);
        loadUsers();
    }

    private void loadUsers()
    {
        DataTable dt = db.Query(@"
            select id, tipe_user, nama, alamat, telepon, password from users
        ");

        dgvUsers.DataSource = dt;
        dgvUsers.Columns["password"].Visible = false;
    }



    private void dgvUsers_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if(e.RowIndex >= 0)
        {
            DataGridViewRow row = dgvUsers.Rows[e.RowIndex];

            cmbTipe.SelectedItem = row.Cells[1].Value.ToString();
            tbnNamaUser.Text = row.Cells[2].Value!.ToString();
            tbPassword.Text = row.Cells[5].Value!.ToString();
            tbUsername.Text = row.Cells[2].Value!.ToString();
            tTelepon.Text = row.Cells[4].Value!.ToString();
            rtbAlamat.Text = row.Cells[3].Value!.ToString();
            id = Convert.ToInt32(row.Cells[0].Value);
        }
    }

    private void btnTambah_CLick(object? sender, EventArgs e) 
    {
        if(
            !string.IsNullOrEmpty(tbnNamaUser.Text) &&
            !string.IsNullOrEmpty(tbPassword.Text) &&
            !string.IsNullOrEmpty(tbUsername.Text) &&
            !string.IsNullOrEmpty(tTelepon.Text) &&
            !string.IsNullOrEmpty(rtbAlamat.Text) 
        )
        {
            SqlParameter[] param =
            {
                new SqlParameter("@tipe", cmbTipe.Text),
                new SqlParameter("@nama", tbnNamaUser.Text),
                new SqlParameter("@password", tbPassword.Text),
                new SqlParameter("@email", tbUsername.Text + "@gmail.com"),
                new SqlParameter("@telepon", tTelepon.Text),
                new SqlParameter("@alamat", rtbAlamat.Text),
            };

            int result = db.Execute(@"
                insert into users (tipe_user, nama, alamat, email, telepon, password)
                values(@tipe, @nama, @alamat, @email, @telepon, @password)
            ", param);

            if(result > 0)
            {
                MessageBox.Show("User berhasil ditambahkan");
                loadUsers();
            } 
        } else
        {
            MessageBox.Show("Isi semua form terlebih dahulu");
        }
    }

    private void btnEdit_CLick(object? sender, EventArgs e) 
    {
        if(
            !string.IsNullOrEmpty(tbnNamaUser.Text) &&
            !string.IsNullOrEmpty(tbPassword.Text) &&
            !string.IsNullOrEmpty(tbUsername.Text) &&
            !string.IsNullOrEmpty(tTelepon.Text) &&
            !string.IsNullOrEmpty(rtbAlamat.Text) 
        )
        {
            SqlParameter[] param =
            {
                new SqlParameter("@tipe", cmbTipe.Text),
                new SqlParameter("@nama", tbnNamaUser.Text),
                new SqlParameter("@password", tbPassword.Text),
                new SqlParameter("@email", tbUsername.Text + "@gmail.com"),
                new SqlParameter("@telepon", tTelepon.Text),
                new SqlParameter("@alamat", rtbAlamat.Text),
            };

            int result = db.Execute(@"
                update users set tipe_user = @tipe, nama = @nama, alamat = @alamat, email = @email, telepon = @telepon, 
                password = @password where id = @id
            ", param);

            if(result > 0)
            {
                MessageBox.Show("User berhasil ditambahkan");
                loadUsers();
            } 
        } else
        {
            MessageBox.Show("Isi semua form terlebih dahulu");
        }
    }

    private void btnHapus_CLick(object? sender, EventArgs e)
    {
        if(id == null)
        {
            MessageBox.Show("Tidak Ada user yang dipilih");
        } else
        {
            SqlParameter[] param =
            {
                new SqlParameter("@id", id)
            };

            int result = db.Execute(@"
                delete from users where id  = @id
            ", param);

            if(result > 0 )
            {
                MessageBox.Show("Data berhasil dihapus");
                loadUsers();
            } else
            {
                MessageBox.Show("Terjadi Kesalahan");
            }
        }
    }
}