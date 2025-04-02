CREATE TABLE [dbo].[LeaveRequests]
(
	[Id] INT NOT NULL IDENTITY ,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    Reason NVARCHAR(500),
    Status INT NOT NULL, -- Enum (Pending = 0, Approved = 1, Rejected = 2)
    EmployeeId INT NOT NULL,
    ApproverId INT NULL,
    CONSTRAINT FK_LeaveRequests_Employee FOREIGN KEY (EmployeeId) REFERENCES Employees(Id) ON DELETE CASCADE,
    CONSTRAINT FK_LeaveRequests_Manager FOREIGN KEY (ApproverId) REFERENCES Managers(Id) ON DELETE NO ACTION,
     CONSTRAINT [PK_LeaveRequests] PRIMARY KEY ([Id]),
)
