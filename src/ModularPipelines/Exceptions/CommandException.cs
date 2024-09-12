namespace ModularPipelines.Exceptions;

public class CommandException : PipelineException
{
    public CommandException(string input, int exitCode, TimeSpan executionTime, string standardOutput, string standardError, Exception? innerException = null) : base(GenerateMessage(input, exitCode, executionTime, standardOutput, standardError), innerException)
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

    private static string GenerateMessage(string input, int exitCode, TimeSpan executionTime,
        string standardOutput, string standardError)
    {
        return $"""
               
               Input: {input}
               
               Error: {GetOutput(standardOutput, standardError)}
               
               Exit Code: {exitCode}
               """;
    }

    private static string GetOutput(string standardOutput, string standardError)
    {
        return !string.IsNullOrEmpty(standardError) ? standardError : standardOutput;
    }
}