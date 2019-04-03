


IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = N'BUSTICKET') BEGIN CREATE DATABASE [BUSTICKET] END;
GO
USE BUSTICKET
GO

Alter table schedules 
drop constraint IF EXISTS FK_SCEDULES_BUS ;

Alter table transactions 
drop constraint IF EXISTS FK_TRANSACTION_CREDIT ;

Alter table transactions 
drop constraint IF EXISTS FK_TRANSACTION_USERS ;

Alter table transactions 
drop constraint IF EXISTS FK_TRANSACTION_SCEDHULE ;

if exists ( select * from sys.objects where name='users' )
drop table users;
GO

if exists ( select * from sys.objects where name='buses' )
drop table buses;
GO

if exists ( select * from sys.objects where name='creditcard_type' )
drop table creditcard_type;
GO

if exists ( select * from sys.objects where name='schedules' )
drop table schedules;
GO

if exists ( select * from sys.objects where name='transactions' )
drop table transactions;
GO

CREATE TABLE users ( 
					[user_id] INT primary key identity(1,1) NOT NULL,
					 [name] VARCHAR(50) NOT NULL,
					  email VARCHAR(50) NOT NULL,
					   contact VARCHAR(11) NOT NULL ,
					   apt_number VARCHAR(10),
					   street_number VARCHAR(10),
					   city VARCHAR(10),
					   state VARCHAR(10),
					   country VARCHAR(10),
					   postal_Code VARCHAR(10)
					    );
GO

CREATE TABLE schedules ( 
					s_id int primary key identity(1,1) NOT NULL,
					 [source] VARCHAR(50) NOT NULL,
					  destination VARCHAR(50) NOT NULL,
					   [date] datetime NOT NULL,
					   cost int NOT NULL,
					   bus_id int not null,
					   description varchar(100) NOT NULL
					    );
GO

CREATE TABLE buses ( 
					bus_id int primary key identity(1,1) NOT NULL,
					  bus_name VARCHAR(50) NOT NULL,
					   bus_type VARCHAR(50) NOT NULL,
					   total_seats int NOT NULL
					    );
GO
CREATE TABLE transactions ( 
					t_id int primary key identity(1,1) NOT NULL,
					  nameOnCard VARCHAR(250) NOT NULL,
					   cardNumber VARCHAR(45) NOT NULL,
					   unit_price decimal NOT NULL,
					   quantity int NOT NULL,
					   total_price decimal NOT NULL,
					   exp_Date VARCHAR(45) NOT NULL,
					   createdOn datetime NOT NULL,
					   createdBy VARCHAR(45) NOT NULL,
					   c_id int not null,
					   s_id int not null,
					   [user_id] int not null
					    );			
GO		

CREATE TABLE creditcard_type ( 
					 c_id int primary key identity(1,1) NOT NULL,
					 [name] VARCHAR(45) NOT NULL,
					  starts_with VARCHAR(250) NOT NULL,
					   [length] decimal NOT NULL
					    );								

GO
ALTER TABLE schedules   
ADD CONSTRAINT FK_SCEDULES_BUS FOREIGN KEY (bus_id)
References buses(bus_id);	

ALTER TABLE transactions   
ADD CONSTRAINT FK_TRANSACTION_USERS FOREIGN KEY ([user_id])
References users([user_id]);
		
ALTER TABLE transactions
ADD CONSTRAINT FK_TRANSACTION_SCHEDULE FOREIGN KEY([S_ID])
References schedules([S_ID])					

ALTER TABLE transactions
ADD CONSTRAINT FK_TRANSACTION_CREDIT FOREIGN KEY(c_id)
References creditcard_type(c_id)

