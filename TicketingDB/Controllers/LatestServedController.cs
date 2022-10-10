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
    public class LatestServedController : ApiController
    {
        private TicketingDBEntities db = new TicketingDBEntities();

        // GET: api/LatestServed
        public IQueryable<LatestServed> GetLatestServeds()
        {
            return db.LatestServeds;
        }

        // GET: api/LatestServed/5
        [ResponseType(typeof(LatestServed))]
        public IHttpActionResult GetLatestServed(int id)
        {
            LatestServed latestServed = db.LatestServeds.Find(id);
            if (latestServed == null)
            {
                return NotFound();
            }

            return Ok(latestServed);
        }

        // PUT: api/LatestServed/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLatestServed(int id, LatestServed latestServed)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != latestServed.TicketNum)
            {
                return BadRequest();
            }

            db.Entry(latestServed).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LatestServedExists(id))
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

        // POST: api/LatestServed
        [ResponseType(typeof(LatestServed))]
        public IHttpActionResult PostLatestServed(LatestServed latestServed)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.LatestServeds.Add(latestServed);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (LatestServedExists(latestServed.TicketNum))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = latestServed.TicketNum }, latestServed);
        }

        // DELETE: api/LatestServed/5
        [ResponseType(typeof(LatestServed))]
        public IHttpActionResult DeleteLatestServed(int id)
        {
            LatestServed latestServed = db.LatestServeds.Find(id);
            if (latestServed == null)
            {
                return NotFound();
            }

            db.LatestServeds.Remove(latestServed);
            db.SaveChanges();

            return Ok(latestServed);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LatestServedExists(int id)
        {
            return db.LatestServeds.Count(e => e.TicketNum == id) > 0;
        }
    }
}