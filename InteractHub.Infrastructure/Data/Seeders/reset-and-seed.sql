-- MSSQL reset + seed script for InteractHub
-- Run from project root:
-- cat InteractHub.Infrastructure/Data/Seeders/reset-and-seed.sql | docker exec -i interacthub-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'Interacthub@123A' -d InteractHubDb -C -i /dev/stdin

BEGIN TRANSACTION;

-- Disable foreign key constraints explicitly
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

-- Delete data in correct order (child tables first)
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

-- Re-enable foreign key constraints
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

-- Reset identity columns
DBCC CHECKIDENT ('[AspNetRoles]', RESEED, 0);
DBCC CHECKIDENT ('[AspNetUsers]', RESEED, 0);
DBCC CHECKIDENT ('[UserProfiles]', RESEED, 0);
DBCC CHECKIDENT ('[Friendships]', RESEED, 0);
DBCC CHECKIDENT ('[Hashtags]', RESEED, 0);
DBCC CHECKIDENT ('[MusicTracks]', RESEED, 0);
DBCC CHECKIDENT ('[Posts]', RESEED, 0);
DBCC CHECKIDENT ('[PostMedia]', RESEED, 0);
DBCC CHECKIDENT ('[PostHashtags]', RESEED, 0);
DBCC CHECKIDENT ('[PostLikes]', RESEED, 0);
DBCC CHECKIDENT ('[Comments]', RESEED, 0);
DBCC CHECKIDENT ('[CommentLikes]', RESEED, 0);
DBCC CHECKIDENT ('[Stories]', RESEED, 0);
DBCC CHECKIDENT ('[StoryViews]', RESEED, 0);
DBCC CHECKIDENT ('[StoryReactions]', RESEED, 0);
DBCC CHECKIDENT ('[Notifications]', RESEED, 0);

-- Roles
SET IDENTITY_INSERT [AspNetRoles] ON;
INSERT INTO [AspNetRoles] ([Id], [description], [delflg], [reg_datetime], [Name], [NormalizedName], [ConcurrencyStamp])
VALUES
(1, 'System Administrator', 0, GETDATE(), 'Admin', 'ADMIN', 'seed-role-admin'),
(2, 'Content Moderator',    0, GETDATE(), 'Moderator', 'MODERATOR', 'seed-role-moderator'),
(3, 'General User',         0, GETDATE(), 'User', 'USER', 'seed-role-user');
SET IDENTITY_INSERT [AspNetRoles] OFF;

-- Users
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
);
SET IDENTITY_INSERT [AspNetUsers] OFF;

INSERT INTO [AspNetUserRoles] ([UserId], [RoleId])
VALUES
(2, 3),
(3, 3),
(4, 3),
(5, 3),
(6, 3);

-- Hashtags
SET IDENTITY_INSERT [Hashtags] ON;
INSERT INTO [Hashtags] ([hashtag_id], [tag_name], [post_count], [trending_score], [is_trending], [delflg], [reg_datetime])
VALUES
(1, 'travel',  15, 95.50, 1, 0, GETDATE()),
(2, 'food',    12, 88.00, 1, 0, GETDATE()),
(3, 'coding',  10, 82.50, 1, 0, GETDATE()),
(4, 'art',      8, 75.00, 0, 0, GETDATE()),
(5, 'vietnam', 11, 85.00, 1, 0, GETDATE());
SET IDENTITY_INSERT [Hashtags] OFF;

