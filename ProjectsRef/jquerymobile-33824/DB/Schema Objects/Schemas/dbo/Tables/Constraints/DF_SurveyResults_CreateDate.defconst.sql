ALTER TABLE [dbo].[SurveyResults]
    ADD CONSTRAINT [DF_SurveyResults_CreateDate] DEFAULT (getdate()) FOR [CreateDate];

