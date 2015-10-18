
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
CREATE PROCEDURE [dbo].[ELMAH_GetErrorsXml] (@Application NVARCHAR(60),
                                            @PageIndex   INT = 0,
                                            @PageSize    INT = 15,
                                            @TotalCount  INT OUTPUT)
AS
  SET NOCOUNT ON

  DECLARE @FirstTimeUTC DATETIME
  DECLARE @FirstSequence INT
  DECLARE @StartRow INT
  DECLARE @StartRowIndex INT

  SELECT @TotalCount = COUNT(1)
  FROM   [ELMAH_Error] WITH (NOLOCK)
  WHERE  [Application] = @Application

  -- Get the ID of the first error for the requested page
  SET @StartRowIndex = @PageIndex * @PageSize + 1

  IF @StartRowIndex <= @TotalCount
    BEGIN
        SET ROWCOUNT @StartRowIndex

        SELECT @FirstTimeUTC = [TimeUtc]
               ,@FirstSequence = [Sequence]
        FROM   [ELMAH_Error] WITH (NOLOCK)
        WHERE  [Application] = @Application
        ORDER  BY [TimeUtc] DESC
                  ,[Sequence] DESC
    END
  ELSE
    BEGIN
        SET @PageSize = 0
    END

  -- Now set the row count to the requested page size and get
  -- all records below it for the pertaining application.
  SET ROWCOUNT @PageSize

  SELECT errorId = [ErrorId]
         ,application = [Application]
         ,host = [Host]
         ,type = [Type]
         ,source = [Source]
         ,message = [Message]
         ,[user] = [User]
         ,statusCode = [StatusCode]
         ,time = CONVERT(VARCHAR(50), [TimeUtc], 126) + 'Z'
  FROM   [ELMAH_Error] error WITH (NOLOCK)
  WHERE  [Application] = @Application
         AND [TimeUtc] <= @FirstTimeUTC
         AND [Sequence] <= @FirstSequence
  ORDER  BY [TimeUtc] DESC
            ,[Sequence] DESC
  FOR XML AUTO