using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public class GcloudCapturedSemanticsTests
{
    [Test]
    public async Task Captured_Command_Enums_Preserve_Different_Edition_Values()
    {
        var firestore = await Scrape("firestore databases create");
        var sql = await Scrape("sql instances create");
        var firestoreEdition = firestore.Options.Single(option => option.PropertyName == "Edition").EnumDefinition!;
        var sqlEdition = sql.Options.Single(option => option.PropertyName == "Edition").EnumDefinition!;
        await Assert.That(firestoreEdition.EnumName).IsNotEqualTo(sqlEdition.EnumName);
        await Assert.That(firestoreEdition.Values.Select(value => value.CliValue)).IsEquivalentTo(["standard", "enterprise"]);
        await Assert.That(sqlEdition.Values.Select(value => value.CliValue)).IsEquivalentTo(["enterprise", "enterprise-plus"]);
    }

    [Test]
    [Arguments("*")]
    [Arguments("▪")]
    [Arguments("◆")]
    [Arguments("▸")]
    [Arguments("▫")]
    [Arguments("◇")]
    [Arguments("▹")]
    [Arguments("■")]
    [Arguments("≡")]
    [Arguments("∞")]
    [Arguments("Φ")]
    [Arguments("·")]
    [Arguments("•")]
    [Arguments("◦")]
    public async Task Captured_Resource_References_Do_Not_Declare_Boolean_Flags(string bullet)
    {
        var command = await Scrape("oracle-database goldengate-connections create", bullet);
        var option = command.Options.Single(option => option.PropertyName == "AmazonRedshiftConnectionPropertiesPasswordSecretVersion");
        await Assert.That(option.IsFlag).IsFalse();
        await Assert.That(option.CSharpType).IsEqualTo("string?");
        await Assert.That(option.Description).Contains("ID of the secretVersion");
        await Assert.That(option.IsSecret).IsFalse();
        await Assert.That(command.Options.Single(option => option.PropertyName == "AmazonRedshiftConnectionPropertiesPassword").IsSecret).IsTrue();
    }

    [Test]
    public async Task Captured_Private_Key_Content_Is_Secret()
    {
        var command = await Scrape("database-migration connection-profiles create mysql");
        await Assert.That(command.Options.Single(option => option.PropertyName == "PrivateKey").IsSecret).IsTrue();
    }

    [Test]
    public async Task Captured_Auth_Token_Resource_Identifiers_Are_Visible()
    {
        var command = await Scrape("memorystore instances token-auth-users auth-tokens describe");
        await Assert.That(command.Options.Single(option => option.PropertyName == "TokenAuthUser").IsSecret).IsFalse();
        await Assert.That(command.PositionalArguments.Single().IsSecret).IsFalse();
    }

    [Test]
    public async Task Captured_Provider_Configurations_Remain_Separate_Optional_Branches()
    {
        var command = await Scrape("developer-connect connections create");
        var group = command.RequiredAlternativeGroups.Single(group => group.IsMutuallyExclusive);
        await Assert.That(group.IsRequired).IsFalse();
        await Assert.That(group.Groups.Count).IsEqualTo(8)
            .Because(string.Join("\n", group.Groups.Select(branch => string.Join(", ", branch.PropertyNames))));
        await Assert.That(command.Options.Any(option => option.IsRequired)).IsFalse()
            .Because(string.Join(", ", command.Options.Where(option => option.IsRequired).Select(option => option.PropertyName)));
        foreach (var branch in group.Groups)
        {
            var prefixes = branch.PropertyNames.Select(name => name.Split("Config", StringSplitOptions.None)[0]).Distinct().ToArray();
            await Assert.That(prefixes.Length).IsEqualTo(1).Because(string.Join(", ", branch.PropertyNames));
        }
    }

    internal static async Task<CliCommandDefinition> Scrape(string command, string bullet = "*")
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "Gcloud", "585.0.0",
            $"gcloud-{command.Replace(' ', '-')}.txt"));
        help = help.Replace("* provide the argument", $"{bullet} provide the argument", StringComparison.Ordinal);
        return (await GcloudResourceArgumentTests.ScrapeFixture(command, help)).Single();
    }
}
