namespace DatabaseLab1.Services.Interfaces;

public interface IPdfGenerator
{
    string GenerateHtmlReport(string reportData);

    byte[] GeneratePdfFromHtml(string htmlContent);
}