using UnityEngine;

public class Phase1 : baseKrakenState
{
    private float headAttackTimer;
    private float tentacleSpawnTimer;

    private float headAttackCooldown = 5f;
    private float tentacleSpawnCooldown = 3f;

    krakenStateMachine sm;

    public Phase1(krakenStateMachine s) : base(s)
    {
        sm = s;
    }

    public override void FrameUpdate()
    {
        HandleHeadAttack();
        HandleTentacleSpawning();
    }

    private void HandleHeadAttack()
    {
        headAttackTimer += Time.deltaTime;

        if (headAttackTimer >= headAttackCooldown)
        {
            headAttackTimer = 0f;

            var headScript = sm.data.head;

            if (headScript != null)
                 switch (Random.Range(0, 3))
                 {
                     case 0:
                         headScript.WaterLaser();
                         break;
                     case 1:
                         headScript.HeadUpAndDown();
                         break;
                }
        }
    }

    private void HandleTentacleSpawning()
    {
        if (sm.data.spawnedTentacles.Count < sm.data.maxTentacles)
        {
            tentacleSpawnTimer += Time.deltaTime;

            if (tentacleSpawnTimer >= tentacleSpawnCooldown)
            {
                tentacleSpawnTimer = 0f;

                SpawnTentacle();
            }
        } 
    }

    private void SpawnTentacle()
    {
        var controller = sm.data;

        if (controller.tentacles.Length == 0)
            return;

        var prefab = controller.tentacles[Random.Range(0, controller.tentacles.Length)];

        Vector3 spawnPos = GetSpawnPosition();

        var tentacleObj = GameObject.Instantiate(prefab, spawnPos, Quaternion.identity);

        sm.data.spawnedTentacles.Add(tentacleObj);

        var tentacle = tentacleObj.GetComponent<TentacleAttack>();


        //if (tentacle != null)
            //tentacle.Init(controller.player);
    }

    private Vector3 GetSpawnPosition()
    {
        Vector3 centerPos = sm.data.transform.position;
        Vector3 playerPos = sm.data.player.transform.position;

        float maxSpawnRadius = sm.data.tentacleSpawnRadius;
        float aroundPlayerRadius = sm.data.tentaclePlayerSpawnRadius;

        for (int i = 0; i < 20; i++)
        {
            // Posición aleatoria alrededor del jugador
            Vector2 randomCircle =
                Random.insideUnitCircle * aroundPlayerRadius;

            Vector3 candidatePos = new Vector3(
                playerPos.x + randomCircle.x,
                centerPos.y,
                playerPos.z + randomCircle.y
            );

            // Verifica que esté dentro del área permitida
            float distanceToCenter =
                Vector3.Distance(centerPos, candidatePos);

            if (distanceToCenter <= maxSpawnRadius)
            {
                return candidatePos;
            }
        }

        // Fallback si no encuentra posición válida
        return centerPos;
    }
}
