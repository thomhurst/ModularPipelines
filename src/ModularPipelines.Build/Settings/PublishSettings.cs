namespace ModularPipelines.Build.Settings;

public record PublishSettings
{
    public bool ShouldPublish { get; init; }

    public bool IsAlpha { get; init; }

    public string? ExpectedVersion { get; init; }

    public string? RehearsalFeedPath { get; init; }
}
