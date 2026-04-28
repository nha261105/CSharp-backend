-- ============================================================
-- MSSQL reset + seed script for InteractHub
-- Idempotent: chạy nhiều lần không lỗi, data luôn đồng nhất
-- ============================================================
-- Cách chạy qua Docker:
--   cat reset-and-seed.sql | docker exec -i interacthub-sqlserver \
--     /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa \
--     -P 'Interacthub@123A' -d InteractHubDb -C -i /dev/stdin
-- ============================================================

BEGIN TRANSACTION;

-- ============================================================
-- BƯỚC 1: TẮT FOREIGN KEY (để DELETE không bị lỗi thứ tự)
-- ============================================================
ALTER TABLE [CommentMentions]  NOCHECK CONSTRAINT ALL;
ALTER TABLE [CommentLikes]     NOCHECK CONSTRAINT ALL;
ALTER TABLE [Comments]         NOCHECK CONSTRAINT ALL;
ALTER TABLE [StoryReactions]   NOCHECK CONSTRAINT ALL;
ALTER TABLE [StoryViews]       NOCHECK CONSTRAINT ALL;
ALTER TABLE [Stories]          NOCHECK CONSTRAINT ALL;
ALTER TABLE [Notifications]    NOCHECK CONSTRAINT ALL;
ALTER TABLE [PostReports]      NOCHECK CONSTRAINT ALL;
ALTER TABLE [PostLikes]        NOCHECK CONSTRAINT ALL;
ALTER TABLE [PostMentions]     NOCHECK CONSTRAINT ALL;
ALTER TABLE [PostHashtags]     NOCHECK CONSTRAINT ALL;
ALTER TABLE [PostMedia]        NOCHECK CONSTRAINT ALL;
ALTER TABLE [PostShares]       NOCHECK CONSTRAINT ALL;
ALTER TABLE [Posts]            NOCHECK CONSTRAINT ALL;
ALTER TABLE [Friendships]      NOCHECK CONSTRAINT ALL;
ALTER TABLE [UserProfiles]     NOCHECK CONSTRAINT ALL;
ALTER TABLE [Hashtags]         NOCHECK CONSTRAINT ALL;
ALTER TABLE [MusicTracks]      NOCHECK CONSTRAINT ALL;
ALTER TABLE [AspNetUserRoles]  NOCHECK CONSTRAINT ALL;
ALTER TABLE [AspNetUserClaims] NOCHECK CONSTRAINT ALL;
ALTER TABLE [AspNetUserLogins] NOCHECK CONSTRAINT ALL;
ALTER TABLE [AspNetUserTokens] NOCHECK CONSTRAINT ALL;
ALTER TABLE [AspNetUsers]      NOCHECK CONSTRAINT ALL;
ALTER TABLE [AspNetRoleClaims] NOCHECK CONSTRAINT ALL;
ALTER TABLE [AspNetRoles]      NOCHECK CONSTRAINT ALL;

-- ============================================================
-- BƯỚC 2: XÓA DATA (child trước, parent sau)
-- ============================================================
DELETE FROM [CommentMentions];
DELETE FROM [CommentLikes];
DELETE FROM [Comments];
DELETE FROM [StoryReactions];
DELETE FROM [StoryViews];
DELETE FROM [Stories];
DELETE FROM [Notifications];
DELETE FROM [PostReports];
DELETE FROM [PostLikes];
DELETE FROM [PostMentions];
DELETE FROM [PostHashtags];
DELETE FROM [PostMedia];
DELETE FROM [PostShares];
DELETE FROM [Posts];
DELETE FROM [Friendships];
DELETE FROM [UserProfiles];
DELETE FROM [Hashtags];
DELETE FROM [MusicTracks];
DELETE FROM [AspNetUserRoles];
DELETE FROM [AspNetUserClaims];
DELETE FROM [AspNetUserLogins];
DELETE FROM [AspNetUserTokens];
DELETE FROM [AspNetUsers];
DELETE FROM [AspNetRoleClaims];
DELETE FROM [AspNetRoles];

-- ============================================================
-- BƯỚC 3: BẬT LẠI FOREIGN KEY
-- ============================================================
ALTER TABLE [CommentMentions]  WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [CommentLikes]     WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [Comments]         WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [StoryReactions]   WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [StoryViews]       WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [Stories]          WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [Notifications]    WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [PostReports]      WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [PostLikes]        WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [PostMentions]     WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [PostHashtags]     WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [PostMedia]        WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [PostShares]       WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [Posts]            WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [Friendships]      WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [UserProfiles]     WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [Hashtags]         WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [MusicTracks]      WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [AspNetUserRoles]  WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [AspNetUserClaims] WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [AspNetUserLogins] WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [AspNetUserTokens] WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [AspNetUsers]      WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [AspNetRoleClaims] WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [AspNetRoles]      WITH CHECK CHECK CONSTRAINT ALL;

-- ============================================================
-- BƯỚC 4: RESET IDENTITY VỀ 0 (trước khi insert)
-- ============================================================
DBCC CHECKIDENT ('[AspNetRoles]',    RESEED, 0);
DBCC CHECKIDENT ('[AspNetUsers]',    RESEED, 0);
DBCC CHECKIDENT ('[UserProfiles]',   RESEED, 0);
DBCC CHECKIDENT ('[Friendships]',    RESEED, 0);
DBCC CHECKIDENT ('[Hashtags]',       RESEED, 0);
DBCC CHECKIDENT ('[MusicTracks]',    RESEED, 0);
DBCC CHECKIDENT ('[Posts]',          RESEED, 0);
DBCC CHECKIDENT ('[PostMedia]',      RESEED, 0);
DBCC CHECKIDENT ('[PostHashtags]',   RESEED, 0);
DBCC CHECKIDENT ('[PostLikes]',      RESEED, 0);
DBCC CHECKIDENT ('[Comments]',       RESEED, 0);
DBCC CHECKIDENT ('[CommentLikes]',   RESEED, 0);
DBCC CHECKIDENT ('[Stories]',        RESEED, 0);
DBCC CHECKIDENT ('[StoryViews]',     RESEED, 0);
DBCC CHECKIDENT ('[StoryReactions]', RESEED, 0);
DBCC CHECKIDENT ('[Notifications]',  RESEED, 0);
DBCC CHECKIDENT ('[PostReports]',    RESEED, 0);

