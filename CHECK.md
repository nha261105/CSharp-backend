# CHECK PROJECT — InteractHub Backend

---

## 1. Tổng quan

- **Dự án làm gì:** Backend API cho mạng xã hội InteractHub — người dùng có thể đăng bài, bình luận, thả cảm xúc, chia sẻ bài, kết bạn, đăng Story và nhận thông báo real-time.
- **Đã implement:** Auth (register/login/logout/forgot/reset password), CRUD Post, Like/Unlike Post, Share Post, Add Comment (có mention), Toggle reaction Comment, Get danh sách comment & cảm xúc bài viết, Friends (gửi/chấp nhận/từ chối/hủy/block/unblock), Stories (CRUD + view + reaction), Notifications (get, read, read all), Upload media (avatar, cover, post-media, story-media), PostReports (CRUD + admin review), Users (search, get, update profile/avatar/cover).
- **Kiến trúc:** Clean Architecture 3 tầng — `InteractHub.API` (Controllers, Middlewares, Hubs), `InteractHub.Core` (Entities, DTOs, Interfaces), `InteractHub.Infrastructure` (EF Core + PostgreSQL, Repositories, Services, Azure Blob Storage). Real-time qua SignalR.
- **Deployment:** Docker + GitHub Actions CI/CD → Azure App Service.

---

## 2. API hiện có

### Auth — `AuthController` (`api/auth`)
| Method | Endpoint | Mô tả |
|--------|----------|-------|
| POST | `/api/auth/register` | Đăng ký tài khoản, tạo JWT ngay |
| POST | `/api/auth/login` | Đăng nhập, cập nhật LastLogin |
| POST | `/api/auth/logout` | Đăng xuất (invalidate security stamp) |
| POST | `/api/auth/forgot-password` | Tạo reset token (dev: trả token về response) |
| POST | `/api/auth/reset-password` | Đặt lại mật khẩu |

---

### Posts — `PostsController` (`api/posts`)
| Method | Endpoint | Mô tả | Service |
|--------|----------|-------|---------|
| GET | `/api/posts` | Danh sách bài viết (phân trang) | `PostsService.GetListPostPageAsync` |
| GET | `/api/posts/{id}` | Chi tiết 1 bài viết | `PostsService.GetPostWithIdAsync` |
| GET | `/api/posts/user/{userId}` | Bài viết của 1 user | `PostsService.GetListUserPagePostAsync` |
| POST | `/api/posts` | Tạo bài viết mới | `PostsService.CreatePostAsync` |
| PUT | `/api/posts/{id}` | Cập nhật bài viết | `PostsService.UpdatePostAsnc` |
| DELETE | `/api/posts/{id}` | Xóa bài viết (soft delete) | `PostsService.DeletePostAsync` |
| POST | `/api/posts/{id}/like` | Like bài viết | `PostsService.LikePostAsync` |
| DELETE | `/api/posts/{id}/like` | Unlike bài viết | `PostsService.UnLikePostAsync` |
| POST | `/api/posts/{id}/share` | Chia sẻ bài viết | `PostsService.SharePostAsync` |
| POST | `/api/posts/{id}/comments` | Thêm bình luận (có mention) | `PostsService.AddCommentAsync` |
| POST | `/api/posts/{commentId}/reaction` | Toggle cảm xúc comment | `PostsService.ToggleCommentReactionAsync` |
| GET | `/api/posts/{commentId}/reactions-detail` | Chi tiết cảm xúc comment | `PostsService.GetCommentReactionsDetailAsync` |
| GET | `/api/posts/{id}/comments-list` | Danh sách comment bài viết | `PostsService.GetPostCommentsAsync` |
| GET | `/api/posts/{id}/post-reactions-detail` | Chi tiết cảm xúc bài viết | `PostsService.GetPostReactionsDetailAsync` |

---

