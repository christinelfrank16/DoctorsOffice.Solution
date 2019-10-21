using Microsoft.AspNetCore.Mvc;
using DoctorsOffice.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace DoctorsOffice.Controllers
{
  [Authorize]
  public class PatientsController : Controller
  {
    private readonly DoctorsOfficeContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public PatientsController(DoctorsOfficeContext db, UserManager<ApplicationUser> userManager)
    {
      _db = db;
      _userManager = userManager;
    }

    public async Task<ActionResult> Index()
    {
        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var currentUser = await _userManager.FindByIdAsync(userId);
        var userPatients = _db.Patients.Where(entry => entry.User.Id == currentUser.Id);
        return View(userPatients);
    }

    public ActionResult Create()
    {
      ViewBag.DoctorId = new SelectList(_db.Doctors.ToList(), "DoctorId", "WholeName");
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Patient patient, int DoctorId)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      patient.User = currentUser;
      _db.Patients.Add(patient);
      if(DoctorId != 0)
      {
        _db.DoctorPatient.Add(new DoctorPatient() { DoctorId = DoctorId, PatientId = patient.PatientId });
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      Patient thisPatient = _db.Patients
      .Include(patient => patient.Doctors)
      .ThenInclude(join => join.Doctor)
      .FirstOrDefault(patients => patients.PatientId == id);
      return View(thisPatient);
    }

    public ActionResult Edit(int id)
    {
      var thisPatient = _db.Patients.FirstOrDefault(patients => patients.PatientId == id);
      ViewBag.DoctorId = new SelectList(_db.Doctors, "DoctorId", "Name");
      return View(thisPatient);
    }

    [HttpPost]
    public ActionResult Edit(Patient patient, int DoctorId)
    {
      if (DoctorId != 0)
      {
        _db.DoctorPatient.Add(new DoctorPatient() { DoctorId =DoctorId, PatientId = patient.PatientId });
      }
      _db.Entry(patient).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddDoctor(int id)
    {
        var thisPatient = _db.Patients.FirstOrDefault(patients => patients.PatientId == id);
        ViewBag.DoctorId = new SelectList(_db.Doctors, "DoctorId", "WholeName");
        return View(thisPatient);
    }

    [HttpPost]
    public ActionResult AddDoctor(Patient patient, int DoctorId)
    {
        if (DoctorId != 0)
        {
        _db.DoctorPatient.Add(new DoctorPatient() { DoctorId = DoctorId, PatientId = patient.PatientId });
        }
        _db.SaveChanges();
        return RedirectToAction("Index");
    }


    public ActionResult Delete(int id)
    {
      var thisPatient = _db.Patients.FirstOrDefault(patients => patients.PatientId == id);
      return View(thisPatient);
    }

    [HttpPost]
    public ActionResult DeleteDoctor(int joinId)
    {
        var joinEntry = _db.DoctorPatient.FirstOrDefault(entry => entry.DoctorPatientId == joinId);
        _db.DoctorPatient.Remove(joinEntry);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisPatient = _db.Patients.FirstOrDefault(patients => patients.PatientId == id);
      _db.Patients.Remove(thisPatient);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}