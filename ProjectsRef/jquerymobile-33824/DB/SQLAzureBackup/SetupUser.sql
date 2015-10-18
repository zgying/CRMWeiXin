-- =============================================
-- Script Template
-- =============================================

--STEP 1 Run in master database Azure does not allow USE MASTER
CREATE LOGIN jQueryMobile WITH password='Password2012';
GO

CREATE USER jQueryMobile FROM LOGIN jQueryMobile;
GO

--STEP 2 RESTORE database using AzureSQLBackup


--STEP 3 If database exsists and want to restore user otherwise skip

CREATE USER jQueryMobile FROM LOGIN jQueryMobile;
GO

EXEC sp_addrolemember 'db_owner', 'jQueryMobile';  -- like dbcreator
GO
