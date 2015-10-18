ALTER TABLE [dbo].[Content]
    ADD CONSTRAINT [DF_Content_CreateDate] DEFAULT (getdate()) FOR [CreateDate];

