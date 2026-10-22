namespace ModularPipelines.UnitTests.Helpers;

// Each test can start several .NET hosts. Bound their overlap so startup remains
// observable within the fixture deadlines on small CI runners.
// CommandTests also shares this budget for pipeline setup in its other tests;
// limiting only the process fixtures still lets those startups compete with them.
public sealed class ProcessFixtureParallelLimit : TUnit.Core.Interfaces.IParallelLimit
{
    public int Limit => 2;
}
