using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Medical_Center.Models;
using Medical_Center.ViewModel;

namespace Medical_Center.Controllers
{
    public class AppointmentsController : Controller
    {
        private medical_centerEntities db = new medical_centerEntities();
        static string amka = "", amkap = "", username = "";

        // GET: Appointments
        public ActionResult Index()
        {
            // Get user's username
            username = TempData["message2"].ToString();
            // Get the data from Doctor table from DB and create List<Doctor> Doctor
            var Doctor = (from doctor in db.Doctor select doctor).ToList();
            // For the signed in doctor, get his AMKA
            for (int i = 0; i < Doctor.Count(); i++)
            {
                if (Doctor[i].username.ToString() == username)
                {
                    amka = Doctor[i].doctorAMKA.ToString();
                }
            }
            DateTime t = DateTime.Now.Date;
            // Show only the appointments of logged in doctor
            var appointment = db.Appointment.Include(a => a.Doctor).Include(a => a.Patient).Where(a => a.DOCTOR_doctorAMKA == amka).Where(a => a.date >= t).Where(a => a.isAvailable == false);
            return View(appointment.ToList());
        }

        // POST: Appointments
        [HttpPost]
        public ActionResult Index(DateTime tripstart, bool week = false)
        {
            // Get user's username
            username = TempData["message2"].ToString();
            // week is the value of the checkbox
            if (week) // if true then search for an additional 7 days
            { 
                DateTime w = tripstart.AddDays(7);
                // Show only the appointments of logged in doctor and specified week
                var appointment = db.Appointment.Include(a => a.Doctor).Include(a => a.Patient).Where(a => a.DOCTOR_doctorAMKA == amka).Where(a => a.date >= tripstart).Where(a => a.date <= w).Where(a => a.isAvailable == false);
                return View(appointment.ToList());
            }
            else // if false then search only for the specified day
            {
                // Show only the appointments of logged in doctor and specified date
                var appointment = db.Appointment.Include(a => a.Doctor).Include(a => a.Patient).Where(a => a.DOCTOR_doctorAMKA == amka).Where(a => a.date == tripstart).Where(a => a.isAvailable == false);
                return View(appointment.ToList());
            }
        }

        //GET: Appointments/Search
        public ActionResult SearchPatient(string doctorSpeciality)
        {
            var SpecialityLst = new List<string>();

            var SpecialityQry = from d in db.Doctor
                           orderby d.speciality
                           select d.speciality;

            SpecialityLst.AddRange(SpecialityQry.Distinct());
            ViewBag.doctorSpeciality = new SelectList(SpecialityLst);
      
            List<string> amkaLst = new List<string>();
            var Doctor = (from doctor in db.Doctor where doctor.speciality == doctorSpeciality select doctor).ToList();
            // For the signed in doctor, get his AMKA
            for (int i = 0; i < Doctor.Count(); i++)
            {
                amkaLst.Add(Doctor[i].doctorAMKA.ToString());
            }
            IQueryable<Appointment> q = Enumerable.Empty<Appointment>().AsQueryable();
            var app = new List<IQueryable<Appointment>>();
            string s, z;
            for (int i = 0; i < Doctor.Count(); i++)
            {
                s = amkaLst[i];
                app.Add(db.Appointment.Where(x => x.DOCTOR_doctorAMKA == s).Where(x => x.isAvailable == true));
                
                if (i > 0)
                {
                    for (int j = 0; j < app.Count() - 1; j++)
                    {
                        z = amkaLst[j];
                        IQueryable<Appointment> tmp = Enumerable.Empty<Appointment>().AsQueryable();
                        tmp = db.Appointment.Where(x => x.DOCTOR_doctorAMKA == z).Where(x => x.isAvailable == true);
                        app[j] = tmp;
                    }
                }
            }
            return View(app.DefaultIfEmpty(q).Aggregate((a, b) => a.Union(b)));
        }

