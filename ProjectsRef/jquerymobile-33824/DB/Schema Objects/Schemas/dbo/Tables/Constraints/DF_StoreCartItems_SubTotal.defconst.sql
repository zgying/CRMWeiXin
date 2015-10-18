ALTER TABLE [dbo].[StoreCartItems]
    ADD CONSTRAINT [DF_StoreCartItems_SubTotal] DEFAULT ((0.00)) FOR [SubTotal];

