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
    public class SongsController : ControllerBase
    {
        private ApiDbContext _dbContext;
        public SongsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        //POST api/<SongsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Song song)
        {
            var imageUrl = await FileHelper.UploadImage(song.Image);
            song.ImageUrl = imageUrl;
            var audioUrl = await FileHelper.UploadFile(song.Audio);
            song.AudioUrl = audioUrl;
            song.UploadedDate = DateTime.Now;
            await _dbContext.Songs.AddAsync(song);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSongs(int? pageNumber, int? pageSize)
        {
            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 5;
            // var songs =_dbContext.Songs;
            // return Ok(songs);
            var songs = await (from song in _dbContext.Songs
                               select new
                               {
                Id = song.Id,
                Title = song.Title,
                Duration = song.Duration,
                ImageUrl = song.ImageUrl,
                AudioUrl = song.AudioUrl
            }).ToListAsync();
            return Ok(songs.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> FeaturedSongs()
        {
            // var songs =_dbContext.Songs;
            // return Ok(songs);
            var songs = await (from song in _dbContext.Songs
                                where song.IsFeatured == true
                               select new
                               {
                                   Id = song.Id,
                                   Title = song.Title,
                                   Duration = song.Duration,
                                   ImageUrl = song.ImageUrl,
                                   AudioUrl = song.AudioUrl
                                }).ToListAsync();
            return Ok(songs);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> NewSongs()
        {
            // var songs =_dbContext.Songs;
            // return Ok(songs);
            var songs = await (from song in _dbContext.Songs
                                orderby song.UploadedDate descending
                               select new
                               {
                                   Id = song.Id,
                                   Title = song.Title,
                                   Duration = song.Duration,
                                   ImageUrl = song.ImageUrl,
                                   AudioUrl = song.AudioUrl
                                }).Take(5).ToListAsync();
            return Ok(songs);
        }

                [HttpGet("[action]")]
        public async Task<IActionResult> SearchSong(string query)
        {
            // var songs =_dbContext.Songs;
            // return Ok(songs);
            var songs = await (from song in _dbContext.Songs
                                where song.Title.StartsWith(query)
                               select new
                               {
                                   Id = song.Id,
                                   Title = song.Title,
                                   Duration = song.Duration,
                                   ImageUrl = song.ImageUrl,
                                   AudioUrl = song.AudioUrl
                                }).Take(5).ToListAsync();
            return Ok(songs);
        }
    }
}