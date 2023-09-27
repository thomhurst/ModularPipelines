namespace ModularPipelines.TeamCity;

public interface ITeamCityEnvironmentVariables
{
    string? TeamCityVersion { get; }

    string? ProjectName { get; }

    string? BuildConfigurationName { get; }

    bool IsPersonal { get; }

    string? BuildNumber { get; }

    string? BuildUrl { get; }

    string? BuildPropertiesFile { get; }
}