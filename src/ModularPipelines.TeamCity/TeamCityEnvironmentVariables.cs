namespace ModularPipelines.TeamCity;

internal class TeamCityEnvironmentVariables : ITeamCityEnvironmentVariables
{
    public string? TeamCityVersion => Get("TEAMCITY_VERSION");
    
    public string? ProjectName => Get("TEAMCITY_PROJECT_NAME");
    
    public string? BuildConfigurationName => Get("TEAMCITY_BUILDCONF_NAME");
    
    public bool IsPersonal => GetBool("BUILD_IS_PERSONAL");

    public string? BuildNumber => Get("BUILD_NUMBER");

    public string? BuildUrl => Get("BUILD_URL");

    public string? BuildPropertiesFile => Get("TEAMCITY_BUILD_PROPERTIES_FILE");

    private string? Get(string name) => Environment.GetEnvironmentVariable(name);

    private bool GetBool(string name) => bool.TryParse(Get(name), out var value) && value;
}