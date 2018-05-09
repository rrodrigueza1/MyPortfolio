CREATE TABLE Project (
    ProjectId int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    ProjectName varchar(50) NOT NULL,
    ProjectDescription varchar(255) NOT NULL,
	ProjectList varchar(255) NOT NULL,
	DateCreated date NOT NULL,
	DateFinished date NULL,
);

CREATE TABLE Image (
    ImageId int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    ImageName varchar(50) NOT NULL,
    ImageDescription varchar(255) NOT NULL,
	Picture image NOT NULL,
	ProjectId int NOT NULL,
	CONSTRAINT FK_ProjectImage FOREIGN KEY (ProjectId)
    REFERENCES Project(ProjectId)
);