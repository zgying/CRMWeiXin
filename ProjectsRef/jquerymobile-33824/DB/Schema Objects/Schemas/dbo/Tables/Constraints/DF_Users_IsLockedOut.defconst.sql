ALTER TABLE [dbo].[Users]
    ADD CONSTRAINT [DF_Users_IsLockedOut] DEFAULT ((0)) FOR [IsLockedOut];

