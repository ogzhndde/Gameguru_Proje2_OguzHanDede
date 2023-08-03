using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CMCam
{
    CMMain
}

public class CameraManager : InstanceManager<CameraManager>
{
    public CMCam cMCamEnum;
    public GameObject CMMain;
    public List<GameObject> CamList = new List<GameObject>();

    void Start()
    {
        CamList.Add(CMMain);

        InvokeRepeating("CamControl", .1f, .1f);
    }

    public void CamControl()
    {
        switch (cMCamEnum)
        {
            case CMCam.CMMain:
                CamUpdate(CMMain);
                break;
        }
    }

    public void CamUpdate(GameObject activeCam)
    {
        for (int i = 0; i < CamList.Count; i++)
        {
            if (CamList[i] != activeCam)
                CamList[i].SetActive(false);

            if (CamList[i] == activeCam)
                CamList[i].SetActive(true);

        }
    }

    // IEnumerator CameraChange(CMCam currentCam, CMCam previousCam) //SATIN ALINILAN YERI BIRKAC SANIYE GOSTERIYOR
    // {
    //     cMCamEnum = currentCam;

    //     yield return new WaitForSeconds(3f);

    //     cMCamEnum = previousCam;

    // }

    //########################################    EVENTS    ###################################################################

    private void OnEnable()
    {
        // EventManager.AddHandler(GameEvent.OnStart, OnStart);
    }

    private void OnDisable()
    {
        // EventManager.RemoveHandler(GameEvent.OnStart, OnStart);
    }

}
