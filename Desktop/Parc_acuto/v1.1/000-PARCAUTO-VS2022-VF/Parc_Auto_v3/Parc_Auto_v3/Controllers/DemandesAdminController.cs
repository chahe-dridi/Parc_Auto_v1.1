using Microsoft.AspNetCore.Mvc;
using Parc_Auto_v3.Models;
using Parc_Auto_v3.Services;
using System.Threading.Tasks;

using System.IO;
using System.Threading.Tasks;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using Microsoft.AspNetCore.Hosting;
 
using Microsoft.AspNetCore.Authorization;
using PdfSharpCore.Drawing.Layout;

using OfficeOpenXml;
using OfficeOpenXml.Style;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using CsvHelper.Configuration;
using CsvHelper;
using System.Text;
using System.Globalization;
using Humanizer;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Parc_Auto_v3.Utils;


namespace Parc_Auto_v3.Controllers
{
    [Authorize(Roles = "admin,superadmin")]
 


 
    public class DemandesAdminController : Controller
    {
        private readonly IDemandesService _demandesService;
        private readonly IVoitureService _voitureService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;


        public DemandesAdminController(IWebHostEnvironment webHostEnvironment, IDemandesService demandesService, IVoitureService voitureService  , ApplicationDbContext context)
        {
            _demandesService = demandesService;
            _voitureService = voitureService;
            _webHostEnvironment = webHostEnvironment;
            _context = context;

        }

     

        public IActionResult Index()
        {
            // Retrieve and include the Voiture navigation property
            var demandes = _context.Demandes
                .Include(d => d.Voiture)  // Ensure the Voiture navigation property is included
                .OrderByDescending(d => d.Id)  // Sorting by Id in descending order
                .ToList();

            return View(demandes);
        }







        public async Task<IActionResult> Details(int id)
        {
            var demande = await _demandesService.GetDemandeByIdAsync(id);
            if (demande == null)
            {
                return NotFound();
            }

           

            ViewBag.AvailableVoitures = await _voitureService.GetAvailableVoituresByTypeAsync(demande.TypeVoiture);
            return View("Details", demande);




            return View(demande);
        }








       
        public async Task<IActionResult> SearchDemandes(string searchString, DateTime? startDate, DateTime? endDate)
        {
            IQueryable<Demandes> demandesQuery = _context.Demandes.Include(d => d.Voiture);

            if (!string.IsNullOrEmpty(searchString))
            {
                demandesQuery = demandesQuery.Where(d => d.IdEmploye.Contains(searchString) || (d.Voiture != null && d.Voiture.Matricule.Contains(searchString)));
            }

            if (startDate.HasValue && endDate.HasValue)
            {
                demandesQuery = demandesQuery.Where(d => d.DateDepart >= startDate.Value && d.DateDepart <= endDate.Value);
            }

            var demandes = await demandesQuery.OrderByDescending(d => d.Id).ToListAsync();

            return PartialView("_DemandesListPartial", demandes);
        }




