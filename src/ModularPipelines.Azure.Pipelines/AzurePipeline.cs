using ModularPipelines.Console;
using ModularPipelines.Context;
using ModularPipelines.Engine;

namespace ModularPipelines.Azure.Pipelines;

internal class AzurePipeline : IAzurePipeline
{
    private readonly IModuleOutputBuffer _buffer;
    private readonly IBuildSystemFormatter _formatter;

    public AzurePipeline(
        AzurePipelineVariables variables,
        IConsoleCoordinator consoleCoordinator,
        IBuildSystemFormatterProvider formatterProvider)
    {
        EnvironmentVariables = variables;
        _buffer = consoleCoordinator.GetUnattributedBuffer();
        _formatter = formatterProvider.GetFormatter();
    }

    public AzurePipelineVariables EnvironmentVariables { get; }

    public void WriteLine(string message)
    {
        _buffer.WriteLine(message);
    }

    public IDisposable BeginSection(string name)
    {
        return new OutputSection(_buffer, name, _formatter);
    }

    private sealed class OutputSection : IDisposable
    {
        private readonly IModuleOutputBuffer _buffer;
        private readonly string _name;
        private readonly IBuildSystemFormatter _formatter;

        public OutputSection(IModuleOutputBuffer buffer, string name, IBuildSystemFormatter formatter)
        {
            _buffer = buffer;
            _name = name;
            _formatter = formatter;

            var startCommand = formatter.GetStartBlockCommand(name);
            buffer.WriteGroupCommand(formatter, startCommand);
        }

        public void Dispose()
        {
            var endCommand = _formatter.GetEndBlockCommand(_name);
            _buffer.WriteGroupCommand(_formatter, endCommand);
        }
    }
}
