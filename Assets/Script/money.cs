using UnityEngine;
using TMPro;

public class money : MonoBehaviour
{
    public TMP_Text textUI;
    public TextMeshProUGUI tmp;

    public move pmoney;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textUI.text = "Money: " +pmoney.money;   
    }
}
