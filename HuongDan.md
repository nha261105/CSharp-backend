# Hướng Dẫn Test API InteractHub

Tài liệu này tổng hợp các endpoint hiện có trong backend và curl mẫu để team frontend test nhanh trên Postman.

## 1) Cấu hình chung

- Base URL local: `http://localhost:5250`
- Nếu chạy profile https, có thể dùng: `https://localhost:7123`
- Header JSON: `Content-Type: application/json`
- Các endpoint có `[Authorize]` cần Bearer token.

Đặt biến môi trường để test nhanh:

```bash
BASE_URL="http://localhost:5250"
TOKEN="<JWT_TOKEN>"
```

Lưu ý:
- Đăng nhập qua `api/auth/login` để lấy `token`.
- Trong Postman, có thể đặt Authorization -> Bearer Token = `{{TOKEN}}`.

---

## 2) Auth APIs

### 2.1 Đăng ký
- Method: `POST`
- Endpoint: `/api/auth/register`
- Auth: Không cần

```bash
curl -X POST "$BASE_URL/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "frontend_demo",
    "email": "frontend_demo@example.com",
    "password": "Pass123!",
    "fullname": "Frontend Demo",
    "phoneNumber": "0900000000"
  }'
```

### 2.2 Đăng nhập
- Method: `POST`
- Endpoint: `/api/auth/login`
- Auth: Không cần

```bash
curl -X POST "$BASE_URL/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "frontend_demo@example.com",
    "password": "Pass123!"
  }'
```

---

## 3) Users APIs

### 3.1 Tìm kiếm user
- Method: `GET`
- Endpoint: `/api/users/search?keyword=...&page=1&pageSize=20`
- Auth: Không bắt buộc

```bash
curl "$BASE_URL/api/users/search?keyword=demo&page=1&pageSize=20"
```

### 3.2 Lấy user theo id
- Method: `GET`
- Endpoint: `/api/users/{id}`
- Auth: Không bắt buộc

```bash
curl "$BASE_URL/api/users/1"
```

### 3.3 Lấy profile user
- Method: `GET`
- Endpoint: `/api/users/{id}/profile`
- Auth: Không bắt buộc

```bash
curl "$BASE_URL/api/users/1/profile"
```

### 3.4 Cập nhật profile
- Method: `PUT`
- Endpoint: `/api/users/{id}`
- Auth: Không bắt buộc (theo controller hiện tại)

```bash
curl -X PUT "$BASE_URL/api/users/1" \
  -H "Content-Type: application/json" \
  -d '{
    "fullname": "Frontend Updated",
    "gender": "Male",
    "dateOfBirth": "2000-01-01T00:00:00Z",
    "bio": "Bio từ frontend",
    "websiteUrl": "https://example.com",
    "location": "HCM",
    "isPrivateAccount": false
  }'
```

### 3.5 Cập nhật avatar
- Method: `PUT`
- Endpoint: `/api/users/{id}/avatar`
- Auth: Không bắt buộc (theo controller hiện tại)
- Body là JSON string (không phải object)

```bash
curl -X PUT "$BASE_URL/api/users/1/avatar" \
  -H "Content-Type: application/json" \
  -d '"https://cdn.example.com/avatar.jpg"'
```

### 3.6 Cập nhật cover photo
- Method: `PUT`
- Endpoint: `/api/users/{id}/cover`
- Auth: Không bắt buộc (theo controller hiện tại)
- Body là JSON string (không phải object)

```bash
curl -X PUT "$BASE_URL/api/users/1/cover" \
  -H "Content-Type: application/json" \
  -d '"https://cdn.example.com/cover.jpg"'
```

---

## 4) Friends APIs

Tất cả Friends APIs đều cần token.

### 4.1 Gửi lời mời kết bạn
- Method: `POST`
- Endpoint: `/api/Friends/send-request`

```bash
curl -X POST "$BASE_URL/api/Friends/send-request" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{ "receiverId": 2 }'
```

### 4.2 Chấp nhận lời mời kết bạn
- Method: `PUT`
- Endpoint: `/api/Friends/accept-request`

