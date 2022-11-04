using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSprite : MonoBehaviour
{
    public static GoalSprite instance = null;
    public GameObject beforeGoal;
    public GameObject afterGoal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Goaled()
    {
        afterGoal.SetActive(true);
        beforeGoal.SetActive(false);
    }
}