        //GET: Appointments/SearchSaved
        public ActionResult SearchSavedPatient()
        {
            // Get user's username
            username = TempData["message2"].ToString();
            // Get the data from Doctor table from DB and create List<Doctor> Doctor
            var Patient = (from patient in db.Patient select patient).ToList();
            // For the signed in doctor, get his AMKA
            for (int i = 0; i < Patient.Count(); i++)
            {
                if (Patient[i].username.ToString() == username)
                {
                    amkap = Patient[i].patientAMKA.ToString();
                }
            }
            DateTime t = DateTime.Now.Date;
            // Show only the appointments of logged in doctor
            var appointment = db.Appointment.Include(a => a.Doctor).Include(a => a.Patient).Where(a => a.PATIENT_patientAMKA == amkap).Where(a => a.date < t).Where(a => a.isAvailable == false);
            return View(appointment.ToList());
        }

        //POST: Appointments/SearchSaved
        [HttpPost]
        public ActionResult SearchSavedPatient(DateTime d)
        {
            // Get user's username
            username = TempData["message2"].ToString();
            // Get the data from Doctor table from DB and create List<Doctor> Doctor
            var Patient = (from patient in db.Patient select patient).ToList();
            // For the signed in doctor, get his AMKA
            for (int i = 0; i < Patient.Count(); i++)
            {
                if (Patient[i].username.ToString() == username)
                {
                    amkap = Patient[i].patientAMKA.ToString();
                }
            }
            DateTime t = DateTime.Now.Date;
            // Show only the appointments of logged in doctor
            var appointment = db.Appointment.Include(a => a.Doctor).Include(a => a.Patient).Where(a => a.PATIENT_patientAMKA == amkap).Where(a => a.date < t).Where(a => a.date == d).Where(a => a.isAvailable == false);
            return View(appointment.ToList());
        }

        //GET: Appointments/SearchSavedFuture
        public ActionResult SearchSavedFuturePatient()
        {
            // Get user's username
            username = TempData["message2"].ToString();
            // Get the data from Doctor table from DB and create List<Doctor> Doctor
            var Patient = (from patient in db.Patient select patient).ToList();
            // For the signed in doctor, get his AMKA
            for (int i = 0; i < Patient.Count(); i++)
            {
                if (Patient[i].username.ToString() == username)
                {
                    amkap = Patient[i].patientAMKA.ToString();
                }
            }
            DateTime t = DateTime.Now.Date;
            // Show only the appointments of logged in doctor
            var appointment = db.Appointment.Include(a => a.Doctor).Include(a => a.Patient).Where(a => a.PATIENT_patientAMKA == amkap).Where(a => a.date >= t).Where(a => a.isAvailable == false);
            return View(appointment.ToList());
        }

        //POST: Appointments/SearchSavedFuture
        [HttpPost]
        public ActionResult SearchSavedFuturePatient(DateTime d)
        {
            // Get user's username
            username = TempData["message2"].ToString();
            // Get the data from Doctor table from DB and create List<Doctor> Doctor
            var Patient = (from patient in db.Patient select patient).ToList();
            // For the signed in doctor, get his AMKA
            for (int i = 0; i < Patient.Count(); i++)
            {
                if (Patient[i].username.ToString() == username)
                {
                    amkap = Patient[i].patientAMKA.ToString();
                }
            }
            DateTime t = DateTime.Now.Date;
            // Show only the appointments of logged in doctor
            var appointment = db.Appointment.Include(a => a.Doctor).Include(a => a.Patient).Where(a => a.PATIENT_patientAMKA == amkap).Where(a => a.date >= t).Where(a => a.date == d).Where(a => a.isAvailable == false);
            return View(appointment.ToList());
        }