```bash
curl -X PUT "$BASE_URL/api/Friends/accept-request" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{ "requesterId": 2 }'
```

### 4.3 Từ chối lời mời kết bạn
- Method: `DELETE`
- Endpoint: `/api/Friends/decline-request/{requesterId}`

```bash
curl -X DELETE "$BASE_URL/api/Friends/decline-request/2" \
  -H "Authorization: Bearer $TOKEN"
```

### 4.4 Huỷ kết bạn / huỷ lời mời
- Method: `DELETE`
- Endpoint: `/api/Friends/unfriend`

```bash
curl -X DELETE "$BASE_URL/api/Friends/unfriend" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{ "targetUserId": 2 }'
```

### 4.5 Chặn người dùng
- Method: `POST`
- Endpoint: `/api/Friends/block`

```bash
curl -X POST "$BASE_URL/api/Friends/block" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{ "targetUserId": 2 }'
```

### 4.6 Bỏ chặn người dùng
- Method: `PUT`
- Endpoint: `/api/Friends/unblock/{targetUserId}`

```bash
curl -X PUT "$BASE_URL/api/Friends/unblock/2" \
  -H "Authorization: Bearer $TOKEN"
```

---

## 5) Posts APIs

Tất cả Posts APIs đều cần token.

### 5.1 Lấy post theo id
- Method: `GET`
- Endpoint: `/api/posts/{id}`

```bash
curl "$BASE_URL/api/posts/1" \
  -H "Authorization: Bearer $TOKEN"
```

### 5.2 Lấy danh sách posts (phân trang)
- Method: `GET`
- Endpoint: `/api/posts?page=1&pageSize=20`

```bash
curl "$BASE_URL/api/posts?page=1&pageSize=20" \
  -H "Authorization: Bearer $TOKEN"
```

### 5.3 Lấy posts theo user
- Method: `GET`
- Endpoint: `/api/posts/user/{userId}?page=1&pageSize=20`

```bash
curl "$BASE_URL/api/posts/user/2?page=1&pageSize=20" \
  -H "Authorization: Bearer $TOKEN"
```

### 5.4 Tạo post
- Method: `POST`
- Endpoint: `/api/posts`

```bash
curl -X POST "$BASE_URL/api/posts" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "postType": "Text",
    "visibility": "Public",
    "musicStartSec": 0,
    "isPinned": false,
    "allowComment": true,
    "content": "Nội dung post demo",
    "contentFormat": "PlainText",
    "locationName": "Hồ Chí Minh",
    "locationLat": 10.7769,
    "locationLng": 106.7009,
    "feeling": "Happy",
    "originalPostId": null,
    "backgroundMusicId": null,
    "musicEndSec": null
  }'
```

### 5.5 Cập nhật post
- Method: `PUT`
- Endpoint: `/api/posts/{id}`

```bash
curl -X PUT "$BASE_URL/api/posts/1" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "content": "Nội dung mới",
    "contentFormat": "PlainText",
    "visibility": "Friends",
    "locationName": "Đà Nẵng"
  }'
```

### 5.6 Xoá post
- Method: `DELETE`
- Endpoint: `/api/posts/{id}`

```bash
curl -X DELETE "$BASE_URL/api/posts/1" \
  -H "Authorization: Bearer $TOKEN"
```

### 5.7 Like post
- Method: `POST`
- Endpoint: `/api/posts/{id}/like`

```bash
curl -X POST "$BASE_URL/api/posts/1/like" \
  -H "Authorization: Bearer $TOKEN"
```

### 5.8 Bỏ like post
- Method: `DELETE`
- Endpoint: `/api/posts/{id}/like`

```bash
curl -X DELETE "$BASE_URL/api/posts/1/like" \
  -H "Authorization: Bearer $TOKEN"
```

### 5.9 Share post
- Method: `POST`
- Endpoint: `/api/posts/{id}/share`

```bash
curl -X POST "$BASE_URL/api/posts/1/share" \
  -H "Authorization: Bearer $TOKEN"
```

---

## 6) Stories APIs

Tất cả Stories APIs đều cần token.

### 6.1 Tạo story
- Method: `POST`
- Endpoint: `/api/stories`

