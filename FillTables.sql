USE [PaymentDB]
GO
INSERT INTO ORDERS(theDate, amount, paymentAmount) VALUES (GETDATE(), 70, 0)
INSERT INTO ORDERS(theDate, amount, paymentAmount) VALUES (GETDATE(), 80, 0)

INSERT INTO CASH(theDate, amount, remainder) VALUES (GETDATE(), 100, 100)
INSERT INTO CASH(theDate, amount, remainder) VALUES (GETDATE(), 150, 150)
INSERT INTO CASH(theDate, amount, remainder) VALUES (GETDATE(), 80, 80)