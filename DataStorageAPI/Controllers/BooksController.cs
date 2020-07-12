using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DataStorageAPI.Providers;
using DataStorageAPI.Models;
using Microsoft.Extensions.Configuration;
using DataStorageAPI.Infrastructure;
using System.Threading.Tasks;
using System.Linq;

namespace DataStorageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<Book> _repository;

        public BooksController(IConfiguration config)
        {
            _configuration = config;
            _repository = new BookRepository(_configuration.GetConnectionString("PostgreSQLConnection"));
        }

        // GET: api/books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Guid>>> Get()
        {
            return Ok(await _repository.GetAllIDs());
        }

        // GET: api/books/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult<Book>> Get(string id)
        {
            Guid guid = Guid.Parse(id);
            Book book = await _repository.GetById(guid);

            if (book != null)
            {
                return Ok(book);
            } else
            {
                return NotFound();
            }
        }

        // POST: api/books
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Book book)
        {
            if (book != null)
            {
                await _repository.Add(book);

                return Ok();
            } else
            {
                return BadRequest();
            }
        }

        // PUT: api/books/5
        [HttpPut("{id}")]
        public async void Put(string id, [FromBody] Book book)
        {
            Guid guid = Guid.Parse(id);
            await _repository.Update(guid, book);
        }

        // DELETE: api/books/5
        [HttpDelete("{id}")]
        public async void Delete(string id)
        {
            Guid guid = Guid.Parse(id);
            await _repository.Remove(guid);
        }
    }
}
