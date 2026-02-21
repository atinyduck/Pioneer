#!/bin/bash

# Define the project subfolders based on team roles
folders=(
    "_Project/Art/Environments"
    "_Project/Art/Models"
    "_Project/Audio"
    "_Project/Prefabs/UI"
    "_Project/Prefabs/Puzzles"
    "_Project/Scenes/Development"
    "_Project/Scripts/Core"
    "_Project/Scripts/API"
    "_Project/Scripts/Python"
)

# Create folders and add .gitkeep to each
for dir in "${folders[@]}"; do
    mkdir -p "$dir"
    touch "$dir/.gitkeep"
    echo "Created: $dir"
done

echo "Structure complete! Now open Unity to let it generate the .meta files."