using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BrineyConstruction.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BrineyConstruction.Models;
using Microsoft.EntityFrameworkCore;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace BrineyConstruction.Pages.Projects
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext db;

        string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");

        BlobServiceClient blobServiceClient;
        BlobContainerClient containerClient;

        public CreateModel(ApplicationDbContext db)
        {
            this.db = db;


        }

   

        [BindProperty]
        public Category Category { get; set; }

        [BindProperty]
        public Photo Photo { get; set; }

        [BindProperty]
        public IList<Category> CategoryList { get; set; }

        public async Task<IActionResult> OnGet()
        {
            CategoryList = await db.Categories.ToListAsync();
            return Page();
        }

        

        public async Task<IActionResult> OnPostPhoto()
        {
            if (!ModelState.IsValid)
            {
                return Page();

            }

            blobServiceClient = new BlobServiceClient(connectionString);
            string containerName = "photos" + Guid.NewGuid().ToString();

            containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);

            string localPath = $"./projects/";
            string fileName = Photo.Name;
            string localFilePath = Path.Combine(localPath, fileName);

            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(localFilePath, true);

            db.Photos.Add(Photo);
            await db.SaveChangesAsync();


            return RedirectToPage("/Photos/Create");

        }

        public async Task<IActionResult> OnPostCategory()
        {
            if (!ModelState.IsValid)
            {
                return Page();

            }

            db.Categories.Add(Category);
            await db.SaveChangesAsync();


            return RedirectToPage("/Photos/Create");

        }


    }


}
