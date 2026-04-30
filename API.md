# InteractHub — Tài liệu API

---

## Phần 1: Danh sách API

---

### Auth APIs — `POST /api/auth/...`

#### POST /api/auth/register
Đăng ký tài khoản mới, trả về JWT ngay.

Request:
```json
{
  "username": "ryan",
  "email": "ryan@example.com",
  "password": "ryan123",
  "fullname": "Ryan Nguyen",
  "phoneNumber": "0901234567"
}
```

Response:
```json
{
  "token": "eyJhbGci...",
  "userId": 1,
  "userName": "ryan",
  "email": "ryan@example.com",
  "fullname": "Ryan Nguyen",
  "roles": ["User"],
  "expiresAt": "2025-05-30T10:00:00Z"
}
```

---

#### POST /api/auth/login
Đăng nhập, cập nhật LastLogin.

Request:
```json
{
  "email": "ryan@example.com",
  "password": "ryan123"
}
```

Response: (giống register)

---

#### POST /api/auth/logout
Đăng xuất, invalidate token hiện tại. Yêu cầu Bearer token.

Response:
```json
{ "message": "Đăng xuất thành công" }
```

---

#### POST /api/auth/forgot-password
Tạo reset token. Ở dev mode trả token về response.

Request:
```json
{ "email": "ryan@example.com" }
```

Response (dev):
```json
{
  "message": "create reset token",
  "userId": 1,
  "resetToken": "Q2ZESjh..."
}
```

---

#### POST /api/auth/reset-password
Đặt lại mật khẩu bằng token từ forgot-password.

Request:
```json
{
  "userId": 1,
  "token": "Q2ZESjh...",
  "newPassword": "newpass123"
}
```

Response:
```json
{ "message": "Đặt lại mật khẩu thành công" }
```

---

### Posts APIs — `api/posts`

#### GET /api/posts?page=1&pageSize=20
Danh sách bài viết (phân trang, mới nhất trước).

Response:
```json
[
  {
    "postId": 10,
    "userId": 1,
    "userName": "ryan",
    "fullName": "Ryan Nguyen",
    "avatarUrl": "https://...",
    "content": "Hello world!",
    "postType": "Text",
    "visibility": "Public",
    "likeCount": 5,
    "commentCount": 2,
    "shareCount": 1,
    "isLikeByMe": false,
    "isPinned": false,
    "allowComment": true,
    "locationName": null,
    "regDateTime": "2025-04-30T08:00:00Z",
    "medias": [],
    "mentions": [],
    "originalPost": null
  }
]
```

---

#### GET /api/posts/{id}
Chi tiết 1 bài viết, kèm comments trang 1 và reactions.

Response:
```json
{
  "postId": 10,
  "userId": 1,
  "userName": "ryan",
  "fullName": "Ryan Nguyen",
  "avatarUrl": "https://...",
  "content": "Hello world!",
  "postType": "Text",
  "visibility": "Public",
  "likeCount": 5,
  "commentCount": 2,
  "shareCount": 1,
  "isLikeByMe": true,
  "isPinned": false,
  "allowComment": true,
  "locationName": null,
  "regDateTime": "2025-04-30T08:00:00Z",
  "medias": [],
  "originalPost": null,
  "postReactions": [
    {
      "reactionType": "Like",
      "count": 5,
      "users": [{ "id": 2, "fullName": "Manh", "avatarUrl": "...", "userName": "manh" }]
    }
  ],
  "comments": [
    {
      "commentId": 55,
      "postId": 10,
      "userId": 2,
      "fullName": "Manh",
      "avatarUrl": "https://...",
      "content": "Nice post!",
      "likeCount": 0,
      "replyCount": 0,
      "isEdited": false,
      "regDatetime": "2025-04-30T09:00:00Z",
      "mentions": [],
      "reactions": []
    }
  ],
  "commentsPage": 1,
  "commentsHasMore": false
}
```

---

#### GET /api/posts/user/{userId}?page=1&pageSize=20
Danh sách bài viết của 1 user cụ thể.

