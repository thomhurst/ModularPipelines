using ModularPipelines.Console;
using ModularPipelines.Context;
using ModularPipelines.Engine;

namespace ModularPipelines.Azure.Pipelines;

internal class AzurePipeline : IAzurePipeline
{
    private readonly IEnvironmentContext _environment;
    private readonly IModuleOutputBuffer _buffer;
    private readonly IBuildSystemFormatter _formatter;

    public AzurePipeline(
        AzurePipelineVariables variables,
        IEnvironmentContext environment,
        IConsoleCoordinator consoleCoordinator,
        IBuildSystemFormatterProvider formatterProvider)
    {
        _environment = environment;
        Variables = variables;
        _buffer = consoleCoordinator.GetUnattributedBuffer();
        _formatter = formatterProvider.GetFormatter();
    }

    public bool IsRunningOnAzurePipelines
        => !string.IsNullOrWhiteSpace(_environment.EnvironmentVariables.GetEnvironmentVariable("TF_BUILD"));

    public AzurePipelineVariables Variables { get; }

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
            if (startCommand != null)
            {
                _buffer.WriteLine(startCommand);
            }
        }

        public void Dispose()
        {
            var endCommand = _formatter.GetEndBlockCommand(_name);
            if (endCommand != null)
            {
                _buffer.WriteLine(endCommand);
            }
        }
    }
}
