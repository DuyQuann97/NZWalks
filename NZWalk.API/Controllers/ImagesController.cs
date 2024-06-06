using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalk.API.Models.DTO;

namespace NZWalk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        //POST: api/Images/upload
        public async Task<IActionResult> Upload([FromForm] ImagesUploadRequestDto request) 
        {
            ValidateFileUpload(request);

            if (ModelState.IsValid)
            {
                //Use Repository to upload imgage
            }

            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImagesUploadRequestDto request) 
        {
            var allowedExtensions = new string[] { "jpg",".png"};

            if (allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("file","Unsupported file extension");
            }

            if (request.File.Length > 0)
            {
                ModelState.AddModelError("file", "File size more then 10mb , please upload a smaller size file ");
            }
        }

    }
}
