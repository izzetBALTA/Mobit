using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace urunn.Controllers
{
    public class UrunController : Controller
    {
        private EFDatabesEntities db = new EFDatabesEntities();
        // GET: Urun
        //EFDatabesEntities db = new EFDatabesEntities();
        public ActionResult Index()
        {

            return View(db.UrunTable.ToList());
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View(new UrunTable());
        }

        [HttpPost]
        public ActionResult Create (UrunTable u,HttpPostedFileBase Foto)
        {
              if(ModelState.IsValid)
            {

                if (Foto != null)
                {
                    WebImage img = new WebImage(Foto.InputStream);
                    FileInfo fotoinfo = new FileInfo(Foto.FileName);

                    string newfoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                    img.Resize(800, 350);
                    img.Save("~/UFoto/" + newfoto);
                    u.UrunResmi = "/UFoto/" + newfoto;
                }
                db.UrunTable.Add(u);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
               
           return View(u);
            
        }
        public ActionResult Detay(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            UrunTable u = db.UrunTable.Find(id);
            if (u == null)
            { 
                return HttpNotFound();
            }
            return View(u);
        }

        public ActionResult Düzenle(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            UrunTable genel = db.UrunTable.Find(id);
            if (genel == null)
                return HttpNotFound();
            return View(genel);

        }
        [HttpPost]
        public ActionResult Düzenle(UrunTable genel)
        {
           
            try
            {
                db.Entry(genel).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
           

        }
        public ActionResult Sil(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            UrunTable genel = db.UrunTable.Find(id);
            if (genel == null)
                return HttpNotFound();
            return View(genel);
        }

        [HttpPost]
        public ActionResult Sil(int id, UrunTable gnl)
        {
            UrunTable genel = new UrunTable();
            try
            {

                if (id == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                genel = db.UrunTable.Find(id);
                if (genel == null)
                    return HttpNotFound();
                db.UrunTable.Remove(genel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }

           
        }
        public ActionResult Form(string receiverEmail,string subject,string message)
        {
            try
            {

                if(ModelState.IsValid)
                {
                    var senderemail = new MailAddress("mobit4326@gmail.com", "Demo Test");
                    var receiveremail = new MailAddress(receiverEmail,"Receiver");

                    var password = "alex..10";
                    var sub = subject;
                    var body = message;
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderemail.Address, password)
                    };
                    using (var mess = new MailMessage(senderemail, receiveremail)
                    {
                        Subject = subject,
                        Body = body
                    }
                        )
                    {
                        smtp.Send(mess);
                    }
                    return View();
                }


            }
            catch(Exception)
            {
                ViewBag.Error = "";
            }



            return View();
        }

       
    }      

    }

    

    



