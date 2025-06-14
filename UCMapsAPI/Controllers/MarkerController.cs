using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims; // Add this namespace for ToListAsync()
using UCMapsAPI.Models.DTO;
using UCMapsAPI.Models;
using GoogleMapsComponents.Maps;

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
        public async Task<ActionResult<Marker>> PostMarkers(PostMarkerDto markerdto)
        {
            // Create a new Marker instance and map the properties from the DTO
            Marker marker = new Marker
            {
                Name = markerdto.Name,
                Description = markerdto.Description,
                Lat = markerdto.Lat,
                Lng = markerdto.Lng,
                StillThereCount = markerdto.StillThereCount,
                NotThereCount = markerdto.NotThereCount
            };

            // Assign the user based on the logged-in user ID
            marker.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == markerdto.UserId);

            // Add the marker to the context and save changes
            _context.Add(marker);
            await _context.SaveChangesAsync();

            // Retrieve the marker from the database to return it
            var markerSuccess = await _context.Marker
                .FirstOrDefaultAsync(p => p.Id == marker.Id);

            return Ok(markerSuccess);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateMarkers(UpdateMarkerDto markerdto)
        {
            Marker marker = new Marker
            {
                Name = markerdto.Name,
                Description = markerdto.Description,
                Lat = markerdto.Lat,
                Lng = markerdto.Lng,
                Id = markerdto.Id
            };
            marker.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == markerdto.UserId);
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

        [HttpPost("{id}/{userid}/vote")]
        public async Task<IActionResult> Vote(int id, int userId, [FromBody] bool voteDto)
        {
            var marker = await _context.Marker.FindAsync(id);
            if (marker == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var existingVote = await _context.Votes
                .FirstOrDefaultAsync(v => v.MarkerId == id && v.UserId == user.Id);

            if (existingVote != null)
            {
                return BadRequest("User has already voted for this marker.");
            }

            var vote = new Vote
            {
                MarkerId = id,
                User = user,
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
