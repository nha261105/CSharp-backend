-- ============================================
-- InteractHub Sample Data Seed Script
-- Chạy lệnh: 
-- docker exec -i interacthub-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "interacthub@123" -d InteractHubDB -No -i seed.sql
-- ============================================

USE InteractHubDB;
GO

SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

-- ============================================
-- 1. HASHTAGS
-- ============================================
SET IDENTITY_INSERT Hashtags ON;

INSERT INTO Hashtags (hashtag_id, tag_name, post_count, trending_score, is_trending, delflg, reg_datetime)
VALUES
(1, 'travel',        15, 95.50, 1, 0, GETDATE()),
(2, 'food',          12, 88.00, 1, 0, GETDATE()),
(3, 'technology',    10, 82.50, 1, 0, GETDATE()),
(4, 'photography',    8, 75.00, 1, 0, GETDATE()),
(5, 'fitness',        7, 70.00, 0, 0, GETDATE()),
(6, 'music',          6, 65.00, 0, 0, GETDATE()),
(7, 'art',            5, 60.00, 0, 0, GETDATE()),
(8, 'coding',         9, 78.00, 1, 0, GETDATE()),
(9, 'vietnam',       11, 85.00, 1, 0, GETDATE()),
(10,'hochiminh',      8, 72.00, 0, 0, GETDATE());

SET IDENTITY_INSERT Hashtags OFF;
GO

-- ============================================
-- 2. MUSIC TRACKS
-- ============================================
SET IDENTITY_INSERT MusicTracks ON;

INSERT INTO MusicTracks (music_id, title, artist, audio_url, thumbnail_url, duration_sec, is_licensed, source, delflg, reg_datetime)
VALUES
(1, 'Chill Vibes',        'Lo-Fi Studio',   'https://storage.interacthub.com/music/chill-vibes.mp3',        'https://storage.interacthub.com/music/thumbs/chill-vibes.jpg',        180, 1, 'Internal', 0, GETDATE()),
(2, 'Summer Breeze',      'Acoustic Lab',   'https://storage.interacthub.com/music/summer-breeze.mp3',      'https://storage.interacthub.com/music/thumbs/summer-breeze.jpg',      210, 1, 'Internal', 0, GETDATE()),
(3, 'Night Drive',        'Synthwave Co',   'https://storage.interacthub.com/music/night-drive.mp3',        'https://storage.interacthub.com/music/thumbs/night-drive.jpg',        240, 1, 'Internal', 0, GETDATE()),
(4, 'Morning Coffee',     'Jazz Quartet',   'https://storage.interacthub.com/music/morning-coffee.mp3',     'https://storage.interacthub.com/music/thumbs/morning-coffee.jpg',     195, 1, 'Internal', 0, GETDATE()),
(5, 'Epic Cinematic',     'Orchestra Pro',  'https://storage.interacthub.com/music/epic-cinematic.mp3',     'https://storage.interacthub.com/music/thumbs/epic-cinematic.jpg',     300, 1, 'Internal', 0, GETDATE());

SET IDENTITY_INSERT MusicTracks OFF;
GO

-- ============================================
-- 3. USERS (AspNetUsers)
-- Passwords đều là: Test@1234
-- PasswordHash được gen bởi ASP.NET Identity PBKDF2
-- ============================================
INSERT INTO AspNetUsers (
    Id, UserName, NormalizedUserName,
    Email, NormalizedEmail, EmailConfirmed,
    PasswordHash, SecurityStamp, ConcurrencyStamp,
    PhoneNumber, PhoneNumberConfirmed,
    TwoFactorEnabled, LockoutEnabled, AccessFailedCount,
    full_name, gender, date_of_birth,
    avatar_url, cover_photo_url, bio,
    website_url, location,
    is_active, is_private_account,
    delflg, reg_datetime
)
VALUES
(
    2, 'alice_nguyen', 'ALICE_NGUYEN',
    'alice@gmail.com', 'ALICE@GMAIL.COM', 1,
    'AQAAAAIAAYagAAAAELk4p5pJz8eMWopqM7TA+Wr2aBpHQ3G6iHEWvU2cBFEFcqJkbKqtJlW8Rh5Ue9BWGA==',
    NEWID(), NEWID(),
    '0901234567', 0,
    0, 1, 0,
    N'Nguyễn Thị Alice', 'Female', '1999-05-15',
    'https://storage.interacthub.com/avatars/alice.jpg',
    'https://storage.interacthub.com/covers/alice-cover.jpg',
    N'Yêu thích du lịch và nhiếp ảnh 📸',
    'https://alice.blog', N'Hồ Chí Minh',
    1, 0,
    0, GETDATE()
),
(
    3, 'bob_tran', 'BOB_TRAN',
    'bob@gmail.com', 'BOB@GMAIL.COM', 1,
    'AQAAAAIAAYagAAAAELk4p5pJz8eMWopqM7TA+Wr2aBpHQ3G6iHEWvU2cBFEFcqJkbKqtJlW8Rh5Ue9BWGA==',
    NEWID(), NEWID(),
    '0912345678', 0,
    0, 1, 0,
    N'Trần Văn Bob', 'Male', '2000-08-20',
    'https://storage.interacthub.com/avatars/bob.jpg',
    'https://storage.interacthub.com/covers/bob-cover.jpg',
    N'Lập trình viên | Coffee addict ☕',
    NULL, N'Hà Nội',
    1, 0,
    0, GETDATE()
),
(
    4, 'carol_le', 'CAROL_LE',
    'carol@gmail.com', 'CAROL@GMAIL.COM', 1,
    'AQAAAAIAAYagAAAAELk4p5pJz8eMWopqM7TA+Wr2aBpHQ3G6iHEWvU2cBFEFcqJkbKqtJlW8Rh5Ue9BWGA==',
    NEWID(), NEWID(),
    '0923456789', 0,
    0, 1, 0,
    N'Lê Thị Carol', 'Female', '2001-03-10',
    'https://storage.interacthub.com/avatars/carol.jpg',
    'https://storage.interacthub.com/covers/carol-cover.jpg',
    N'Foodie | Review ẩm thực Sài Gòn 🍜',
    'https://carol.food', N'Hồ Chí Minh',
    1, 0,
    0, GETDATE()
),
(
    5, 'david_pham', 'DAVID_PHAM',
    'david@gmail.com', 'DAVID@GMAIL.COM', 1,
    'AQAAAAIAAYagAAAAELk4p5pJz8eMWopqM7TA+Wr2aBpHQ3G6iHEWvU2cBFEFcqJkbKqtJlW8Rh5Ue9BWGA==',
    NEWID(), NEWID(),
    '0934567890', 0,
    0, 1, 0,
    N'Phạm Văn David', 'Male', '1998-12-25',
    'https://storage.interacthub.com/avatars/david.jpg',
    NULL,
    N'Tech enthusiast | Open source contributor 💻',
    'https://github.com/david', N'Đà Nẵng',
    1, 0,
    0, GETDATE()
),
(
    6, 'eva_hoang', 'EVA_HOANG',
    'eva@gmail.com', 'EVA@GMAIL.COM', 1,
    'AQAAAAIAAYagAAAAELk4p5pJz8eMWopqM7TA+Wr2aBpHQ3G6iHEWvU2cBFEFcqJkbKqtJlW8Rh5Ue9BWGA==',
    NEWID(), NEWID(),
    '0945678901', 0,
    0, 1, 0,
    N'Hoàng Thị Eva', 'Female', '2002-07-04',
    'https://storage.interacthub.com/avatars/eva.jpg',
    'https://storage.interacthub.com/covers/eva-cover.jpg',
    N'Nghệ sĩ tự do | Vẽ tranh & thiết kế 🎨',
    NULL, N'Hồ Chí Minh',
    1, 0,
    0, GETDATE()
);
GO

