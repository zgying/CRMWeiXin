ALTER TABLE [dbo].[Users]
    ADD CONSTRAINT [DF_Users_LastLoginDate] DEFAULT (getdate()) FOR [LastLoginDate];

