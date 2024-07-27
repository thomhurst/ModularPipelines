using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Git.Models;

[ExcludeFromCodeCoverage]
public record GitVersionInformation
{
    public string? AssemblySemFileVer { get; init; }
    
    public string? AssemblySemVer { get; init; }
    
    public string? BranchName { get; init; }
    
    public int? BuildMetaData { get; init; }
    
    public DateTimeOffset CommitDate { get; init; }
    
    public int CommitsSinceVersionSource { get; init; }
    
    public string? EscapedBranchName { get; init; }
    
    public string? FullBuildMetaData { get; init; }
    
    public string? FullSemVer { get; init; }
    
    public string? InformationalVersion { get; init; }
    
    public int Major { get; init; }
    
    public string? MajorMinorPatch { get; init; }
    
    public int Minor { get; init; }
    
    public int Patch { get; init; }
    
    public string? PreReleaseLabel { get; init; }
    
    public string? PreReleaseLabelWithDash { get; init; }
    
    public int? PreReleaseNumber { get; init; }
    
    public string? PreReleaseTag { get; init; }
    
    public string? PreReleaseTagWithDash { get; init; }
    
    public string? SemVer { get; init; }
    
    public string? Sha { get; init; }
    
    public string? ShortSha { get; init; }
    
    public int UncommittedChanges { get; init; }
    
    public string? VersionSourceSha { get; init; }
    
    public int? WeightedPreReleaseNumber { get; init; }

    public string CommitsSinceVersionSourcePadded => CommitsSinceVersionSource.ToString().PadLeft(4, '0');
}