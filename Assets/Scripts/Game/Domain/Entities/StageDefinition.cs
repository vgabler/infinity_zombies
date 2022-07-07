using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage", menuName = "Stages/Stage Definition")]
public class StageDefinition : ScriptableObject
{
    public float waveTimeout = 40;
    public float defaultWaveDelay = 5;
    public List<Wave> waves = new List<Wave>();
}

//TODO criar um custom editor pra poder mostrar os nomes dos prefabs lá em baixo

[System.Serializable]
public class Wave
{
    [Tooltip("Sobreescreve o delay padrão da fase")]
    public float waveDelay = -1;
    public float defaultActionDelay = 2;
    public List<GameObject> prefabs = new List<GameObject>();
    public List<WaveAction> actions = new List<WaveAction>();
}

[System.Serializable]
public class WaveAction
{
    public float actionDelay = -1;
    [Tooltip("Indices dos prefabs a serem instanciados")]
    public List<int> spawns = new List<int>();
    //TODO em vez disso, criar uma lista de int (index) pra int (quantidade)
}