        [HttpPost]
        public async Task<IActionResult> ConfirmAcceptance(int id, int? voitureId, string action)
        {
            var demande = await _demandesService.GetDemandeByIdAsync(id);
            if (demande == null)
            {
                return NotFound();
            }
          
            if (action == "Accept")
            {
                if (!voitureId.HasValue)
                {
                    // Return an error message if no car is selected
                    ModelState.AddModelError("VoitureId", "Veuillez sélectionner une voiture pour accepter la demande.");

                    // Filter available cars based on the type of car in the demande and availability
                    ViewBag.AvailableVoitures = await _voitureService.GetAvailableVoituresByTypeAsync(demande.TypeVoiture);
                    return View("Details", demande); // Return to the same view with the error message
                }
                
                demande.Etat = "Accepted"; // Update the state to Accepted
                demande.VoitureId = voitureId; // Link the selected car

                var voiture = await _voitureService.GetVoitureByIdAsync(voitureId.Value);
                if (voiture != null)
                {
                    voiture.Disponibilite = "Reserved"; // Update the availability status
                    await _voitureService.UpdateVoitureAsync(voiture);
                }
            }
            else if (action == "Refuse")
            {
                demande.Etat = "Refused"; // Update the state to Refused
                demande.VoitureId = null; // Unlink any car
            }

            await _demandesService.UpdateDemandeAsync(demande);
            return RedirectToAction(nameof(Index)); // Redirect after successful update
        }
 
         
        public async Task<IActionResult> Edit(int id)
        {
            var demande = await _demandesService.GetDemandeByIdAsync(id);
            if (demande == null)
            {
                return NotFound();
            }

            // Fetch available cars based on current demande state
            var availableVoitures = await _voitureService.GetAvailableVoituresByTypeAsync(demande.TypeVoiture);

            // Fetch the currently assigned voiture if it's not available
            if (demande.VoitureId.HasValue)
            {
                var selectedVoiture = await _voitureService.GetVoitureByIdAsync(demande.VoitureId.Value);
                if (selectedVoiture != null && !availableVoitures.Contains(selectedVoiture))
                {
                    availableVoitures = availableVoitures.Append(selectedVoiture).ToList();
                }
            }

            ViewBag.AvailableVoitures = availableVoitures;

            return View(demande);
        }

 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Demandes updatedDemande)
        {
            if (id != updatedDemande.Id)
            {
                return BadRequest();
            }

           // if (ModelState.IsValid)
            {
                try
                {
                    // Fetch the existing demande
                    var existingDemande = await _demandesService.GetDemandeByIdAsync(id);

                    // Update all fields
                    existingDemande.Nom = updatedDemande.Nom;
                    existingDemande.Prenom = updatedDemande.Prenom;
                    existingDemande.IdEmploye = updatedDemande.IdEmploye;
                    existingDemande.AffectationDepartement = updatedDemande.AffectationDepartement;
                    existingDemande.TypeVoiture = updatedDemande.TypeVoiture;
                    existingDemande.Destination = updatedDemande.Destination;
                    existingDemande.DateDepart = updatedDemande.DateDepart;
                    existingDemande.DateArrivee = updatedDemande.DateArrivee;
                    existingDemande.Description = updatedDemande.Description;
                    existingDemande.Mission = updatedDemande.Mission;
                    existingDemande.Kilometrage = updatedDemande.Kilometrage;
                    if (updatedDemande.Accompagnateur== null) { updatedDemande.Accompagnateur = " "; }
                    existingDemande.Accompagnateur = updatedDemande.Accompagnateur;

                    // Handle Etat and VoitureId updates
                    if (updatedDemande.Etat == "Accepted")
                    {
                        if (updatedDemande.VoitureId != null)
                        {
                            existingDemande.VoitureId = updatedDemande.VoitureId;

                            // Update car availability
                            var voiture = await _voitureService.GetVoitureByIdAsync(updatedDemande.VoitureId.Value);
                            if (voiture != null)
                            {
                                voiture.Disponibilite = "Reserved";
                                await _voitureService.UpdateVoitureAsync(voiture);
                            }
                        }
                        existingDemande.Etat = "Accepted";
                    }
                    else if (updatedDemande.Etat == "En attente")
                    {
                        existingDemande.Etat = "En attente";
                        existingDemande.VoitureId = null; // Clear car assignment
                    }
                    else
                    {
                        existingDemande.Etat = "Refused";
                        existingDemande.VoitureId = null; // Clear car assignment
                    }

                    await _demandesService.UpdateDemandeAsync(existingDemande);
                }
                catch (Exception ex)
                {
                    // Handle potential errors
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }

                return RedirectToAction(nameof(Index));
            }

            // Re-populate the list of available cars in case of validation failure
            var availableVoitures = await _voitureService.GetAvailableVoituresByTypeAsync(updatedDemande.TypeVoiture);
            ViewBag.AvailableVoitures = availableVoitures;

            return View(updatedDemande);
        }


        //pdf for one demande



