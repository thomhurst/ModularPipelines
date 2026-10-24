using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public partial class AwsCliScraperTests
{
    [Test]
    [Arguments("modify-cache-cluster")]
    [Arguments("modify-replication-group")]
    public async Task Strategy_Explanations_Do_Not_Hide_The_Canonical_Enum(string commandName)
    {
        // AWS CLI 2.38.1 includes both an explanatory list and the canonical choices.
        const string help = """
            OPTIONS
                   --auth-token-update-strategy (string)
                      Specifies the strategy to use to update the AUTH token. This parameter must be specified with the auth-token parameter. Possible values:

                      • ROTATE - default, if no update strategy is provided

                      • SET - allowed only after ROTATE

                      • DELETE - allowed only when transitioning to RBAC

                      For more information, see Authenticating Users with AUTH

                      Possible values:

                      • SET

                      • ROTATE

                      • DELETE

                   --auth-token (string)
                      The password used to access a password protected server.
            """;
        var scraper = new TestAwsCliScraper();
        var command = (await scraper.Parse(["aws", "elasticache", commandName], help))!;
        var enhancer = new OptionTypeEnhancer(
            new OptionTypeDetectorPipeline([], NullLogger<OptionTypeDetectorPipeline>.Instance),
            NullLogger<OptionTypeEnhancer>.Instance);
        var tool = await enhancer.EnhanceManualOverridesAsync(
            scraper.CreateToolDefinition() with { Commands = [command] });
        var strategy = tool.Commands.Single().Options.Single(option => option.PropertyName == "AuthTokenUpdateStrategy");
        await Assert.That(strategy.EnumDefinition).IsNotNull();
        await Assert.That(strategy.EnumDefinition!.Values.Select(value => value.CliValue))
            .IsEquivalentTo(["SET", "ROTATE", "DELETE"]);
        await Assert.That(strategy.IsSecret).IsFalse();
        await Assert.That(tool.Commands.Single().Options.Single(option => option.PropertyName == "AuthToken").IsSecret).IsTrue();
        var files = await new OptionsClassGenerator().GenerateAsync(tool);
        await Assert.That(files.Single().Content).Contains($"public {strategy.EnumDefinition.EnumName}? AuthTokenUpdateStrategy");
    }

    [Test]
    [Arguments("modify-cache-cluster", "o")]
    [Arguments("modify-cache-cluster", "\u2022")]
    [Arguments("modify-replication-group", "\u2022")]
    public async Task Auth_Token_Strategy_Remains_A_Nonsecret_Enum(string commandName, string bullet)
    {
        var help = $"""
            OPTIONS
                   --auth-token-update-strategy (string)
                      The strategy to use to update the AUTH token. Possible values:

                      {bullet} SET

                      {bullet} ROTATE

                      {bullet} DELETE

                   --auth-token (string)
                      The password used to access a password protected server.
            """;
        var scraper = new TestAwsCliScraper();
        var command = (await scraper.Parse(["aws", "elasticache", commandName], help))!;
        var enhancer = new OptionTypeEnhancer(
            new OptionTypeDetectorPipeline([], NullLogger<OptionTypeDetectorPipeline>.Instance),
            NullLogger<OptionTypeEnhancer>.Instance);
        var tool = await enhancer.EnhanceManualOverridesAsync(
            scraper.CreateToolDefinition() with { Commands = [command] });
        var strategy = tool.Commands.Single().Options.Single(option => option.PropertyName == "AuthTokenUpdateStrategy");
        await Assert.That(strategy.EnumDefinition).IsNotNull();
        await Assert.That(strategy.EnumDefinition!.Values.Select(value => value.CliValue))
            .IsEquivalentTo(["SET", "ROTATE", "DELETE"]);
        await Assert.That(strategy.IsSecret).IsFalse();
        await Assert.That(tool.Commands.Single().Options.Single(option => option.PropertyName == "AuthToken").IsSecret).IsTrue();
        var files = await new OptionsClassGenerator().GenerateAsync(tool);
        await Assert.That(files.Single().Content).Contains($"public {strategy.EnumDefinition.EnumName}? AuthTokenUpdateStrategy");
        await Assert.That(files.Single().Content).DoesNotContain($"[SecretValue]{Environment.NewLine}    [CliOption(\"--auth-token-update-strategy\")]");
    }
}
