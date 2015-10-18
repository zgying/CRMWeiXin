
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
CREATE PROCEDURE [dbo].[ELMAH_LogError] (@ErrorId     UNIQUEIDENTIFIER,
                                        @Application NVARCHAR(60),
                                        @Host        NVARCHAR(30),
                                        @Type        NVARCHAR(100),
                                        @Source      NVARCHAR(60),
                                        @Message     NVARCHAR(500),
                                        @User        NVARCHAR(50),
                                        @AllXml      NTEXT,
                                        @StatusCode  INT,
                                        @TimeUtc     DATETIME)
AS
  SET NOCOUNT ON

  INSERT INTO [ELMAH_Error]
              ([ErrorId],
               [Application],
               [Host],
               [Type],
               [Source],
               [Message],
               [User],
               [AllXml],
               [StatusCode],
               [TimeUtc])
  VALUES      ( @ErrorId,
                @Application,
                @Host,
                @Type,
                @Source,
                @Message,
                @User,
                @AllXml,
                @StatusCode,
                @TimeUtc )