using Pipeline.NET.DotNet;
using Pipeline.NET.DotNet.Modules;
using Pipeline.NET.DotNet.Options;

namespace Pipeline.NET.Build;

public class RunUnitTestsCommandModule : DotNetCommandModule
{
    public RunUnitTestsCommandModule(IModuleContext context) : base(context)
    {
    }

    public override DotNetCommandModuleOptions Options => new()
    {
        Command = "test",
        AssertSuccess = true,
        WorkingDirectory = Path.Combine(Context.Environment.GitRootDirectory!.FullName, "Pipeline.NET.UnitTests")
    };
}