        public async Task<IActionResult> DownloadPdf(int id)
        {
            /*var demande = await _demandesService.GetDemandeByIdAsync(id);
            if (demande == null)
            {
                return NotFound();
            }

            var pdf = new PdfDocument();
            var page = pdf.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            var fontBold = new XFont("Verdana", 12, XFontStyle.Bold);
            var fontTitle = new XFont("Verdana", 14, XFontStyle.Bold);
            var fontContent = new XFont("Verdana", 12, XFontStyle.Regular);


            var photoPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "Logo_Banque_de_Tunisie.png"); // Adjust the image file name if necessary
            var photo = XImage.FromFile(photoPath);

            // Set photo position and size
            var photoWidth = 100; // Adjust as needed
            var photoHeight = 75; // Adjust as needed
            var photoX = (page.Width - photoWidth) / 2;
            var photoY = 60; // Adjust based on the vertical spacing you need

            gfx.DrawImage(photo, photoX, photoY, photoWidth, photoHeight);



            // Top Left Corner
            gfx.DrawString("Direction des Moyens Généraux", fontBold, XBrushes.Black, new XRect(40, 40, page.Width - 80, 20), XStringFormats.TopLeft);

            // Underline for "Division PARC AUTOS"
            var divisionText = "Division PARC AUTOS";
            var divisionSize = gfx.MeasureString(divisionText, fontBold);
            var divisionX = 40;
            var divisionY = 60;
            gfx.DrawString(divisionText, fontBold, XBrushes.Black, new XRect(divisionX, divisionY, page.Width - 80, 20), XStringFormats.TopLeft);
            gfx.DrawLine(XPens.Black, divisionX, divisionY + 20, divisionX + divisionSize.Width, divisionY + 20);

            // Center the photo between "Division PARC AUTOS" and "ORDRE DE MISSION"
            var todayDate = DateTime.Now.ToString("dd/MM/yyyy");
            gfx.DrawString($"Tunis le : {todayDate}", fontBold, XBrushes.Black, new XRect(page.Width - 200, 40, 160, 20), XStringFormats.TopLeft);

            int yPosition = photoY + photoHeight + 60; // Starting y position for details
            int columnWidth = 200; // Width of each column


            // Middle - Centered Title with Underline
            var titleText = "ORDRE DE MISSION";
            var titleSize = gfx.MeasureString(titleText, fontTitle);
            var titleX = (page.Width - titleSize.Width) / 2; // Center the text horizontally
            gfx.DrawString(titleText, fontTitle, XBrushes.Black, new XRect(titleX, photoY + photoHeight + 20, titleSize.Width, titleSize.Height), XStringFormats.TopLeft);

            // Underline for "ORDRE DE MISSION"
            gfx.DrawLine(XPens.Black, titleX, photoY + photoHeight + 40, titleX + titleSize.Width, photoY + photoHeight + 40);

            // Structured Details
       
            // Define content labels and values
            var details = new (string label, string value)[]
            {
            ("Objet:", "Autorisation"),
            ("Destination:", demande.Destination),
            ("Mission:", demande.Mission),
            ("Voiture de service:", demande.Voiture?.Matricule),
            ("Date et/Ou Horaire:", $"{demande.DateDepart:dd/MM/yyyy} - {demande.DateArrivee:dd/MM/yyyy}"),
             ("Utilisateur:",$"{demande.Prenom} {demande.Nom}"),
             ("Accompagnateur:",$"{demande.Accompagnateur}")

            };

            // Draw each label and value in columns
            for (int i = 0; i < details.Length; i++)
            {
                gfx.DrawString(details[i].label, fontBold, XBrushes.Black, new XRect(40, yPosition + (i * 40), columnWidth, 20), XStringFormats.TopLeft);
                gfx.DrawString(details[i].value, fontContent, XBrushes.Black, new XRect(40 + columnWidth, yPosition + (i * 40), page.Width - 80 - columnWidth, 20), XStringFormats.TopLeft);
            }

            // Bottom
            
            gfx.DrawString("CHEF DE PARC", fontBold, XBrushes.Black, new XRect(page.Width - 160, yPosition + (details.Length * 40) + 40, 160, 20), XStringFormats.TopLeft);

            // Underline for "CHEF DE PARC"
            var chefDeParcSize = gfx.MeasureString("CHEF DE PARC", fontBold);
            var chefDeParcX = page.Width - 160;
            gfx.DrawLine(XPens.Black, chefDeParcX, yPosition + (details.Length * 40) + 60, chefDeParcX + chefDeParcSize.Width, yPosition + (details.Length * 40) + 60);
            */

            /* string _MailTo = HelperDonGen.MailTo;; string _MailFrom = HelperDonGen.MailFrom; string _msg = "";
             string _MailBody = ""; 
             string _MailServer = HelperDonGen.MailServer; string _MailSubject = "MISSION "; string _MailToRappGlobal = "";
             //BTDevinSSISService@BT.COM.TN	10.120.2.1	ESSID.SAMI@BT.COM.TN
             StringBuilder sb = new StringBuilder();
             _MailBody = "** Vous Trouvez ci-joint Ordre Mission :  " + demande.Prenom + " " + demande.Nom + Environment.NewLine;
             //"** Nombre Total   :  " + _LogList.Count() + Environment.NewLine;


             string _LigneBody = "";


             sb.Append("** Vous Trouvez ci-joint Ordre Mission :  " + demande.Prenom + " " + demande.Nom + Environment.NewLine);
             sb.Append(Environment.NewLine);
             sb.Append("</table><br/> Meilleures Salutations  <br/>  Direction Moyens Generaux BT ");
             //_MailBody += "</table>";

             _MailBody = sb.ToString();





            */
            var demande = await _demandesService.GetDemandeByIdAsync(id);
            if (demande == null)
            {
                return NotFound();
            }

            var pdf = new PdfDocument();
            var page = pdf.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var tf = new XTextFormatter(gfx);

            var fontBold = new XFont("Verdana", 12, XFontStyle.Bold);
            var fontTitle = new XFont("Verdana", 14, XFontStyle.Bold);
            var fontContent = new XFont("Verdana", 12, XFontStyle.Regular);

            var photoPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "Logo_Banque_de_Tunisie.png"); // Adjust the image file name if necessary
            var photo = XImage.FromFile(photoPath);

