using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Submit : MonoBehaviour
{
    MainManager Instance;
    // Start is called before the first frame update
    private Button submit;
    void Start()
    {
        Instance = MainManager.Instance;
       submit = gameObject.GetComponent<Button>();
    }


}
