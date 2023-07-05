using System.Xml.Serialization;

namespace ModularPipelines.DotNet;

[Serializable]
[XmlRoot(ElementName = "UnitTestResult")]
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
    public string? Duration { get; init; }

    [XmlAttribute("startTime")]
    public string? StartTime { get; init; }

    [XmlAttribute("endTime")]
    public string? EndTime { get; init; }

    [XmlAttribute("testType")]
    public string? TestType { get; init; }

    [XmlAttribute("outcome")]
    public string? Outcome { get; init; }

    [XmlAttribute("testListId")]
    public string? TestListId { get; init; }

    [XmlAttribute("relativeResultsDirectory")]
    public string? RelativeResultsDirectory { get; init; }

    [XmlElement("Output")]
    public TestOutput? Output { get; init; }
}
