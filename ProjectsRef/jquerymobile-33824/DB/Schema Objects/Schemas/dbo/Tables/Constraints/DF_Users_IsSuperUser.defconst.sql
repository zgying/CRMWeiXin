ALTER TABLE [dbo].[Users]
    ADD CONSTRAINT [DF_Users_IsSuperUser] DEFAULT ((0)) FOR [IsSuperUser];

