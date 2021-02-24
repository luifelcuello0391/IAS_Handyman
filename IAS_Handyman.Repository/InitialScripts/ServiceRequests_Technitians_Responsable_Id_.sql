/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
IF(OBJECT_ID('FK_dbo.ServiceRequests_dbo.Technicians_Responsable_Id', 'F') IS NOT NULL)
BEGIN

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT

BEGIN TRANSACTION
ALTER TABLE dbo.ServiceRequests
	DROP CONSTRAINT [FK_dbo.ServiceRequests_dbo.Technicians_Responsable_Id]

ALTER TABLE dbo.Technicians SET (LOCK_ESCALATION = TABLE)

COMMIT
BEGIN TRANSACTION

ALTER TABLE dbo.ServiceRequests SET (LOCK_ESCALATION = TABLE)

COMMIT

END