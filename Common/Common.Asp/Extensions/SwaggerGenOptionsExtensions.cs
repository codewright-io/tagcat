using System.Reflection;
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
        return options;
    }
}
