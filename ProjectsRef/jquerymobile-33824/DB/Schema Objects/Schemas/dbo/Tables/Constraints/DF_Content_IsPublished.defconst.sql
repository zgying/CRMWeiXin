ALTER TABLE [dbo].[Content]
    ADD CONSTRAINT [DF_Content_IsPublished] DEFAULT ((0)) FOR [IsPublished];

