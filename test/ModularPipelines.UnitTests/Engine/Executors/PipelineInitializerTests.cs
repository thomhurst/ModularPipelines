using System.Collections.Specialized;
using ModularPipelines.Constants;
using ModularPipelines.Engine.Executors;
using Spectre.Console;

namespace ModularPipelines.UnitTests.Engine.Executors;

public class PipelineInitializerTests
{
    [Test]
    public async Task EnvironmentVariables_AreRenderedAsMaskedSortedTable()
    {
        var variables = new OrderedDictionary
        {
            ["SECOND"] = "two",
            ["FIRST"] = "secret",
        };

        var table = PipelineInitializer.CreateEnvironmentVariablesTable(
            variables,
            value => value == "secret" ? "***" : value);
        var output = Render(table);

        await Assert.That(output).Contains("Environment variables");
        await Assert.That(output).Contains("***");
        await Assert.That(output).Contains("two");
        await Assert.That(output.IndexOf("FIRST", StringComparison.Ordinal))
            .IsLessThan(output.IndexOf("SECOND", StringComparison.Ordinal));
    }

    [Test]
    public async Task EnvironmentVariables_MaskLongValuesInsteadOfTruncatingThem()
    {
        var variables = new OrderedDictionary
        {
            ["LONG_VALUE"] = new string('x', 200),
        };

        var table = PipelineInitializer.CreateEnvironmentVariablesTable(
            variables,
            value => value,
            consoleWidth: 40);
        var output = Render(table, width: 40);

        await Assert.That(output).Contains("LONG_VALUE");
        await Assert.That(output).Contains(LoggingConstants.SecretMask);
        await Assert.That(output).DoesNotContain("x");
    }

    [Test]
    public async Task EnvironmentVariables_MaskRawValuesBasedOnActualRenderWidth()
    {
        var variables = new OrderedDictionary
        {
            ["CONFIG"] = new string('x', 32),
        };

        var table = PipelineInitializer.CreateEnvironmentVariablesTable(
            variables,
            value => value,
            consoleWidth: 40);
        var output = Render(table, width: 40);

        await Assert.That(output).Contains(LoggingConstants.SecretMask);
        await Assert.That(output).DoesNotContain("x");
    }

    [Test]
    public async Task EnvironmentVariables_RecheckSafetyWhenRenderWidthShrinks()
    {
        var rawValue = new string('x', 60);
        var variables = new OrderedDictionary
        {
            ["CONFIG"] = rawValue,
        };

        var table = PipelineInitializer.CreateEnvironmentVariablesTable(
            variables,
            value => value,
            consoleWidth: 120);
        var output = Render(table, width: 40);

        await Assert.That(output).Contains(LoggingConstants.SecretMask);
        await Assert.That(output).DoesNotContain("x");
    }

    [Test]
    public async Task EnvironmentVariables_IncludeHeaderWidthInRawValueSafety()
    {
        var variables = new OrderedDictionary
        {
            ["X"] = new string('x', 32),
        };

        var table = PipelineInitializer.CreateEnvironmentVariablesTable(
            variables,
            value => value,
            consoleWidth: 40);
        var output = Render(table, width: 40);

        await Assert.That(output).Contains(LoggingConstants.SecretMask);
        await Assert.That(output).DoesNotContain(new string('x', 32));
    }

    [Test]
    public async Task EnvironmentVariables_RenderRawValuesThatFitActualWidth()
    {
        var rawValue = new string('x', 27);
        var variables = new OrderedDictionary
        {
            ["CONFIG"] = rawValue,
        };

        var table = PipelineInitializer.CreateEnvironmentVariablesTable(
            variables,
            value => value,
            consoleWidth: 40);
        var output = Render(table, width: 40);

        await Assert.That(output).Contains(rawValue);
        await Assert.That(output).DoesNotContain(LoggingConstants.SecretMask);
    }

    [Test]
    public async Task EnvironmentVariables_RenderObfuscatedLongValues()
    {
        var variables = new OrderedDictionary
        {
            ["LONG_VALUE"] = new string('x', 200),
        };

        var table = PipelineInitializer.CreateEnvironmentVariablesTable(
            variables,
            _ => "[REDACTED]",
            maskValue: "[REDACTED]");
        var output = Render(table);

        await Assert.That(output).Contains("[REDACTED]");
        await Assert.That(output).DoesNotContain("x");
    }

