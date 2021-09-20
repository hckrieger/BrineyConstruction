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
using Microsoft.AspNetCore.Authorization;
using BrineyConstruction.Utility;

namespace BrineyConstruction.Pages.Projects
{
    [Authorize(Roles = SD.AdminEndUser)]
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
        public Project Project { get; set; }

        [BindProperty]
        public Image Image { get; set; }

        //[BindProperty]
        public IList<Project> CategoryList { get; set; }

        public async Task<IActionResult> OnGet()
        {
            CategoryList = await db.Projects.ToListAsync();
            return Page();
        }

        
        public async Task<IActionResult> OnPostImageAsync()
        {
            CategoryList = await db.Projects.ToListAsync();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Add the details of the Photo model to the database
            db.Images.Add(Image);
            await db.SaveChangesAsync();

            //Functionality for uploading image file to blob
            blobServiceClient = new BlobServiceClient(connectionString);
            string containerName = "photos" + Guid.NewGuid().ToString();

            containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);

            string localPath = "./projects/";
            string fileName = Image.Name;
            string localFilePath = Path.Combine(localPath, fileName);

            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(localFilePath, true);

            //Redirect to page when all is successfully uploaded
            return RedirectToPage("/Projects/Create");

        }

        public async Task<IActionResult> OnPostProjectAsync()
        {
            CategoryList = await db.Projects.ToListAsync();
            if (!ModelState.IsValid)
            {
                return Page();

            }

            db.Projects.Add(Project);
            await db.SaveChangesAsync();


            return RedirectToPage("/Projects/Create");

        }


    }


}
