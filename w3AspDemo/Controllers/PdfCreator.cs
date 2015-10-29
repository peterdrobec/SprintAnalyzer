using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using MigraDoc.DocumentObjectModel;

namespace w3AspDemo.Controllers
{
    public static class PdfCreator
    {
        public static void PdfPrinter(FilterResults results)
        {
            PdfDocument pdf = new PdfDocument();
            int numPages;
            int reminder = 0;

            results.issues.OrderBy(i => i.fields.issuetype.name);
            List<JiraTicket> issues = new List<JiraTicket>();

                numPages = results.issues.Count / 6 + 1;
                reminder = results.issues.Count % 6;

                for (int i = 0; i < numPages - 1; i++)
                {
                    issues = results.issues.Skip(i*6).Take(6).ToList();
                    CreatePdfPage(issues, ref pdf);
                }

                if (reminder > 0)
                {
                    issues = results.issues.Skip((numPages-1)*6).Take(reminder).ToList();
                    CreatePdfPage(issues, ref pdf);                    
                }                  

            pdf.Save("C:/ProgramData/JiraCharts/issues.pdf");
         }

        private static void CreatePdfPage (List<JiraTicket> issues, ref PdfDocument pdf)
        {
            PdfPage page = pdf.AddPage();
            page.Size = PdfSharp.PageSize.A4;
            page.Orientation = PdfSharp.PageOrientation.Landscape;

            for (int j = 0; j < issues.Count; j++)
            {
                string text = issues[j].fields.issuetype.name + System.Environment.NewLine + issues[j].key
                  + System.Environment.NewLine + issues[j].fields.summary + System.Environment.NewLine
                  + issues[j].fields.customfield_10008;

                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont font = new XFont("Verdana", 20, XFontStyle.Bold);
                XTextFormatter tf = new XTextFormatter(gfx);
                XRect rect = new XRect();

                if (j < 3)
                {
                    rect = new XRect(15, 15 + j * 180, 400, 170);
                }
                else
                {
                    rect = new XRect(430, 15 + (j - 3) * 180, 400, 170);
                }

                gfx.DrawRectangle(XBrushes.SeaShell, rect);
                tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);
                gfx.Dispose();
            }
           
        }
    }
}