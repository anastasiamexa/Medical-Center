using Medical_Center.Models;
using Medical_Center.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace Medical_Center.Controllers
{
    public class AccountController : Controller
    {
        static string role = "";

        // Return Home page.
        public ActionResult Index()
        {
            TempData["message"] = role;
            return View();
        }

        //Return Register view
        public ActionResult Register()
        {
            return View();
        }

        //Return Register view
        public ActionResult RegisterDoctors()
        {
            return View();
        }

        //The form's data in Register view is posted to this method. 
        //We have binded the Register View with Register ViewModel, so we can accept object of Register class as parameter.
        //This object contains all the values entered in the form by the user.
        [HttpPost]
        public ActionResult SaveRegisterDetailsPatients(Register registerDetails)
        {
            //We check if the model state is valid or not. We have used DataAnnotation attributes.
            //If any form value fails the DataAnnotation validation the model state becomes invalid.
            if (ModelState.IsValid)
            {
                //create database context using Entity framework 
                using (var databaseContext = new medical_centerEntities())
                {
                    //If the model state is valid i.e. the form values passed the validation then we are storing the User's details in DB.
                    Patient reglog = new Patient();

                    //Save all details in RegitserUser object
                    reglog.patientAMKA = registerDetails.AMKA;
                    reglog.name = registerDetails.FirstName;
                    reglog.surname = registerDetails.LastName;
                    reglog.username = registerDetails.Username;
                    reglog.password = registerDetails.Password;

                    try
                    {
                        //Calling the SaveDetails method which saves the details.
                        databaseContext.Patient.Add(reglog);
                        databaseContext.SaveChanges();
                        ViewBag.Message = "User Details Saved";
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("Failure", "Couldn't save user to database !");
                    }
                }
                return View("Register");
            }
            else
            {

                //If the validation fails, we are returning the model object with errors to the view, which will display the error messages.
                return View("Register", registerDetails);
            }
        }

        //The form's data in Register view is posted to this method. 
        //We have binded the Register View with Register ViewModel, so we can accept object of Register class as parameter.
        //This object contains all the values entered in the form by the user.
        [HttpPost]
        public ActionResult SaveRegisterDetailsDoctors(RegisterDoctors registerDetails)
        {
            //We check if the model state is valid or not. We have used DataAnnotation attributes.
            //If any form value fails the DataAnnotation validation the model state becomes invalid.
            if (ModelState.IsValid)
            {
                //create database context using Entity framework 
                using (var databaseContext = new medical_centerEntities())
                {
                    //If the model state is valid i.e. the form values passed the validation then we are storing the User's details in DB.
                    Doctor reglog = new Doctor();

                    //Save all details in RegitserUser object
                    reglog.doctorAMKA = registerDetails.AMKA;
                    reglog.name = registerDetails.FirstName;
                    reglog.surname = registerDetails.LastName;
                    reglog.username = registerDetails.Username;
                    reglog.password = registerDetails.Password;
                    reglog.speciality = registerDetails.Speciality;
                    reglog.ADMIN_userid = Int32.Parse(registerDetails.Admin_id);


                    try
                    {
                        //Calling the SaveDetails method which saves the details.
                        databaseContext.Doctor.Add(reglog);
                        databaseContext.SaveChanges();
                        ViewBag.Message = "User Details Saved";
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("Failure", "Couldn't save user to database !");
                    }
                    
                }
                return View("RegisterDoctors");
            }
            else
            {

                //If the validation fails, we are returning the model object with errors to the view, which will display the error messages.
                return View("RegisterDoctors", registerDetails);
            }
        }


        public ActionResult LoginAdmin()
        {
            return View();
        }

        public ActionResult LoginDoctor()
        {
            return View();
        }

        public ActionResult LoginPatient()
        {
            return View();
        }

        //The login for admins form is posted to this method.
        [HttpPost]
        public ActionResult LoginAdmin(LoginViewModel model)
        {
            //Checking the state of model passed as parameter.
            if (ModelState.IsValid)
            {

                //Validating the user, whether the user is valid or not.
                var isValidUser = IsValidUser(model, "admin");
                Admin a = (Admin)IsValidUser(model, "admin");
                //If user is valid & present in database, we are redirecting it to Welcome page.
                if (isValidUser != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Username, false);
                    role = "admin";
                    TempData["message"] = role;
                    return RedirectToAction("Index");

                }
                else
                {
                    //If the username and password combination is not present in DB then error message is shown.
                    ModelState.AddModelError("Failure", "Wrong Username and password combination !");
                    return View();
                }
            }
            else
            {
                //If model state is not valid, the model with error message is returned to the View.
                return View(model);
            }
        }

        //The login for doctors form is posted to this method.
        [HttpPost]
        public ActionResult LoginDoctor(LoginViewModel model)
        {
            //Checking the state of model passed as parameter.
            if (ModelState.IsValid)
            {

                //Validating the user, whether the user is valid or not.
                var isValidUser = IsValidUser(model, "doctor");
                Doctor a = (Doctor)IsValidUser(model, "doctor");
                //If user is valid & present in database, we are redirecting it to Welcome page.
                if (isValidUser != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Username, false);
                    role = "doctor";
                    TempData["message"] = role;
                    return RedirectToAction("Index");
                }
                else
                {
                    //If the username and password combination is not present in DB then error message is shown.
                    ModelState.AddModelError("Failure", "Wrong Username and password combination !");
                   return View();
                }
            }
            else
            {
                //If model state is not valid, the model with error message is returned to the View.
                return View(model);
            }
        }

        //The login for patients form is posted to this method.
        [HttpPost]
        public ActionResult LoginPatient(LoginViewModel model)
        {
            //Checking the state of model passed as parameter.
            if (ModelState.IsValid)
            {

                //Validating the user, whether the user is valid or not.
                var isValidUser = IsValidUser(model, "patient");
                Patient a = (Patient)IsValidUser(model, "patient");
                //If user is valid & present in database, we are redirecting it to Welcome page.
                if (isValidUser != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Username, false);
                    role = "patient";
                    TempData["message"] = role;
                    return RedirectToAction("Index");
                }
                else
                {
                    //If the username and password combination is not present in DB then error message is shown.
                    ModelState.AddModelError("Failure", "Wrong Username and password combination !");
                    return View();
                }
            }
            else
            {
                //If model state is not valid, the model with error message is returned to the View.
                return View(model);
            }
        }

        //function to check if User is valid or not
        public Object IsValidUser(LoginViewModel model, string s)
        {
            using (var dataContext = new medical_centerEntities())
            {
                if (s == "admin")
                {
                    //Retireving the user details from DB based on username and password enetered by user.
                    Admin user = dataContext.Admin.Where(query => query.username.Equals(model.Username) && query.password.Equals(model.Password)).SingleOrDefault();
                    //If user is present, then true is returned.
                    if (user == null)
                        return null;
                    //If user is not present false is returned.
                    else
                        return user;
                }
                else if (s == "doctor")
                {
                    //Retireving the user details from DB based on username and password enetered by user.
                    Doctor user = dataContext.Doctor.Where(query => query.username.Equals(model.Username) && query.password.Equals(model.Password)).SingleOrDefault();
                    //If user is present, then true is returned.
                    if (user == null)
                        return null;
                    //If user is not present false is returned.
                    else
                        return user;
                }
                else if (s == "patient")
                {
                    //Retireving the user details from DB based on username and password enetered by user.
                    Patient user = dataContext.Patient.Where(query => query.username.Equals(model.Username) && query.password.Equals(model.Password)).SingleOrDefault();
                    //If user is present, then true is returned.
                    if (user == null)
                        return null;
                    //If user is not present false is returned.
                    else
                        return user;
                }
                return null;
            }
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}