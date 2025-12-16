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
-- Course Table
If Exists(Select Name From Sys.tables where Name = 'Courses')
Begin
	Drop Table Courses;
End
Go
Create Table Courses
(
	CourseId int primary key,
	UserId int not null, 
	CourseName nvarchar(255) not null,
	D2LCourseID nvarchar(255) unique not null,
	Semester nvarchar(255) not null,
	Professor nvarchar(255) not null,
	CourseCode nvarchar(255) not null,
	CreateDate datetime default Getdate(),
	IsArchived bit not null Default 0,
	Constraint Uniqe_UserId_CourseID unique(UserId,CourseId)
)
Go
-- Add non clustered index for filter course by semester
Create Nonclustered Index IX_For_Filter_By_Term On dbo.Courses(Semester);
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
	CourseId int not null, --enforce 1-to-mnay relationship
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
	Constraint FK_Assignments_Courses Foreign key(CourseId) References Courses(CourseId) On Delete Cascade,
	Constraint UK_CourseId_Title Unique(CourseId,Title) -- prevent for accidentially inserting two assignments with same title
)
Go
-- create non clustered for faster query
Create Nonclustered index IX_CourseID on dbo.Assignments(CourseId);
Create Nonclustered index IX_DueDate on dbo.Assignments(DueDate);
Create Nonclustered index IX_Status on dbo.Assignments(Status);
-- composite index
Create Nonclustered index IX_Priority_DueDate on dbo.Assignments(Priority DESC,DueDate ASC);
Go
-- CourseWeeks Table
if Exists(Select Name from sys.tables where Name = 'CourseWeeks')
Begin
 Drop Table CourseWeeks;
End
Go
Create Table CourseWeeks
(
	WeekId int primary key,
	CourseId int not null,
	WeekTitle nvarchar(255),
	WeekNumber int not null, 
	Description nvarchar(255),
	StartDate datetime,
	EndDate datetime,
	OrderIndex int,
	IsPublished bit not null default 1,
	Constraint FK_CourseId_CourseWeek_Table Foreign key(CourseId) References Courses(CourseId) on Delete cascade,
	Constraint UniqueKey_CourseId_WeekNumber Unique(CourseId,WeekNumber),
	Constraint CK_CourseWeeks_EndDate CHECK (EndDate >= StartDate)
)
GO
if Exists(Select Name from sys.tables where Name = 'CourseMaterials')
Begin
 Drop Table CourseMaterials;
End
Go
Create table CourseMaterials
(
	MaterialId int primary key,
	WeekId int not null,
	MaterialName nvarchar(255),
	MaterialType nvarchar(255),
	FileUrl nvarchar(255),
	Description nvarchar(255),
	FileSizeKb int check(FileSizeKb between 0 and 102400),
	UploadDate datetime default Getdate(),
	LastAccessedDate datetime,
	ViewCount int default 0,
	IsDownloadable bit not null default 1,
	constraint FK_WeekId_CourseMaterials_Table Foreign key(WeekId) References CourseWeeks(WeekId) On Delete Cascade
)
Go
-- non clustered index for faster query
Create Nonclustered index IX_WeekId_Get_All_Material_For_This_Week on dbo.CourseMaterials(WeekId);
Create Nonclustered index IX_MaterialType on dbo.CourseMaterials(MaterialType);


