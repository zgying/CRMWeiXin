CREATE TABLE [dbo].[StoreProduct] (
    [ProductID]    INT           IDENTITY (1, 1) NOT NULL,
    [PortalID]     INT           NULL,
    [DisplayOrder] INT           NULL,
    [CreateDate]   DATETIME      NULL,
    [CategoryID]   INT           NULL,
    [Name]         NVARCHAR (50) NULL,
    [Description]  TEXT          NULL,
    [Published]    BIT           NULL,
    [Deleted]      BIT           NULL,
    [Price]        MONEY         NULL
);