### Friends — `FriendsController` (`api/friends`)
| Method | Endpoint | Mô tả |
|--------|----------|-------|
| POST | `/api/friends/send-request` | Gửi lời mời kết bạn |
| PUT | `/api/friends/accept-request` | Chấp nhận lời mời |
| DELETE | `/api/friends/decline-request/{requesterId}` | Từ chối lời mời |
| DELETE | `/api/friends/unfriend` | Hủy bạn / hủy lời mời |
| POST | `/api/friends/block` | Chặn người dùng |
| PUT | `/api/friends/unblock/{targetUserId}` | Bỏ chặn |
| GET | `/api/friends/my-friends` | Danh sách bạn bè |
| GET | `/api/friends/pending-requests` | Danh sách lời mời chờ |
| GET | `/api/friends/mutual-friends/{targetUserId}` | Bạn chung với user cụ thể |
| GET | `/api/friends/blocked-users` | Danh sách đã chặn |

---

### Stories — `StoriesController` (`api/stories`)
| Method | Endpoint | Mô tả |
|--------|----------|-------|
| POST | `/api/stories` | Tạo story |
| GET | `/api/stories/{id}` | Lấy story theo ID |
| GET | `/api/stories/user/{userId}` | Story của 1 user |
| GET | `/api/stories/friends` | Story của bạn bè |
| GET | `/api/stories/feed` | Feed story |
| GET | `/api/stories/highlights` | Story nổi bật |
| PUT | `/api/stories/{id}` | Cập nhật story |
| DELETE | `/api/stories/{id}` | Xóa story |
| POST | `/api/stories/{id}/view` | Đánh dấu đã xem |
| POST | `/api/stories/{id}/reaction` | Thêm reaction |
| DELETE | `/api/stories/{id}/reaction` | Xóa reaction |
| GET | `/api/stories/{id}/viewers` | Danh sách người xem |
| GET | `/api/stories/{id}/reactions` | Danh sách reaction |

---

### Users — `UsersController` (`api/users`)
| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | `/api/users/search?keyword=xxx` | Tìm kiếm user theo username/fullname/email |
| GET | `/api/users/{id}` | Lấy thông tin user |
| GET | `/api/users/{id}/profile` | Lấy profile user |
| PUT | `/api/users/{id}` | Cập nhật profile |
| PUT | `/api/users/{id}/avatar` | Cập nhật avatar URL |
| PUT | `/api/users/{id}/cover` | Cập nhật ảnh bìa URL |

---

### Notifications — `NotificationsController` (`api/notifications`)
| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | `/api/notifications` | Danh sách thông báo (filter unread) |
| GET | `/api/notifications/unread-count` | Đếm thông báo chưa đọc |
| PATCH | `/api/notifications/{id}/read` | Đánh dấu đã đọc |
| PATCH | `/api/notifications/read-all` | Đánh dấu tất cả đã đọc |

---

### Uploads — `UploadsController` (`api/uploads`)
| Method | Endpoint | Mô tả |
|--------|----------|-------|
| POST | `/api/uploads/avatar` | Upload ảnh đại diện → Azure Blob |
| POST | `/api/uploads/cover` | Upload ảnh bìa → Azure Blob |
| POST | `/api/uploads/post-media` | Upload media bài viết → Azure Blob |
| POST | `/api/uploads/story-media` | Upload media story → Azure Blob |

---

### PostReports — `PostReportsController` (`api/postreports`)
| Method | Endpoint | Mô tả | Role |
|--------|----------|-------|------|
| POST | `/api/postreports` | Báo cáo bài viết | User |
| PUT | `/api/postreports/post/{postId}/status` | Duyệt báo cáo | Admin/Moderator |
| DELETE | `/api/postreports/post/{postId}` | Xóa báo cáo | Admin |
| GET | `/api/postreports/summary` | Xem tóm tắt báo cáo gộp | Admin/Moderator |
| GET | `/api/postreports/post/{postId}` | Xem báo cáo theo bài viết | Admin/Moderator |

---

## 3. Business Logic

### Module Auth
- ✅ Validate trùng email/username khi register
- ✅ Kiểm tra IsActive khi login
- ✅ Lockout sau nhiều lần sai password
- ✅ Rollback nếu không gán được role sau khi tạo user
- ⚠️ `forgot-password` chưa gửi email thực sự — chỉ trả token về response ở dev mode; production trả thông báo chung (chưa có email service)