```bash
curl -X POST "$BASE_URL/api/stories" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "mediaUrl": "https://cdn.example.com/story.jpg",
    "mediaType": "Image",
    "thumbnailUrl": null,
    "caption": "Story từ frontend",
    "captionFormat": "PlainText",
    "bgColor": "#FFFFFF",
    "fontStyle": "Default",
    "durationSec": 5,
    "visibility": "Friends",
    "backgroundMusicId": null,
    "musicStartSec": 0,
    "musicEndSec": null
  }'
```

### 6.2 Lấy story theo id
- Method: `GET`
- Endpoint: `/api/stories/{id}`

```bash
curl "$BASE_URL/api/stories/1" \
  -H "Authorization: Bearer $TOKEN"
```

### 6.3 Lấy stories theo user
- Method: `GET`
- Endpoint: `/api/stories/user/{userId}?page=1&pageSize=20`

```bash
curl "$BASE_URL/api/stories/user/2?page=1&pageSize=20" \
  -H "Authorization: Bearer $TOKEN"
```

### 6.4 Lấy stories của bạn bè
- Method: `GET`
- Endpoint: `/api/stories/friends?page=1&pageSize=20`

```bash
curl "$BASE_URL/api/stories/friends?page=1&pageSize=20" \
  -H "Authorization: Bearer $TOKEN"
```

### 6.5 Lấy feed stories
- Method: `GET`
- Endpoint: `/api/stories/feed?page=1&pageSize=20`

```bash
curl "$BASE_URL/api/stories/feed?page=1&pageSize=20" \
  -H "Authorization: Bearer $TOKEN"
```

### 6.6 Lấy highlights
- Method: `GET`
- Endpoint: `/api/stories/highlights`

```bash
curl "$BASE_URL/api/stories/highlights" \
  -H "Authorization: Bearer $TOKEN"
```

### 6.7 Cập nhật story
- Method: `PUT`
- Endpoint: `/api/stories/{id}`

```bash
curl -X PUT "$BASE_URL/api/stories/1" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "mediaUrl": "https://cdn.example.com/story-updated.jpg",
    "thumbnailUrl": "https://cdn.example.com/thumb.jpg",
    "caption": "Cập nhật story",
    "captionFormat": "PlainText",
    "bgColor": "#F5F5F5",
    "fontStyle": "Bold",
    "durationSec": 10,
    "visibility": "Public",
    "backgroundMusicId": null,
    "musicStartSec": 0,
    "musicEndSec": null,
    "isHighlighted": true,
    "highlightName": "Travel"
  }'
```

### 6.8 Xoá story
- Method: `DELETE`
- Endpoint: `/api/stories/{id}`

```bash
curl -X DELETE "$BASE_URL/api/stories/1" \
  -H "Authorization: Bearer $TOKEN"
```

### 6.9 Đánh dấu đã xem story
- Method: `POST`
- Endpoint: `/api/stories/{id}/view?viewDuration=5`

```bash
curl -X POST "$BASE_URL/api/stories/1/view?viewDuration=5" \
  -H "Authorization: Bearer $TOKEN"
```

### 6.10 Thêm reaction cho story
- Method: `POST`
- Endpoint: `/api/stories/{id}/reaction`

```bash
curl -X POST "$BASE_URL/api/stories/1/reaction" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{ "reactionType": "Like" }'
```

### 6.11 Xoá reaction của story
- Method: `DELETE`
- Endpoint: `/api/stories/{id}/reaction`

```bash
curl -X DELETE "$BASE_URL/api/stories/1/reaction" \
  -H "Authorization: Bearer $TOKEN"
```

### 6.12 Lấy danh sách người xem story
- Method: `GET`
- Endpoint: `/api/stories/{id}/viewers`

```bash
curl "$BASE_URL/api/stories/1/viewers" \
  -H "Authorization: Bearer $TOKEN"
```

### 6.13 Lấy danh sách reaction của story
- Method: `GET`
- Endpoint: `/api/stories/{id}/reactions`

```bash
curl "$BASE_URL/api/stories/1/reactions" \
  -H "Authorization: Bearer $TOKEN"
```

---

## 7) PostReports APIs

