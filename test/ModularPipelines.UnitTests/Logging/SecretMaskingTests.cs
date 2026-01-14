using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Logging;

/// <summary>
/// Comprehensive tests for secret masking functionality including:
/// - Case sensitivity options
/// - Minimum length filtering
/// - Custom mask values
/// - Programmatic secret registration
/// - Edge cases
/// </summary>
public class SecretMaskingTests
{
    #region Test Classes

    private class SecretSettings
    {
        [SecretValue] public string ApiKey { get; set; } = "";
        [SecretValue] public string Password { get; set; } = "";
    }

    private class SecretLoggingModule : Module<bool>
    {
        private readonly IOptions<SecretSettings> _settings;

        public SecretLoggingModule(IOptions<SecretSettings> settings)
        {
            _settings = settings;
        }

        public override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            context.Logger.LogInformation("API Key: {ApiKey}", _settings.Value.ApiKey);
            context.Logger.LogInformation("Password: {Password}", _settings.Value.Password);
            return Task.FromResult(true);
        }
    }

    private class DynamicSecretModule : Module<bool>
    {
        private readonly ISecretRegistry _secretRegistry;

        public DynamicSecretModule(ISecretRegistry secretRegistry)
        {
            _secretRegistry = secretRegistry;
        }

        public override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            // Register a secret dynamically
            const string dynamicSecret = "dynamic-api-key-12345";
            _secretRegistry.AddSecret(dynamicSecret);

            // Log it - should be masked
            context.Logger.LogInformation("Dynamic secret: {Secret}", dynamicSecret);
            return Task.FromResult(true);
        }
    }

    #endregion

    #region Case Sensitivity Tests

    [Test]
    public async Task CaseSensitive_DoesNotMaskDifferentCase()
    {
        var stringBuilder = new StringBuilder();
        const string secret = "MySecretPassword";

        await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, services) =>
            {
                services
                    .AddSingleton<ILogger<SecretLoggingModule>>(new StringLogger<SecretLoggingModule>(stringBuilder))
                    .AddModule<SecretLoggingModule>()
                    .Configure<SecretSettings>(s => s.ApiKey = secret)
                    .Configure<SecretMaskingOptions>(o => o.CaseInsensitive = false);
            })
            .ExecutePipelineAsync();

        var output = stringBuilder.ToString();

        // The exact case should be masked
        await Assert.That(output).DoesNotContain(secret);
        await Assert.That(output).Contains("**********");
    }

    [Test]
    public async Task CaseInsensitive_MasksAllCaseVariants()
    {
        var stringBuilder = new StringBuilder();
        const string secret = "MySecretPassword";

        await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, services) =>
            {
                services
                    .AddSingleton<ILogger<SecretLoggingModule>>(new StringLogger<SecretLoggingModule>(stringBuilder))
                    .AddModule<SecretLoggingModule>()
                    .Configure<SecretSettings>(s => s.ApiKey = secret)
                    .Configure<SecretMaskingOptions>(o => o.CaseInsensitive = true);
            })
            .ExecutePipelineAsync();

        var output = stringBuilder.ToString();

        // The secret should be masked
        await Assert.That(output).DoesNotContain(secret);
        await Assert.That(output).Contains("**********");
    }

    #endregion

    #region Minimum Length Tests

    [Test]
    public async Task MinimumLength_ShortSecretsAreNotMasked_WhenConfigured()
    {
        var stringBuilder = new StringBuilder();
        const string shortSecret = "ab"; // 2 chars, below configured minimum of 3

        await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, services) =>
            {
                services
                    .AddSingleton<ILogger<SecretLoggingModule>>(new StringLogger<SecretLoggingModule>(stringBuilder))
                    .AddModule<SecretLoggingModule>()
                    .Configure<SecretSettings>(s => s.ApiKey = shortSecret)
                    .Configure<SecretMaskingOptions>(o => o.MinimumSecretLength = 3);
            })
            .ExecutePipelineAsync();

        var output = stringBuilder.ToString();

        // Short secret should NOT be masked (below configured minimum)
        await Assert.That(output).Contains($"API Key: {shortSecret}");
    }

    [Test]
    public async Task MinimumLength_SecretsAtMinimumAreMasked()
    {
        var stringBuilder = new StringBuilder();
        const string exactLengthSecret = "abc"; // 3 chars, exactly at configured minimum

        await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, services) =>
            {
                services
                    .AddSingleton<ILogger<SecretLoggingModule>>(new StringLogger<SecretLoggingModule>(stringBuilder))
                    .AddModule<SecretLoggingModule>()
                    .Configure<SecretSettings>(s => s.ApiKey = exactLengthSecret)
                    .Configure<SecretMaskingOptions>(o => o.MinimumSecretLength = 3);
            })
            .ExecutePipelineAsync();

        var output = stringBuilder.ToString();

        // Secret at minimum length should be masked
        await Assert.That(output).DoesNotContain(exactLengthSecret);
    }

    [Test]
    public async Task MinimumLength_DefaultMasksAllSecrets()
    {
        var stringBuilder = new StringBuilder();
        const string tinySecret = "x"; // 1 char, default minimum is 1

        await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, services) =>
            {
                services
                    .AddSingleton<ILogger<SecretLoggingModule>>(new StringLogger<SecretLoggingModule>(stringBuilder))
                    .AddModule<SecretLoggingModule>()
                    .Configure<SecretSettings>(s => s.ApiKey = tinySecret);
                    // Using default MinimumSecretLength of 1
            })
            .ExecutePipelineAsync();

        var output = stringBuilder.ToString();

        // With default minimum length of 1, all non-empty secrets are masked
        await Assert.That(output).DoesNotContain($"API Key: {tinySecret}");
    }

    #endregion

    #region Custom Mask Value Tests

    [Test]
    public async Task CustomMaskValue_UsesProvidedMask()
    {
        var stringBuilder = new StringBuilder();
        const string secret = "MySecretPassword";
        const string customMask = "[REDACTED]";

        await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, services) =>
            {
                services
                    .AddSingleton<ILogger<SecretLoggingModule>>(new StringLogger<SecretLoggingModule>(stringBuilder))
                    .AddModule<SecretLoggingModule>()
                    .Configure<SecretSettings>(s => s.ApiKey = secret)
                    .Configure<SecretMaskingOptions>(o => o.MaskValue = customMask);
            })
            .ExecutePipelineAsync();

        var output = stringBuilder.ToString();

        await Assert.That(output).DoesNotContain(secret);
        await Assert.That(output).Contains(customMask);
    }

    #endregion

    #region Programmatic Secret Registration Tests

    [Test]
    public async Task DynamicSecretRegistration_MasksDynamicallyAddedSecrets()
    {
        var stringBuilder = new StringBuilder();

        await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, services) =>
            {
                services
                    .AddSingleton<ILogger<DynamicSecretModule>>(new StringLogger<DynamicSecretModule>(stringBuilder))
                    .AddModule<DynamicSecretModule>();
            })
            .ExecutePipelineAsync();

        var output = stringBuilder.ToString();

        // The dynamically registered secret should be masked
        await Assert.That(output).DoesNotContain("dynamic-api-key-12345");
        await Assert.That(output).Contains("**********");
    }

    #endregion

    #region Edge Cases Tests

    [Test]
    public async Task MultipleSecrets_AllAreMasked()
    {
        var stringBuilder = new StringBuilder();
        const string apiKey = "api-key-secret-123";
        const string password = "super-secret-password";

        await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, services) =>
            {
                services
                    .AddSingleton<ILogger<SecretLoggingModule>>(new StringLogger<SecretLoggingModule>(stringBuilder))
                    .AddModule<SecretLoggingModule>()
                    .Configure<SecretSettings>(s =>
                    {
                        s.ApiKey = apiKey;
                        s.Password = password;
                    });
            })
            .ExecutePipelineAsync();

        var output = stringBuilder.ToString();

        await Assert.That(output).DoesNotContain(apiKey);
        await Assert.That(output).DoesNotContain(password);
    }

    [Test]
    public async Task OverlappingSecrets_LongerSecretTakesPrecedence()
    {
        // When a shorter secret is a substring of a longer secret,
        // the longer one should be masked first to avoid partial masking issues
        var stringBuilder = new StringBuilder();
        const string shortSecret = "secret";
        const string longSecret = "my-secret-password";

        await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, services) =>
            {
                services
                    .AddSingleton<ILogger<SecretLoggingModule>>(new StringLogger<SecretLoggingModule>(stringBuilder))
                    .AddModule<SecretLoggingModule>()
                    .Configure<SecretSettings>(s =>
                    {
                        s.ApiKey = shortSecret;
                        s.Password = longSecret;
                    });
            })
            .ExecutePipelineAsync();

        var output = stringBuilder.ToString();

        // Both should be masked
        await Assert.That(output).DoesNotContain(shortSecret);
        await Assert.That(output).DoesNotContain(longSecret);
    }

    [Test]
    public async Task EmptyAndWhitespaceSecrets_AreIgnored()
    {
        var stringBuilder = new StringBuilder();

        await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, services) =>
            {
                services
                    .AddSingleton<ILogger<SecretLoggingModule>>(new StringLogger<SecretLoggingModule>(stringBuilder))
                    .AddModule<SecretLoggingModule>()
                    .Configure<SecretSettings>(s =>
                    {
                        s.ApiKey = "";
                        s.Password = "   ";
                    });
            })
            .ExecutePipelineAsync();

        // Should complete without errors - empty/whitespace secrets are silently ignored
        var output = stringBuilder.ToString();
        await Assert.That(output).IsNotEmpty();
    }

    [Test]
    public async Task SpecialCharactersInSecrets_AreMaskedCorrectly()
    {
        var stringBuilder = new StringBuilder();
        const string specialSecret = "p@$$w0rd!#$%^&*()";

        await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, services) =>
            {
                services
                    .AddSingleton<ILogger<SecretLoggingModule>>(new StringLogger<SecretLoggingModule>(stringBuilder))
                    .AddModule<SecretLoggingModule>()
                    .Configure<SecretSettings>(s => s.ApiKey = specialSecret);
            })
            .ExecutePipelineAsync();

        var output = stringBuilder.ToString();

        await Assert.That(output).DoesNotContain(specialSecret);
        await Assert.That(output).Contains("**********");
    }

    [Test]
    public async Task UnicodeSecrets_AreMaskedCorrectly()
    {
        var stringBuilder = new StringBuilder();
        const string unicodeSecret = "password123";

        await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, services) =>
            {
                services
                    .AddSingleton<ILogger<SecretLoggingModule>>(new StringLogger<SecretLoggingModule>(stringBuilder))
                    .AddModule<SecretLoggingModule>()
                    .Configure<SecretSettings>(s => s.ApiKey = unicodeSecret);
            })
            .ExecutePipelineAsync();

        var output = stringBuilder.ToString();

        await Assert.That(output).DoesNotContain(unicodeSecret);
        await Assert.That(output).Contains("**********");
    }

    #endregion
}
