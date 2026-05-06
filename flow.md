# Flow Các Tính Năng Backend - InteractHub

Dựa vào mã nguồn backend, dưới đây là luồng hoạt động chi tiết của 7 feature:

### 1. Flow: Đăng ký -> Đăng nhập -> Cập nhật thông tin tài khoản
*   **Đăng ký:** User nhập thông tin (Email, Username, Password) -> Frontend gọi `POST /api/auth/register` -> `AuthController` validate dữ liệu -> Sử dụng `UserManager` tạo `User` trong DB -> Gán role `"User"` -> Generate JWT token -> Response trả về `Token` và thông tin User.
*   **Đăng nhập:** User nhập (Email, Password) -> `POST /api/auth/login` -> Validate Email tồn tại và chưa bị khóa -> `SignInManager.CheckPasswordSignInAsync` -> Update `LastLoginDateTime` trong DB -> Generate JWT token -> Response trả về `Token` và thông tin.
*   **Cập nhật thông tin:** User (truyền JWT vào Header) -> `PUT /api/users/{id}` (với payload cập nhật) -> `UsersService.UpdateProfileAsync` -> Update DB -> Response thành công. (Riêng Avatar và Cover được cập nhật qua flow `POST /api/uploads/avatar`).

### 2. Flow: Đăng bài -> thả cảm xúc -> comment -> thả cảm xúc comment -> phản hồi
*   **Đăng bài:** User -> `POST /api/posts` (kèm Content, Privacy, PostType) -> `PostService.CreatePostAsync` -> Lưu vào bảng `Posts` -> Response trả về Post mới tạo.
*   **Thả cảm xúc:** User -> `POST /api/posts/{id}/reaction` (Truyền `ReactionType`) -> `PostService.TogglePostReactionAsync` -> Thêm/Sửa/Xóa bản ghi trong `PostReactions` -> Cập nhật & trả về `likeCount` mới của bài viết.
*   **Comment (Bình luận):** User -> `POST /api/posts/{id}/comments` -> `PostService.AddCommentAsync` -> Lưu vào bảng `Comments` -> Response trả về data comment mới.
*   **Thả cảm xúc comment:** User -> `POST /api/posts/{postId}/comments/{commentId}/reaction` -> `PostService.ToggleCommentReactionAsync` -> Update `CommentReactions` -> Trả về `likeCount` của comment.
*   **Phản hồi (Reply):** User -> `POST /api/posts/{id}/comments` (như comment thường nhưng payload truyền thêm `ParentCommentId`) -> Backend xác định đây là reply -> Lưu với cấu trúc phân cấp -> Response.

### 3. Flow: Tạo story -> Xem story mới tạo -> Một account khác vào xem -> thả cảm xúc
*   **Tạo story:** User -> `POST /api/stories` -> `StoriesService.CreateStoryAsync` -> Lưu cấu hình Story (Content, Media, Duration) -> Response.
*   **Xem story mới tạo:** User -> `GET /api/stories/{id}` hoặc `GET /api/stories/user/{userId}` -> Backend check quyền và trả về chi tiết Story.
*   **Account khác vào xem:** Account B click vào story -> Load data qua `GET /api/stories/{id}` -> Đồng thời ngầm gọi `POST /api/stories/{id}/view` -> `StoriesService.MarkStoryAsViewedAsync` -> Lưu Account B vào danh sách `StoryViewers`.
*   **Thả cảm xúc:** Account B -> `POST /api/stories/{id}/reaction` -> `StoriesService.AddReactionAsync` -> Lưu vào `StoryReactions` -> Gửi thông báo (nếu có).

