using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAsyncSafetyAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleHelperRefCancellationAnalyzerTests
{
    [TestMethod]
    [DataRow("ref token, ref token", "", "var saved = first; if (Flag) { second = default; while (Flag) { break; } if (Flag) goto done; return saved; } done: return first;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; if (Flag) { second = default; if (Flag) goto done; return saved; } done: return first;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; if (Flag) { second = default; while (Flag) { break; } if (Flag) goto done; done: return saved; } return first;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; switch (Flag.GetHashCode()) { case 0: second = default; while (Flag) { break; } if (Flag) goto default; return saved; default: return first; }", true)]
    [DataRow("ref token, ref token", "", "var saved = first; if (Flag) { second = default; for (int i = 0; i < 1; i++) continue; return saved; } return first;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; if (Flag) { second = default; while (Flag) { break; } return saved; } return first;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; while (Flag) { if (Flag) { second = default; continue; } return first; } return saved;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; while (Flag) { if (Flag) { second = default; break; } return first; } return first;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; again: while (Flag) { if (Flag) { second = default; break; } return first; } if (Flag) goto again; return saved;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; while (Flag) { if (Flag) { second = default; break; } return first; } return saved;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; for (; Flag;) { if (Flag) { second = default; break; } return first; } return saved;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; do { if (Flag) { second = default; break; } return first; } while (Flag); return saved;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; while (Flag) { if (Flag) { second = default; if (Flag) break; } return first; } return saved;", true)]
    [DataRow("ref token, ref token", "", "while (Flag) { if (Flag) { second = default; break; } } return first;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; while (Flag) { while (Flag) { if (Flag) { second = default; break; } return first; } } return saved;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; while (Flag) { if (Flag) { second = default; switch (Flag) { default: break; } } return first; } return saved;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; while (Flag) { try { if (Flag) { second = default; break; } } finally { saved = first; } return first; } return saved;", true)]
    [DataRow("ref token, ref token", "", "CopyNested(ref first, first); return first;", false)]
    [DataRow("ref token, ref token", "", "CopyNested(destination: ref first, source: first); return first;", false)]
    [DataRow("ref token, ref token", "", "CopyNested(ref first, default); return first;", true)]
    [DataRow("ref token, ref token", "", "second = default; CopyNested(ref first, first); return first;", true)]
    [DataRow("ref token, ref token", "", "CopyNestedCleared(ref first, first); return first;", true)]
    [DataRow("ref token, ref token", "", "CopyRecursive(ref first, first); return first;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; var copy = saved; CopyNested(ref copy, saved); return copy;", false)]
    [DataRow("ref token, ref token", "", "Copy(ref first, first); return first;", false)]
    [DataRow("ref token, ref token", "", "Copy(source: first, destination: ref second); return first;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; Copy(ref saved, saved); return saved;", false)]
    [DataRow("ref token, ref token", "", "Copy(ref first, default); return first;", true)]
    [DataRow("ref token, ref token", "", "second = default; Copy(ref first, first); return first;", true)]
    [DataRow("ref token, ref token", "", "CopyCleared(ref first, first); return first;", true)]
    [DataRow("ref token, ref token", "", "CopyAfterClear(ref first, ref second); return first;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; try { second = default; if (Flag) throw new Exception(); goto done; } catch { return first; } done: return saved;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; try { second = default; goto done; } finally { saved = first; } done: return saved;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; if (Flag) { second = default; goto done; } return first; done: return saved;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; if (Flag) { second = default; if (Flag) goto done; } return first; done: return saved;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; if (Flag) { second = default; goto done; } return first; done: return first;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; again: if (Flag) { second = default; goto done; } return first; done: if (Flag) goto again; return saved;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; if (Flag) { second = default; goto done; } return first; done: goto earlier; earlier: return saved;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; switch (Flag.GetHashCode()) { case 0: while (Flag) { if (Flag) goto default; second = default; break; } return saved; default: return first; }", false)]
    [DataRow("ref token, ref token", "", "var saved = first; switch (Flag.GetHashCode()) { case 0: while (Flag) { if (Flag) goto default; second = default; if (Flag) break; } return saved; default: return first; }", true)]
    [DataRow("ref token, ref token", "", "var saved = first; switch (Flag.GetHashCode()) { case 0: while (Flag) { if (Flag) goto default; second = default; continue; } return saved; default: return first; }", true)]
    [DataRow("ref token, ref token", "", "var saved = first; var copy = saved; ref CancellationToken alias = ref copy; alias = IdentityValue(default, saved); return copy;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; var copy = saved; ref CancellationToken alias = ref copy; saved = default; alias = IdentityValue(default, saved); return copy;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; var copy = saved; ref CancellationToken alias = ref copy; alias = IdentityValue(saved, default); return copy;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; switch (Flag.GetHashCode()) { case 0: for (int i = 0; i < 2; i++) { if (i == 1) goto default; second = default; } return saved; default: return first; }", true)]
    [DataRow("ref token, ref token", "", "switch (Flag) { case true: int visits = 0; again: if (visits++ != 0) goto default; second = default; goto again; default: return first; }", true)]
    [DataRow("ref token, ref token", "", "var saved = first; var copy = saved; ref CancellationToken alias = ref copy; alias = saved; return copy;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; var copy = saved; ref CancellationToken alias = ref copy; saved = default; alias = saved; return copy;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; switch (Flag.GetHashCode()) { case 0: second = default; if (Flag) goto case 1; return saved; case 1: return saved; default: return first; }", false)]
    [DataRow("ref token, ref token", "", "var saved = first; switch (Flag) { case true: if (Flag) goto default; second = default; return saved; default: return first; }", false)]
    [DataRow("ref token, ref token", "", "var saved = first; switch (Flag.GetHashCode()) { case 0: second = default; if (Flag) goto case 1; return saved; case 1: goto default; default: return first; }", true)]
    [DataRow("ref token, ref token", "", "var saved = first; switch (Flag.GetHashCode()) { case 0: second = default; goto case 1; case 1: if (Flag) goto case 0; return saved; default: return first; }", false)]
    [DataRow("ref token, ref token", "", "var saved = first; switch (Flag) { case true: second = default; if (Flag) goto default; return saved; default: return first; }", true)]
    [DataRow("ref token, ref token", "", "var saved = first; switch (Flag) { case true: second = default; if (Flag) goto default; break; default: return first; } return saved;", true)]
    [DataRow("ref token, ref token", "", "ref CancellationToken alias = ref first; first = IdentityValue(default, first); return alias;", false)]
    [DataRow("ref token, ref token", "", "ref CancellationToken alias = ref first; first = IdentityValue(first, default); return alias;", true)]
    [DataRow("ref token, ref token", "", "ref CancellationToken alias = ref first; second = default; first = IdentityValue(default, first); return alias;", true)]
    [DataRow("ref token, ref token", "", "RecursivePreserve(ref first); return first;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; switch (Flag) { case true: second = default; break; default: return first; } return saved;", false)]
    [DataRow("ref token, ref token", "", "PreserveLinkedConditional(ref first); return first;", false)]
    [DataRow("ref token, ref token", "", "DiscardLinkedConditional(ref first); return first;", true)]
    [DataRow("ref token, ref token", "", "PreserveLinked(ref first); return first;", false)]
    [DataRow("ref token, ref token", "", "PreserveLinkedNested(ref first); return first;", false)]
    [DataRow("ref token, ref token", "", "ClearThenLink(ref first); return first;", true)]
    [DataRow("ref token, ref token", "", "DiscardLinked(ref first); return first;", true)]
    [DataRow("ref token, ref token", "", "PreserveViaValueHelper(ref first); return first;", false)]
    [DataRow("ref token, ref token", "", "ClearPairLocal(ref second); return first;", true)]
    [DataRow("ref token, ref token", "", "ClearPairCall(ref first); return first;", true)]
    [DataRow("ref token, ref token", "", "ClearNestedPair(ref second); return first;", true)]
    [DataRow("ref token, ref token", "", "ObservePair(ref first); return first;", false)]
    [DataRow("ref token, ref token", "", "ObserveDiscard(ref first); return first;", false)]
    [DataRow("ref token, ref token", "", "Observe(ref first, new int[1]); return first;", false)]
    [DataRow("ref token, ref token", "", "ObserveNested(ref first, new int[1]); return first;", false)]
    [DataRow("ref token, ref token", "", "ObserveAndClear(ref first, new int[1]); return first;", true)]
    [DataRow("ref token, ref token", "", "return CancellationTokenSource.CreateLinkedTokenSource(new[] { first }).Token;", true)]
    [DataRow("ref token, ref token", "", "CancellationToken[] inputs = [first]; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", true)]
    [DataRow("ref token, ref token", "", "second = default; return first;", true)]
    [DataRow("second: ref token, first: ref token", "", "second = default; return first;", true)]
    [DataRow("ref token, ref alias", "ref CancellationToken alias = ref token;", "second = default; return first;", true)]
    [DataRow("ref alias, ref token", "ref CancellationToken alias = ref token;", "second = default; return first;", true)]
    [DataRow("ref token, ref alias", "ref CancellationToken alias = ref token; ref CancellationToken chained = ref alias; alias = ref chained;", "second = default; return first;", true)]
    [DataRow("ref token, ref token", "", "return first;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "second = default; return first;", false)]
    [DataRow("second: ref other, first: ref token", "CancellationToken other = default;", "second = default; return first;", false)]
    [DataRow("ref token, ref alias", "CancellationToken other = default; ref CancellationToken alias = ref other;", "second = default; return first;", false)]
    [DataRow("ref token, ref token", "", "var original = first; second = default; return original;", false)]
    [DataRow("ref token, ref token", "", "if (false) second = default; return first;", false)]
    [DataRow("ref token, ref alias", "CancellationToken other = default; ref CancellationToken alias = ref token; alias = ref other;", "second = default; return first;", false)]
    [DataRow("ref token, ref alias", "CancellationToken other = default; ref CancellationToken alias = ref other; alias = ref token;", "second = default; return first;", true)]
    [DataRow("ref token, ref token", "", "second = first; return first;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = saved; return first;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; saved = default; second = saved; return first;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; second = saved; return first;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; var copy = saved; second = copy; return first;", false)]
    [DataRow("ref token, ref token", "", "second = Forward(ref first, ref second); return first;", true)]
    [DataRow("ref token, ref token", "", "ref CancellationToken alias = ref second; return first;", false)]
    [DataRow("ref token, ref token", "", "var stored = first; ref CancellationToken alias = ref stored; alias = default; second = stored; return first;", true)]
    [DataRow("ref token, ref token", "", "var stored = first; ref CancellationToken alias = ref stored; second = stored; return first;", false)]
    [DataRow("ref token, ref token", "", "var stored = first; ref CancellationToken alias = ref stored; alias = default; stored = first; second = stored; return first;", false)]
    [DataRow("ref token, ref token", "", "ref CancellationToken alias = ref second; alias = default; return first;", true)]
    [DataRow("ref token, ref token", "", "ref CancellationToken alias = ref first; alias = ref second; alias = default; return first;", true)]
    [DataRow("ref token, ref token", "", "ref CancellationToken alias = ref second; var saved = first; alias = default; return saved;", false)]
    [DataRow("ref token, ref token", "", "ref CancellationToken alias = ref second; alias = default; var saved = first; return saved;", true)]
    [DataRow("ref token, ref token", "", "int count = 0; ref int alias = ref count; alias = 0; return first;", false)]
    [DataRow("ref token, ref token", "", "ref int count = ref Count; count = 0; return first;", false)]
    [DataRow("ref token, ref token", "", "if (Flag) { var saved = first; second = default; return saved; } return first;", false)]
    [DataRow("ref token, ref token", "", "if (Flag) { second = default; return first; } return first;", true)]
    [DataRow("ref token, ref token", "", "if (Flag) { second = default; } return first;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; if (Flag) return first; second = default; return saved;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "int count = 0; (first, count) = (first, 1); return first;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "(first, second) = (second, first); return first;", true)]
    [DataRow("ref token, ref Field", "", "second = default; return first;", false)]
    [DataRow("ref token, ref token", "", "int count = 0; (second, count) = (first, 1); return first;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; int count = 0; (count, (second, count)) = (1, (saved, 2)); return first;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; saved = default; int count = 0; (second, count) = (saved, 1); return first;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; ref CancellationToken alias = ref saved; alias = default; return saved;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; switch (Flag) { case true: second = default; return saved; default: return first; }", false)]
    [DataRow("ref token, ref token", "", "CancellationToken extra = default; ref CancellationToken alias = ref extra; alias = default; return CancellationTokenSource.CreateLinkedTokenSource(first, extra).Token;", false)]
    [DataRow("ref token, ref token", "", "return Identity(ref second, ref first);", false)]
    [DataRow("ref token, ref token", "", "var saved = first; ref CancellationToken alias = ref saved; alias = first; return saved;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; ref CancellationToken alias = ref saved; second = default; alias = first; return saved;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; ref CancellationToken alias = ref saved; alias = default; return CancellationTokenSource.CreateLinkedTokenSource(saved, first).Token;", false)]
    [DataRow("ref token, ref token", "", "switch (Flag) { case true: second = default; goto default; default: return first; }", true)]
    [DataRow("ref token, ref token", "", "switch (Flag) { case true: second = default; break; default: break; } return first;", true)]
    [DataRow("ref token, ref token", "", "return IdentityValue(second = default, first);", true)]
    [DataRow("ref token, ref token", "", "second = CancellationTokenSource.CreateLinkedTokenSource(first, CancellationToken.None).Token; return first;", false)]
    [DataRow("ref token, ref token", "", "second = IdentityValue(default, first); return first;", false)]
    [DataRow("ref token, ref token", "", "second = default; second = IdentityValue(default, first); return first;", true)]
    [DataRow("ref token, ref token", "", "second = Flag ? first : CancellationTokenSource.CreateLinkedTokenSource(first, CancellationToken.None).Token; return first;", false)]
    [DataRow("ref token, ref token", "", "second = Flag ? first : default; return first;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; return IdentityValue(first, saved);", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; return IdentityValue(value: saved, ignored: first);", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; return IdentityValue(saved, first);", true)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; return IdentityValue(first, IdentityValue(first, saved));", false)]
    [DataRow("ref token, ref token", "", "var saved = first; ref CancellationToken alias = ref saved; alias = default; return IdentityValue(first, saved);", true)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; second = IdentityValue(first, saved); return first;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; var result = IdentityValue(first, saved); return result;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; var result = IdentityValue(saved, first); return result;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; var result = Flag ? saved : CancellationTokenSource.CreateLinkedTokenSource(first, saved).Token; return result;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; var result = Flag switch { true => saved, false => CancellationTokenSource.CreateLinkedTokenSource(first, saved).Token }; return result;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; var result = Flag ? saved : IdentityValue(first, saved); return result;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; var result = Flag ? saved : first; return result;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; var result = Flag switch { true => saved, false => first }; return result;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; var result = true ? saved : first; return result;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; var result = Flag ? saved : throw new System.InvalidOperationException(); return result;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; var result = Flag ? saved : (Flag switch { true => saved, false => IdentityValue(first, saved) }); return result;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; (saved, second) = (first, default(CancellationToken)); return saved;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; (second, saved) = (default(CancellationToken), first); return saved;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; (saved, second) = (first, default(CancellationToken)); return saved;", true)]
    [DataRow("ref token, ref token", "", "Identity(ref second, ref first); return first;", false)]
    [DataRow("ref token, ref token", "", "Identity(ref first, ref second); return first;", false)]
    [DataRow("ref token, ref token", "", "Clear(ref second); return first;", true)]
    [DataRow("ref token, ref token", "", "Preserve(ref second); return first;", false)]
    [DataRow("ref token, ref token", "", "ClearNested(ref second); return first;", true)]
    [DataRow("ref token, ref token", "", "IdentityNested(ref second); return first;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; CancellationToken extra = default; (extra, extra) = (second = default(CancellationToken), saved = first); return saved;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; CancellationToken extra = default; (extra, extra) = (saved = first, second = default(CancellationToken)); return saved;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; CancellationToken extra = default; (extra, extra) = (second = first, saved = first); return saved;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; return Flag switch { true => IdentityValue(second = default, saved), false => first };", false)]
    [DataRow("ref token, ref token", "", "var saved = first; return Flag switch { true => IdentityValue(second = default, first), false => saved };", true)]
    [DataRow("ref token, ref token", "", "var saved = first; return Flag switch { _ when (second = default) == default && Flag => saved, _ => first };", true)]
    public async Task Checks_Helper_Ref_Argument_Storage(
        string arguments, string setup, string body, bool reports)
        => await VerifyStorage(arguments, setup, body, reports, firstByRef: true);

    [TestMethod]
    [DataRow("token, ref Field", "", "var saved = first; var holder = new Holder(); holder.Token = default; return saved;", false)]
    [DataRow("token, ref Field", "", "var saved = first; var holder = new Holder(); Clear(ref holder.Token); return saved;", false)]
    [DataRow("token, ref Field", "", "var saved = first; var holder = new Holder(); holder.Token = default; saved = default; return saved;", true)]
    [DataRow("token, ref Field", "", "var saved = first; var holder = new TokenReference(ref saved); holder.Token = default; return saved;", true)]
    [DataRow("token, ref Field", "", "var saved = first; var other = first; ref CancellationToken alias = ref (Flag ? ref saved : ref other); var holder = new Holder(); holder.Token = default; alias = default; return saved;", true)]
    [DataRow("token, ref Field", "", "return ((long)1) switch { 1L => default(CancellationToken), _ => first };", true)]
    [DataRow("token, ref Field", "", "return ((long)1) switch { 1L => first, _ => default(CancellationToken) };", false)]
    [DataRow("token, ref Field", "", "return ((long)(Flag ? 1 : 1)) switch { 1L => default(CancellationToken), _ => first };", true)]
    [DataRow("token, ref Field", "", "return (Flag ? (long)1 : (long)1) switch { 1L => first, _ => default(CancellationToken) };", false)]
    [DataRow("token, ref Field", "", "return (Flag ? (long)1 : (long)2) switch { 1L => default(CancellationToken), _ => first };", true)]
    [DataRow("token, ref holder.Token", "var holder = new Holder();", "var saved = first; var other = first; ref CancellationToken alias = ref (Flag ? ref saved : ref other); alias = default; return saved;", true)]
    [DataRow("token, ref holder.Token", "var holder = new Holder();", "var saved = first; var other = first; ref CancellationToken alias = ref (Flag ? ref saved : ref other); Clear(ref alias); return saved;", true)]
    [DataRow("token, ref holder.Token", "var holder = new Holder();", "var saved = first; var other = first; ref CancellationToken alias = ref saved; if (Flag) alias = ref other; alias = default; return saved;", true)]
    [DataRow("token, ref holder.Token", "var holder = new Holder();", "var saved = first; ref CancellationToken alias = ref second; alias = default; return saved;", false)]
    [DataRow("token, ref holder.Token", "var holder = new Holder();", "var saved = first; second = default; return saved;", false)]
    [DataRow("token, ref holder.Token", "var holder = new Holder();", "var saved = first; second = default; saved = default; return saved;", true)]
    [DataRow("token, ref holder.Token", "var holder = new Holder();", "var saved = first; ref CancellationToken alias = ref saved; second = default; alias = default; return saved;", true)]
    [DataRow("token, ref token", "", "second = default; return first;", false)]
    [DataRow("token, ref Field", "", "second = default; return first;", false)]
    [DataRow("token, ref Field", "", "first = default; return first;", true)]
    [DataRow("token, ref Field", "", "ref CancellationToken alias = ref first; alias = default; return first;", true)]
    [DataRow("token, ref Field", "", "var saved = first; ref CancellationToken alias = ref first; alias = default; return saved;", false)]
    [DataRow("token, ref Field", "", "var saved = first; second = saved; return first;", false)]
    [DataRow("token, ref Field", "", "int count = 0; (second, count) = (default(CancellationToken), 1); return first;", false)]
    [DataRow("token, ref Field", "", "int count = 0; (first, count) = (default(CancellationToken), 1); return first;", true)]
    [DataRow("token, ref Field", "", "var saved = first; int count = 0; (first, count) = (saved, 1); return first;", false)]
    [DataRow("token, ref Field", "", "first = CancellationTokenSource.CreateLinkedTokenSource(first, CancellationToken.None).Token; return first;", false)]
    [DataRow("token, ref Field", "", "first = IdentityValue(default, first); return first;", false)]
    [DataRow("token, ref Field", "", "first = default; first = CancellationTokenSource.CreateLinkedTokenSource(first, CancellationToken.None).Token; return first;", true)]
    [DataRow("token, ref Field", "", "return Flag ? first : CancellationTokenSource.CreateLinkedTokenSource(Forward(first, ref second), first).Token;", false)]
    [DataRow("token, ref Field", "", "return Link(first, 2);", false)]
    [DataRow("token, ref Field", "", "return Mutual(first, 2);", false)]
    [DataRow("token, ref Field", "", "first = IdentityValue(first, default); return first;", true)]
    [DataRow("token, ref Field", "", "first = Flag ? first : CancellationTokenSource.CreateLinkedTokenSource(first, CancellationToken.None).Token; return first;", false)]
    [DataRow("token, ref Field", "", "first = Flag ? first : default; return first;", true)]
    [DataRow("token, ref Field", "", "first = true ? first : default; return first;", false)]
    [DataRow("token, ref Field", "", "first = Flag ? first : throw new System.InvalidOperationException(); return first;", false)]
    [DataRow("token, ref Field", "", "first = Flag switch { true => first, false => CancellationTokenSource.CreateLinkedTokenSource(first, CancellationToken.None).Token }; return first;", false)]
    [DataRow("token, ref Field", "", "first = Flag switch { true => first, false => default }; return first;", true)]
    // A constant switch selector preserves the token from its only reachable arm.
    [DataRow("token, ref Field", "", "first = true switch { true => first, false => default }; return first;", false)]
    [DataRow("token, ref Field", "", "first = Flag switch { true => first, false => throw new System.InvalidOperationException() }; return first;", false)]
    [DataRow("token, ref Field", "", "first = Flag ? first : (Flag switch { true => first, false => CancellationTokenSource.CreateLinkedTokenSource(first, CancellationToken.None).Token }); return first;", false)]
    public async Task Checks_Helper_Private_Value_Parameter_Storage(
        string arguments, string setup, string body, bool reports)
        => await VerifyStorage(arguments, setup, body, reports, firstByRef: false);

    [TestMethod]
    [DataRow("token = cancellationToken;", false)]
    [DataRow("System.Console.WriteLine(); token = cancellationToken;", true)]
    [DataRow("if (context != null) token = cancellationToken;", true)]
    public async Task Checks_Token_Assignment_Before_Finally(string body, bool reports)
    {
        var source = $$"""
            {{TestSourceConstants.StandardModuleHeader}}
            public class BuildModule : Module<List<string>>
            {
                protected override async Task<List<string>> ExecuteAsync(
                    IModuleContext context, CancellationToken cancellationToken)
                {
                    CancellationToken token = default;
                    try { {{body}} }
                    finally { await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}; }
                    return null!;
                }
            }
            """;
        if (reports)
        {
            await VerifyCS.VerifyAnalyzerAsync(source,
                VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                    .WithLocation(0).WithArguments("Delay"));
        }
        else
        {
            await VerifyCS.VerifyAnalyzerAsync(source);
        }
    }

    private static async Task VerifyStorage(string arguments, string setup, string body, bool reports, bool firstByRef)
    {
        var call = "Task.Delay(1, snapshot)";
        var source = $$"""
            {{TestSourceConstants.StandardModuleHeader}}

            public class BuildModule : Module<List<string>>
            {
                private sealed class Holder { public CancellationToken Token; }
                private ref struct TokenReference
                {
                    public ref CancellationToken Token;
                    public TokenReference(ref CancellationToken token) { Token = ref token; }
                }
                private static int Count;
                private static bool Flag;
                private static CancellationToken Field;

                private static CancellationToken Forward({{(firstByRef ? "scoped ref " : "")}}CancellationToken first, scoped ref CancellationToken second)
                {
                    {{body}}
                }

                private static CancellationToken Identity(scoped ref CancellationToken ignored, scoped ref CancellationToken value) => value;

                private static void Observe(scoped ref CancellationToken value, int[] counts) => counts[0] = 1;
                private static void ObserveNested(scoped ref CancellationToken value, int[] counts) => Observe(ref value, counts);
                private static void ObserveAndClear(scoped ref CancellationToken value, int[] counts)
                {
                    counts[0] = 1;
                    value = default;
                }

                private static (CancellationToken, int) GetPair() => (default, 0);
                private static void ClearPairLocal(scoped ref CancellationToken value)
                {
                    var pair = GetPair();
                    int count;
                    (value, count) = pair;
                }
                private static void ClearPairCall(scoped ref CancellationToken value) => (value, _) = GetPair();
                private static void ClearNestedPair(scoped ref CancellationToken value)
                {
                    var pair = (0, GetPair());
                    int count;
                    (_, (value, count)) = pair;
                }
                private static void ObserveDiscard(scoped ref CancellationToken value)
                {
                    var pair = GetPair();
                    int count;
                    (_, count) = pair;
                }
                private static void ObservePair(scoped ref CancellationToken value)
                {
                    int first, second;
                    (first, second) = (1, 2);
                }

                private static void PreserveLinked(scoped ref CancellationToken value)
                    => value = CancellationTokenSource.CreateLinkedTokenSource(value, CancellationToken.None).Token;
                private static void PreserveLinkedConditional(scoped ref CancellationToken value)
                    => value = Flag ? value : CancellationTokenSource.CreateLinkedTokenSource(value, CancellationToken.None).Token;
                private static void DiscardLinkedConditional(scoped ref CancellationToken value) => value = Flag ? value : default;
                private static void PreserveLinkedNested(scoped ref CancellationToken value) => PreserveLinked(ref value);
                private static void ClearThenLink(scoped ref CancellationToken value)
                {
                    value = default;
                    PreserveLinked(ref value);
                }
                private static void DiscardLinked(scoped ref CancellationToken value)
                    => value = CancellationTokenSource.CreateLinkedTokenSource(default(CancellationToken), CancellationToken.None).Token;
                private static void PreserveViaValueHelper(scoped ref CancellationToken value) => value = IdentityValue(default, value);

                private static void RecursivePreserve(scoped ref CancellationToken value) => value = RecursiveIdentity(ref value);
                private static CancellationToken RecursiveIdentity(scoped ref CancellationToken token)
                {
                    if (Flag) RecursivePreserve(ref token);
                    return token;
                }

                private static void Clear(scoped ref CancellationToken value) => value = default;
                private static void Preserve(scoped ref CancellationToken value) => value = value;
                private static void ClearNested(scoped ref CancellationToken value) => Clear(ref value);
                private static void IdentityNested(scoped ref CancellationToken value) => Identity(ref value, ref value);


                private static void CopyNested(ref CancellationToken destination, CancellationToken source) => Copy(ref destination, source);
                private static void CopyNestedCleared(ref CancellationToken destination, CancellationToken source) { source = default; Copy(ref destination, source); }
                private static void CopyRecursive(ref CancellationToken destination, CancellationToken source) => CopyRecursive(ref destination, source);
                private static void Copy(ref CancellationToken destination, CancellationToken source) => destination = source;
                private static void CopyCleared(ref CancellationToken destination, CancellationToken source) { source = default; destination = source; }
                private static void CopyAfterClear(ref CancellationToken destination, ref CancellationToken source) { destination = default; destination = source; }
                private static CancellationToken IdentityValue(CancellationToken ignored, CancellationToken value) => value;

                private static CancellationToken Link(CancellationToken token, int remaining)
                    => remaining == 0 ? token : CancellationTokenSource.CreateLinkedTokenSource(Link(token, remaining - 1), token).Token;

                private static CancellationToken Mutual(CancellationToken token, int remaining)
                    => remaining == 0 ? token : CancellationTokenSource.CreateLinkedTokenSource(MutualOther(token, remaining - 1), token).Token;

                private static CancellationToken MutualOther(CancellationToken token, int remaining) => Mutual(token, remaining);


                protected override async Task<List<string>> ExecuteAsync(
                    IModuleContext context, CancellationToken cancellationToken)
                {
                    var token = cancellationToken;
                    {{setup}}
                    var snapshot = Forward({{arguments}});
                    for (var i = 0; i < 2; i++)
                        await {{(reports ? "{|#0:" + call + "|}" : call)}};
                    return null!;
                }
            }
            """;

        if (reports)
        {
            await VerifyCS.VerifyAnalyzerAsync(source,
                VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                    .WithLocation(0).WithArguments("Delay"));
        }
        else
        {
            await VerifyCS.VerifyAnalyzerAsync(source);
        }
    }
}
