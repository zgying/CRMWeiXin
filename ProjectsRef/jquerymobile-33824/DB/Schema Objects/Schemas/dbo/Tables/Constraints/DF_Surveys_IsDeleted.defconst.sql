ALTER TABLE [dbo].[Surveys]
    ADD CONSTRAINT [DF_Surveys_IsDeleted] DEFAULT ((0)) FOR [IsDeleted];

