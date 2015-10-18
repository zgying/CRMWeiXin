
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
-- Create date: 12/18/2011
-- Alter date:  4/17/2012
-- Description:	Returns users roles
-- EXEC dbo.usp_GetUserRoles @UserID=1
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetUserRoles] @UserID INT
AS
  BEGIN
      SELECT R.RoleID
             ,R.RoleName
             ,UR.CreateDate
      FROM   dbo.Roles R WITH (NOLOCK) 
             INNER JOIN dbo.UsersInRoles UR WITH (NOLOCK) 
               ON R.RoleID = UR.RoleID
                  AND UR.UserID = @UserID
      WHERE  UR.UserID = @UserID
  END