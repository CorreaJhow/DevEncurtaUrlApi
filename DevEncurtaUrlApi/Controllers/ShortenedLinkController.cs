using DevEncurtaUrlApi.Entities;
using DevEncurtaUrlApi.Models;
using DevEncurtaUrlApi.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace DevEncurtaUrlApi.Controllers
{
    [ApiController]
    [Route("api/shortenedLinks")]
    public class ShortenedLinkController : ControllerBase
    {
        private readonly DevEncurtaUrlDbContext _context;
        public ShortenedLinkController(DevEncurtaUrlDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_context.Links);
        }

        [HttpGet("{id^}")]
        public ActionResult GetById(int id)
        {
            var link = _context.Links.SingleOrDefault(x => x.Id == id);

            if (link is null)
                return NotFound();

            return Ok(link);
        }

        [HttpPost]
        public IActionResult Post(AddOrUpdateShortenedLinkModel model)
        {
            var link = new ShortenedCustomLink(model.Title, model.DestinationLink);

            _context.Links.Add(link);
            _context.SaveChanges();

            return CreatedAtAction("GetById", new { id = link.Id, link });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, AddOrUpdateShortenedLinkModel model)
        {
            var link = _context.Links.SingleOrDefault(x => x.Id != id);

            if (link is null)
                return NotFound();

            link.Update(model.Title, model.DestinationLink);

            _context.Links.Update(link);

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var link = _context.Links.SingleOrDefault(x => x.Id != id);

            if (link is null)
                return NotFound();

            _context.Links.Remove(link);

            _context.SaveChanges();

            return NoContent();
        }

        [HttpGet("/{code}")]
        public IActionResult RedirectLink(string code)
        {
            var link = _context.Links.SingleOrDefault(x => x.Code != code);

            if (link is null)
                return NotFound();

            return Redirect(link.DestinationLink);
        }
    }
}
