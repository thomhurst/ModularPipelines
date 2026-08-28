using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Logging;

public class ConfigurationSectionSecretMaskingTests
{
    private sealed class ConfigurationLoggingModule(IConfiguration configuration) : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            context.Logger.LogInformation("First secret: {Secret}", configuration["Secrets:ApiKey"]);
            context.Logger.LogInformation("Nested secret: {Secret}", configuration["Secrets:Database:Password"]);
            context.Logger.LogInformation("Section secret: {Secret}", configuration["Secrets"]);
            context.Logger.LogInformation("Connection: {Secret}", configuration["ConnectionStrings:Database"]);
            context.Logger.LogInformation("Public value: {Value}", configuration["Public:Value"]);
            return Task.FromResult(true);
        }
    }

    [Test]
    public async Task FluentConfigurationMasksSectionValuesAndNestedLeaves()
    {
        var output = new StringBuilder();
        using var builder = CreateBuilder(output);
        builder.MaskConfigurationSection("Secrets");

        await builder.RunAsync();

        var log = output.ToString();
        await Assert.That(log).DoesNotContain("api-secret-123");
        await Assert.That(log).DoesNotContain("database-secret-456");
        await Assert.That(log).DoesNotContain("section-secret-789");
        await Assert.That(log).Contains("connection-visible");
        await Assert.That(log).Contains("public-visible");
        await Assert.That(log).Contains("**********");
    }

    [Test]
    public async Task OptionsCanMaskMultipleSections()
    {
        var output = new StringBuilder();
        using var builder = CreateBuilder(output);
        builder.Services.Configure<SecretMaskingOptions>(options =>
            options.MaskedConfigurationSections = ["Secrets", "ConnectionStrings", "Missing"]);

        await builder.RunAsync();

        var log = output.ToString();
        await Assert.That(log).DoesNotContain("api-secret-123");
        await Assert.That(log).DoesNotContain("database-secret-456");
        await Assert.That(log).DoesNotContain("section-secret-789");
        await Assert.That(log).DoesNotContain("connection-visible");
        await Assert.That(log).Contains("public-visible");
    }

    [Test]
    [Arguments(null)]
    [Arguments("")]
    [Arguments("   ")]
    public async Task FluentConfigurationRejectsInvalidPaths(string? sectionPath)
    {
        using var builder = TestPipelineBuilder.Create();

        await Assert.That(() => builder.MaskConfigurationSection(sectionPath!))
            .Throws<ArgumentException>();
    }

    private static PipelineBuilder CreateBuilder(StringBuilder output)
    {
        var builder = TestPipelineBuilder.Create();
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Secrets"] = "section-secret-789",
            ["Secrets:ApiKey"] = "api-secret-123",
            ["Secrets:Database:Password"] = "database-secret-456",
            ["ConnectionStrings:Database"] = "connection-visible",
            ["Public:Value"] = "public-visible",
        });
        builder.Services.AddSingleton<ILogger<ConfigurationLoggingModule>>(
            new StringLogger<ConfigurationLoggingModule>(output));
        builder.AddModule<ConfigurationLoggingModule>();
        return builder;
    }
}