### Module Post
- ✅ CRUD đầy đủ, soft delete (`Delflg`)
- ✅ `GetListPostPageAsync` trả về `FullName`, `AvatarUrl`, `UserName` của người đăng
- ✅ `OriginalPost` được embed trong response (1 cấp sâu, không đệ quy vô hạn)
- ✅ `PostMedias` được trả trong response
- ✅ Transaction + ExecutionStrategy cho LikePost, UnlikePost, SharePost
- ⚠️ **`GET /api/posts/{id}` KHÔNG trả về comments & reactions** — muốn có phải gọi thêm 2 API riêng
- ⚠️ **Route nhầm lẫn:** `POST /api/posts/{commentId}/reaction` dùng `commentId` nhưng nằm trong `PostsController`
- ⚠️ **Share Post flow không rõ:** `POST /{id}/share` chỉ tăng `ShareCount`, KHÔNG tự tạo bài Post mới có `OriginalPostId`; client muốn share phải tự gọi thêm `POST /posts` với `OriginalPostId`
- ❌ Không validate `OriginalPostId` phải tồn tại khi tạo Post
- ❌ Không có mention (`PostMention`) trong Create Post dù entity đã tồn tại
- ❌ Không có sửa/xóa Comment
- ❌ `LikePostAsync` hardcode `ReactionType = "Like"` — chưa hỗ trợ nhiều loại cảm xúc bài viết

### Module Comment
- ✅ Tạo comment có mention, có reply (ParentCommentId)
- ✅ Toggle reaction comment (đa cảm xúc, remove nếu cùng type, đổi nếu khác type)
- ✅ Get danh sách comment (phân trang, kèm mentions)
- ✅ Get chi tiết cảm xúc comment
- ⚠️ `GetPostCommentsAsync` không kèm reactions của từng comment — phải gọi riêng per comment
- ❌ Chưa có sửa/xóa comment
- ❌ Chưa có GET replies của 1 comment cha riêng biệt

### Module Friendship
- ✅ Đầy đủ: gửi, chấp nhận, từ chối, hủy bạn, block, unblock
- ✅ Kiểm tra self-request, block, duplicate request
- ✅ Tính mutual friends (kèm TopMutualFriends)
- ❌ Không có API gợi ý kết bạn (friend suggestion)

### Module Story
- ✅ CRUD, view, reaction, highlights, feed bạn bè
- ⚠️ Logic validate bên trong StoriesService chưa đọc đủ để đánh giá edge case

### Module Notifications
- ✅ Lấy danh sách, đọc 1, đọc tất cả, đếm unread
- ✅ Push real-time qua SignalR khi like/share post
- ⚠️ Notification cho comment và friend request chưa thấy được gửi trong code hiện tại

### Module Upload
- ✅ Upload ảnh/video cho avatar, cover, post-media, story-media → Azure Blob
- ⚠️ `CreatePostRequestDto` không có field `MediaUrls[]` — không liên kết `PostMedia` khi tạo Post; flow upload rời với create post

### Module Search
- ✅ `GET /api/users/search?keyword=` tìm user theo username/fullname/email
- ❌ Chưa có global search (users + posts cùng 1 endpoint)

---

## 4. Đối chiếu yêu cầu

### ✅ Đã có
- `POST /api/posts/{id}/comments` — Thêm comment (có mention)
- `POST /api/posts/{commentId}/reaction` — Thả cảm xúc comment
- `GET /api/posts/{id}/comments-list` — Danh sách comment
- `GET /api/posts/{commentId}/reactions-detail` — Danh sách cảm xúc comment
- `GET /api/posts/{id}/post-reactions-detail` — Danh sách cảm xúc bài viết
- Get Posts (list & by ID) trả về `FullName`, `AvatarUrl`, `UserName` người đăng
- `OriginalPost` được embed trong response bài viết (1 cấp)
- Entity `PostLike` và `CommentLike` đã có `ReactionType` — DB sẵn sàng cho đa cảm xúc
- Upload media lên Azure Blob

