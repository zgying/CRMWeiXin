CREATE TABLE [dbo].[SurveyAnswers] (
    [AnswerID]      INT           IDENTITY (1, 1) NOT NULL,
    [PortalID]      INT           NOT NULL,
    [DisplayOrder]  INT           NOT NULL,
    [AnswerGroupID] INT           NOT NULL,
    [Answer]        NVARCHAR (50) NOT NULL,
    [IsDeleted]     BIT           NOT NULL
);





