ALTER TABLE [dbo].[StoreCartItems]
    ADD CONSTRAINT [DF_StoreCartItems_InsertDT] DEFAULT (getdate()) FOR [CreateDate];