### ⚠️ Cần fix / chưa đúng
- **`GET /api/posts/{id}`** — yêu cầu trả kèm all comments + reactions; hiện tại KHÔNG làm vậy
- **Route Comment reaction không nhất quán** — nên đổi sang `/api/posts/{postId}/comments/{commentId}/reaction`
- **Share Post flow** — `POST /{id}/share` chỉ tăng `ShareCount`; không tạo bài Post mới → lệch với `OriginalPostId` flow
- **Mention trong bài Post** — entity `PostMention` tồn tại nhưng `CreatePostRequestDto` thiếu field Mentions
- **Media khi tạo Post** — upload và create post tách rời, không có liên kết `PostMedia` → `PostId` khi tạo

### ❌ Chưa có
- **Sửa/xóa Comment** (`PUT`, `DELETE` cho comment)
- **Đa cảm xúc bài Post** — đổi `LikePost/UnlikePost` → `TogglePostReaction(reactionType)`
- **API gợi ý kết bạn** — `GET /api/friends/suggestions`
- **API global search** — `GET /api/search?q=keyword` → `{ users[], posts[] }`

---

## 5. Thiếu thông tin

- **StoriesService:** chưa đọc hết 15KB logic nội bộ — chỉ đánh giá qua endpoint; edge case bên trong chưa xác nhận.
- **Notification cho FriendRequest / Comment:** không thấy gọi `_notificationsService` trong `FriendsService` và comment flow — có thể chưa implement.
- **FluentValidation:** package đã khai báo trong README nhưng không thấy validator class nào — có thể chưa dùng.
- **InteractHub.Tests:** folder tồn tại nhưng chưa đọc — không đánh giá được test coverage.

---

## 6. Danh sách task cần làm (Priority cao → thấp)

### 🔴 P1 — Fix bug / inconsistency
1. **Fix `GET /api/posts/{id}`** — trả kèm comments (page 1) + reactions list trong cùng 1 response
2. **Fix Share Post flow** — khi `POST /api/posts/{id}/share`, phải tạo bài Post mới với `OriginalPostId`; hoặc bỏ `POST /{id}/share` và để client tự tạo Post với `OriginalPostId`; đồng thời validate `OriginalPostId` hợp lệ
3. **Fix route reaction comment** — đổi `POST /api/posts/{commentId}/reaction` → `POST /api/posts/{postId}/comments/{commentId}/reaction`

### 🟠 P2 — Tính năng thiếu quan trọng
4. **Đổi PostLike → PostReaction** — thêm `TogglePostReactionAsync(userId, postId, reactionType)`; xóa `LikePostAsync`/`UnLikePostAsync` hoặc giữ làm wrapper
5. **API sửa/xóa Comment:**
   - `PUT /api/posts/{postId}/comments/{commentId}`
   - `DELETE /api/posts/{postId}/comments/{commentId}`
6. **Media khi tạo Post** — thêm `MediaUrls[]` vào `CreatePostRequestDto`; trong `CreatePostAsync` tạo `PostMedia` records liên kết ngay lúc tạo Post

### 🟡 P3 — Feature mới
7. **API Global Search:** `GET /api/search?q=keyword` → `{ searchKeyword, results: { users: [...], posts: [...] } }`
8. **API Friend Suggestion:** `GET /api/friends/suggestions` — bạn của bạn bè (2 hop), loại bỏ đã kết bạn, lọc trùng, trả kèm `mutualFriendsCount`
9. **Mention trong bài Post** — thêm `Mentions[]` vào `CreatePostRequestDto`, xử lý `PostMention` tương tự `CommentMention`

### 🟢 P4 — Cải thiện / polish
10. **Notification cho FriendRequest & Comment** — gọi `_notificationsService` trong `FriendsService.AcceptFriendRequestAsync` và `PostsService.AddCommentAsync`
11. **FluentValidation** — implement validator cho `CreatePostRequestDto`, `CreateCommentRequestDto`
12. **GET replies của comment** — `GET /api/posts/{postId}/comments/{commentId}/replies`
