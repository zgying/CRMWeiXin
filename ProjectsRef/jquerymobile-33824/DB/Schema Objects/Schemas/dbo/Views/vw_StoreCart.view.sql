/*CREATE VIEW dbo.vw_StoreCart
AS
SELECT     dbo.StoreCart.CartID, dbo.StoreProduct.ProductID, dbo.StoreProduct.Name AS Item, dbo.StoreProduct.Price
FROM         dbo.StoreCart INNER JOIN
                      dbo.StoreCartItems ON dbo.StoreCart.CartID = dbo.StoreCartItems.CartID INNER JOIN
                      dbo.StoreProduct ON dbo.StoreCartItems.ProductID = dbo.StoreProduct.ProductID*/