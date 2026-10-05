using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Generated;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Context;

public class GlobalSwitchScopeTests : TestBase
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Global_And_Local_Switches_Render_In_Separate_Scopes(bool reflection)
    {
        var builder = await GetService<ICommandLineBuilder>();
        var options = reflection ? new ReflectionScopedOptions<int>() : new ScopedOptions();
        await Assert.That(GeneratedCommandMetadata.TryGet(options.GetType(), out _)).IsEqualTo(!reflection);
        options.GlobalDebug = true;
        options.LocalDebug = false;
        options.GlobalConfig = "client";
        options.LocalConfig = "command";

        await Assert.That(builder.Build(options).ToString()).IsEqualTo(
            "tool --debug --config client run --no-debug --config command");
    }

    [Test]
    [Arguments("--debug")]
    [Arguments("-D")]
    [Arguments("--no-debug")]
    public async Task Manual_Colliding_Flags_Stay_After_Subcommand(string flag)
    {
        var builder = await GetService<ICommandLineBuilder>();
        var result = builder.Build(new ScopedOptions
        {
            Arguments = [flag],
            ArgumentsContainToolOptions = true,
            RunSettings = ["setting=value"],
        });

        await Assert.That(result.ToString()).IsEqualTo($"tool run {flag} -- setting=value");
    }

    [Test]
    [Arguments("--config")]
    [Arguments("-c")]
    public async Task Manual_Colliding_Values_Keep_Their_Operand(string name)
    {
        var builder = await GetService<ICommandLineBuilder>();
        var result = builder.Build(new ScopedOptions
        {
            Arguments = [name, "--debug"],
            ArgumentsContainToolOptions = true,
            RunSettings = ["setting=value"],
            AdditionalArguments = [new("--debug", IsGlobalOption: true)],
        });

        await Assert.That(result.ToString()).IsEqualTo($"tool --debug run {name} --debug -- setting=value");
    }

    [Test]
    public async Task Manual_Local_Value_Takes_Precedence_Over_Global_Flag()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var result = builder.Build(new DifferentArityOptions
        {
            Arguments = ["-D", "trace"],
            ArgumentsContainToolOptions = true,
            RunSettings = ["setting=value"],
        });

        await Assert.That(result.ToString()).IsEqualTo("tool run -D trace -- setting=value");
    }

    [Test]
    public async Task Manual_Local_Flag_Does_Not_Consume_A_Global_Option_Operand()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var result = builder.Build(new LocalFlagOptions
        {
            Arguments = ["-c", "operand"],
            ArgumentsContainToolOptions = true,
            RunSettings = ["setting=value"],
        });

        await Assert.That(result.ToString()).IsEqualTo("tool run -c operand -- setting=value");
    }

    [Test]
    public async Task Noncolliding_Global_Alias_Remains_Global()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var result = builder.Build(new DifferentArityOptions
        {
            Arguments = ["--no-debug"],
            ArgumentsContainToolOptions = true,
            RunSettings = ["setting=value"],
        });

        await Assert.That(result.ToString()).IsEqualTo("tool --no-debug run -- setting=value");
    }

    [Test]
    public async Task Terminal_Global_Property_Collides_With_Command_Property()
    {
        var builder = await GetService<ICommandLineBuilder>();
        await Assert.That(() => builder.Build(new TerminalOptions()))
            .Throws<InvalidOperationException>().And.HasMessageContaining("--output");
    }

    [CliTool("tool")]
    [CliGlobalOptions]
    internal abstract record GlobalOptions : CommandLineToolOptions
    {
        [CliFlag("--debug", ShortForm = "-D", NegatedName = "--no-debug")]
        public virtual bool? GlobalDebug { get; set; }

        [CliOption("--config", ShortForm = "-c")]
        public string? GlobalConfig { get; set; }
    }

    [CliSubCommand("run")]
    internal record ScopedOptions : GlobalOptions
    {
        public override bool? GlobalDebug { get; set; }

        [CliFlag("--debug", ShortForm = "-D", NegatedName = "--no-debug")]
        public bool? LocalDebug { get; set; }

        [CliOption("--config", ShortForm = "-c")]
        public string? LocalConfig { get; set; }
    }

    internal record ReflectionScopedOptions<T> : ScopedOptions;

    [CliSubCommand("run")]
    internal sealed record DifferentArityOptions : GlobalOptions
    {
        [CliOption("--debug", ShortForm = "-D")]
        public string? LocalDebug { get; set; }
    }

    [CliTool("tool")]
    [CliGlobalOptions]
    internal abstract record TerminalGlobalOptions : CommandLineToolOptions
    {
        [CliOption("--output", Phase = CommandLinePhase.Terminal)]
        public string? GlobalOutput { get; set; }
    }

    [CliSubCommand("run")]
    internal sealed record LocalFlagOptions : GlobalOptions
    {
        [CliFlag("--config", ShortForm = "-c")]
        public bool? LocalConfig { get; set; }
    }

    [CliSubCommand("run")]
    internal sealed record TerminalOptions : TerminalGlobalOptions
    {
        [CliOption("--output")]
        public string? LocalOutput { get; set; }
    }
}
