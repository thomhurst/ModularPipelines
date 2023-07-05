using System.Xml.Linq;
using ModularPipelines.Context;

namespace ModularPipelines.DotNet;

public class TrxParser : ITrxParser
{
    private readonly IXml _xml;

    public TrxParser(IXml xml)
    {
        _xml = xml;
    }

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
            Duration = element.Attribute("duration")!.Value,
            StartTime = element.Attribute("startTime")!.Value,
            EndTime = element.Attribute("endTime")!.Value,
            TestType = element.Attribute("testType")!.Value,
            Outcome = element.Attribute("outcome")!.Value,
            TestListId = element.Attribute("testListId")!.Value,
            RelativeResultsDirectory = element.Attribute("relativeResultsDirectory")!.Value,
            Output = new TestOutput
            {
                StdOut = element.Descendants().FirstOrDefault(x => x.Name.LocalName == "StdOut")?.Value
            }
        };
    }
}
