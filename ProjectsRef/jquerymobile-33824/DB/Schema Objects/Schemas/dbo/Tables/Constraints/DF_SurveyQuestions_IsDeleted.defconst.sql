ALTER TABLE [dbo].[SurveyQuestions]
    ADD CONSTRAINT [DF_SurveyQuestions_IsDeleted] DEFAULT ((0)) FOR [IsDeleted];

