ALTER TABLE [dbo].[Users]
    ADD CONSTRAINT [DF_Users_FailedPasswordAttemptCount] DEFAULT ((0)) FOR [FailedPasswordAttemptCount];

