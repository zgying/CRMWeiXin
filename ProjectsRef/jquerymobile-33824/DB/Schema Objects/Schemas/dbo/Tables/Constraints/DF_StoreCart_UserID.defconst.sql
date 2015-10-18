ALTER TABLE [dbo].[StoreCart]
    ADD CONSTRAINT [DF_StoreCart_UserID] DEFAULT ((0)) FOR [UserID];

