# Generator formatting in CI

The `.NET` workflow verifies changed handwritten OptionsGenerator C# files with
the repository SDK and the generator solution. The fast-fail job propagates any
formatter failure to the required `pipeline (ubuntu-latest)` aggregate. The core
analyzer check remains separate.

Pull requests compare against their merge base; pushes compare the complete push
range. Deleted files and generated C# output are excluded. Unrelated changes skip
the formatter. A manual `.NET` workflow dispatch verifies all tracked handwritten
generator C# files on the selected branch, including when no new source change is
available to trigger verification. Use that dispatch to obtain hosted evidence
for a feature branch after a local validation limit.

The formatter runs read-only with `--verify-no-changes --severity info`, after the
generator restore and tests. It has a 600-second process guard and a 12-minute
step limit; guard exits 124 and 137 fail validation. Do not interpret skipped
formatting or a formatting timeout as a successful check.

Run selection regressions with `pwsh scripts/Test-GeneratorFormattingFiles.ps1`.
