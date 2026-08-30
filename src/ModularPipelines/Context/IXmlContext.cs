using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;

namespace ModularPipelines.Context;

/// <summary>
/// Provides XML serialization and deserialization functionality.
/// </summary>
public interface IXmlContext
{
    /// <summary>
    /// Serializes an object to XML string using the specified save options.
    /// </summary>
    /// <typeparam name="T">The type of object to serialize.</typeparam>
    /// <param name="input">The object to serialize.</param>
    /// <param name="options">The save options to use when generating the XML string.</param>
    /// <returns>The XML string representation of the object.</returns>
    [RequiresDynamicCode("XmlSerializer may require runtime code generation.")]
    [RequiresUnreferencedCode("XmlSerializer requires members that trimming cannot statically discover.")]
    string ToXml<T>(T input, SaveOptions options = SaveOptions.None);

    /// <summary>
    /// Deserializes an XML string to an object using the specified load options.
    /// </summary>
    /// <typeparam name="T">The type to deserialize to.</typeparam>
    /// <param name="input">The XML string to deserialize.</param>
    /// <param name="options">The load options to use when parsing the XML string.</param>
    /// <returns>The deserialized object, or null if deserialization fails.</returns>
#pragma warning disable RS0026 // String and XElement overloads intentionally share the optional load options.
    [RequiresDynamicCode("XmlSerializer may require runtime code generation.")]
    [RequiresUnreferencedCode("XmlSerializer requires members that trimming cannot statically discover.")]
    T? FromXml<T>(string input, LoadOptions options = LoadOptions.PreserveWhitespace)
        where T : class;

    /// <summary>
    /// Deserializes an XElement to an object using the specified load options.
    /// </summary>
    /// <typeparam name="T">The type to deserialize to.</typeparam>
    /// <param name="element">The XElement to deserialize.</param>
    /// <param name="options">The load options to use when processing the element.</param>
    /// <returns>The deserialized object, or null if deserialization fails.</returns>
    [RequiresDynamicCode("XmlSerializer may require runtime code generation.")]
    [RequiresUnreferencedCode("XmlSerializer requires members that trimming cannot statically discover.")]
    T? FromXml<T>(XElement element, LoadOptions options = LoadOptions.PreserveWhitespace)
        where T : class;
#pragma warning restore RS0026
}
