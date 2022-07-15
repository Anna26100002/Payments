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
        IPayRepository payRepository;

        public Payments()
        {
            InitializeComponent();
        }


        private void AddAdvance_Click(object sender, EventArgs e)
        {
            if (newAdvance.Text != ""&& newAdvance.Text != "0")
            {
                int newSum = Convert.ToInt32(newAdvance.Text);
                Cash cash = new Cash()
                {
                    TheDate = DateTime.Today,
                    Amount = newSum,
                    Remainder = newSum
                };
                payRepository = new PayRepository();
                payRepository.InsertCash(cash);
                
            }
            else
            {
                MessageBox.Show("Введите сумму прихода денег!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            LoadData();
        }
             
        private void LoadData()
        {
            payRepository = new PayRepository();
            dataGridView1.DataSource = payRepository.SelectCash();
            dataGridView2.DataSource = payRepository.SelectOrder();
            dataGridView3.DataSource = payRepository.SelectPayment(); 
        }

        private void Payments_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void AddOrder_Click(object sender, EventArgs e)
        {
            if (newOrder.Text != ""&& newOrder.Text != "0")
            {
                int newSum = Convert.ToInt32(newOrder.Text);
                Orders order = new Orders()
                {
                    TheDate = DateTime.Today,
                    Amount = newSum,
                    PaymentAmount = 0
                };
                payRepository = new PayRepository();
                payRepository.InsertOrder(order);
                
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
                int numberRowCash = -1;
                int numberRowOrder = -1;

                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (Convert.ToInt32(dataGridView1[0, i].Value) == numberAdvance)
                    {
                        numberRowCash = i;
                        break;
                    }
                }

                for (int i = 0; i < dataGridView2.RowCount; i++)
                {
                    if (Convert.ToInt32(dataGridView2[0, i].Value) == numberOrder)
                    {
                        numberRowOrder = i;
                        break;
                    }
                }

                payRepository = new PayRepository();

                if (dataGridView1[3, numberRowCash].Value.ToString() != payRepository.GetCash(numberAdvance).ToString())
                {
                    var result = MessageBox.Show("Остаток прихода денег изменился. Продолжить оплату?", "Произошли изменения", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        StringBuilder errorMessages = new StringBuilder();
                        try
                        {
                            Payment payment = new Payment()
                            {
                                Cash = numberAdvance,
                                OrderNumber = numberOrder,
                                Amount = paymentAmount
                            };
                            payRepository = new PayRepository();
                            payRepository.InsertPayment(payment);
                        }
                        catch (SqlException ex)
                        {
                            for (int i = 0; i < ex.Errors.Count; i++)
                            {

                                errorMessages.Append(
                                    ex.Errors[i].Message + "\n");
                                MessageBox.Show(errorMessages.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    
                    LoadData();
                }

                if (dataGridView2[3, numberRowOrder].Value.ToString() != payRepository.GetOrder(numberOrder).ToString())
                {
                    var result = MessageBox.Show("Остаток заказа изменился. Продолжить оплату?", "Произошли изменения", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        StringBuilder errorMessages = new StringBuilder();
                        try
                        {
                            Payment payment = new Payment()
                            {
                                Cash = numberAdvance,
                                OrderNumber = numberOrder,
                                Amount = paymentAmount
                            };
                            payRepository = new PayRepository();
                            payRepository.InsertPayment(payment);
                        }
                        catch (SqlException ex)
                        {
                            for (int i = 0; i < ex.Errors.Count; i++)
                            {
                                errorMessages.Append(
                                    ex.Errors[i].Message + "\n");

                                MessageBox.Show(errorMessages.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    LoadData();
                }
                else
                {
                    StringBuilder errorMessages = new StringBuilder();
                    try
                    {
                        Payment payment = new Payment()
                        {
                            Cash = numberAdvance,
                            OrderNumber = numberOrder,
                            Amount = paymentAmount
                        };
                        payRepository = new PayRepository();
                        payRepository.InsertPayment(payment);
                    }
                    catch (SqlException ex)
                    {
                        for (int i = 0; i < ex.Errors.Count; i++)
                        {

                            errorMessages.Append(
                                ex.Errors[i].Message + "\n");

                            MessageBox.Show(errorMessages.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
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
