# HƯỚNG DẪN CHẠY TESTS - INTERACTHUB

## 📋 Yêu cầu

- .NET 8.0 SDK
- Terminal/Command Prompt

## 🚀 Các cách chạy tests

### Cách 1: Chạy tất cả tests (Đơn giản nhất)

```bash
dotnet test
```

**Kết quả mong đợi:**
```
Passed!  - Failed:     0, Passed:    30, Skipped:     0, Total:    30
```

---

### Cách 2: Chạy với output chi tiết

```bash
dotnet test --logger "console;verbosity=detailed"
```

**Hiển thị:**
- Tên từng test case
- Thời gian chạy từng test
- Kết quả chi tiết

---

### Cách 3: Chạy tests trong một project cụ thể

```bash
dotnet test InteractHub.Tests/InteractHub.Tests.csproj
```

---

### Cách 4: Chạy tests theo filter

#### Chạy tất cả tests trong một class

```bash
# Chạy tất cả tests trong PostsServiceTests
dotnet test --filter "FullyQualifiedName~PostsServiceTests"

# Chạy tất cả tests trong AuthServiceTests
dotnet test --filter "FullyQualifiedName~AuthServiceTests"

# Chạy tất cả tests trong PostsControllerTests
dotnet test --filter "FullyQualifiedName~PostsControllerTests"
```

#### Chạy một test method cụ thể

```bash
# Chạy test CreatePost_ReturnsPostDto_WhenValid
dotnet test --filter "FullyQualifiedName~CreatePost_ReturnsPostDto_WhenValid"

# Chạy test Login_ReturnsToken_WhenCredentialsValid
dotnet test --filter "FullyQualifiedName~Login_ReturnsToken_WhenCredentialsValid"
```

#### Chạy tests theo category

```bash
# Chạy tất cả Service tests
dotnet test --filter "FullyQualifiedName~Services"

# Chạy tất cả Controller tests
dotnet test --filter "FullyQualifiedName~Controllers"
```

---

### Cách 5: Chạy với Code Coverage

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

**Kết quả:** Tạo file `coverage.opencover.xml` với thông tin coverage

---

### Cách 6: Sử dụng script (Linux/Mac)

```bash
# Cấp quyền thực thi (chỉ cần làm 1 lần)
chmod +x run-tests.sh

# Chạy script
./run-tests.sh
```

---

## 📊 Hiểu kết quả tests

### Kết quả thành công

```
Test Run Successful.
Total tests: 30
     Passed: 30
     Failed: 0
 Total time: 2.8025 Seconds
```

**Ý nghĩa:**
- ✅ Tất cả 30 tests đều passed
- ❌ Không có test nào failed
- ⏱️ Tổng thời gian: ~2.8 giây

### Kết quả có lỗi (ví dụ)

```
Test Run Failed.
Total tests: 30
     Passed: 28
     Failed: 2
```

**Cách xử lý:**
1. Xem chi tiết lỗi trong output
2. Kiểm tra test case bị failed
3. Fix code hoặc fix test
4. Chạy lại tests

---

## 🔍 Debug tests

### Chạy một test cụ thể để debug

```bash
dotnet test --filter "FullyQualifiedName~CreatePost_ReturnsPostDto_WhenValid" --logger "console;verbosity=detailed"
```

### Xem stack trace khi test failed

```bash
dotnet test --logger "console;verbosity=diagnostic"
```

---

## 📁 Cấu trúc Test Project

```
InteractHub.Tests/
├── Services/
│   ├── AuthServiceTests.cs          # 3 tests
│   ├── PostsServiceTests.cs         # 4 tests
│   └── FriendsServiceTests.cs       # 3 tests
├── Controllers/
│   └── PostsControllerTests.cs      # 18 tests
├── UnitTest1.cs                     # 2 basic tests
├── README.md                        # Documentation
├── TEST_CASES.md                    # Danh sách test cases
└── LATEX_SUMMARY.md                 # Tóm tắt cho báo cáo
```

**Tổng cộng: 30 test cases**

---

## 🎯 Test Cases Summary

| Category | Test Count | Status |
|----------|------------|--------|
| AuthService | 3 | ✅ All Pass |
| PostsService | 4 | ✅ All Pass |
| FriendsService | 3 | ✅ All Pass |
| PostsController | 18 | ✅ All Pass |
| Basic Tests | 2 | ✅ All Pass |
| **Total** | **30** | **✅ 100%** |

---

## 💡 Tips

### 1. Chạy tests nhanh hơn

```bash
# Skip build nếu đã build rồi
dotnet test --no-build

# Chạy tests song song
dotnet test --parallel
```

### 2. Xem danh sách tests

```bash
dotnet test --list-tests
```

### 3. Chạy tests với timeout

```bash
dotnet test --blame-hang-timeout 30s
```

### 4. Xuất kết quả ra file

```bash
dotnet test --logger "trx;LogFileName=test-results.trx"
```

---

## ❓ Troubleshooting

### Lỗi: "No test is available"

**Nguyên nhân:** Project chưa được build

**Giải pháp:**
```bash
dotnet build InteractHub.Tests/InteractHub.Tests.csproj
dotnet test InteractHub.Tests/InteractHub.Tests.csproj
```

### Lỗi: "The test source file ... could not be found"

**Nguyên nhân:** Đang ở sai thư mục

**Giải pháp:**
```bash
# Di chuyển về thư mục root của project
cd /path/to/interacthub-backend
dotnet test
```

### Tests chạy chậm

**Nguyên nhân:** In-Memory Database hoặc nhiều tests

**Giải pháp:**
```bash
# Chạy song song
dotnet test --parallel

# Hoặc chạy từng nhóm tests
dotnet test --filter "FullyQualifiedName~Services"
```

---

## 📚 Tài liệu tham khảo

- [xUnit Documentation](https://xunit.net/)
- [dotnet test Command](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-test)
- [Test Filtering](https://learn.microsoft.com/en-us/dotnet/core/testing/selective-unit-tests)

---

## 📞 Hỗ trợ

Nếu gặp vấn đề khi chạy tests:

1. Kiểm tra .NET SDK version: `dotnet --version`
2. Xem log chi tiết: `dotnet test --logger "console;verbosity=diagnostic"`
3. Xem file `InteractHub.Tests/README.md` để biết thêm chi tiết
4. Xem file `TEST_CASES.md` để biết danh sách test cases

---

**Cập nhật:** 2026-05-03  
**Version:** 1.0.0  
**Status:** ✅ All 30 tests passing
