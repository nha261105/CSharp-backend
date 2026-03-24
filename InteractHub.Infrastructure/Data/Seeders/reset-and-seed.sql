-- NOTE: This file stores commands used to reset + seed sample data.
-- Run all commands from project root: interacthub-backend/

-- ============================================================
-- STEP 1) RESET SAMPLE DATA
-- Use -i (not -it) so command works in non-interactive terminal.
-- ============================================================
docker exec -i interacthub-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "interacthub@123" -d InteractHubDB -No -C -Q "
SET QUOTED_IDENTIFIER ON;
DELETE FROM StoryReactions;
DELETE FROM StoryViews;
DELETE FROM Stories;
DELETE FROM Notifications;
DELETE FROM PostReports;
DELETE FROM CommentLikes;
DELETE FROM CommentMentions;
DELETE FROM Comments;
DELETE FROM PostLikes;
DELETE FROM PostMentions;
DELETE FROM PostHashtags;
DELETE FROM PostMedia;
DELETE FROM PostShares;
DELETE FROM Posts;
DELETE FROM Friendships;
DELETE FROM UserProfiles;
DELETE FROM Hashtags;
DELETE FROM MusicTracks;
DELETE FROM AspNetUserRoles WHERE UserId IN (2,3,4,5,6);
DELETE FROM AspNetUsers WHERE Id IN (2,3,4,5,6);
DBCC CHECKIDENT ('Hashtags', RESEED, 0);
DBCC CHECKIDENT ('MusicTracks', RESEED, 0);
DBCC CHECKIDENT ('Posts', RESEED, 0);
DBCC CHECKIDENT ('PostMedia', RESEED, 0);
DBCC CHECKIDENT ('PostLikes', RESEED, 0);
DBCC CHECKIDENT ('PostShares', RESEED, 0);
DBCC CHECKIDENT ('PostHashtags', RESEED, 0);
DBCC CHECKIDENT ('Comments', RESEED, 0);
DBCC CHECKIDENT ('CommentLikes', RESEED, 0);
DBCC CHECKIDENT ('Friendships', RESEED, 0);
DBCC CHECKIDENT ('Stories', RESEED, 0);
DBCC CHECKIDENT ('StoryViews', RESEED, 0);
DBCC CHECKIDENT ('StoryReactions', RESEED, 0);
DBCC CHECKIDENT ('Notifications', RESEED, 0);
DBCC CHECKIDENT ('UserProfiles', RESEED, 0);
PRINT 'Reset done!';
"

-- ============================================================
-- STEP 2) BUILD TEMP SEED SCRIPT (fixed from seed-data-example.sql)
-- seed-data-example.sql currently has duplicated/corrupted blocks.
-- ============================================================
awk 'NR>=225 && NR<=666 {print} END{print "GO"}' InteractHub.Infrastructure/Data/Seeders/seed-data-example.sql > /tmp/interacthub-seed-clean.sql

cat >/tmp/interacthub-seed-prefix.sql <<'SQL'
USE InteractHubDB;
GO
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

SET IDENTITY_INSERT AspNetRoles ON;
IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Id = 1)
INSERT INTO AspNetRoles (Id, description, delflg, reg_datetime, Name, NormalizedName, ConcurrencyStamp)
VALUES (1, N'System Administrator', 0, GETDATE(), 'Admin', 'ADMIN', NEWID());
IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Id = 2)
INSERT INTO AspNetRoles (Id, description, delflg, reg_datetime, Name, NormalizedName, ConcurrencyStamp)
VALUES (2, N'Content Moderator', 0, GETDATE(), 'Moderator', 'MODERATOR', NEWID());
IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Id = 3)
INSERT INTO AspNetRoles (Id, description, delflg, reg_datetime, Name, NormalizedName, ConcurrencyStamp)
VALUES (3, N'General User', 0, GETDATE(), 'User', 'USER', NEWID());
SET IDENTITY_INSERT AspNetRoles OFF;
GO
SQL

cat /tmp/interacthub-seed-prefix.sql /tmp/interacthub-seed-clean.sql > /tmp/interacthub-seed-fixed.sql

awk 'BEGIN{ins=0;done=0} {
	if(index($0,"INSERT INTO AspNetUsers (") && done==0){print "SET IDENTITY_INSERT AspNetUsers ON;"; ins=1}
	if(ins==1 && done==0 && $0=="GO"){print "SET IDENTITY_INSERT AspNetUsers OFF;"; done=1}
	print
}' /tmp/interacthub-seed-fixed.sql > /tmp/interacthub-seed-fixed2.sql

-- ============================================================
-- STEP 3) RUN SEED
-- ============================================================
docker exec -i interacthub-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "interacthub@123" -d InteractHubDB -No -C < /tmp/interacthub-seed-fixed2.sql

-- ============================================================
-- STEP 4) QUICK VERIFY COUNTS
-- ============================================================
docker exec -i interacthub-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "interacthub@123" -d InteractHubDB -No -C -Q "
SET QUOTED_IDENTIFIER ON;
SELECT 'AspNetRoles' AS [Table], COUNT(*) AS Cnt FROM AspNetRoles
UNION ALL SELECT 'AspNetUsers', COUNT(*) FROM AspNetUsers WHERE Id IN (2,3,4,5,6)
UNION ALL SELECT 'AspNetUserRoles', COUNT(*) FROM AspNetUserRoles WHERE UserId IN (2,3,4,5,6)
UNION ALL SELECT 'Posts', COUNT(*) FROM Posts
UNION ALL SELECT 'Comments', COUNT(*) FROM Comments
UNION ALL SELECT 'Stories', COUNT(*) FROM Stories
UNION ALL SELECT 'Notifications', COUNT(*) FROM Notifications;
"