-- ============================================================
-- BƯỚC 5: INSERT SEED DATA
-- ============================================================

-- ------------------------------------------------------------
-- Roles (3 roles)
-- ------------------------------------------------------------
SET IDENTITY_INSERT [AspNetRoles] ON;
INSERT INTO [AspNetRoles] ([Id], [description], [delflg], [reg_datetime], [Name], [NormalizedName], [ConcurrencyStamp])
VALUES
(1, 'System Administrator', 0, GETDATE(), 'Admin',     'ADMIN',     'seed-role-admin'),
(2, 'Content Moderator',    0, GETDATE(), 'Moderator', 'MODERATOR', 'seed-role-moderator'),
(3, 'General User',         0, GETDATE(), 'User',      'USER',      'seed-role-user');
SET IDENTITY_INSERT [AspNetRoles] OFF;

-- ------------------------------------------------------------
-- Users (9 users: Id 2-10)
-- Ghi chú: Id=1 bỏ trống theo thiết kế gốc
-- Id 2-6  : seed users (alice, bob, carol, david, eva)
-- Id 7    : manh      (test user - private account)
-- Id 8    : admin     (system admin)
-- Id 9    : ryan      (real user)
-- Id 10   : string    (test/swagger user)
-- ------------------------------------------------------------
SET IDENTITY_INSERT [AspNetUsers] ON;
INSERT INTO [AspNetUsers] (
    [Id], [UserName], [NormalizedUserName],
    [Email], [NormalizedEmail], [EmailConfirmed],
    [PasswordHash], [SecurityStamp], [ConcurrencyStamp],
    [PhoneNumber], [PhoneNumberConfirmed],
    [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount],
    [full_name], [gender], [date_of_birth],
    [avatar_url], [cover_photo_url], [bio],
    [website_url], [location],
    [is_active], [is_private_account],
    [delflg], [reg_datetime]
)
VALUES
(
    2, 'alice_nguyen', 'ALICE_NGUYEN',
    'alice@gmail.com', 'ALICE@GMAIL.COM', 1,
    'AQAAAAIAAYagAAAAELk4p5pJz8eMWopqM7TA+Wr2aBpHQ3G6iHEWvU2cBFEFcqJkbKqtJlW8Rh5Ue9BWGA==',
    'seed-security-2', 'seed-concurrency-2',
    '0901234567', 0,
    0, 1, 0,
    N'Nguyễn Thị Alice', 'Female', CAST('1999-05-15' AS DATE),
    'https://storage.interacthub.com/avatars/alice.jpg',
    'https://storage.interacthub.com/covers/alice-cover.jpg',
    N'Yêu thích du lịch và nhiếp ảnh',
    'https://alice.blog', N'Hồ Chí Minh',
    1, 0,
    0, GETDATE()
),
(
    3, 'bob_tran', 'BOB_TRAN',
    'bob@gmail.com', 'BOB@GMAIL.COM', 1,
    'AQAAAAIAAYagAAAAELk4p5pJz8eMWopqM7TA+Wr2aBpHQ3G6iHEWvU2cBFEFcqJkbKqtJlW8Rh5Ue9BWGA==',
    'seed-security-3', 'seed-concurrency-3',
    '0912345678', 0,
    0, 1, 0,
    N'Trần Văn Bob', 'Male', CAST('2000-08-20' AS DATE),
    'https://storage.interacthub.com/avatars/bob.jpg',
    'https://storage.interacthub.com/covers/bob-cover.jpg',
    N'Lập trình viên | Coffee addict',
    NULL, N'Hà Nội',
    1, 0,
    0, GETDATE()
),
(
    4, 'carol_le', 'CAROL_LE',
    'carol@gmail.com', 'CAROL@GMAIL.COM', 1,
    'AQAAAAIAAYagAAAAELk4p5pJz8eMWopqM7TA+Wr2aBpHQ3G6iHEWvU2cBFEFcqJkbKqtJlW8Rh5Ue9BWGA==',
    'seed-security-4', 'seed-concurrency-4',
    '0923456789', 0,
    0, 1, 0,
    N'Lê Thị Carol', 'Female', CAST('2001-03-10' AS DATE),
    'https://storage.interacthub.com/avatars/carol.jpg',
    'https://storage.interacthub.com/covers/carol-cover.jpg',
    N'Foodie | Review ẩm thực Sài Gòn',
    'https://carol.food', N'Hồ Chí Minh',
    1, 0,
    0, GETDATE()
),
(
    5, 'david_pham', 'DAVID_PHAM',
    'david@gmail.com', 'DAVID@GMAIL.COM', 1,
    'AQAAAAIAAYagAAAAELk4p5pJz8eMWopqM7TA+Wr2aBpHQ3G6iHEWvU2cBFEFcqJkbKqtJlW8Rh5Ue9BWGA==',
    'seed-security-5', 'seed-concurrency-5',
    '0934567890', 0,
    0, 1, 0,
    N'Phạm Văn David', 'Male', CAST('1998-12-25' AS DATE),
    'https://storage.interacthub.com/avatars/david.jpg',
    NULL,
    N'Tech enthusiast | Open source contributor',
    'https://github.com/david', N'Đà Nẵng',
    1, 0,
    0, GETDATE()
),
(
    6, 'eva_hoang', 'EVA_HOANG',
    'eva@gmail.com', 'EVA@GMAIL.COM', 1,
    'AQAAAAIAAYagAAAAELk4p5pJz8eMWopqM7TA+Wr2aBpHQ3G6iHEWvU2cBFEFcqJkbKqtJlW8Rh5Ue9BWGA==',
    'seed-security-6', 'seed-concurrency-6',
    '0945678901', 0,
    0, 1, 0,
    N'Hoàng Thị Eva', 'Female', CAST('2002-07-04' AS DATE),
    'https://storage.interacthub.com/avatars/eva.jpg',
    'https://storage.interacthub.com/covers/eva-cover.jpg',
    N'Nghệ sĩ tự do | Vẽ tranh & thiết kế',
    NULL, N'Hồ Chí Minh',
    1, 0,
    0, GETDATE()
),
(
    -- manh: test user, private account
    7, 'manh', 'MANH',
    'manh@example.com', 'MANH@EXAMPLE.COM', 0,
    'AQAAAAIAAYagAAAAEGe3FC3ImDVN8cFU64FCnc6W11z5CjRjwhk8PBMagj492BoTx+7CNuTwgncVOEyuEw==',
    'QHLVSNGKJ6PVTKM4OQ4XF2KEYTS3UV6H', '53d3bd05-337b-40b4-b550-e2a4ede54978',
    '0912838231', 0,
    0, 1, 0,
    N'Con bò biết bay', 'nam', CAST('2026-04-27' AS DATE),
    'urlavt', 'url cover',
    N'string',
    'https://hungmanh-mobile-developer.vercel.app', N'string',
    1, 1,
    0, GETDATE()
),
(
    -- admin: system administrator
    8, 'admin', 'ADMIN',
    'admin@test.com', 'ADMIN@TEST.COM', 1,
    'AQAAAAIAAYagAAAAEPPId2WNR2nIqqx6U5n8RMK4Yi33NpnJGFMQAPvZ+DwQyKfHU9mHp+Fdyao5NlNfEQ==',
    'FLZMRJEZAKLJWKLTQ4JA6P7WFE4N4QC7', '6b3dd65f-5328-4c32-b5fb-c70c6245d37e',
    NULL, 0,
    0, 1, 0,
    N'Admin System', NULL, NULL,
    NULL, NULL,
    NULL,
    NULL, NULL,
    1, 0,
    0, GETDATE()
),
(
    -- ryan: real user
    9, 'ryan', 'RYAN',
    'ryan@example.com', 'RYAN@EXAMPLE.COM', 0,
    'AQAAAAIAAYagAAAAEGEw0t3OTvZN/e4l/YHZ35xe/vdntDC77ImsAr4ykZ2lJxLLyrXc+wXEtdLpE1AGWw==',
    '5PQ6MTH77KMD7SS753HY2N6QG2TLA3GX', '99d36dcc-250f-4df7-a136-b4ad0555a398',
    '0985922611', 0,
    0, 1, 0,
    N'Ryan Nguyen', NULL, NULL,
    'https://interacthubstore.blob.core.windows.net/uploads/avatars/9/20260427074452_b286255e610c4dbf916334e9e047aaed.png',
    NULL,
    NULL,
    NULL, NULL,
    1, 0,
    0, GETDATE()
),
(
    -- string: swagger/test user
    10, 'string', 'STRING',
    'user@example.com', 'USER@EXAMPLE.COM', 0,
    'AQAAAAIAAYagAAAAELrCpsQx6qHJ7dY6xjrrh4PbaPGeKnhypqQF3GkpUF9I5yhQSv4e1kkF3mzW4lrUVw==',
    '3F4DEMUGBHVHNFTHE6HR3FSSR34D2CX6', 'b479463b-dbf4-4a82-83cb-86b9d639e56d',
    '0862915493', 0,
    0, 1, 0,
    N'string', NULL, NULL,
    NULL, NULL,
    NULL,
    NULL, NULL,
    1, 0,
    0, GETDATE()
);
SET IDENTITY_INSERT [AspNetUsers] OFF;