    [Test]
    public async Task EnvironmentVariables_MaskPartiallyObfuscatedCompositeValues()
    {
        const string rawValue = """{"known":"registered","unknown":"unregistered"}""";
        const string partiallyObfuscatedValue = """{"known":"********","unknown":"unregistered"}""";
        var variables = new OrderedDictionary
        {
            ["CONFIG"] = rawValue,
        };

        var table = PipelineInitializer.CreateEnvironmentVariablesTable(
            variables,
            _ => partiallyObfuscatedValue);
        var output = Render(table);

        await Assert.That(output).Contains(LoggingConstants.SecretMask);
        await Assert.That(output).DoesNotContain("unregistered");
        await Assert.That(output).DoesNotContain(partiallyObfuscatedValue);
    }

    [Test]
    [Arguments("GITHUB_TOKEN")]
    [Arguments("client_secret")]
    [Arguments("DatabasePassword")]
    [Arguments("GPG_PASSPHRASE")]
    [Arguments("SSH_PASSPHRASE")]
    [Arguments("RABBITMQ_DEFAULT_PASS")]
    [Arguments("P4PASSWD")]
    [Arguments("COSIGN_PKCS11_PIN")]
    [Arguments("API_KEY")]
    [Arguments("SERVICE_PWD")]
    [Arguments("credential")]
    [Arguments("DOCKER_AUTH_CONFIG")]
    [Arguments("NPM_CONFIG__AUTH")]
    [Arguments("GIT_AUTHOR_TOKEN")]
    [Arguments("GIT_AUTHOR_PASSWORD")]
    [Arguments("GIT_CONFIG_VALUE_0")]
    [Arguments("git_config_key_0")]
    [Arguments("Git_CONFIG_KEY_0")]
    [Arguments("OTEL_EXPORTER_OTLP_HEADERS")]
    [Arguments("OTEL_EXPORTER_OTLP_TRACES_HEADERS")]
    [Arguments("pwd")]
    [Arguments("ALL_PROXY")]
    [Arguments("CLOUDAMQP_URL")]
    [Arguments("DATABASE_URL")]
    [Arguments("HTTP_PROXY")]
    [Arguments("HTTPS_PROXY")]
    [Arguments("MONGODB_URI")]
    [Arguments("PIP_EXTRA_INDEX_URL")]
    [Arguments("PIP_INDEX_URL")]
    [Arguments("REDIS_URL")]
    [Arguments("AZURE_DEVOPS_EXT_PAT")]
    [Arguments("AZURE_APPCONFIG_CONNECTION_STRING")]
    [Arguments("IDENTITY_HEADER")]
    [Arguments("ConnectionStrings__Default")]
    [Arguments("SQLCONNSTR_Main")]
    [Arguments("SLACK_WEBHOOK_URL")]
    [Arguments("VCAP_SERVICES")]
    [Arguments("AzureWebJobsStorage")]
    [Arguments("AWS_CONTAINER_AUTHORIZATION_TOKEN")]
    [Arguments("VSS_NUGET_EXTERNAL_FEED_ENDPOINTS")]
    public async Task EnvironmentVariables_MaskSensitiveNamesWithoutRegisteredSecret(
        string variableName)
    {
        const string unregisteredSecret = "unregistered-secret-value";
        var variables = new OrderedDictionary
        {
            [variableName] = unregisteredSecret,
        };

        var table = PipelineInitializer.CreateEnvironmentVariablesTable(
            variables,
            value => value);
        var output = Render(table);

        await Assert.That(output).Contains(LoggingConstants.SecretMask);
        await Assert.That(output).DoesNotContain(unregisteredSecret);
    }

    [Test]
    [Arguments("[REDACTED]", "[REDACTED]")]
    [Arguments(" ", LoggingConstants.SecretMask)]
    public async Task EnvironmentVariables_HonorConfiguredSecretMask(
        string configuredMask,
        string expectedMask)
    {
        var variables = new OrderedDictionary
        {
            ["API_KEY"] = "unregistered-secret-value",
        };

        var table = PipelineInitializer.CreateEnvironmentVariablesTable(
            variables,
            value => value,
            configuredMask);
        var output = Render(table);

        await Assert.That(output).Contains(expectedMask);
        await Assert.That(output).DoesNotContain("unregistered-secret-value");
    }

