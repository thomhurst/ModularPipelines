using ModularPipelines.Exceptions;
using ModularPipelines.Google.Options;
using ModularPipelines.Google.Services;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Google.UnitTests;

public class GcloudResourceArgumentTests
{
    [Test]
    [Arguments(typeof(GcloudKmsKeyrings), "DeleteAsync", typeof(GcloudKmsKeyringsDeleteOptions))]
    [Arguments(typeof(GcloudMetastoreServicesMigrations), "DescribeAsync", typeof(GcloudMetastoreServicesMigrationsDescribeOptions))]
    [Arguments(typeof(GcloudMetastoreServicesMigrations), "DeleteAsync", typeof(GcloudMetastoreServicesMigrationsDeleteOptions))]
    public async Task Resource_Service_Requires_Options(Type serviceType, string methodName, Type optionsType)
    {
        var parameter = serviceType.GetMethod(methodName)!.GetParameters()[0];
        await Assert.That(parameter.ParameterType).IsEqualTo(optionsType);
        await Assert.That(parameter.IsOptional).IsFalse();
    }

    [Test]
    [Arguments(typeof(GcloudKmsKeyringsDeleteOptions), "projects/project/locations/global/keyRings/ring")]
    [Arguments(typeof(GcloudMetastoreServicesMigrationsDescribeOptions), "projects/project/locations/us-central1/services/service/migrations/migration")]
    [Arguments(typeof(GcloudMetastoreServicesMigrationsDeleteOptions), "projects/project/locations/us-central1/services/service/migrations/migration")]
    public async Task Fully_Qualified_Resource_Needs_No_Selector_Options(Type optionsType, string resource)
    {
        var options = CreateOptions(optionsType, resource);
        await AssertArguments(BuildArguments(options), [resource]);
    }

    [Test]
    [Arguments(typeof(GcloudKmsKeyringsDeleteOptions), false)]
    [Arguments(typeof(GcloudMetastoreServicesMigrationsDescribeOptions), true)]
    [Arguments(typeof(GcloudMetastoreServicesMigrationsDeleteOptions), true)]
    public async Task Short_Resource_Name_Renders_Before_Selector_Options(Type optionsType, bool hasService)
    {
        var options = CreateOptions(optionsType, "resource");
        optionsType.GetProperty("Location")!.SetValue(options, "us-central1");
        var expected = new List<string> { "resource", "--location=us-central1" };
        if (hasService)
        {
            optionsType.GetProperty("Service")!.SetValue(options, "service");
            expected.Add("--service=service");
        }

        await AssertArguments(BuildArguments(options), expected);
    }

    [Test]
    [Arguments(typeof(GcloudKmsKeyringsDeleteOptions))]
    [Arguments(typeof(GcloudMetastoreServicesMigrationsDescribeOptions))]
    [Arguments(typeof(GcloudMetastoreServicesMigrationsDeleteOptions))]
    public async Task Missing_Resource_Is_Rejected(Type optionsType)
    {
        var options = CreateOptions(optionsType, "");
        await Assert.That(() => BuildArguments(options)).Throws<CommandOptionsValidationException>();
    }

    private static object CreateOptions(Type optionsType, string resource) =>
        optionsType.GetConstructor([typeof(string)])!.Invoke([resource]);
}
