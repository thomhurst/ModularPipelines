using ModularPipelines.Logging;

namespace ModularPipelines.TeamCity;

internal class TeamCity : ITeamCity
{
    private readonly ModuleOutputWriter _outputWriter;

    public TeamCity(ITeamCityEnvironmentVariables environmentVariables,
        IModuleOutputWriterFactory outputWriterFactory)
    {
        EnvironmentVariables = environmentVariables;
        _outputWriter = outputWriterFactory.Create("TeamCity");
    }

    public ITeamCityEnvironmentVariables EnvironmentVariables { get; }

    public void WriteLine(string message)
    {
        _outputWriter.WriteLine(message);
    }

    public void WriteLineDirect(string message)
    {
        _outputWriter.WriteLineDirect(message);
    }

    public IDisposable BeginSection(string name)
    {
        return _outputWriter.BeginSection(name);
    }
}