-- ============================================
-- 4. ASSIGN ROLE "User" CHO CÁC SAMPLE USERS
-- Role Id: 1=Admin, 2=Moderator, 3=User
-- ============================================
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES
(2, 3),  -- alice → User
(3, 3),  -- bob   → User
(4, 3),  -- carol → User
(5, 3),  -- david → User
(6, 3);  -- eva   → User
GO

-- ============================================
-- 5. USER PROFILES
-- ============================================
SET IDENTITY_INSERT UserProfiles ON;

INSERT INTO UserProfiles (
    profile_id, user_id,
    relationship_status, work_place, position, education,
    hometown, current_city,
    facebook_link, instagram_link, twitter_link,
    follower_count, following_count, post_count, friend_count,
    privacy_posts, privacy_friends, privacy_photos,
    notification_email_flg, notification_push_flg,
    delflg, reg_datetime
)
VALUES
(1, 2, N'Độc thân',   N'FPT Software',      N'Designer',          N'Đại học Sài Gòn', N'Hà Nội',    N'Hồ Chí Minh', NULL, 'alice_ng',  NULL,        120, 85,  8,  45, 'Public', 'Public', 'Public', 1, 1, 0, GETDATE()),
(2, 3, N'Độc thân',   N'VNG Corporation',   N'Backend Developer', N'HCMUT',           N'Hà Nội',    N'Hà Nội',      NULL, 'bob_tran',  'bobtran',   98,  72,  5,  38, 'Public', 'Friends','Public', 1, 1, 0, GETDATE()),
(3, 4, N'Hẹn hò',     N'Grab Vietnam',      N'Content Creator',   N'Đại học Sài Gòn', N'Cần Thơ',   N'Hồ Chí Minh', NULL, 'carol_le',  NULL,        215, 130, 12, 67, 'Public', 'Public', 'Public', 1, 1, 0, GETDATE()),
(4, 5, N'Độc thân',   N'Freelance',         N'Full-stack Dev',    N'ĐH Đà Nẵng',      N'Đà Nẵng',   N'Đà Nẵng',     NULL, 'david_ph',  'davidpham', 76,  54,  6,  29, 'Public', 'Public', 'Friends',1, 0, 0, GETDATE()),
(5, 6, N'Độc thân',   N'Studio Freelance',  N'Illustrator',       N'ĐH Mỹ thuật HCM', N'Huế',       N'Hồ Chí Minh', NULL, 'eva_hoang', NULL,        340, 210, 15, 89, 'Public', 'Public', 'Public', 1, 1, 0, GETDATE());

SET IDENTITY_INSERT UserProfiles OFF;
GO

-- ============================================
-- 6. FRIENDSHIPS
-- ============================================
SET IDENTITY_INSERT Friendships ON;

INSERT INTO Friendships (friendship_id, requester_id, addressee_id, status, action_user_id, is_blocked, delflg, reg_datetime)
VALUES
(1, 2, 3, 'Accepted', 3, 0, 0, GETDATE()),  -- alice ↔ bob
(2, 2, 4, 'Accepted', 4, 0, 0, GETDATE()),  -- alice ↔ carol
(3, 3, 5, 'Accepted', 5, 0, 0, GETDATE()),  -- bob   ↔ david
(4, 4, 6, 'Accepted', 6, 0, 0, GETDATE()),  -- carol ↔ eva
(5, 5, 6, 'Accepted', 6, 0, 0, GETDATE()),  -- david ↔ eva
(6, 2, 5, 'Pending',  2, 0, 0, GETDATE()),  -- alice → david (chờ)
(7, 3, 6, 'Pending',  3, 0, 0, GETDATE());  -- bob   → eva   (chờ)

SET IDENTITY_INSERT Friendships OFF;
GO

-- ============================================
-- 7. POSTS
-- ============================================
SET IDENTITY_INSERT Posts ON;

