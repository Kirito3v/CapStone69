using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELECTROVIA_AI
{
    public class SearchWithImageService
    {
        private readonly PyTimeService PythonService;

        public static bool Wait = false;
        public SearchWithImageService(PyTimeService pythonService)
        {
            PythonService = pythonService;
        }

        public async Task<string> SearchWithImageAsync(IFormFile imageFile)
        {


            if (imageFile == null || imageFile.Length == 0)
                return "";

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var url = Path.Combine(uploadsFolder, imageFile.FileName);

            using (var stream = new FileStream(url, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }


            PythonService.PyMeth("ImageSearchFromGenerator");

            PythonService.PyMeth("AllInOne");


            await Task.Delay(10);

            PythonService.PyMeth(url);

            Wait = true;

            await waituntilProcessEnd();

            await Task.Delay(10);

            var result = PythonService.LastMessage;

            PythonService._logger.LogInformation(result);

            return result;

        }

        public async Task waituntilProcessEnd()
        {
            while (Wait)
            {
                await Task.Delay(10);
            }
        }
    }
}
