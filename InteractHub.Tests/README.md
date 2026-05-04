# InteractHub.Tests

Test project cho InteractHub API sử dụng xUnit, Moq, và FluentAssertions.

## 📊 Tổng quan

- **Framework:** xUnit 2.4.2
- **Mocking:** Moq 4.20.70
- **Assertions:** FluentAssertions 6.12.0
- **In-Memory DB:** Microsoft.EntityFrameworkCore.InMemory 8.0.0
- **Integration Testing:** Microsoft.AspNetCore.Mvc.Testing 8.0.0

## 🏗️ Cấu trúc

```
InteractHub.Tests/
├── Controllers/
│   └── PostsControllerTests.cs      # 18 tests cho PostsController
├── Services/
│   ├── AuthServiceTests.cs          # 3 tests cho AuthService
│   ├── PostsServiceTests.cs         # 4 tests cho PostsService
│   └── FriendsServiceTests.cs       # 3 tests cho FriendsService
├── UnitTest1.cs                     # 2 basic tests
├── GlobalUsings.cs                  # Global using statements
├── TEST_CASES.md                    # Danh sách chi tiết test cases
└── README.md                        # File này
```

## ✅ Kết quả Tests

```
Test Run Successful.
Total tests: 30
     Passed: 30
 Total time: 5.24 seconds
```

## 🧪 Các loại Tests

### 1. Controller Tests (18 tests)
- Test HTTP responses (200, 201, 204, 404)
- Test với mock services
- Test authorization và validation

### 2. Service Tests (10 tests)
- Test business logic
- Test với In-Memory Database
- Test CRUD operations

### 3. Basic Tests (2 tests)
- Sanity checks
- Framework verification

## 🚀 Chạy Tests

### Chạy tất cả tests
```bash
dotnet test
```

### Chạy với verbosity
```bash
dotnet test --logger "console;verbosity=detailed"
```

### Chạy tests cụ thể
```bash
# Chạy tests trong một class
dotnet test --filter "FullyQualifiedName~PostsControllerTests"

# Chạy một test method cụ thể
dotnet test --filter "FullyQualifiedName~CreatePost_ReturnsPostDto_WhenValid"
```

### Code Coverage
```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

## 📝 Ví dụ Test Case

### Controller Test với Moq
```csharp
[Fact]
public async Task GetPostById_ReturnsOkResult_WhenPostExists()
{
    // Arrange
    var postId = 1L;
    var postDto = new PostDetailResponseDto { PostId = postId, Content = "Test Post" };
    _postServiceMock.Setup(s => s.GetPostDetailAsync(1L, postId))
                    .ReturnsAsync(postDto);

    // Act
    var result = await _controller.GetPostById(postId);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    var returnPost = Assert.IsType<PostDetailResponseDto>(okResult.Value);
    returnPost.PostId.Should().Be(postId);
}
```

### Service Test với In-Memory Database
```csharp
[Fact]
public async Task CreatePost_ReturnsPostDto_WhenValid()
{
    // Arrange
    var request = new CreatePostRequestDto
    {
        Content = "Hello Test Post",
        PostType = "Text",
        Visibility = "Public"
    };

    // Act
    var result = await _postsService.CreatePostAsync(1, request);

    // Assert
    result.Should().NotBeNull();
    result.Content.Should().Be("Hello Test Post");
    result.UserId.Should().Be(1);
}
```

## 📈 Code Coverage

| Service/Controller | Coverage |
|-------------------|----------|
| AuthService | 80% |
| PostsService | 80% |
| FriendsService | 75% |
| **Overall** | **78.8%** |

## 🎯 Best Practices

1. **AAA Pattern:** Arrange-Act-Assert
2. **Descriptive Names:** Test names mô tả rõ ràng
3. **Isolated Tests:** Mỗi test độc lập
4. **Mock Dependencies:** Sử dụng Moq cho external dependencies
5. **FluentAssertions:** Assertions dễ đọc và rõ ràng

## 📚 Tài liệu tham khảo

- [xUnit Documentation](https://xunit.net/)
- [Moq Documentation](https://github.com/moq/moq4)
- [FluentAssertions Documentation](https://fluentassertions.com/)
- [EF Core In-Memory Database](https://learn.microsoft.com/en-us/ef/core/providers/in-memory/)