        // GET: Appointments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointment.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // GET: Appointments/Create
        public ActionResult Create()
        {
            // Get user's username
            username = TempData["message2"].ToString();
            // Get the data from Doctor table from DB and create List<Doctor> Doctor
            var Doctor = (from doctor in db.Doctor select doctor).ToList();
            // For the signed in doctor, get his AMKA
            for (int i=0; i< Doctor.Count(); i++)
            {
                if (Doctor[i].username.ToString() == username)
                {
                    amka = Doctor[i].doctorAMKA.ToString();
                }
            }
            TempData["message1"] = amka; // Send AMKA to the Create page
            ViewBag.DOCTOR_doctorAMKA = new SelectList(db.Doctor, "doctorAMKA", "doctorAMKA");
            ViewBag.PATIENT_patientAMKA = new SelectList(db.Patient, "patientAMKA", "patientAMKA");
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "appid,date,startSlotTime,endSlotTime,PATIENT_patientAMKA,DOCTOR_doctorAMKA,isAvailable")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                DateTime now = DateTime.Now.Date;
                if (appointment.date >= now && appointment.startSlotTime < appointment.endSlotTime)
                {
                    // Get the data from Appointment table from DB and create List<Appointment> App
                    var App = (from app in db.Appointment select app).ToList();

                    for (int i = 0; i < App.Count(); i++)
                    {
                        if (App[i].date == appointment.date && App[i].startSlotTime == appointment.startSlotTime)
                        {
                            //The appointment already exists
                            TempData["message1"] = amka; // Send AMKA to the Create page
                            ModelState.AddModelError("Failure", "The appointment already exists !");
                            return View();
                        }
                    }

                    db.Appointment.Add(appointment);
                    db.SaveChanges();
                }
                else if (appointment.date < now)
                {
                    //If the date has passed
                    TempData["message1"] = amka; // Send AMKA to the Create page
                    ModelState.AddModelError("Failure", "Future dates are only accepted !");
                    return View();
                }
                else if (appointment.startSlotTime >= appointment.endSlotTime)
                {
                    //If the end time is less than start time
                    TempData["message1"] = amka; // Send AMKA to the Create page
                    ModelState.AddModelError("Failure", "Start time must be less than end time !");
                    return View();
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Appointments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointment.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }

            // Get user's username
            username = TempData["message2"].ToString();
            // Get the data from Patient table from DB and create List<Patient> Patient
            var Patient = (from patient in db.Patient select patient).ToList();
            // For the signed in doctor, get his AMKA
            for (int i = 0; i < Patient.Count(); i++)
            {
                if (Patient[i].username.ToString() == username)
                {
                    amkap = Patient[i].patientAMKA.ToString();
                }
            }
            TempData["message1"] = amkap; // Send AMKA to the Edit page

            ViewBag.DOCTOR_doctorAMKA = new SelectList(db.Doctor, "doctorAMKA", "doctorAMKA", appointment.DOCTOR_doctorAMKA);
            ViewBag.PATIENT_patientAMKA = new SelectList(db.Patient, "patientAMKA", "patientAMKA", appointment.PATIENT_patientAMKA);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "appid,date,startSlotTime,endSlotTime,PATIENT_patientAMKA,DOCTOR_doctorAMKA,isAvailable")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Index", "Account", new { area = "" });
            }
            ViewBag.DOCTOR_doctorAMKA = new SelectList(db.Doctor, "doctorAMKA", "doctorAMKA", appointment.DOCTOR_doctorAMKA);
            ViewBag.PATIENT_patientAMKA = new SelectList(db.Patient, "patientAMKA", "patientAMKA", appointment.PATIENT_patientAMKA);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointment.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([Bind(Include = "appid,date,startSlotTime,endSlotTime,PATIENT_patientAMKA,DOCTOR_doctorAMKA,isAvailable")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Index", "Account", new { area = "" });
            }
            ViewBag.DOCTOR_doctorAMKA = new SelectList(db.Doctor, "doctorAMKA", "doctorAMKA", appointment.DOCTOR_doctorAMKA);
            ViewBag.PATIENT_patientAMKA = new SelectList(db.Patient, "patientAMKA", "patientAMKA", appointment.PATIENT_patientAMKA);
            return View(appointment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
