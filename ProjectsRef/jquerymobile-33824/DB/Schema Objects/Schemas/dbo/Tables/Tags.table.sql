CREATE TABLE [dbo].[Tags] (
    [TagID]      INT           IDENTITY (1, 1) NOT NULL,
    [CreateDate] DATETIME      NOT NULL,
    [PortalID]   INT           NOT NULL,
    [ContentID]  INT           NOT NULL,
    [Tag]        NVARCHAR (50) NOT NULL
);