-- Music Tracks
SET IDENTITY_INSERT [MusicTracks] ON;
INSERT INTO [MusicTracks] ([music_id], [title], [artist], [audio_url], [thumbnail_url], [duration_sec], [is_licensed], [source], [delflg], [reg_datetime])
VALUES
(1, 'Chill Vibes',    'Lo-Fi Studio',   'https://storage.interacthub.com/music/chill-vibes.mp3',    'https://storage.interacthub.com/music/thumbs/chill-vibes.jpg',    180, 1, 'Internal', 0, GETDATE()),
(2, 'Morning Coffee', 'Jazz Quartet',   'https://storage.interacthub.com/music/morning-coffee.mp3', 'https://storage.interacthub.com/music/thumbs/morning-coffee.jpg', 195, 1, 'Internal', 0, GETDATE()),
(3, 'Epic Cinematic', 'Orchestra Pro',  'https://storage.interacthub.com/music/epic-cinematic.mp3', 'https://storage.interacthub.com/music/thumbs/epic-cinematic.jpg',  300, 1, 'Internal', 0, GETDATE());
SET IDENTITY_INSERT [MusicTracks] OFF;

-- User Profiles
SET IDENTITY_INSERT [UserProfiles] ON;
INSERT INTO [UserProfiles] (
    [profile_id], [user_id],
    [relationship_status], [work_place], [position], [education],
    [hometown], [current_city],
    [facebook_link], [instagram_link], [twitter_link],
    [follower_count], [following_count], [post_count], [friend_count],
    [privacy_posts], [privacy_friends], [privacy_photos],
    [notification_email_flg], [notification_push_flg],
    [delflg], [reg_datetime]
)
VALUES
(1, 2, N'Độc thân', 'FPT Software',     'Designer',        N'Đại học Sài Gòn', N'Hà Nội',  N'Hồ Chí Minh', NULL, 'alice_ng',  NULL,     120, 85,  8,  45, 'Public', 'Public',  'Public',  1, 1, 0, GETDATE()),
(2, 3, N'Độc thân', 'VNG Corporation',  'Backend Developer','HCMUT',            N'Hà Nội',  N'Hà Nội',      NULL, 'bob_tran',  'bobtran', 98,  72,  5,  38, 'Public', 'Friends', 'Public',  1, 1, 0, GETDATE()),
(3, 4, N'Hẹn hò',   'Grab Vietnam',     'Content Creator', N'Đại học Sài Gòn', N'Cần Thơ', N'Hồ Chí Minh', NULL, 'carol_le',  NULL,     215, 130, 12, 67, 'Public', 'Public',  'Public',  1, 1, 0, GETDATE()),
(4, 5, N'Độc thân', 'Freelance',        'Full-stack Dev',  N'ĐH Đà Nẵng',      N'Đà Nẵng', N'Đà Nẵng',     NULL, 'david_ph',  'davidpham',76, 54,  6,  29, 'Public', 'Public',  'Friends', 1, 0, 0, GETDATE()),
(5, 6, N'Độc thân', 'Studio Freelance', 'Illustrator',     N'ĐH Mỹ thuật HCM', N'Huế',     N'Hồ Chí Minh', NULL, 'eva_hoang', NULL,     340, 210, 15, 89, 'Public', 'Public',  'Public',  1, 1, 0, GETDATE());
SET IDENTITY_INSERT [UserProfiles] OFF;

-- Friendships
SET IDENTITY_INSERT [Friendships] ON;
INSERT INTO [Friendships] ([friendship_id], [requester_id], [addressee_id], [status], [action_user_id], [is_blocked], [delflg], [reg_datetime])
VALUES
(1, 2, 3, 'Accepted', 3, 0, 0, DATEADD(day, -6, GETDATE())),
(2, 2, 4, 'Accepted', 4, 0, 0, DATEADD(day, -5, GETDATE())),
(3, 3, 5, 'Accepted', 5, 0, 0, DATEADD(day, -4, GETDATE())),
(4, 4, 6, 'Accepted', 6, 0, 0, DATEADD(day, -3, GETDATE())),
(5, 2, 5, 'Pending',  2, 0, 0, DATEADD(day, -1, GETDATE()));
SET IDENTITY_INSERT [Friendships] OFF;

