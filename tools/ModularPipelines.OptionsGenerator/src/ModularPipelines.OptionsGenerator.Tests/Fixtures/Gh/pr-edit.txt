Edit a pull request.

Without an argument, the pull request that belongs to the current branch
is selected.

Editing a pull request's projects requires authorization with the `project` scope.
To authorize, run `gh auth refresh -s project`.

Use `--attach` to upload an image or video. Without a body flag the pull
request keeps the body it already has and the attachment is appended to it. If the
body references an attached file, such as `![alt](./login.png)`, that reference
is rewritten to point at the uploaded asset instead.
You can attach up to 50 files per command.

Alt text for an image follows the path after `#`, as in
`--attach './login.png#The login error state'`. Without it the filename is used.
A reference already in the body keeps the alt text written there. Video renders
as a player and has no alt text, so it cannot be given any.

If some attachments upload and others fail, the pull request is still updated with the
ones that succeeded. The command then exits with a non-zero status, but the pull
request's URL is still printed to stdout.

The `--add-assignee` and `--remove-assignee` flags both support
the following special values:
- `@me`: assign or unassign yourself
- `@copilot`: assign or unassign Copilot (not supported on GitHub Enterprise Server)

The `--add-reviewer` and `--remove-reviewer` flags support
the following special value:
- `@copilot`: request or remove review from Copilot (not supported on GitHub Enterprise Server)


USAGE
  gh pr edit [<number> | <url> | <branch>] [flags]

FLAGS
      --add-assignee login      Add assigned users by their login. Use "@me" to assign yourself, or "@copilot" to assign Copilot.
      --add-label name          Add labels by name
      --add-project title       Add the pull request to projects by title
      --add-reviewer login      Add or re-request reviewers by their login. Use "@copilot" to request review from Copilot.
      --attach file             Attach an image or video file, in '<file>#<image alt text>' format
  -B, --base branch             Change the base branch for this pull request
  -b, --body string             Set the new body.
  -F, --body-file file          Read body text from file (use "-" to read from standard input)
  -m, --milestone name          Edit the milestone the pull request belongs to by name
      --remove-assignee login   Remove assigned users by their login. Use "@me" to unassign yourself, or "@copilot" to unassign Copilot.
      --remove-label name       Remove labels by name
      --remove-milestone        Remove the milestone association from the pull request
      --remove-project title    Remove the pull request from projects by title
      --remove-reviewer login   Remove reviewers by their login. Use "@copilot" to remove review request from Copilot.
  -t, --title string            Set the new title.

INHERITED FLAGS
      --help                     Show help for command
  -R, --repo [HOST/]OWNER/REPO   Select another repository using the [HOST/]OWNER/REPO format

EXAMPLES
  # Edit the title and body of a pull request
  $ gh pr edit 23 --title "I found a bug" --body "Nothing works"

  # Use a file as the body
  $ gh pr edit 23 --body-file body.txt

  # Append a screenshot to the body, with alt text after "#"
  $ gh pr edit 23 --attach './login.png#The login error state'

  # Append multiple files by repeating the flag
  $ gh pr edit 23 --attach ./before.png --attach ./after.png

  # Manage labels
  $ gh pr edit 23 --add-label "bug,help wanted" --remove-label "core"

  # Manage reviewers
  $ gh pr edit 23 --add-reviewer monalisa,hubot --remove-reviewer myorg/team-name

  # Re-request review
  $ gh pr edit 23 --add-reviewer monalisa

  # Request a review from GitHub Copilot
  $ gh pr edit 23 --add-reviewer "@copilot"

  # Manage assignees
  $ gh pr edit 23 --add-assignee "@me" --remove-assignee monalisa,hubot

  # Assign GitHub Copilot
  $ gh pr edit 23 --add-assignee "@copilot"

  # Manage projects and milestones
  $ gh pr edit 23 --add-project "Roadmap" --remove-project v1,v2
  $ gh pr edit 23 --milestone "Version 1"
  $ gh pr edit 23 --remove-milestone

LEARN MORE
  Use `gh <command> <subcommand> --help` for more information about a command.
  Read the manual at https://cli.github.com/manual
  Learn about exit codes using `gh help exit-codes`
  Learn about accessibility experiences using `gh help accessibility`
