#./_git_scripts/commit_levels.sh "commit before pulling"
./_git_scripts/commit_levels_with_prompt.sh

git switch dev
git pull
git switch levels_design
git merge dev --no-edit -m "Automatic merge: dev -> levels_design from pull_not_wait.sh script"
