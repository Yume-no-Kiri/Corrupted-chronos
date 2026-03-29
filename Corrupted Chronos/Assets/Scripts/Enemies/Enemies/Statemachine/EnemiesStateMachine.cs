using UnityEngine;

public class EnemiesStateMachine: baseStateMachine
{
    public EnemyPair enemyPair;
    public EnemiesStateMachine(EnemyPair p)
    {
        enemyPair = p;
    }
}
