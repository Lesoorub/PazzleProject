using UnityEngine;

[CreateAssetMenu(fileName = "person", menuName = "core/AI Person")]
public class AIPerson : Person
{
    public FightAI.AIDifficulty Difficulty = FightAI.AIDifficulty.Medium;
}
