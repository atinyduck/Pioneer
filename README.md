# Pioneer 🤖 
**Promoting Computational Thinking through Gamified Exploration**

Pioneer is a Unity-based educational game designed to introduce 14-17 year olds to Computer Science concepts. Players program a drone to solve puzzles across different "Worlds," moving from visual logic to Python syntax.

---

## 🛠 Tech Stack
* **Engine:** Unity 6000.3.9f1 
* **Language:** C# (Unity Backend), Python (Player Scripts via Brython)
* **Platform:** WebGL
* **Version Control:** Git + Git LFS

---

## 🚀 Getting Started (For Developers)

### 1. Prerequisites
* Install [Git LFS](https://git-lfs.com/). **This is mandatory** to handle 3D assets.
* Unity Hub & Unity Editor (Version: 6000.3.9f1).

### 2. Setup
1. Clone the repository:
   `git clone https://github.com/atinyduck/pioneer.git`
2. Initialize LFS:
   `git lfs install`
   `git lfs pull`
3. Open the folder in Unity.

---

## 🌲 Git Workflow & Branching
To keep the project stable, we follow a **Feature Branch** workflow.

* **`main`**: Production-ready code. Only merged after a full QA pass.
* **`develop`**: Integration branch. All features merge here first.
* **`feat/xxx`**: Temporary branches for specific tasks (e.g., `feat/world1-logic`).

**The Golden Rules:**
1. **Pull before you push:** Always run `git pull origin develop` before starting work.
2. **Never work in `main`:** Create a branch for your task.
3. **Unity Scenes:** Coordinate with the team before touching a `.unity` file to avoid merge conflicts. Use **Prefabs** whenever possible.

---

## 📂 Project Structure
* **/Assets/Scripts**: All C# logic.
* **/Assets/Prefabs**: Reusable game objects (Drones, UI, Puzzles).
* **/Assets/Worlds**: Level-specific assets and scenes.
* **/Assets/Plugins/Brython**: The Python-to-JavaScript integration.

---

## 👥 The Team
* **Jake Morgan**: Lead Architect & Development
* **Md Jasim Uddin**: Version Control & QA
* **Amy Owen**: Level & Environment Design
* **Jack Cutler**: Python Integration & Sandbox Logic
* **Md Asiful Hasan Saadi**: API & Database Management

---
*Developed for CMP2804-2526 — Team 12*