            // Set photo position and size
            var photoWidth = 100; // Adjust as needed
            var photoHeight = 75; // Adjust as needed
            var photoX = (page.Width - photoWidth) / 2;
            var photoY = 60; // Adjust based on the vertical spacing you need

            gfx.DrawImage(photo, photoX, photoY, photoWidth, photoHeight);

            // Top Left Corner
            gfx.DrawString("Direction des Moyens Généraux", fontBold, XBrushes.Black, new XRect(40, 40, page.Width - 80, 20), XStringFormats.TopLeft);

            // Underline for "Division PARC AUTOS"
            var divisionText = "Division PARC AUTOS";
            var divisionSize = gfx.MeasureString(divisionText, fontBold);
            var divisionX = 40;
            var divisionY = 60;
            gfx.DrawString(divisionText, fontBold, XBrushes.Black, new XRect(divisionX, divisionY, page.Width - 80, 20), XStringFormats.TopLeft);
            gfx.DrawLine(XPens.Black, divisionX, divisionY + 20, divisionX + divisionSize.Width, divisionY + 20);

            // Center the photo between "Division PARC AUTOS" and "ORDRE DE MISSION"
            var todayDate = DateTime.Now.ToString("dd/MM/yyyy");
            gfx.DrawString($"Tunis le : {todayDate}", fontBold, XBrushes.Black, new XRect(page.Width - 200, 40, 160, 20), XStringFormats.TopLeft);

            int yPosition = photoY + photoHeight + 60; // Starting y position for details
            int columnWidth = 200; // Width of each column

            // Middle - Centered Title with Underline
            var titleText = "ORDRE DE MISSION";
            var titleSize = gfx.MeasureString(titleText, fontTitle);
            var titleX = (page.Width - titleSize.Width) / 2; // Center the text horizontally
            gfx.DrawString(titleText, fontTitle, XBrushes.Black, new XRect(titleX, photoY + photoHeight + 20, titleSize.Width, titleSize.Height), XStringFormats.TopLeft);

            // Underline for "ORDRE DE MISSION"
            gfx.DrawLine(XPens.Black, titleX, photoY + photoHeight + 40, titleX + titleSize.Width, photoY + photoHeight + 40);

            // Structured Details
            // Define content labels and values
            var details = new (string label, string value)[]
            {
        ("Objet:", "Autorisation"),
         
        ("Destination:", demande.Destination),
        ("Mission:", demande.Mission),
        ("Voiture de service:", demande.Voiture?.Matricule ?? "N/A"),
        ("Date et/Ou Horaire:", $"{demande.DateDepart:dd/MM/yyyy} - {demande.DateArrivee:dd/MM/yyyy}"),
        ("Utilisateur:", $"{demande.Prenom} {demande.Nom}"),
        ("Accompagnateur:", demande.Accompagnateur)
            };

