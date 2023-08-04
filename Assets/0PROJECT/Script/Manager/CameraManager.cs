using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CMCam
{
    CMMain,
    CMFinish
}

public class CameraManager : InstanceManager<CameraManager>
{
    public CMCam cMCamEnum;
    public GameObject CMMain, CMFinish;
    public List<GameObject> CamList = new List<GameObject>();

    void Start()
    {
        SetCams();

        InvokeRepeating("CamControl", .1f, .1f);
    }

    void SetCams()
    {
        CamList.Add(CMMain);
        CamList.Add(CMFinish);
    }

    public void CamControl()
    {
        switch (cMCamEnum)
        {
            case CMCam.CMMain:
                CamUpdate(CMMain);
                break;

            case CMCam.CMFinish:
                CamUpdate(CMFinish);
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
    void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnFinish, OnFinish);
        EventManager.AddHandler(GameEvent.OnGenerateLevel, OnGenerateLevel);
    }

    void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnFinish, OnFinish);
        EventManager.RemoveHandler(GameEvent.OnGenerateLevel, OnGenerateLevel);

    }

    private void OnGenerateLevel()
    {
        cMCamEnum = CMCam.CMMain;
       
    }

    private void OnFinish()
    {
        cMCamEnum = CMCam.CMFinish;
    }

}
