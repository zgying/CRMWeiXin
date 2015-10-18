
/*  
 *  Copyright © 2012 Matthew David Elgert - mdelgert@yahoo.com
 *
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation; either version 2.1 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with this program; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA 
 * 
 *  Project URL: http://jquerymobile.codeplex.com/                           
 *  
 */
-- =============================================
-- Author:		Matthew David Elgert
-- Create date: 6/17/2012
-- Description:	Gets cart items
-- EXEC dbo.usp_StoreGetCart @CartID=3
-- =============================================
CREATE PROCEDURE [dbo].[usp_StoreGetCart] @CartID INT
AS
  BEGIN
      SET NOCOUNT ON;

      SELECT dbo.StoreCart.CartID
             ,dbo.StoreProduct.ProductID
             ,dbo.StoreProduct.Name               AS Item
             ,COUNT(dbo.StoreCartItems.ProductID) AS Qty
             ,dbo.StoreProduct.Price
             ,SUM(dbo.StoreProduct.Price)         AS SubTotal
      FROM   dbo.StoreCart WITH (NOLOCK)
             INNER JOIN dbo.StoreCartItems WITH (NOLOCK)
               ON dbo.StoreCart.CartID = dbo.StoreCartItems.CartID
             INNER JOIN dbo.StoreProduct WITH (NOLOCK)
               ON dbo.StoreCartItems.ProductID = dbo.StoreProduct.ProductID
      WHERE  dbo.StoreCart.CartID = @CartID
      GROUP  BY dbo.StoreCart.CartID
                ,dbo.StoreProduct.ProductID
                ,dbo.StoreProduct.Name
                ,dbo.StoreProduct.Price
  END