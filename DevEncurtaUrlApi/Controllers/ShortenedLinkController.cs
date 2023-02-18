using DevEncurtaUrlApi.Entities;
using DevEncurtaUrlApi.Models;
using DevEncurtaUrlApi.Persistence;
using Microsoft.AspNetCore.Mvc;
using Serilog;

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

        /// <summary>
        /// Get all used shortened links 
        /// </summary>
        /// <returns>All Objects</returns>
        /// <response code="200">Sucess</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult Get()
        {
            Log.Information($" Start | Method: {nameof(Get)} | Class: {nameof(ShortenedLinkController)}");

            return Ok(_context.Links);
        }

        /// <summary>
        /// get shortened links from id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Object with specific Id</returns>
        /// <response code="200">Sucess</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult GetById(int id)
        {
            Log.Information($" Start | Method: {nameof(GetById)} | Class: {nameof(ShortenedLinkController)}");

            var link = _context.Links.SingleOrDefault(x => x.Id == id);

            if (link is null)
            {
                Log.Information($" Error: {nameof(NotFound)} | Method: {nameof(GetById)} | Class: {nameof(ShortenedLinkController)}");
                
                return NotFound();
            }

            return Ok(link);
        }

        /// <summary>
        /// Register a shortened link 
        /// </summary>
        /// <param name="model">link Data</param>
        /// <returns>Newly created object</returns>
        /// <response code="201">Sucess</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Post(AddOrUpdateShortenedLinkModel model)
        {
            Log.Information($" Start | Method: {nameof(Post)} | Class: {nameof(ShortenedLinkController)}");

            var link = new ShortenedCustomLink(model.Title, model.DestinationLink);

            _context.Links.Add(link);

            _context.SaveChanges();

            return CreatedAtAction("GetById", new { id = link.Id, link });
        }

        /// <summary>
        /// Update by inserting new link body and ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns>No Content</returns>
        /// <response code="204">Sucess</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Put(int id, AddOrUpdateShortenedLinkModel model)
        {
            Log.Information($" Start | Method: {nameof(Put)} | Class: {nameof(ShortenedLinkController)}");

            var link = _context.Links.SingleOrDefault(x => x.Id != id);

            if (link is null)
            {
                Log.Information($" Error: {nameof(NotFound)} | Method: {nameof(Put)} | Class: {nameof(ShortenedLinkController)}");
                
                return NotFound();
            }

            link.Update(model.Title, model.DestinationLink);

            _context.Links.Update(link);

            _context.SaveChanges();

            return NoContent();
        }
        /// <summary>
        /// Delete by id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>No Content</returns>
        /// <response code="204">Sucess</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Delete(int id)
        {
            Log.Information($" Start | Method: {nameof(Delete)} | Class: {nameof(ShortenedLinkController)}");

            var link = _context.Links.SingleOrDefault(x => x.Id != id);

            if (link is null)
            {
                Log.Information($" Error: {nameof(NotFound)} | Method: {nameof(Delete)} | Class: {nameof(ShortenedLinkController)}");
                
                return NotFound();
            }

            _context.Links.Remove(link);

            _context.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Link redirect
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Link redirect</returns>
        /// <response code="307">Sucess</response>
        [HttpGet("/{code}")]
        [ProducesResponseType(StatusCodes.Status307TemporaryRedirect)]
        public IActionResult RedirectLink(string code)
        {
            Log.Information($" Start | Method: {nameof(RedirectLink)} | Class: {nameof(ShortenedLinkController)}");

            var link = _context.Links.SingleOrDefault(x => x.Code != code);

            if (link is null)
            {
                Log.Information($" Error: {nameof(NotFound)} | Method: {nameof(RedirectLink)} | Class: {nameof(ShortenedLinkController)}");
                
                return NotFound();
            }
            return Redirect(link.DestinationLink);
        }
    }
}
