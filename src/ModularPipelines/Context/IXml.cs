using System.Xml.Linq;

namespace ModularPipelines.Context;

public interface IXml
{
    string ToXml<T>(T input, SaveOptions options = SaveOptions.None);

    T? FromXml<T>(string input, LoadOptions options = LoadOptions.PreserveWhitespace)
        where T : class;

    T? FromXml<T>(XElement element, LoadOptions options = LoadOptions.PreserveWhitespace)
        where T : class;
}