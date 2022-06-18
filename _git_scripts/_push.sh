cd ..

./_git_scripts/pull_not_wait.sh
git switch levels_design || git switch -c levels_design
git push -u origin levels_design

echo 
read -p "Press enter to continue"