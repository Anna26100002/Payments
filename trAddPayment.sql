USE [PaymentDB]
GO
/****** Object:  Trigger [dbo].[trAddPayment]    Script Date: 24.04.2022 20:21:05 ******/
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
IF (SELECT COUNT(i.amount) FROM inserted i WHERE i.amount=0)>0
	BEGIN
	THROW 55552, 'Введите сумму для перевода!', 1;
	RETURN
	END
IF (SELECT COUNT(c.number) FROM CASH c INNER JOIN inserted i ON c.number=i.cash)=0
	BEGIN
	THROW 55553, 'Такого номера прихода денег не существует!', 1;
	RETURN
	END
IF (SELECT i.amount FROM ORDERS o INNER JOIN inserted i ON o.orderNumber=i.orderNumber WHERE o.paymentAmount=o.amount)>0
	BEGIN
	THROW 55554, 'Заказ полностью оплачен!', 1;
	RETURN
	END
IF (SELECT COUNT(o.orderNumber) FROM ORDERS o INNER JOIN inserted i ON o.orderNumber=i.orderNumber)=0
	BEGIN
	THROW 55555, 'Такого номера заказа не существует!', 1;
	RETURN
	END
IF (SELECT c.remainder-i.amount FROM CASH c INNER JOIN inserted i ON c.number=i.cash)<0
	BEGIN
	THROW 55556, 'Нет средств!', 1;
	RETURN
	END
IF (SELECT o.amount-o.paymentAmount-i.amount FROM ORDERS o INNER JOIN inserted i ON o.orderNumber=i.orderNumber)<0
	BEGIN
	THROW 55557, 'Введите меньшую сумму платежа для перевода!', 1
	RETURN
	END
ELSE
	BEGIN
	UPDATE CASH SET remainder = c.remainder-i.amount FROM inserted i INNER JOIN CASH c ON c.number=i.cash
	UPDATE ORDERS SET paymentAmount = o.paymentAmount+i.amount FROM inserted i INNER JOIN ORDERS o ON o.orderNumber=i.orderNumber
	END