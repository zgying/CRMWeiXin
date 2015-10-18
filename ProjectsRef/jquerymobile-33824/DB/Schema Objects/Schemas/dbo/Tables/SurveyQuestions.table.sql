CREATE TABLE [dbo].[SurveyQuestions] (
    [QuestionID]    INT            IDENTITY (1, 1) NOT NULL,
    [PortalID]      INT            NOT NULL,
    [DisplayOrder]  INT            NOT NULL,
    [SurveyID]      INT            NOT NULL,
    [AnswerGroupID] INT            NOT NULL,
    [Question]      NVARCHAR (255) NOT NULL,
    [IsDeleted]     BIT            NOT NULL
);







