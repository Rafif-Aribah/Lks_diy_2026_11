using System.Data;
using Microsoft.Data.SqlClient;

public class FormAdmin: Form 
{
    DbConn db = new DbConn();

    Button btnKelolaUser = new Button{Anchor = AnchorStyles.None, Height = 40, Width = 100, Text = "Kelola user", Dock = DockStyle.Fill,};
    Button btnKelolaLaporan = new Button{Anchor = AnchorStyles.None, Height = 40, Width = 100, Text = "Kelola Laporan", Dock = DockStyle.Fill,};
    Button btnLogActivity = new Button{Anchor = AnchorStyles.None, Height = 40, Width = 100, Text = "Log Activity", Dock = DockStyle.Fill,};
    Button btnLogout = new Button{Anchor = AnchorStyles.None, Height = 40, Width = 80, Text = "Logout",};

    DateTimePicker dtpFrom = new DateTimePicker{Anchor = AnchorStyles.Left | AnchorStyles.Right,};
    DateTimePicker dtpEnd = new DateTimePicker{Anchor = AnchorStyles.Left | AnchorStyles.Right,};
    Button btnFilter = new Button{Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom, Height = 40, Width = 80, Text ="Filter", BackColor = Color.CadetBlue, ForeColor = Color.White};

    DataGridView dgvActivity = new DataGridView{Dock = DockStyle.Fill, AllowUserToAddRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode
    .Fill, RowHeadersVisible = false};
    public FormAdmin()
    {
        btnFilter.Click += btnFilter_Click;

        this.Size = new Size(1000, 700);

        var rootLogActivity = new TableLayoutPanel
        {
            Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 3,
            ColumnStyles =
            {
                new ColumnStyle(SizeType.Percent, 20),
                new ColumnStyle(SizeType.Percent, 20),
                new ColumnStyle(SizeType.Percent, 15),
                new ColumnStyle(SizeType.Percent, 45),
            },
            RowStyles =
            {
                new RowStyle(SizeType.Percent, 20),  
                new RowStyle(SizeType.Percent, 20),  
                new RowStyle(SizeType.Percent, 60),    
            },
            Controls =
            {
                new TableLayoutPanel{Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 2, 
                    RowStyles = {new RowStyle(SizeType.Percent, 20), new RowStyle(SizeType.Percent, 80)},
                    ColumnStyles = {
                        new ColumnStyle(SizeType.Percent, 50),
                        new ColumnStyle(SizeType.Percent, 50)
                    },
                    Controls = {new Label {Dock = DockStyle.Fill, Text = "Pages/Dashboard"}, new Panel(),
                    new Label{Dock = DockStyle.Fill, Text = "Log Activity", Font = new Font("FontFamily", 18)}}
                },
                new TableLayoutPanel{Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 2, 
                    RowStyles = {new RowStyle(SizeType.Percent, 50), new RowStyle(SizeType.Percent,50)},
                    ColumnStyles = {
                        new ColumnStyle(SizeType.Percent, 40),
                        new ColumnStyle(SizeType.Percent, 40),
                        new ColumnStyle(SizeType.Percent, 20),
                    },
                    Controls =
                    {
                        new Label {Text = "Dari Tanggal", Dock = DockStyle.Fill},
                        new Label {Text = "Sampai Tanggal", Dock = DockStyle.Fill}, new Panel(),
                        dtpFrom, dtpEnd, btnFilter
                    }
                },
                dgvActivity
            }
        };rootLogActivity.SetColumnSpan(rootLogActivity.Controls[0], 4);
        rootLogActivity.SetColumnSpan(rootLogActivity.Controls[1], 4);
        rootLogActivity.SetColumnSpan(dgvActivity, 4);

        var rootSideBar = new TableLayoutPanel{Dock = DockStyle.Fill, RowCount = 3, ColumnCount = 1, ColumnStyles = {new ColumnStyle(SizeType.Percent, 100)},
            RowStyles = {new RowStyle(SizeType.Percent, 50), new RowStyle(SizeType.Percent, 30), new RowStyle(SizeType.Percent, 20)},
            Controls =
            {
                new TableLayoutPanel{Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2,ColumnStyles = {new ColumnStyle(SizeType.Percent, 100)},
                    RowStyles = {new RowStyle(SizeType.Percent, 60), new RowStyle(SizeType.Percent, 40)},
                    Controls =
                    {
                        new Panel{Size = new Size(100, 100), BackColor = Color.LightGray, Anchor = AnchorStyles.Bottom},
                        new Label{Text = "Admin", ForeColor = Color.White, Font = new Font("FontFamily", 16),Dock = DockStyle.Fill, TextAlign = ContentAlignment.TopCenter}
                    }
                },
                new TableLayoutPanel{Dock = DockStyle.Fill, RowCount = 3, ColumnCount = 1, ColumnStyles = {new ColumnStyle(SizeType.Percent, 100)},
                    RowStyles = {new RowStyle(SizeType.Percent, 33), new RowStyle(SizeType.Percent, 33), new RowStyle(SizeType.Percent, 34)},
                    Controls =
                    {
                         btnKelolaUser, btnKelolaLaporan, btnLogActivity
                    }
                },
                btnLogout
            }
        };

        var root = new TableLayoutPanel{Dock = DockStyle.Fill, RowCount = 1, ColumnCount = 2, RowStyles = {new RowStyle(SizeType.Percent, 100)},
        ColumnStyles =
            {
                new ColumnStyle(SizeType.Percent, 35),
                new ColumnStyle(SizeType.Percent, 65),
            },
        Controls =
            {
                rootSideBar, rootLogActivity
            }
        };

        this.Controls.Add(root);
        loadDgvActivity();
    }

    private void loadDgvActivity()
    {
        DataTable dt = db.Query(@"
            select * from tbl_log
        ");

        dgvActivity.DataSource = dt;
    }


    private void btnFilter_Click(object? sender, EventArgs e)
    {
        if(dtpFrom.Value > dtpEnd.Value)
        {
            MessageBox.Show("date from tidak boleh melebihi date end");
        } else
        {
            SqlParameter[] param =
            {
              new SqlParameter("@tglawal", dtpFrom.Value),
              new SqlParameter("@tglakhir", dtpEnd.Value.AddDays(1).AddSeconds(-1)),
            };

           DataTable dt = db.Query(@"
                select * from tbl_log where waktu >= @tglawal and waktu <= @tglakhir
            ", param);

            dgvActivity.DataSource = dt;
        }
    }
}