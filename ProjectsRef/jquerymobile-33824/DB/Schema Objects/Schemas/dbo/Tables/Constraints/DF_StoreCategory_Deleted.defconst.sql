ALTER TABLE [dbo].[StoreCategory]
    ADD CONSTRAINT [DF_StoreCategory_Deleted] DEFAULT ((0)) FOR [Deleted];

