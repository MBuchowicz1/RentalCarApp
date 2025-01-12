using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentalCarApp.Data;
using RentalCarApp.Models;

namespace RentalCarApp.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reservations
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Reservations.Include(r => r.Client).Include(r => r.Vehicle);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Reservations/Details/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Vehicle)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        [Authorize]
        // GET: Reservations/Create
        public IActionResult Create()
        {
            ViewData["ClientID"] = new SelectList(_context.Clients, "ID", "FullName");
            ViewData["VehicleID"] = new SelectList(_context.Vehicles, "ID", "DisplayName");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientID,VehicleID,StartDate,EndDate")] Reservation reservation)
        {
            if (reservation.EndDate <= reservation.StartDate)
            {
                ModelState.AddModelError("EndDate", "Data zakończenia musi być późniejsza niż data rozpoczęcia.");
            }

            var vehicle = await _context.Vehicles.FindAsync(reservation.VehicleID);
            if (vehicle == null)
            {
                ModelState.AddModelError("VehicleID", "Wybrany pojazd nie istnieje.");
                ViewData["ClientID"] = new SelectList(_context.Clients, "ID", "FullName", reservation.ClientID);
                ViewData["VehicleID"] = new SelectList(_context.Vehicles, "ID", "DisplayName", reservation.VehicleID);
                return View(reservation);
            }

            if (ModelState.IsValid)
            {
                int numberOfDays = (reservation.EndDate - reservation.StartDate).Days;
                reservation.TotalCost = vehicle.DailyPrice * numberOfDays;

                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ClientID"] = new SelectList(_context.Clients, "ID", "FullName", reservation.ClientID);
            ViewData["VehicleID"] = new SelectList(_context.Vehicles, "ID", "DisplayName", reservation.VehicleID);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["ClientID"] = new SelectList(_context.Clients, "ID", "ID", reservation.ClientID);
            ViewData["VehicleID"] = new SelectList(_context.Vehicles, "ID", "ID", reservation.VehicleID);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ClientID,VehicleID,StartDate,EndDate,TotalCost")] Reservation reservation)
        {
            if (id != reservation.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientID"] = new SelectList(_context.Clients, "ID", "ID", reservation.ClientID);
            ViewData["VehicleID"] = new SelectList(_context.Vehicles, "ID", "ID", reservation.VehicleID);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Vehicle)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.ID == id);
        }
    }
}
