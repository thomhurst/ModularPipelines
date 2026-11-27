namespace ModularPipelines.Build.Settings;

public record PublishSettings
{
    public bool ShouldPublish { get; init; }

    public bool IsAlpha { get; init; }

    /// <summary>
    /// The version taken from a published GitHub release's tag, without its <c>v</c> prefix.
    /// It replaces GitVersion, so the release ships the version its tag names.
    /// </summary>
    public string? ReleaseVersion { get; init; }

    public string? RehearsalFeedPath { get; init; }
}
