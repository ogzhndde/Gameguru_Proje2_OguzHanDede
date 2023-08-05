using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//DEFINE ALL CAMERA ENUMS
public enum CMCam
{
    CMMain,
    CMFinish,
    CMFail
}

public class CameraManager : InstanceManager<CameraManager>
{
    public CMCam cMCamEnum;
    public GameObject CMMain, CMFinish, CMFail;
    public List<GameObject> CamList = new List<GameObject>();

    void Start()
    {
        //ADD ALL CINEMACHINES IN THE LIST
        SetCams();

        //CHECK CURRENT CAMERA ALL TIME
        InvokeRepeating("CamControl", .1f, .1f);
    }

    void SetCams()
    {
        CamList.Add(CMMain);
        CamList.Add(CMFinish);
        CamList.Add(CMFail);
    }

    public void CamControl()
    {
        //CHECK CURRENT CAMERA WITH ENUM VALUE
        GameObject CurrentCam;

        CurrentCam = cMCamEnum switch
        {
            CMCam.CMMain => CMMain,
            CMCam.CMFinish => CMFinish,
            CMCam.CMFail => CMFail,
            _ => CMMain
        };

        CamUpdate(CurrentCam);
    }

    public void CamUpdate(GameObject activeCam)
    {
        //ACTIVATE CURRENT CAM, DEACTIVATE ALL OTHERS CAM
        for (int i = 0; i < CamList.Count; i++)
        {
            if (CamList[i] != activeCam)
                CamList[i].SetActive(false);

            if (CamList[i] == activeCam)
                CamList[i].SetActive(true);

        }
    }

    //########################################    EVENTS    ###################################################################
    void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnFinish, OnFinish);
        EventManager.AddHandler(GameEvent.OnFail, OnFail);
        EventManager.AddHandler(GameEvent.OnGenerateLevel, OnGenerateLevel);
    }

    void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnFinish, OnFinish);
        EventManager.RemoveHandler(GameEvent.OnFail, OnFail);
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

    private void OnFail()
    {
        cMCamEnum = CMCam.CMFail;

        //LEAVE THE CAMERA TRACKING ON FAIL SITUATION
        var camZOffset = 12f;
        CMFail.transform.position = new Vector3(CMFail.transform.position.x, CMFail.transform.position.y, PlayerManager.Instance.transform.position.z - camZOffset);
        CMFail.GetComponent<CinemachineVirtualCamera>().Follow = null;
    }

}
