using System.Data;
using Microsoft.Data.SqlClient;

public class DbConn()
{
    string connString =  "server=localhost\\SQLEXPRESS;database='DIYsMART';trusted_connection=true;trustservercertificate=true";

    SqlConnection getConnection()
    {
        return new SqlConnection(connString);
    }

    public DataTable Query(string query, SqlParameter[]? parameters = null)
    {
        using SqlConnection conn = getConnection();
        using SqlDataAdapter adapter = new SqlDataAdapter(query, conn);

        if(parameters != null ) 
            adapter.SelectCommand.Parameters.AddRange(parameters);

        DataTable dt = new DataTable();
        adapter.Fill(dt);
        return dt;
    }

    public int Execute(string query, SqlParameter[]? param = null)
    {
        using SqlConnection conn = getConnection();
        using SqlCommand cmd = new SqlCommand(query, conn);

        if(param != null)
            cmd.Parameters.AddRange(param);

        conn.Open();
        return cmd.ExecuteNonQuery();
    }

}