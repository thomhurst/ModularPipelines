namespace TemplatePipeline.Settings;

public sealed class BuildSettings
{
    public string Solution { get; init; } = "../MySolution.slnx";

    public string PublishProject { get; init; } = "../src/MyApp/MyApp.csproj";

    public string Configuration { get; init; } = "Release";

    public string PublishDirectory { get; init; } = "artifacts/publish";
}
