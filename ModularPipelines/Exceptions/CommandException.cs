using CliWrap.Buffered;

namespace ModularPipelines.Exceptions;

public class CommandException : PipelineException
{
    public CommandException(string input, BufferedCommandResult bufferedCommandResult) : base(GenerateMessage(input, bufferedCommandResult))
    {
        CommandResult = bufferedCommandResult;
    }

    public BufferedCommandResult CommandResult { get; }

    private static string? GenerateMessage(string input, BufferedCommandResult bufferedCommandResult)
    {
        return $"Error: {GetOutput(bufferedCommandResult)}{Environment.NewLine}Exit Code: {bufferedCommandResult.ExitCode}{Environment.NewLine}Input: {input}";
    }

    private static string GetOutput(BufferedCommandResult bufferedCommandResult)
    {
        return !string.IsNullOrEmpty(bufferedCommandResult.StandardError) ? bufferedCommandResult.StandardError : bufferedCommandResult.StandardOutput;
    }
}