ALTER TABLE [dbo].[StoreCart]
    ADD CONSTRAINT [DF_StoreCart_Tax] DEFAULT ((0.00)) FOR [Tax];

