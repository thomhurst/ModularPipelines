using ModularPipelines.Attributes;
using ModularPipelines.Engine;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.SonarScanner.Options;
using Moq;

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

    [Test]
    public async Task Sensitive_Define_Values_Are_Discovered_As_Secrets()
    {
        var provider = new SecretProvider(
            Mock.Of<IOptionsProvider>(),
            Mock.Of<IBuildSystemSecretMasker>(),
            Microsoft.Extensions.Options.Options.Create(new SecretMaskingOptions()),
            Mock.Of<Microsoft.Extensions.Logging.ILogger<SecretProvider>>());
        var options = new SonarScannerExecuteOptions
        {
            Define =
            [
                new KeyValue("sonar.projectKey", "visible-project"),
                new KeyValue("SONAR.TOKEN", "token-from-define"),
                new KeyValue("sonar.login", "legacy-login-secret"),
            ],
        };

        var secrets = provider.GetSecretsInObject(options).ToList();

        await Assert.That(secrets).IsEquivalentTo(["token-from-define", "legacy-login-secret"]);
        await Assert.That(secrets).DoesNotContain("visible-project");
    }

    private List<string> BuildArguments(object options)
    {
        var model = _modelProvider.GetCommandModel(options.GetType());
        return _argumentBuilder.BuildArguments(model, options);
    }
}
