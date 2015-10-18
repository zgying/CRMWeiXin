/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/


SET IDENTITY_INSERT [dbo].[ContentFiles] ON;

BEGIN TRANSACTION;
INSERT INTO [dbo].[ContentFiles]([FileID], [CreateDate], [PortalID], [Name], [ParentID], [MimeType], [IsDirectory], [Size], [FileContent])
SELECT 1, getdate(), 1, N'ROOT', NULL, NULL, 1, NULL, NULL
COMMIT;
RAISERROR (N'[dbo].[ContentFiles]: Insert Batch: 1.....Done!', 10, 1) WITH NOWAIT;
GO

SET IDENTITY_INSERT [dbo].[ContentFiles] OFF;