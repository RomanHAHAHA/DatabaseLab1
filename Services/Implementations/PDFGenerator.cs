using DatabaseLab1.Services.Interfaces;
using DinkToPdf;
using DinkToPdf.Contracts;
using Newtonsoft.Json;
using System.Text;

namespace DatabaseLab1.Services.Implementations;

public class PdfGenerator(IConverter converter) : IPdfGenerator
{
    private readonly IConverter _converter = converter;

    public string GenerateHtmlReport(string reportData)
    {
        var description = ExtractData(reportData, "Description");
        var date = ExtractData(reportData, "Date");
        var jsonData = ExtractJsonArray(reportData, "Object");

        var jsonObjectList = JsonConvert
            .DeserializeObject<List<Dictionary<string, object>>>(jsonData);

        var sb = new StringBuilder();
        sb.Append("<html><head><style>table { width: 100%; } th, td " +
                  "{ padding: 10px; border: 1px solid black; }</style></head><body>");

        sb.Append($"<p>{description}</p>");
        sb.Append($"<p>{date}</p>");
        sb.Append("<table>");

        if (jsonObjectList.Count > 0)
        {
            sb.Append("<tr>");
            foreach (var key in jsonObjectList[0].Keys)
            {
                sb.Append($"<th>{key}</th>");
            }
            sb.Append("</tr>");

            foreach (var item in jsonObjectList)
            {
                sb.Append("<tr>");
                foreach (var value in item.Values)
                {
                    sb.Append($"<td>{value}</td>");
                }
                sb.Append("</tr>");
            }
        }

        sb.Append("</table>");
        sb.Append("</body></html>");

        return sb.ToString();
    }

    private string ExtractData(string input, string key)
    {
        var startIndex = input.IndexOf(key + ":") + key.Length + 1;
        var endIndex = input.IndexOf(",", startIndex);

        return input.Substring(startIndex, endIndex - startIndex).Trim();
    }

    private string ExtractJsonArray(string input, string key)
    {
        var startIndex = input.IndexOf($"{key}:") + key.Length + 1;
        var jsonArray = input.Substring(startIndex).Trim();

        return jsonArray;
    }

    public byte[] GeneratePdfFromHtml(string htmlContent)
    {
        var document = new HtmlToPdfDocument
        {
            GlobalSettings = new GlobalSettings
            {
                PaperSize = PaperKind.A4,
                Orientation = Orientation.Portrait,
            },
            Objects = {
            new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8" }
            }
        }
        };

        return _converter.Convert(document);
    }
}
