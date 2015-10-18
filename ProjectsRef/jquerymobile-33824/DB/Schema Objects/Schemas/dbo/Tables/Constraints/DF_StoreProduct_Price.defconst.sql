ALTER TABLE [dbo].[StoreProduct]
    ADD CONSTRAINT [DF_StoreProduct_Price] DEFAULT ((0.00)) FOR [Price];

