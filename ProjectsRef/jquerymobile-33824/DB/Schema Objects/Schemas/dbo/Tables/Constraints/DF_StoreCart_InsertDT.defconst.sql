ALTER TABLE [dbo].[StoreCart]
    ADD CONSTRAINT [DF_StoreCart_InsertDT] DEFAULT (getdate()) FOR [CreateDate];

