CREATE TABLE [dbo].[TaxRate] (
    [TaxRateID]  INT             IDENTITY (1, 1) NOT NULL,
    [CreateDate] DATETIME        NOT NULL,
    [PortalID]   INT             NOT NULL,
    [Name]       NVARCHAR (50)   NOT NULL,
    [Rate]       DECIMAL (18, 2) NOT NULL
);

