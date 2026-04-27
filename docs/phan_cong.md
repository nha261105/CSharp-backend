# Phân Công Backend

## Trạng thái hiện tại theo codebase

| # | Hạng mục | Branch | Trạng thái hiện tại |
| --- | --- | --- | --- |
| 0 | JWT Auth (Register, Login, Seed Roles) | `feature/auth-jwt` | Done |
| 1 | Seed Data (Roles, Admin, MusicTracks, Hashtags) | `feature/seed-data` | Done |
| 2 | PostsService + PostsController | `feature/posts-service` | Done |
| 3 | NotificationService + NotificationsController | `feature/notifications-service` | Done phần REST inbox, chưa có SignalR push |
| 4 | FileUploadService (Azure Blob) | `feature/file-upload-service` | Chưa thực hiện |
| 5 | CI/CD + Azure Deployment | `feature/cicd` | Done |
| 6 | UserService + UsersController | `feature/users-service` | Done |
| 7 | StoriesService + StoriesController | `feature/stories-service` | Done |
| 8 | Unit Test — UserService, StoriesService | `test/users-stories` | Chưa thực hiện đầy đủ |
| 9 | FriendsService + FriendsController | `feature/friends-service` | Done |
| 10 | PostReportService + PostReportsController | `feature/postreports-service` | Done |
| 11 | SignalR Hub (Notifications real-time) | `feature/signalr-hub` | Chưa thực hiện |
| 12 | Unit Test — FriendsService, PostReportService | `test/friends-reports` | Chưa thực hiện đầy đủ |

## Nhận xét nhanh

- CRUD nghiệp vụ chính của Auth, Posts, Users, Friends, Stories, Notifications, PostReports đã có code chạy được.
- Phần notification hiện mới là inbox REST API, chưa có luồng phát sinh notification real-time từ các nghiệp vụ khác.
- File upload vẫn là khoảng trống lớn nhất nếu team muốn hoàn thiện upload avatar, cover, story media, post media.

## Flow chuẩn khi thêm tính năng mới

`Request -> Controller -> Service -> DbContext -> Database`

### 1. Tạo DTOs

- Đặt trong `InteractHub.Core/DTOs/<Feature>/`
- Tách riêng request và response
- Không trả entity ra ngoài

### 2. Tạo interface service

- Đặt trong `InteractHub.Core/Interfaces/Services/`
- Mỗi feature có một interface riêng

### 3. Implement service

- Đặt trong `InteractHub.Infrastructure/Services/`
- Chỉ xử lý business logic và làm việc với `AppDbContext`

### 4. Đăng ký DI

- Đăng ký trong `InteractHub.API/Program.cs`
- Mỗi service mới thêm một dòng `AddScoped`

### 5. Tạo controller

- Đặt trong `InteractHub.API/Controllers/`
- Controller chỉ nhận request, gọi service và trả response

### 6. Viết test sau

- Đặt trong `InteractHub.Tests/Services/`
- Ưu tiên service test trước, sau đó mới controller/integration test
