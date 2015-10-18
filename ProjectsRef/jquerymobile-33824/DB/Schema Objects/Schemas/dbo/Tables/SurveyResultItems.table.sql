CREATE TABLE [dbo].[SurveyResultItems] (
    [ItemID]     INT            IDENTITY (1, 1) NOT NULL,
    [ResultID]   INT            NOT NULL,
    [PortalID]   INT            NOT NULL,
    [QuestionID] INT            NOT NULL,
    [AnswerID]   INT            NOT NULL,
    [UserID]     INT            NOT NULL,
    [AnswerText] NVARCHAR (255) NULL
);