-- ------------------------------------------------------------
-- User Roles (9 rows: admin=Role1, còn lại=Role3)
-- ------------------------------------------------------------
INSERT INTO [AspNetUserRoles] ([UserId], [RoleId])
VALUES
(8,  1),  -- admin  → Admin
(2,  3),  -- alice  → User
(3,  3),  -- bob    → User
(4,  3),  -- carol  → User
(5,  3),  -- david  → User
(6,  3),  -- eva    → User
(7,  3),  -- manh   → User
(9,  3),  -- ryan   → User
(10, 3);  -- string → User

-- ------------------------------------------------------------
-- Hashtags (5 rows)
-- ------------------------------------------------------------
SET IDENTITY_INSERT [Hashtags] ON;
INSERT INTO [Hashtags] ([hashtag_id], [tag_name], [post_count], [trending_score], [is_trending], [delflg], [reg_datetime])
VALUES
(1, 'travel',  15, 95.50, 1, 0, GETDATE()),
(2, 'food',    12, 88.00, 1, 0, GETDATE()),
(3, 'coding',  10, 82.50, 1, 0, GETDATE()),
(4, 'art',      8, 75.00, 0, 0, GETDATE()),
(5, 'vietnam', 11, 85.00, 1, 0, GETDATE());
SET IDENTITY_INSERT [Hashtags] OFF;

-- ------------------------------------------------------------
-- Music Tracks (3 rows)
-- ------------------------------------------------------------
SET IDENTITY_INSERT [MusicTracks] ON;
INSERT INTO [MusicTracks] ([music_id], [title], [artist], [audio_url], [thumbnail_url], [duration_sec], [is_licensed], [source], [delflg], [reg_datetime])
VALUES
(1, 'Chill Vibes',    'Lo-Fi Studio',  'https://storage.interacthub.com/music/chill-vibes.mp3',    'https://storage.interacthub.com/music/thumbs/chill-vibes.jpg',    180, 1, 'Internal', 0, GETDATE()),
(2, 'Morning Coffee', 'Jazz Quartet',  'https://storage.interacthub.com/music/morning-coffee.mp3', 'https://storage.interacthub.com/music/thumbs/morning-coffee.jpg', 195, 1, 'Internal', 0, GETDATE()),
(3, 'Epic Cinematic', 'Orchestra Pro', 'https://storage.interacthub.com/music/epic-cinematic.mp3', 'https://storage.interacthub.com/music/thumbs/epic-cinematic.jpg',  300, 1, 'Internal', 0, GETDATE());
SET IDENTITY_INSERT [MusicTracks] OFF;

