CREATE TABLE [dbo].[StoreCategory] (
    [CategoryID]       INT           IDENTITY (1, 1) NOT NULL,
    [PortalID]         INT           NOT NULL,
    [DisplayOrder]     INT           NOT NULL,
    [CreateDate]       DATETIME      NOT NULL,
    [Name]             NVARCHAR (50) NOT NULL,
    [ParentCategoryId] INT           NULL,
    [IsMenu]           BIT           NULL,
    [Deleted]          BIT           NULL
);





