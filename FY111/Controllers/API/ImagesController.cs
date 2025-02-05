﻿using FY111.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FY111.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private SignInManager<FY111User> _signInManager;
        private readonly IWebHostEnvironment _env;

        public ImagesController(
            SignInManager<FY111User> signInManager, IWebHostEnvironment hostingEnv)
        {
            _signInManager = signInManager;
            _env = hostingEnv;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles(IFormCollection data, IList<IFormFile> files)
        {
            if (_signInManager.IsSignedIn(User))
            {
                foreach (IFormFile source in files)
                {
                    var fileName = source.FileName;
                    var dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image\\" + data["dirPath"]);
                    var filePath = Path.Combine(dirPath, fileName);
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }

                    using var image = Image.Load(source.OpenReadStream());
                    image.Mutate(x => x.Resize(0, 480));
                    image.Save(filePath);
                    return Ok(new { success = true });
                }
            }
            return BadRequest(new { success = false }); 
        }

    }
}
