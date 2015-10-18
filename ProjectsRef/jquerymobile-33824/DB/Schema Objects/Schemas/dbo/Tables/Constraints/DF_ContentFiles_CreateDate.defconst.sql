ALTER TABLE [dbo].[ContentFiles]
    ADD CONSTRAINT [DF_ContentFiles_CreateDate] DEFAULT (getdate()) FOR [CreateDate];

