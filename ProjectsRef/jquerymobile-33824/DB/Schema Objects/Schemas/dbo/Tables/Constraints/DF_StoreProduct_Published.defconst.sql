ALTER TABLE [dbo].[StoreProduct]
    ADD CONSTRAINT [DF_StoreProduct_Published] DEFAULT ((0)) FOR [Published];

