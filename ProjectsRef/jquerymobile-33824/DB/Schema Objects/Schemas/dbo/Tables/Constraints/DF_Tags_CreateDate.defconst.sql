ALTER TABLE [dbo].[Tags]
    ADD CONSTRAINT [DF_Tags_CreateDate] DEFAULT (getdate()) FOR [CreateDate];

