using InteractHub.Core.DTOs.Uploads;

namespace InteractHub.Core.Interfaces.Services;

public interface IFileUploadService
{
    Task<FileUploadResponseDto> UploadAvatarAsync(
        long currentUserId,
        UploadAvatarRequestDto dto,
        Stream fileStream,
        string fileName,
        string contentType,
        long fileSize
    );

    Task<FileUploadResponseDto> UploadCoverPhotoAsync(
        long currentUserId,
        UploadCoverPhotoRequestDto dto,
        Stream fileStream,
        string fileName,
        string contentType,
        long fileSize
    );

    Task<FileUploadResponseDto> UploadPostMediaAsync(
        long currentUserId,
        UploadPostMediaRequestDto dto,
        Stream fileStream,
        string fileName,
        string contentType,
        long fileSize
    );

    Task<FileUploadResponseDto> UploadStoryMediaAsync(
        long currentUserId,
        UploadStoryMediaRequestDto dto,
        Stream fileStream,
        string fileName,
        string contentType,
        long fileSize
    );
}
