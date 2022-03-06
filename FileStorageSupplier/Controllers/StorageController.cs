using FileStorageSupplier.Requests;
using FileStorageSupplier.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FileStorageSupplier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;

        public StorageController(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        [HttpPost("upload-single")]
        public async Task<IActionResult> UploadFile([FromForm]UploadFileRequest request)
        {
            string url = await _fileStorageService.UploadFileAsync(request.File);
            return Ok(url);
        }
        [HttpPost("upload-multiple")]
        public async Task<IActionResult> UploadFile([FromForm]UploadMultipleFileRequest request)
        {
            string url = await _fileStorageService.UploadFileAsync(request.File);
            return Ok(url);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteFile([FromQuery]string path)
        {
            await _fileStorageService.RemoveFileAsync(path);
            return Ok();
        }
    }
}