            // Draw each label and value in columns
            for (int i = 0; i < details.Length; i++)
            {
                gfx.DrawString(details[i].label, fontBold, XBrushes.Black, new XRect(40, yPosition, columnWidth, 20), XStringFormats.TopLeft);
                var rect = new XRect(40 + columnWidth, yPosition, page.Width - 80 - columnWidth, double.MaxValue);
                tf.DrawString(details[i].value, fontContent, XBrushes.Black, rect, XStringFormats.TopLeft);
                var textHeight = MeasureTextHeight(gfx, details[i].value, fontContent, page.Width - 80 - columnWidth);
                yPosition += (int)textHeight + 20; // Adjust yPosition based on text height
            }

            // Bottom
            gfx.DrawString("CHEF DE PARC", fontBold, XBrushes.Black, new XRect(page.Width - 160, yPosition + 40, 160, 20), XStringFormats.TopLeft);

            // Underline for "CHEF DE PARC"
            var chefDeParcSize = gfx.MeasureString("CHEF DE PARC", fontBold);
            var chefDeParcX = page.Width - 160;
            gfx.DrawLine(XPens.Black, chefDeParcX, yPosition + 60, chefDeParcX + chefDeParcSize.Width, yPosition + 60);



            /*   using (var stream = new MemoryStream())
                                                                                                                            {
                                                                                                                                pdf.Save(stream, false);
                                                                                                                                var fileBytes = stream.ToArray();
                                                                                                                                 Attachment _MailAttachement1 = new Attachment(new MemoryStream(fileBytes), "Mission_" + demande.IdEmploye + "_" + DateTime.Now+ ".pdf");

                                                                                                                               if (MailLibrary.CreateMail(_MailServer, _MailFrom, _MailTo, _MailSubject, _MailBody, _MailAttachement1, ref _msg, true) == false)
                                                             {
                                                                                                                                     var ss = "echec"; 
                                                        }


                                                                                                                            return File(fileBytes, "application/pdf", "DemandeDetails.pdf");
                                                               }*/


