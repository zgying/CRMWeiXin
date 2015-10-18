
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
-- Create date: 3/27/2012
-- Modified date: 7/21/2012
-- Description:	Clears database for fresh install
-- Usage: EXEC [dbo].[usp_ClearDatabase] @Confirmation=1
-- =============================================
CREATE PROCEDURE [dbo].[usp_ClearDatabase] @Confirmation BIT = 0
AS
  BEGIN
      IF @Confirmation = 1
        BEGIN
        --TRUNCATE TABLE [dbo].[Content]
        --TRUNCATE TABLE [dbo].[ContentFiles]
        TRUNCATE TABLE [dbo].[ELMAH_Error]
        --TRUNCATE TABLE [dbo].[IPN]
        --TRUNCATE TABLE [dbo].[PortalAlias]
        --TRUNCATE TABLE [dbo].[Portals]
        --TRUNCATE TABLE [dbo].[Roles]
        TRUNCATE TABLE [dbo].[SiteLog]
        TRUNCATE TABLE [dbo].[StoreCart]
        --TRUNCATE TABLE [dbo].[StoreCartItems]
        --TRUNCATE TABLE [dbo].[StoreCategory]
        --TRUNCATE TABLE [dbo].[StoreProduct]
        --TRUNCATE TABLE [dbo].[StoreVariantGroups]
        --TRUNCATE TABLE [dbo].[StoreVariants]
        --TRUNCATE TABLE [dbo].[StoreVariantTypes]
        --TRUNCATE TABLE [dbo].[SurveyAnswerGroups]
        --TRUNCATE TABLE [dbo].[SurveyAnswers]
        --TRUNCATE TABLE [dbo].[SurveyAnswersTypes]
        --TRUNCATE TABLE [dbo].[SurveyQuestions]
        --TRUNCATE TABLE [dbo].[SurveyResultItems]
        --TRUNCATE TABLE [dbo].[SurveyResults]
        --TRUNCATE TABLE [dbo].[Surveys]
        --TRUNCATE TABLE [dbo].[Tags]
        --TRUNCATE TABLE [dbo].[TaxRate]
        --TRUNCATE TABLE [dbo].[Users]
        --TRUNCATE TABLE [dbo].[UsersInRoles]
        END
  END