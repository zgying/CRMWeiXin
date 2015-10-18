ALTER TABLE [dbo].[StoreCartItems]
    ADD CONSTRAINT [DF_StoreCartItems_Price] DEFAULT ((0.00)) FOR [Price];

