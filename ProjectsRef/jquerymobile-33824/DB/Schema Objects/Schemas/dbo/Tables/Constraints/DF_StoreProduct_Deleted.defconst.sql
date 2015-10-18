ALTER TABLE [dbo].[StoreProduct]
    ADD CONSTRAINT [DF_StoreProduct_Deleted] DEFAULT ((0)) FOR [Deleted];

