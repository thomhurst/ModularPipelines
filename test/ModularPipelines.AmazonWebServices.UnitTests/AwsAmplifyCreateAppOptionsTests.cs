using System.Text.Json;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Models;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.AmazonWebServices.UnitTests;

public class AwsAmplifyCreateAppOptionsTests
{
    [Test]
    public async Task CreateApp_Joins_Environment_Variables()
    {
        // JSON supplies constructor inputs when regeneration makes CLI options required.
        var options = JsonSerializer.Deserialize<AwsAmplifyCreateAppOptions>("""{"Name":"test-app"}""")!;
        options.EnvironmentVariables =
        [
            new KeyValue("FIRST", "one"),
            new KeyValue("SECOND", "two"),
        ];
        var arguments = BuildArguments(options);

        await AssertArguments(arguments,
        [
            "--name",
            "test-app",
            "--environment-variables",
            "FIRST=one,SECOND=two",
        ]);
    }
}
