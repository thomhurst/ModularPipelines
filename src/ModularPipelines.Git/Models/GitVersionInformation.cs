namespace ModularPipelines.Git.Models;

public record GitVersionInformation
{
    public int Major { get; init; }
    public int Minor { get; init; }
    public int Patch { get; init; }
    public string? PreReleaseTag { get; init; }
    public string? PreReleaseTagWithDash { get; init; }
    public string? PreReleaseLabel { get; init; }
    public string? PreReleaseLabelWithDash { get; init; }
    public int? PreReleaseNumber { get; init; }
    public int? WeightedPreReleaseNumber { get; init; }
    public string? BuildMetaData { get; init; }
    public string? BuildMetaDataPadded { get; init; }
    public string? FullBuildMetaData { get; init; }
    public string? MajorMinorPatch { get; init; }
    public string? SemVer { get; init; }
    public string? LegacySemVer { get; init; }
    public string? LegacySemVerPadded { get; init; }
    public string? AssemblySemVer { get; init; }
    public string? AssemblySemFileVer { get; init; }
    public string? FullSemVer { get; init; }
    public string? InformationalVersion { get; init; }
    public string? BranchName { get; init; }
    public string? EscapedBranchName { get; init; }
    public string? Sha { get; init; }
    public string? ShortSha { get; init; }
    public string? NuGetVersionV2 { get; init; }
    public string? NuGetVersion { get; init; }
    public string? NuGetPreReleaseTagV2 { get; init; }
    public string? NuGetPreReleaseTag { get; init; }
    public string? VersionSourceSha { get; init; }
    public int CommitsSinceVersionSource { get; init; }
    public string? CommitsSinceVersionSourcePadded { get; init; }
    public int UncommittedChanges { get; init; }
    public DateTime? CommitDate { get; init; }
}