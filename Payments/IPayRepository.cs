using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments
{
    public interface IPayRepository
    {
        Cash InsertCash(Cash cash); //Добавление прихода денег
        Orders InsertOrder(Orders order); //Добавление заказа
        Payment InsertPayment(Payment payment); //Добавление платежа
        IEnumerable<Payment> SelectPayment(); //Список платежей
        IEnumerable<Cash> SelectCash(); //Список прихода денег
        IEnumerable<Orders> SelectOrder(); //Список заказов
        int GetCash(int number); //Выбор 1 прихода денег
        int GetOrder(int orderNumber); //Выбор 1 заказа
    }
}