-- ------------------------------------------------------------
-- User Profiles (6 rows: user 2-7)
-- ------------------------------------------------------------
SET IDENTITY_INSERT [UserProfiles] ON;
INSERT INTO [UserProfiles] (
    [profile_id], [user_id],
    [relationship_status], [work_place], [position], [education],
    [hometown], [current_city],
    [instagram_link], [twitter_link],
    [follower_count], [following_count], [post_count], [friend_count],
    [privacy_posts], [privacy_friends], [privacy_photos],
    [notification_email_flg], [notification_push_flg],
    [delflg], [reg_datetime]
)
VALUES
(1, 2, N'Độc thân', 'FPT Software',     'Designer',          N'Đại học Sài Gòn', N'Hà Nội',  N'Hồ Chí Minh', 'alice_ng',      NULL,        120, 85,  8,  45, 'Public', 'Public',  'Public',  1, 1, 0, GETDATE()),
(2, 3, N'Độc thân', 'VNG Corporation',  'Backend Developer', 'HCMUT',            N'Hà Nội',  N'Hà Nội',      'bob_tran',      'bobtran',   98,  72,  5,  38, 'Public', 'Friends', 'Public',  1, 1, 0, GETDATE()),
(3, 4, N'Hẹn hò',   'Grab Vietnam',     'Content Creator',   N'Đại học Sài Gòn', N'Cần Thơ', N'Hồ Chí Minh', 'carol_le',      NULL,        215, 130, 12, 67, 'Public', 'Public',  'Public',  1, 1, 0, GETDATE()),
(4, 5, N'Độc thân', 'Freelance',        'Full-stack Dev',    N'ĐH Đà Nẵng',      N'Đà Nẵng', N'Đà Nẵng',     'david_ph',      'davidpham', 76,  54,  6,  29, 'Public', 'Public',  'Friends', 1, 0, 0, GETDATE()),
(5, 6, N'Độc thân', 'Studio Freelance', 'Illustrator',       N'ĐH Mỹ thuật HCM', N'Huế',     N'Hồ Chí Minh', 'eva_hoang',     NULL,        340, 210, 15, 89, 'Public', 'Public',  'Public',  1, 1, 0, GETDATE()),
(6, 7, N'Độc thân', 'IT Company',       'Backend Engineer',  N'Đại học',         N'Hà Nội',  N'Hồ Chí Minh', 'manh_backend',  'manh_dev',  65,  80,  1,  15, 'Public', 'Public',  'Public',  1, 1, 0, GETDATE());
SET IDENTITY_INSERT [UserProfiles] OFF;

-- ------------------------------------------------------------
-- Friendships (13 rows theo JSON — bao gồm test data của user 7, 9)
-- ------------------------------------------------------------
SET IDENTITY_INSERT [Friendships] ON;
INSERT INTO [Friendships] ([friendship_id], [requester_id], [addressee_id], [status], [action_user_id], [is_blocked], [delflg], [reg_datetime])
VALUES
(1,  2, 3, 'Accepted', 3, 0, 0, DATEADD(day, -11, GETDATE())),
(2,  2, 4, 'Accepted', 4, 0, 0, DATEADD(day, -10, GETDATE())),
(3,  3, 5, 'Accepted', 5, 0, 0, DATEADD(day, -9,  GETDATE())),
(4,  4, 6, 'Accepted', 6, 0, 0, DATEADD(day, -8,  GETDATE())),
(5,  2, 5, 'Pending',  2, 0, 0, DATEADD(day, -6,  GETDATE())),
(6,  7, 3, 'Accepted', 3, 0, 0, DATEADD(day, -7,  GETDATE())),
-- delflg=1: soft-deleted friendship (user 5 & 7)
(7,  5, 7, 'None',     7, 0, 1, DATEADD(day, -6,  GETDATE())),
(8,  7, 4, 'Pending',  7, 0, 0, DATEADD(day, -1,  GETDATE())),
-- user 9 (ryan) gửi nhiều lời mời
(9,  9, 6, 'Pending',  9, 0, 0, DATEADD(day, -1,  GETDATE())),
(10, 9, 5, 'Pending',  9, 0, 0, DATEADD(day, -1,  GETDATE())),
(11, 9, 4, 'Pending',  9, 0, 0, DATEADD(day, -1,  GETDATE())),
(12, 9, 3, 'Pending',  9, 0, 0, DATEADD(day, -1,  GETDATE())),
(13, 9, 2, 'Pending',  9, 0, 0, DATEADD(day, -1,  GETDATE()));
-- delflg=1: user 9 & 7 bị block rồi xóa
SET IDENTITY_INSERT [Friendships] OFF;

-- ------------------------------------------------------------
-- Posts (6 rows — post_id 6 của user 7, delflg=1)
-- ------------------------------------------------------------
SET IDENTITY_INSERT [Posts] ON;
INSERT INTO [Posts] (
    [post_id], [user_id], [content], [post_type], [visibility],
    [location_name], [feeling],
    [like_count], [comment_count], [share_count],
    [is_edited], [is_pinned], [is_reported], [report_count], [allow_comment],
    [delflg], [reg_datetime]
)
VALUES
(1, 2, N'Vừa đến Đà Lạt, thời tiết tuyệt vời quá! #travel #vietnam',
    'Text',  'Public', N'Đà Lạt',           N'hạnh phúc', 24, 5,  3,  0, 0, 0, 0, 1, 0, DATEADD(day, -11, GETDATE())),
(2, 3, N'Vừa deploy xong feature mới lên production mà không có bug nào 🎉 #coding',
    'Text',  'Public', NULL,                 N'tự hào',    18, 7,  2,  0, 0, 0, 0, 1, 0, DATEADD(day, -10, GETDATE())),
(3, 4, N'Review bánh mì Hòa Mã - địa chỉ quá quen thuộc với dân Sài Gòn 🥖 #food',
    'Image', 'Public', N'Bánh mì Hòa Mã',   N'thích thú', 45, 12, 8,  0, 0, 0, 0, 1, 0, DATEADD(day, -9,  GETDATE())),
(4, 5, N'Mình vừa contribute vào một open-source project khá thú vị. Ai quan tâm đến Rust không? #coding #vietnam',
    'Text',  'Public', NULL,                 NULL,         31, 9,  5,  0, 0, 0, 0, 1, 0, DATEADD(day, -8,  GETDATE())),
