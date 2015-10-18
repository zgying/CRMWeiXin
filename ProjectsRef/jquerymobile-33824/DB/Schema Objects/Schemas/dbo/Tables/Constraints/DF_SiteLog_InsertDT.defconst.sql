ALTER TABLE [dbo].[SiteLog]
    ADD CONSTRAINT [DF_SiteLog_InsertDT] DEFAULT (getdate()) FOR [CreateDate];

