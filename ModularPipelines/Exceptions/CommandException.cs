using CliWrap.Buffered;

namespace ModularPipelines.Exceptions;

public class CommandException : PipelineException
{
    public CommandException(string input, BufferedCommandResult commandResult) : base(GenerateMessage(input, commandResult))
    {
        CommandResult = commandResult;
    }

    public BufferedCommandResult CommandResult { get; }

    private static string? GenerateMessage(string input, BufferedCommandResult commandResult)
    {
        return $"Error: {GetOutput(commandResult)}{Environment.NewLine}Exit Code: {commandResult.ExitCode}{Environment.NewLine}Input: {input}";
    }

    private static string GetOutput(BufferedCommandResult commandResult)
    {
        return !string.IsNullOrEmpty(commandResult.StandardError) ? commandResult.StandardError : commandResult.StandardOutput;
    }
}