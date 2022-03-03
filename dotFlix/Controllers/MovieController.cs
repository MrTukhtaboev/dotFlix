using dotFlix.Data;
using dotFlix.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace dotFlix.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MovieController : ControllerBase
    {
        private readonly MovieDbContext _dbContext;
        private readonly IWebHostEnvironment _env;
        public MovieController(MovieDbContext dbContext, IWebHostEnvironment env)
        {
            _dbContext =dbContext;
            _env = env;
        }

        [HttpPost]
        public async Task<IActionResult> SetImage(long movieId, IFormFile file)
        {
            var movie = await _dbContext.Movies
                                .Include(p => p.Author)
                                .FirstOrDefaultAsync(p => p.Id  == movieId);

            string fileExt = Path.GetExtension(file.FileName);
            string fileName = "Images/" + Guid.NewGuid() + fileExt;
            string path = Path.Combine(_env.WebRootPath, $"{fileName}");

            if (fileExt == ".png" || fileExt == ".jpg")
            {

                FileStream fileStream = System.IO.File.Open(path, FileMode.Create);
                await file.OpenReadStream().CopyToAsync(fileStream);

                //delete image
                if(!string.IsNullOrEmpty(movie.Image))
                {
                    System.IO.File.Delete(Path.Combine(_env.WebRootPath, movie.Image));
                }

                fileStream.Flush();
                fileStream.Close();
                movie.Image = fileName;
                
                await _dbContext.SaveChangesAsync();

                return Ok(movie);
            } 
            else
            {
                return BadRequest("Ishkal");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetImage(long movieId)
        {
            var movie = await _dbContext.Movies.FirstOrDefaultAsync(p => p.Id == movieId);

            if (movie is not null)
            {
                string path = Path.Combine(_env.WebRootPath, movie.Image);

                byte[] file = await System.IO.File.ReadAllBytesAsync(path);

                return File(file, "octet/stream", Path.GetFileName(path));
            }
            else
            {
                return NotFound("Topilmadi uzrrr");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            var movies = await _dbContext.Movies.Include(p => p.Author).ToListAsync();

            return Ok(movies);
        }

        [HttpPost]
        public async Task<IActionResult> PostMovie([FromBody]Movie movie)
        {
            var entry = await _dbContext.Movies.AddAsync(movie);

            await _dbContext.SaveChangesAsync();

            return Ok(entry.Entity);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMovie(Movie movie)
        {
            var entry = _dbContext.Movies.Update(movie);

            await _dbContext.SaveChangesAsync();

            return Ok(entry.Entity);
        }

        [HttpGet]
        public async Task<IActionResult> GetMovie(long id)
        {
            var entity = await _dbContext.Movies.FindAsync(id);

            return Ok(entity);
        }
    }
}
