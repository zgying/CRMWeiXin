CREATE TABLE [dbo].[ContentFiles] (
    [FileID]      INT           IDENTITY (1, 1) NOT NULL,
    [CreateDate]  DATETIME      NULL,
    [PortalID]    INT           NOT NULL,
    [ParentID]    INT           NULL,
    [Name]        NVARCHAR (50) NULL,
    [MimeType]    NVARCHAR (50) NULL,
    [IsDirectory] BIT           NOT NULL,
    [Size]        INT           NULL,
    [FileContent] IMAGE         NULL,
    [upsize_ts]   TIMESTAMP     NOT NULL
);

