ALTER TABLE [dbo].[Surveys]
    ADD CONSTRAINT [DF_Surveys_InsertDT] DEFAULT (getdate()) FOR [CreateDate];

