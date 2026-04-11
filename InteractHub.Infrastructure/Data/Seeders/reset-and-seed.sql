-- PostgreSQL reset + seed script for InteractHub
-- Run from project root:
-- docker exec -i interacthub-postgres psql -U postgres -d interacthubdb -f InteractHub.Infrastructure/Data/Seeders/reset-and-seed.sql

BEGIN;

TRUNCATE TABLE
    "CommentMentions",
    "CommentLikes",
    "Comments",
    "StoryReactions",
    "StoryViews",
    "Stories",
    "Notifications",
    "PostReports",
    "PostLikes",
    "PostMentions",
    "PostHashtags",
    "PostMedia",
    "PostShares",
    "Posts",
    "Friendships",
    "UserProfiles",
    "Hashtags",
    "MusicTracks",
    "AspNetUserRoles",
    "AspNetUsers",
    "AspNetRoleClaims",
    "AspNetRoles",
    "AspNetUserClaims",
    "AspNetUserLogins",
    "AspNetUserTokens"
RESTART IDENTITY CASCADE;

INSERT INTO "AspNetRoles" ("Id", description, delflg, reg_datetime, "Name", "NormalizedName", "ConcurrencyStamp")
VALUES
(1, 'System Administrator', FALSE, CURRENT_TIMESTAMP, 'Admin', 'ADMIN', 'seed-role-admin'),
(2, 'Content Moderator', FALSE, CURRENT_TIMESTAMP, 'Moderator', 'MODERATOR', 'seed-role-moderator'),
(3, 'General User', FALSE, CURRENT_TIMESTAMP, 'User', 'USER', 'seed-role-user');

