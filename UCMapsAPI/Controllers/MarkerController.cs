using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Add this namespace for ToListAsync()

namespace UCMapsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarkerController : Controller
    {
        private readonly SampleDBContext _context;
        public MarkerController(SampleDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Marker>>> GetMarkers()
        {
            return Ok(await _context.Marker.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult<Marker>> PostMarkers(Marker marker)
        {
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