(5, 6, N'Xong bức tranh mới sau 2 tuần! Cảm ơn mọi người đã ủng hộ mình 🎨 #art',
    'Image', 'Public', NULL,                 N'tự hào',    89, 23, 14, 0, 1, 1, 1, 1, 0, DATEADD(day, -7,  GETDATE())),
-- post của user 7 (manh) - đã bị soft delete
(6, 7, N'Con bò biết bay',
    'Text',  'Public', NULL,                 N'tập trung', 12, 2,  0,  1, 0, 0, 0, 1, 1, DATEADD(day, -6,  GETDATE()));
SET IDENTITY_INSERT [Posts] OFF;

-- ------------------------------------------------------------
-- Post Media (3 rows)
-- ------------------------------------------------------------
SET IDENTITY_INSERT [PostMedia] ON;
INSERT INTO [PostMedia] ([media_id], [post_id], [media_url], [media_type], [file_name], [file_size_kb], [width_px], [height_px], [sort_order], [processing_status], [delflg], [reg_datetime])
VALUES
(1, 3, 'https://storage.interacthub.com/posts/3/banhmi-1.jpg', 'Image', 'banhmi-1.jpg', 245, 1080, 1080, 0, 'Ready', 0, DATEADD(day, -9, GETDATE())),
(2, 3, 'https://storage.interacthub.com/posts/3/banhmi-2.jpg', 'Image', 'banhmi-2.jpg', 312, 1080, 720,  1, 'Ready', 0, DATEADD(day, -9, GETDATE())),
(3, 5, 'https://storage.interacthub.com/posts/5/painting.jpg', 'Image', 'painting.jpg', 892, 2048, 1536, 0, 'Ready', 0, DATEADD(day, -7, GETDATE()));
SET IDENTITY_INSERT [PostMedia] OFF;

-- ------------------------------------------------------------
-- Post Hashtags (7 rows — post 6 có hashtag coding)
-- ------------------------------------------------------------
SET IDENTITY_INSERT [PostHashtags] ON;
INSERT INTO [PostHashtags] ([post_hashtag_id], [post_id], [hashtag_id], [delflg], [reg_datetime])
VALUES
(1, 1, 1, 0, DATEADD(day, -11, GETDATE())),  -- post 1 → #travel
(2, 1, 5, 0, DATEADD(day, -11, GETDATE())),  -- post 1 → #vietnam
(3, 2, 3, 0, DATEADD(day, -10, GETDATE())),  -- post 2 → #coding
(4, 3, 2, 0, DATEADD(day, -9,  GETDATE())),  -- post 3 → #food
(5, 4, 3, 0, DATEADD(day, -8,  GETDATE())),  -- post 4 → #coding
(6, 5, 4, 0, DATEADD(day, -7,  GETDATE())),  -- post 5 → #art
(7, 6, 3, 0, DATEADD(day, -6,  GETDATE()));  -- post 6 → #coding
SET IDENTITY_INSERT [PostHashtags] OFF;

-- ------------------------------------------------------------
-- Post Likes (7 rows)
-- ------------------------------------------------------------
SET IDENTITY_INSERT [PostLikes] ON;
INSERT INTO [PostLikes] ([like_id], [post_id], [user_id], [reaction_type], [delflg], [reg_datetime])
VALUES
(1, 1, 3, 'Love', 0, DATEADD(day, -11, GETDATE())),
(2, 1, 4, 'Like', 0, DATEADD(day, -11, GETDATE())),
(3, 2, 2, 'Haha', 0, DATEADD(day, -10, GETDATE())),
(4, 3, 2, 'Love', 0, DATEADD(day, -9,  GETDATE())),
(5, 5, 2, 'Love', 0, DATEADD(day, -7,  GETDATE())),
(6, 6, 3, 'Like', 0, DATEADD(day, -6,  GETDATE())),
(7, 6, 5, 'Wow',  0, DATEADD(day, -6,  GETDATE()));
SET IDENTITY_INSERT [PostLikes] OFF;

-- ------------------------------------------------------------
-- Comments (7 rows — comment 7 trên post 6 của bob)
-- ------------------------------------------------------------
SET IDENTITY_INSERT [Comments] ON;
INSERT INTO [Comments] ([comment_id], [post_id], [user_id], [parent_comment_id], [content], [like_count], [reply_count], [is_edited], [is_reported], [delflg], [reg_datetime])
VALUES
(1, 1, 3, NULL, N'Đà Lạt đẹp lắm! Mình cũng muốn đi quá 😍',                                            5, 1, 0, 0, 0, DATEADD(day, -10, GETDATE())),
(2, 1, 4, NULL, N'Thời điểm này đi Đà Lạt là chuẩn nhất luôn!',                                          3, 0, 0, 0, 0, DATEADD(day, -9,  GETDATE())),
(3, 2, 2, NULL, N'Giỏi vậy! Mình thì deploy xong là có bug ngay 😅',                                      4, 1, 0, 0, 0, DATEADD(day, -9,  GETDATE())),
(4, 3, 2, NULL, N'Bánh mì Hòa Mã ngon thật!',                                                             6, 0, 0, 0, 0, DATEADD(day, -8,  GETDATE())),
(5, 5, 2, NULL, N'Tranh đẹp quá! Bạn vẽ bao lâu vậy?',                                                   8, 1, 0, 0, 0, DATEADD(day, -6,  GETDATE())),
(6, 5, 6, 5,   N'Mình vẽ khoảng 2 tuần, tranh 60x80cm bạn ơi!',                                          3, 0, 0, 0, 0, DATEADD(day, -5,  GETDATE())),
(7, 6, 3, NULL, N'Đỉnh quá bro! Xong TOEIC thì qua chiến tiếng Đức chung nhé, đang cần người luyện nghe chung.', 2, 0, 0, 0, 0, DATEADD(day, -6, GETDATE()));
SET IDENTITY_INSERT [Comments] OFF;

