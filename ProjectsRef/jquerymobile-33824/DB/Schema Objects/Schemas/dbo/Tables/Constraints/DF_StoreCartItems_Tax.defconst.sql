ALTER TABLE [dbo].[StoreCartItems]
    ADD CONSTRAINT [DF_StoreCartItems_Tax] DEFAULT ((0.00)) FOR [TaxRate];



