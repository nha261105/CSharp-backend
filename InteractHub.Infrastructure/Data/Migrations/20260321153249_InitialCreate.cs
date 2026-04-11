using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InteractHub.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    delflg = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    reg_datetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    full_name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    gender = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    date_of_birth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    avatar_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    cover_photo_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    bio = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    website_url = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    is_private_account = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    last_login_datetime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    delflg = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    reg_datetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    upd_datetime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hashtags",
                columns: table => new
                {
                    hashtag_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tag_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    post_count = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    trending_score = table.Column<decimal>(type: "decimal(10,2)", nullable: false, defaultValue: 0m),
                    is_trending = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    delflg = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    reg_datetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    upd_datetime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hashtags", x => x.hashtag_id);
                });

            migrationBuilder.CreateTable(
                name: "MusicTracks",
                columns: table => new
                {
                    music_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    artist = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    audio_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    thumbnail_url = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    duration_sec = table.Column<int>(type: "int", nullable: true),
                    is_licensed = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    source = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Internal"),
                    delflg = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    reg_datetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    upd_datetime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicTracks", x => x.music_id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Friendships",
                columns: table => new
                {
                    friendship_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    requester_id = table.Column<long>(type: "bigint", nullable: false),
                    addressee_id = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pending"),
                    action_user_id = table.Column<long>(type: "bigint", nullable: true),
                    is_blocked = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    blocked_by_id = table.Column<long>(type: "bigint", nullable: true),
                    delflg = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    reg_datetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    upd_datetime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendships", x => x.friendship_id);
                    table.ForeignKey(
                        name: "FK_Friendships_AspNetUsers_action_user_id",
                        column: x => x.action_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Friendships_AspNetUsers_addressee_id",
                        column: x => x.addressee_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Friendships_AspNetUsers_blocked_by_id",
                        column: x => x.blocked_by_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Friendships_AspNetUsers_requester_id",
                        column: x => x.requester_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    notification_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    recipient_id = table.Column<long>(type: "bigint", nullable: false),
                    sender_id = table.Column<long>(type: "bigint", nullable: true),
                    notification_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    reference_id = table.Column<long>(type: "bigint", nullable: true),
                    reference_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    is_read = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    read_datetime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    redirect_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    delflg = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    reg_datetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.notification_id);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_recipient_id",
                        column: x => x.recipient_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_sender_id",
                        column: x => x.sender_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    profile_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    relationship_status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    work_place = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    position = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    education = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    hometown = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    current_city = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    facebook_link = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    instagram_link = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    twitter_link = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    follower_count = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    following_count = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    post_count = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    friend_count = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    privacy_posts = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Public"),
                    privacy_friends = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Public"),
                    privacy_photos = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Public"),
                    notification_email_flg = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    notification_push_flg = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    delflg = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    reg_datetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    upd_datetime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.profile_id);
                    table.ForeignKey(
                        name: "FK_UserProfiles_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    post_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    content_format = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    post_type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "Text"),
                    visibility = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Public"),
                    location_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    location_lat = table.Column<decimal>(type: "decimal(10,7)", nullable: true),
                    location_lng = table.Column<decimal>(type: "decimal(10,7)", nullable: true),
                    feeling = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    original_post_id = table.Column<long>(type: "bigint", nullable: true),
                    background_music_id = table.Column<long>(type: "bigint", nullable: true),
                    music_start_sec = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    music_end_sec = table.Column<int>(type: "int", nullable: true),
                    like_count = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    comment_count = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    share_count = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    is_edited = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    is_pinned = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    is_reported = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    report_count = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    allow_comment = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    delflg = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    reg_datetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    upd_datetime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.post_id);
                    table.ForeignKey(
                        name: "FK_Posts_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Posts_MusicTracks_background_music_id",
                        column: x => x.background_music_id,
                        principalTable: "MusicTracks",
                        principalColumn: "music_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Posts_Posts_original_post_id",
                        column: x => x.original_post_id,
                        principalTable: "Posts",
                        principalColumn: "post_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Stories",
                columns: table => new
                {
                    story_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    media_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    media_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    thumbnail_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    caption = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    caption_format = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bg_color = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    font_style = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    duration_sec = table.Column<int>(type: "int", nullable: false, defaultValue: 5),
                    visibility = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Friends"),
                    background_music_id = table.Column<long>(type: "bigint", nullable: true),
                    music_start_sec = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    music_end_sec = table.Column<int>(type: "int", nullable: true),
                    view_count = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    reaction_count = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    expire_datetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_expired = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    is_highlighted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    highlight_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    delflg = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    reg_datetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stories", x => x.story_id);
                    table.ForeignKey(
                        name: "FK_Stories_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stories_MusicTracks_background_music_id",
                        column: x => x.background_music_id,
                        principalTable: "MusicTracks",
                        principalColumn: "music_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    comment_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    post_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    parent_comment_id = table.Column<long>(type: "bigint", nullable: true),
                    content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    content_format = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    image_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    like_count = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    reply_count = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    is_edited = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    is_reported = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    delflg = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    reg_datetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    upd_datetime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.comment_id);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Comments_parent_comment_id",
                        column: x => x.parent_comment_id,
                        principalTable: "Comments",
                        principalColumn: "comment_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Posts_post_id",
                        column: x => x.post_id,
                        principalTable: "Posts",
                        principalColumn: "post_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostHashtags",
                columns: table => new
                {
                    post_hashtag_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    post_id = table.Column<long>(type: "bigint", nullable: false),
                    hashtag_id = table.Column<int>(type: "int", nullable: false),
                    delflg = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    reg_datetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostHashtags", x => x.post_hashtag_id);
                    table.ForeignKey(
                        name: "FK_PostHashtags_Hashtags_hashtag_id",
                        column: x => x.hashtag_id,
                        principalTable: "Hashtags",
                        principalColumn: "hashtag_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostHashtags_Posts_post_id",
                        column: x => x.post_id,
                        principalTable: "Posts",
                        principalColumn: "post_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostLikes",
                columns: table => new
                {
                    like_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    post_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    reaction_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Like"),
                    delflg = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    reg_datetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostLikes", x => x.like_id);
                    table.ForeignKey(
                        name: "FK_PostLikes_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostLikes_Posts_post_id",
                        column: x => x.post_id,
                        principalTable: "Posts",
                        principalColumn: "post_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostMedia",
                columns: table => new
                {
                    media_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    post_id = table.Column<long>(type: "bigint", nullable: false),
                    media_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    media_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    thumbnail_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    file_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    file_size_kb = table.Column<int>(type: "int", nullable: true),
                    width_px = table.Column<int>(type: "int", nullable: true),
                    height_px = table.Column<int>(type: "int", nullable: true),
                    duration_seconds = table.Column<int>(type: "int", nullable: true),
                    sort_order = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    processing_status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Ready"),
                    delflg = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    reg_datetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostMedia", x => x.media_id);
                    table.ForeignKey(
                        name: "FK_PostMedia_Posts_post_id",
                        column: x => x.post_id,
                        principalTable: "Posts",
                        principalColumn: "post_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostMentions",
                columns: table => new
                {
                    mention_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    post_id = table.Column<long>(type: "bigint", nullable: false),
                    mentioned_user_id = table.Column<long>(type: "bigint", nullable: false),
                    start_pos = table.Column<int>(type: "int", nullable: true),
                    end_pos = table.Column<int>(type: "int", nullable: true),
                    delflg = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    reg_datetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostMentions", x => x.mention_id);
                    table.ForeignKey(
                        name: "FK_PostMentions_AspNetUsers_mentioned_user_id",
                        column: x => x.mentioned_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostMentions_Posts_post_id",
                        column: x => x.post_id,
                        principalTable: "Posts",
                        principalColumn: "post_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostReports",
                columns: table => new
                {
                    report_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    post_id = table.Column<long>(type: "bigint", nullable: false),
                    reporter_id = table.Column<long>(type: "bigint", nullable: false),
                    reason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pending"),
                    reviewed_by_id = table.Column<long>(type: "bigint", nullable: true),
                    review_note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    action_taken = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    review_datetime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    delflg = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    reg_datetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    upd_datetime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostReports", x => x.report_id);
                    table.ForeignKey(
                        name: "FK_PostReports_AspNetUsers_reporter_id",
                        column: x => x.reporter_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostReports_AspNetUsers_reviewed_by_id",
                        column: x => x.reviewed_by_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostReports_Posts_post_id",
                        column: x => x.post_id,
                        principalTable: "Posts",
                        principalColumn: "post_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostShares",
                columns: table => new
                {
                    share_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    post_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    share_content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    visibility = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Public"),
                    delflg = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    reg_datetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostShares", x => x.share_id);
                    table.ForeignKey(
                        name: "FK_PostShares_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostShares_Posts_post_id",
                        column: x => x.post_id,
                        principalTable: "Posts",
                        principalColumn: "post_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoryReactions",
                columns: table => new
                {
                    reaction_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    story_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    reaction_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Like"),
                    delflg = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    reg_datetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryReactions", x => x.reaction_id);
                    table.ForeignKey(
                        name: "FK_StoryReactions_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoryReactions_Stories_story_id",
                        column: x => x.story_id,
                        principalTable: "Stories",
                        principalColumn: "story_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoryViews",
                columns: table => new
                {
                    view_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    story_id = table.Column<long>(type: "bigint", nullable: false),
                    viewer_id = table.Column<long>(type: "bigint", nullable: false),
                    view_duration = table.Column<int>(type: "int", nullable: true),
                    delflg = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    reg_datetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryViews", x => x.view_id);
                    table.ForeignKey(
                        name: "FK_StoryViews_AspNetUsers_viewer_id",
                        column: x => x.viewer_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoryViews_Stories_story_id",
                        column: x => x.story_id,
                        principalTable: "Stories",
                        principalColumn: "story_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentLikes",
                columns: table => new
                {
                    like_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    comment_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    reaction_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Like"),
                    delflg = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    reg_datetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentLikes", x => x.like_id);
                    table.ForeignKey(
                        name: "FK_CommentLikes_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommentLikes_Comments_comment_id",
                        column: x => x.comment_id,
                        principalTable: "Comments",
                        principalColumn: "comment_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentMentions",
                columns: table => new
                {
                    mention_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    comment_id = table.Column<long>(type: "bigint", nullable: false),
                    mentioned_user_id = table.Column<long>(type: "bigint", nullable: false),
                    start_pos = table.Column<int>(type: "int", nullable: true),
                    end_pos = table.Column<int>(type: "int", nullable: true),
                    delflg = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    reg_datetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentMentions", x => x.mention_id);
                    table.ForeignKey(
                        name: "FK_CommentMentions_AspNetUsers_mentioned_user_id",
                        column: x => x.mentioned_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommentMentions_Comments_comment_id",
                        column: x => x.comment_id,
                        principalTable: "Comments",
                        principalColumn: "comment_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CommentLikes_user_id",
                table: "CommentLikes",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "UQ_CommentLikes_pair",
                table: "CommentLikes",
                columns: new[] { "comment_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommentMentions_comment_id",
                table: "CommentMentions",
                column: "comment_id");

            migrationBuilder.CreateIndex(
                name: "IX_CommentMentions_mentioned_user_id",
                table: "CommentMentions",
                column: "mentioned_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_parent_id",
                table: "Comments",
                column: "parent_comment_id");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_post_id",
                table: "Comments",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_user_id",
                table: "Comments",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_action_user_id",
                table: "Friendships",
                column: "action_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_addressee",
                table: "Friendships",
                columns: new[] { "addressee_id", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_blocked_by_id",
                table: "Friendships",
                column: "blocked_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_requester",
                table: "Friendships",
                columns: new[] { "requester_id", "status" });

            migrationBuilder.CreateIndex(
                name: "UQ_Friendships_pair",
                table: "Friendships",
                columns: new[] { "requester_id", "addressee_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hashtags_trending",
                table: "Hashtags",
                columns: new[] { "trending_score", "is_trending" });

            migrationBuilder.CreateIndex(
                name: "UQ_Hashtags_tagName",
                table: "Hashtags",
                column: "tag_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_recipient",
                table: "Notifications",
                columns: new[] { "recipient_id", "is_read" });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_sender_id",
                table: "Notifications",
                column: "sender_id");

            migrationBuilder.CreateIndex(
                name: "IX_PostHashtags_hashtag_id",
                table: "PostHashtags",
                column: "hashtag_id");

            migrationBuilder.CreateIndex(
                name: "UQ_PostHashtags_pair",
                table: "PostHashtags",
                columns: new[] { "post_id", "hashtag_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostLikes_user_id",
                table: "PostLikes",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "UQ_PostLikes_pair",
                table: "PostLikes",
                columns: new[] { "post_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostMedia_post_id",
                table: "PostMedia",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_PostMentions_mentioned_user_id",
                table: "PostMentions",
                column: "mentioned_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_PostMentions_post_id",
                table: "PostMentions",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_PostReports_post_id",
                table: "PostReports",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_PostReports_reporter_id",
                table: "PostReports",
                column: "reporter_id");

            migrationBuilder.CreateIndex(
                name: "IX_PostReports_reviewed_by_id",
                table: "PostReports",
                column: "reviewed_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_background_music_id",
                table: "Posts",
                column: "background_music_id");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_original_post_id",
                table: "Posts",
                column: "original_post_id");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_reg_datetime",
                table: "Posts",
                column: "reg_datetime");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_user_id",
                table: "Posts",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_visibility",
                table: "Posts",
                column: "visibility");

            migrationBuilder.CreateIndex(
                name: "IX_PostShares_post_id",
                table: "PostShares",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_PostShares_user_id",
                table: "PostShares",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Stories_background_music_id",
                table: "Stories",
                column: "background_music_id");

            migrationBuilder.CreateIndex(
                name: "IX_Stories_expire",
                table: "Stories",
                column: "expire_datetime");

            migrationBuilder.CreateIndex(
                name: "IX_Stories_user_id",
                table: "Stories",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_StoryReactions_user_id",
                table: "StoryReactions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "UQ_StoryReactions_pair",
                table: "StoryReactions",
                columns: new[] { "story_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoryViews_viewer_id",
                table: "StoryViews",
                column: "viewer_id");

            migrationBuilder.CreateIndex(
                name: "UQ_StoryViews_pair",
                table: "StoryViews",
                columns: new[] { "story_id", "viewer_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_user_id",
                table: "UserProfiles",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CommentLikes");

            migrationBuilder.DropTable(
                name: "CommentMentions");

            migrationBuilder.DropTable(
                name: "Friendships");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "PostHashtags");

            migrationBuilder.DropTable(
                name: "PostLikes");

            migrationBuilder.DropTable(
                name: "PostMedia");

            migrationBuilder.DropTable(
                name: "PostMentions");

            migrationBuilder.DropTable(
                name: "PostReports");

            migrationBuilder.DropTable(
                name: "PostShares");

            migrationBuilder.DropTable(
                name: "StoryReactions");

            migrationBuilder.DropTable(
                name: "StoryViews");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Hashtags");

            migrationBuilder.DropTable(
                name: "Stories");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "MusicTracks");
        }
    }
}
