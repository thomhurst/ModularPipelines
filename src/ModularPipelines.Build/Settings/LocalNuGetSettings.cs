namespace ModularPipelines.Build.Settings;

/// <summary>
/// Settings for local NuGet development environment.
/// </summary>
public record LocalNuGetSettings
{
    /// <summary>
    /// The name for the local NuGet source. Defaults to "ModularPipelinesLocalNuGet".
    /// </summary>
    public string SourceName { get; init; } = "ModularPipelinesLocalNuGet";

    /// <summary>
    /// The folder name under AppData for the local NuGet repository. Defaults to "ModularPipelines".
    /// </summary>
    public string AppDataFolderName { get; init; } = "ModularPipelines";

    /// <summary>
    /// The subfolder name for local NuGet packages. Defaults to "LocalNuget".
    /// </summary>
    public string LocalNugetFolderName { get; init; } = "LocalNuget";
}
