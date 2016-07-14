using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugText : Singleton<DebugText>
{
    Text debugText;

    protected override void Awake()
    {
        base.Awake();

        debugText = GetComponent<Text>();
    }

    public void SetText(params object[] args)
    {
        debugText.text = "";

        foreach (object o in args)
        {
            debugText.text += o.ToString() + "\n";
        }
    }
}
