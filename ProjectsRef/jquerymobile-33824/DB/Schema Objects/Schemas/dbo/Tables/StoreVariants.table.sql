CREATE TABLE [dbo].[StoreVariants] (
    [VariantID]      INT           IDENTITY (1, 1) NOT NULL,
    [PortalID]       INT           NOT NULL,
    [DisplayOrder]   INT           NOT NULL,
    [VariantGroupID] INT           NOT NULL,
    [Variant]        NVARCHAR (50) NOT NULL,
    [Price]          MONEY         NOT NULL,
    [IsDeleted]      BIT           NOT NULL
);



