# 🚀 Pioneer: Team Onboarding & Workflow

Welcome to the **Pioneer** development team! To keep our Unity project stable and our Git history clean, please follow these steps before you start coding.

## 1. Environment Setup
* **Git LFS:** We use Large File Storage for 3D models and textures. You **must** install it before cloning.
    * Download: [git-lfs.com](https://git-lfs.com/)
    * Command: `git lfs install`
* **Unity Version:** We are locked to **Unity 2022.3 LTS**. 
* **IDE:** VS Code (with C# Dev Kit) or JetBrains Rider is recommended for Linux/Windows compatibility.

## 2. Getting the Code
1.  Clone the repo: `git clone https://github.com/atinyduck/Pioneer.git`
2.  Switch to the development branch: `git checkout develop`
3.  Ensure LFS assets are pulled: `git lfs pull`

## 3. The "Golden Rules" of the Workflow
* **Rule #1: The Main Branch is Lava.** Never push directly to `main`. 
* **Rule #2: Feature Branches.** Create a branch for every task: `git checkout -b feat/your-name-task-description`.
* **Rule #3: Prefabs > Scenes.** Do not move objects around in the main scene files if you can avoid it. Build your logic inside a **Prefab** in your own folder, then drag it into the scene. This prevents merge conflicts.
* **Rule #4: Pull Frequently.** Run `git pull origin develop` every time you sit down to work to ensure you have the latest changes from the rest of the team.

## 4. Folder Hierarchy (Where to put your stuff)
Everything we create lives in `Assets/_Project/`. Please respect the subfolders:
* **Art/**: Models, Textures, and Environments (Amy)
* **Scripts/Python/**: Brython and Sandbox logic (Jack)
* **Scripts/API/**: Backend and Database integration (Asiful)
* **Scripts/Core/**: Game engine and Command Queue (Jake)
* **Prefabs/**: All reusable game objects.

## 5. Submitting Work
When your feature is done:
1.  Push your branch to GitHub.
2.  Open a **Pull Request (PR)** to merge into `develop`.
3.  Tag **Jasim** (QA) or **Jake** (Lead) for a code review.
