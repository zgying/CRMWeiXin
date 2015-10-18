CREATE TABLE [dbo].[SurveyAnswerGroups] (
    [AnswerGroupID] INT           IDENTITY (1, 1) NOT NULL,
    [CreateDate]    DATETIME      NOT NULL,
    [PortalID]      INT           NOT NULL,
    [AnswerTypeID]  INT           NOT NULL,
    [Name]          NVARCHAR (50) NOT NULL,
    [IsDeleted]     BIT           NOT NULL
);



