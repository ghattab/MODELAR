using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedoUndo : MonoBehaviour
{
    public struct HistoryItem
    {
        public GameObject obj;
        public string effect;
        public float value;
        public Color color;
        
        public HistoryItem(GameObject obj, string effect, float value)
        {
            this.obj = obj;
            this.effect = effect;
            this.value = value;
            this.color = Color.black;
        }
        public HistoryItem(GameObject obj, string effect, Color color)
        {
            this.obj = obj;
            this.effect = effect;
            this.value = 0;
            this.color = color;
        }
    }
    static int currentBehindState = 0;
    public static List<HistoryItem> history = new List<HistoryItem>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void addToHistory(GameObject obj, string effect, float value)
    {
        for(int i = 0; i < currentBehindState; i++)
        {
            history.RemoveAt(history.Count-1);
        }
        currentBehindState = 0;
        history.Add(new HistoryItem(obj, effect, value));
    }
    public static void addToHistory(GameObject obj, string effect, Color color)
    {
        for (int i = 0; i < currentBehindState; i++)
        {
            history.RemoveAt(history.Count - 1);
        }
        currentBehindState = 0;
        history.Add(new HistoryItem(obj, effect, color));
    }

    public void redo()
    {
        if(currentBehindState > 0) {
            currentBehindState--;
            HistoryItem item = history[history.Count - currentBehindState -1];
            if(item.effect != "_Color") {
                item.obj.GetComponent<MeshRenderer>().material.SetFloat(item.effect, item.value);

            }
            else {
                item.obj.GetComponent<MeshRenderer>().material.SetColor("_Color", item.color);
                Category10ColorSelect.markSelectedButton(item.color);
                Debug.Log("Color set");
            }
            Debug.Log("Redo");
        }
        Debug.Log(history.Count - currentBehindState + "/" + history.Count);
        GameObject.FindObjectOfType<SliderApply>().UpdateSliders();
    }

    public void undo()
    {
        if (currentBehindState < history.Count - 1)
        {
            currentBehindState++;
            HistoryItem item = history[history.Count - currentBehindState - 1];

            if (item.effect != "_Color")
            {
                item.obj.GetComponent<MeshRenderer>().material.SetFloat(item.effect, item.value);

            }
            else
            {
                item.obj.GetComponent<MeshRenderer>().material.SetColor("_Color", item.color);
                Category10ColorSelect.markSelectedButton(item.color);
                Debug.Log("Color set");
            }
            Debug.Log("Undo");
        }
        Debug.Log(history.Count - currentBehindState + "/" + history.Count);
        GameObject.FindObjectOfType<SliderApply>().UpdateSliders();
    }
}
