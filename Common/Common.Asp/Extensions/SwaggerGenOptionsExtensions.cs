using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;
using CodeWright.Common.Asp;
using Microsoft.Extensions.DependencyInjection;

namespace Swashbuckle.AspNetCore.SwaggerGen;

public static class SwaggerGenOptionsExtensions
{
    public static SwaggerGenOptions AddXmlDocument(this SwaggerGenOptions options)
        => AddXmlDocument(options, Assembly.GetCallingAssembly());

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
}
