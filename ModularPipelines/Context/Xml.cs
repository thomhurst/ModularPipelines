using System.Xml.Linq;
using System.Xml.Serialization;

namespace ModularPipelines.Context;

public class Xml : IXml
{
    public string ToXml<T>(T input, SaveOptions options = SaveOptions.None)
    {
        var serializer = new XmlSerializer(typeof(T));
        var document = new XDocument();

        using (var writer = document.CreateWriter())
        {
            serializer.Serialize(writer, input);
        }

        return document.ToString();
    }

    public T? FromXml<T>(string input, LoadOptions options = LoadOptions.PreserveWhitespace) where T : class
    {
        var document = XDocument.Parse(input, options);

        if (document.Root == null)
        {
            return default;
        }

        return FromXml<T>(document.Root, options);
    }

    public T? FromXml<T>(XElement element, LoadOptions options = LoadOptions.PreserveWhitespace) where T : class
    {
        using var reader = element.CreateReader();

        var serializer = new XmlSerializer(typeof(T));

        return serializer.Deserialize(reader) as T;
    }
}
