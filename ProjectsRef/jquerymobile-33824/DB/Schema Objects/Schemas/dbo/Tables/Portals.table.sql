CREATE TABLE [dbo].[Portals] (
    [PortalID]             INT            IDENTITY (1, 1) NOT NULL,
    [CreateDate]           DATETIME       NOT NULL,
    [LastUpdated]          DATETIME       NULL,
    [CreatedByUserID]      INT            NOT NULL,
    [LastModifiedByUserID] INT            NULL,
    [Name]                 NVARCHAR (50)  NOT NULL,
    [MasterPage]           NVARCHAR (50)  NULL,
    [Theme]                NVARCHAR (50)  NULL,
    [UrlRedirect]          NVARCHAR (255) NULL,
    [FacebookAppID]        NVARCHAR (20)  NULL,
    [FacebookSecret]       NVARCHAR (50)  NULL,
    [PayPalEnvironment]    BIT            NULL,
    [PayPalBusiness]       NVARCHAR (50)  NULL,
    [EmailHost]            NVARCHAR (50)  NULL,
    [EmailPort]            NCHAR (10)     NULL,
    [EmailUser]            NVARCHAR (50)  NULL,
    [EmailPass]            NVARCHAR (250) NULL,
    [EmailPassSalt]        NVARCHAR (250) NULL,
    [EmailSSL]             BIT            NULL,
    [GoogleAnalytics]      TEXT           NULL,
    [RobotsText]           TEXT           NULL,
    [StartMethod]          NVARCHAR (255) NULL,
    [TaxRate]              REAL           NULL
);











