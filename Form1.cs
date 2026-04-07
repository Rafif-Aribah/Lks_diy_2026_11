using System.Data;
using Microsoft.Data.SqlClient;

namespace Lks_diy_2026_11;

public partial class Form1 : Form
{   
    DbConn db = new DbConn();

    TextBox tbEmail = new TextBox{Anchor = AnchorStyles.Left | AnchorStyles.Right, BorderStyle = BorderStyle.FixedSingle};
    TextBox tbPassword = new TextBox{Anchor = AnchorStyles.Left | AnchorStyles.Right, BorderStyle = BorderStyle.FixedSingle};

    Button btnLogin = new Button{Anchor = AnchorStyles.Left | AnchorStyles.Right, Text = "Login", Height = 50};
    Button btnReset = new Button{Anchor = AnchorStyles.Left | AnchorStyles.Right, Text = "Reset", Height = 50};
    
    public Form1()
    {
        btnLogin.Click += btnLogin_CLick;
        
        this.Size = new Size(1000, 700);
        this.Text = "DIY sMART";

        var formLogin = new TableLayoutPanel{Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 3, Padding = new Padding(16),
            RowStyles = {
                new RowStyle(SizeType.Percent, 20),
                new RowStyle(SizeType.Percent, 40),
                new RowStyle(SizeType.Percent, 20),
            },
            ColumnStyles = {
                new ColumnStyle(SizeType.Percent, 50), new ColumnStyle(SizeType.Percent, 50)
            },
            Controls =
            {
                new TableLayoutPanel{Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, RowStyles = {new RowStyle(SizeType.Percent
                , 60), new RowStyle(SizeType.Percent, 40)}, ColumnStyles = {new ColumnStyle(SizeType.Percent, 100)},
                Controls = {
                    new Label{Font = new Font(Font.FontFamily, 18), Text = "Log In", Dock = DockStyle.Fill},
                    new Label{Text = "Enter your email and password to Log In", Dock = DockStyle.Fill}
                }},

                new TableLayoutPanel{Dock = DockStyle.Fill, RowCount = 4, ColumnCount = 1, ColumnStyles = {new ColumnStyle(SizeType.Percent,100)},
                    RowStyles = {
                        new RowStyle(SizeType.Percent, 25),
                        new RowStyle(SizeType.Percent, 25),
                        new RowStyle(SizeType.Percent, 25),
                        new RowStyle(SizeType.Percent, 25),
                    },
                    Controls =
                    {
                        new Label{Text = "Email*", Anchor = AnchorStyles.Left | AnchorStyles.Bottom}, tbEmail,
                        new Label{Text = "Password*", Anchor = AnchorStyles.Left | AnchorStyles.Bottom}, tbPassword
                    }
                }, new Panel(),
                btnLogin, btnReset
            }
        };
        formLogin.SetColumnSpan(formLogin.Controls[0], 2);

        var rootSideBar = new TableLayoutPanel{Anchor = AnchorStyles.None, Size = new Size(200, 200),RowCount = 2, ColumnCount = 1, ColumnStyles = {
            new ColumnStyle(SizeType.Percent, 100)}, 
            RowStyles = {new RowStyle(SizeType.Percent, 50), new RowStyle(SizeType.Percent, 50)},
            Controls =
            {
                new Label{Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Text ="DIY sMART", Font = new Font("FontFamily", 16)},
                new Panel{Size = new Size(90, 90), Anchor = AnchorStyles.None, BackColor = Color.Gray }
            }
        };

        var root = new TableLayoutPanel{Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1,
        RowStyles ={new RowStyle(SizeType.Percent, 100)}, 
        ColumnStyles = {new ColumnStyle(SizeType.Percent, 50), new ColumnStyle(SizeType.Percent, 50)},
        Controls = {
            new TableLayoutPanel{Dock = DockStyle.Fill, Controls = {rootSideBar}, BackColor = Color.SkyBlue}, 
            formLogin, 
        }
        };
        

        this.Controls.Add(root);
    }

    private void btnLogin_CLick(object? sender, EventArgs e)
    {
        if(string.IsNullOrWhiteSpace(tbEmail.Text) && string.IsNullOrWhiteSpace(tbPassword.Text))
        {
            MessageBox.Show("email atau password yang anda masukkan tidak sesuai !");
        } else if( string.IsNullOrWhiteSpace(tbEmail.Text))
        {
            MessageBox.Show("email atau password yang anda masukkan tidak sesuai !");
        } else if(string.IsNullOrWhiteSpace(tbPassword.Text))
        {
            MessageBox.Show("email atau password yang anda masukkan tidak sesuai !");            
        }else
        {

            SqlParameter[] parameters =
            {
                new SqlParameter("@email", tbEmail.Text),
                new SqlParameter("@password", tbPassword.Text),
            };

            DataTable dt = db.Query(@"
                select * from users where email = @email and password = @password
            ", parameters);

            if(dt.Rows.Count == 1)
            {
                int id = Convert.ToInt32(dt.Rows[0]["id"]);

                SqlParameter[] param =
                {
                    new SqlParameter("@aktivitas", "login"),
                    new SqlParameter("@id_user", id)
                };

                int result = db.Execute(@"
                    insert into tbl_log(aktvitas, id_user) values(@aktivitas, @id_user)
                ", param);

                if(result == 1)
                {
                    MessageBox.Show("Log In berhasil");
                this.Hide();
                Form frmAdmin = new FormAdmin();
                frmAdmin.Show();
                }
                
            } else
            {
                MessageBox.Show("Password dan Email salah");
            }
        }
    }

}
