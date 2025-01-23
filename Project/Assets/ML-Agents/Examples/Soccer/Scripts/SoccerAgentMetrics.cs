using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Policies;
using System.Collections.Generic;
using System.IO;
using System;

public class SoccerAgentMetrics : MonoBehaviour
{
    private Agent agent;
    private string teamName;
    private float lastReward = 0f;
    private int goalsScored = 0;
    private int goalsConceded = 0;
    private string logFilePath;

    void Start()
    {
        // Get the Agent component from this GameObject
        agent = GetComponent<Agent>();
        if (agent == null)
        {
            Debug.LogError("No Agent component found!");
            return;
        }

        // Get the behavior name to identify the team
        var behaviorParams = GetComponent<BehaviorParameters>();
        teamName = behaviorParams.BehaviorName;
        Debug.Log($"Started tracking agent: {teamName}");

        // Setup logging
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        logFilePath = Path.Combine(Application.dataPath, $"SoccerMetrics_{teamName}_{timestamp}.csv");
        File.WriteAllText(logFilePath, "Time,Team,Reward,Goals,Conceded\n");
    }

    void FixedUpdate()
    {
        if (agent == null) return;

        // Get the current reward directly from the agent
        float reward = agent.GetCumulativeReward();
        float deltaReward = reward - lastReward;

        // Only log when there's a significant reward change
        if (Mathf.Abs(deltaReward) > 0.001f)
        {
            Debug.Log($"{teamName} reward: {deltaReward:F6} (cumulative: {reward:F6})");

            // Check for goals (large rewards)
            if (Mathf.Abs(deltaReward) > 0.9f)
            {
                if (deltaReward > 0)
                {
                    goalsScored++;
                    Debug.Log($"{teamName} SCORED! Total goals: {goalsScored}");
                }
                else
                {
                    goalsConceded++;
                    Debug.Log($"{teamName} CONCEDED! Total conceded: {goalsConceded}");
                }

                // Log to file
                string logEntry = $"{DateTime.Now:HH:mm:ss},{teamName},{reward:F6},{goalsScored},{goalsConceded}\n";
                try
                {
                    File.AppendAllText(logFilePath, logEntry);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to write to log: {e.Message}");
                }
            }
        }

        lastReward = reward;
    }

    void OnEpisodeBegin()
    {
        Debug.Log($"\n=== New Episode for {teamName} ===");
        Debug.Log($"Previous episode stats:");
        Debug.Log($"Goals scored: {goalsScored}");
        Debug.Log($"Goals conceded: {goalsConceded}");
        Debug.Log($"Final reward: {lastReward:F6}");
        
        // Reset episode-specific tracking
        lastReward = 0f;
    }

    void OnDisable()
    {
        Debug.Log($"\n=== Final Stats for {teamName} ===");
        Debug.Log($"Total Goals: {goalsScored}");
        Debug.Log($"Total Conceded: {goalsConceded}");
        Debug.Log($"Final Reward: {lastReward:F6}");
        Debug.Log($"Stats saved to: {logFilePath}");
    }
} 