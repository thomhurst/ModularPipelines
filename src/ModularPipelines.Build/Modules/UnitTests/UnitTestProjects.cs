namespace ModularPipelines.Build.Modules.UnitTests;

/// <summary>
/// Unit test modules registered by the build pipeline, excluding the options generator
/// tests that can be completed by the fast-fail job.
/// </summary>
public static class UnitTestProjects
{
    public static Type[] ModuleTypes { get; } =
    [
        typeof(RunCoreUnitTestsModule),
        typeof(RunSourceGeneratorUnitTestsModule),
        typeof(RunAnsibleUnitTestsModule),
        typeof(RunArgoCdUnitTestsModule),
        typeof(RunCosignUnitTestsModule),
        typeof(RunDockerUnitTestsModule),
        typeof(RunEksctlUnitTestsModule),
        typeof(RunEmailUnitTestsModule),
        typeof(RunFtpUnitTestsModule),
        typeof(RunGitHubUnitTestsModule),
        typeof(RunHadolintUnitTestsModule),
        typeof(RunJavaUnitTestsModule),
        typeof(RunJqUnitTestsModule),
        typeof(RunLiquibaseUnitTestsModule),
        typeof(RunMicrosoftTeamsUnitTestsModule),
        typeof(RunSlackUnitTestsModule),
        typeof(RunNerdbankGitVersioningUnitTestsModule),
        typeof(RunNodeUnitTestsModule),
        typeof(RunOpenTelemetryUnitTestsModule),
        typeof(RunShellcheckUnitTestsModule),
        typeof(RunSnykUnitTestsModule),
        typeof(RunSonarScannerUnitTestsModule),
        typeof(RunTrivyUnitTestsModule),
        typeof(RunAzureUnitTestsModule),
        typeof(RunAzurePipelinesUnitTestsModule),
        typeof(RunTeamCityUnitTestsModule),
        typeof(RunTestingUnitTestsModule),
        typeof(RunAnalyzersUnitTestsModule),
        typeof(RunConsumerAnalyzersUnitTestsModule),
        typeof(RunDistributedUnitTestsModule),
        typeof(RunDistributedRedisUnitTestsModule),
        typeof(RunDistributedArtifactsS3UnitTestsModule),
        typeof(RunDistributedSignalRUnitTestsModule),
        typeof(RunDistributedDiscoveryRedisUnitTestsModule),
        .. GeneratedOptionsUnitTestProjects.ModuleTypes,
    ];
}
