using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;
using CodeWright.Common.Asp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;

namespace Swashbuckle.AspNetCore.SwaggerGen;

public static class SwaggerGenOptionsExtensions
{
    /// <summary>
    /// Add XML comments to Swagger
    /// </summary>
    public static SwaggerGenOptions AddXmlDocument(this SwaggerGenOptions options)
        => AddXmlDocument(options, Assembly.GetCallingAssembly());

    /// <summary>
    /// Add XML comments to Swagger
    /// </summary>
    public static SwaggerGenOptions AddXmlDocument(this SwaggerGenOptions options, Assembly assembly)
    {
        var xmlFilePath = assembly.GetName().Name + ".xml";
        var xmlDoc = Path.Combine(AppContext.BaseDirectory, xmlFilePath);

        if (!File.Exists(xmlDoc))
            throw new FileNotFoundException("Can't load XML file : " + xmlDoc);

        options.IncludeXmlComments(xmlDoc, true);
        options.IncludeXmlDocumentEnums(xmlDoc);

        return options;
    }

    /// <summary>
    /// Include enum XML descriptions
    /// </summary>
    /// <remarks>
    /// Source: https://stackoverflow.com/questions/53282170/swaggerui-not-display-enum-summary-description-c-sharp-net-core
    /// </remarks>
    private static SwaggerGenOptions IncludeXmlDocumentEnums(this SwaggerGenOptions options, string xmlDocumentPath)
    {
        if (string.IsNullOrEmpty(xmlDocumentPath))
            throw new ArgumentNullException(nameof(xmlDocumentPath));

        // load the XML docs for processing.
        var XmlDocs = (
          from DocPath in xmlDocumentPath select XDocument.Load(xmlDocumentPath)
        ).ToList();

        // ...<snip other code>

        // add preprocessed XML docs to Swagger.
        foreach (var doc in XmlDocs)
        {
            options.IncludeXmlComments(() => new XPathDocument(doc.CreateReader()), true);

            // apply schema filter to add description of enum members.
            options.SchemaFilter<DescribeEnumMembers>(doc);
        }

        return options;
    }

    /// <summary>
    /// Add extra information to OpenAPI
    /// </summary>
    public static SwaggerGenOptions IncludeDocumentInformation(this SwaggerGenOptions options, 
        string title, 
        string version,
        string description,
        Uri? logoUrl = null,
        string? contactName = null,
        Uri? contactUrl = null,
        string? contactEmail = null,
        Uri? licenseUrl = null
        )
    {
        if (options is null)
            throw new ArgumentNullException(nameof(options));

        if (string.IsNullOrEmpty(title))
            throw new ArgumentNullException(nameof(title));

        if (string.IsNullOrEmpty(version))
            throw new ArgumentNullException(nameof(version));

        if (string.IsNullOrEmpty(description))
            throw new ArgumentNullException(nameof(description));

        options.SwaggerDoc(version, new OpenApiInfo
        {
            Title = title,
            Version = version,
            Description = description,
            Contact = contactUrl != null && !string.IsNullOrEmpty(contactName) ? 
                new OpenApiContact { Name = contactName, Url = contactUrl, Email = contactEmail, } : null,
            License = licenseUrl != null ? new OpenApiLicense { Name = "License", Url = licenseUrl } : null,
            Extensions = logoUrl != null ? new Dictionary<string, IOpenApiExtension>
            {
                { "x-logo", new OpenApiObject
                    {
                       { "url", new OpenApiString(logoUrl.ToString())},
                       { "altText", new OpenApiString("Logo")}
                    }
                },
            } : null,
        });

        return options;
    }
}