---

#### POST /api/posts
Tạo bài viết mới.

Request:
```json
{
  "postType": "Text",
  "visibility": "Public",
  "content": "Hôm nay đi chơi với @manh",
  "locationName": "Hà Nội",
  "medias": [
    {
      "mediaUrl": "https://blob.../image.jpg",
      "mediaType": "Image",
      "sortOrder": 0,
      "thumbnailUrl": null
    }
  ],
  "mentions": [
    { "mentionedUserId": 2, "startPos": 20, "endPos": 25 }
  ]
}
```

Response: PostResponseDto (201 Created)

---

#### PUT /api/posts/{id}
Cập nhật nội dung bài viết (chỉ chủ bài).

Request:
```json
{
  "content": "Nội dung đã sửa",
  "visibility": "Friends",
  "locationName": "Hà Nội"
}
```

---

#### DELETE /api/posts/{id}
Xóa mềm bài viết (chỉ chủ bài). Response: 204 No Content.

---

#### POST /api/posts/{id}/reaction
Toggle cảm xúc bài viết. Gọi lại cùng type để bỏ reaction.

Request:
```json
{ "reactionType": "Like" }
```
Các giá trị hợp lệ: `Like`, `Love`, `Haha`, `Wow`, `Sad`, `Angry`

Response:
```json
{ "postId": 10, "likeCount": 6, "reactionType": "Like" }
```

---

#### POST /api/posts/{id}/share
Chia sẻ bài viết, tạo bài Post mới với OriginalPostId.

Request:
```json
{
  "content": "Hay quá!",
  "visibility": "Public"
}
```

Response: PostResponseDto của bài share mới (201 Created)

---

#### GET /api/posts/{id}/comments-list?page=1&pageSize=10
Danh sách comment của bài viết (phân trang).

Response:
```json
[
  {
    "commentId": 55,
    "postId": 10,
    "userId": 2,
    "fullName": "Manh",
    "avatarUrl": "https://...",
    "content": "Nice post!",
    "parentCommentId": null,
    "likeCount": 0,
    "replyCount": 1,
    "isEdited": false,
    "regDatetime": "2025-04-30T09:00:00Z",
    "mentions": []
  }
]
```

---

#### POST /api/posts/{id}/comments
Thêm bình luận (hỗ trợ reply và mention).

Request:
```json
{
  "content": "Đồng ý nha @ryan",
  "parentCommentId": null,
  "mentions": [
    { "mentionedUserId": 1, "startPos": 10, "endPos": 15 }
  ]
}
```

---

#### PUT /api/posts/{postId}/comments/{commentId}
Sửa nội dung comment (chỉ chủ comment).

Request:
```json
{ "content": "Nội dung đã sửa" }
```

---

#### DELETE /api/posts/{postId}/comments/{commentId}
Xóa mềm comment (chỉ chủ comment). Response: 204 No Content.

---

#### GET /api/posts/{postId}/comments/{commentId}/replies?page=1&pageSize=10
Lấy danh sách replies của 1 comment (sort cũ nhất trước).

Response: (giống comments-list)

---

#### POST /api/posts/{postId}/comments/{commentId}/reaction
Toggle cảm xúc comment.

Request:
```json
{ "reactionType": "Like" }
```

Response:
```json
{ "commentId": 55, "likeCount": 3 }
```

---

#### GET /api/posts/{postId}/comments/{commentId}/reactions-detail
Chi tiết cảm xúc của 1 comment.

Response:
```json
[
  {
    "reactionType": "Like",
    "count": 3,
    "users": [{ "id": 1, "fullName": "Ryan", "avatarUrl": "...", "userName": "ryan" }]
  }
]
```

---

#### GET /api/posts/{id}/post-reactions-detail?page=1&pageSize=20
Chi tiết cảm xúc của bài viết (grouped by type).

Response: (giống reactions-detail)

---

### Friends APIs — `api/friends`

#### POST /api/friends/send-request
Gửi lời mời kết bạn.

