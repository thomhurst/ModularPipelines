using System.ComponentModel.DataAnnotations;
using ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
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
