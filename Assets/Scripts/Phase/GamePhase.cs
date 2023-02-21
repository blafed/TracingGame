using UnityEngine;

public class GamePhase : Phase<GamePhase>
{
    public int letterId { get; set; }
    public Vector3 origin { get; set; }
}