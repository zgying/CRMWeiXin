ALTER TABLE [dbo].[SurveyAnswerGroups]
    ADD CONSTRAINT [DF_SurveyAnswerGroups_InsertDT] DEFAULT (getdate()) FOR [CreateDate];

