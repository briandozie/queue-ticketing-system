using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TicketingDB.Models;

namespace TicketingDB.Controllers
{
    public class CountersController : ApiController
    {
        private TicketingDBEntities db = new TicketingDBEntities();

        // GET: api/Counters
        public IQueryable<Counter> GetCounters()
        {
            return db.Counters;
        }

        // GET: api/Counters/5
        [ResponseType(typeof(Counter))]
        public IHttpActionResult GetCounter(int id)
        {
            Counter counter = db.Counters.Find(id);
            if (counter == null)
            {
                return NotFound();
            }

            return Ok(counter);
        }

        // PUT: api/Counters/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCounter(int id, Counter counter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != counter.Id)
            {
                return BadRequest();
            }

            db.Entry(counter).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CounterExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Counters
        [ResponseType(typeof(Counter))]
        public IHttpActionResult PostCounter(Counter counter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Counters.Add(counter);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CounterExists(counter.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = counter.Id }, counter);
        }

        // DELETE: api/Counters/5
        [ResponseType(typeof(Counter))]
        public IHttpActionResult DeleteCounter(int id)
        {
            Counter counter = db.Counters.Find(id);
            if (counter == null)
            {
                return NotFound();
            }

            db.Counters.Remove(counter);
            db.SaveChanges();

            return Ok(counter);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CounterExists(int id)
        {
            return db.Counters.Count(e => e.Id == id) > 0;
        }
    }
}