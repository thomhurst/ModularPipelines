using System.Text;
using ModularPipelines.DotNet.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Models;

namespace ModularPipelines.DotNet.Exceptions;

public class DotNetTestFailedException : PipelineException
{
    public CommandResult CommandResult { get; }

    public DotNetTestResult? DotNetTestResult { get; }

    internal DotNetTestFailedException(CommandResult commandResult, DotNetTestResult? dotNetTestResult)
    {
        CommandResult = commandResult;
        DotNetTestResult = dotNetTestResult;
    }

    /// <inheritdoc/>
    public override string Message
    {
        get
        {
            if (DotNetTestResult?.UnitTestResults.ToList() is not { } unitTestResults)
            {
                return GetStandardError();
            }

            var sb = new StringBuilder();

            foreach (var unitTestResult in unitTestResults.Where(x => x.Outcome == TestOutcome.Failed))
            {
                sb.AppendLine($"Failed Test: {unitTestResult.TestName}");

                if (!string.IsNullOrEmpty(unitTestResult.Output?.StdOut))
                {
                    sb.AppendLine("Output:");
                    sb.AppendLine(unitTestResult.Output.StdOut);
                    sb.AppendLine();
                }

                if (!string.IsNullOrEmpty(unitTestResult.Output?.ErrorInfo?.Message))
                {
                    sb.AppendLine("Error:");
                    sb.AppendLine(unitTestResult.Output.ErrorInfo.Message);
                    sb.AppendLine(unitTestResult.Output.ErrorInfo.StackTrace);
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }
    }

    private string GetStandardError()
    {
        if (string.IsNullOrEmpty(CommandResult.StandardError))
        {
            return CommandResult.StandardOutput;
        }

        return CommandResult.StandardError;
    }
}