-- ------------------------------------------------------------
-- Comment Likes (4 rows)
-- ------------------------------------------------------------
SET IDENTITY_INSERT [CommentLikes] ON;
INSERT INTO [CommentLikes] ([like_id], [comment_id], [user_id], [reaction_type], [delflg], [reg_datetime])
VALUES
(1, 1, 2, 'Like', 0, DATEADD(day, -10, GETDATE())),
(2, 1, 5, 'Love', 0, DATEADD(day, -10, GETDATE())),
(3, 3, 3, 'Haha', 0, DATEADD(day, -9,  GETDATE())),
(4, 5, 6, 'Like', 0, DATEADD(day, -6,  GETDATE()));
SET IDENTITY_INSERT [CommentLikes] OFF;

-- ------------------------------------------------------------
-- Stories (5 rows — story_id 4 của user 7, story_id 7 delflg=1)
-- Lưu ý: story_id nhảy từ 4 → 7 (5, 6 không tồn tại trong JSON)
-- ------------------------------------------------------------
SET IDENTITY_INSERT [Stories] ON;
INSERT INTO [Stories] (
    [story_id], [user_id],
    [media_url], [media_type],
    [caption], [bg_color], [duration_sec], [visibility],
    [view_count], [reaction_count],
    [expire_datetime], [is_expired], [is_highlighted], [highlight_name],
    [delflg], [reg_datetime]
)
VALUES
(1, 2,
    'https://storage.interacthub.com/stories/alice-dalat.jpg', 'Image',
    N'Đà Lạt buổi sáng 🌿', '#2D5016', 5, 'Friends',
    45, 8,
    DATEADD(hour, 24, DATEADD(day, -5, GETDATE())), 0, 0, NULL,
    0, DATEADD(day, -5, GETDATE())),
(2, 3,
    'https://storage.interacthub.com/stories/bob-code.jpg', 'Image',
    N'Coding time ☕', '#1a1a2e', 5, 'Public',
    33, 5,
    DATEADD(hour, 24, DATEADD(day, -5, GETDATE())), 0, 0, NULL,
    0, DATEADD(day, -5, GETDATE())),
(3, 6,
    'https://storage.interacthub.com/stories/eva-art.jpg', 'Image',
    N'Work in progress 🎨', '#fff5e6', 5, 'Public',
    120, 25,
    DATEADD(hour, 24, DATEADD(day, -5, GETDATE())), 0, 1, 'Artwork',
    0, DATEADD(day, -5, GETDATE())),
-- story của user 7 (manh)
(4, 7,
    'string', 'Image',
    N'lskdfjsldkfj', 'string', 10, 'Public',
    18, 3,
    DATEADD(hour, 24, DATEADD(day, -5, GETDATE())), 0, 1, 'Con bo biet bay',
    0, DATEADD(day, -5, GETDATE())),
-- story bị soft delete (delflg=1)
(7, 7,
    'string', 'string',
    N'string', 'string', 10, 'Public',
    0, 0,
    DATEADD(hour, 24, GETDATE()), 0, 0, NULL,
    1, DATEADD(day, -1, GETDATE()));
SET IDENTITY_INSERT [Stories] OFF;

-- ------------------------------------------------------------
-- Story Views (6 rows — view 5 & 6 là từ user 3 & 7)
-- ------------------------------------------------------------
SET IDENTITY_INSERT [StoryViews] ON;
INSERT INTO [StoryViews] ([view_id], [story_id], [viewer_id], [view_duration], [delflg], [reg_datetime])
VALUES
(1, 1, 3, 5,  0, DATEADD(day, -5, GETDATE())),
(2, 1, 4, 5,  0, DATEADD(day, -5, GETDATE())),
(3, 2, 2, 5,  0, DATEADD(day, -5, GETDATE())),
(4, 3, 2, 12, 0, DATEADD(day, -5, GETDATE())),
(5, 4, 3, 5,  0, DATEADD(day, -5, GETDATE())),
(6, 2, 7, 10, 0, DATEADD(day, -1, GETDATE()));
SET IDENTITY_INSERT [StoryViews] OFF;

-- ------------------------------------------------------------
-- Story Reactions (6 rows — reaction 5 & 6 là từ user 3 & 7)
-- reaction 6 (user 7): delflg=1 (đã xóa)
-- ------------------------------------------------------------
SET IDENTITY_INSERT [StoryReactions] ON;
INSERT INTO [StoryReactions] ([reaction_id], [story_id], [user_id], [reaction_type], [delflg], [reg_datetime])
VALUES
(1, 1, 3, 'Love',   0, DATEADD(day, -5,  GETDATE())),
(2, 2, 2, 'Like',   0, DATEADD(day, -5,  GETDATE())),
(3, 3, 2, 'Love',   0, DATEADD(day, -5,  GETDATE())),
(4, 3, 5, 'Wow',    0, DATEADD(day, -5,  GETDATE())),
(5, 4, 3, 'Love',   0, DATEADD(day, -5,  GETDATE())),
(6, 2, 7, 'string', 1, DATEADD(day, -1,  GETDATE()));  -- soft-deleted
SET IDENTITY_INSERT [StoryReactions] OFF;

-- ------------------------------------------------------------
-- Notifications (7 rows — thêm 2 notification cho user 7)
-- ------------------------------------------------------------
SET IDENTITY_INSERT [Notifications] ON;
INSERT INTO [Notifications] (
    [notification_id], [recipient_id], [sender_id],
    [notification_type], [reference_id], [reference_type],
    [message], [is_read], [redirect_url],
    [delflg], [reg_datetime]
)
VALUES
(1, 2, 3, 'post_like',      1, 'Post',       N'Bob đã thích bài viết của bạn',               0, '/posts/1',   0, DATEADD(day, -5, GETDATE())),
(2, 2, 4, 'post_comment',   1, 'Post',       N'Carol đã bình luận về bài viết của bạn',       0, '/posts/1',   0, DATEADD(day, -5, GETDATE())),
(3, 3, 2, 'friend_request', 1, 'Friendship', N'Alice đã gửi lời mời kết bạn',                1, '/friends',   0, DATEADD(day, -5, GETDATE())),
(4, 6, 2, 'story_view',     3, 'Story',      N'Alice đã xem story của bạn',                  0, '/stories/3', 0, DATEADD(day, -5, GETDATE())),
(5, 6, 5, 'post_like',      5, 'Post',       N'David đã thích bài viết của bạn',             0, '/posts/5',   0, DATEADD(day, -5, GETDATE())),
-- notification cho user 7 (manh)
(6, 7, 3, 'post_comment',   6, 'Post',       N'Bob đã bình luận về bài viết của bạn',        1, '/posts/6',   0, DATEADD(day, -6, GETDATE())),
(7, 7, 5, 'friend_accept',  7, 'Friendship', N'David đã chấp nhận lời mời kết bạn',          1, '/friends',   0, DATEADD(day, -6, GETDATE()));
SET IDENTITY_INSERT [Notifications] OFF;

