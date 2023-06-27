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
            .Select(element => _xml.FromXml<UnitTestResult>(element)!)
            .ToList();
    }
}

public interface ITrxParser
{
    DotNetTestResult ParseTestResult(string input);
}

public record DotNetTestResult(IReadOnlyList<UnitTestResult> UnitTestResults)
{
}

public record UnitTestResult
(
    string ExecutionId,
    string TestId,
    string TestName,
    string ComputerName,
    string Duration,
    string StartTime,
    string EndTime,
    string TestType,
    string Outcome,
    string TestListId,
    string RelativeResultsDirectory,
    TestOutput Output
);

public record TestOutput(string StdOut);