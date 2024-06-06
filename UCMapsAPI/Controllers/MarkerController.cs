using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims; // Add this namespace for ToListAsync()
using UCMapsAPI.Models.DTO;
using UCMapsAPI.Models;

namespace UCMapsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarkerController : Controller
    {
        private readonly SampleDBContext _context;
        public IHttpContextAccessor _httpContextAccessor { get; }
        public MarkerController(SampleDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User
        .FindFirstValue(ClaimTypes.NameIdentifier)!); //finds userId

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Marker>>> GetMarkers()
        {
            var markers = await _context.Marker.Include(m => m.User).ToListAsync();
            return Ok(markers);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Marker>> PostMarkers(Marker marker)
        {
            marker.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            _context.Add(marker);
            await _context.SaveChangesAsync();
            var markerSuccess = await _context.Marker
                .FirstOrDefaultAsync(p => p.Id.Equals(marker.Id));
            return Ok(markerSuccess);
        }
        [HttpPut]
        public async Task<ActionResult> UpdateMarkers(Marker marker)
        {
            var markerSuccess = await _context.Marker
                .FirstOrDefaultAsync(p => p.Id.Equals(marker.Id));
            if (markerSuccess != null)
            {
                markerSuccess.Name = marker.Name;
                markerSuccess.Lng = marker.Lng;
                markerSuccess.Lat = marker.Lat;
                await _context.SaveChangesAsync();
            }
            return Ok(markerSuccess);
        }

        [HttpPost("{id}/vote")]
        public async Task<IActionResult> Vote(int id, [FromBody] bool voteDto)
        {
            var marker = await _context.Marker.FindAsync(id);
            if (marker == null)
            {
                return NotFound();
            }

            var userId = GetUserId();

            var existingVote = await _context.Votes
                .FirstOrDefaultAsync(v => v.MarkerId == id && v.UserId == userId);

            if (existingVote != null)
            {
                return BadRequest("User has already voted for this marker.");
            }

            var vote = new Vote
            {
                MarkerId = id,
                UserId = userId,
                IsStillThere = voteDto
            };

            _context.Votes.Add(vote);

            if (voteDto)
            {
                marker.StillThereCount++;
            }
            else
            {
                marker.NotThereCount++;
            }

            await _context.SaveChangesAsync();

            return Ok(marker);
        }

        [HttpGet("{id}/user-vote-status")]
        public async Task<IActionResult> GetUserVoteStatus(int id, [FromQuery] string userId)
        {
            var hasVoted = await _context.Votes
                .AnyAsync(v => v.MarkerId == id && v.UserId.ToString() == userId);

            return Ok(hasVoted);
        }
    }
}
