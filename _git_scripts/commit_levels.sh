if [[ -z $1 ]]; then
	message="--- $(date +%D-%T)"
else
	message=$1
fi

echo
echo "Haciendo commit con el mensaje "\"$message\"
git switch levels_design || git switch -c levels_design
git add ./Assets/Scenes/Levels/
git commit -m "$message"
git restore .
echo 