INSERT INTO Posts (
    post_id, user_id, content, post_type, visibility,
    location_name, feeling,
    like_count, comment_count, share_count,
    is_edited, is_pinned, is_reported, report_count, allow_comment,
    delflg, reg_datetime
)
VALUES
(1,  2, N'Vừa đến Đà Lạt, thời tiết tuyệt vời quá! 🌿 #travel #vietnam',                                        'Text',  'Public',  N'Đà Lạt',        N'hạnh phúc',  24, 5, 3, 0, 0, 0, 0, 1, 0, DATEADD(day,-6,GETDATE())),
(2,  3, N'Vừa deploy xong feature mới lên production mà không có bug nào 🎉 Cảm giác tuyệt vời! #coding',        'Text',  'Public',  NULL,              N'tự hào',     18, 7, 2, 0, 0, 0, 0, 1, 0, DATEADD(day,-5,GETDATE())),
(3,  4, N'Review bánh mì Hòa Mã - địa chỉ quá quen thuộc với dân Sài Gòn 🥖 #food #hochiminh',                  'Image', 'Public',  N'Bánh mì Hòa Mã', N'thích thú',  45, 12, 8, 0, 0, 0, 0, 1, 0, DATEADD(day,-4,GETDATE())),
(4,  5, N'Mình vừa contribute vào một open-source project khá thú vị. Ai quan tâm đến Rust không? #coding #technology', 'Text', 'Public', NULL,           NULL,          31, 9, 5, 0, 0, 0, 0, 1, 0, DATEADD(day,-3,GETDATE())),
(5,  6, N'Xong bức tranh mới sau 2 tuần! Cảm ơn mọi người đã ủng hộ mình 🎨 #art #photography',                 'Image', 'Public',  NULL,              N'tự hào',     89, 23, 14,0, 1, 0, 0, 1, 0, DATEADD(day,-2,GETDATE())),
(6,  2, N'Sáng nay uống cà phê ở một quán view đẹp, nhạc hay, không khí dễ chịu ☕ #hochiminh',                  'Image', 'Public',  N'The Coffee House',N'thư thái',  12, 3, 1, 0, 0, 0, 0, 1, 0, DATEADD(day,-1,GETDATE())),
(7,  3, N'Tip hay cho anh em dev: Hãy đọc docs thay vì copy paste StackOverflow mà không hiểu 😅 #coding',       'Text',  'Public',  NULL,              N'hài hước',   56, 18, 22,1, 0, 0, 0, 1, 0, DATEADD(hour,-12,GETDATE())),
(8,  4, N'Cuối tuần này ai muốn đi ăn thử nhà hàng mới khai trương ở Q1 không? #food #hochiminh',               'Text',  'Friends', NULL,              N'hào hứng',   8,  4, 0, 0, 0, 0, 0, 1, 0, DATEADD(hour,-6,GETDATE())),
(9,  5, N'Dark mode mãi là số 1. Ai không đồng ý thì bình luận bên dưới 😂 #coding #technology',                 'Text',  'Public',  NULL,              N'hài hước',   73, 31, 15,0, 0, 0, 0, 1, 0, DATEADD(hour,-3,GETDATE())),
(10, 6, N'Nhận commission vẽ portrait cho khá-- ============================================
-- InteractHub Sample Data Seed Script
-- Chạy lệnh: 
-- docker exec -i interacthub-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "InteractHub@123" -d InteractHubDB -No -i seed.sql
-- ============================================

USE InteractHubDB;
GO

-- ============================================
-- 1. HASHTAGS
-- ============================================
SET IDENTITY_INSERT Hashtags ON;

INSERT INTO Hashtags (hashtag_id, tag_name, post_count, trending_score, is_trending, delflg, reg_datetime)
VALUES
(1, 'travel',        15, 95.50, 1, 0, GETDATE()),
(2, 'food',          12, 88.00, 1, 0, GETDATE()),
(3, 'technology',    10, 82.50, 1, 0, GETDATE()),
(4, 'photography',    8, 75.00, 1, 0, GETDATE()),
(5, 'fitness',        7, 70.00, 0, 0, GETDATE()),
(6, 'music',          6, 65.00, 0, 0, GETDATE()),
(7, 'art',            5, 60.00, 0, 0, GETDATE()),
(8, 'coding',         9, 78.00, 1, 0, GETDATE()),
(9, 'vietnam',       11, 85.00, 1, 0, GETDATE()),
(10,'hochiminh',      8, 72.00, 0, 0, GETDATE());

SET IDENTITY_INSERT Hashtags OFF;
GO

-- ============================================
-- 2. MUSIC TRACKS
-- ============================================
SET IDENTITY_INSERT MusicTracks ON;

INSERT INTO MusicTracks (music_id, title, artist, audio_url, thumbnail_url, duration_sec, is_licensed, source, delflg, reg_datetime)
VALUES
(1, 'Chill Vibes',        'Lo-Fi Studio',   'https://storage.interacthub.com/music/chill-vibes.mp3',        'https://storage.interacthub.com/music/thumbs/chill-vibes.jpg',        180, 1, 'Internal', 0, GETDATE()),
(2, 'Summer Breeze',      'Acoustic Lab',   'https://storage.interacthub.com/music/summer-breeze.mp3',      'https://storage.interacthub.com/music/thumbs/summer-breeze.jpg',      210, 1, 'Internal', 0, GETDATE()),
(3, 'Night Drive',        'Synthwave Co',   'https://storage.interacthub.com/music/night-drive.mp3',        'https://storage.interacthub.com/music/thumbs/night-drive.jpg',        240, 1, 'Internal', 0, GETDATE()),
(4, 'Morning Coffee',     'Jazz Quartet',   'https://storage.interacthub.com/music/morning-coffee.mp3',     'https://storage.interacthub.com/music/thumbs/morning-coffee.jpg',     195, 1, 'Internal', 0, GETDATE()),
(5, 'Epic Cinematic',     'Orchestra Pro',  'https://storage.interacthub.com/music/epic-cinematic.mp3',     'https://storage.interacthub.com/music/thumbs/epic-cinematic.jpg',     300, 1, 'Internal', 0, GETDATE());

SET IDENTITY_INSERT MusicTracks OFF;
GO

-- ============================================
-- 3. USERS (AspNetUsers)
-- Passwords đều là: Test@1234
-- PasswordHash được gen bởi ASP.NET Identity PBKDF2
-- ============================================
INSERT INTO AspNetUsers (
    Id, UserName, NormalizedUserName,
    Email, NormalizedEmail, EmailConfirmed,
    PasswordHash, SecurityStamp, ConcurrencyStamp,
    PhoneNumber, PhoneNumberConfirmed,
    TwoFactorEnabled, LockoutEnabled, AccessFailedCount,
    full_name, gender, date_of_birth,
    avatar_url, cover_photo_url, bio,
    website_url, location,
    is_active, is_private_account,
    delflg, reg_datetime
)
VALUES
(
    2, 'alice_nguyen', 'ALICE_NGUYEN',
    'alice@gmail.com', 'ALICE@GMAIL.COM', 1,
    'AQAAAAIAAYagAAAAELk4p5pJz8eMWopqM7TA+Wr2aBpHQ3G6iHEWvU2cBFEFcqJkbKqtJlW8Rh5Ue9BWGA==',
    NEWID(), NEWID(),
    '0901234567', 0,
    0, 1, 0,
    N'Nguyễn Thị Alice', 'Female', '1999-05-15',
    'https://storage.interacthub.com/avatars/alice.jpg',
    'https://storage.interacthub.com/covers/alice-cover.jpg',
    N'Yêu thích du lịch và nhiếp ảnh 📸',
    'https://alice.blog', N'Hồ Chí Minh',
    1, 0,
    0, GETDATE()
),
(
    3, 'bob_tran', 'BOB_TRAN',
    'bob@gmail.com', 'BOB@GMAIL.COM', 1,
    'AQAAAAIAAYagAAAAELk4p5pJz8eMWopqM7TA+Wr2aBpHQ3G6iHEWvU2cBFEFcqJkbKqtJlW8Rh5Ue9BWGA==',
    NEWID(), NEWID(),
    '0912345678', 0,
    0, 1, 0,
    N'Trần Văn Bob', 'Male', '2000-08-20',
    'https://storage.interacthub.com/avatars/bob.jpg',
    'https://storage.interacthub.com/covers/bob-cover.jpg',
    N'Lập trình viên | Coffee addict ☕',
    NULL, N'Hà Nội',
    1, 0,
    0, GETDATE()
),
(
    4, 'carol_le', 'CAROL_LE',
    'carol@gmail.com', 'CAROL@GMAIL.COM', 1,
    'AQAAAAIAAYagAAAAELk4p5pJz8eMWopqM7TA+Wr2aBpHQ3G6iHEWvU2cBFEFcqJkbKqtJlW8Rh5Ue9BWGA==',
    NEWID(), NEWID(),
    '0923456789', 0,
    0, 1, 0,
    N'Lê Thị Carol', 'Female', '2001-03-10',
    'https://storage.interacthub.com/avatars/carol.jpg',
    'https://storage.interacthub.com/covers/carol-cover.jpg',
    N'Foodie | Review ẩm thực Sài Gòn 🍜',
    'https://carol.food', N'Hồ Chí Minh',
    1, 0,
    0, GETDATE()
),
(
    5, 'david_pham', 'DAVID_PHAM',
    'david@gmail.com', 'DAVID@GMAIL.COM', 1,
    'AQAAAAIAAYagAAAAELk4p5pJz8eMWopqM7TA+Wr2aBpHQ3G6iHEWvU2cBFEFcqJkbKqtJlW8Rh5Ue9BWGA==',
    NEWID(), NEWID(),
    '0934567890', 0,
    0, 1, 0,
    N'Phạm Văn David', 'Male', '1998-12-25',
    'https://storage.interacthub.com/avatars/david.jpg',
    NULL,
    N'Tech enthusiast | Open source contributor 💻',
    'https://github.com/david', N'Đà Nẵng',
    1, 0,
    0, GETDATE()
),
(
    6, 'eva_hoang', 'EVA_HOANG',
    'eva@gmail.com', 'EVA@GMAIL.COM', 1,
    'AQAAAAIAAYagAAAAELk4p5pJz8eMWopqM7TA+Wr2aBpHQ3G6iHEWvU2cBFEFcqJkbKqtJlW8Rh5Ue9BWGA==',
    NEWID(), NEWID(),
    '0945678901', 0,
    0, 1, 0,
    N'Hoàng Thị Eva', 'Female', '2002-07-04',
    'https://storage.interacthub.com/avatars/eva.jpg',
    'https://storage.interacthub.com/covers/eva-cover.jpg',
    N'Nghệ sĩ tự do | Vẽ tranh & thiết kế 🎨',
    NULL, N'Hồ Chí Minh',
    1, 0,
    0, GETDATE()
);
GO

-- ============================================
-- 4. ASSIGN ROLE "User" CHO CÁC SAMPLE USERS
-- Role Id: 1=Admin, 2=Moderator, 3=User
-- ============================================
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES
(2, 3),  -- alice → User
(3, 3),  -- bob   → User
(4, 3),  -- carol → User
(5, 3),  -- david → User
(6, 3);  -- eva   → User
GO

-- ============================================
-- 5. USER PROFILES
-- ============================================
SET IDENTITY_INSERT UserProfiles ON;

INSERT INTO UserProfiles (
    profile_id, user_id,
    relationship_status, work_place, position, education,
    hometown, current_city,
    facebook_link, instagram_link, twitter_link,
    follower_count, following_count, post_count, friend_count,
    privacy_posts, privacy_friends, privacy_photos,
    notification_email_flg, notification_push_flg,
    delflg, reg_datetime
)
VALUES
(1, 2, N'Độc thân',   N'FPT Software',      N'Designer',          N'Đại học Sài Gòn', N'Hà Nội',    N'Hồ Chí Minh', NULL, 'alice_ng',  NULL,        120, 85,  8,  45, 'Public', 'Public', 'Public', 1, 1, 0, GETDATE()),
(2, 3, N'Độc thân',   N'VNG Corporation',   N'Backend Developer', N'HCMUT',           N'Hà Nội',    N'Hà Nội',      NULL, 'bob_tran',  'bobtran',   98,  72,  5,  38, 'Public', 'Friends','Public', 1, 1, 0, GETDATE()),
(3, 4, N'Hẹn hò',     N'Grab Vietnam',      N'Content Creator',   N'Đại học Sài Gòn', N'Cần Thơ',   N'Hồ Chí Minh', NULL, 'carol_le',  NULL,        215, 130, 12, 67, 'Public', 'Public', 'Public', 1, 1, 0, GETDATE()),
(4, 5, N'Độc thân',   N'Freelance',         N'Full-stack Dev',    N'ĐH Đà Nẵng',      N'Đà Nẵng',   N'Đà Nẵng',     NULL, 'david_ph',  'davidpham', 76,  54,  6,  29, 'Public', 'Public', 'Friends',1, 0, 0, GETDATE()),
(5, 6, N'Độc thân',   N'Studio Freelance',  N'Illustrator',       N'ĐH Mỹ thuật HCM', N'Huế',       N'Hồ Chí Minh', NULL, 'eva_hoang', NULL,        340, 210, 15, 89, 'Public', 'Public', 'Public', 1, 1, 0, GETDATE());

SET IDENTITY_INSERT UserProfiles OFF;
GO

-- ============================================
-- 6. FRIENDSHIPS
-- ============================================
SET IDENTITY_INSERT Friendships ON;

INSERT INTO Friendships (friendship_id, requester_id, addressee_id, status, action_user_id, is_blocked, delflg, reg_datetime)
VALUES
(1, 2, 3, 'Accepted', 3, 0, 0, GETDATE()),  -- alice ↔ bob
(2, 2, 4, 'Accepted', 4, 0, 0, GETDATE()),  -- alice ↔ carol
(3, 3, 5, 'Accepted', 5, 0, 0, GETDATE()),  -- bob   ↔ david
(4, 4, 6, 'Accepted', 6, 0, 0, GETDATE()),  -- carol ↔ eva
(5, 5, 6, 'Accepted', 6, 0, 0, GETDATE()),  -- david ↔ eva
(6, 2, 5, 'Pending',  2, 0, 0, GETDATE()),  -- alice → david (chờ)
(7, 3, 6, 'Pending',  3, 0, 0, GETDATE());  -- bob   → eva   (chờ)

SET IDENTITY_INSERT Friendships OFF;
GO

-- ============================================
-- 7. POSTS
-- ============================================
SET IDENTITY_INSERT Posts ON;

INSERT INTO Posts (
    post_id, user_id, content, post_type, visibility,
    location_name, feeling,
    like_count, comment_count, share_count,
    is_edited, is_pinned, is_reported, report_count, allow_comment,
    delflg, reg_datetime
)
VALUES
(1,  2, N'Vừa đến Đà Lạt, thời tiết tuyệt vời quá! 🌿 #travel #vietnam',                                        'Text',  'Public',  N'Đà Lạt',        N'hạnh phúc',  24, 5, 3, 0, 0, 0, 0, 1, 0, DATEADD(day,-6,GETDATE())),
(2,  3, N'Vừa deploy xong feature mới lên production mà không có bug nào 🎉 Cảm giác tuyệt vời! #coding',        'Text',  'Public',  NULL,              N'tự hào',     18, 7, 2, 0, 0, 0, 0, 1, 0, DATEADD(day,-5,GETDATE())),
(3,  4, N'Review bánh mì Hòa Mã - địa chỉ quá quen thuộc với dân Sài Gòn 🥖 #food #hochiminh',                  'Image', 'Public',  N'Bánh mì Hòa Mã', N'thích thú',  45, 12, 8, 0, 0, 0, 0, 1, 0, DATEADD(day,-4,GETDATE())),
(4,  5, N'Mình vừa contribute vào một open-source project khá thú vị. Ai quan tâm đến Rust không? #coding #technology', 'Text', 'Public', NULL,           NULL,          31, 9, 5, 0, 0, 0, 0, 1, 0, DATEADD(day,-3,GETDATE())),
(5,  6, N'Xong bức tranh mới sau 2 tuần! Cảm ơn mọi người đã ủng hộ mình 🎨 #art #photography',                 'Image', 'Public',  NULL,              N'tự hào',     89, 23, 14,0, 1, 0, 0, 1, 0, DATEADD(day,-2,GETDATE())),
(6,  2, N'Sáng nay uống cà phê ở một quán view đẹp, nhạc hay, không khí dễ chịu ☕ #hochiminh',                  'Image', 'Public',  N'The Coffee House',N'thư thái',  12, 3, 1, 0, 0, 0, 0, 1, 0, DATEADD(day,-1,GETDATE())),
(7,  3, N'Tip hay cho anh em dev: Hãy đọc docs thay vì copy paste StackOverflow mà không hiểu 😅 #coding',       'Text',  'Public',  NULL,              N'hài hước',   56, 18, 22,1, 0, 0, 0, 1, 0, DATEADD(hour,-12,GETDATE())),
(8,  4, N'Cuối tuần này ai muốn đi ăn thử nhà hàng mới khai trương ở Q1 không? #food #hochiminh',               'Text',  'Friends', NULL,              N'hào hứng',   8,  4, 0, 0, 0, 0, 0, 1, 0, DATEADD(hour,-6,GETDATE())),
(9,  5, N'Dark mode mãi là số 1. Ai không đồng ý thì bình luận bên dưới 😂 #coding #technology',                 'Text',  'Public',  NULL,              N'hài hước',   73, 31, 15,0, 0, 0, 0, 1, 0, DATEADD(hour,-3,GETDATE())),
(10, 6, N'Nhận commission vẽ portrait cho khách hàng đầu tiên! Cảm ơn mọi người đã giới thiệu 🙏 #art',         'Image', 'Public',  NULL,              N'biết ơn',    102,27, 18,0, 0, 0, 0, 1, 0, DATEADD(hour,-1,GETDATE()));

SET IDENTITY_INSERT Posts OFF;
GO

-- ============================================
-- 8. POST MEDIA
-- ============================================
SET IDENTITY_INSERT PostMedia ON;

INSERT INTO PostMedia (media_id, post_id, media_url, media_type, file_name, file_size_kb, width_px, height_px, sort_order, processing_status, delflg, reg_datetime)
VALUES
(1,  3,  'https://storage.interacthub.com/posts/3/banhmi-1.jpg',   'Image', 'banhmi-1.jpg',   245, 1080, 1080, 0, 'Ready', 0, GETDATE()),
(2,  3,  'https://storage.interacthub.com/posts/3/banhmi-2.jpg',   'Image', 'banhmi-2.jpg',   312, 1080, 720,  1, 'Ready', 0, GETDATE()),
(3,  5,  'https://storage.interacthub.com/posts/5/painting.jpg',   'Image', 'painting.jpg',   892, 2048, 1536, 0, 'Ready', 0, GETDATE()),
(4,  6,  'https://storage.interacthub.com/posts/6/coffee.jpg',     'Image', 'coffee.jpg',     156, 1080, 1080, 0, 'Ready', 0, GETDATE()),
(5,  10, 'https://storage.interacthub.com/posts/10/portrait.jpg',  'Image', 'portrait.jpg',   734, 1920, 2560, 0, 'Ready', 0, GETDATE());

SET IDENTITY_INSERT PostMedia OFF;
GO

-- ============================================
-- 9. POST HASHTAGS
-- ============================================
SET IDENTITY_INSERT PostHashtags ON;

INSERT INTO PostHashtags (post_hashtag_id, post_id, hashtag_id, delflg, reg_datetime)
VALUES
(1,  1, 1,  0, GETDATE()),  -- post 1 → travel
(2,  1, 9,  0, GETDATE()),  -- post 1 → vietnam
(3,  2, 8,  0, GETDATE()),  -- post 2 → coding
(4,  3, 2,  0, GETDATE()),  -- post 3 → food
(5,  3, 10, 0, GETDATE()),  -- post 3 → hochiminh
(6,  4, 8,  0, GETDATE()),  -- post 4 → coding
(7,  4, 3,  0, GETDATE()),  -- post 4 → technology
(8,  5, 7,  0, GETDATE()),  -- post 5 → art
(9,  5, 4,  0, GETDATE()),  -- post 5 → photography
(10, 6, 10, 0, GETDATE()),  -- post 6 → hochiminh
(11, 7, 8,  0, GETDATE()),  -- post 7 → coding
(12, 8, 2,  0, GETDATE()),  -- post 8 → food
(13, 8, 10, 0, GETDATE()),  -- post 8 → hochiminh
(14, 9, 8,  0, GETDATE()),  -- post 9 → coding
(15, 9, 3,  0, GETDATE()),  -- post 9 → technology
(16, 10,7,  0, GETDATE());  -- post 10→ art

SET IDENTITY_INSERT PostHashtags OFF;
GO

-- ============================================
-- 10. POST LIKES
-- ============================================
SET IDENTITY_INSERT PostLikes ON;

INSERT INTO PostLikes (like_id, post_id, user_id, reaction_type, delflg, reg_datetime)
VALUES
(1,  1, 3, 'Love',  0, GETDATE()),
(2,  1, 4, 'Like',  0, GETDATE()),
(3,  1, 5, 'Like',  0, GETDATE()),
(4,  2, 2, 'Haha',  0, GETDATE()),
(5,  2, 4, 'Like',  0, GETDATE()),
(6,  3, 2, 'Love',  0, GETDATE()),
(7,  3, 3, 'Like',  0, GETDATE()),
(8,  3, 5, 'Wow',   0, GETDATE()),
(9,  4, 2, 'Like',  0, GETDATE()),
(10, 4, 4, 'Like',  0, GETDATE()),
(11, 5, 2, 'Love',  0, GETDATE()),
(12, 5, 3, 'Wow',   0, GETDATE()),
(13, 5, 4, 'Love',  0, GETDATE()),
(14, 7, 2, 'Haha',  0, GETDATE()),
(15, 7, 4, 'Like',  0, GETDATE()),
(16, 9, 2, 'Haha',  0, GETDATE()),
(17, 9, 4, 'Haha',  0, GETDATE()),
(18, 10,2, 'Love',  0, GETDATE()),
(19, 10,3, 'Love',  0, GETDATE());

SET IDENTITY_INSERT PostLikes OFF;
GO

-- ============================================
-- 11. COMMENTS
-- ============================================
SET IDENTITY_INSERT Comments ON;

INSERT INTO Comments (comment_id, post_id, user_id, parent_comment_id, content, like_count, reply_count, is_edited, is_reported, delflg, reg_datetime)
VALUES
(1,  1, 3, NULL, N'Đà Lạt đẹp lắm! Mình cũng muốn đi quá 😍',              5, 1, 0, 0, 0, DATEADD(hour,-5,GETDATE())),
(2,  1, 4, NULL, N'Thời điểm này đi Đà Lạt là chuẩn nhất luôn!',            3, 0, 0, 0, 0, DATEADD(hour,-4,GETDATE())),
(3,  1, 5, 1,    N'Đúng rồi, mình cũng muốn đi lần nữa 🌿',                 1, 0, 0, 0, 0, DATEADD(hour,-3,GETDATE())),
(4,  2, 2, NULL, N'Giỏi vậy! Mình thì deploy xong là có bug ngay 😅',        4, 1, 0, 0, 0, DATEADD(hour,-4,GETDATE())),
(5,  2, 5, NULL, N'Chúc mừng! Cảm giác đó rất tuyệt 🎉',                    2, 0, 0, 0, 0, DATEADD(hour,-3,GETDATE())),
(6,  2, 6, 4,    N'Haha đúng rồi, mình cũng vậy 😂',                        1, 0, 0, 0, 0, DATEADD(hour,-2,GETDATE())),
(7,  3, 2, NULL, N'Bánh mì Hòa Mã ngon thật! Mình ăn hoài không chán',      6, 0, 0, 0, 0, DATEADD(hour,-3,GETDATE())),
(8,  3, 5, NULL, N'Bao nhiêu tiền một ổ vậy bạn?',                          2, 1, 0, 0, 0, DATEADD(hour,-2,GETDATE())),
(9,  3, 4, 8,    N'Khoảng 25k-35k tùy loại nhân nha!',                      1, 0, 0, 0, 0, DATEADD(hour,-1,GETDATE())),
(10, 5, 2, NULL, N'Tranh đẹp quá! Bạn vẽ bao lâu vậy?',                     8, 1, 0, 0, 0, DATEADD(hour,-1,GETDATE())),
(11, 5, 3, NULL, N'Tài năng thật sự! Tiếp tục phát huy nhé 🎨',             5, 0, 0, 0, 0, DATEADD(minute,-45,GETDATE())),
(12, 5, 6, 10,   N'Mình vẽ khoảng 2 tuần, tranh 60x80cm bạn ơi!',           3, 0, 0, 0, 0, DATEADD(minute,-30,GETDATE())),
(13, 7, 2, NULL, N'Quá đúng luôn! Bookmark lại để nhắc bản thân 😂',        7, 0, 0, 0, 0, DATEADD(minute,-30,GETDATE())),
(14, 9, 2, NULL, N'Dark mode gang! 🌙',                                      9, 0, 0, 0, 0, DATEADD(minute,-20,GETDATE())),
(15, 9, 4, NULL, N'Light mode mới là đúng, đọc tài liệu dễ hơn nhiều 📄',   4, 1, 0, 0, 0, DATEADD(minute,-15,GETDATE())),
(16, 9, 5, 15,   N'Mình không đồng ý! Dark mode bảo vệ mắt hơn 👁️',        3, 0, 0, 0, 0, DATEADD(minute,-10,GETDATE()));

SET IDENTITY_INSERT Comments OFF;
GO

-- ============================================
-- 12. COMMENT LIKES
-- ============================================
SET IDENTITY_INSERT CommentLikes ON;

INSERT INTO CommentLikes (like_id, comment_id, user_id, reaction_type, delflg, reg_datetime)
VALUES
(1,  1,  2, 'Like', 0, GETDATE()),
(2,  1,  5, 'Love', 0, GETDATE()),
(3,  4,  3, 'Haha', 0, GETDATE()),
(4,  7,  3, 'Like', 0, GETDATE()),
(5,  7,  6, 'Like', 0, GETDATE()),
(6,  10, 3, 'Like', 0, GETDATE()),
(7,  11, 2, 'Love', 0, GETDATE()),
(8,  13, 4, 'Haha', 0, GETDATE()),
(9,  14, 3, 'Like', 0, GETDATE()),
(10, 14, 6, 'Like', 0, GETDATE());

SET IDENTITY_INSERT CommentLikes OFF;
GO

-- ============================================
-- 13. STORIES
-- ============================================
SET IDENTITY_INSERT Stories ON;

INSERT INTO Stories (
    story_id, user_id,
    media_url, media_type, thumbnail_url,
    caption, bg_color, duration_sec, visibility,
    view_count, reaction_count,
    expire_datetime, is_expired, is_highlighted, highlight_name,
    delflg, reg_datetime
)
VALUES
(1, 2, 'https://storage.interacthub.com/stories/alice-dalat.jpg',    'Image', NULL,                                                          N'Đà Lạt buổi sáng 🌿', '#2D5016', 5, 'Friends', 45, 8, DATEADD(hour,18,GETDATE()), 0, 0, NULL,       0, DATEADD(hour,-6,GETDATE())),
(2, 3, 'https://storage.interacthub.com/stories/bob-code.jpg',       'Image', NULL,                                                          N'Coding time ☕',       '#1a1a2e', 5, 'Public',  32, 5, DATEADD(hour,18,GETDATE()), 0, 0, NULL,       0, DATEADD(hour,-4,GETDATE())),
(3, 4, 'https://storage.interacthub.com/stories/carol-food.mp4',     'Video', 'https://storage.interacthub.com/stories/carol-food-thumb.jpg', N'Ăn sáng nào 🍳',       NULL,      15, 'Public',  78, 12,DATEADD(hour,20,GETDATE()), 0, 0, NULL,       0, DATEADD(hour,-3,GETDATE())),
(4, 6, 'https://storage.interacthub.com/stories/eva-art.jpg',        'Image', NULL,                                                          N'Work in progress 🎨',  '#fff5e6', 5, 'Public',  120,25,DATEADD(hour,22,GETDATE()), 0, 1, N'Artwork', 0, DATEADD(hour,-2,GETDATE())),
(5, 2, 'https://storage.interacthub.com/stories/alice-coffee.jpg',   'Image', NULL,                                                          N'Cà phê sáng ☕',        '#8B4513', 5, 'Friends', 28, 4, DATEADD(hour,23,GETDATE()), 0, 0, NULL,       0, DATEADD(hour,-1,GETDATE()));

SET IDENTITY_INSERT Stories OFF;
GO

-- ============================================
-- 14. STORY VIEWS
-- ============================================
SET IDENTITY_INSERT StoryViews ON;

INSERT INTO StoryViews (view_id, story_id, viewer_id, view_duration, delflg, reg_datetime)
VALUES
(1,  1, 3, 5,  0, GETDATE()),
(2,  1, 4, 5,  0, GETDATE()),
(3,  1, 5, 3,  0, GETDATE()),
(4,  2, 2, 5,  0, GETDATE()),
(5,  2, 4, 5,  0, GETDATE()),
(6,  3, 2, 12, 0, GETDATE()),
(7,  3, 3, 15, 0, GETDATE()),
(8,  3, 5, 10, 0, GETDATE()),
(9,  4, 2, 5,  0, GETDATE()),
(10, 4, 3, 5,  0, GETDATE()),
(11, 4, 5, 5,  0, GETDATE()),
(12, 5, 3, 4,  0, GETDATE()),
(13, 5, 4, 5,  0, GETDATE());

SET IDENTITY_INSERT StoryViews OFF;
GO

-- ============================================
-- 15. STORY REACTIONS
-- ============================================
SET IDENTITY_INSERT StoryReactions ON;

INSERT INTO StoryReactions (reaction_id, story_id, user_id, reaction_type, delflg, reg_datetime)
VALUES
(1, 1, 3, 'Love', 0, GETDATE()),
(2, 1, 4, 'Like', 0, GETDATE()),
(3, 2, 2, 'Like', 0, GETDATE()),
(4, 3, 2, 'Love', 0, GETDATE()),
(5, 3, 3, 'Wow',  0, GETDATE()),
(6, 4, 2, 'Love', 0, GETDATE()),
(7, 4, 3, 'Wow',  0, GETDATE()),
(8, 4, 5, 'Love', 0, GETDATE()),
(9, 5, 4, 'Like', 0, GETDATE());

SET IDENTITY_INSERT StoryReactions OFF;
GO

-- ============================================
-- 16. NOTIFICATIONS
-- ============================================
SET IDENTITY_INSERT Notifications ON;

INSERT INTO Notifications (
    notification_id, recipient_id, sender_id,
    notification_type, reference_id, reference_type,
    message, is_read, redirect_url,
    delflg, reg_datetime
)
VALUES
(1,  2, 3, 'post_like',       1,  'Post',       N'Bob đã thích bài viết của bạn',                0, '/posts/1',       0, DATEADD(hour,-5,GETDATE())),
(2,  2, 4, 'post_comment',    1,  'Post',       N'Carol đã bình luận về bài viết của bạn',       0, '/posts/1',       0, DATEADD(hour,-4,GETDATE())),
(3,  3, 2, 'friend_request',  1,  'Friendship', N'Alice đã gửi lời mời kết bạn',                1, '/friends',       0, DATEADD(hour,-3,GETDATE())),
(4,  4, 2, 'post_like',       3,  'Post',       N'Alice đã thích bài viết của bạn',              1, '/posts/3',       0, DATEADD(hour,-3,GETDATE())),
(5,  5, 3, 'friend_accepted', 3,  'Friendship', N'Bob đã chấp nhận lời mời kết bạn của bạn',    1, '/friends',       0, DATEADD(hour,-2,GETDATE())),
(6,  6, 4, 'friend_accepted', 4,  'Friendship', N'Carol đã chấp nhận lời mời kết bạn của bạn',  1, '/friends',       0, DATEADD(hour,-2,GETDATE())),
(7,  6, 2, 'post_like',       5,  'Post',       N'Alice đã thích bài viết của bạn',              0, '/posts/5',       0, DATEADD(hour,-1,GETDATE())),
(8,  6, 3, 'post_comment',    5,  'Post',       N'Bob đã bình luận về bài viết của bạn',         0, '/posts/5',       0, DATEADD(minute,-45,GETDATE())),
(9,  3, 2, 'post_like',       7,  'Post',       N'Alice đã thích bài viết của bạn',              0, '/posts/7',       0, DATEADD(minute,-30,GETDATE())),
(10, 5, 2, 'post_like',       9,  'Post',       N'Alice đã thích bài viết của bạn',              0, '/posts/9',       0, DATEADD(minute,-20,GETDATE())),
(11, 6, 2, 'story_view',      4,  'Story',      N'Alice đã xem story của bạn',                   0, '/stories/4',     0, DATEADD(minute,-10,GETDATE())),
(12, 2, 5, 'post_mention',    4,  'Post',       N'David đã nhắc đến bạn trong bài viết',         0, '/posts/4',       0, DATEADD(minute,-5, GETDATE()));

SET IDENTITY_INSERT Notifications OFF;
GO

PRINT 'Seed data completed successfully!';
PRINT 'Users: alice(id=2), bob(id=3), carol(id=4), david(id=5), eva(id=6)';
PRINT 'Password for all sample users: Test@1234';
GOch hàng đầu tiên! Cảm ơn mọi người đã giới thiệu 🙏 #art',         'Image', 'Public',  NULL,              N'biết ơn',    102,27, 18,0, 0, 0, 0, 1, 0, DATEADD(hour,-1,GETDATE()));

SET IDENTITY_INSERT Posts OFF;
GO

-- ============================================
-- 8. POST MEDIA
-- ============================================
SET IDENTITY_INSERT PostMedia ON;

INSERT INTO PostMedia (media_id, post_id, media_url, media_type, file_name, file_size_kb, width_px, height_px, sort_order, processing_status, delflg, reg_datetime)
VALUES
(1,  3,  'https://storage.interacthub.com/posts/3/banhmi-1.jpg',   'Image', 'banhmi-1.jpg',   245, 1080, 1080, 0, 'Ready', 0, GETDATE()),
(2,  3,  'https://storage.interacthub.com/posts/3/banhmi-2.jpg',   'Image', 'banhmi-2.jpg',   312, 1080, 720,  1, 'Ready', 0, GETDATE()),
(3,  5,  'https://storage.interacthub.com/posts/5/painting.jpg',   'Image', 'painting.jpg',   892, 2048, 1536, 0, 'Ready', 0, GETDATE()),
(4,  6,  'https://storage.interacthub.com/posts/6/coffee.jpg',     'Image', 'coffee.jpg',     156, 1080, 1080, 0, 'Ready', 0, GETDATE()),
(5,  10, 'https://storage.interacthub.com/posts/10/portrait.jpg',  'Image', 'portrait.jpg',   734, 1920, 2560, 0, 'Ready', 0, GETDATE());

SET IDENTITY_INSERT PostMedia OFF;
GO

-- ============================================
-- 9. POST HASHTAGS
-- ============================================
SET IDENTITY_INSERT PostHashtags ON;

INSERT INTO PostHashtags (post_hashtag_id, post_id, hashtag_id, delflg, reg_datetime)
VALUES
(1,  1, 1,  0, GETDATE()),  -- post 1 → travel
(2,  1, 9,  0, GETDATE()),  -- post 1 → vietnam
(3,  2, 8,  0, GETDATE()),  -- post 2 → coding
(4,  3, 2,  0, GETDATE()),  -- post 3 → food
(5,  3, 10, 0, GETDATE()),  -- post 3 → hochiminh
(6,  4, 8,  0, GETDATE()),  -- post 4 → coding
(7,  4, 3,  0, GETDATE()),  -- post 4 → technology
(8,  5, 7,  0, GETDATE()),  -- post 5 → art
(9,  5, 4,  0, GETDATE()),  -- post 5 → photography
(10, 6, 10, 0, GETDATE()),  -- post 6 → hochiminh
(11, 7, 8,  0, GETDATE()),  -- post 7 → coding
(12, 8, 2,  0, GETDATE()),  -- post 8 → food
(13, 8, 10, 0, GETDATE()),  -- post 8 → hochiminh
(14, 9, 8,  0, GETDATE()),  -- post 9 → coding
(15, 9, 3,  0, GETDATE()),  -- post 9 → technology
(16, 10,7,  0, GETDATE());  -- post 10→ art

SET IDENTITY_INSERT PostHashtags OFF;
GO

-- ============================================
-- 10. POST LIKES
-- ============================================
SET IDENTITY_INSERT PostLikes ON;

INSERT INTO PostLikes (like_id, post_id, user_id, reaction_type, delflg, reg_datetime)
VALUES
(1,  1, 3, 'Love',  0, GETDATE()),
(2,  1, 4, 'Like',  0, GETDATE()),
(3,  1, 5, 'Like',  0, GETDATE()),
(4,  2, 2, 'Haha',  0, GETDATE()),
(5,  2, 4, 'Like',  0, GETDATE()),
(6,  3, 2, 'Love',  0, GETDATE()),
(7,  3, 3, 'Like',  0, GETDATE()),
(8,  3, 5, 'Wow',   0, GETDATE()),
(9,  4, 2, 'Like',  0, GETDATE()),
(10, 4, 4, 'Like',  0, GETDATE()),
(11, 5, 2, 'Love',  0, GETDATE()),
(12, 5, 3, 'Wow',   0, GETDATE()),
(13, 5, 4, 'Love',  0, GETDATE()),
(14, 7, 2, 'Haha',  0, GETDATE()),
(15, 7, 4, 'Like',  0, GETDATE()),
(16, 9, 2, 'Haha',  0, GETDATE()),
(17, 9, 4, 'Haha',  0, GETDATE()),
(18, 10,2, 'Love',  0, GETDATE()),
(19, 10,3, 'Love',  0, GETDATE());

SET IDENTITY_INSERT PostLikes OFF;
GO

-- ============================================
-- 11. COMMENTS
-- ============================================
SET IDENTITY_INSERT Comments ON;

INSERT INTO Comments (comment_id, post_id, user_id, parent_comment_id, content, like_count, reply_count, is_edited, is_reported, delflg, reg_datetime)
VALUES
(1,  1, 3, NULL, N'Đà Lạt đẹp lắm! Mình cũng muốn đi quá 😍',              5, 1, 0, 0, 0, DATEADD(hour,-5,GETDATE())),
(2,  1, 4, NULL, N'Thời điểm này đi Đà Lạt là chuẩn nhất luôn!',            3, 0, 0, 0, 0, DATEADD(hour,-4,GETDATE())),
(3,  1, 5, 1,    N'Đúng rồi, mình cũng muốn đi lần nữa 🌿',                 1, 0, 0, 0, 0, DATEADD(hour,-3,GETDATE())),
(4,  2, 2, NULL, N'Giỏi vậy! Mình thì deploy xong là có bug ngay 😅',        4, 1, 0, 0, 0, DATEADD(hour,-4,GETDATE())),
(5,  2, 5, NULL, N'Chúc mừng! Cảm giác đó rất tuyệt 🎉',                    2, 0, 0, 0, 0, DATEADD(hour,-3,GETDATE())),
(6,  2, 6, 4,    N'Haha đúng rồi, mình cũng vậy 😂',                        1, 0, 0, 0, 0, DATEADD(hour,-2,GETDATE())),
(7,  3, 2, NULL, N'Bánh mì Hòa Mã ngon thật! Mình ăn hoài không chán',      6, 0, 0, 0, 0, DATEADD(hour,-3,GETDATE())),
(8,  3, 5, NULL, N'Bao nhiêu tiền một ổ vậy bạn?',                          2, 1, 0, 0, 0, DATEADD(hour,-2,GETDATE())),
(9,  3, 4, 8,    N'Khoảng 25k-35k tùy loại nhân nha!',                      1, 0, 0, 0, 0, DATEADD(hour,-1,GETDATE())),
(10, 5, 2, NULL, N'Tranh đẹp quá! Bạn vẽ bao lâu vậy?',                     8, 1, 0, 0, 0, DATEADD(hour,-1,GETDATE())),
(11, 5, 3, NULL, N'Tài năng thật sự! Tiếp tục phát huy nhé 🎨',             5, 0, 0, 0, 0, DATEADD(minute,-45,GETDATE())),
(12, 5, 6, 10,   N'Mình vẽ khoảng 2 tuần, tranh 60x80cm bạn ơi!',           3, 0, 0, 0, 0, DATEADD(minute,-30,GETDATE())),
(13, 7, 2, NULL, N'Quá đúng luôn! Bookmark lại để nhắc bản thân 😂',        7, 0, 0, 0, 0, DATEADD(minute,-30,GETDATE())),
(14, 9, 2, NULL, N'Dark mode gang! 🌙',                                      9, 0, 0, 0, 0, DATEADD(minute,-20,GETDATE())),
(15, 9, 4, NULL, N'Light mode mới là đúng, đọc tài liệu dễ hơn nhiều 📄',   4, 1, 0, 0, 0, DATEADD(minute,-15,GETDATE())),
(16, 9, 5, 15,   N'Mình không đồng ý! Dark mode bảo vệ mắt hơn 👁️',        3, 0, 0, 0, 0, DATEADD(minute,-10,GETDATE()));

SET IDENTITY_INSERT Comments OFF;
GO

-- ============================================
-- 12. COMMENT LIKES
-- ============================================
SET IDENTITY_INSERT CommentLikes ON;

INSERT INTO CommentLikes (like_id, comment_id, user_id, reaction_type, delflg, reg_datetime)
VALUES
(1,  1,  2, 'Like', 0, GETDATE()),
(2,  1,  5, 'Love', 0, GETDATE()),
(3,  4,  3, 'Haha', 0, GETDATE()),
(4,  7,  3, 'Like', 0, GETDATE()),
(5,  7,  6, 'Like', 0, GETDATE()),
(6,  10, 3, 'Like', 0, GETDATE()),
(7,  11, 2, 'Love', 0, GETDATE()),
(8,  13, 4, 'Haha', 0, GETDATE()),
(9,  14, 3, 'Like', 0, GETDATE()),
(10, 14, 6, 'Like', 0, GETDATE());

SET IDENTITY_INSERT CommentLikes OFF;
GO

-- ============================================
-- 13. STORIES
-- ============================================
SET IDENTITY_INSERT Stories ON;

INSERT INTO Stories (
    story_id, user_id,
    media_url, media_type, thumbnail_url,
    caption, bg_color, duration_sec, visibility,
    view_count, reaction_count,
    expire_datetime, is_expired, is_highlighted, highlight_name,
    delflg, reg_datetime
)
VALUES
(1, 2, 'https://storage.interacthub.com/stories/alice-dalat.jpg',    'Image', NULL,                                                          N'Đà Lạt buổi sáng 🌿', '#2D5016', 5, 'Friends', 45, 8, DATEADD(hour,18,GETDATE()), 0, 0, NULL,       0, DATEADD(hour,-6,GETDATE())),
(2, 3, 'https://storage.interacthub.com/stories/bob-code.jpg',       'Image', NULL,                                                          N'Coding time ☕',       '#1a1a2e', 5, 'Public',  32, 5, DATEADD(hour,18,GETDATE()), 0, 0, NULL,       0, DATEADD(hour,-4,GETDATE())),
(3, 4, 'https://storage.interacthub.com/stories/carol-food.mp4',     'Video', 'https://storage.interacthub.com/stories/carol-food-thumb.jpg', N'Ăn sáng nào 🍳',       NULL,      15, 'Public',  78, 12,DATEADD(hour,20,GETDATE()), 0, 0, NULL,       0, DATEADD(hour,-3,GETDATE())),
(4, 6, 'https://storage.interacthub.com/stories/eva-art.jpg',        'Image', NULL,                                                          N'Work in progress 🎨',  '#fff5e6', 5, 'Public',  120,25,DATEADD(hour,22,GETDATE()), 0, 1, N'Artwork', 0, DATEADD(hour,-2,GETDATE())),
(5, 2, 'https://storage.interacthub.com/stories/alice-coffee.jpg',   'Image', NULL,                                                          N'Cà phê sáng ☕',        '#8B4513', 5, 'Friends', 28, 4, DATEADD(hour,23,GETDATE()), 0, 0, NULL,       0, DATEADD(hour,-1,GETDATE()));

SET IDENTITY_INSERT Stories OFF;
GO

-- ============================================
-- 14. STORY VIEWS
-- ============================================
SET IDENTITY_INSERT StoryViews ON;

INSERT INTO StoryViews (view_id, story_id, viewer_id, view_duration, delflg, reg_datetime)
VALUES
(1,  1, 3, 5,  0, GETDATE()),
(2,  1, 4, 5,  0, GETDATE()),
(3,  1, 5, 3,  0, GETDATE()),
(4,  2, 2, 5,  0, GETDATE()),
(5,  2, 4, 5,  0, GETDATE()),
(6,  3, 2, 12, 0, GETDATE()),
(7,  3, 3, 15, 0, GETDATE()),
(8,  3, 5, 10, 0, GETDATE()),
(9,  4, 2, 5,  0, GETDATE()),
(10, 4, 3, 5,  0, GETDATE()),
(11, 4, 5, 5,  0, GETDATE()),
(12, 5, 3, 4,  0, GETDATE()),
(13, 5, 4, 5,  0, GETDATE());

SET IDENTITY_INSERT StoryViews OFF;
GO

-- ============================================
-- 15. STORY REACTIONS
-- ============================================
SET IDENTITY_INSERT StoryReactions ON;

INSERT INTO StoryReactions (reaction_id, story_id, user_id, reaction_type, delflg, reg_datetime)
VALUES
(1, 1, 3, 'Love', 0, GETDATE()),
(2, 1, 4, 'Like', 0, GETDATE()),
(3, 2, 2, 'Like', 0, GETDATE()),
(4, 3, 2, 'Love', 0, GETDATE()),
(5, 3, 3, 'Wow',  0, GETDATE()),
(6, 4, 2, 'Love', 0, GETDATE()),
(7, 4, 3, 'Wow',  0, GETDATE()),
(8, 4, 5, 'Love', 0, GETDATE()),
(9, 5, 4, 'Like', 0, GETDATE());

SET IDENTITY_INSERT StoryReactions OFF;
GO

-- ============================================
-- 16. NOTIFICATIONS
-- ============================================
SET IDENTITY_INSERT Notifications ON;

INSERT INTO Notifications (
    notification_id, recipient_id, sender_id,
    notification_type, reference_id, reference_type,
    message, is_read, redirect_url,
    delflg, reg_datetime
)
VALUES
(1,  2, 3, 'post_like',       1,  'Post',       N'Bob đã thích bài viết của bạn',                0, '/posts/1',       0, DATEADD(hour,-5,GETDATE())),
(2,  2, 4, 'post_comment',    1,  'Post',       N'Carol đã bình luận về bài viết của bạn',       0, '/posts/1',       0, DATEADD(hour,-4,GETDATE())),
(3,  3, 2, 'friend_request',  1,  'Friendship', N'Alice đã gửi lời mời kết bạn',                1, '/friends',       0, DATEADD(hour,-3,GETDATE())),
(4,  4, 2, 'post_like',       3,  'Post',       N'Alice đã thích bài viết của bạn',              1, '/posts/3',       0, DATEADD(hour,-3,GETDATE())),
(5,  5, 3, 'friend_accepted', 3,  'Friendship', N'Bob đã chấp nhận lời mời kết bạn của bạn',    1, '/friends',       0, DATEADD(hour,-2,GETDATE())),
(6,  6, 4, 'friend_accepted', 4,  'Friendship', N'Carol đã chấp nhận lời mời kết bạn của bạn',  1, '/friends',       0, DATEADD(hour,-2,GETDATE())),
(7,  6, 2, 'post_like',       5,  'Post',       N'Alice đã thích bài viết của bạn',              0, '/posts/5',       0, DATEADD(hour,-1,GETDATE())),
(8,  6, 3, 'post_comment',    5,  'Post',       N'Bob đã bình luận về bài viết của bạn',         0, '/posts/5',       0, DATEADD(minute,-45,GETDATE())),
(9,  3, 2, 'post_like',       7,  'Post',       N'Alice đã thích bài viết của bạn',              0, '/posts/7',       0, DATEADD(minute,-30,GETDATE())),
(10, 5, 2, 'post_like',       9,  'Post',       N'Alice đã thích bài viết của bạn',              0, '/posts/9',       0, DATEADD(minute,-20,GETDATE())),
(11, 6, 2, 'story_view',      4,  'Story',      N'Alice đã xem story của bạn',                   0, '/stories/4',     0, DATEADD(minute,-10,GETDATE())),
(12, 2, 5, 'post_mention',    4,  'Post',       N'David đã nhắc đến bạn trong bài viết',         0, '/posts/4',       0, DATEADD(minute,-5, GETDATE()));

SET IDENTITY_INSERT Notifications OFF;
GO

PRINT 'Seed data completed successfully!';
PRINT 'Users: alice(id=2), bob(id=3), carol(id=4), david(id=5), eva(id=6)';
PRINT 'Password for all sample users: Test@1234';
GO