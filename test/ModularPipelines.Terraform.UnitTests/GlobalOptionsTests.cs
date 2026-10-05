using System.Reflection;
using ModularPipelines.Context;
using ModularPipelines.Terraform.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Terraform.UnitTests;

public class GlobalOptionsTests : TestBase
{
    [Test]
    public async Task Chdir_Precedes_Command_While_Local_Options_Follow_It()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var command = builder.Build(new TerraformValidateOptions
        {
            Chdir = "infrastructure",
            Json = true,
        });

        await Assert.That(command.ToString()).IsEqualTo("terraform -chdir=infrastructure validate -json");
    }

    [Test]
    public async Task Chdir_Precedes_Nested_Command_And_Preserves_Spaced_Path()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var command = builder.Build(new TerraformStateListOptions
        {
            Chdir = "infrastructure production",
            State = "production.tfstate",
            Address = ["module.example"],
        });

        await Assert.That(string.Join("|", command.Arguments)).IsEqualTo(
            "-chdir=infrastructure production|state|list|-state=production.tfstate|module.example");
    }

    [Test]
    public async Task Unset_Chdir_Is_Omitted()
    {
        var builder = await GetService<ICommandLineBuilder>();
        await Assert.That(builder.Build(new TerraformValidateOptions()).ToString()).IsEqualTo("terraform validate");
    }

    [Test]
    public async Task All_Command_Options_Inherit_Exactly_One_Chdir_Property()
    {
        var commands = typeof(TerraformOptions).Assembly.GetTypes()
            .Where(type => !type.IsAbstract && type.IsAssignableTo(typeof(TerraformOptions)))
            .ToArray();
        await Assert.That(commands).IsNotEmpty();
        foreach (var command in commands)
        {
            var properties = command.GetProperties().Where(property => property.Name == nameof(TerraformOptions.Chdir)).ToArray();
            await Assert.That(properties).Count().IsEqualTo(1);
            await Assert.That(properties.Single().DeclaringType).IsEqualTo(typeof(TerraformOptions));
            await Assert.That(command.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                .Any(property => property.Name == nameof(TerraformOptions.Chdir))).IsFalse();
        }
    }
}
