# AWS help rendering

On POSIX systems, AWS CLI invokes `groff -m man -T ascii` directly. Terminal-width
formatting inserts hyphens inside words and can wrap command names at real hyphens.
Once rendered as ASCII, those two cases cannot be distinguished safely.

The generation workflow puts this wrapper first on `PATH`. It disables automatic
hyphenation and uses a 10,000-character line width, preserving command tokens and
identifiers before the scraper sees them. The scraper rejects dangling-hyphen
commands instead of guessing their spelling. Windows AWS help uses a different
renderer and does not need this wrapper.

For a local POSIX generation run, set the real formatter path before changing `PATH`:

```bash
export MODULARPIPELINES_REAL_GROFF="$(command -v groff)"
export PATH="$PWD/tools/ModularPipelines.OptionsGenerator/scripts/aws-help-renderer:$PATH"
# Run the generator from this environment.
```

Run `bash tools/ModularPipelines.OptionsGenerator/scripts/aws-help-renderer/Test-AwsHelpRenderer.sh`
before changing `PATH` to check long command tokens and description words with the
installed formatter. The hosted AWS installation step runs this check.

Sources: [AWS help renderer](https://github.com/aws/aws-cli/blob/v2/awscli/help.py),
[groff man registers](https://www.gnu.org/software/groff/manual/groff-man-pages.pdf).
