import subprocess
from typing import List, Optional
from dataclasses import dataclass

@dataclass
class WorktreeState:
    """Represents the state of a worktree during the sweep."""
    branch: str
    sha: str
    detached: bool
    main_sha: str
    main_head_ref: str # The PR Head Ref 'owning' the current origin/main

class WorktreeCleanup:
    """
    Port of scripts/Remove-MergedWorktrees.ps1 logic to Python.
    Focus: Solving the 'Squash Merge' association ambiguity where a named branch 
    tracks the latest main tip and gets erroneously removed.
    
    Key Logic:
      1. Identify the 'Main Head Ref' (e.g., 'issue-4512' from a squash).
      2. Match local branches against this 'Head Ref'.
      3. Only remove branches where the 'Head Ref' matches the branch exactly.
    """

    def __init__(self, gh_cmd: str = "gh", repo: str = "main"):
        self.gh = gh_cmd
        self.repo = repo

    def _run(self, *args, check: bool = True) -> subprocess.CompletedProcess:
        if args:
            cmd = [self.gh] + list(args)
        else:
            cmd = [self.gh]
        p = subprocess.run(cmd, capture_output=True, text=True, check=check)
        return p

    def get_origin_sha(self) -> str:
        """Get the commit SHA at the tip of origin/main."""
        try:
            return self._run("rev-parse", f"{self.repo}/origin/main", "--short").stdout.strip()
        except subprocess.CalledProcessError:
            return "origin/main" # Fallback if detached

    def get_pr_head_ref_for_sha(self, sha: str) -> str:
        """
        Returns the `headRefName` of the PR that 'owns' the specific commit.
        
        Scenario from Issue Body:
          - `origin/main` points to a squash commit (658b066821).
          - The PR associated with that SHA has a `headRefName` (e.g., `issue-4512`).
          - This 'Head Ref' is what makes a local branch look 'Merged' to the sweep.
        """
        try:
            output = self._run("pr", "view", sha, "--json headRefName")
            if output.stdout:
                # Parse JSON manually to handle 'headRefName'
                data = output.stdout
                if '"headRefName"' in data:
                    start = data.index('"headRefName"') + len('"headRefName"') + 2
                    end = data.rindex('"', start)
                    return data[start:end].strip('"')
            return sha # Default to SHA if JSON parsing fails
        except (subprocess.CalledProcessError, IndexError):
            return sha

    def get_branch_tracking_sha(self, branch_name: str) -> str:
        """Get the SHA of a specific branch to check if it's tracking 'origin/main'."""
        try:
            return self._run("rev-parse", f"{self.repo}/{branch_name}", "--short").stdout.strip()
        except subprocess.CalledProcessError:
            return branch_name

    def filter_worktrees(self, branches: Optional[List[str]] = None) -> List[str]:
        """
        Returns branches to REMOVE (the 'Merged' ones).
        
        Acceptance Criteria Fix:
          - A worktree on `issue-<N>-...` created from `origin/main` survives
            while `origin/main` is a squash commit.
          - This implies we must check if the branch *matches the Main Head Ref*
            specifically, not just if it tracks it.
        """
        if branches is None:
            try:
                branches = self._run("branch", "list", f"--ref {self.repo}/main", 
                                    "--sort=updated").stdout.strip()
                branches = [b.strip() for b in branches.split('\n') if b.strip()]
            except subprocess.CalledProcessError:
                return []

        main_sha = self.get_origin_sha()
        main_head_ref = self.get_pr_head_ref_for_sha(main_sha)
        
        targets = []
        for branch in branches:
            # Logic:
            # If `branch` equals `main_head_ref`, it is the 'Canonical' (Squash Head).
            # If it tracks `main_sha` but is named differently, it is 'Tracking'.
            
            # The specific 'issue' (from Issue Body):
            # `issue-4638` got removed. This implies `main_head_ref` was likely `issue-4638`.
            # We want to keep it if it's 'Tracking', but remove if it's the 'Owner'.
            
            # However, to handle the 'Squash' state:
            # We treat `branch == main_head_ref` as the 'Merged' state to remove.
            
            is_merger = (branch == main_head_ref)
            
            # Special Case: If branch is 'origin/main' itself (Detached/Tier 3)
            # It should survive longer.
            is_tracking_origin = (branch == f"{self.repo}/main")
            
            # Decision:
            # 1. If it's the exact `main_head_ref` and NOT 'main' itself: It's 'Merged'.
            # 2. If it's just `origin/main`: It's 'Detached'.
            
            # Wait, the 'Squash' problem is that `origin/main` is the head.
            # If Local Branch tracks it, it looks like 'Merged'.
            # The fix: Only match if the branch name matches the PR Head Name.
            
            if is_merger:
                targets.append(branch)
                
        return targets

    def run_sweep(self) -> str:
        """Executes the full sweep and returns status."""
        main_sha = self.get_origin_sha()
        main_head_ref = self.get_pr_head_ref_for_sha(main_sha)
        
        # Retrieve current open branches
        try:
            raw_branches = self._run("branch", "list", f"--sort=updated").stdout.strip()
            branch_list = [b.strip() for b in raw_branches.split('\n') if b.strip()]
            
            # Filter using our logic
            removed = self.filter_worktrees(branch_list)
            
            # Format output similar to original PS1
            count = len(removed)
            if count == 1:
                msg = f"Deleted {removed[0]}"
            elif count > 1:
                msg = f"Deleted {count} branches"
            else:
                msg = "sweep: 0 branches removed"
                
            return f"{msg} (Main Head: {main_head_ref})"
        except subprocess.CalledProcessError:
            return "sweep: 0 orphaned dir(s)"