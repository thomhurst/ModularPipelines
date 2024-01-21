namespace ModularPipelines.Exceptions;

public class CommandException : PipelineException
{
    public CommandException(string input, int exitCode, TimeSpan executionTime, string standardOutput, string standardError) : base(GenerateMessage(input, exitCode, executionTime, standardOutput, standardError))
    {
        ExitCode = exitCode;
        ExecutionTime = executionTime;
        StandardOutput = standardOutput;
        StandardError = standardError;
    }

    public int ExitCode { get; }

    public TimeSpan ExecutionTime { get; }

    public string StandardOutput { get; }

    public string StandardError { get; }

    private static string? GenerateMessage(string input, int exitCode, TimeSpan executionTime,
        string standardOutput, string standardError)
    {
        return $"Error: {GetOutput(standardOutput, standardError)}{Environment.NewLine}Exit Code: {exitCode}{Environment.NewLine}Input: {input}";
    }

    private static string GetOutput(string standardOutput, string standardError)
    {
        return !string.IsNullOrEmpty(standardError) ? standardError : standardOutput;
    }
}