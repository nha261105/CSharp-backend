# InteractHub API - Hướng Dẫn Test API

## 📋 Mục Lục
1. [Chuẩn Bị](#chuẩn-bị)
2. [Authentication](#1-authentication)
3. [Posts](#2-posts)
4. [Friends](#3-friends)
5. [Stories](#4-stories)
6. [Users](#5-users)
7. [Notifications](#6-notifications)
8. [Uploads](#7-uploads)
9. [Search](#8-search)
10. [Post Reports](#9-post-reports)

---

## Chuẩn Bị

**Base URL**: `http://localhost:5000`

**Test Accounts**:
- Account 1: `ryan@example.com` / `ryan123` (User ID: 1)
- Account 2: `manh@example.com` / `manh123` (User ID: 2)

**Lưu ý**: Sau khi login, lưu JWT token để dùng cho các request tiếp theo.

---

## 1. Authentication

### 🔑 Bước 1: Login (BẮT BUỘC - Làm đầu tiên)

```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "ryan@example.com",
    "password": "ryan123"
  }'
```

**Response mẫu**:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "userId": 1,
  "userName": "ryan",
  "email": "ryan@example.com",
  "fullname": "Ryan Nguyen",
  "roles": ["User"],
  "expiresAt": "2025-05-30T10:00:00Z"
}
```

**⚠️ QUAN TRỌNG**: Copy giá trị `token` từ response và dùng cho tất cả request sau:
```bash
export TOKEN="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

---

### 📝 Register - Đăng ký tài khoản mới

```bash
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "email": "test@example.com",
    "password": "test123",
    "fullname": "Test User",
    "phoneNumber": "0901234567"
  }'
```

---

### 🚪 Logout - Đăng xuất

```bash
curl -X POST http://localhost:5000/api/auth/logout \
  -H "Authorization: Bearer $TOKEN"
```

---

### 🔐 Forgot Password - Quên mật khẩu

```bash
curl -X POST http://localhost:5000/api/auth/forgot-password \
  -H "Content-Type: application/json" \
  -d '{
    "email": "ryan@example.com"
  }'
```

**Response (dev mode)**:
```json
{
  "message": "create reset token",
  "userId": 1,
  "resetToken": "Q2ZESjh..."
}
```

---

### 🔄 Reset Password - Đặt lại mật khẩu

```bash
curl -X POST http://localhost:5000/api/auth/reset-password \
  -H "Content-Type: application/json" \
  -d '{
    "userId": 1,
    "token": "Q2ZESjh...",
    "newPassword": "newpass123"
  }'
```

---

## 2. Posts

### 📄 Lấy danh sách bài viết

```bash
curl -X GET "http://localhost:5000/api/posts?page=1&pageSize=20" \
  -H "Authorization: Bearer $TOKEN"
```

---

### 📝 Tạo bài viết text đơn giản

```bash
curl -X POST http://localhost:5000/api/posts \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "postType": "Text",
    "visibility": "Public",
    "content": "Hôm nay thời tiết đẹp quá!"
  }'
```

---

### 📸 Tạo bài viết có ảnh

**Bước 1**: Upload ảnh trước (xem phần [Uploads](#7-uploads))

**Bước 2**: Tạo post với media URL
```bash
curl -X POST http://localhost:5000/api/posts \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "postType": "Image",
    "visibility": "Public",
    "content": "Ảnh đẹp nè mọi người!",
    "locationName": "Hà Nội",
    "medias": [
      {
        "mediaUrl": "https://blob.../image.jpg",
        "mediaType": "Image",
        "sortOrder": 0
      }
    ]
  }'
```

---

### 👤 Tạo bài viết có mention

```bash
curl -X POST http://localhost:5000/api/posts \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "postType": "Text",
    "visibility": "Public",
    "content": "Hôm nay đi chơi với @manh rất vui!",
    "mentions": [
      {
        "mentionedUserId": 2,
        "startPos": 20,
        "endPos": 25
      }
    ]
  }'
```

---

### 🔍 Xem chi tiết 1 bài viết

```bash
curl -X GET http://localhost:5000/api/posts/10 \
  -H "Authorization: Bearer $TOKEN"
```

---

### 📋 Lấy bài viết của 1 user cụ thể

```bash
curl -X GET "http://localhost:5000/api/posts/user/1?page=1&pageSize=20" \
  -H "Authorization: Bearer $TOKEN"
```

---

### ✏️ Sửa bài viết

```bash
curl -X PUT http://localhost:5000/api/posts/10 \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "content": "Nội dung đã được cập nhật",
    "visibility": "Friends",
    "locationName": "TP.HCM"
  }'
```

---

### 🗑️ Xóa bài viết

```bash
curl -X DELETE http://localhost:5000/api/posts/10 \
  -H "Authorization: Bearer $TOKEN"
```

---

### ❤️ React bài viết (Like, Love, Haha, Wow, Sad, Angry)

```bash
curl -X POST http://localhost:5000/api/posts/10/reaction \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "reactionType": "Love"
  }'
```

**Các reactionType hợp lệ**: `Like`, `Love`, `Haha`, `Wow`, `Sad`, `Angry`

**Lưu ý**: Gọi lại với cùng type để bỏ reaction.

---

### 🔄 Chia sẻ bài viết

```bash
curl -X POST http://localhost:5000/api/posts/10/share \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "content": "Bài viết hay quá, share cho mọi người cùng xem!",
    "visibility": "Public"
  }'
```

---

### 💬 Lấy danh sách comment của bài viết

```bash
curl -X GET "http://localhost:5000/api/posts/10/comments-list?page=1&pageSize=10" \
  -H "Authorization: Bearer $TOKEN"
```

---

### 💬 Thêm comment vào bài viết

```bash
curl -X POST http://localhost:5000/api/posts/10/comments \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "content": "Bài viết rất hay!"
  }'
```

---

### 💬 Reply comment (trả lời comment)

```bash
curl -X POST http://localhost:5000/api/posts/10/comments \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "content": "Mình cũng đồng ý!",
    "parentCommentId": 55
  }'
```

---

### 💬 Comment có mention

```bash
curl -X POST http://localhost:5000/api/posts/10/comments \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "content": "Đồng ý với @ryan nha!",
    "mentions": [
      {
        "mentionedUserId": 1,
        "startPos": 10,
        "endPos": 15
      }
    ]
  }'
```

---

### ✏️ Sửa comment

```bash
curl -X PUT http://localhost:5000/api/posts/10/comments/55 \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "content": "Comment đã được sửa"
  }'
```

---

### 🗑️ Xóa comment

```bash
curl -X DELETE http://localhost:5000/api/posts/10/comments/55 \
  -H "Authorization: Bearer $TOKEN"
```

---

### 📋 Lấy danh sách replies của 1 comment

```bash
curl -X GET "http://localhost:5000/api/posts/10/comments/55/replies?page=1&pageSize=10" \
  -H "Authorization: Bearer $TOKEN"
```

---

### ❤️ React comment

```bash
curl -X POST http://localhost:5000/api/posts/10/comments/55/reaction \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "reactionType": "Like"
  }'
```

---

### 📊 Xem chi tiết reactions của comment

```bash
curl -X GET http://localhost:5000/api/posts/10/comments/55/reactions-detail \
  -H "Authorization: Bearer $TOKEN"
```

---

### 📊 Xem chi tiết reactions của bài viết

```bash
curl -X GET "http://localhost:5000/api/posts/10/post-reactions-detail?page=1&pageSize=20" \
  -H "Authorization: Bearer $TOKEN"
```

---

## 3. Friends

### 👥 Gửi lời mời kết bạn

```bash
curl -X POST http://localhost:5000/api/friends/send-request \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "receiverId": 2
  }'
```

---

### ✅ Chấp nhận lời mời kết bạn

**Lưu ý**: Cần login bằng account nhận lời mời (manh@example.com)

```bash
curl -X PUT http://localhost:5000/api/friends/accept-request \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "requesterId": 1
  }'
```

---

### ❌ Từ chối lời mời kết bạn

```bash
curl -X DELETE http://localhost:5000/api/friends/decline-request/1 \
  -H "Authorization: Bearer $TOKEN"
```

---

### 💔 Hủy kết bạn

```bash
curl -X DELETE http://localhost:5000/api/friends/unfriend \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "targetUserId": 2
  }'
```

---

### 🚫 Chặn người dùng

```bash
curl -X POST http://localhost:5000/api/friends/block \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "targetUserId": 2
  }'
```

---

### ✅ Bỏ chặn người dùng

```bash
curl -X PUT http://localhost:5000/api/friends/unblock/2 \
  -H "Authorization: Bearer $TOKEN"
```

---

### 📋 Xem danh sách bạn bè

```bash
curl -X GET http://localhost:5000/api/friends/my-friends \
  -H "Authorization: Bearer $TOKEN"
```

---

### 📬 Xem lời mời kết bạn đang chờ

```bash
curl -X GET http://localhost:5000/api/friends/pending-requests \
  -H "Authorization: Bearer $TOKEN"
```

---

### 👥 Xem bạn chung với 1 user

```bash
curl -X GET http://localhost:5000/api/friends/mutual-friends/2 \
  -H "Authorization: Bearer $TOKEN"
```

---

### 🚫 Xem danh sách người đã chặn

```bash
curl -X GET http://localhost:5000/api/friends/blocked-users \
  -H "Authorization: Bearer $TOKEN"
```

---

### 💡 Gợi ý kết bạn

```bash
curl -X GET "http://localhost:5000/api/friends/suggestions?limit=10" \
  -H "Authorization: Bearer $TOKEN"
```

---

## 4. Stories

### 📸 Tạo story mới

**Bước 1**: Upload media trước (xem phần [Uploads](#7-uploads))

**Bước 2**: Tạo story
```bash
curl -X POST http://localhost:5000/api/stories \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "mediaUrl": "https://blob.../story.jpg",
    "mediaType": "Image",
    "caption": "Ngày đẹp trời!",
    "visibility": "Friends",
    "durationSec": 5
  }'
```

---

### 🔍 Xem chi tiết 1 story

```bash
curl -X GET http://localhost:5000/api/stories/1 \
  -H "Authorization: Bearer $TOKEN"
```

---

### 📋 Xem story của 1 user

```bash
curl -X GET "http://localhost:5000/api/stories/user/1?page=1&pageSize=20" \
  -H "Authorization: Bearer $TOKEN"
```

---

### 👥 Xem story của bạn bè

```bash
curl -X GET "http://localhost:5000/api/stories/friends?page=1&pageSize=20" \
  -H "Authorization: Bearer $TOKEN"
```

---

### 📰 Xem feed story (bạn bè + public)

```bash
curl -X GET "http://localhost:5000/api/stories/feed?page=1&pageSize=20" \
  -H "Authorization: Bearer $TOKEN"
```

---

### ⭐ Xem story nổi bật của bản thân

```bash
curl -X GET http://localhost:5000/api/stories/highlights \
  -H "Authorization: Bearer $TOKEN"
```

---

### ✏️ Cập nhật story

```bash
curl -X PUT http://localhost:5000/api/stories/1 \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "caption": "Caption mới",
    "visibility": "Public"
  }'
```

---

### 🗑️ Xóa story

```bash
curl -X DELETE http://localhost:5000/api/stories/1 \
  -H "Authorization: Bearer $TOKEN"
```

---

### 👁️ Đánh dấu đã xem story

```bash
curl -X POST "http://localhost:5000/api/stories/1/view?viewDuration=3" \
  -H "Authorization: Bearer $TOKEN"
```

---

### ❤️ React story

```bash
curl -X POST http://localhost:5000/api/stories/1/reaction \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "reactionType": "Love"
  }'
```

---

### ❌ Xóa reaction khỏi story

```bash
curl -X DELETE http://localhost:5000/api/stories/1/reaction \
  -H "Authorization: Bearer $TOKEN"
```

---

### 👥 Xem danh sách người đã xem story (chỉ chủ story)

```bash
curl -X GET http://localhost:5000/api/stories/1/viewers \
  -H "Authorization: Bearer $TOKEN"
```

---

### ❤️ Xem danh sách reactions của story (chỉ chủ story)

```bash
curl -X GET http://localhost:5000/api/stories/1/reactions \
  -H "Authorization: Bearer $TOKEN"
```

---

## 5. Users

### 🔍 Tìm kiếm user

```bash
curl -X GET "http://localhost:5000/api/users/search?keyword=ryan&page=1&pageSize=20" \
  -H "Authorization: Bearer $TOKEN"
```

---

### 👤 Xem thông tin user theo ID

```bash
curl -X GET http://localhost:5000/api/users/1 \
  -H "Authorization: Bearer $TOKEN"
```

---

### 📋 Xem profile đầy đủ của user

```bash
curl -X GET http://localhost:5000/api/users/1/profile \
  -H "Authorization: Bearer $TOKEN"
```

---

### ✏️ Cập nhật profile

```bash
curl -X PUT http://localhost:5000/api/users/1 \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "fullname": "Ryan Nguyen Updated",
    "bio": "Backend Developer | ASP.NET Core",
    "location": "TP.HCM",
    "gender": "Male",
    "dateOfBirth": "2000-01-01",
    "websiteUrl": "https://ryan.dev"
  }'
```

---

### 🖼️ Cập nhật avatar

**Bước 1**: Upload avatar (xem phần [Uploads](#7-uploads))

**Bước 2**: Cập nhật URL
```bash
curl -X PUT http://localhost:5000/api/users/1/avatar \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '"https://blob.../avatar.jpg"'
```

---

### 🖼️ Cập nhật ảnh bìa

```bash
curl -X PUT http://localhost:5000/api/users/1/cover \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '"https://blob.../cover.jpg"'
```

---

## 6. Notifications

### 🔔 Lấy danh sách thông báo

```bash
curl -X GET "http://localhost:5000/api/notifications?page=1&pageSize=20&unreadOnly=false" \
  -H "Authorization: Bearer $TOKEN"
```

---

### 🔔 Lấy chỉ thông báo chưa đọc

```bash
curl -X GET "http://localhost:5000/api/notifications?page=1&pageSize=20&unreadOnly=true" \
  -H "Authorization: Bearer $TOKEN"
```

---

### 🔢 Đếm số thông báo chưa đọc

```bash
curl -X GET http://localhost:5000/api/notifications/unread-count \
  -H "Authorization: Bearer $TOKEN"
```

---

### ✅ Đánh dấu 1 thông báo đã đọc

```bash
curl -X PATCH http://localhost:5000/api/notifications/1/read \
  -H "Authorization: Bearer $TOKEN"
```

---

### ✅ Đánh dấu tất cả thông báo đã đọc

```bash
curl -X PATCH http://localhost:5000/api/notifications/read-all \
  -H "Authorization: Bearer $TOKEN"
```

---

## 7. Uploads

**Lưu ý**: Tất cả upload API sử dụng `multipart/form-data`

### 🖼️ Upload avatar

```bash
curl -X POST http://localhost:5000/api/uploads/avatar \
  -H "Authorization: Bearer $TOKEN" \
  -F "file=@/path/to/avatar.jpg"
```

**Response**:
```json
{
  "fileUrl": "https://blob.../avatars/abc.jpg",
  "fileName": "abc.jpg",
  "fileSize": 102400,
  "contentType": "image/jpeg"
}
```

---

### 🖼️ Upload ảnh bìa

```bash
curl -X POST http://localhost:5000/api/uploads/cover \
  -H "Authorization: Bearer $TOKEN" \
  -F "file=@/path/to/cover.jpg"
```

---

### 📸 Upload media cho bài viết

```bash
curl -X POST http://localhost:5000/api/uploads/post-media \
  -H "Authorization: Bearer $TOKEN" \
  -F "file=@/path/to/image.jpg"
```

---

### 📸 Upload media cho story

```bash
curl -X POST http://localhost:5000/api/uploads/story-media \
  -H "Authorization: Bearer $TOKEN" \
  -F "file=@/path/to/story.jpg"
```

---

## 8. Search

### 🔍 Tìm kiếm tổng hợp (users + posts)

```bash
curl -X GET "http://localhost:5000/api/search?q=ryan&page=1&pageSize=10" \
  -H "Authorization: Bearer $TOKEN"
```

**Response**:
```json
{
  "users": [
    {
      "id": 1,
      "userName": "ryan",
      "fullname": "Ryan Nguyen",
      "avatarUrl": "https://..."
    }
  ],
  "posts": [
    {
      "postId": 10,
      "content": "Hello world",
      "userName": "ryan"
    }
  ]
}
```

---

## 9. Post Reports

### 🚨 Báo cáo bài viết (User role)

```bash
curl -X POST http://localhost:5000/api/postreports \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "postId": 10,
    "reason": "Spam",
    "description": "Bài viết spam quảng cáo"
  }'
```

**Các reason hợp lệ**: `Spam`, `Harassment`, `FakeNews`, `HateSpeech`, `Violence`, `Nudity`, `Other`

---

### 📊 Xem tóm tắt báo cáo (Admin/Moderator)

```bash
curl -X GET "http://localhost:5000/api/postreports/summary?page=1&pageSize=10" \
  -H "Authorization: Bearer $TOKEN"
```

---

### 📋 Xem chi tiết báo cáo của 1 bài viết (Admin/Moderator)

```bash
curl -X GET "http://localhost:5000/api/postreports/post/10?page=1&pageSize=10" \
  -H "Authorization: Bearer $TOKEN"
```

---

### ✅ Duyệt/xử lý báo cáo (Admin/Moderator)

```bash
curl -X PUT http://localhost:5000/api/postreports/post/10/status \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "status": "Resolved",
    "actionTaken": "PostRemoved"
  }'
```

**Các status hợp lệ**: `Pending`, `Reviewing`, `Resolved`, `Dismissed`

**Các actionTaken hợp lệ**: `PostRemoved`, `UserWarned`, `UserBanned`, `NoAction`

---

### 🗑️ Xóa toàn bộ báo cáo của 1 bài viết (Admin only)

```bash
curl -X DELETE http://localhost:5000/api/postreports/post/10 \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "postId": 10
  }'
```

---

## 🔄 Luồng Test Đầy Đủ

### Scenario 1: Test cơ bản với 2 users

**Account 1 (Ryan)**:
1. Login với ryan@example.com
2. Tạo bài viết text
3. Tạo bài viết có ảnh
4. Gửi lời mời kết bạn cho user 2

**Account 2 (Manh)**:
1. Login với manh@example.com
2. Chấp nhận lời mời kết bạn từ user 1
3. React bài viết của user 1
4. Comment vào bài viết của user 1

**Account 1 (Ryan)**:
1. Xem thông báo (sẽ thấy notification về friend request accepted và comment)
2. Reply comment của user 2
3. Tạo story
4. Chia sẻ bài viết của mình

---

### Scenario 2: Test Upload Flow

1. Login
2. Upload avatar → lấy fileUrl
3. Cập nhật avatar với URL vừa upload
4. Upload post-media → lấy fileUrl
5. Tạo bài viết với media URL
6. Upload story-media → lấy fileUrl
7. Tạo story với media URL

---

### Scenario 3: Test Friend System

1. User 1 gửi lời mời cho User 2
2. User 2 xem pending requests
3. User 2 chấp nhận lời mời
4. User 1 và User 2 xem danh sách bạn bè
5. User 1 xem bạn chung với User 2
6. User 1 xem gợi ý kết bạn

---

### Scenario 4: Test Notification System

1. User 1 tạo bài viết
2. User 2 react bài viết → User 1 nhận notification
3. User 2 comment bài viết → User 1 nhận notification
4. User 2 share bài viết → User 1 nhận notification
5. User 1 xem danh sách notifications
6. User 1 đánh dấu đã đọc tất cả

---

## 📝 Ghi Chú

### HTTP Status Codes
- `200 OK`: Request thành công
- `201 Created`: Tạo resource thành công
- `204 No Content`: Xóa thành công
- `400 Bad Request`: Dữ liệu không hợp lệ
- `401 Unauthorized`: Chưa đăng nhập hoặc token không hợp lệ
- `403 Forbidden`: Không có quyền truy cập
- `404 Not Found`: Không tìm thấy resource
- `500 Internal Server Error`: Lỗi server

### Tips
- Luôn kiểm tra token còn hạn hay không
- Lưu lại các ID (postId, userId, commentId) để test các API liên quan
- Test cả trường hợp thành công và thất bại
- Kiểm tra validation errors với dữ liệu không hợp lệ
- Test phân quyền bằng cách dùng account khác thử sửa/xóa resource không phải của mình

---

## 🌐 Real-time Notifications (SignalR)

Kết nối WebSocket:
```
ws://localhost:5000/hubs/notifications?access_token={JWT_TOKEN}
```

**Event**: `NotificationCreated`

**Các hành động trigger notification**:
- React bài viết → `PostReacted`
- Share bài viết → `PostShared`
- Comment bài viết → `PostCommented`
- Gửi lời mời kết bạn → `FriendRequestSent`
- Chấp nhận lời mời → `FriendRequestAccepted`
