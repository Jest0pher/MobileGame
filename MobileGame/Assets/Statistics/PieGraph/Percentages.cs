using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Percentages : MonoBehaviour
{
    public List<TMP_Text> textMeshes;
    public float textR;

    // Start is called before the first frame update
    void Start()
    {
        
        textMeshes[(int)TeamTypes.Neutral].color = Color.white;
        textMeshes[(int)TeamTypes.Red].color = Color.red;
        textMeshes[(int)TeamTypes.Green].color = Color.green;
        textMeshes[(int)TeamTypes.Blue].color = Color.blue;
        textMeshes[(int)TeamTypes.Purple].color = Color.magenta;
        textMeshes[(int)TeamTypes.Orange].color = new Color(1.0f, .5f, 0, 1);
        textMeshes[(int)TeamTypes.Yellow].color = Color.yellow;

        Vector2 vec = Vector2.zero;
        float radianRot = (2 * Mathf.PI) / textMeshes.Count;

        for (int i = 0; i < textMeshes.Count; i++) {
            vec.Set(textR * Mathf.Sin(radianRot*i), textR * Mathf.Cos(radianRot*i));
            textMeshes[i].gameObject.transform.localPosition = vec;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < textMeshes.Count; i++) {
            float percentage = ((float)GameManager.Instance.teamCounts[(TeamTypes)i] / (float)GameManager.Instance.grid.activeNodes) * 10000.0f;
            percentage = (int)percentage;
            percentage /= 100.0f;
            string text = percentage.ToString();
            textMeshes[i].text = text + "%";       
        }
    }
}
