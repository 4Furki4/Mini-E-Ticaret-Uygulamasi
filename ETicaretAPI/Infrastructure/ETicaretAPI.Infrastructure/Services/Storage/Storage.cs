using ETicaretAPI.Application.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services.Storage
{
    public class Storage
    {

        protected delegate bool HasFile(string fileName, string pathOrContainer);
        protected async Task<string> FileRenameAsync(string pathOrContainer, HasFile hasFile ,string fileName)
        {
            return await Task.Run<string>(() =>
            {
                string oldName = Path.GetFileNameWithoutExtension(fileName);
                string extension = Path.GetExtension(fileName);
                string newFileName = $"{NameOperation.NameRegulatory(oldName)}{extension}";
                bool fileIsExists = false;
                int fileIndex = 0;
                do
                {
                    if (hasFile(newFileName, pathOrContainer))
                    {
                        fileIsExists = true;
                        fileIndex++;
                        newFileName = $"{NameOperation.NameRegulatory(oldName + "-" + fileIndex)}{extension}";
                    }
                    else
                    {
                        fileIsExists = false;
                    }
                } while (fileIsExists);

                return newFileName;
            });



        }
    }
}
