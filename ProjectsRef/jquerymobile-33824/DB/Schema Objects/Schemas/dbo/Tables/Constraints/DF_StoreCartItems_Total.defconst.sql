ALTER TABLE [dbo].[StoreCartItems]
    ADD CONSTRAINT [DF_StoreCartItems_Total] DEFAULT ((0.00)) FOR [Total];

