ALTER TABLE [dbo].[StoreProduct]
    ADD CONSTRAINT [DF_StoreProduct_InsertDT] DEFAULT (getdate()) FOR [CreateDate];

