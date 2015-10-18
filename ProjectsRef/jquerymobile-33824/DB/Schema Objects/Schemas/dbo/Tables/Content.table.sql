CREATE TABLE [dbo].[Content] (
    [ContentID]     INT            IDENTITY (1, 1) NOT NULL,
    [DisplayOrder]  INT            NOT NULL,
    [CreateDate]    DATETIME       NOT NULL,
    [PortalID]      INT            NOT NULL,
    [UserID]        INT            NOT NULL,
    [Title]         NVARCHAR (50)  NOT NULL,
    [URL]           NVARCHAR (200) NOT NULL,
    [ContentTypeID] INT            NOT NULL,
    [ContentText]   TEXT           NULL,
    [IsPublished]   BIT            NULL,
    [IsMenu]        BIT            NULL,
    [StartMethod]   NVARCHAR (255) NULL
);