INSERT INTO "AspNetUsers" (
    "Id", "UserName", "NormalizedUserName",
    "Email", "NormalizedEmail", "EmailConfirmed",
    "PasswordHash", "SecurityStamp", "ConcurrencyStamp",
    "PhoneNumber", "PhoneNumberConfirmed",
    "TwoFactorEnabled", "LockoutEnabled", "AccessFailedCount",
    full_name, gender, date_of_birth,
    avatar_url, cover_photo_url, bio,
    website_url, location,
    is_active, is_private_account,
    delflg, reg_datetime
)
VALUES
(
    2, 'alice_nguyen', 'ALICE_NGUYEN',
    'alice@gmail.com', 'ALICE@GMAIL.COM', TRUE,
    'AQAAAAIAAYagAAAAELk4p5pJz8eMWopqM7TA+Wr2aBpHQ3G6iHEWvU2cBFEFcqJkbKqtJlW8Rh5Ue9BWGA==',
    'seed-security-2', 'seed-concurrency-2',
    '0901234567', FALSE,
    FALSE, TRUE, 0,
    'Nguyễn Thị Alice', 'Female', DATE '1999-05-15',
    'https://storage.interacthub.com/avatars/alice.jpg',
    'https://storage.interacthub.com/covers/alice-cover.jpg',
    'Yêu thích du lịch và nhiếp ảnh',
    'https://alice.blog', 'Hồ Chí Minh',
    TRUE, FALSE,
    FALSE, CURRENT_TIMESTAMP
),
(
    3, 'bob_tran', 'BOB_TRAN',
    'bob@gmail.com', 'BOB@GMAIL.COM', TRUE,
    'AQAAAAIAAYagAAAAELk4p5pJz8eMWopqM7TA+Wr2aBpHQ3G6iHEWvU2cBFEFcqJkbKqtJlW8Rh5Ue9BWGA==',
    'seed-security-3', 'seed-concurrency-3',
    '0912345678', FALSE,
    FALSE, TRUE, 0,
    'Trần Văn Bob', 'Male', DATE '2000-08-20',
    'https://storage.interacthub.com/avatars/bob.jpg',
    'https://storage.interacthub.com/covers/bob-cover.jpg',
    'Lập trình viên | Coffee addict',
    NULL, 'Hà Nội',
    TRUE, FALSE,
    FALSE, CURRENT_TIMESTAMP
),
(
    4, 'carol_le', 'CAROL_LE',
    'carol@gmail.com', 'CAROL@GMAIL.COM', TRUE,
    'AQAAAAIAAYagAAAAELk4p5pJz8eMWopqM7TA+Wr2aBpHQ3G6iHEWvU2cBFEFcqJkbKqtJlW8Rh5Ue9BWGA==',
    'seed-security-4', 'seed-concurrency-4',
    '0923456789', FALSE,
    FALSE, TRUE, 0,
    'Lê Thị Carol', 'Female', DATE '2001-03-10',
    'https://storage.interacthub.com/avatars/carol.jpg',
    'https://storage.interacthub.com/covers/carol-cover.jpg',
    'Foodie | Review ẩm thực Sài Gòn',
    'https://carol.food', 'Hồ Chí Minh',
    TRUE, FALSE,
    FALSE, CURRENT_TIMESTAMP
),
(
    5, 'david_pham', 'DAVID_PHAM',
    'david@gmail.com', 'DAVID@GMAIL.COM', TRUE,
    'AQAAAAIAAYagAAAAELk4p5pJz8eMWopqM7TA+Wr2aBpHQ3G6iHEWvU2cBFEFcqJkbKqtJlW8Rh5Ue9BWGA==',
    'seed-security-5', 'seed-concurrency-5',
    '0934567890', FALSE,
    FALSE, TRUE, 0,
    'Phạm Văn David', 'Male', DATE '1998-12-25',
    'https://storage.interacthub.com/avatars/david.jpg',
    NULL,
    'Tech enthusiast | Open source contributor',
    'https://github.com/david', 'Đà Nẵng',
    TRUE, FALSE,
    FALSE, CURRENT_TIMESTAMP
),
(
    6, 'eva_hoang', 'EVA_HOANG',
    'eva@gmail.com', 'EVA@GMAIL.COM', TRUE,
    'AQAAAAIAAYagAAAAELk4p5pJz8eMWopqM7TA+Wr2aBpHQ3G6iHEWvU2cBFEFcqJkbKqtJlW8Rh5Ue9BWGA==',
    'seed-security-6', 'seed-concurrency-6',
    '0945678901', FALSE,
    FALSE, TRUE, 0,
    'Hoàng Thị Eva', 'Female', DATE '2002-07-04',
    'https://storage.interacthub.com/avatars/eva.jpg',
    'https://storage.interacthub.com/covers/eva-cover.jpg',
    'Nghệ sĩ tự do | Vẽ tranh & thiết kế',
    NULL, 'Hồ Chí Minh',
    TRUE, FALSE,
    FALSE, CURRENT_TIMESTAMP
);

INSERT INTO "AspNetUserRoles" ("UserId", "RoleId")
VALUES
(2, 3),
(3, 3),
(4, 3),
(5, 3),
(6, 3);

INSERT INTO "Hashtags" (hashtag_id, tag_name, post_count, trending_score, is_trending, delflg, reg_datetime)
VALUES
(1, 'travel', 15, 95.50, TRUE, FALSE, CURRENT_TIMESTAMP),
(2, 'food', 12, 88.00, TRUE, FALSE, CURRENT_TIMESTAMP),
(3, 'coding', 10, 82.50, TRUE, FALSE, CURRENT_TIMESTAMP),
(4, 'art', 8, 75.00, FALSE, FALSE, CURRENT_TIMESTAMP),
(5, 'vietnam', 11, 85.00, TRUE, FALSE, CURRENT_TIMESTAMP);

