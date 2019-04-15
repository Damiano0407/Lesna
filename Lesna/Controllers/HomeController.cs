using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Lesna.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult WyslijEmail(string email, string telefon, string tresc)
        {

            string emailklienta = email;
            string emaillesnej = "lesnadolinaa@gmail.com";
            string templatedoklienta = "doklienta";
            string templatedolesnej = "dolesnej";
            string tytuldlaklienta = "Leśna Dolina otrzymała Twoją wiadomość!";
            string tytuldlalesnej = "Nowa wiadomość ze strony www.leśnadolinazlotystok.pl !";
            string wiadomoscdoklienta = "Witaj. Twoja wiadomość została wysłana do Leśnej Doliny . Wkrótce ktoś się z Tobą skontaktuje!";
            string wiadomoscsdolesnej = "Witaj. Skorzystano z formularza. Dane wysyłającego: <br/> e-mail: " + email + "<br/> telefon: "+ telefon + "<br/> Treść wiadomości : <br/> "  + tresc ;
            BuildEmailTemplate(emailklienta,templatedoklienta,tytuldlaklienta,wiadomoscdoklienta);

            BuildEmailTemplate(emaillesnej, templatedolesnej, tytuldlalesnej, wiadomoscsdolesnej);

            return Json(new { Wynik = "OK" });
        }

        public void BuildEmailTemplate(string email, string template ,string tytul, string wiadomosc)
        {
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + template + ".html");
            body = body.Replace("@ViewBag.wiadomosc", wiadomosc);
            BuildEmailTemplateX(tytul, body, email);
        }

        public void BuildEmailTemplateX(string subjectText, string bodyText, string sendTo)
        {
            string from, to, bcc, cc, subject, body;
            from = "formularz@lesnadolinazlotystok.pl";
            to = sendTo;
            bcc = "";
            cc = "";
            subject = subjectText;
            StringBuilder sb = new StringBuilder();
            sb.Append(bodyText);
            body = sb.ToString();
            MailMessage email = new MailMessage();
            email.From = new MailAddress(from);
            email.To.Add(new MailAddress(to));
            if (!string.IsNullOrEmpty(bcc))
            {
                email.Bcc.Add(new MailAddress(bcc));
            }
            if (!string.IsNullOrEmpty(cc))
            {
                email.CC.Add(new MailAddress(cc));
            }
            email.Subject = subject;
            email.Body = body;
            email.IsBodyHtml = true;
            SendEmail(email);

        }

        public void SendEmail(MailMessage email)
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.webio.pl";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new System.Net.NetworkCredential("formularz@lesnadolinazlotystok.pl", "lesna23%");
            try
            {
                client.Send(email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}