### 4. Flow: Vào trang cá nhân -> gửi lời mời kết bạn -> click thông báo -> xem trang cá nhân -> xác nhận
*   **Vào trang cá nhân:** User A -> `GET /api/users/{targetId}/profile` -> Xem thông tin User B.
*   **Gửi kết bạn:** User A -> `POST /api/friends/send-request` (Payload: targetId) -> `FriendsService` tạo record `Friendship` với Status = `Pending` -> Tự động sinh `Notification` báo cho User B.
*   **Click thông báo:** User B load `GET /api/notifications` -> Nhận thông báo "User A gửi kết bạn" -> Click vào thông báo gọi `PATCH /api/notifications/{id}/read` (đánh dấu đã đọc) -> Frontend điều hướng tới Profile của A hoặc danh sách Pending.
*   **Xem trang cá nhân:** User B -> `GET /api/users/{id}/profile` (của A) để check.
*   **Xác nhận:** User B -> `PUT /api/friends/accept-request` -> `FriendsService.AcceptFriendRequestAsync` -> Cập nhật trạng thái `Friendship` thành `Accepted` -> Trở thành bạn bè.

### 5. Flow: Click vào hashtag -> tìm kiếm các bài viết (sort theo tương tác) -> Click xem
*(Lưu ý: Dựa trên mã nguồn hiện tại, flow này đang hoạt động dưới dạng keyword search chứ logic hashtag chuyên biệt chưa được hoàn thiện 100%)*
*   **Click hashtag:** User click vào `#hashtag` trên UI -> Frontend tự động đưa `#hashtag` vào ô search.
*   **Tìm kiếm:** Gọi API `GET /api/search?q=#hashtag` -> `SearchService.GlobalSearchAsync` chạy. Hiện tại hàm `SearchPostsAsync` trong service này đang sử dụng toán tử `LIKE %keyword%` để quét nội dung bài viết (`p.Content`), và nó đang được **sắp xếp theo thời gian (`RegDatetime`)** chứ chưa sort theo số lượng tương tác (`LikeCount` / `CommentCount`). 
*   **Click xem bài:** User chọn 1 bài -> Gọi `GET /api/posts/{id}` -> Lấy chi tiết bài viết.

### 6. Flow: Review quy trình code -> deploy -> show kết quả CI
Theo file `deploy.yml` nằm trong folder `.github/workflows`:
*   **Trigger:** Khi Developer `git push` hoặc merge code vào nhánh `main` (hoặc `feature/notifications-service`).
*   **CI Pipeline (GitHub Actions):** 
    1. Checkout code (`actions/checkout`).
    2. Đăng nhập vào GitHub Container Registry (`ghcr.io`).
    3. Docker Build & Push: Đóng gói project thành Docker image -> Gắn tag (theo chuỗi hash của commit và `latest`) -> Push lên registry.
*   **CD Pipeline (Deploy):** 
    4. Kéo image vừa build deploy thẳng lên **Azure App Service** thông qua action `azure/webapps-deploy` cùng với Publish Profile (được lưu trong Github Secrets `AZURE_WEBAPP_PUBLISH_PROFILE`).

### 7. Show các sơ đồ trên Azure -> Thảo luận về sơ đồ
Dựa vào cách hệ thống được thiết kế và code, **Sơ đồ kiến trúc thực tế trên Azure** đang hoạt động như sau:
*   **GitHub Repository** -> Kích hoạt **GitHub Actions CI/CD** qua Webhook.
*   **Azure App Service (Web App - Linux):** Host API backend, kéo Docker Image từ GitHub Container Registry (`ghcr.io/owner/interacthub-api`).
*   **Azure SQL Database:** Lưu trữ toàn bộ dữ liệu quan hệ (Users, Posts, Comments, Friendships...). Kết nối thông qua chuỗi `DefaultConnection` trong biến môi trường.
*   *(Tuỳ chọn mở rộng)*: Hệ thống lưu trữ file (Avatar, Cover, Post Media) hiện tại trong file `UploadsController` dùng Interface `IFileUploadService`. Trên môi trường thực tế Azure, mục này sẽ trỏ tới **Azure Blob Storage** để tối ưu lưu trữ thay vì lưu ở ổ cứng của App Service.
