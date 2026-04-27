# API Reference

Tài liệu này liệt kê các API đã có trong codebase hiện tại.

## Quy ước chung

- Hầu hết endpoints đều cần JWT `Bearer` token.
- `AuthController` là nhóm public duy nhất.
- Các endpoint quản trị trong `PostReportsController` yêu cầu role `Admin` hoặc `Moderator`.

## Auth

| Method | Route                | Mục đích                         | Auth   |
| ------ | -------------------- | -------------------------------- | ------ |
| `POST` | `/api/auth/register` | Đăng ký tài khoản mới và trả JWT | Public |
| `POST` | `/api/auth/login`    | Đăng nhập và trả JWT             | Public |

## Users

| Method | Route                                        | Mục đích                                           | Auth |
| ------ | -------------------------------------------- | -------------------------------------------------- | ---- |
| `GET`  | `/api/users/search?keyword=&page=&pageSize=` | Tìm kiếm người dùng theo username, fullname, email | JWT  |
| `GET`  | `/api/users/{id}`                            | Lấy thông tin người dùng theo ID                   | JWT  |
| `GET`  | `/api/users/{id}/profile`                    | Lấy profile người dùng theo ID                     | JWT  |
| `PUT`  | `/api/users/{id}`                            | Cập nhật profile                                   | JWT  |
| `PUT`  | `/api/users/{id}/avatar`                     | Cập nhật avatar URL                                | JWT  |
| `PUT`  | `/api/users/{id}/cover`                      | Cập nhật cover photo URL                           | JWT  |

## Posts

| Method   | Route                      | Mục đích                          | Auth |
| -------- | -------------------------- | --------------------------------- | ---- |
| `GET`    | `/api/posts`               | Lấy danh sách bài viết theo trang | JWT  |
| `GET`    | `/api/posts/{id}`          | Lấy chi tiết bài viết             | JWT  |
| `GET`    | `/api/posts/user/{userId}` | Lấy bài viết của một user         | JWT  |
| `POST`   | `/api/posts`               | Tạo bài viết mới                  | JWT  |
| `PUT`    | `/api/posts/{id}`          | Cập nhật bài viết                 | JWT  |
| `DELETE` | `/api/posts/{id}`          | Xóa bài viết mềm                  | JWT  |
| `POST`   | `/api/posts/{id}/like`     | Like bài viết                     | JWT  |
| `DELETE` | `/api/posts/{id}/like`     | Bỏ like bài viết                  | JWT  |
| `POST`   | `/api/posts/{id}/share`    | Chia sẻ bài viết                  | JWT  |

## Friends

| Method   | Route                                        | Mục đích                            | Auth |
| -------- | -------------------------------------------- | ----------------------------------- | ---- |
| `POST`   | `/api/friends/send-request`                  | Gửi lời mời kết bạn                 | JWT  |
| `PUT`    | `/api/friends/accept-request`                | Chấp nhận lời mời kết bạn           | JWT  |
| `DELETE` | `/api/friends/decline-request/{requesterId}` | Từ chối lời mời kết bạn             | JWT  |
| `DELETE` | `/api/friends/unfriend`                      | Hủy kết bạn hoặc hủy lời mời đã gửi | JWT  |
| `POST`   | `/api/friends/block`                         | Chặn người dùng                     | JWT  |
| `PUT`    | `/api/friends/unblock/{targetUserId}`        | Bỏ chặn người dùng                  | JWT  |
| `GET`    | `/api/friends/my-friends`                    | Danh sách bạn bè của tôi            | JWT  |
| `GET`    | `/api/friends/pending-requests`              | Danh sách lời mời đang chờ          | JWT  |
| `GET`    | `/api/friends/mutual-friends/{targetUserId}` | Danh sách bạn chung                 | JWT  |
| `GET`    | `/api/friends/blocked-users`                 | Danh sách user đã chặn              | JWT  |

## Stories

| Method   | Route                         | Mục đích                  | Auth |
| -------- | ----------------------------- | ------------------------- | ---- |
| `POST`   | `/api/stories`                | Tạo story mới             | JWT  |
| `GET`    | `/api/stories/{id}`           | Lấy chi tiết story        | JWT  |
| `GET`    | `/api/stories/user/{userId}`  | Lấy story theo user       | JWT  |
| `GET`    | `/api/stories/friends`        | Lấy story của bạn bè      | JWT  |
| `GET`    | `/api/stories/feed`           | Lấy feed story            | JWT  |
| `GET`    | `/api/stories/highlights`     | Lấy story highlight       | JWT  |
| `PUT`    | `/api/stories/{id}`           | Cập nhật story            | JWT  |
| `DELETE` | `/api/stories/{id}`           | Xóa story mềm             | JWT  |
| `POST`   | `/api/stories/{id}/view`      | Đánh dấu đã xem story     | JWT  |
| `POST`   | `/api/stories/{id}/reaction`  | Thêm/sửa reaction story   | JWT  |
| `DELETE` | `/api/stories/{id}/reaction`  | Xóa reaction story        | JWT  |
| `GET`    | `/api/stories/{id}/viewers`   | Danh sách người xem story | JWT  |
| `GET`    | `/api/stories/{id}/reactions` | Danh sách reaction story  | JWT  |

## Notifications

| Method  | Route                             | Mục đích                                     | Auth |
| ------- | --------------------------------- | -------------------------------------------- | ---- |
| `GET`   | `/api/notifications`              | Lấy danh sách notification của user hiện tại | JWT  |
| `GET`   | `/api/notifications/unread-count` | Đếm notification chưa đọc                    | JWT  |
| `PATCH` | `/api/notifications/{id}/read`    | Đánh dấu 1 notification đã đọc               | JWT  |
| `PATCH` | `/api/notifications/read-all`     | Đánh dấu toàn bộ notification đã đọc         | JWT  |

## Uploads

| Method | Route | Mục đích | Auth |
| ------ | ----- | -------- | ---- |
| `POST` | `/api/uploads/avatar` | Upload avatar cho user hiện tại | JWT |
| `POST` | `/api/uploads/cover` | Upload cover photo cho user hiện tại | JWT |
| `POST` | `/api/uploads/post-media` | Upload media cho bài viết của user hiện tại | JWT |
| `POST` | `/api/uploads/story-media` | Upload media cho story của user hiện tại | JWT |

Lưu ý: các endpoint `Uploads` nhận `multipart/form-data`.

## PostReports

| Method   | Route                                   | Mục đích                             | Auth                    |
| -------- | --------------------------------------- | ------------------------------------ | ----------------------- |
| `POST`   | `/api/postreports`                      | Gửi báo cáo bài viết                 | JWT                     |
| `PUT`    | `/api/postreports/post/{postId}/status` | Cập nhật trạng thái xử lý báo cáo    | JWT + `Admin,Moderator` |
| `DELETE` | `/api/postreports/post/{postId}`        | Xóa toàn bộ báo cáo của một bài viết | JWT + `Admin`           |
| `GET`    | `/api/postreports/summary`              | Thống kê báo cáo theo nhóm           | JWT + `Admin,Moderator` |
| `GET`    | `/api/postreports/post/{postId}`        | Danh sách báo cáo theo bài viết      | JWT + `Admin,Moderator` |

## Ghi chú

- Các API trả về dữ liệu DTO, không trả entity trực tiếp.
- `NotificationsService` hiện chỉ quản lý inbox notification, chưa có cơ chế push real-time qua SignalR.
- Đã có API upload file trực tiếp qua `UploadsController`; endpoint URL update cũ trong `UsersController` vẫn được giữ để tương thích ngược.
