using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims; // Add this namespace for ToListAsync()

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
    }
}
