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

If Exists(Select Name From Sys.tables where Name = 'Courses')
Begin
    Drop Table Courses;
End
Create Table Courses
(
    Id int primary key identity(1,1),
    UserId nvarchar(450) not null,
    Title nvarchar(256) not null,
    Description nvarchar(256),
    Semester nvarchar(256),
    Professor nvarchar(256),
    CourseCode nvarchar(256) not null,
    CreateDate datetime default GetDate()
)
Go
-- Add Relationship with AspNetUsers Table
Alter Table Courses Add Constraint FK_Course_User Foreign key(UserId) References AspNetUsers(Id) On Delete Cascade;
Go
-- Add possible index for faster query
Create Nonclustered Index IX_Filter_Term On Courses(Semester);
Go

if Exists(Select Name from sys.tables where Name = 'CourseWeeks')
Begin
 Drop Table CourseWeeks;
End
Go
Create Table CourseWeeks
(
   Id int primary key identity(1,1),
   CourseId int not null,
   Title nvarchar(256) not null,
   Description nvarchar(256)
)
-- Add Relation 1-to-many with course
Alter Table CourseWeeks Add Constraint FK_Course_CourseWeek Foreign key(CourseId) References Courses(Id);
Go

If Exists(Select Name From Sys.Tables where Name = 'Assignments')
Begin
    Drop Table Assignments;
End
Create Table Assignments
(
    Id int primary key Identity(1,1),
    CourseWeekId int not null,
    Title nvarchar(256) not null,
    Description nvarchar(256),
    DueDate datetime not null,
    Grade int,
    Weight int not null,
    Status nvarchar(256),
    Priority int not null,
    CreateDate datetime default GetDate(),
    CompleteDate datetime,
    EstimatedHours decimal(4,1) not null default 1.0
)
Go
-- Add Relationship and constraint
Alter Table Assignments Add Constraint FK_CW_Assignment Foreign key(CourseWeekId) References CourseWeeks(Id) On Delete Cascade;
Alter Table Assignments Add Constraint UK_CourseId_Title Unique(CourseWeekId, Title); -- make unique for each course
Alter Table Assignments Add Constraint CK_Grade check(Grade is null or Grade between 0 and 100);
Alter Table Assignments Add Constraint CK_Weight check(Weight between 0 and 100);
Alter Table Assignments Add Constraint CK_Status check(Status in ('Finish','InProcess','OverDue','NotStarted'));
-- Add Possible Index For Faster Query
Create Nonclustered index IX_DueDate on Assignments(DueDate);
Create Nonclustered index IX_Status on Assignments(Status);
Create Nonclustered index IX_Priority_DueDate on Assignments(Priority DESC, DueDate ASC);
-- View For Aassignment detail
Go
Create View Assignment_Detail
As
Select a.Id, a.Title, a.DueDate, a.Status, a.Priority, a.EstimatedHours, c.UserId, c.Title AS CourseTitle, cw.Title AS WeekTitle
From Assignments as a
Inner join CourseWeeks as cw 
On a.CourseWeekId = cw.Id
Inner join Courses c ON cw.CourseId = c.Id;
Go
-- CRUD Opration Course
-- For Calendar
Create Procedure Get_Calendar(@UsedId nvarchar(450),@StartDate datetime, @EndDate datetime)
As
Begin
    -- check UserId must exist in database 
    IF Exists(Select 1 From AspNetUsers Where Id = @UsedId)
        Begin
            --Can Get rn
            Select a.Id, a.Title, a.DueDate, a.Status, a.Priority, a.EstimatedHours, c.Title AS CourseTitle, cw.Title AS WeekTitle From Assignments as a
            Inner join CourseWeeks as cw on a.CourseWeekId = cw.Id
            Inner join Courses c on cw.CourseId = c.Id
            Where c.UserId = @UsedId and a.DueDate Between @StartDate And @EndDate
            Order by a.Priority DESC, a.DueDate ASC;
        End
    ELSE
        Begin
            Throw 50000,'Error User Id', 1;
        End
End
Go
Create Procedure Get_Important_Tasks(@UserId nvarchar(450), @Days Int = 7, @NumberOfTask Int=15)
As
Begin
    IF Exists(Select 1 From AspNetUsers Where Id = @UserId)
        Begin
            --Can Get rn
            Select Top (@NumberOfTask) a.Id, a.Title, a.DueDate, a.Status, a.Priority, a.EstimatedHours, c.Title AS CourseTitle, cw.Title AS WeekTitle From Assignments as a
            Inner join CourseWeeks as cw on a.CourseWeekId = cw.Id
            Inner join Courses c on cw.CourseId = c.Id
            Where c.UserId = @UserId and a.Status <> 'Finish' and a.DueDate <= DATEADD(DAY,@Days,GETDATE());
        End
    ELSE
        Begin
            Throw 50000,'Error User Id', 1;
        End
