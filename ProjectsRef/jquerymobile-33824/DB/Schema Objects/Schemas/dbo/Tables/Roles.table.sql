CREATE TABLE [dbo].[Roles] (
    [RoleID]      INT            IDENTITY (1, 1) NOT NULL,
    [CreateDate]  DATETIME       NOT NULL,
    [PortalID]    INT            NOT NULL,
    [RoleName]    NVARCHAR (100) NOT NULL,
    [Description] NVARCHAR (255) NULL
);

