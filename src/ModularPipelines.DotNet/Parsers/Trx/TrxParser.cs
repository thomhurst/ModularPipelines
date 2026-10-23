using System.Xml.Linq;
using ModularPipelines.DotNet.Enums;

namespace ModularPipelines.DotNet.Parsers.Trx;

public class TrxParser : ITrxParser
{
    public DotNetTestResult ParseTrxContents(string input)
    {
        var xDocument = XDocument.Load(new StringReader(input));
        return new DotNetTestResult(GetUnitTestResults(xDocument), GetResultSummary(xDocument));
    }

    private static ResultSummary GetResultSummary(XDocument document)
    {
        return ParseResultSummary(document
            .Descendants()
            .First(e => e.Name.LocalName == "ResultSummary"));
    }

    private static ResultSummary ParseResultSummary(XElement element)
    {
        var outcome = element.Attribute("outcome")!.Value;

        var counters = ParseCounters(element.Descendants().First(e => e.Name.LocalName == "Counters"));

        return new ResultSummary(outcome, counters);
    }

    private static Counters ParseCounters(XElement element)
    {
        return new Counters(
            int.Parse(element.Attribute("total")!.Value),
            int.Parse(element.Attribute("executed")!.Value),
            int.Parse(element.Attribute("passed")!.Value),
            int.Parse(element.Attribute("failed")!.Value),
            int.Parse(element.Attribute("error")!.Value),
            int.Parse(element.Attribute("timeout")!.Value),
            int.Parse(element.Attribute("aborted")!.Value),
            int.Parse(element.Attribute("inconclusive")!.Value),
            int.Parse(element.Attribute("passedButRunAborted")!.Value),
            int.Parse(element.Attribute("notRunnable")!.Value),
            int.Parse(element.Attribute("notExecuted")!.Value),
            int.Parse(element.Attribute("disconnected")!.Value),
            int.Parse(element.Attribute("warning")!.Value),
            int.Parse(element.Attribute("completed")!.Value),
            int.Parse(element.Attribute("inProgress")!.Value),
            int.Parse(element.Attribute("pending")!.Value)
        );
    }

    private static List<UnitTestResult> GetUnitTestResults(XDocument xDocument)
    {
        return [.. xDocument.Descendants()
            .Where(d => d.Name.LocalName == "Results")
            .Elements()
            .Where(d => d.Name.LocalName == "UnitTestResult")
            .Select(ParseUnitTestResult)];
    }

    private static UnitTestResult ParseUnitTestResult(XElement element)
    {
        var output = element.Elements().FirstOrDefault(x => x.Name.LocalName == "Output");
        var errorInfo = output?.Elements().FirstOrDefault(x => x.Name.LocalName == "ErrorInfo");

        return new UnitTestResult
        {
            ExecutionId = element.Attribute("executionId")?.Value,
            TestId = element.Attribute("testId")?.Value,
            TestName = element.Attribute("testName")?.Value,
            ComputerName = element.Attribute("computerName")?.Value,
            Duration = element.Attribute("duration") is { } duration ? TimeSpan.Parse(duration.Value) : null,
            StartTime = element.Attribute("startTime") is { } startTime ? DateTimeOffset.Parse(startTime.Value) : null,
            EndTime = element.Attribute("endTime") is { } endTime ? DateTimeOffset.Parse(endTime.Value) : null,
            TestType = element.Attribute("testType")?.Value,
            Outcome = ParseTestOutcome(element.Attribute("outcome")?.Value),
            TestListId = element.Attribute("testListId")?.Value,
            RelativeResultsDirectory = element.Attribute("relativeResultsDirectory")?.Value,
            Output = new TestOutput
            {
                StdOut = output?.Elements().FirstOrDefault(x => x.Name.LocalName == "StdOut")?.Value,
                DebugTrace = output?.Elements().FirstOrDefault(x => x.Name.LocalName == "DebugTrace")?.Value,
                ErrorInfo = errorInfo == null ? null : new ErrorInfo
                {
                    Message = errorInfo.Elements().FirstOrDefault(x => x.Name.LocalName == "Message")?.Value,
                    StackTrace = errorInfo.Elements().FirstOrDefault(x => x.Name.LocalName == "StackTrace")?.Value,
                },
            },
        };
    }

    private static TestOutcome ParseTestOutcome(string? value)
    {
        return Enum.TryParse<TestOutcome>(value, ignoreCase: true, out var outcome)
               && Enum.IsDefined(outcome)
            ? outcome
            : TestOutcome.Unknown;
    }
}
