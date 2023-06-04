using CliWrap.Buffered;

namespace ModularPipelines.Command.Exceptions;

public class CommandException : ModularPipelines.Exceptions.PipelineException
{
    public CommandException(string input, BufferedCommandResult bufferedCommandResult) : base(GenerateMessage(input, bufferedCommandResult))
    {
        CommandResult = bufferedCommandResult;
    }

    public BufferedCommandResult CommandResult { get; }

    private static string? GenerateMessage(string input, BufferedCommandResult bufferedCommandResult)
    {
        return $"Error: {bufferedCommandResult.StandardError}{Environment.NewLine}Exit Code: {bufferedCommandResult.ExitCode}{Environment.NewLine}Input: {input}";
    }
}