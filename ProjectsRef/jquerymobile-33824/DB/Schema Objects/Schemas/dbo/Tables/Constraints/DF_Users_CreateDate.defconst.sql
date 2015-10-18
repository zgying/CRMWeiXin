ALTER TABLE [dbo].[Users]
    ADD CONSTRAINT [DF_Users_CreateDate] DEFAULT (getdate()) FOR [CreateDate];

