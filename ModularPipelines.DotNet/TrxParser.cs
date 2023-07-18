using System.Xml.Linq;
using ModularPipelines.DotNet.Enums;

namespace ModularPipelines.DotNet;

internal class TrxParser : ITrxParser
{
    public DotNetTestResult ParseTestResult(string input)
    {
        return new DotNetTestResult(GetUnitTestResults(input));
    }

    private List<UnitTestResult> GetUnitTestResults(string input)
    {
        return XDocument.Load(new StringReader(input)).Descendants()
            .Where(d => d.Name.LocalName == "UnitTestResult")
            .Select(ParseElement)
            .ToList();
    }

    private UnitTestResult ParseElement(XElement element)
    {
        return new UnitTestResult
        {
            ExecutionId = element.Attribute("executionId")!.Value,
            TestId = element.Attribute("testId")!.Value,
            TestName = element.Attribute("testName")!.Value,
            ComputerName = element.Attribute("computerName")!.Value,
            Duration = TimeSpan.Parse(element.Attribute("duration")!.Value),
            StartTime = DateTimeOffset.Parse(element.Attribute("startTime")!.Value),
            EndTime = DateTimeOffset.Parse(element.Attribute("endTime")!.Value),
            TestType = element.Attribute("testType")!.Value,
            Outcome = Enum.Parse<TestOutcome>(element.Attribute("outcome")!.Value),
            TestListId = element.Attribute("testListId")!.Value,
            RelativeResultsDirectory = element.Attribute("relativeResultsDirectory")!.Value,
            Output = new TestOutput
            {
                StdOut = element.Descendants().FirstOrDefault(x => x.Name.LocalName == "StdOut")?.Value
            }
        };
    }
}
