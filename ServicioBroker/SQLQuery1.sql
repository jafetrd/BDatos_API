IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'bd_api')
BEGIN
    SELECT 'Database Name already Exist' AS Message
END
ELSE
BEGIN
    CREATE DATABASE [bd_api]
    SELECT 'New Database is Created'
END