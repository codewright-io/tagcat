using System;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CodeWright.Common.Asp;

/// <summary>
/// Swagger schema filter to modify description of enum types so they
/// show the XML docs attached to each member of the enum.
/// </summary>
/// <remarks>
/// Source: https://stackoverflow.com/questions/53282170/swaggerui-not-display-enum-summary-description-c-sharp-net-core
/// </remarks>
public class DescribeEnumMembers : ISchemaFilter
{
    private readonly XDocument mXmlComments;

    /// <summary>
    /// Initialize schema filter.
    /// </summary>
    /// <param name="argXmlComments">Document containing XML docs for enum members.</param>
    public DescribeEnumMembers(XDocument argXmlComments)
      => mXmlComments = argXmlComments;

    /// <summary>
    /// Apply this schema filter.
    /// </summary>
    /// <param name="schema">Target schema object.</param>
    /// <param name="context">Schema filter context.</param>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var EnumType = context.Type;

        if (!EnumType.IsEnum) return;

        var sb = new StringBuilder(schema.Description);

        sb.AppendLine("<p>Possible values:</p>");
        sb.AppendLine("<ul>");

        foreach (var EnumMemberName in Enum.GetNames(EnumType))
        {
            var FullEnumMemberName = $"F:{EnumType.FullName}.{EnumMemberName}";

            var EnumMemberDescription = mXmlComments.XPathEvaluate(
              $"normalize-space(//member[@name = '{FullEnumMemberName}']/summary/text())"
            ) as string;

            if (string.IsNullOrEmpty(EnumMemberDescription)) continue;

            sb.AppendLine(FormattableString.Invariant($"<li><b>{EnumMemberName}</b>: {EnumMemberDescription}</li>"));
        }

        sb.AppendLine("</ul>");

        schema.Description = sb.ToString();
    }
}
