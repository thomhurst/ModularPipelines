namespace ModularPipelines.Git;

public interface IGitInformation
{
    public string BranchName { get; }
    public string SemanticVersion { get; }
    public string Sha { get; }
    public string MajorMinorPatch { get; }
    public string Major { get; }
    public string Minor { get; }
    public string Patch { get; }
    public string Tag { get; }
    public string Label { get; }
    public int CommitsOnBranch { get; }
    public DateOnly CommitDate { get; }
}

public interface IGitInformation<T> : IGitInformation
{
}