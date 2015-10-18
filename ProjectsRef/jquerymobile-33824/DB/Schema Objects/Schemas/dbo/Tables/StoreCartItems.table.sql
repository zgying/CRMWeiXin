CREATE TABLE [dbo].[StoreCartItems] (
    [CartItemID] INT      IDENTITY (1, 1) NOT NULL,
    [PortalID]   INT      NOT NULL,
    [CreateDate] DATETIME NOT NULL,
    [CartID]     INT      NOT NULL,
    [ProductID]  INT      NOT NULL,
    [TaxRate]    REAL     NOT NULL,
    [Qty]        INT      NOT NULL,
    [Price]      MONEY    NOT NULL,
    [SubTotal]   MONEY    NOT NULL,
    [Tax]        MONEY    NOT NULL,
    [Total]      MONEY    NOT NULL
);









