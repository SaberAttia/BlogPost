using Blog.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Blog.API.Hubs;
using Blog.Services.Interfaces.Services.Article;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;

namespace Blog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("corsPolicy")]
    public class ArticlesController : ControllerBase
    {
        private readonly ILogger<ArticlesController> _logger;
        private readonly IArticleService _articleService;
        private readonly IHubContext<NotificationHub, ITypedHubClient> _hubContext;


        public ArticlesController(ILogger<ArticlesController> logger, IArticleService articleService, IHubContext<NotificationHub, ITypedHubClient> hubContext)
        {
            _logger = logger;
            _articleService = articleService;
            _hubContext = hubContext;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _articleService.GetAll();
            if (result.Result != null)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost]
        public IActionResult Insert(ArticleDto articleDto)
        {
            var result = _articleService.Insert(articleDto);

            if (result.Success)
            {
                _hubContext.Clients.All.BroadcastMessageAsync("Notification",
                    $"blog Has been added by {result.Data.Author}",
                    "http://localhost:4200/",
                    $"{result.Data.ImageUrl}");
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPut]
        public IActionResult Update(ArticleDto articleDto)
        {
            var result = _articleService.Edit(articleDto);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var returnData = _articleService.GetById(id);
                switch (returnData.Statuscode)
                {
                    case 200:
                        return Ok(returnData);
                    case 404:
                        return NotFound(returnData);
                    default:
                        return BadRequest(returnData);
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = _articleService.Delete(id);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}