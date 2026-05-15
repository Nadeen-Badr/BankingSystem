using BankingSystem.Core.Models;
using BankingSystem.Core.Services.Interfaces;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

public class PdfExportService : IPdfExportService
{
    public void ExportTransactions(List<Transaction> transactions, string filePath)
    {
        Document doc = new Document(PageSize.A4, 30f, 30f, 40f, 30f);
        PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));

        doc.Open();

        // ================= COLORS =================
        BaseColor primaryColor = new BaseColor(34, 40, 49);       // Dark Navy
        BaseColor secondaryColor = new BaseColor(57, 62, 70);     // Gray
        BaseColor accentColor = new BaseColor(0, 173, 181);       // Cyan
        BaseColor lightBg = new BaseColor(245, 245, 245);         // Light Gray
        BaseColor white = BaseColor.WHITE;

        // ================= FONTS =================
        Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 24, white);
        Font sectionFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, primaryColor);
        Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 11, primaryColor);
        Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11, white);

        // ================= HEADER =================
        PdfPTable headerTable = new PdfPTable(1);
        headerTable.WidthPercentage = 100;

        PdfPCell headerCell = new PdfPCell
        {
            BackgroundColor = primaryColor,
            Border = Rectangle.NO_BORDER,
            Padding = 20
        };

        Paragraph title = new Paragraph("BANKING SYSTEM REPORT", titleFont);
        title.Alignment = Element.ALIGN_CENTER;

        Paragraph date = new Paragraph(
            $"Generated: {DateTime.Now:yyyy-MM-dd HH:mm}",
            FontFactory.GetFont(FontFactory.HELVETICA, 10, white));

        date.Alignment = Element.ALIGN_CENTER;

        headerCell.AddElement(title);
        headerCell.AddElement(date);

        headerTable.AddCell(headerCell);

        doc.Add(headerTable);
        doc.Add(new Paragraph(" "));

        // ================= SECTION TITLE =================
        Paragraph transactionTitle = new Paragraph("Recent Transactions", sectionFont);
        transactionTitle.SpacingAfter = 15f;
        doc.Add(transactionTitle);

        // ================= TABLE =================
        PdfPTable table = new PdfPTable(3);
        table.WidthPercentage = 100;
        table.SetWidths(new float[] { 3f, 3f, 4f });

        // Header Cells
        AddHeaderCell(table, "Amount", headerFont, accentColor);
        AddHeaderCell(table, "Type", headerFont, accentColor);
        AddHeaderCell(table, "Date", headerFont, accentColor);

        decimal totalDeposits = 0;
        decimal totalWithdrawals = 0;

        bool alternate = false;

        foreach (var t in transactions)
        {
            BaseColor rowColor = alternate ? white : lightBg;
            alternate = !alternate;

            AddBodyCell(table, t.Amount.ToString("N2"), normalFont, rowColor);
            AddBodyCell(table, t.Type.ToString(), normalFont, rowColor);
            AddBodyCell(table, t.Date.ToString("yyyy-MM-dd"), normalFont, rowColor);

            if (t.Type.ToString().ToLower().Contains("deposit"))
                totalDeposits += t.Amount;
            else
                totalWithdrawals += t.Amount;
        }

        doc.Add(table);

        doc.Add(new Paragraph(" "));

        // ================= SUMMARY SECTION =================
        PdfPTable summaryTable = new PdfPTable(2);
        summaryTable.WidthPercentage = 40;
        summaryTable.HorizontalAlignment = Element.ALIGN_RIGHT;

        AddSummaryRow(summaryTable, "Total Deposits", totalDeposits.ToString("N2"), sectionFont, normalFont);
        AddSummaryRow(summaryTable, "Total Withdrawals", totalWithdrawals.ToString("N2"), sectionFont, normalFont);
        AddSummaryRow(summaryTable, "Net Balance",
            (totalDeposits - totalWithdrawals).ToString("N2"),
            sectionFont,
            normalFont);

        doc.Add(summaryTable);

        doc.Add(new Paragraph(" "));

        // ================= FOOTER =================
        Paragraph footer = new Paragraph(
            "Generated securely by Banking System",
            FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 9, secondaryColor));

        footer.Alignment = Element.ALIGN_CENTER;

        doc.Add(footer);

        doc.Close();

        Process.Start(new ProcessStartInfo
        {
            FileName = filePath,
            UseShellExecute = true
        });
    }

    private void AddHeaderCell(PdfPTable table, string text, Font font, BaseColor bgColor)
    {
        PdfPCell cell = new PdfPCell(new Phrase(text, font))
        {
            BackgroundColor = bgColor,
            HorizontalAlignment = Element.ALIGN_CENTER,
            Padding = 10
        };

        table.AddCell(cell);
    }

    private void AddBodyCell(PdfPTable table, string text, Font font, BaseColor bgColor)
    {
        PdfPCell cell = new PdfPCell(new Phrase(text, font))
        {
            BackgroundColor = bgColor,
            Padding = 8
        };

        table.AddCell(cell);
    }

    private void AddSummaryRow(
        PdfPTable table,
        string label,
        string value,
        Font labelFont,
        Font valueFont)
    {
        PdfPCell labelCell = new PdfPCell(new Phrase(label, labelFont))
        {
            Border = Rectangle.NO_BORDER,
            Padding = 6
        };

        PdfPCell valueCell = new PdfPCell(new Phrase(value, valueFont))
        {
            Border = Rectangle.NO_BORDER,
            Padding = 6,
            HorizontalAlignment = Element.ALIGN_RIGHT
        };

        table.AddCell(labelCell);
        table.AddCell(valueCell);
    }
}