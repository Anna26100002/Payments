USE [PaymentDB]
GO
/****** Object:  Trigger [dbo].[trAddPayment]    Script Date: 21.04.2022 20:11:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER TRIGGER [dbo].[trAddPayment]
ON [dbo].[PAYMENTS]
FOR INSERT
AS
IF @@ROWCOUNT = 0
	RETURN
IF ((SELECT COUNT(*) FROM CASH c, inserted i WHERE c.number=i.cash)=0)
	BEGIN
	THROW 55553, 'Такого номера прихода денег не существует!', 1;
	RETURN
	END
IF ((SELECT COUNT(*) FROM ORDERS o, inserted i WHERE o.orderNumber=i.orderNumber)=0)
	BEGIN
	THROW 55554, 'Такого номера заказа не существует!', 1;
	RETURN
	END
IF ((SELECT c.remainder-i.amount FROM CASH c, inserted i WHERE c.number=i.cash)<0)
	BEGIN
	THROW 55555, 'Нет средств!', 1;
	RETURN
	END
IF ((SELECT o.amount-o.paymentAmount-i.amount FROM ORDERS o, inserted i WHERE o.orderNumber=i.orderNumber)<0)
	BEGIN
	THROW 55556, 'Введите меньшую сумму платежа для перевода!', 1
	RETURN
	END
ELSE
	BEGIN
	UPDATE CASH SET remainder = c.remainder-i.amount FROM inserted i, CASH c WHERE c.number=i.cash
	UPDATE ORDERS SET paymentAmount = o.paymentAmount+i.amount FROM inserted i, ORDERS o WHERE o.orderNumber=i.orderNumber
	END