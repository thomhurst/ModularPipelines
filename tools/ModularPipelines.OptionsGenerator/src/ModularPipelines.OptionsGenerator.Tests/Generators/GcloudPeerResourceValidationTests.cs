using System.ComponentModel.DataAnnotations;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    [Test]
    public async Task Gcloud_Authentication_Branches_Remain_Independent()
    {
        var command = await GcloudCapturedSemanticsTests.Scrape("apihub plugins instances create");
        await Assert.That(command.Options.Single(option => option.PropertyName == "UserPasswordConfigUsername").Description)
            .DoesNotContain("Oauth");
        var group = command.RequiredAlternativeGroups.Single(group => group.PropertyNames.Contains("AuthConfigType"));
        await ValidateCapturedGroup(command, group,
        [
            ("", true),
            ("AuthConfigType", true),
            ("AuthConfigType,UserPasswordConfigSecretVersion,UserPasswordConfigUsername", true),
            ("AuthConfigType,Oauth2ClientCredentialsConfigId,Oauth2ClientCredentialsConfigSecretVersion", true),
            ("AuthConfigType,ApiKeyConfigHttpElementLocation,ApiKeyConfigName,ApiKeyConfigSecretVersion", true),
            ("UserPasswordConfigSecretVersion,UserPasswordConfigUsername", false),
            ("AuthConfigType,Oauth2ClientCredentialsConfigSecretVersion", false),
            ("AuthConfigType,Oauth2ClientCredentialsConfigId", false),
            ("AuthConfigType,UserPasswordConfigUsername", false),
            ("AuthConfigType,Oauth2ClientCredentialsConfigId,Oauth2ClientCredentialsConfigSecretVersion,UserPasswordConfigSecretVersion,UserPasswordConfigUsername", false),
        ]);
    }

    [Test]
    public async Task Gcloud_Required_Database_Bundle_Retains_Resource_Members()
    {
        var command = await GcloudCapturedSemanticsTests.Scrape("oracle-database db-systems create");
        var group = Descendants(command.RequiredAlternativeGroups)
            .Single(group => group.Members.Any(member => member.PropertyName == "DbHomeVersion"));
        await ValidateCapturedGroup(command, group,
        [
            ("", true),
            ("DbHomeVersion", false),
            ("DbHomeVersion,DatabaseAdminPassword", true),
            ("DbHomeVersion,DatabaseAdminPasswordSecretVersion", true),
            ("DbHomeVersion,DatabaseDbName", true),
            ("DatabaseAdminPasswordSecretVersion", false),
        ]);
    }

    private static IEnumerable<CliRequiredAlternativeGroup> Descendants(IEnumerable<CliRequiredAlternativeGroup> groups) =>
        groups.SelectMany(group => new[] { group }.Concat(Descendants(group.Groups)));

    private static async Task ValidateCapturedGroup(CliCommandDefinition command, CliRequiredAlternativeGroup group,
        (string Properties, bool Valid)[] cases)
    {
        var options = command.Options.Where(option => group.PropertyNames.Contains(option.PropertyName)).ToList();
        var generated = await Generate(options, alternativeGroups: [group]);
        const string secretAttribute = "namespace ModularPipelines.Secrets { public sealed class SecretValueAttribute : System.Attribute; }";
        var optionsType = Compile(generated, secretAttribute).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        foreach (var (properties, valid) in cases)
        {
            var instance = Activator.CreateInstance(optionsType)!;
            foreach (var name in properties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                optionsType.GetProperty(name)!.SetValue(instance, "value");
            }

            var errors = new List<ValidationResult>();
            await Assert.That(Validator.TryValidateObject(instance, new(instance), errors, true))
                .IsEqualTo(valid).Because($"Selected {properties}: {string.Join("; ", errors)}");
        }
    }
}
