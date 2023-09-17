using FolderStructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.IO;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace FolderStructure.Pages
{
    public class UploadModel : PageModel
    {
        private IHostingEnvironment _environment;
        private readonly FolderContext _context;

        public UploadModel(IHostingEnvironment environment, FolderContext context)
        {
            _environment = environment;
            _context = context;
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
                ExportData();
                ClearRows();
                ImportDataFromTxtFile(file);
            }
            // return RedirectToPage("/Index");
        }
        public void ImportDataFromTxtFile(string filePath)
        {
            try
            {
                var lines = System.IO.File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var data = line.Split(',');
                        var newItem = new Folder
                        {
                            Id = int.Parse(data[0]),
                            Name = data[1],
                            ParentId = int.Parse(data[2])
                        };
                        _context.Folder.Add(newItem);
                    _context.SaveChanges();
                }
                Message = "File uploaded successfully!";

            }
            catch
            {
                Message = "Oops, data in the file isn't correct";
            }
        }
        public void ExportData()
        {
            var dataToExport = _context.Folder.ToList();

            string exportFileName = "exported_data.txt";
            string filePath = Path.Combine(_environment.ContentRootPath, "uploads", exportFileName);

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var item in dataToExport)
                {
                    writer.WriteLine($"{item.Id},{item.Name},{item.ParentId}");
                }
            }
        }
        public void ClearRows()
        {
            var tableData = _context.Folder;
            tableData.RemoveRange(tableData);
            _context.SaveChanges();
        }

    }
}