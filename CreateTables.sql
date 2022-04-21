USE [PaymentDB]
GO
CREATE TABLE ORDERS(
orderNumber int IDENTITY(1,1) NOT NULL,
theDate date NOT NULL,
amount int NOT NULL,
paymentAmount int NOT NULL,
PRIMARY KEY(orderNumber)
);

CREATE TABLE CASH(
number int IDENTITY(1,1) NOT NULL,
theDate date NOT NULL,
amount int NOT NULL,
remainder int NOT NULL,
PRIMARY KEY(number)
);

CREATE TABLE PAYMENTS(
id int IDENTITY(1,1) NOT NULL,
cash int NOT NULL,
orderNumber int NOT NULL,
amount int NOT NULL,
PRIMARY KEY(id)
);
