IF DB_ID('SchemaForD2LExtensionAPP') IS NOT NULL
BEGIN
	Use Master;
    ALTER DATABASE SchemaForD2LExtensionAPP SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE SchemaForD2LExtensionAPP;
END

CREATE DATABASE SchemaForD2LExtensionAPP;
Go
Use SchemaForD2LExtensionAPP;
Go 
-- user table
if Exists(Select name From Sys.tables where name = 'Users' ) 
Begin
	Drop Table Users;
End
Go
Use SchemaForD2LExtensionAPP
Go

Create table Users
(
	UserId int Identity(1,1) primary key,
	Email nvarchar(255) unique  not null check(Charindex('@',Email) <> 0),
	Password nvarchar(255) not null,
	FirstName nvarchar(255) not null,
	LastName nvarchar(255) not null,
	CreatedDate datetime not null Default Getdate(),
	LastLogin datetime not null Default GetDate(),
	IsActive bit not null Default 1
)
Go
-- Non cluster index
IF EXISTS(Select name from sys.indexes where name = N'IX_On_Email_For_Login' and object_id = Object_Id('dbo.Users'))
Begin
	Drop Index IX_On_Email_For_Login On dbo.Users;
End

Create Nonclustered index IX_On_Email_For_Login On dbo.Users(Email);

Go

IF EXISTS(Select name from sys.indexes where name = N'IX_On_Email_For_Filtering_Active_User' and object_id = Object_Id('dbo.Users'))
Begin
	Drop Index IX_On_Email_For_Filtering_Active_User On dbo.Users;
End

Create Nonclustered index IX_On_Email_For_Filtering_Active_User On dbo.Users(IsActive);

Go
Alter Table Users Alter column Password nvarchar(256) not null;
Go
Alter Table Users Alter column Email nvarchar(255) not null;