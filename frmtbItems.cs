using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Practical_PRN292_SE140060_ThaiDucLoi
{
    public partial class frmtbItems : Form
    {
        public frmtbItems()
        {
            InitializeComponent();
        }
        SqlConnection conn = new SqlConnection(@"Data Source=SE140060\SQLEXPRESS;Initial Catalog=Pe2021;Integrated Security=SSPI");
    
        private void loadData()
        {
            conn.Open();

            SqlCommand com = new SqlCommand();
            com.Connection = conn;
            com.CommandText = "Select ItemCode,ItemName,Price From Items";

            SqlDataReader dr = com.ExecuteReader();

            int i = 0;


            while (dr.Read())
            {
                ListItem.Items.Add(dr["ItemCode"].ToString());
                ListItem.Items[i].SubItems.Add(dr["ItemName"].ToString());
                ListItem.Items[i].SubItems.Add(dr["Price"].ToString());
                i++;
            }

            conn.Close();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                loadData();
                MessageBox.Show("The connection is successful!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("The connection failed!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            if (validate() == true)
            {
                try
                {
                    conn.Open();
                    String code = txtItemCode.Text.Trim();
                    String name = txtItemName.Text;
                    float price = float.Parse(txtPrice.Text);

                    SqlCommand com = new SqlCommand("Insert into Items values (@ItemCode,@ItemName,@Price)", conn);

                    com.Parameters.Add("@ItemCode", SqlDbType.VarChar);
                    com.Parameters.Add("@ItemName", SqlDbType.NVarChar);
                    com.Parameters.Add("@Price", SqlDbType.Float);
                    com.Parameters["@ItemCode"].Value = code;
                    com.Parameters["@ItemName"].Value = name;
                    com.Parameters["@Price"].Value = price;

                    com.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Insert data successfully!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ListItem.Items.Clear();
                    loadData();

                }
                catch
                {
                    MessageBox.Show("Insert data failed!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                String code = txtItemCode.Text;
                String name = txtItemName.Text;
                float price = float.Parse(txtPrice.Text);

                SqlCommand com = new SqlCommand("Update Items Set Price=@Price,ItemName=@ItemName Where ItemCode=@ItemCode", conn);


                com.Parameters.Add("@ItemCode", SqlDbType.VarChar);
                com.Parameters.Add("@ItemName", SqlDbType.NVarChar);
                com.Parameters.Add("@Price", SqlDbType.Float);
                com.Parameters["@ItemCode"].Value = code;
                com.Parameters["@ItemName"].Value = name;
                com.Parameters["@Price"].Value = price;

                com.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Update data successfully!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ListItem.Items.Clear();
                loadData();
            }
            catch
            {
                MessageBox.Show("Update data failed!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try { 
                conn.Open();
                String code = txtItemCode.Text.Trim();
                String name = txtItemName.Text;

                SqlCommand com = new SqlCommand("Delete From Items Where ItemCode=@ItemCode and itemName=@itemname", conn);

                com.Parameters.Add("@ItemCode", SqlDbType.VarChar);
                com.Parameters.Add("@ItemName", SqlDbType.NVarChar);
                com.Parameters["@ItemCode"].Value = code;
                com.Parameters["@ItemName"].Value = name;


                com.ExecuteNonQuery();
                conn.Close();
                txtItemCode.Text = "";
                txtItemName.Text = "";
                txtPrice.Text = "";
                MessageBox.Show("Delete data successfully!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ListItem.Items.Clear();
                loadData();
            }
            catch
            {
                MessageBox.Show("Delete data failed!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ListItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem list in ListItem.SelectedItems)
            {
                txtItemCode.Text = list.SubItems[0].Text;
                txtItemName.Text = list.SubItems[1].Text;
                txtPrice.Text = list.SubItems[2].Text;
            }
        }


        private bool validate()
        {
            bool valid = false;
            string code = txtItemCode.Text;
            string name = txtItemName.Text;
            conn.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = conn;
            com.CommandText = "Select ItemCode,ItemName,Price From Items Where ItemCode=@ItemCode";
            com.Parameters.Add("@ItemCode", SqlDbType.VarChar);
            com.Parameters["@ItemCode"].Value = code;
            SqlDataReader dr = com.ExecuteReader();

            if (dr.Read())
            {
                errorProvider1.SetError(txtItemCode, "Item's Code are duplicate");
                valid = true;
            }
            conn.Close();
            try
            {

                float price = float.Parse(txtPrice.Text);
                if (price < 1)
                {
                    MessageBox.Show("Price must be > 0", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    valid = true;
                }
                if (txtItemCode.Text.Length > 5 || txtItemCode.Text.Length < 0)
                {
                    MessageBox.Show("Item's Code must be 5 digit", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    valid = true;
                }
                if (txtItemName.Text.Length > 50 || txtItemName.Text.Length < 0)
                {
                    MessageBox.Show("Item's Name must be 50 digit", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    valid = true;
                }
            }
            catch
            {
                errorProvider1.SetError(txtPrice, "Item's Price must be a float number");
                valid = true;
            }
            if (valid == true)
            {
                return false;
            }
            else
                errorProvider1.Clear();
            return true;
        }
    }

}
