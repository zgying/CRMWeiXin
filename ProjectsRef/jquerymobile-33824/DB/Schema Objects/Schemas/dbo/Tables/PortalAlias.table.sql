CREATE TABLE [dbo].[PortalAlias] (
    [PortalAliasID]        INT            IDENTITY (1, 1) NOT NULL,
    [CreateDate]           DATETIME       NOT NULL,
    [PortalID]             INT            NOT NULL,
    [HTTPAlias]            NVARCHAR (200) NOT NULL,
    [LastUpdated]          DATETIME       NULL,
    [CreatedByUserID]      INT            NOT NULL,
    [LastModifiedByUserID] INT            NULL
);

