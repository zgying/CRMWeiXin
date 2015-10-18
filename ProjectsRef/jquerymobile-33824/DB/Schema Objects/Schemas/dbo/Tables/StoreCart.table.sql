CREATE TABLE [dbo].[StoreCart] (
    [CartID]     INT      IDENTITY (1, 1) NOT NULL,
    [PortalID]   INT      NOT NULL,
    [CreateDate] DATETIME NOT NULL,
    [UserID]     INT      NULL,
    [SubTotal]   MONEY    NULL,
    [Tax]        MONEY    NULL,
    [Total]      MONEY    NULL
);





