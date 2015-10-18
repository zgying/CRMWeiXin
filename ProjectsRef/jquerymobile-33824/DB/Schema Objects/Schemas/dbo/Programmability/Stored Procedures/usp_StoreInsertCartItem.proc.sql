
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
-- Alter date:  6/18/2012
-- Description:	Insert store item
-- EXEC [dbo].[usp_StoreInsertCartItem] @CartID=1, @Qty=1, @ProductID=1, @PortalID=1
-- =============================================
CREATE PROCEDURE [dbo].[usp_StoreInsertCartItem] @CartID    INT,
                                                @Qty       INT = 1,
                                                @ProductID INT,
                                                @PortalID  INT
AS
  BEGIN
      SET NOCOUNT ON;

      DECLARE @Price        MONEY,
              @TaxRate      REAL,
              @ItemTax      MONEY,
              @CartTax      MONEY,
              @ItemSubTotal MONEY,
              @CartSubTotal MONEY,
              @ItemTotal    MONEY,
              @CartTotal    MONEY

      SELECT @Price = [Price]
      FROM   [dbo].[StoreProduct] WITH (NOLOCK)
      WHERE  ProductID = @ProductID

      SELECT @TaxRate = [TaxRate]
      FROM   [dbo].[Portals] WITH (NOLOCK)
      WHERE  PortalID = @PortalID

      SET @ItemSubTotal = @Qty * @Price
      
      SET @ItemTax = @ItemSubTotal * @TaxRate
     
     -- Round the tax before rounding before adding to the total 
      SET @ItemTax = ROUND(@ItemTax, 2)
      
      SET @ItemTotal = @ItemTax + @ItemSubTotal

      INSERT INTO [dbo].[StoreCartItems]
                  ([PortalID],
                   [CreateDate],
                   [CartID],
                   [ProductID],
                   [Qty],
                   [Price],
                   [TaxRate],
                   [SubTotal],
                   [Tax],
                   [Total])
      VALUES      (@PortalID,
                   GETDATE(),
                   @CartID,
                   @ProductID,
                   @Qty,
                   @Price,
                   @TaxRate,
                   @ItemSubTotal,
                   @ItemTax,
                   @ItemTotal )

      SELECT @CartSubTotal = SUM([SubTotal])
             ,@CartTax = SUM([Tax])
             ,@CartTotal = SUM([Total])
      FROM   [dbo].[StoreCartItems] WITH (NOLOCK)
      WHERE  CartID = @CartID

      UPDATE [dbo].[StoreCart]
      SET    [SubTotal] = @CartSubTotal,
             [Tax] = @CartTax,
             [Total] = @CartTotal
      WHERE  CartID = @CartID
      
  END