-- ------------------------------------------------------------
-- Post Reports (1 row — user 7 report post 5 của eva)
-- ------------------------------------------------------------
SET IDENTITY_INSERT [PostReports] ON;
INSERT INTO [PostReports] ([report_id], [post_id], [reporter_id], [reason], [status], [description], [delflg], [reg_datetime])
VALUES
(1, 5, 7, 'Spam', 'Pending', N'Cuộc sống mà', 0, DATEADD(day, -1, GETDATE()));
SET IDENTITY_INSERT [PostReports] OFF;

-- ------------------------------------------------------------
-- SEED DATA 
-- ------------------------------------------------------------

-- ------------------------------------------------------------
-- Friendships (Thêm kết bạn giữa admin, ryan và manh)
-- ------------------------------------------------------------
SET IDENTITY_INSERT [Friendships] ON;
INSERT INTO [Friendships] ([friendship_id], [requester_id], [addressee_id], [status], [action_user_id], [is_blocked], [delflg], [reg_datetime])
VALUES
(15, 8, 7, 'Accepted', 7, 0, 0, DATEADD(day, -4, GETDATE())), -- admin và manh
(16, 8, 9, 'Accepted', 9, 0, 0, DATEADD(day, -4, GETDATE())), -- admin và ryan
(17, 9, 7, 'Accepted', 7, 0, 0, DATEADD(day, -2, GETDATE())); -- ryan và manh (đã gỡ block và accept)
SET IDENTITY_INSERT [Friendships] OFF;

-- ------------------------------------------------------------
-- Posts (Thêm bài viết mới)
-- ------------------------------------------------------------
SET IDENTITY_INSERT [Posts] ON;
INSERT INTO [Posts] (
    [post_id], [user_id], [content], [post_type], [visibility],
    [location_name], [feeling],
    [like_count], [comment_count], [share_count],
    [is_edited], [is_pinned], [is_reported], [report_count], [allow_comment],
    [delflg], [reg_datetime]
)
VALUES
(7, 7, N'Vừa deploy thành công con backend lên Azure App Service dùng free tier. Data test seed trực tiếp vào Azure SQL Database chạy mượt phết! ☁️ #coding',
    'Text',  'Public', NULL,                 N'tự hào',    15, 3,  1,  0, 0, 0, 0, 1, 0, DATEADD(day, -3, GETDATE())),
(8, 7, N'Mục tiêu tháng tới: cày xong bộ flashcard 1000 từ vựng và pass TOEIC 700+ trước khi chuyển sang IELTS. Anh em có tip nào không? 📚',
    'Text',  'Public', NULL,                 N'quyết tâm', 22, 2,  0,  0, 0, 0, 0, 1, 0, DATEADD(day, -2, GETDATE())),
(9, 9, N'Cuối tuần xách máy ra quán cà phê fix bug C# với ngồi nghiên cứu lại mớ Delegates với LINQ. Càng học càng thấy lú ☕',
    'Image', 'Public', N'The Coffee House',   N'tập trung', 30, 4,  0,  0, 0, 0, 0, 1, 0, DATEADD(day, -1, GETDATE())),
(10, 8, N'[THÔNG BÁO HỆ THỐNG] InteractHub sẽ tiến hành bảo trì server và update API vào 00:00 đêm nay. Mong mọi người lưu ý.',
    'Text',  'Public', NULL,                 NULL,         45, 0,  5,  0, 1, 0, 0, 0, 0, GETDATE());
SET IDENTITY_INSERT [Posts] OFF;

-- ------------------------------------------------------------
-- Post Hashtags
-- ------------------------------------------------------------
SET IDENTITY_INSERT [PostHashtags] ON;
INSERT INTO [PostHashtags] ([post_hashtag_id], [post_id], [hashtag_id], [delflg], [reg_datetime])
VALUES
(8, 7, 3, 0, DATEADD(day, -3, GETDATE())), -- post 7 -> #coding
(9, 9, 3, 0, DATEADD(day, -1, GETDATE())); -- post 9 -> #coding
SET IDENTITY_INSERT [PostHashtags] OFF;

-- ------------------------------------------------------------
-- Post Likes
-- ------------------------------------------------------------
SET IDENTITY_INSERT [PostLikes] ON;
INSERT INTO [PostLikes] ([like_id], [post_id], [user_id], [reaction_type], [delflg], [reg_datetime])
VALUES
(8,  7, 9, 'Wow',  0, DATEADD(day, -3, GETDATE())),
(9,  7, 8, 'Like', 0, DATEADD(day, -3, GETDATE())),
(10, 8, 3, 'Love', 0, DATEADD(day, -2, GETDATE())),
(11, 9, 7, 'Haha', 0, DATEADD(day, -1, GETDATE()));
SET IDENTITY_INSERT [PostLikes] OFF;

-- ------------------------------------------------------------
-- Post Shares (ryan share bài của manh, admin share nội quy)
-- Giả định schema PostShares cơ bản
-- ------------------------------------------------------------
SET IDENTITY_INSERT [PostShares] ON;
INSERT INTO [PostShares] ([share_id], [post_id], [user_id], [share_content], [delflg], [reg_datetime])
VALUES
(1, 7, 9, N'Lưu lại để bữa sau hỏi bác này cách config Azure.', 0, DATEADD(day, -2, GETDATE())),
(2, 10, 2, NULL, 0, GETDATE());
SET IDENTITY_INSERT [PostShares] OFF;

