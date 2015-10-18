ALTER TABLE [dbo].[StoreProduct]
    ADD CONSTRAINT [DF_StoreProduct_PortalID] DEFAULT ((1)) FOR [PortalID];

