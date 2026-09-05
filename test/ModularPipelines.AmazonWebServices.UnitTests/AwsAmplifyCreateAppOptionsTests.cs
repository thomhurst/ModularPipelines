using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Models;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.AmazonWebServices.UnitTests;

public class AwsAmplifyCreateAppOptionsTests
{
    [Test]
    public async Task CreateApp_Joins_Environment_Variables()
    {
        var arguments = BuildArguments(new AwsAmplifyCreateAppOptions("sample-app")
        {
            EnvironmentVariables =
            [
                new KeyValue("FIRST", "one"),
                new KeyValue("SECOND", "two"),
            ],
        });

        await AssertArguments(arguments,
        [
            "--name",
            "sample-app",
            "--environment-variables",
            "FIRST=one,SECOND=two",
        ]);
    }
}
