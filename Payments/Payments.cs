using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Payments
{
 
    public partial class Payments : Form
    {
        DataBase dataBase = new DataBase();
        private SqlConnection sqlConnection = null;
        private SqlCommandBuilder sqlBuilder = null;
        private SqlDataAdapter sqlDataAdapter = null;
        private DataSet dataSet = null;
        public Payments()
        {
            InitializeComponent();
        }


        private void AddAdvance_Click(object sender, EventArgs e)
        {
            if (newAdvance.Text != "")
            {
                int newSum = Convert.ToInt32(newAdvance.Text);

                SqlCommand command = new SqlCommand($"INSERT INTO [CASH] (theDate, amount, remainder) VALUES ('{DateTime.Today}', '{newSum}', '{newSum}')", sqlConnection);
                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Приход денег добавлен!", "Успех");
                }
                else
                {
                    MessageBox.Show("Запись не создана!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Введите сумму прихода денег!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            LoadData();
        }
           
       
        
        private void LoadData()
        {
            try
            {
                sqlDataAdapter = new SqlDataAdapter("SELECT number AS Номер, theDate AS Дата, amount AS Сумма, remainder AS Остаток FROM CASH", sqlConnection);

                sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);
                sqlBuilder.GetInsertCommand();
                sqlBuilder.GetUpdateCommand();

                dataSet = new DataSet();

                sqlDataAdapter.Fill(dataSet, "CASH");

                dataGridView1.DataSource = dataSet.Tables["CASH"];

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                sqlDataAdapter = new SqlDataAdapter("SELECT orderNumber AS Номер, theDate AS Дата, amount AS Сумма, paymentAmount AS 'Сумма Оплаты' FROM ORDERS", sqlConnection);

                sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);
                sqlBuilder.GetInsertCommand();
                sqlBuilder.GetUpdateCommand();

                dataSet = new DataSet();

                sqlDataAdapter.Fill(dataSet, "ORDERS");

                dataGridView2.DataSource = dataSet.Tables["ORDERS"];

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                sqlDataAdapter = new SqlDataAdapter("SELECT id AS 'Номер платежа', cash AS 'Приход денег', orderNumber AS Заказ, amount AS 'Сумма платежа' FROM PAYMENTS", sqlConnection);

                sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);
                sqlBuilder.GetInsertCommand();
                sqlBuilder.GetUpdateCommand();

                dataSet = new DataSet();

                sqlDataAdapter.Fill(dataSet, "PAYMENTS");

                dataGridView3.DataSource = dataSet.Tables["PAYMENTS"];

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Payments_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(@"Data Source=DESKTOP-70VDT2H\SQLEXPRESS2;Initial Catalog=PaymentDB;Integrated Security=True");
            sqlConnection.Open();
            LoadData();

        }

        private void AddOrder_Click(object sender, EventArgs e)
        {
            if (newOrder.Text != "")
            {
                int newSum = Convert.ToInt32(newOrder.Text);
                
                SqlCommand command = new SqlCommand($"INSERT INTO [ORDERS] (theDate, amount, paymentAmount) VALUES ('{DateTime.Today}', '{newSum}', '0')", sqlConnection);
                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Заказ добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Запись не создана!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Введите сумму заказа!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            LoadData();
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            if (textNumberAdvance.Text == "")
            {
                MessageBox.Show("Введите номер прихода денег!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (textNumberOrder.Text == "")
            {
                MessageBox.Show("Введите номер заказа!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (textAmount.Text == "")
            {
                MessageBox.Show("Введите сумму для перевода!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int numberAdvance = Convert.ToInt32(textNumberAdvance.Text);
                int numberOrder = Convert.ToInt32(textNumberOrder.Text);
                int paymentAmount = Convert.ToInt32(textAmount.Text);


                SqlCommand command = new SqlCommand("INSERT INTO PAYMENTS(cash, orderNumber, amount) VALUES (@numberCash, @orderNumber, @amount)", sqlConnection);
                command.Parameters.AddWithValue("@numberCash", numberAdvance);
                command.Parameters.AddWithValue("@orderNumber", numberOrder);
                command.Parameters.AddWithValue("@amount", paymentAmount);
                StringBuilder errorMessages = new StringBuilder();

                try
                {
                    command.ExecuteNonQuery();

                }
                catch (SqlException ex)
                {
                    for (int i = 0; i < ex.Errors.Count; i++)
                    {

                        errorMessages.Append(
                            ex.Errors[i].Message + "\n");
                        //"LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                        //"Source: " + ex.Errors[i].Source + "\n" +
                        //"Procedure: " + ex.Errors[i].Procedure + "\n");

                        //if (ex.Errors[i].Number == 547)
                        //{
                        //    MessageBox.Show("Вводимого номера заказа и/или прихода денег не существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //    continue;
                        //}
                        //else
                        //{
                        //    errorMessages.Append(
                        //    "Message: " + ex.Errors[i].Message + "\n");
                        //}
                        MessageBox.Show(errorMessages.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            LoadData();
            textNumberAdvance.Text = "";
            textNumberOrder.Text = "";
            textAmount.Text = "";
        }

        private void newAdvance_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8) // цифры и клавиша BackSpace
            {
                e.Handled = true;
            }
        }

        private void textNumberAdvance_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8) // цифры и клавиша BackSpace
            {
                e.Handled = true;
            }
        }

        private void textNumberOrder_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8) // цифры и клавиша BackSpace
            {
                e.Handled = true;
            }
        }

        private void textAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8) // цифры и клавиша BackSpace
            {
                e.Handled = true;
            }
        }

        private void newOrder_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8) // цифры и клавиша BackSpace
            {
                e.Handled = true;
            }
        }
    }
}
