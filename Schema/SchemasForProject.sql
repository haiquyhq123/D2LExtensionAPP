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
Go
-- Table Assingments
If Exists(Select Name From Sys.tables where Name = 'Assignments')
Begin
	Drop Table Assignments;
End
Go
Create Table Assignments
(
	AssignmentId int primary key,
	-- enforce 1-to-many relationship
	CourseId int not null unique,
	UserId int not null unique,
	Title nvarchar(255) not null,
	Description nvarchar(255),
	DueDate datetime,
	Weight int check(Weight between 0 and 100),
	Status nvarchar(255),
	Grade decimal(4,2) check(Grade is null or Grade between 0 and 100),
	Priority int,
	CreateDate datetime default Getdate(),
	CompletedDate datetime default Getdate(),
	-- Ensure 1 to many relationship
	Constraint FK_Assingments_User Foreign key(UserId) References Users(UserId) On Delete Cascade,
	Constraint FK_Assignments_Courses Foreign key(CourseId) References Courses(CourseId) On Delete No Action -- when I set cascade, it report error so i set to no action
)
Create Nonclustered index IX_CourseID on dbo.Assignments(CourseId);
Create Nonclustered index IX_UserId on dbo.Assignments(UserId);
Create Nonclustered index IX_DueDate on dbo.Assignments(DueDate);
Create Nonclustered index IX_Status on dbo.Assignments(Status);
-- composite index
Create Nonclustered index IX_UserID_Status_DueDate on dbo.Assignments(UserId,Status,DueDate);
Create Nonclustered index IX_Priority_DueDate on dbo.Assignments(Priority DESC,DueDate ASC);

