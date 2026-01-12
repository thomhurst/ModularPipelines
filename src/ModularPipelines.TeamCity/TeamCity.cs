using ModularPipelines.Console;
using ModularPipelines.Engine;

namespace ModularPipelines.TeamCity;

internal class TeamCity : ITeamCity
{
    private readonly IModuleOutputBuffer _buffer;
    private readonly IBuildSystemFormatter _formatter;

    public TeamCity(
        ITeamCityEnvironmentVariables environmentVariables,
        IConsoleCoordinator consoleCoordinator,
        IBuildSystemFormatterProvider formatterProvider)
    {
        EnvironmentVariables = environmentVariables;
        _buffer = consoleCoordinator.GetUnattributedBuffer();
        _formatter = formatterProvider.GetFormatter();
    }

    public ITeamCityEnvironmentVariables EnvironmentVariables { get; }

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
