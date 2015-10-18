ALTER TABLE [dbo].[StoreCartItems]
    ADD CONSTRAINT [DF_StoreCartItems_Qty] DEFAULT ((1)) FOR [Qty];

