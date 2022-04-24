using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Payments
{
    public class PayRepository : IPayRepository
    {
        public int GetCash(int number)
        {
            IDbConnection db = new SqlConnection(AppConnection.ConnectionString);
            if (db.State == ConnectionState.Closed)
                db.Open();
            return db.QueryFirstOrDefault<int>("SELECT remainder FROM CASH WHERE number = @NumberCash", new
            {
                NumberCash = number
            });
        }

        public int GetOrder(int orderNumber)
        {
            IDbConnection db = new SqlConnection(AppConnection.ConnectionString);
            if (db.State == ConnectionState.Closed)
                db.Open();
            return db.QueryFirstOrDefault<int>("SELECT paymentAmount FROM ORDERS WHERE orderNumber = @OrderNumber", new
            {
                OrderNumber = orderNumber
            });
        }

        public Cash InsertCash(Cash cash)
        {
            IDbConnection db = new SqlConnection(AppConnection.ConnectionString);
            if (db.State == ConnectionState.Closed)
                db.Open();
            return db.QueryFirstOrDefault<Cash>("INSERT INTO [CASH] (theDate, amount, remainder) VALUES (@TheDate, @Amount, @Remainder)", new{
                TheDate = DateTime.Today.ToString("yyyy - MM - dd"),
                Amount = cash.Amount,
                Remainder = cash.Remainder
            });
        }

        public Orders InsertOrder(Orders order)
        {
            IDbConnection db = new SqlConnection(AppConnection.ConnectionString);
            if (db.State == ConnectionState.Closed)
                db.Open();
            return db.QueryFirstOrDefault<Orders>("INSERT INTO [ORDERS] (theDate, amount, paymentAmount) VALUES (@theDate, @amount, @paymentAmount)", new
            {
                TheDate = DateTime.Today.ToString("yyyy - MM - dd"),
                Amount = order.Amount,
                PaymentAmount = 0
            });
        }

        public Payment InsertPayment(Payment payment)
        {
            IDbConnection db = new SqlConnection(AppConnection.ConnectionString);
            if (db.State == ConnectionState.Closed)
                db.Open();
            return db.QueryFirstOrDefault<Payment>("INSERT INTO PAYMENTS(cash, orderNumber, amount) VALUES (@Cash, @OrderNumber, @Amount)", new
            {
                Cash = payment.Cash,
                OrderNumber = payment.OrderNumber,
                Amount = payment.Amount
            });
        }

        public IEnumerable<Cash> SelectCash()
        {
            IDbConnection db = new SqlConnection(AppConnection.ConnectionString);
            if (db.State == ConnectionState.Closed)
                db.Open();
            return db.Query<Cash>("select number, theDate, amount, remainder from CASH", commandType: CommandType.Text);
        }

        public IEnumerable<Orders> SelectOrder()
        {
            IDbConnection db = new SqlConnection(AppConnection.ConnectionString);
            if (db.State == ConnectionState.Closed)
                db.Open();
            return db.Query<Orders>("select orderNumber, theDate, amount, paymentAmount FROM ORDERS", commandType: CommandType.Text);
        }

        public IEnumerable<Payment> SelectPayment()
        {
            IDbConnection db = new SqlConnection(AppConnection.ConnectionString);
            if (db.State == ConnectionState.Closed)
                db.Open();
            return db.Query<Payment>("SELECT id, cash, orderNumber, amount FROM PAYMENTS", commandType: CommandType.Text);
        }
    }
}
