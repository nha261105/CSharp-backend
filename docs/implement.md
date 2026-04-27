# Các Case Chưa Implement

Tài liệu này liệt kê những case chưa có đủ implementation hoặc còn thiếu một phần business logic trong codebase hiện tại.

## SignalR notification real-time

### Hiện trạng

- Có `NotificationsController` và `NotificationsService` cho inbox notification.
- Chưa có SignalR Hub trong codebase.
- Chưa có cơ chế push real-time khi có sự kiện mới.

### Cần làm

1. Tạo `NotificationHub`.
2. Đăng ký SignalR trong `Program.cs`.
3. Map hub endpoint, ví dụ `/hubs/notifications`.
4. Khi các service xử lý xong sự kiện, gọi `CreateNotificationAsync` rồi push qua hub đến đúng recipient.
5. Đồng bộ trạng thái read/unread giữa REST API và hub client.

### Sự kiện nên bắn notification

- Gửi lời mời kết bạn.
- Chấp nhận lời mời kết bạn.
- Like bài viết.
- Share bài viết.
- Comment hoặc mention nếu sau này có thêm feature.
- Reaction/view story nếu team muốn đồng bộ realtime đầy đủ.
- Cập nhật trạng thái báo cáo bài viết cho reporter.

## 3. Phát sinh notification từ nghiệp vụ thật

### Hiện trạng

- `INotificationsService.CreateNotificationAsync(...)` đã có.
- Nhưng các service khác chưa gọi nó một cách đồng bộ theo nghiệp vụ.

### Cần làm

- Trong `FriendsService`, tạo notification khi gửi/chấp nhận kết bạn.
- Trong `PostsService`, tạo notification khi like/share.
- Trong `StoriesService`, tạo notification khi reaction/view nếu cần.
- Trong `PostReportService`, tạo notification cho người báo cáo khi bài viết được xử lý.

### Mục tiêu business

- Người dùng nhận thông báo ngay khi có sự kiện liên quan.
- Notification phải xuất hiện ở cả inbox REST API và kênh realtime nếu có hub.

## 4. Test coverage

### Hiện trạng

- Project có test project nhưng chưa thấy bộ test đủ cho các service/controller chính.

### Cần làm

- Viết unit test cho:
  - Auth service/controller
  - PostsService
  - UsersService
  - FriendsService
  - StoriesService
  - NotificationsService
  - PostReportService
- Viết integration test cho flow đăng nhập và một vài endpoint chính.

### Gợi ý ưu tiên

1. Test business rule.
2. Test permission/authorization.
3. Test case lỗi và edge case.

## 5. Những phần đã có API nhưng business vẫn nên rà lại

- `NotificationsController` hiện chỉ đọc và cập nhật inbox, chưa phải hệ thống notification hoàn chỉnh.
- `UsersController` update avatar/cover bằng URL nên chưa hỗ trợ upload file thật.
- Một số flow như like/share/reaction/view đã có ở API, nhưng vẫn nên bổ sung notification và test để đồng bộ với nghiệp vụ mạng xã hội.
