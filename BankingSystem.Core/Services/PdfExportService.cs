using BankingSystem.Core.Models;
using BankingSystem.Core.Services.Interfaces;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
public class PdfExportService : IPdfExportService
{
    public void ExportTransactions(List<Transaction> transactions, string filePath)
    {
        Document doc = new Document(PageSize.A4);
        PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));

        doc.Open();

        doc.Add(new Paragraph("Recent Transactions"));
        doc.Add(new Paragraph(" "));

        PdfPTable table = new PdfPTable(3);

        table.AddCell("Amount");
        table.AddCell("Type");
        table.AddCell("Date");

        foreach (var t in transactions)
        {
            table.AddCell(t.Amount.ToString());
            table.AddCell(t.Type.ToString());
            table.AddCell(t.Date.ToString("yyyy-MM-dd"));
        }

        doc.Add(table);

        doc.Close();

        // ✅ OPEN AFTER EXPORT
        Process.Start(new ProcessStartInfo
        {
            FileName = filePath,
            UseShellExecute = true
        });
    }
}