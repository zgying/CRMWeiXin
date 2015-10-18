ALTER TABLE [dbo].[Roles]
    ADD CONSTRAINT [DF_Roles_CreateDate] DEFAULT (getdate()) FOR [CreateDate];