-- ------------------------------------------------------------
-- Comments (Tương tác qua lại giữa manh, ryan và các user khác)
-- ------------------------------------------------------------
SET IDENTITY_INSERT [Comments] ON;
INSERT INTO [Comments] ([comment_id], [post_id], [user_id], [parent_comment_id], [content], [like_count], [reply_count], [is_edited], [is_reported], [delflg], [reg_datetime])
VALUES
(8,  7, 9, NULL, N'Đỉnh quá bác, bữa nào rảnh chỉ mình cách setup vụ database với nhé!', 1, 1, 0, 0, 0, DATEADD(day, -3, GETDATE())),
(9,  7, 7, 8,    N'Ok bác ơi, dễ lắm, dùng tool có sẵn của Microsoft deploy 1 nốt nhạc.', 2, 0, 0, 0, 0, DATEADD(day, -3, GETDATE())),
(10, 8, 3, NULL, N'Tạo set từ vựng trên STUDY4 học cho lẹ bác, mỗi ngày 50 từ là vừa miếng.', 1, 0, 0, 0, 0, DATEADD(day, -2, GETDATE())),
(11, 9, 7, NULL, N'C# thì IEnumerable với LINQ là chân ái rồi, cố lên bác!', 2, 0, 0, 0, 0, DATEADD(day, -1, GETDATE()));
SET IDENTITY_INSERT [Comments] OFF;

-- ------------------------------------------------------------
-- Stories
-- ------------------------------------------------------------
SET IDENTITY_INSERT [Stories] ON;
INSERT INTO [Stories] (
    [story_id], [user_id],
    [media_url], [media_type],
    [caption], [bg_color], [duration_sec], [visibility],
    [view_count], [reaction_count],
    [expire_datetime], [is_expired], [is_highlighted], [highlight_name],
    [delflg], [reg_datetime]
)
VALUES
(8, 9,
    'https://storage.interacthub.com/stories/ryan-coffee.jpg', 'Image',
    N'Cuối tuần chạy deadline 😢', '#000000', 5, 'Public',
    15, 2,
    DATEADD(hour, 24, GETDATE()), 0, 0, NULL,
    0, GETDATE()),
(9, 8,
    'https://storage.interacthub.com/stories/admin-system.jpg', 'Image',
    N'Monitoring server health... 99.9% Uptime', '#1a2b3c', 10, 'Public',
    100, 5,
    DATEADD(hour, 24, GETDATE()), 0, 1, 'System Logs',
    0, GETDATE());
SET IDENTITY_INSERT [Stories] OFF;

-- ------------------------------------------------------------
-- Story Views & Reactions
-- ------------------------------------------------------------
SET IDENTITY_INSERT [StoryViews] ON;
INSERT INTO [StoryViews] ([view_id], [story_id], [viewer_id], [view_duration], [delflg], [reg_datetime])
VALUES
(7, 8, 7, 5, 0, GETDATE()),
(8, 9, 7, 3, 0, GETDATE()),
(9, 9, 9, 4, 0, GETDATE());
SET IDENTITY_INSERT [StoryViews] OFF;

SET IDENTITY_INSERT [StoryReactions] ON;
INSERT INTO [StoryReactions] ([reaction_id], [story_id], [user_id], [reaction_type], [delflg], [reg_datetime])
VALUES
(7, 8, 7, 'Haha', 0, GETDATE()),
(8, 9, 7, 'Wow',  0, GETDATE());
SET IDENTITY_INSERT [StoryReactions] OFF;


-- ============================================================
-- BƯỚC 6: CẬP NHẬT LẠI IDENTITY SEED = MAX(id) đã insert
--         để AUTO_INCREMENT tiếp tục đúng từ số tiếp theo
-- ============================================================
DBCC CHECKIDENT ('[AspNetRoles]',    RESEED, 3);
DBCC CHECKIDENT ('[AspNetUsers]',    RESEED, 10);
DBCC CHECKIDENT ('[UserProfiles]',   RESEED, 6);
DBCC CHECKIDENT ('[Friendships]',    RESEED, 17);
DBCC CHECKIDENT ('[Hashtags]',       RESEED, 5);
DBCC CHECKIDENT ('[MusicTracks]',    RESEED, 3);
DBCC CHECKIDENT ('[Posts]',          RESEED, 10);
DBCC CHECKIDENT ('[PostMedia]',      RESEED, 3);
DBCC CHECKIDENT ('[PostHashtags]',   RESEED, 9);
DBCC CHECKIDENT ('[PostLikes]',      RESEED, 11);
DBCC CHECKIDENT ('[PostShares]',     RESEED, 2);
DBCC CHECKIDENT ('[Comments]',       RESEED, 11);
DBCC CHECKIDENT ('[CommentLikes]',   RESEED, 4);
DBCC CHECKIDENT ('[Stories]',        RESEED, 9);
DBCC CHECKIDENT ('[StoryViews]',     RESEED, 9);
DBCC CHECKIDENT ('[StoryReactions]', RESEED, 8);
DBCC CHECKIDENT ('[Notifications]',  RESEED, 7);
DBCC CHECKIDENT ('[PostReports]',    RESEED, 1);

COMMIT TRANSACTION;

-- ============================================================
-- HƯỚNG DẪN THÊM DATA MỚI (chạy full file vẫn ổn định):
-- ============================================================
-- Khi muốn thêm user mới, chỉ cần thêm vào block INSERT [AspNetUsers]
-- với Id tiếp theo (11, 12, ...) và cập nhật RESEED tương ứng.
--
-- Tương tự cho các bảng khác: thêm row mới vào block INSERT,
-- cập nhật RESEED ở cuối = max Id mới.
--
-- File đã được thiết kế idempotent:
--   Mỗi lần chạy full: DELETE hết → INSERT lại → RESEED đúng
--   Thêm INSERT/UPDATE bên dưới COMMIT vẫn hoạt động bình thường
--    Foreign key được bật lại đúng thứ tự
-- ============================================================