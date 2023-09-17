using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace FolderStructure.Pages
{
    public class UploadModel : PageModel
    {
        private IHostingEnvironment _environment;
        public UploadModel(IHostingEnvironment environment)
        {
            _environment = environment;
        }
        [BindProperty]
        public string Message { get; set; }
        public IFormFile UploadFile { get; set; }
        public async Task OnPostAsync()
        {
            if (UploadFile != null && UploadFile.Length > 0)
            {
                var file = Path.Combine(_environment.ContentRootPath, "uploads", UploadFile.FileName);
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await UploadFile.CopyToAsync(fileStream);
                }
                Message = "File uploaded successfully!";
            }
           // return RedirectToPage("/Index");
        }

    }
}