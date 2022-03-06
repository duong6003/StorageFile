namespace FileStorageSupplier.Requests
{
    public class UploadFileRequest
    {
        public IFormFile? File { get; set; }
    }
    public class UploadMultipleFileRequest
    {
        public IEnumerable<IFormFile>? File { get; set; }
    }
}
