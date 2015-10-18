CREATE TABLE [dbo].[Users] (
    [UserID]                     INT            IDENTITY (1, 1) NOT NULL,
    [IsSuperUser]                BIT            NOT NULL,
    [CreateDate]                 DATETIME       NOT NULL,
    [PortalID]                   INT            NOT NULL,
    [Email]                      NVARCHAR (250) NOT NULL,
    [Password]                   NVARCHAR (250) NOT NULL,
    [PasswordSalt]               NVARCHAR (250) NOT NULL,
    [IsLockedOut]                BIT            NOT NULL,
    [FailedPasswordAttemptCount] INT            NOT NULL,
    [LastLoginDate]              DATETIME       NOT NULL,
    [FacebookID]                 NVARCHAR (255) NULL,
    [FullName]                   NVARCHAR (255) NULL,
    [FirstName]                  NVARCHAR (50)  NULL,
    [MiddleName]                 NVARCHAR (50)  NULL,
    [LastName]                   NVARCHAR (50)  NULL,
    [Gender]                     NVARCHAR (10)  NULL,
    [Local]                      NVARCHAR (20)  NULL,
    [Link]                       NVARCHAR (255) NULL,
    [TimeZone]                   INT            NULL,
    [Birthday]                   DATETIME       NULL,
    [Picture]                    NVARCHAR (255) NULL,
    [LocationID]                 NVARCHAR (255) NULL,
    [Address]                    NVARCHAR (60)  NULL,
    [City]                       NVARCHAR (50)  NULL,
    [Region]                     NVARCHAR (50)  NULL,
    [PostalCode]                 NCHAR (15)     NULL,
    [Telephone]                  NVARCHAR (50)  NULL,
    [AltPhone]                   NVARCHAR (50)  NULL
);