            using (var stream = new MemoryStream())
            {
                pdf.Save(stream, false);
                var fileBytes = stream.ToArray();
                return File(fileBytes, "application/pdf", "DemandeDetails.pdf");
            }


        }

        private double MeasureTextHeight(XGraphics gfx, string text, XFont font, double width)
        {
            var size = gfx.MeasureString(text, font);
            return size.Height;
        }

        public static bool fEnvoiMailRembAnticipe(int _journee, string _DISqlConnexion, string _user, ref string _message)
        {
            Boolean result = false; string vUserName = ""; int verr = 0; string vmsg = "";

            int Nberr = 0; int NberrFiles = 0; result = false;

            string vreq = ""; string ss = ""; string ss1 = ""; string ss0 = "";
            try
            {

                string _MailTo = "ESSID.SAMI@BT.COM.TN;elyes.cherif@bt.com.tn"; string _MailFrom = "BTMOYENGENERAUX@BT.COM.TN"; string _msg = ""; 
                string _MailBody = ""; Attachment _MailAttachement = null; 
                string _MailServer = "10.120.2.1"; string _MailSubject = "MISSION "; string _MailToRappGlobal = "";
                //BTDevinSSISService@BT.COM.TN	10.120.2.1	ESSID.SAMI@BT.COM.TN
                StringBuilder sb = new StringBuilder();
                _MailBody = "** LETTRE MISSION   :  " + Environment.NewLine;
                //"** Nombre Total   :  " + _LogList.Count() + Environment.NewLine;


                string _LigneBody = "";



                sb.Append("</table><br/> Meilleures Salutations  <br/>  Service Moyens Generaux BT ");
                //_MailBody += "</table>";

                _MailBody = sb.ToString();


                if (MailLibrary.CreateMail(_MailServer, _MailFrom, _MailToRappGlobal, _MailSubject, _MailBody, _MailAttachement, ref _msg, true) == false)
                {
                    Nberr += 1;
                    _message = "Exception  Envoi Mail  :" + _msg;
                }


                //*****  GEt Liste Fichiers a Recevoir 

                if (Nberr == 0)
                {
                  
                    //using (MemoryStream memoryStream = new MemoryStream())
                    //{
                    //    using (StreamWriter memoryStreamWriter = new StreamWriter(memoryStream)) // fichier crée dans memoire
                    //                                                                             //using (StreamWriter writer = new StreamWriter(fileName)) // fichier crée sur disque
                    //    {
                    //        using (CsvWriter csv = new CsvWriter(memoryStreamWriter, config))
                    //        {
                    //            csv.WriteRecords(_LogList);
                    //        }
                    //    }
                    //    byte[] file = memoryStream.ToArray();
                    //    _MailAttachement = MailLibrary.ConvertBytesToCSVAttachment(file, "RECAP_RA_" + _journee + "_" + DateTime.Now);
                    //}
                    //byte[] file = memoryStream.ToArray();
                    //_MailAttachement = MailLibrary.ConvertBytesToCSVAttachment(file, "RECAP_RA_" + _journee + "_" + DateTime.Now);

                   
                    if (Nberr == 0) { return true; }
                }


            }
            catch (Exception ex)
            {
                _message = "Exception:  " + ex.Message;
            }
            return result;
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var demande = await _demandesService.GetDemandeByIdAsync(id);
            if (demande == null)
            {
                return NotFound();
            }

            await _demandesService.DeleteDemandeAsync(id);
            return RedirectToAction(nameof(Index));
        }

         


        public async Task<IActionResult> SearchByDate(DateTime? DateDebut, DateTime? DateFin)
        {
            if (!DateDebut.HasValue || !DateFin.HasValue)
            {
                // Return an error view or redirect to a different action if the dates are not provided
                return View("Error");
            }

            var demandes = _context.Demandes.AsQueryable();

            demandes = demandes.Where(d => d.DateDepart >= DateDebut && d.DateArrivee <= DateFin);

            var demandesList = await demandes.ToListAsync();

            // Pass the dates back to the view for display in the search form
            ViewData["DateDebut"] = DateDebut.Value.ToString("yyyy-MM-dd");
            ViewData["DateFin"] = DateFin.Value.ToString("yyyy-MM-dd");

            return View(demandesList);
        }







        public async Task<IActionResult> DownloadExcel(string searchString, string searchMatricule)
        {
            var demandes = await _demandesService.GetAllDemandesAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                demandes = demandes.Where(d => d.IdEmploye.Contains(searchString)).ToList();
            }

            if (!string.IsNullOrEmpty(searchMatricule))
            {
                demandes = demandes.Where(d => d.Voiture != null && d.Voiture.Matricule.Contains(searchMatricule)).ToList();
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Demandes Report");

                // Set headers
                var headers = new[]
                {
            "Nom & Prenom", "Id Employe", "Affectation Departement", "Type Voiture",
            "Destination", "Dates", "Description", "Mission", "Etat", "Matricule","Kilometrage"
        };

                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    worksheet.Cells[1, i + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[1, i + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[1, i + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[1, i + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                }

                // Populate data
                for (int row = 0; row < demandes.Count; row++)
                {
                    var demande = demandes[row];
                    worksheet.Cells[row + 2, 1].Value = $"{demande.Nom} {demande.Prenom}";
                    worksheet.Cells[row + 2, 2].Value = demande.IdEmploye;
                    worksheet.Cells[row + 2, 3].Value = demande.AffectationDepartement;
                    worksheet.Cells[row + 2, 4].Value = demande.TypeVoiture;
                    worksheet.Cells[row + 2, 5].Value = demande.Destination;
                    worksheet.Cells[row + 2, 6].Value = $"{demande.DateDepart.ToShortDateString()} - {demande.DateArrivee.ToShortDateString()}";
                    worksheet.Cells[row + 2, 7].Value = demande.Description;
                    worksheet.Cells[row + 2, 8].Value = demande.Mission;
             
                    worksheet.Cells[row + 2, 9].Value = demande.Etat;

                    if(demande.Etat == "Refused")
                    {
                        worksheet.Cells[row + 2, 10].Value = "Refuse";
                    }
                    else if (demande.Etat == "En attente")
                    {
                        worksheet.Cells[row + 2, 10].Value = "En attente";

                    }
                    else
                    {
                        worksheet.Cells[row + 2, 10].Value = demande.Voiture.Matricule;
                    }
                    worksheet.Cells[row + 2, 11].Value = demande.Kilometrage;
                }

                using (var stream = new MemoryStream())
                {
                    package.SaveAs(stream); 
                    var fileBytes = stream.ToArray();
                    return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Demandes.xlsx");
                }
            }
        }






 
    }
}
