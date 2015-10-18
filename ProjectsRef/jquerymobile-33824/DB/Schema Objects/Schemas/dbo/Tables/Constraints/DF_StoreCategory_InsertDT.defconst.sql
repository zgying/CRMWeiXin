ALTER TABLE [dbo].[StoreCategory]
    ADD CONSTRAINT [DF_StoreCategory_InsertDT] DEFAULT (getdate()) FOR [CreateDate];

