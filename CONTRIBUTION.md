# 🚀 Pioneer: Team Onboarding & Workflow

Welcome to the **Pioneer** development team! As we move into **Milestone 2 (Core System Development)**, please follow these steps to ensure our Unity project remains stable.

## 🛠️ Environment Setup
* **Git LFS:** Mandatory for 3D models and textures.
    * Download: [git-lfs.com](https://git-lfs.com/)
    * Run: `git lfs install`
* **Unity Version:** Locked to **Unity 2022.3 LTS**. 
* **.NET SDK:** Ensure you have the .NET SDK installed for C# IntelliSense.

## 📂 Repository Structure
All project work happens in `Assets/_Project/`. Please stick to your designated folders:
* **Core/**: Command Queue and Architecture (**Jake**)
* **Art/**: Environments and Models (**Amy**)
* **Scripts/Python/**: Brython and Sandbox logic (**Jack**)
* **Scripts/API/**: RESTful API and Database integration (**Asiful**)
* **QA/**: Testing scripts and optimization (**Jasim**)

## 🌲 Git Workflow
To avoid "Merge Hell," follow these rules:
1. **Never push to `main`:** It is a protected branch.
2. **Feature Branches:** Create a branch for every task: 
   `git checkout -b feat/yourname-task-description`
3. **Pull Before Work:** Run `git pull origin develop` every morning.
4. **The Scene Rule:** Only one person edits a `.unity` scene at a time. Work in **Prefabs** whenever possible to avoid clashing files.

## ✅ Submitting Work
1. Push your feature branch to GitHub.
2. Open a **Pull Request (PR)** to merge into `develop`.
3. Request a review from **Jasim (QA)** or **Jake (Lead)**.