End
Go
-- CRUD on Assigment
Create Procedure Create_Assignment(@UserId nvarchar(450), @CourseWeekId int, @Title nvarchar(256), @Description nvarchar(256) = '', @DueDate datetime, @Grade int, @Weight int, @Status nvarchar(256), @EstimatedHours decimal(4,1) = 1.0 )
As
Begin
    IF Exists(Select 1 From AspNetUsers Where Id = @UserId) And Exists(Select 1 From CourseWeeks Where Id = @CourseWeekId)
    Begin
        --Can Insert rn
        Insert into Assignments(CourseWeekId, Title, Description, DueDate, Grade, Weight, Status, EstimatedHours)
        Values(@CourseWeekId, @Title, @Description, @DueDate, @Grade, @Weight, @Status, @EstimatedHours); 
    End
    ELSE
    Begin
        Throw 50000,'Error User Id Or WeekId', 1;
    End
End
Go
Create Procedure Update_Assignment_Status(@AssignmentId int, @Status nvarchar(256))
As
Begin
 IF Exists(Select 1 From Assignments Where Id = @AssignmentId) 
    Begin
        Update Assignments SET Status = @Status, CompleteDate = Case
            When @Status = 'Finish' Then GETDATE()
            Else Null
        End
        Where Id = @AssignmentId;
    End
    ELSE
    Begin
        Throw 50000,'Error User Id', 1;
    End
End
Go
-- Delete Assignment
Create Procedure Delete_Assignment(@WeekId Int, @AssignmentId Int)
As
Begin
    If(Exists(Select 1 From CourseWeeks as cw inner join Assignments as a on cw.Id = a.CourseWeekId where cw.Id = @WeekId And a.Id = @AssignmentId))
    Begin
        Delete From Assignments Where Id = @AssignmentId;
    End
    ELse
    Begin
        Throw 50000,'CourseWeek Not Found Or Assignment Not Found',1;
    End
End
-- Trigger: Auto-Update OverDue
Create Trigger Assignment_OverDue On Assignments After Insert, Update
As 
Begin
    Update a Set Status = 'OverDue' From Assignments as a
    Join inserted i on a.Id = i.Id 
    Where a.Status <> 'Finish' And a.DueDate < GETDATE();
End
-- Calculate Priority
Go
Create trigger Assignment_Priority On Assignments After insert, update
As
Begin
	Update a
	Set priority =
		case
			when a.duedate < getdate() then 100
			when datediff(day, getdate(), a.duedate) <= 1 then 90
			when datediff(day, getdate(), a.duedate) <= 3 then 75
			when datediff(day, getdate(), a.duedate) <= 7 then 50
			else 25
		end + isnull(a.weight, 0)
	from Assignments a
	inner join inserted i on a.id = i.id;
End
Go

-- CRUD on COURSE
Create procedure Create_Course(@UserId nvarchar(450),@title nvarchar(256),@description nvarchar(256) = null,@semester nvarchar(256) = null,@professor nvarchar(256) = null,@coursecode nvarchar(256))
As
Begin
	If Exists(Select 1 from AspNetUsers Where Id = @UserId)
	Begin
		Insert into Courses(UserId, Title, Description, Semester, Professor, CourseCode)
		values(@UserId, @title, @description, @semester, @professor, @coursecode);
	End
	Else
	Begin
		Throw 50000, 'error user id', 1;
	End
End
Go
Create procedure Get_Courses_By_User(@Userid nvarchar(450))
As
Begin
	If Exists(Select 1 from AspNetUsers Where id = @userid)
	Begin
		Select * from Courses Where UserId = @UserId order by CreateDate desc;
	End
	Else
	Begin
		Throw 50000, 'error user id', 1;
	End
End
Go
Create procedure Update_Course(@CourseId int, @title nvarchar(256), @description nvarchar(256), @semester nvarchar(256), @professor nvarchar(256), @coursecode nvarchar(256))
As
Begin
	Update Courses Set Title = @title, Description = @description, Semester = @semester, Professor = @professor, Coursecode = @coursecode Where Id = @CourseId;
End
Go
Create procedure Delete_Course(@CourseId int)
As
Begin
	Delete from Courses Where id = @courseid;
End
Go
-- CRUD on CourseWeek
Create Procedure Get_CourseWeeks_By_Course(@courseid int)
As
Begin
	If exists(Select 1 from Courses Where id = @courseid)
	Begin
		Select * from CourseWeeks Where CourseId = @courseid Order by id ASC;
	End
	Else
	Begin
		throw 50000, 'error course id', 1;
	End
End
Go
Create procedure Create_CourseWeek(@courseid int, @title nvarchar(256), @description nvarchar(256) = null)
As
Begin
	If exists(select 1 from Courses where Id = @courseid)
	Begin
		Insert into CourseWeeks(courseid,title,description)
		Values(@courseid,@title,@description);
	End
	Else
	Begin
		Throw 50000, 'error course id', 1;
	End
End
Go
Create procedure Update_CourseWeek(@courseweekid int, @title nvarchar(256), @description nvarchar(256))
As
Begin
	Update CourseWeeks Set title = @title, description = @description Where id = @courseweekid;
End
Go
Create procedure Delete_CourseWeek(@courseweekid int)
As
Begin
	Delete from CourseWeeks Where id = @courseweekid;
End
go

-- Trigger For Course And CourseWeek Table
-- Fix Priority Error In Add Assingment
ALTER TABLE Assignments ADD CONSTRAINT Assignments_Priority DEFAULT 0 FOR Priority;