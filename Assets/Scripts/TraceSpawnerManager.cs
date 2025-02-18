using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TraceSpawnerManager : MonoBehaviour
{
    public float cooldown = 0.0f;
    public int tracesCollected = 0;
    public Transform player;
    public GameObject traceNodePrefab;
    public TMP_Text tracesPlacedText;
    public TMP_Text tracesCollectedText;
    public TMP_Text traceCooldownText;
    public List<GameObject> traces;

    public static readonly float COOLDOWN = 0.5f;

    public TraceSpawnerManager Instance
    {
        get => this;
    }

    void Start()
    {
        traces = new List<GameObject>();
        UpdateTracePlacedText();
        UpdateTraceCollectedText();
    }

    void Update()
    {
        ProcessInput();

        if (GameManager.gameState == GameManager.GameState.NormalMode)
        {
            if (cooldown > 0.0f)
            { 
                cooldown -= Time.deltaTime; 
            }
            else
            {
                traceCooldownText.text = "PLACE A TRACE";
                traceCooldownText.color = Color.cyan;
            }
        }
    }

    public void CollectTrace()
    {
        tracesCollected++;
        UpdateTraceCollectedText();
    }

    public void ClearTraces()
    {
        for (int i = 0; i < traces.Count; i++)
        {
            if (traces[i] != null)
            {
                Destroy(traces[i]);
            }
        }
        traces.Clear();
        tracesCollected = 0;
        cooldown = 0.0f;

        UpdateTracePlacedText();
        UpdateTraceCollectedText();
    }

    public bool EnoughTracesPlaced()
    {
        return traces.Count == Constants.MAX_TRACES;
    }

    public bool EnoughTracesCollected()
    {
        return tracesCollected >= Constants.MAX_TRACES * 0.7f;
    }

    void UpdateTraceCollectedText()
    {
        tracesCollectedText.text = string.Format("Traces collected: {0}", tracesCollected);

        if (EnoughTracesCollected())
        {
            tracesCollectedText.color = Color.green;
        }
        else
        {
            tracesCollectedText.color = Color.white;
        }
    }

    void UpdateTracePlacedText()
    {
        tracesPlacedText.text = string.Format("Traces placed: {0}/10", traces.Count);
    }

    void ProcessInput()
    {
        if (GameManager.gameState == GameManager.GameState.NormalMode)
        {
            if (Input.GetKeyDown(KeyCode.C) && cooldown <= 0.0f && traces.Count < Constants.MAX_TRACES)
            {
                GameObject newNode = Instantiate(traceNodePrefab, player.position, Quaternion.identity);
                traces.Add(newNode);
                UpdateTracePlacedText();
                cooldown = COOLDOWN;
                traceCooldownText.text = "TRACE ON COOLDOWN";
                traceCooldownText.color = Color.yellow;
            }
        }
    }
}