-- Posts
SET IDENTITY_INSERT [Posts] ON;
INSERT INTO [Posts] (
    [post_id], [user_id], [content], [post_type], [visibility],
    [location_name], [feeling],
    [like_count], [comment_count], [share_count],
    [is_edited], [is_pinned], [is_reported], [report_count], [allow_comment],
    [delflg], [reg_datetime]
)
VALUES
(1, 2, N'Vừa đến Đà Lạt, thời tiết tuyệt vời quá! #travel #vietnam', 'Text', 'Public', N'Đà Lạt',          N'hạnh phúc', 24, 5,  3,  0, 0, 0, 0, 1, 0, DATEADD(day, -6, GETDATE())),
(2, 3, N'Vừa deploy xong feature mới lên production mà không có bug nào 🎉 #coding', 'Text', 'Public', NULL, N'tự hào',    18, 7,  2,  0, 0, 0, 0, 1, 0, DATEADD(day, -5, GETDATE())),
(3, 4, N'Review bánh mì Hòa Mã - địa chỉ quá quen thuộc với dân Sài Gòn 🥖 #food', 'Image', 'Public', N'Bánh mì Hòa Mã', N'thích thú', 45, 12, 8,  0, 0, 0, 0, 1, 0, DATEADD(day, -4, GETDATE())),
(4, 5, N'Mình vừa contribute vào một open-source project khá thú vị. Ai quan tâm đến Rust không? #coding #vietnam', 'Text', 'Public', NULL, NULL, 31, 9,  5,  0, 0, 0, 0, 1, 0, DATEADD(day, -3, GETDATE())),
(5, 6, N'Xong bức tranh mới sau 2 tuần! Cảm ơn mọi người đã ủng hộ mình 🎨 #art',  'Image', 'Public', NULL, N'tự hào',    89, 23, 14, 0, 1, 0, 0, 1, 0, DATEADD(day, -2, GETDATE()));
SET IDENTITY_INSERT [Posts] OFF;

-- Post Media
SET IDENTITY_INSERT [PostMedia] ON;
INSERT INTO [PostMedia] ([media_id], [post_id], [media_url], [media_type], [file_name], [file_size_kb], [width_px], [height_px], [sort_order], [processing_status], [delflg], [reg_datetime])
VALUES
(1, 3, 'https://storage.interacthub.com/posts/3/banhmi-1.jpg', 'Image', 'banhmi-1.jpg', 245, 1080, 1080, 0, 'Ready', 0, DATEADD(day, -4, GETDATE())),
(2, 3, 'https://storage.interacthub.com/posts/3/banhmi-2.jpg', 'Image', 'banhmi-2.jpg', 312, 1080, 720,  1, 'Ready', 0, DATEADD(day, -4, GETDATE())),
(3, 5, 'https://storage.interacthub.com/posts/5/painting.jpg', 'Image', 'painting.jpg', 892, 2048, 1536, 0, 'Ready', 0, DATEADD(day, -2, GETDATE()));
SET IDENTITY_INSERT [PostMedia] OFF;

-- Post Hashtags
SET IDENTITY_INSERT [PostHashtags] ON;
INSERT INTO [PostHashtags] ([post_hashtag_id], [post_id], [hashtag_id], [delflg], [reg_datetime])
VALUES
(1, 1, 1, 0, DATEADD(day, -6, GETDATE())),
(2, 1, 5, 0, DATEADD(day, -6, GETDATE())),
(3, 2, 3, 0, DATEADD(day, -5, GETDATE())),
(4, 3, 2, 0, DATEADD(day, -4, GETDATE())),
(5, 4, 3, 0, DATEADD(day, -3, GETDATE())),
(6, 5, 4, 0, DATEADD(day, -2, GETDATE()));
SET IDENTITY_INSERT [PostHashtags] OFF;

-- Post Likes
SET IDENTITY_INSERT [PostLikes] ON;
INSERT INTO [PostLikes] ([like_id], [post_id], [user_id], [reaction_type], [delflg], [reg_datetime])
VALUES
(1, 1, 3, 'Love', 0, DATEADD(day, -6, GETDATE())),
(2, 1, 4, 'Like', 0, DATEADD(day, -6, GETDATE())),
(3, 2, 2, 'Haha', 0, DATEADD(day, -5, GETDATE())),
(4, 3, 2, 'Love', 0, DATEADD(day, -4, GETDATE())),
(5, 5, 2, 'Love', 0, DATEADD(day, -2, GETDATE()));
SET IDENTITY_INSERT [PostLikes] OFF;

