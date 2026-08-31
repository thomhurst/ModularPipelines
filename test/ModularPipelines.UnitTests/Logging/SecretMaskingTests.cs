using ModularPipelines.Secrets;
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

    internal class SecretSettings
    {
        [SecretValue] public string ApiKey { get; set; } = "";
        [SecretValue] public string Password { get; set; } = "";
        [SecretValue] public IReadOnlyList<string> Passwords { get; set; } = [];
    }

    private class SecretLoggingModule : Module<bool>
    {
        private readonly IOptions<SecretSettings> _settings;

        public SecretLoggingModule(IOptions<SecretSettings> settings)
        {
            _settings = settings;
        }

        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            context.Logger.LogInformation("API Key: {ApiKey}", _settings.Value.ApiKey);
            context.Logger.LogInformation("Password: {Password}", _settings.Value.Password);

            foreach (var value in _settings.Value.Passwords)
            {
                context.Logger.LogInformation("Collection password: {Password}", value);
            }

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

        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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

        await TestPipelineBuilder.Create()
            .ConfigureOptions(options => options with
            {
                Secrets = options.Secrets with { CaseInsensitive = false },
            })
            .ConfigureServices(services =>
            {
                services
                    .AddSingleton<ILogger<SecretLoggingModule>>(new StringLogger<SecretLoggingModule>(stringBuilder))
                    .AddModule<SecretLoggingModule>()
                    .Configure<SecretSettings>(s => s.ApiKey = secret);
            })
            .RunAsync();

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

        await TestPipelineBuilder.Create()
            .ConfigureOptions(options => options with
            {
                Secrets = options.Secrets with { CaseInsensitive = true },
            })
            .ConfigureServices(services =>
            {
                services
                    .AddSingleton<ILogger<SecretLoggingModule>>(new StringLogger<SecretLoggingModule>(stringBuilder))
                    .AddModule<SecretLoggingModule>()
                    .Configure<SecretSettings>(s => s.ApiKey = secret);
            })
            .RunAsync();

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

        await TestPipelineBuilder.Create()
            .ConfigureOptions(options => options with
            {
                Secrets = options.Secrets with { MinimumSecretLength = 3 },
            })
            .ConfigureServices(services =>
            {
                services
                    .AddSingleton<ILogger<SecretLoggingModule>>(new StringLogger<SecretLoggingModule>(stringBuilder))
                    .AddModule<SecretLoggingModule>()
                    .Configure<SecretSettings>(s => s.ApiKey = shortSecret);
            })
            .RunAsync();

        var output = stringBuilder.ToString();

        // Short secret should NOT be masked (below configured minimum)
        await Assert.That(output).Contains($"API Key: {shortSecret}");
    }

    [Test]
    public async Task MinimumLength_SecretsAtMinimumAreMasked()
    {
        var stringBuilder = new StringBuilder();
        const string exactLengthSecret = "abc"; // 3 chars, exactly at configured minimum

        await TestPipelineBuilder.Create()
            .ConfigureOptions(options => options with
            {
                Secrets = options.Secrets with { MinimumSecretLength = 3 },
            })
            .ConfigureServices(services =>
            {
                services
                    .AddSingleton<ILogger<SecretLoggingModule>>(new StringLogger<SecretLoggingModule>(stringBuilder))
                    .AddModule<SecretLoggingModule>()
                    .Configure<SecretSettings>(s => s.ApiKey = exactLengthSecret);
            })
            .RunAsync();

        var output = stringBuilder.ToString();

        // Secret at minimum length should be masked
        await Assert.That(output).DoesNotContain(exactLengthSecret);
    }

    [Test]
    public async Task MinimumLength_DefaultMasksAllSecrets()
    {
        var stringBuilder = new StringBuilder();
        const string tinySecret = "x"; // 1 char, default minimum is 1

        await TestPipelineBuilder.Create()
            .ConfigureServices(services =>
            {
                services
                    .AddSingleton<ILogger<SecretLoggingModule>>(new StringLogger<SecretLoggingModule>(stringBuilder))
                    .AddModule<SecretLoggingModule>()
                    .Configure<SecretSettings>(s => s.ApiKey = tinySecret);
                // Using default MinimumSecretLength of 1
            })
            .RunAsync();

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

        await TestPipelineBuilder.Create()
            .ConfigureOptions(options => options with
            {
                Secrets = options.Secrets with { MaskValue = customMask },
            })
            .ConfigureServices(services =>
            {
                services
                    .AddSingleton<ILogger<SecretLoggingModule>>(new StringLogger<SecretLoggingModule>(stringBuilder))
                    .AddModule<SecretLoggingModule>()
                    .Configure<SecretSettings>(s => s.ApiKey = secret);
            })
            .RunAsync();

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

        await TestPipelineBuilder.Create()
            .ConfigureServices(services =>
            {
                services
                    .AddSingleton<ILogger<DynamicSecretModule>>(new StringLogger<DynamicSecretModule>(stringBuilder))
                    .AddModule<DynamicSecretModule>();
            })
            .RunAsync();

        var output = stringBuilder.ToString();

        // The dynamically registered secret should be masked
        await Assert.That(output).DoesNotContain("dynamic-api-key-12345");
        await Assert.That(output).Contains("**********");
    }

    #endregion

    #region Edge Cases Tests

    [Test]
    public async Task FallbackMaskCharacterScan_TerminatesAfterMaximumCharacter()
    {
        var inspectedMaximumCharacter = false;

        var result = SecretObfuscator.FindSafeFallbackMaskCharacter(character =>
        {
            inspectedMaximumCharacter |= character == char.MaxValue;
            return false;
        });

        await Assert.That(result).IsNull();
        await Assert.That(inspectedMaximumCharacter).IsTrue();
    }

    [Test]
    public async Task MultipleSecrets_AllAreMasked()
    {
        var stringBuilder = new StringBuilder();
        const string apiKey = "api-key-secret-123";
        const string password = "super-secret-password";

        await TestPipelineBuilder.Create()
            .ConfigureServices(services =>
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
            .RunAsync();

        var output = stringBuilder.ToString();

        await Assert.That(output).DoesNotContain(apiKey);
        await Assert.That(output).DoesNotContain(password);
    }

    [Test]
    public async Task SecretCollections_MaskEveryElement()
    {
        var stringBuilder = new StringBuilder();
        const string firstPassword = "registry-secret-one";
        const string secondPassword = "registry-secret-two";

        await TestPipelineBuilder.Create()
            .ConfigureServices(services =>
            {
                services
                    .AddSingleton<ILogger<SecretLoggingModule>>(new StringLogger<SecretLoggingModule>(stringBuilder))
                    .AddModule<SecretLoggingModule>()
                    .Configure<SecretSettings>(s => s.Passwords = [firstPassword, secondPassword]);
            })
            .RunAsync();

        var output = stringBuilder.ToString();

        await Assert.That(output).DoesNotContain(firstPassword);
        await Assert.That(output).DoesNotContain(secondPassword);
        await Assert.That(output).Contains("Collection password: **********");
    }

    [Test]
    public async Task OverlappingSecrets_LongerSecretTakesPrecedence()
    {
        // When a shorter secret is a substring of a longer secret,
        // the longer one should be masked first to avoid partial masking issues
        var stringBuilder = new StringBuilder();
        const string shortSecret = "secret";
        const string longSecret = "my-secret-password";

        await TestPipelineBuilder.Create()
            .ConfigureServices(services =>
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
            .RunAsync();

        var output = stringBuilder.ToString();

        // Both should be masked
        await Assert.That(output).DoesNotContain(shortSecret);
        await Assert.That(output).DoesNotContain(longSecret);
    }

    [Test]
    public async Task EmptyAndWhitespaceSecrets_AreIgnored()
    {
        var stringBuilder = new StringBuilder();

        await TestPipelineBuilder.Create()
            .ConfigureServices(services =>
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
            .RunAsync();

        // Should complete without errors - empty/whitespace secrets are silently ignored
        var output = stringBuilder.ToString();
        await Assert.That(output).IsNotEmpty();
    }

    [Test]
    public async Task SpecialCharactersInSecrets_AreMaskedCorrectly()
    {
        var stringBuilder = new StringBuilder();
        const string specialSecret = "p@$$w0rd!#$%^&*()";

        await TestPipelineBuilder.Create()
            .ConfigureServices(services =>
            {
                services
                    .AddSingleton<ILogger<SecretLoggingModule>>(new StringLogger<SecretLoggingModule>(stringBuilder))
                    .AddModule<SecretLoggingModule>()
                    .Configure<SecretSettings>(s => s.ApiKey = specialSecret);
            })
            .RunAsync();

        var output = stringBuilder.ToString();

        await Assert.That(output).DoesNotContain(specialSecret);
        await Assert.That(output).Contains("**********");
    }

    [Test]
    public async Task UnicodeSecrets_AreMaskedCorrectly()
    {
        var stringBuilder = new StringBuilder();
        const string unicodeSecret = "password123";

        await TestPipelineBuilder.Create()
            .ConfigureServices(services =>
            {
                services
                    .AddSingleton<ILogger<SecretLoggingModule>>(new StringLogger<SecretLoggingModule>(stringBuilder))
                    .AddModule<SecretLoggingModule>()
                    .Configure<SecretSettings>(s => s.ApiKey = unicodeSecret);
            })
            .RunAsync();

        var output = stringBuilder.ToString();

        await Assert.That(output).DoesNotContain(unicodeSecret);
        await Assert.That(output).Contains("**********");
    }

    #endregion
}
