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

## CMake 
Is required to build certain components in the Unity ML-Agents environment. Depending on your operating system, follow the instructions below to install CMake and resolve any potential errors.

Installing CMake (Windows)
  Install CMake via Chocolatey:
   ```bash
   choco install cmake
   ```

Installing CMake (macOS):
1. Install Homebrew (if not already installed):
   ```bash
   /bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"
   ```
2. Once Homebrew is installed, install CMake via Homebrew:
   ```bash
   brew install cmake
   ```

Installing CMake (Linux):
1. Update your package manager and install CMake:
   ```bash
   sudo apt update
   sudo apt install cmake
   ```
2. Verify the installation:
   ```bash
   cmake --version
   ```
   
### CMake Errors

**"CMake not found" Error**: this error typically means that CMake isn't installed or is not in your system's PATH.

-**Windows**: during installation, ensure you check the box to "Add CMake to the system PATH for all users".
-**macOS**: ensure you run the following to add CMake to your path.
  ```bash
  echo 'export PATH="/usr/local/bin:$PATH"' >> ~/.zshrc
  
  source ~/.zshrc
  ```
-**Linux**: make sure CMake is installed and in your PATH by checking:
  ```bash
  cmake --version
  ```

**"CMake version mismatch" Error**

Ensure you have the correct version of CMake installed:
  ```bash
  cmake --version
  ```
If the installed version is incompatible, update CMake using the package manager:

-**Windows**:
  ```bash
  choco upgrade cmake
  ```
-**macOS**:
  ```bash
  brew upgrade cmake
  ```
-**Linux**:
  ```bash
  sudo apt upgrade cmake
  ```
**Build Errors Related to CMake Cache**
If you encounter build errors due to cache issues, you can clear the CMake cache by removing the CMakeCache.txt and CMakeFiles/ from the directory, then rerun the CMake command:

  ```bash
  rm -rf CMakeCache.txt CMakeFiles/
  
  cmake .
  ```

## Training Agents

### Training Configuration
Training configurations are defined in YAML files. Here's a basic example:

```yaml
behaviors:
  3DBall:  # Your behavior name
    trainer_type: ppo
    hyperparameters:
      batch_size: 64
      buffer_size: 12000
      learning_rate: 3.0e-4
      beta: 5.0e-4
      epsilon: 0.2
      lambd: 0.95
      num_epoch: 3
    network_settings:
      normalize: false
      hidden_units: 128
      num_layers: 2
    reward_signals:
      extrinsic:
        gamma: 0.99
        strength: 1.0
    max_steps: 500000
    time_horizon: 64
    summary_freq: 10000
```

### Running Training

To start training:
1. Open a terminal window
2. Navigate to your project directory
3. Run the training command:
```bash
mlagents-learn config/ppo/3DBall.yaml --run-id=first3DBallRun
```

Key parameters:
- `config/ppo/3DBall.yaml`: Path to your configuration file
- `--run-id`: Unique identifier for your training run

### Monitoring Training Progress

There are two main ways to monitor your training progress:

1. **Command Line Output**
   During training, you'll see progress in your terminal:
   ```bash
   INFO:mlagents.trainers:first3DBallRun: 3DBall: Step: 1000. Mean Reward: 0.8. Std of Reward: 0.4.
   ```

2. **TensorBoard Visualization**
   For detailed metrics and graphs:

   a. Start TensorBoard (use either command):
   ```bash
   tensorboard --logdir results
   # OR
   tensorboard --logdir=results
   ```

   b. Open `localhost:6006` in your web browser

   c. Key metrics to monitor:
   - Environment/Cumulative Reward (ELO): Shows overall agent performance
   - Policy/Learning Rate: Shows training progression
   - Episode Length: Shows the length of each match, indicating how fast teh agents "score"

   d. TensorBoard tips:
   - Compare multiple runs by selecting different runs in the sidebar
   - Smooth graphs using the slider in TensorBoard
   - Export data for further analysis
   - Use different tabs (SCALARS, IMAGES, GRAPHS) for different views of your training data

Note: Training results are saved in the `results` directory, with each run in its own subdirectory based on the `run-id` you specified.

### Creating Executable Environments

To create an executable of your environment:

1. In Unity Editor:
   - Go to File > Build Settings
   - Select your target platform
   - Set architecture to x86_64
   - Enable "Headless Mode" for server builds
   - Click "Build"

2. Training with executable:
```bash
mlagents-learn config/ppo/3DBall.yaml --env=./3DBall --run-id=training1
```

### Advanced Training Options

1. **Resume Training**:
```bash
mlagents-learn config/ppo/3DBall.yaml --run-id=training1 --resume
```

2. **Initialize from Previous Model**:
```bash
mlagents-learn config/ppo/3DBall.yaml --run-id=training2 --initialize-from=training1
```

3. **Multiple Environment Instances**:
```bash
mlagents-learn config/ppo/3DBall.yaml --run-id=training1 --num-envs=2
```

### YAML Configuration Details

Key configuration sections:

1. **Hyperparameters**:
```yaml
hyperparameters:
  batch_size: 64        # Number of experiences per iteration
  buffer_size: 12000    # Number of experiences to collect before training
  learning_rate: 3.0e-4 # Learning rate for neural network
  beta: 5.0e-4         # Entropy regularization strength
  epsilon: 0.2         # Trust region clipping value
```

2. **Network Settings**:
```yaml
network_settings:
  normalize: false      # Whether to normalize observations
  hidden_units: 128    # Number of units in hidden layers
  num_layers: 2        # Number of hidden layers
```

3. **Reward Signals**:
```yaml
reward_signals:
  extrinsic:
    gamma: 0.99        # Discount factor for future rewards
    strength: 1.0      # Relative strength of this reward
```

### Additional Training Limitations

- Training only works with Mono scripting backend
- GPU inference not supported on WebGL and GLES 3/2 mobile platforms
- Headless mode doesn't support visual observations
- Physics speed limited to 100x real-time
- Training requires Unity 2019.4 or later

### Troubleshooting Training Issues

1. **Memory Issues**:
   - Reduce `buffer_size` and `batch_size` in your YAML configuration
   - Limit the number of concurrent environment instances

2. **Performance Issues**:
   - Enable GPU training when possible
   - Adjust `time_horizon` and `summary_freq` values
   - Consider using headless mode for faster training

3. **Learning Issues**:
   - Adjust `learning_rate` and `beta` values
   - Increase `num_epoch` for more training iterations
   - Modify network architecture (`hidden_units` and `num_layers`)

## Existing Documentation & Credentials
For further details on features, installation, and other documentation, refer to the original repository's documentation and credentials provided in both the forked and original repositories.

## References
- Original fork from Dennis Soemers: : https://github.com/DennisSoemers/ml-agents/tree/fix-numpy-release-21-branch



   