    [Test]
    [Arguments("PWD")]
    [Arguments("OLDPWD")]
    [Arguments("SSH_AUTH_SOCK")]
    [Arguments("XAUTHORITY")]
    [Arguments("APPVEYOR_REPO_COMMIT_AUTHOR")]
    [Arguments("APPVEYOR_REPO_COMMIT_AUTHOR_EMAIL")]
    [Arguments("AWS_CONTAINER_AUTHORIZATION_TOKEN_FILE")]
    [Arguments("AWS_SHARED_CREDENTIALS_FILE")]
    [Arguments("AWS_WEB_IDENTITY_TOKEN_FILE")]
    [Arguments("AZURE_FEDERATED_TOKEN_FILE")]
    [Arguments("AZURE_STORAGE_AUTH_MODE")]
    [Arguments("BUILD_SOURCEVERSIONAUTHOR")]
    [Arguments("CI_COMMIT_AUTHOR")]
    [Arguments("CLOUDSDK_AUTH_CREDENTIAL_FILE_OVERRIDE")]
    [Arguments("GOOGLE_APPLICATION_CREDENTIALS")]
    [Arguments("NPM_CONFIG_AUTH_TYPE")]
    [Arguments("NUGET_CREDENTIALPROVIDER_SESSIONTOKENCACHE_ENABLED")]
    [Arguments("GIT_AUTHOR_NAME")]
    [Arguments("GIT_AUTHOR_EMAIL")]
    [Arguments("GIT_AUTHOR_DATE")]
    [Arguments("GIT_ASKPASS")]
    [Arguments("GIT_CONFIG_KEY_0")]
    [Arguments("TOKENIZERS_PARALLELISM")]
    [Arguments("REGISTRY_AUTH_FILE")]
    [Arguments("YARN_NPM_ALWAYS_AUTH")]
    public async Task EnvironmentVariables_DoNotMaskKnownNonSecretNames(
        string variableName)
    {
        const string workingDirectory = "/home/runner/work";
        var variables = new OrderedDictionary
        {
            [variableName] = workingDirectory,
        };

        var table = PipelineInitializer.CreateEnvironmentVariablesTable(
            variables,
            value => value);
        var output = Render(table);

        await Assert.That(output).Contains(workingDirectory);
        await Assert.That(output).DoesNotContain(LoggingConstants.SecretMask);
    }

    [Test]
    [Arguments("url.https://oauth2:secret@gitlab.example/.insteadOf")]
    [Arguments("url.https://token@gitlab.example/.insteadOf")]
    public async Task EnvironmentVariables_MaskGitConfigKeysWithEmbeddedCredentials(
        string gitConfigKey)
    {
        var variables = new OrderedDictionary
        {
            ["GIT_CONFIG_KEY_0"] = gitConfigKey,
        };

        var table = PipelineInitializer.CreateEnvironmentVariablesTable(
            variables,
            value => value);
        var output = Render(table);

        await Assert.That(output).Contains(LoggingConstants.SecretMask);
        await Assert.That(output).DoesNotContain(gitConfigKey);
    }

    [Test]
    public async Task EnvironmentVariables_MaskValuesWithEmbeddedNewlines()
    {
        var variables = new OrderedDictionary
        {
            ["PUBLIC_VALUE"] = "first\r\nsecond",
        };

        var table = PipelineInitializer.CreateEnvironmentVariablesTable(
            variables,
            value => value);
        var output = Render(table);

        await Assert.That(output).Contains(LoggingConstants.SecretMask);
        await Assert.That(output).DoesNotContain("first");
        await Assert.That(output).DoesNotContain("second");
    }

    private static string Render(Table table, int width = 120)
    {
        using var writer = new StringWriter();
        var console = AnsiConsole.Create(new AnsiConsoleSettings
        {
            Out = new AnsiConsoleOutput(writer),
            Ansi = AnsiSupport.No,
            ColorSystem = ColorSystemSupport.NoColors,
        });
        console.Profile.Width = width;

        console.Write(table);
        return writer.ToString();
    }
}
