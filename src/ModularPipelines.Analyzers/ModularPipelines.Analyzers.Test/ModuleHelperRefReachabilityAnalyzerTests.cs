using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleHelperRefReachabilityAnalyzerTests
{
    [TestMethod]
    [DataRow("goto Clear; Again: return first; Clear: second = default; goto Again;", true)]
    [DataRow("bool done = false; Again: if (done) return first; second = default; done = true; goto Again;", true)]
    [DataRow("bool done = false; Again: if (done) return first; second = first; done = true; goto Again;", false)]
    [DataRow("var saved = first; Again: if (Flag) return saved; second = default; goto Again;", false)]
    [DataRow("if (Flag) { second = default; throw new System.InvalidOperationException(); } return first;", false)]
    [DataRow("if (Flag) { second = default; if (Flag) throw new System.InvalidOperationException(); else return default; } return first;", true)]
    [DataRow("try { second = default; throw new System.InvalidOperationException(); } catch { return first; }", true)]
    [DataRow("try { second = default; throw new System.InvalidOperationException(); } catch { } return first;", true)]
    [DataRow("var saved = first; try { second = default; throw new System.InvalidOperationException(); } catch { return saved; }", false)]
    [DataRow("if (Flag) { try { second = default; throw new System.InvalidOperationException(); } catch { throw; } } return first;", false)]
    [DataRow("try { try { second = default; throw new System.InvalidOperationException(); } catch { throw; } } catch { return first; }", true)]
    public async Task Checks_Helper_Mutation_Reachability(string body, bool reports)
    {
        await new ModuleHelperRefCancellationAnalyzerTests().Checks_Helper_Ref_Argument_Storage(
            "ref token, ref token", "", body, reports);
    }
}
