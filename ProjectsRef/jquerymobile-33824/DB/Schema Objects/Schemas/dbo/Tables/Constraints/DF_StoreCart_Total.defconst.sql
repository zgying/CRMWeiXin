ALTER TABLE [dbo].[StoreCart]
    ADD CONSTRAINT [DF_StoreCart_Total] DEFAULT ((0.00)) FOR [Total];

