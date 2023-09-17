using FolderStructure.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace FolderStructure.Pages
{
    public class IndexModel : PageModel
    {
        private readonly FolderContext _context;
        public Folder Folder { get; set; }
        public List<Folder> Folders { get; set; }
        public List<Folder> ChildFolders { get; set; }
        public IndexModel(FolderContext db)
        {
            _context = db;
        }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                id = 1;
            }
            Folder = _context.Folder.FirstOrDefault(f => f.Id == id);
            ChildFolders = _context.Folder
             .Where(f => f.ParentId == Folder.Id)
             .ToList();
            Folders = _context.Folder.ToList();
            return Page();
        }
    }
}