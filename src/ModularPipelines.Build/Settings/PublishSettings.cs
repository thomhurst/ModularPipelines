namespace ModularPipelines.Build.Settings;

public record PublishSettings
{
    public bool ShouldPublish { get; init; }
    
    public bool IsAlpha { get; init; }
}