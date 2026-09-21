using System.ComponentModel.DataAnnotations;
using ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    [Test]
    [Arguments("550.0.0")]
    [Arguments("585.0.0")]
    public async Task Gcloud_Topic_Schema_Remains_Conditional_On_Schema_Settings(string version)
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory,
            "Fixtures", "Gcloud", version, "gcloud-pubsub-topics-create.txt"));
        var command = (await GcloudResourceArgumentTests.ScrapeFixture("pubsub topics create", help)).Single();
        await Assert.That(command.Options.Single(option => option.PropertyName == "Schema").IsRequired).IsFalse();
        var groups = command.RequiredAlternativeGroups.Where(group => group.PropertyNames.Contains("Schema")).ToList();
        await Assert.That(groups).Count().IsEqualTo(1);
        await Assert.That(groups[0].IsRequired).IsFalse();
        await Assert.That(groups[0].PropertyNames).Contains("MessageEncoding");

        var options = command.Options.Where(option => groups[0].PropertyNames.Contains(option.PropertyName)).ToList();
        var generated = await Generate(options, alternativeGroups: groups);
        var encoding = options.Single(option => option.PropertyName == "MessageEncoding").EnumDefinition!;
        var enumSource = $"namespace ModularPipelines.Tool.Enums {{ public enum {encoding.EnumName} {{ {string.Join(", ", encoding.Values.Select(value => value.MemberName))} }} }}";
        var optionsType = Compile(generated, enumSource).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        (string Properties, bool Valid)[] cases =
        [
            ("", true),
            ("Schema", false),
            ("MessageEncoding", false),
            ("Schema,MessageEncoding", true),
            ("SchemaProject", false),
            ("Schema,SchemaProject,MessageEncoding", true),
            ("FirstRevisionId", false),
            ("LastRevisionId", false),
        ];
        foreach (var (properties, valid) in cases)
        {
            var instance = Activator.CreateInstance(optionsType)!;
            foreach (var name in properties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                var property = optionsType.GetProperty(name)!;
                var type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                property.SetValue(instance, type.IsEnum ? Enum.GetValues(type).GetValue(0) : "value");
            }

            var errors = new List<ValidationResult>();
            await Assert.That(Validator.TryValidateObject(instance, new(instance), errors, true))
                .IsEqualTo(valid).Because($"Selected {properties}: {string.Join("; ", errors)}");
        }
    }

    [Test]
    public async Task Gcloud_Sql_Region_Does_Not_Inherit_Autoscaling_Description()
    {
        var command = await GcloudCapturedSemanticsTests.Scrape("sql instances create");
        foreach (var name in new[] { "Region", "GceZone", "SecondaryZone", "Zone" })
        {
            await Assert.That(command.Options.Single(option => option.PropertyName == name).Description)
                .DoesNotContain("auto scale");
        }
    }

    [Test]
    public async Task Gcloud_Sql_Independent_Groups_Validate_Only_Their_Own_Members()
    {
        var command = await GcloudCapturedSemanticsTests.Scrape("sql instances create");
        var names = command.RequiredAlternativeGroups.SelectMany(group => group.PropertyNames).ToHashSet();
        names.Add("AutoScaleMaxNodeCount");
        var generated = await Generate(command.Options.Where(option => names.Contains(option.PropertyName)).ToList(),
            alternativeGroups: command.RequiredAlternativeGroups);
        var optionsType = Compile(generated).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;

        (string Properties, bool Valid)[] cases =
        [
            ("", true),
            ("Region", true),
            ("AutoScaleMaxNodeCount", true),
            ("DiskEncryptionKey", true),
            ("DiskEncryptionKeyKeyring", false),
            ("EntraIdApplicationId", false),
            ("EntraIdTenantId", false),
            ("EntraIdApplicationId,EntraIdTenantId", true),
            ("Region,DiskEncryptionKey", true),
            ("Region,Zone", false),
        ];
        foreach (var (properties, valid) in cases)
        {
            var instance = Activator.CreateInstance(optionsType)!;
            foreach (var name in properties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                var property = optionsType.GetProperty(name)!;
                var type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                property.SetValue(instance, Convert.ChangeType("1", type));
            }

            var errors = new List<ValidationResult>();
            await Assert.That(Validator.TryValidateObject(instance, new(instance), errors, true))
                .IsEqualTo(valid).Because($"Selected {properties}: {string.Join("; ", errors)}");
        }
    }
}