### 7.1 Tạo báo cáo bài viết
- Method: `POST`
- Endpoint: `/api/postreports`
- Auth: User thường (Admin/Moderator không được report)

```bash
curl -X POST "$BASE_URL/api/postreports" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "postId": 1,
    "reason": "Spam",
    "description": "Nội dung nghi ngờ là spam"
  }'
```

### 7.2 Cập nhật trạng thái xử lý report (Admin/Moderator)
- Method: `PUT`
- Endpoint: `/api/postreports/post/{postId}/status`
- Auth: Role `Admin` hoặc `Moderator`

```bash
curl -X PUT "$BASE_URL/api/postreports/post/1/status" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "status": "Reviewing",
    "reviewNote": "Đang kiểm tra",
    "actionTaken": null
  }'
```

Nếu chuyển sang `Resolved`, cần có `actionTaken` thuộc 1 trong: `PostRemoved`, `UserWarned`, `UserBanned`.

```bash
curl -X PUT "$BASE_URL/api/postreports/post/1/status" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "status": "Resolved",
    "reviewNote": "Vi phạm rõ ràng",
    "actionTaken": "PostRemoved"
  }'
```

### 7.3 Xoá toàn bộ report của post (Admin)
- Method: `DELETE`
- Endpoint: `/api/postreports/post/{postId}`
- Auth: Role `Admin`
- Lưu ý: controller đang đọc `PostId` từ body

```bash
curl -X DELETE "$BASE_URL/api/postreports/post/1" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{ "postId": 1 }'
```

### 7.4 Lấy tổng hợp report (Admin/Moderator)
- Method: `GET`
- Endpoint: `/api/postreports/summary?page=1&pageSize=10`
- Auth: Role `Admin` hoặc `Moderator`

```bash
curl "$BASE_URL/api/postreports/summary?page=1&pageSize=10" \
  -H "Authorization: Bearer $TOKEN"
```

### 7.5 Lấy danh sách report theo post (Admin/Moderator)
- Method: `GET`
- Endpoint: `/api/postreports/post/{postId}?page=1&pageSize=10`
- Auth: Role `Admin` hoặc `Moderator`

```bash
curl "$BASE_URL/api/postreports/post/1?page=1&pageSize=10" \
  -H "Authorization: Bearer $TOKEN"
```

---

## 8) Danh sách nhanh endpoint

### Auth
- `POST /api/auth/register`
- `POST /api/auth/login`

### Users
- `GET /api/users/search`
- `GET /api/users/{id}`
- `GET /api/users/{id}/profile`
- `PUT /api/users/{id}`
- `PUT /api/users/{id}/avatar`
- `PUT /api/users/{id}/cover`

### Friends
- `POST /api/Friends/send-request`
- `PUT /api/Friends/accept-request`
- `DELETE /api/Friends/decline-request/{requesterId}`
- `DELETE /api/Friends/unfriend`
- `POST /api/Friends/block`
- `PUT /api/Friends/unblock/{targetUserId}`

### Posts
- `GET /api/posts/{id}`
- `GET /api/posts`
- `GET /api/posts/user/{userId}`
- `POST /api/posts`
- `PUT /api/posts/{id}`
- `DELETE /api/posts/{id}`
- `POST /api/posts/{id}/like`
- `DELETE /api/posts/{id}/like`
- `POST /api/posts/{id}/share`

### Stories
- `POST /api/stories`
- `GET /api/stories/{id}`
- `GET /api/stories/user/{userId}`
- `GET /api/stories/friends`
- `GET /api/stories/feed`
- `GET /api/stories/highlights`
- `PUT /api/stories/{id}`
- `DELETE /api/stories/{id}`
- `POST /api/stories/{id}/view`
- `POST /api/stories/{id}/reaction`
- `DELETE /api/stories/{id}/reaction`
- `GET /api/stories/{id}/viewers`
- `GET /api/stories/{id}/reactions`

### PostReports
- `POST /api/postreports`
- `PUT /api/postreports/post/{postId}/status`
- `DELETE /api/postreports/post/{postId}`
- `GET /api/postreports/summary`
- `GET /api/postreports/post/{postId}`