Request:
```json
{ "receiverId": 2 }
```

Response:
```json
{ "message": "Gửi lời mời thành công." }
```

---

#### PUT /api/friends/accept-request
Chấp nhận lời mời kết bạn.

Request:
```json
{ "requesterId": 1 }
```

Response:
```json
{ "message": "Đã trở thành bạn bè" }
```

---

#### DELETE /api/friends/decline-request/{requesterId}
Từ chối lời mời kết bạn.

Response:
```json
{ "message": "Đã từ chối lời mời kết bạn" }
```

---

#### DELETE /api/friends/unfriend
Hủy bạn hoặc hủy lời mời đã gửi.

Request:
```json
{ "targetUserId": 2 }
```

---

#### POST /api/friends/block
Chặn người dùng.

Request:
```json
{ "targetUserId": 2 }
```

---

#### PUT /api/friends/unblock/{targetUserId}
Bỏ chặn người dùng.

---

#### GET /api/friends/my-friends
Danh sách bạn bè hiện tại.

Response:
```json
[
  {
    "userId": 2,
    "fullName": "Manh",
    "avatarUrl": "https://...",
    "actionDate": "2 ngày trước",
    "mutualFriendsCount": 3,
    "topMutualFriends": []
  }
]
```

---

#### GET /api/friends/pending-requests
Danh sách lời mời kết bạn đang chờ (người khác gửi cho mình).

Response: (giống my-friends)

---

#### GET /api/friends/mutual-friends/{targetUserId}
Danh sách bạn chung với 1 user.

Response:
```json
[
  {
    "userId": 3,
    "fullname": "An",
    "avatarUrl": "https://...",
    "mutualFriendsCount": 2,
    "topMutualAvatars": ["https://..."]
  }
]
```

---

#### GET /api/friends/blocked-users
Danh sách người đã chặn.

Response:
```json
[
  { "userId": 5, "fullname": "Tên user", "avatarUrl": "https://..." }
]
```

---

#### GET /api/friends/suggestions?limit=10
Gợi ý kết bạn (bạn của bạn bè — 2 hop), sort theo số bạn chung.

Response:
```json
[
  {
    "userId": 7,
    "userName": "an.nguyen",
    "fullName": "An Nguyen",
    "avatarUrl": "https://...",
    "mutualFriendsCount": 4
  }
]
```

---

### Stories APIs — `api/stories`

#### POST /api/stories
Tạo story mới.

Request:
```json
{
  "mediaUrl": "https://blob.../story.jpg",
  "mediaType": "Image",
  "caption": "Ngày đẹp trời",
  "visibility": "Friends",
  "durationSec": 5
}
```

Response: StoryResponseDto (201 Created)

---

#### GET /api/stories/{id}
Lấy chi tiết 1 story.

Response:
```json
{
  "storyId": 1,
  "userId": 1,
  "username": "ryan",
  "fullname": "Ryan Nguyen",
  "avatarUrl": "https://...",
  "mediaUrl": "https://...",
  "mediaType": "Image",
  "caption": "Ngày đẹp trời",
  "visibility": "Friends",
  "durationSec": 5,
  "viewCount": 10,
  "reactionCount": 3,
  "expireDatetime": "2025-05-01T08:00:00Z",
  "isExpired": false,
  "isHighlighted": false,
  "hasViewed": false,
  "hasReacted": false,
  "regDatetime": "2025-04-30T08:00:00Z"
}
```

---

#### GET /api/stories/user/{userId}?page=1&pageSize=20
Story của 1 user cụ thể.

---

#### GET /api/stories/friends?page=1&pageSize=20
Story của bạn bè.

---

#### GET /api/stories/feed?page=1&pageSize=20
Feed story (bạn bè + public).

---

#### GET /api/stories/highlights
Story nổi bật của bản thân.

---

#### PUT /api/stories/{id}
Cập nhật story (caption, visibility...).

Request:
```json
{
  "caption": "Caption mới",
  "visibility": "Public"
}
```

---

#### DELETE /api/stories/{id}
Xóa story. Response: 204 No Content.

