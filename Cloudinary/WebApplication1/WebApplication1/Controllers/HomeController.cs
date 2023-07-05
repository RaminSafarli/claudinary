using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApplication1.Models;
using WebApplication1.Models.Entities;
using static System.Net.Mime.MediaTypeNames;

public class HomeController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly AppDbContext db;

    public HomeController(IConfiguration configuration, AppDbContext db)
    {
        _configuration = configuration;
        this.db = db;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Products()
    {
        var entity = await db.Products.ToListAsync();

        return View(entity);
    }

    [HttpPost]
    public async Task<IActionResult> UploadImage( Product product)
    {
        var cloudinary = new Cloudinary(
            new Account(
                _configuration["Cloudinary:CloudName"],
                _configuration["Cloudinary:ApiKey"],
                _configuration["Cloudinary:ApiSecret"]
            )
        );

        var file = Request.Form.Files[0];
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, file.OpenReadStream()),
            Folder = "demo" 
        };

        var uploadResult = cloudinary.Upload(uploadParams);

        var imageUrl = uploadResult.SecureUri.ToString();

        var entity = new Product
        {
            Name = product.Name,
            Url = uploadResult.SecureUri.ToString()
        };
        db.Products.Add(entity);
        await db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
