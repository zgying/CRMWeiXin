CREATE TABLE [dbo].[UsersInRoles] (
    [RecID]      INT      IDENTITY (1, 1) NOT NULL,
    [CreateDate] DATETIME NOT NULL,
    [PortalID]   INT      NOT NULL,
    [RoleID]     INT      NOT NULL,
    [UserID]     INT      NOT NULL
);