---

#### POST /api/stories/{id}/view?viewDuration=3
Đánh dấu đã xem story.

Response:
```json
{ "message": "Đã xem story" }
```

---

#### POST /api/stories/{id}/reaction
Thêm reaction cho story.

Request:
```json
{ "reactionType": "Love" }
```

---

#### DELETE /api/stories/{id}/reaction
Xóa reaction của mình khỏi story. Response: 204 No Content.

---

#### GET /api/stories/{id}/viewers
Danh sách người đã xem story (chỉ chủ story xem được).

---

#### GET /api/stories/{id}/reactions
Danh sách reaction của story (chỉ chủ story xem được).

---

### Users APIs — `api/users`

#### GET /api/users/search?keyword=ryan&page=1&pageSize=20
Tìm kiếm user theo username / fullname / email.

Response:
```json
[
  {
    "id": 1,
    "userName": "ryan",
    "email": "ryan@example.com",
    "fullname": "Ryan Nguyen",
    "avatarUrl": "https://...",
    "isActive": true
  }
]
```

---

#### GET /api/users/{id}
Lấy thông tin user theo ID.

Response:
```json
{
  "id": 1,
  "userName": "ryan",
  "email": "ryan@example.com",
  "fullname": "Ryan Nguyen",
  "gender": "Male",
  "dateOfBirth": "2000-01-01",
  "avatarUrl": "https://...",
  "coverPhotoUrl": "https://...",
  "bio": "Hello!",
  "location": "Hà Nội",
  "isActive": true,
  "isPrivateAccount": false,
  "postCount": 12,
  "friendCount": 30,
  "regDateTime": "2024-01-01T00:00:00Z"
}
```

---

#### GET /api/users/{id}/profile
Lấy profile đầy đủ của user (tương tự GET /api/users/{id}).

---

#### PUT /api/users/{id}
Cập nhật profile.

Request:
```json
{
  "fullname": "Ryan Nguyen Updated",
  "bio": "Backend dev",
  "location": "HCM",
  "gender": "Male",
  "dateOfBirth": "2000-01-01",
  "websiteUrl": "https://ryan.dev"
}
```

---

#### PUT /api/users/{id}/avatar
Cập nhật URL avatar (sau khi đã upload qua /api/uploads/avatar).

Request:
```json
"https://blob.../avatar.jpg"
```

Response:
```json
{ "avatarUrl": "https://blob.../avatar.jpg" }
```

---

#### PUT /api/users/{id}/cover
Cập nhật URL ảnh bìa.

Request:
```json
"https://blob.../cover.jpg"
```

Response:
```json
{ "coverPhotoUrl": "https://blob.../cover.jpg" }
```

---

### Notifications APIs — `api/notifications`

#### GET /api/notifications?page=1&pageSize=20&unreadOnly=false
Danh sách thông báo của user hiện tại.

Response:
```json
[
  {
    "notificationId": 1,
    "recipientId": 1,
    "notificationType": "PostReacted",
    "isRead": false,
    "senderId": 2,
    "senderUserName": "manh",
    "senderFullname": "Manh",
    "senderAvatarUrl": "https://...",
    "referenceId": 10,
    "referenceType": "Post",
    "message": "đã bày tỏ cảm xúc về bài viết của bạn",
    "redirectUrl": "/posts/10",
    "regDatetime": "2025-04-30T09:00:00Z"
  }
]
```

Các `notificationType` hiện có: `PostReacted`, `PostShared`, `PostCommented`, `FriendRequestSent`, `FriendRequestAccepted`

---

#### GET /api/notifications/unread-count
Đếm số thông báo chưa đọc.

Response:
```json
{ "unreadCount": 5 }
```

---

#### PATCH /api/notifications/{id}/read
Đánh dấu 1 thông báo đã đọc.

Response: NotificationResponseDto với `isRead: true`

---

#### PATCH /api/notifications/read-all
Đánh dấu tất cả thông báo đã đọc.

Response:
```json
{ "updatedCount": 5 }
```

