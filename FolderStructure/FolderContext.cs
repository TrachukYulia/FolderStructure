using FolderStructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FolderStructure
{
    public class FolderContext : DbContext
    {
        public virtual DbSet<Folder> Folder { get; set; }
        public FolderContext(DbContextOptions<FolderContext> options) : base(options)
        {
        }
    }
}
