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
    public class ArtistsController : ControllerBase
    {
        private ApiDbContext _dbContext;
        public ArtistsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext; 
        }

        //POST api/<ArtistsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Artist artist)
        {
            var imageUrl = await FileHelper.UploadImage(artist.Image);
            artist.ImageUrl = imageUrl;
            await _dbContext.Artists.AddAsync(artist);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        //api/artist
        [HttpGet]
        public async Task<IActionResult> GetArtist(int? pageNumber, int? pageSize)
        {
            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 5;
            // var artists =_dbContext.Artists;
            // return Ok(artists);
            var artists = await (from artist in _dbContext.Artists
                                select new
            {
                Id = artist.Id,
                Name = artist.Name,
                ImageUrl = artist.ImageUrl
            }).ToListAsync();
            return Ok(artists.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));


        }
        
        [HttpGet("[action]")]
        public async Task<IActionResult> ArtistDetails(int artistId)
        {
            var artistDetails = await _dbContext.Artists.Where(a=>a.Id == artistId).Include(a=>a.Songs).ToListAsync();
            return Ok(artistDetails);

        }
    }
}