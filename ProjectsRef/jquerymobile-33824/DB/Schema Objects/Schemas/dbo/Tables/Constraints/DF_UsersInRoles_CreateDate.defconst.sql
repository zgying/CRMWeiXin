ALTER TABLE [dbo].[UsersInRoles]
    ADD CONSTRAINT [DF_UsersInRoles_CreateDate] DEFAULT (getdate()) FOR [CreateDate];

