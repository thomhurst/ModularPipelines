namespace ModularPipelines.Git.UnitTests;

public class GitApiSurfaceTests
{
    [Test]
    public async Task Package_Exports_Only_The_Grouped_Git_Facade()
    {
        var exportedGitInterfaces = typeof(IGit).Assembly
            .GetExportedTypes()
            .Where(type => type.Name == nameof(IGit));

        await Assert.That(exportedGitInterfaces).IsEquivalentTo([typeof(IGit)]);
    }

    [Test]
    public async Task Handwritten_Public_Types_Use_The_Package_Namespace()
    {
        // Generated CLI option snapshots deliberately do not use PublicAPI baselines.
        // Keep the handwritten facade's exported types explicit and independently reviewed.
        string[] expected =
        [
            "GitAuthor", "GitCommit", "GitExtensions", "GitHash", "GitMessage",
            "GitRepositoryInfo", "GitVersionInformation", "IGit", "IGitBranchCommands",
            "IGitChanges", "IGitCommandRunner", "IGitCommands", "IGitCommitMapper",
            "IGitHistoryCommands", "IGitInformation", "IGitMaintenanceCommands",
            "IGitRemoteCommands", "IGitRepositoryCommands", "IGitVersioning",
            "IGitWorkingTreeCommands", "IRawGitCommandRunner", "RunIfBranchAttribute",
            "RunIfBranchStartsWithAttribute", "RunIfChangedAttribute",
            "RunOnlyIfBranchStartsWithAttribute", "RunOnlyOnBranchAttribute", "SkipIfBranchAttribute",
        ];
        var exportedTypes = typeof(IGit).Assembly.GetExportedTypes()
            .Where(type => type.Namespace != "ModularPipelines.Context")
            .Where(type => type.Namespace != "ModularPipelines.Git.Options"
                || type == typeof(Options.GitVersioningOptions))
            .Select(type => type.FullName!);

        await Assert.That(exportedTypes)
            .IsEquivalentTo(expected.Select(name => $"ModularPipelines.Git.{name}")
                .Append("ModularPipelines.Git.Options.GitVersioningOptions"));
    }

    [Test]
    public async Task Implementation_Types_Are_Internal()
    {
        Type[] implementations = [typeof(GitCommands), typeof(GitCommandRunner), typeof(GitCommitMapper), typeof(GitCommandService)];

        foreach (var implementation in implementations)
        {
            await Assert.That(implementation.IsVisible).IsFalse();
        }
    }
}
