using ModularPipelines.Attributes;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Models;
using ModularPipelines.SonarScanner.Options;

namespace ModularPipelines.UnitTests.Attributes;

public class SonarScannerOptionsTests
{
    private readonly CommandModelProvider _modelProvider = new();
    private readonly CommandArgumentBuilder _argumentBuilder = new();

    [Test]
    public async Task Execute_Renders_Defines_Flags_And_Properties()
    {
        var arguments = BuildArguments(new SonarScannerExecuteOptions
        {
            Define =
            [
                new KeyValue("sonar.branch.name", "main"),
                new KeyValue("sonar.qualitygate.wait", "true"),
            ],
            Debug = true,
            ProjectKey = "example-project",
            Sources = "src",
            Token = "token-value",
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "--define",
            "sonar.branch.name=main",
            "--define",
            "sonar.qualitygate.wait=true",
            "--debug",
            "-Dsonar.projectKey=example-project",
            "-Dsonar.sources=src",
            "-Dsonar.token=token-value",
        ]);
    }

    [Test]
    public async Task Token_Is_Marked_As_Secret()
    {
        var token = typeof(SonarScannerExecuteOptions).GetProperty(nameof(SonarScannerExecuteOptions.Token));

        await Assert.That(token!.IsDefined(typeof(SecretValueAttribute), inherit: true)).IsTrue();
    }

    private List<string> BuildArguments(object options)
    {
        var model = _modelProvider.GetCommandModel(options.GetType());
        return _argumentBuilder.BuildArguments(model, options);
    }
}