INSERT INTO "MusicTracks" (music_id, title, artist, audio_url, thumbnail_url, duration_sec, is_licensed, source, delflg, reg_datetime)
VALUES
(1, 'Chill Vibes', 'Lo-Fi Studio', 'https://storage.interacthub.com/music/chill-vibes.mp3', 'https://storage.interacthub.com/music/thumbs/chill-vibes.jpg', 180, TRUE, 'Internal', FALSE, CURRENT_TIMESTAMP),
(2, 'Morning Coffee', 'Jazz Quartet', 'https://storage.interacthub.com/music/morning-coffee.mp3', 'https://storage.interacthub.com/music/thumbs/morning-coffee.jpg', 195, TRUE, 'Internal', FALSE, CURRENT_TIMESTAMP),
(3, 'Epic Cinematic', 'Orchestra Pro', 'https://storage.interacthub.com/music/epic-cinematic.mp3', 'https://storage.interacthub.com/music/thumbs/epic-cinematic.jpg', 300, TRUE, 'Internal', FALSE, CURRENT_TIMESTAMP);

INSERT INTO "UserProfiles" (
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
(1, 2, 'Độc thân', 'FPT Software', 'Designer', 'Đại học Sài Gòn', 'Hà Nội', 'Hồ Chí Minh', NULL, 'alice_ng', NULL, 120, 85, 8, 45, 'Public', 'Public', 'Public', TRUE, TRUE, FALSE, CURRENT_TIMESTAMP),
(2, 3, 'Độc thân', 'VNG Corporation', 'Backend Developer', 'HCMUT', 'Hà Nội', 'Hà Nội', NULL, 'bob_tran', 'bobtran', 98, 72, 5, 38, 'Public', 'Friends', 'Public', TRUE, TRUE, FALSE, CURRENT_TIMESTAMP),
(3, 4, 'Hẹn hò', 'Grab Vietnam', 'Content Creator', 'Đại học Sài Gòn', 'Cần Thơ', 'Hồ Chí Minh', NULL, 'carol_le', NULL, 215, 130, 12, 67, 'Public', 'Public', 'Public', TRUE, TRUE, FALSE, CURRENT_TIMESTAMP),
(4, 5, 'Độc thân', 'Freelance', 'Full-stack Dev', 'ĐH Đà Nẵng', 'Đà Nẵng', 'Đà Nẵng', NULL, 'david_ph', 'davidpham', 76, 54, 6, 29, 'Public', 'Public', 'Friends', TRUE, FALSE, FALSE, CURRENT_TIMESTAMP),
(5, 6, 'Độc thân', 'Studio Freelance', 'Illustrator', 'ĐH Mỹ thuật HCM', 'Huế', 'Hồ Chí Minh', NULL, 'eva_hoang', NULL, 340, 210, 15, 89, 'Public', 'Public', 'Public', TRUE, TRUE, FALSE, CURRENT_TIMESTAMP);

INSERT INTO "Friendships" (friendship_id, requester_id, addressee_id, status, action_user_id, is_blocked, delflg, reg_datetime)
VALUES
(1, 2, 3, 'Accepted', 3, FALSE, FALSE, CURRENT_TIMESTAMP - INTERVAL '6 days'),
(2, 2, 4, 'Accepted', 4, FALSE, FALSE, CURRENT_TIMESTAMP - INTERVAL '5 days'),
(3, 3, 5, 'Accepted', 5, FALSE, FALSE, CURRENT_TIMESTAMP - INTERVAL '4 days'),
(4, 4, 6, 'Accepted', 6, FALSE, FALSE, CURRENT_TIMESTAMP - INTERVAL '3 days'),
(5, 2, 5, 'Pending', 2, FALSE, FALSE, CURRENT_TIMESTAMP - INTERVAL '1 day');

INSERT INTO "Posts" (
    post_id, user_id, content, post_type, visibility,
    location_name, feeling,
    like_count, comment_count, share_count,
    is_edited, is_pinned, is_reported, report_count, allow_comment,
    delflg, reg_datetime
)
VALUES
(1, 2, 'Vừa đến Đà Lạt, thời tiết tuyệt vời quá! #travel #vietnam', 'Text', 'Public', 'Đà Lạt', 'hạnh phúc', 24, 5, 3, FALSE, FALSE, FALSE, 0, TRUE, FALSE, CURRENT_TIMESTAMP - INTERVAL '6 days'),
(2, 3, 'Vừa deploy xong feature mới lên production mà không có bug nào 🎉 #coding', 'Text', 'Public', NULL, 'tự hào', 18, 7, 2, FALSE, FALSE, FALSE, 0, TRUE, FALSE, CURRENT_TIMESTAMP - INTERVAL '5 days'),
(3, 4, 'Review bánh mì Hòa Mã - địa chỉ quá quen thuộc với dân Sài Gòn 🥖 #food', 'Image', 'Public', 'Bánh mì Hòa Mã', 'thích thú', 45, 12, 8, FALSE, FALSE, FALSE, 0, TRUE, FALSE, CURRENT_TIMESTAMP - INTERVAL '4 days'),
(4, 5, 'Mình vừa contribute vào một open-source project khá thú vị. Ai quan tâm đến Rust không? #coding #vietnam', 'Text', 'Public', NULL, NULL, 31, 9, 5, FALSE, FALSE, FALSE, 0, TRUE, FALSE, CURRENT_TIMESTAMP - INTERVAL '3 days'),
(5, 6, 'Xong bức tranh mới sau 2 tuần! Cảm ơn mọi người đã ủng hộ mình 🎨 #art', 'Image', 'Public', NULL, 'tự hào', 89, 23, 14, FALSE, TRUE, FALSE, 0, TRUE, FALSE, CURRENT_TIMESTAMP - INTERVAL '2 days');

INSERT INTO "PostMedia" (media_id, post_id, media_url, media_type, file_name, file_size_kb, width_px, height_px, sort_order, processing_status, delflg, reg_datetime)
VALUES
(1, 3, 'https://storage.interacthub.com/posts/3/banhmi-1.jpg', 'Image', 'banhmi-1.jpg', 245, 1080, 1080, 0, 'Ready', FALSE, CURRENT_TIMESTAMP - INTERVAL '4 days'),
(2, 3, 'https://storage.interacthub.com/posts/3/banhmi-2.jpg', 'Image', 'banhmi-2.jpg', 312, 1080, 720, 1, 'Ready', FALSE, CURRENT_TIMESTAMP - INTERVAL '4 days'),
(3, 5, 'https://storage.interacthub.com/posts/5/painting.jpg', 'Image', 'painting.jpg', 892, 2048, 1536, 0, 'Ready', FALSE, CURRENT_TIMESTAMP - INTERVAL '2 days');

INSERT INTO "PostHashtags" (post_hashtag_id, post_id, hashtag_id, delflg, reg_datetime)
VALUES
(1, 1, 1, FALSE, CURRENT_TIMESTAMP - INTERVAL '6 days'),
(2, 1, 5, FALSE, CURRENT_TIMESTAMP - INTERVAL '6 days'),
(3, 2, 3, FALSE, CURRENT_TIMESTAMP - INTERVAL '5 days'),
(4, 3, 2, FALSE, CURRENT_TIMESTAMP - INTERVAL '4 days'),
(5, 4, 3, FALSE, CURRENT_TIMESTAMP - INTERVAL '3 days'),
(6, 5, 4, FALSE, CURRENT_TIMESTAMP - INTERVAL '2 days');

INSERT INTO "PostLikes" (like_id, post_id, user_id, reaction_type, delflg, reg_datetime)
VALUES
(1, 1, 3, 'Love', FALSE, CURRENT_TIMESTAMP - INTERVAL '6 days'),
(2, 1, 4, 'Like', FALSE, CURRENT_TIMESTAMP - INTERVAL '6 days'),
(3, 2, 2, 'Haha', FALSE, CURRENT_TIMESTAMP - INTERVAL '5 days'),
(4, 3, 2, 'Love', FALSE, CURRENT_TIMESTAMP - INTERVAL '4 days'),
(5, 5, 2, 'Love', FALSE, CURRENT_TIMESTAMP - INTERVAL '2 days');

INSERT INTO "Comments" (comment_id, post_id, user_id, parent_comment_id, content, like_count, reply_count, is_edited, is_reported, delflg, reg_datetime)
VALUES
(1, 1, 3, NULL, 'Đà Lạt đẹp lắm! Mình cũng muốn đi quá 😍', 5, 1, FALSE, FALSE, FALSE, CURRENT_TIMESTAMP - INTERVAL '5 days'),
(2, 1, 4, NULL, 'Thời điểm này đi Đà Lạt là chuẩn nhất luôn!', 3, 0, FALSE, FALSE, FALSE, CURRENT_TIMESTAMP - INTERVAL '4 days'),
(3, 2, 2, NULL, 'Giỏi vậy! Mình thì deploy xong là có bug ngay 😅', 4, 1, FALSE, FALSE, FALSE, CURRENT_TIMESTAMP - INTERVAL '4 days'),
(4, 3, 2, NULL, 'Bánh mì Hòa Mã ngon thật!', 6, 0, FALSE, FALSE, FALSE, CURRENT_TIMESTAMP - INTERVAL '3 days'),
(5, 5, 2, NULL, 'Tranh đẹp quá! Bạn vẽ bao lâu vậy?', 8, 1, FALSE, FALSE, FALSE, CURRENT_TIMESTAMP - INTERVAL '1 day'),
(6, 5, 6, 5, 'Mình vẽ khoảng 2 tuần, tranh 60x80cm bạn ơi!', 3, 0, FALSE, FALSE, FALSE, CURRENT_TIMESTAMP - INTERVAL '20 hours');

INSERT INTO "CommentLikes" (like_id, comment_id, user_id, reaction_type, delflg, reg_datetime)
VALUES
(1, 1, 2, 'Like', FALSE, CURRENT_TIMESTAMP - INTERVAL '5 days'),
(2, 1, 5, 'Love', FALSE, CURRENT_TIMESTAMP - INTERVAL '5 days'),
(3, 3, 3, 'Haha', FALSE, CURRENT_TIMESTAMP - INTERVAL '4 days'),
(4, 5, 6, 'Like', FALSE, CURRENT_TIMESTAMP - INTERVAL '1 day');

INSERT INTO "Stories" (
    story_id, user_id,
    media_url, media_type, thumbnail_url,
    caption, bg_color, duration_sec, visibility,
    view_count, reaction_count,
    expire_datetime, is_expired, is_highlighted, highlight_name,
    delflg, reg_datetime
)
VALUES
(1, 2, 'https://storage.interacthub.com/stories/alice-dalat.jpg', 'Image', NULL, 'Đà Lạt buổi sáng 🌿', '#2D5016', 5, 'Friends', 45, 8, CURRENT_TIMESTAMP + INTERVAL '18 hours', FALSE, FALSE, NULL, FALSE, CURRENT_TIMESTAMP - INTERVAL '6 hours'),
(2, 3, 'https://storage.interacthub.com/stories/bob-code.jpg', 'Image', NULL, 'Coding time ☕', '#1a1a2e', 5, 'Public', 32, 5, CURRENT_TIMESTAMP + INTERVAL '18 hours', FALSE, FALSE, NULL, FALSE, CURRENT_TIMESTAMP - INTERVAL '4 hours'),
(3, 6, 'https://storage.interacthub.com/stories/eva-art.jpg', 'Image', NULL, 'Work in progress 🎨', '#fff5e6', 5, 'Public', 120, 25, CURRENT_TIMESTAMP + INTERVAL '22 hours', FALSE, TRUE, 'Artwork', FALSE, CURRENT_TIMESTAMP - INTERVAL '2 hours');

INSERT INTO "StoryViews" (view_id, story_id, viewer_id, view_duration, delflg, reg_datetime)
VALUES
(1, 1, 3, 5, FALSE, CURRENT_TIMESTAMP - INTERVAL '5 hours'),
(2, 1, 4, 5, FALSE, CURRENT_TIMESTAMP - INTERVAL '4 hours'),
(3, 2, 2, 5, FALSE, CURRENT_TIMESTAMP - INTERVAL '3 hours'),
(4, 3, 2, 12, FALSE, CURRENT_TIMESTAMP - INTERVAL '2 hours');

INSERT INTO "StoryReactions" (reaction_id, story_id, user_id, reaction_type, delflg, reg_datetime)
VALUES
(1, 1, 3, 'Love', FALSE, CURRENT_TIMESTAMP - INTERVAL '5 hours'),
(2, 2, 2, 'Like', FALSE, CURRENT_TIMESTAMP - INTERVAL '3 hours'),
(3, 3, 2, 'Love', FALSE, CURRENT_TIMESTAMP - INTERVAL '2 hours'),
(4, 3, 5, 'Wow', FALSE, CURRENT_TIMESTAMP - INTERVAL '90 minutes');

INSERT INTO "Notifications" (
    notification_id, recipient_id, sender_id,
    notification_type, reference_id, reference_type,
    message, is_read, redirect_url,
    delflg, reg_datetime
)
VALUES
(1, 2, 3, 'post_like', 1, 'Post', 'Bob đã thích bài viết của bạn', FALSE, '/posts/1', FALSE, CURRENT_TIMESTAMP - INTERVAL '5 hours'),
(2, 2, 4, 'post_comment', 1, 'Post', 'Carol đã bình luận về bài viết của bạn', FALSE, '/posts/1', FALSE, CURRENT_TIMESTAMP - INTERVAL '4 hours'),
(3, 3, 2, 'friend_request', 1, 'Friendship', 'Alice đã gửi lời mời kết bạn', TRUE, '/friends', FALSE, CURRENT_TIMESTAMP - INTERVAL '3 hours'),
(4, 6, 2, 'story_view', 3, 'Story', 'Alice đã xem story của bạn', FALSE, '/stories/3', FALSE, CURRENT_TIMESTAMP - INTERVAL '2 hours'),
(5, 6, 5, 'post_like', 5, 'Post', 'David đã thích bài viết của bạn', FALSE, '/posts/5', FALSE, CURRENT_TIMESTAMP - INTERVAL '1 hour');

SELECT setval(pg_get_serial_sequence('"AspNetRoles"', 'Id'), 3, true);
SELECT setval(pg_get_serial_sequence('"AspNetUsers"', 'Id'), 6, true);
SELECT setval(pg_get_serial_sequence('"UserProfiles"', 'profile_id'), 5, true);
SELECT setval(pg_get_serial_sequence('"Friendships"', 'friendship_id'), 5, true);
SELECT setval(pg_get_serial_sequence('"Hashtags"', 'hashtag_id'), 5, true);
SELECT setval(pg_get_serial_sequence('"MusicTracks"', 'music_id'), 3, true);
SELECT setval(pg_get_serial_sequence('"Posts"', 'post_id'), 5, true);
SELECT setval(pg_get_serial_sequence('"PostMedia"', 'media_id'), 3, true);
SELECT setval(pg_get_serial_sequence('"PostHashtags"', 'post_hashtag_id'), 6, true);
SELECT setval(pg_get_serial_sequence('"PostLikes"', 'like_id'), 5, true);
SELECT setval(pg_get_serial_sequence('"Comments"', 'comment_id'), 6, true);
SELECT setval(pg_get_serial_sequence('"CommentLikes"', 'like_id'), 4, true);
SELECT setval(pg_get_serial_sequence('"Stories"', 'story_id'), 3, true);
SELECT setval(pg_get_serial_sequence('"StoryViews"', 'view_id'), 4, true);
SELECT setval(pg_get_serial_sequence('"StoryReactions"', 'reaction_id'), 4, true);
SELECT setval(pg_get_serial_sequence('"Notifications"', 'notification_id'), 5, true);

COMMIT;