---

### Uploads APIs — `api/uploads`

> Tất cả upload dùng `multipart/form-data`. Upload trước, lấy URL, rồi dùng URL đó khi tạo post/story/cập nhật profile.

#### POST /api/uploads/avatar
Upload ảnh đại diện lên Azure Blob.

Form fields: `file` (binary)

Response:
```json
{
  "fileUrl": "https://blob.../avatars/abc.jpg",
  "fileName": "abc.jpg",
  "fileSize": 102400,
  "contentType": "image/jpeg"
}
```

---

#### POST /api/uploads/cover
Upload ảnh bìa lên Azure Blob.

Form fields: `file` (binary)

---

#### POST /api/uploads/post-media
Upload ảnh/video cho bài viết lên Azure Blob.

Form fields: `file` (binary)

---

#### POST /api/uploads/story-media
Upload ảnh/video cho story lên Azure Blob.

Form fields: `file` (binary)

---

### PostReports APIs — `api/postreports`

#### POST /api/postreports
Báo cáo bài viết (User role).

Request:
```json
{
  "postId": 10,
  "reason": "Spam",
  "description": "Bài viết spam quảng cáo"
}
```

Các `reason` hợp lệ: `Spam`, `Harassment`, `FakeNews`, `HateSpeech`, `Violence`, `Nudity`, `Other`

Response:
```json
{ "message": "Gửi báo cáo thành công." }
```

---

#### GET /api/postreports/summary?page=1&pageSize=10
Xem tóm tắt báo cáo gộp theo bài viết. (Admin/Moderator)

---

#### GET /api/postreports/post/{postId}?page=1&pageSize=10
Xem chi tiết báo cáo của 1 bài viết. (Admin/Moderator)

---

#### PUT /api/postreports/post/{postId}/status
Duyệt / xử lý báo cáo. (Admin/Moderator)

Luồng trạng thái: `Pending` → `Reviewing` → `Resolved` hoặc `Dismissed`

Request:
```json
{
  "status": "Resolved",
  "actionTaken": "PostRemoved"
}
```

Các `actionTaken` hợp lệ: `PostRemoved`, `UserWarned`, `UserBanned`, `NoAction`

---

#### DELETE /api/postreports/post/{postId}
Xóa toàn bộ báo cáo của 1 bài viết. (Admin only)

Request:
```json
{ "postId": 10 }
```

---

### Search API — `api/search`

#### GET /api/search?q=keyword&page=1&pageSize=10
Tìm kiếm tổng hợp users + posts.

Response:
```json
{
  "users": [
    { "id": 1, "userName": "ryan", "fullname": "Ryan Nguyen", "avatarUrl": "..." }
  ],
  "posts": [
    { "postId": 10, "content": "Hello world", "userName": "ryan" }
  ]
}
```

---

## Phần 2: Cách test API bằng Swagger UI

---

### Bước 1: Mở Swagger

Truy cập: `http://localhost:5000/docs`

---

### Bước 2: Đăng nhập lấy token

Dùng endpoint `POST /api/auth/login` trong Swagger.

Account test 1:
```json
{ "email": "ryan@example.com", "password": "ryan123" }
```

Account test 2:
```json
{ "email": "manh@example.com", "password": "manh123" }
```

Copy giá trị `token` từ response.

---

### Bước 3: Authorize

1. Nhấn nút **Authorize** (góc trên phải Swagger)
2. Nhập vào ô Value:
```
Bearer eyJhbGci...
```
3. Nhấn **Authorize** → **Close**

Từ đây tất cả request sẽ tự gắn token.

---

### Bước 4: Test từng nhóm API

---

#### Test Auth

**Đăng ký:**
`POST /api/auth/register`
```json
{
  "username": "testuser",
  "email": "test@example.com",
  "password": "test123",
  "fullname": "Test User"
}
```

**Đăng xuất:**
`POST /api/auth/logout` — không cần body

---

#### Test Posts

