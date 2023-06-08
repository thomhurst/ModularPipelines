namespace ModularPipelines.Git;

public class GitInformation<T> : IGitInformation<T>
{
    public string BranchName => GitVersionInformation.BranchName;
    public string SemanticVersion => GitVersionInformation.SemVer;
    public string MajorMinorPatch => GitVersionInformation.MajorMinorPatch;
    public string Major => GitVersionInformation.Major;
    public string Minor => GitVersionInformation.Minor;
    public string Patch => GitVersionInformation.Patch;
    public string Tag => GitVersionInformation.PreReleaseTag;
    public string Label => GitVersionInformation.PreReleaseLabel;
    public int CommitsOnBranch => int.Parse(GitVersionInformation.CommitsSinceVersionSource);
    public DateOnly CommitDate => DateOnly.Parse(GitVersionInformation.CommitDate);
    public string Sha => GitVersionInformation.Sha;
}