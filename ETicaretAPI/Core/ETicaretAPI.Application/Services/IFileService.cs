﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Services
{
    public interface IFileService
    {
        Task<List<(string path, string name)>> UploadAsync(string path, IFormFileCollection files);
        Task<string> FileRenameAsync(string path);

        Task<bool> CopyAsync(IFormFile file, string path);

    }
}
