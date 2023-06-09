﻿using System.Xml.Serialization;

namespace ModularPipelines.DotNet;

[XmlRoot(ElementName = "Output")]
public record TestOutput
{
    [XmlElement("StdOut")]
    public string? StdOut { get; init; }
}
