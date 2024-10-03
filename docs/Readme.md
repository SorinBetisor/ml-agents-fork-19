# Unity ML-Agents Toolkit Fork - Group 19

[![docs badge](https://img.shields.io/badge/docs-reference-blue.svg)](https://github.com/Unity-Technologies/ml-agents/tree/release_21_docs/docs/)

[![license badge](https://img.shields.io/badge/license-Apache--2.0-green.svg)](../LICENSE.md)

([latest release](https://github.com/Unity-Technologies/ml-agents/releases/tag/latest_release))  
([all releases](https://github.com/Unity-Technologies/ml-agents/releases))

## Project Overview
This project focuses on the implementation and analysis of Deep Reinforcement Learning (DRL) algorithms applied to agents capable of navigating and making decisions in complex, real-time 3D environments. Using the Unity game Engine and Unity ML-Agents Toolkit, we aim to develop a system where agents can learn from their environment and improve their behaviour through constant interactiong with different game environments.

Our goal is to test the effectiveness of these DRL algorithms and make the agents more adaptable to complex environments, focusing on their ability to make real-time decisions and interact easily with the game world.

## Key Objectives
- **DRL Exploration**: Implement, train, and analyze DRL algorithms to control agents in real-time 3D environments.
- **Improve Agent Behaviour**: Optimize the learning models to help the agents make better decisions and respond more effectively in different game scenarios.
- **Performance Monitoring**: Analyse how well the models perform and find ways to improve computational efficiency.
  
## Setup the Project

### 1. Download the Project

Clone the Unity ML-Agents forked repository:
```bash
git clone https://github.com/SorinBetisor/ml-agents-fork-19.git
```

### 2. Initialization of Project

1. Open **Unity Hub** and navigate to **Projects**.
2. Click on **Add**, then select **Add project from disk**.
3. Navigate to the forked repository folder in your file system.
4. Open this folder and add the **Project** folder as a Unity project.
5. Open this project in Unity.

### Common Errors

- **Missing Packages**:  
   - Go to **Window** > **Package Manager**.
   - In the top-left, change **Packages: In Project** to **Packages: Unity Registry**.
   - Search for any missing packages, most likely **TextMeshPro**.
   - Download the missing packages.

## GitHub Setup Commands

To set up the repository locally, follow these commands:

Clone the repository:
   ```bash
   git clone https://github.com/SorinBetisor/ml-agents-fork-19.git
```
Check out the release-21-branch:
```bash
git checkout release-21-branch
git pull
```

## Conda Environment Setup
Create a new conda environment with Python 3.10.12:
```bash
conda create -n mlagents python=3.10.12
conda activate mlagents
```
Install necessary packages:
```bash
python -m pip install ./ml-agents-envs
python -m pip install ./ml-agents
```

## CMake Errors
If you encounter errors related to CMake, install CMake with the following commands based on your operating system:

- On macOS:
```bash
brew install cmake 
```

- On Windows:
```bash
choco install cmake 
```

- On Linux:
```bash
sudo apt update
sudo apt install cmake
```

## Existing Documentation & Credentials
For further details on features, installation, and other documentation, refer to the original repository's documentation and credentials provided in both the forked and original repositories.




   
