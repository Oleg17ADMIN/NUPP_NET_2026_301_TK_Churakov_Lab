using Microsoft.AspNetCore.Mvc;
using LibrarySystem.Infrastructure.Models;
using LibrarySystem.Infrastructure.Services;
using LibrarySystem.REST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibrarySystem.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Шлях: api/publishers
    public class PublishersController : ControllerBase
    {
        private readonly LibraryServiceAsync<PublisherModel> _publisherService;

        public PublishersController(LibraryServiceAsync<PublisherModel> publisherService)
        {
            _publisherService = publisherService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublisherDto>>> GetPublishers()
        {
            var publishers = await _publisherService.ReadAllAsync();
            var dtos = publishers.Select(p => new PublisherDto
            {
                Id = p.Id,
                Name = p.Name
            });
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<ActionResult<PublisherDto>> PostPublisher(PublisherDto dto)
        {
            var publisher = new PublisherModel
            {
                Id = Guid.NewGuid(),
                Name = dto.Name
            };

            await _publisherService.CreateAsync(publisher);
            dto.Id = publisher.Id;

            return CreatedAtAction(nameof(GetPublishers), new { id = dto.Id }, dto);
        }
    }
}