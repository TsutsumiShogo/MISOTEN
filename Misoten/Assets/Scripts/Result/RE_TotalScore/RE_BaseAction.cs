using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_BaseAction : MonoBehaviour {


    virtual public void Init()
    {

    }

    virtual public TotalScore.RE_TOTAL_STEP Action()
    {
        return TotalScore.RE_TOTAL_STEP.HEADER_IN;
    }

    
}
