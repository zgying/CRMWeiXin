ALTER TABLE [dbo].[SurveyAnswers]
    ADD CONSTRAINT [DF_SurveyAnswers_IsDeleted] DEFAULT ((0)) FOR [IsDeleted];

