using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using ModularPipelines.DotNet.Enums;

namespace ModularPipelines.DotNet.Parsers.Trx;

[Serializable]
[XmlRoot(ElementName = "UnitTestResult")]
[ExcludeFromCodeCoverage]
public record UnitTestResult
{
    [XmlAttribute("executionId")]
    public string? ExecutionId { get; init; }

    [XmlAttribute("testId")]
    public string? TestId { get; init; }

    [XmlAttribute("testName")]
    public string? TestName { get; init; }

    [XmlAttribute("computerName")]
    public string? ComputerName { get; init; }

    [XmlAttribute("duration")]
    public TimeSpan? Duration { get; init; }

    [XmlAttribute("startTime")]
    public DateTimeOffset? StartTime { get; init; }

    [XmlAttribute("endTime")]
    public DateTimeOffset? EndTime { get; init; }

    [XmlAttribute("testType")]
    public string? TestType { get; init; }

    [XmlAttribute("outcome")]
    public TestOutcome? Outcome { get; init; }

    [XmlAttribute("testListId")]
    public string? TestListId { get; init; }

    [XmlAttribute("relativeResultsDirectory")]
    public string? RelativeResultsDirectory { get; init; }

    [XmlElement("Output")]
    public TestOutput? Output { get; init; }
}