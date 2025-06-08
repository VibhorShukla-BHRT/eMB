using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace PHEDChhattisgarh
{
public class SqlFunction
{
    SqlCommand cmd;
    SqlDataAdapter adapt;
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["eMB"].ConnectionString);
    public SqlFunction()
    {

    }
    public DataTable select_data(string query, SqlParameter[] parameter,SqlConnection con1)
    {
        DataTable dt = new DataTable();
        cmd = new SqlCommand(query, con1);
        if (parameter != null)
        {
            foreach (SqlParameter data in parameter)
            {
                cmd.Parameters.Add(data.ParameterName, data.Value);
            }
        }
        adapt = new SqlDataAdapter(cmd);
        adapt.Fill(dt);
        return dt;
    }
    public DataTable select_data(string query, SqlParameter[] parameter)
    {
        DataTable dt = new DataTable();
        cmd = new SqlCommand(query, con);
        if (parameter != null)
        {
            foreach (SqlParameter data in parameter)
            {
                cmd.Parameters.Add(data.ParameterName, data.Value);
            }
        }
        adapt = new SqlDataAdapter(cmd);
        adapt.Fill(dt);
        return dt;
    }
    public DataTable select_data(string query)
    {
        DataTable dt = new DataTable();
        cmd = new SqlCommand(query, con);
        adapt = new SqlDataAdapter(cmd);
        adapt.Fill(dt);
        return dt;
    }
    public DataTable select_data(string query, SqlConnection con1, SqlTransaction tra1)
    {
        DataTable dt = new DataTable();
        cmd = new SqlCommand(query, con1, tra1);
        adapt = new SqlDataAdapter(cmd);
        adapt.Fill(dt);
        return dt;
    }
    public DataTable select_data(string query, SqlParameter[] parameter, SqlConnection con1, SqlTransaction tra1)
    {
        DataTable dt = new DataTable();
        cmd = new SqlCommand(query, con1, tra1);
        if (parameter != null)
        {
            foreach (SqlParameter data in parameter)
            {
                cmd.Parameters.Add(data.ParameterName, data.Value);
            }
        }
        adapt = new SqlDataAdapter(cmd);
        adapt.Fill(dt);
        return dt;
    }

    public void fill_Dropdown_join(string query, string value, string name,string defualtSelect, DropDownList ddlList, SqlParameter[] parameter)
    {
        ddlList.Items.Clear();
        if (name.Trim() == "")
        {
            name = value;
        }
        string query1 = query;
        DataTable dt = new DataTable();
        cmd = new SqlCommand(query1, con);
        if (parameter != null)
        {
            foreach (SqlParameter data in parameter)
            {
                cmd.Parameters.Add(data.ParameterName, data.Value);
            }
        }
        adapt = new SqlDataAdapter(cmd);
        adapt.Fill(dt);
        ddlList.DataSource = dt;
        ddlList.DataTextField = name;
        ddlList.DataValueField = value;
        ddlList.DataBind();
        if (defualtSelect == "YES")
        {
            ddlList.Items.Insert(0, new ListItem("चुनें", ""));
        }
        else if (defualtSelect == "ALL")
        {
            ddlList.Items.Insert(0, new ListItem("ALL", ""));
        }
        else if (defualtSelect == "NOSELECT")
        {
        }
        else
        {
            ddlList.Items.Insert(0, new ListItem(defualtSelect, ""));
        }
    }
    public void fill_Dropdown(string table, string value, string name, string where, string orderby,string defualtSelect, DropDownList ddlList, SqlParameter[] parameter,SqlConnection con1)
    {
        ddlList.Items.Clear();
        string query = "select distinct " + name + "," + value + " from " + table + " " + where + " order by " + orderby + "";
        DataTable dt = new DataTable();
        cmd = new SqlCommand(query, con1);
        if (parameter != null)
        {
            foreach (SqlParameter data in parameter)
            {
                cmd.Parameters.Add(data.ParameterName, data.Value);
            }
        }
        adapt = new SqlDataAdapter(cmd);
        adapt.Fill(dt);
        ddlList.DataSource = dt;
        ddlList.DataTextField = name;
        ddlList.DataValueField = value;
        ddlList.DataBind();
        ddlList.Items.Insert(0, new ListItem(defualtSelect, ""));
    }
     public void fillDropdown(string table, string value, string where, string orderby, string defualtSelect, DropDownList ddlList, SqlParameter[] parameter)
    {
        ddlList.Items.Clear();
        string query = "select distinct " + value + " from " + table + " " + where + " order by " + orderby + "";
        DataTable dt = new DataTable();
        cmd = new SqlCommand(query, con);
        if (parameter != null)
        {
            foreach (SqlParameter data in parameter)
            {
                cmd.Parameters.Add(data.ParameterName, data.Value);
            }
        }
        adapt = new SqlDataAdapter(cmd);
        adapt.Fill(dt);
        ddlList.DataSource = dt;
        ddlList.DataTextField = value;
        ddlList.DataValueField = value;
        ddlList.DataBind();
        ddlList.Items.Insert(0, new ListItem(defualtSelect, ""));
    }

    
    public void fill_Dropdown(string table, string value, string orderby, DropDownList ddlList, SqlParameter[] parameter)
    {
        ddlList.Items.Clear();
        string query = "select distinct " + value + " from " + table + " order by " + orderby + "";
        DataTable dt = new DataTable();
        cmd = new SqlCommand(query, con);
        if (parameter != null)
        {
            foreach (SqlParameter data in parameter)
            {
                cmd.Parameters.Add(data.ParameterName, data.Value);
            }
        }
        adapt = new SqlDataAdapter(cmd);
        adapt.Fill(dt);
        ddlList.DataSource = dt;
        ddlList.DataTextField = value;
        ddlList.DataValueField = value;
        ddlList.DataBind();
        ddlList.Items.Insert(0, new ListItem("Select", ""));
    }

    public void fill_Dropdown(string table, string value,string where, string orderby, DropDownList ddlList, SqlParameter[] parameter)
    {
        ddlList.Items.Clear();
        string query = "select distinct " + value + " from " + table + " " + where + " order by " + orderby + "";
        DataTable dt = new DataTable();
        cmd = new SqlCommand(query, con);
        if (parameter != null)
        {
            foreach (SqlParameter data in parameter)
            {
                cmd.Parameters.Add(data.ParameterName, data.Value);
            }
        }
        adapt = new SqlDataAdapter(cmd);
        adapt.Fill(dt);
        ddlList.DataSource = dt;
        ddlList.DataTextField = value;
        ddlList.DataValueField = value;
        ddlList.DataBind();
        ddlList.Items.Insert(0, new ListItem("Select", ""));
    }

    public void fill_Dropdown(string table, string value, string name, string where, string orderby, DropDownList ddlList, SqlParameter[] parameter)
    {
        ddlList.Items.Clear();
        string query = "select distinct " + name + "," + value + " from " + table + " " + where + " order by " + orderby + "";
        DataTable dt = new DataTable();
        cmd = new SqlCommand(query, con);
        if (parameter != null)
        {
            foreach (SqlParameter data in parameter)
            {
                cmd.Parameters.Add(data.ParameterName, data.Value);
            }
        }
        adapt = new SqlDataAdapter(cmd);
        adapt.Fill(dt);
        ddlList.DataSource = dt;
        ddlList.DataTextField = name;
        ddlList.DataValueField = value;
        ddlList.DataBind();
        ddlList.Items.Insert(0, new ListItem("Select", ""));
    }
    
    public void FillYear(DropDownList ddl)
    {
        ddl.Items.Add(new ListItem("Select Year", ""));
        for (int i = DateTime.Now.Year; i >= 2021; i--)
        {
            ddl.Items.Add(i.ToString());
        }
    }
    public void fill_Dropdown(string table, string value, string name, string where, string orderby, string select, DropDownList ddlList, SqlParameter[] parameter)
    {
        ddlList.Items.Clear();
        string query = "select distinct " + name + "," + value + " from " + table + " " + where + " order by " + orderby + "";
        DataTable dt = new DataTable();
        cmd = new SqlCommand(query, con);
        if (parameter != null)
        {
            foreach (SqlParameter data in parameter)
            {
                cmd.Parameters.Add(data.ParameterName, data.Value);
            }
        }
        adapt = new SqlDataAdapter(cmd);
        adapt.Fill(dt);
        ddlList.DataSource = dt;
        ddlList.DataTextField = name;
        ddlList.DataValueField = value;
        ddlList.DataBind();
        if (select == "YES")
        {
            ddlList.Items.Insert(0, new ListItem("चुनें", ""));
        }
        else if (select == "ALL")
        {
            ddlList.Items.Insert(0, new ListItem("ALL", ""));
        }
        else if (select == "NOSELECT")
        {
        }
        else
        {
            ddlList.Items.Insert(0, new ListItem(select, ""));
        }
    }
    public void fill_ListBox(string table, string value, string name, string where, string orderby, string select, ListBox ddlList, SqlParameter[] parameter)
    {
        ddlList.Items.Clear();
        string query = "select distinct " + name + "," + value + " from " + table + " " + where + " " + orderby + "";
        DataTable dt = new DataTable();
        cmd = new SqlCommand(query, con);
        if (parameter != null)
        {
            foreach (SqlParameter data in parameter)
            {
                cmd.Parameters.Add(data.ParameterName, data.Value);
            }
        }
        adapt = new SqlDataAdapter(cmd);
        adapt.Fill(dt);
        ddlList.DataSource = dt;
        ddlList.DataTextField = name;
        ddlList.DataValueField = value;
        ddlList.DataBind();
        if (select == "YES")
        {
            ddlList.Items.Insert(0, new ListItem("चुनें", ""));
        }
        else if (select == "ALL")
        {
            ddlList.Items.Insert(0, new ListItem("ALL", ""));
        }
    }

    public void fill_Dropdown(string table, string value, string name, DropDownList ddlList)
    {
        ddlList.Items.Clear();
        string query = "select " + name + "," + value + " from " + table + " order by " + name + " asc";
        DataTable dt = new DataTable();
        cmd = new SqlCommand(query, con);
        adapt = new SqlDataAdapter(cmd);
        adapt.Fill(dt);
        ddlList.DataSource = dt;
        ddlList.DataTextField = name;
        ddlList.DataValueField = value;
        ddlList.DataBind();
        ddlList.Items.Insert(0, new ListItem("चुनें", ""));
    }
    public void fill_Dropdown(string table, string value, string name, string select, string order_by, DropDownList ddlList)
    {
        ddlList.Items.Clear();
        string query = "select " + name + "," + value + " from " + table + " " + order_by;
        DataTable dt = new DataTable();
        cmd = new SqlCommand(query, con);
        adapt = new SqlDataAdapter(cmd);
        adapt.Fill(dt);
        ddlList.DataSource = dt;
        ddlList.DataTextField = name;
        ddlList.DataValueField = value;
        ddlList.DataBind();
        if (select == "YES")
        {
            ddlList.Items.Insert(0, new ListItem("चुनें", ""));
        }
        else if (select == "ALL")
        {
            ddlList.Items.Insert(0, new ListItem("ALL", "ALL"));
        }
    }

    public void fill_CheckBox(string table, string value, string name, string select, string order_by, CheckBoxList ddlList)
    {
        ddlList.Items.Clear();
        string query = "select " + name + "," + value + " from " + table + " " + order_by;
        DataTable dt = new DataTable();
        cmd = new SqlCommand(query, con);
        adapt = new SqlDataAdapter(cmd);
        adapt.Fill(dt);
        ddlList.DataSource = dt;
        ddlList.DataTextField = name;
        ddlList.DataValueField = value;
        ddlList.DataBind();
        if (select == "YES")
        {
            ddlList.Items.Insert(0, new ListItem("चुनें", ""));
        }
        else if (select == "ALL")
        {
            ddlList.Items.Insert(0, new ListItem("ALL", "ALL"));
        }
    }

    public void fill_CheckBox(string table, string value, string name, string select, string where, CheckBoxList ddlList, SqlParameter[] parameter)
    {
        ddlList.Items.Clear();
        string query = "select " + name + "," + value + " from " + table + " where " + where + " order by " + name + " asc";
        DataTable dt = new DataTable();
        cmd = new SqlCommand(query, con);
        if (parameter != null)
        {
            foreach (SqlParameter data in parameter)
            {
                cmd.Parameters.Add(data.ParameterName, data.Value);
            }
        }
        adapt = new SqlDataAdapter(cmd);
        adapt.Fill(dt);
        ddlList.DataSource = dt;
        ddlList.DataTextField = name;
        ddlList.DataValueField = value;
        ddlList.DataBind();
        if (select == "YES")
        {
            ddlList.Items.Insert(0, new ListItem("चुनें", ""));
        }
        else if (select == "ALL")
        {
            ddlList.Items.Insert(0, new ListItem("ALL", ""));
        }

    }

    public int insert(string query, SqlParameter[] parameter, SqlConnection con)
    {
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        cmd = new SqlCommand(query, con);
        if (parameter != null)
        {
            foreach (SqlParameter data in parameter)
            {
                cmd.Parameters.Add(data.ParameterName, data.Value);
            }
        }
        int res = cmd.ExecuteNonQuery();
        con.Close();
        return res;
    }
    public int insert(string query, SqlParameter[] parameter, SqlConnection con1, SqlTransaction tra1)
    {
        cmd = new SqlCommand(query, con1, tra1);
        if (parameter != null)
        {
            foreach (SqlParameter data in parameter)
            {
                cmd.Parameters.Add(data.ParameterName, data.Value);
            }
        }
        int res = cmd.ExecuteNonQuery();
        return res;
    }
    public int update(string query, SqlParameter[] parameter, SqlConnection con1, SqlTransaction tra1)
    {
        cmd = new SqlCommand(query, con1, tra1);
        if (parameter != null)
        {
            foreach (SqlParameter data in parameter)
            {
                cmd.Parameters.Add(data.ParameterName, data.Value);
            }
        }
        int res = cmd.ExecuteNonQuery();
        return res;
    }
    public int update(string query, SqlParameter[] parameter)
    {
        con.Open();
        cmd = new SqlCommand(query, con);
        if (parameter != null)
        {
            foreach (SqlParameter data in parameter)
            {
                cmd.Parameters.Add(data.ParameterName, data.Value);
            }
        }
        int res = cmd.ExecuteNonQuery();
        con.Close();
        return res;
    }
    public int delete(string query, SqlParameter[] parameter)
    {
        con.Open();
        cmd = new SqlCommand(query, con);
        if (parameter != null)
        {
            foreach (SqlParameter data in parameter)
            {
                cmd.Parameters.Add(data.ParameterName, data.Value);
            }
        }
        int res = cmd.ExecuteNonQuery();
        con.Close();
        return res;
    }
}

}