**Tạo bài viết text:**
`POST /api/posts`
```json
{
  "postType": "Text",
  "visibility": "Public",
  "content": "Bài viết test đầu tiên!"
}
```

**Tạo bài viết có media (upload trước):**
1. Upload: `POST /api/uploads/post-media` → lấy `fileUrl`
2. Tạo post:
```json
{
  "postType": "Image",
  "visibility": "Public",
  "content": "Ảnh đẹp nè",
  "medias": [
    { "mediaUrl": "https://blob.../image.jpg", "mediaType": "Image", "sortOrder": 0 }
  ]
}
```

**Lấy danh sách bài viết:**
`GET /api/posts?page=1&pageSize=10`

**React bài viết:**
`POST /api/posts/{id}/reaction`
```json
{ "reactionType": "Love" }
```

**Chia sẻ bài viết:**
`POST /api/posts/{id}/share`
```json
{ "content": "Hay quá!", "visibility": "Public" }
```

**Thêm comment:**
`POST /api/posts/{id}/comments`
```json
{ "content": "Nice post!" }
```

**Reply comment:**
`POST /api/posts/{id}/comments`
```json
{ "content": "Đồng ý!", "parentCommentId": 55 }
```

**Lấy replies:**
`GET /api/posts/{postId}/comments/{commentId}/replies?page=1&pageSize=10`

---

#### Test Friends

**Gửi lời mời (dùng account 1, gửi cho account 2):**
`POST /api/friends/send-request`
```json
{ "receiverId": 2 }
```

**Chấp nhận (đổi sang account 2, accept lời mời từ account 1):**
`PUT /api/friends/accept-request`
```json
{ "requesterId": 1 }
```

**Xem bạn bè:**
`GET /api/friends/my-friends`

**Gợi ý kết bạn:**
`GET /api/friends/suggestions?limit=10`

---

#### Test Stories

**Tạo story:**
`POST /api/stories`
```json
{
  "mediaUrl": "https://blob.../story.jpg",
  "mediaType": "Image",
  "caption": "Story test",
  "visibility": "Friends",
  "durationSec": 5
}
```

**Xem story:**
`POST /api/stories/{id}/view`

**React story:**
`POST /api/stories/{id}/reaction`
```json
{ "reactionType": "Love" }
```

---

#### Test Notifications

**Lấy thông báo:**
`GET /api/notifications?page=1&pageSize=20`

**Lấy số chưa đọc:**
`GET /api/notifications/unread-count`

**Đọc tất cả:**
`PATCH /api/notifications/read-all`

---

#### Test Uploads

1. Mở endpoint `POST /api/uploads/post-media`
2. Nhấn **Try it out**
3. Chọn file ảnh/video từ máy
4. Nhấn **Execute**
5. Copy `fileUrl` từ response để dùng khi tạo post/story

---

#### Test Search

**Tìm kiếm tổng hợp:**
`GET /api/search?q=ryan`

**Tìm user:**
`GET /api/users/search?keyword=ryan`

---

#### Test PostReports (cần account Admin/Moderator)

**Báo cáo bài viết (User):**
`POST /api/postreports`
```json
{
  "postId": 10,
  "reason": "Spam",
  "description": "Bài viết spam"
}
```

**Xem báo cáo (Admin/Moderator):**
`GET /api/postreports/summary`

**Duyệt báo cáo (Admin/Moderator):**
`PUT /api/postreports/post/{postId}/status`
```json
{ "status": "Reviewing" }
```

---

### Real-time Notifications (SignalR)

Kết nối WebSocket tới:
```
ws://localhost:5000/hubs/notifications?access_token={JWT_TOKEN}
```

Lắng nghe event: `NotificationCreated`

Các hành động trigger notification:
- React bài viết → chủ bài nhận `PostReacted`
- Share bài viết → chủ bài nhận `PostShared`
- Comment bài viết → chủ bài nhận `PostCommented`
- Gửi lời mời kết bạn → người nhận nhận `FriendRequestSent`
- Chấp nhận lời mời → người gửi nhận `FriendRequestAccepted`
