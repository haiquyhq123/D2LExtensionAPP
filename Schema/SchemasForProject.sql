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
Go
Create Nonclustered index IX_On_Email_For_Filtering_Active_User On dbo.Users(IsActive);
Go
-- Course Table
If Exists(Select Name From Sys.tables where Name = 'Courses')
Begin
	Drop Table Courses;
End
Go
Create Table Courses
(
	CourseId int primary key,
	UserId int not null Unique, -- this one enfore 1 to many relationship
	CourseName nvarchar(255) not null,
	D2LCourseID nvarchar(255) unique not null,
	Semester nvarchar(255) not null,
	Professor nvarchar(255) not null,
	CourseCode nvarchar(255) not null,
	CreateDate datetime default Getdate(),
	IsArchived bit not null Default 0,
	Constraint FK_UserID_Courses_Table Foreign key(UserId) References Users(UserId) On Delete Cascade -- this one enfore 1 to many relationship
)
Go
-- Add Index
Create Nonclustered Index IX_For_Filter_By_Term On dbo.Courses(Semester);
-- Add Composite index to get active coursees for users
Go
Create Nonclustered Index IX_For_Get_Active_Course_For_User On dbo.Courses(UserId,IsArchived);