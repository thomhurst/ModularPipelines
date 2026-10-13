# Git 2.39.0 help fixtures

Captured from the installed Git for Windows executable on 13 October 2026 for
the final root-option audit in #5731. `version.txt` records
`git version 2.39.0.windows.1`; `root-help.txt` and `apply-help.txt` contain
the complete `git -h` and `git apply -h` responses (exit codes 0 and 129,
respectively).

Root usage advertises inherited execution settings alongside version/help actions,
path-reporting actions, and the internal `--super-prefix` argument. Tests accept
only the execution settings. The apply fixture verifies that its integer `-C`
context requirement remains distinct from the repeated root `-C` directory setting.

These fixtures complement the newer [2.56.0 fixtures](../2.56.0/README.md).
They do not replace complete Linux regeneration or its command-coverage manifest.
