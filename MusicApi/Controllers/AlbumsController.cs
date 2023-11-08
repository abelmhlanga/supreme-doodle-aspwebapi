using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MusicApi.Data;
using MusicApi.Helpers;
using MusicApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MusicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
        private ApiDbContext _dbContext;
        public AlbumsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //POST api/<AlbumsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Album album)
        {
            var imageUrl = await FileHelper.UploadImage(album.Image);
            album.ImageUrl = imageUrl;
            await _dbContext.Albums.AddAsync(album);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpGet]
        public async Task<IActionResult> GetAlbums(int? pageNumber, int? pageSize)
        {
            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 5;
        //    var albums = _dbContext.Albums;
        //    return Ok(albums);
        //use LINQ query to return only properties you need
            var albums = await (from album in _dbContext.Albums
            select new 
            {
                Id = album.Id,
                Name = album.Name,
                ImageUrl = album.ImageUrl
            }).ToListAsync();
            return Ok(albums.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> AlbumDetails(int albumId)
        {
            var albumDetails = await _dbContext.Albums.Where(a=>a.Id == albumId).Include(a=>a.Songs).ToListAsync();
            return Ok(albumDetails);

        }


    }
}