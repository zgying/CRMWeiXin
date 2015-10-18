ALTER TABLE [dbo].[StoreCart]
    ADD CONSTRAINT [DF_StoreCart_SubTotal] DEFAULT ((0.00)) FOR [SubTotal];

