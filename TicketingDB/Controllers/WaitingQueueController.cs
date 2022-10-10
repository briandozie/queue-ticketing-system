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
    public class WaitingQueueController : ApiController
    {
        private TicketingDBEntities db = new TicketingDBEntities();

        // GET: api/WaitingQueue
        public IQueryable<WaitingQueue> GetWaitingQueues()
        {
            return db.WaitingQueues;
        }

        // GET: api/WaitingQueue/5
        [ResponseType(typeof(WaitingQueue))]
        public IHttpActionResult GetWaitingQueue(int id)
        {
            WaitingQueue waitingQueue = db.WaitingQueues.Find(id);
            if (waitingQueue == null)
            {
                return NotFound();
            }

            return Ok(waitingQueue);
        }

        // PUT: api/WaitingQueue/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutWaitingQueue(int id, WaitingQueue waitingQueue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != waitingQueue.TicketNum)
            {
                return BadRequest();
            }

            db.Entry(waitingQueue).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WaitingQueueExists(id))
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

        // POST: api/WaitingQueue
        [ResponseType(typeof(WaitingQueue))]
        public IHttpActionResult PostWaitingQueue(WaitingQueue waitingQueue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.WaitingQueues.Add(waitingQueue);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (WaitingQueueExists(waitingQueue.TicketNum))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = waitingQueue.TicketNum }, waitingQueue);
        }

        // DELETE: api/WaitingQueue/5
        [ResponseType(typeof(WaitingQueue))]
        public IHttpActionResult DeleteWaitingQueue(int id)
        {
            WaitingQueue waitingQueue = db.WaitingQueues.Find(id);
            if (waitingQueue == null)
            {
                return NotFound();
            }

            db.WaitingQueues.Remove(waitingQueue);
            db.SaveChanges();

            return Ok(waitingQueue);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WaitingQueueExists(int id)
        {
            return db.WaitingQueues.Count(e => e.TicketNum == id) > 0;
        }
    }
}