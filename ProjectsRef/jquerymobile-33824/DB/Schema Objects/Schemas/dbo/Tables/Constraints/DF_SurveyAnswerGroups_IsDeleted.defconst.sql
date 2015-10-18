ALTER TABLE [dbo].[SurveyAnswerGroups]
    ADD CONSTRAINT [DF_SurveyAnswerGroups_IsDeleted] DEFAULT ((0)) FOR [IsDeleted];