-- Comments
SET IDENTITY_INSERT [Comments] ON;
INSERT INTO [Comments] ([comment_id], [post_id], [user_id], [parent_comment_id], [content], [like_count], [reply_count], [is_edited], [is_reported], [delflg], [reg_datetime])
VALUES
(1, 1, 3, NULL, N'Đà Lạt đẹp lắm! Mình cũng muốn đi quá 😍',          5, 1, 0, 0, 0, DATEADD(day, -5, GETDATE())),
(2, 1, 4, NULL, N'Thời điểm này đi Đà Lạt là chuẩn nhất luôn!',        3, 0, 0, 0, 0, DATEADD(day, -4, GETDATE())),
(3, 2, 2, NULL, N'Giỏi vậy! Mình thì deploy xong là có bug ngay 😅',   4, 1, 0, 0, 0, DATEADD(day, -4, GETDATE())),
(4, 3, 2, NULL, N'Bánh mì Hòa Mã ngon thật!',                           6, 0, 0, 0, 0, DATEADD(day, -3, GETDATE())),
(5, 5, 2, NULL, N'Tranh đẹp quá! Bạn vẽ bao lâu vậy?',                 8, 1, 0, 0, 0, DATEADD(day, -1, GETDATE())),
(6, 5, 6, 5,   N'Mình vẽ khoảng 2 tuần, tranh 60x80cm bạn ơi!',        3, 0, 0, 0, 0, DATEADD(hour, -20, GETDATE()));
SET IDENTITY_INSERT [Comments] OFF;

-- Comment Likes
SET IDENTITY_INSERT [CommentLikes] ON;
INSERT INTO [CommentLikes] ([like_id], [comment_id], [user_id], [reaction_type], [delflg], [reg_datetime])
VALUES
(1, 1, 2, 'Like', 0, DATEADD(day, -5, GETDATE())),
(2, 1, 5, 'Love', 0, DATEADD(day, -5, GETDATE())),
(3, 3, 3, 'Haha', 0, DATEADD(day, -4, GETDATE())),
(4, 5, 6, 'Like', 0, DATEADD(day, -1, GETDATE()));
SET IDENTITY_INSERT [CommentLikes] OFF;

-- Stories
SET IDENTITY_INSERT [Stories] ON;
INSERT INTO [Stories] (
    [story_id], [user_id],
    [media_url], [media_type], [thumbnail_url],
    [caption], [bg_color], [duration_sec], [visibility],
    [view_count], [reaction_count],
    [expire_datetime], [is_expired], [is_highlighted], [highlight_name],
    [delflg], [reg_datetime]
)
VALUES
(1, 2, 'https://storage.interacthub.com/stories/alice-dalat.jpg', 'Image', NULL, N'Đà Lạt buổi sáng 🌿', '#2D5016', 5, 'Friends', 45,  8,  DATEADD(hour, 18, GETDATE()), 0, 0, NULL,     0, DATEADD(hour, -6, GETDATE())),
(2, 3, 'https://storage.interacthub.com/stories/bob-code.jpg',    'Image', NULL, N'Coding time ☕',       '#1a1a2e', 5, 'Public',  32,  5,  DATEADD(hour, 18, GETDATE()), 0, 0, NULL,     0, DATEADD(hour, -4, GETDATE())),
(3, 6, 'https://storage.interacthub.com/stories/eva-art.jpg',     'Image', NULL, N'Work in progress 🎨', '#fff5e6', 5, 'Public',  120, 25, DATEADD(hour, 22, GETDATE()), 0, 1, 'Artwork', 0, DATEADD(hour, -2, GETDATE()));
SET IDENTITY_INSERT [Stories] OFF;

