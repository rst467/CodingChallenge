using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CodingChallenge.Models;

namespace CodingChallenge.Controllers
{
    //[Route("api/[controller]/[action]")]
    [Route("[action]")]
    [ApiController]
    public class DeliveryTrucksController : ControllerBase
    {
        private readonly DeliveryTruckContext _context;
        
        //für den Zugriff auf die appsettings.json
        private readonly IConfiguration _configuration;

        public DeliveryTrucksController(DeliveryTruckContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;//für den Zugriff auf die appsettings.json
        }

        // GET: api/DeliveryTrucks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryTruck>>> GetDeliveryTrucks()
        {
            return await _context.DeliveryTrucks.ToListAsync();
        }

        // GET: api/DeliveryTrucks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveryTruck>> GetDeliveryTruck(long id)
        {
            var deliveryTruck = await _context.DeliveryTrucks.FindAsync(id);

            if (deliveryTruck == null)
            {
                return NotFound();
            }

            return deliveryTruck;
        }

        // PUT: api/DeliveryTrucks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeliveryTruck(long id, DeliveryTruck deliveryTruck)
        {
            if (id != deliveryTruck.Id)
            {
                return BadRequest();
            }

            _context.Entry(deliveryTruck).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeliveryTruckExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/DeliveryTrucks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DeliveryTruck>> PostDeliveryTruck(DeliveryTruck deliveryTruck)
        {
            _context.DeliveryTrucks.Add(deliveryTruck);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetDeliveryTruck", new { id = deliveryTruck.Id }, deliveryTruck);
            return CreatedAtAction(nameof(GetDeliveryTruck), new { id = deliveryTruck.Id }, deliveryTruck);
        }

        // DELETE: api/DeliveryTrucks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeliveryTruck(long id)
        {
            var deliveryTruck = await _context.DeliveryTrucks.FindAsync(id);
            if (deliveryTruck == null)
            {
                return NotFound();
            }

            _context.DeliveryTrucks.Remove(deliveryTruck);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //*******************************************************************************************************

        [HttpGet("{distance}")]
        public int GetDeliveryPrice(int distance)
        {
            var pricePerKm = _configuration.GetValue<int>("MySettings:PricePerKm");

            return distance * pricePerKm;
        }

        [HttpGet("{startAddress},{destinationAddress}")]
        public int GetDeliveryDistance(int startAddress, int destinationAddress)
        {
            //Distanz berechnen
            //an dieser Stelle sollte idealerweise eine eigebundene c# API die Distanz ermitteln
            
            //irgendein Rückgabewert
            return startAddress + destinationAddress;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryTruck>>> GetFreeDeliveryTruck()
        {
            //Kriterium 1. frei nach "aktueller timeStamp" - deliveryTruck.timeStamp >= 2 Stunden
            //Annahme: Zustellgeschwindigkeit in der Stadt konstant bei 50 km/h
            //Kriterium 2. frei nach deliveryTruck.distanz / Zustellgeschwindigkeit >=2 Stunden
            //return einen Truck für die Kriterium 1 oder 2 erfüllt sind oder 0 für sonstiges.

            return await _context.DeliveryTrucks.ToListAsync();
            //durchlaufe await _context.DeliveryTrucks.ToListAsync();

        }

        [HttpPut]
        public async Task<IActionResult> BookFreeTruck()
        {
            //einen freien Truck ermitteln und für die Fahrt buchen (DeliveryFor, Distance und Timestamp setzen)
            var freeDeliveryTruck = await _context.DeliveryTrucks.ToListAsync();

            return NoContent();
        }

        private bool DeliveryTruckExists(long id)
        {
            return _context.DeliveryTrucks.Any(e => e.Id == id);
        }
    }
}
