CREATE TABLE [dbo].[StoreVariantGroups] (
    [VariantGroupID] INT           IDENTITY (1, 1) NOT NULL,
    [PortalID]       INT           NOT NULL,
    [DisplayOrder]   INT           NOT NULL,
    [CreateDate]     DATETIME      NOT NULL,
    [VariantTypeID]  INT           NOT NULL,
    [Name]           NVARCHAR (50) NOT NULL,
    [IsDeleted]      BIT           NOT NULL
);