-- Story Views
SET IDENTITY_INSERT [StoryViews] ON;
INSERT INTO [StoryViews] ([view_id], [story_id], [viewer_id], [view_duration], [delflg], [reg_datetime])
VALUES
(1, 1, 3, 5,  0, DATEADD(hour, -5, GETDATE())),
(2, 1, 4, 5,  0, DATEADD(hour, -4, GETDATE())),
(3, 2, 2, 5,  0, DATEADD(hour, -3, GETDATE())),
(4, 3, 2, 12, 0, DATEADD(hour, -2, GETDATE()));
SET IDENTITY_INSERT [StoryViews] OFF;

-- Story Reactions
SET IDENTITY_INSERT [StoryReactions] ON;
INSERT INTO [StoryReactions] ([reaction_id], [story_id], [user_id], [reaction_type], [delflg], [reg_datetime])
VALUES
(1, 1, 3, 'Love', 0, DATEADD(hour, -5,  GETDATE())),
(2, 2, 2, 'Like', 0, DATEADD(hour, -3,  GETDATE())),
(3, 3, 2, 'Love', 0, DATEADD(hour, -2,  GETDATE())),
(4, 3, 5, 'Wow',  0, DATEADD(minute, -90, GETDATE()));
SET IDENTITY_INSERT [StoryReactions] OFF;

-- Notifications
SET IDENTITY_INSERT [Notifications] ON;
INSERT INTO [Notifications] (
    [notification_id], [recipient_id], [sender_id],
    [notification_type], [reference_id], [reference_type],
    [message], [is_read], [redirect_url],
    [delflg], [reg_datetime]
)
VALUES
(1, 2, 3, 'post_like',     1, 'Post',       N'Bob đã thích bài viết của bạn',        0, '/posts/1',   0, DATEADD(hour, -5, GETDATE())),
(2, 2, 4, 'post_comment',  1, 'Post',       N'Carol đã bình luận về bài viết của bạn',0, '/posts/1',   0, DATEADD(hour, -4, GETDATE())),
(3, 3, 2, 'friend_request',1, 'Friendship', N'Alice đã gửi lời mời kết bạn',          1, '/friends',   0, DATEADD(hour, -3, GETDATE())),
(4, 6, 2, 'story_view',    3, 'Story',      N'Alice đã xem story của bạn',            0, '/stories/3', 0, DATEADD(hour, -2, GETDATE())),
(5, 6, 5, 'post_like',     5, 'Post',       N'David đã thích bài viết của bạn',       0, '/posts/5',   0, DATEADD(hour, -1, GETDATE()));
SET IDENTITY_INSERT [Notifications] OFF;

-- Update identity seeds to match inserted max IDs
DBCC CHECKIDENT ('[AspNetRoles]',    RESEED, 3);
DBCC CHECKIDENT ('[AspNetUsers]',    RESEED, 6);
DBCC CHECKIDENT ('[UserProfiles]',   RESEED, 5);
DBCC CHECKIDENT ('[Friendships]',    RESEED, 5);
DBCC CHECKIDENT ('[Hashtags]',       RESEED, 5);
DBCC CHECKIDENT ('[MusicTracks]',    RESEED, 3);
DBCC CHECKIDENT ('[Posts]',          RESEED, 5);
DBCC CHECKIDENT ('[PostMedia]',      RESEED, 3);
DBCC CHECKIDENT ('[PostHashtags]',   RESEED, 6);
DBCC CHECKIDENT ('[PostLikes]',      RESEED, 5);
DBCC CHECKIDENT ('[Comments]',       RESEED, 6);
DBCC CHECKIDENT ('[CommentLikes]',   RESEED, 4);
DBCC CHECKIDENT ('[Stories]',        RESEED, 3);
DBCC CHECKIDENT ('[StoryViews]',     RESEED, 4);
DBCC CHECKIDENT ('[StoryReactions]', RESEED, 4);
DBCC CHECKIDENT ('[Notifications]',  RESEED, 5);

COMMIT TRANSACTION;