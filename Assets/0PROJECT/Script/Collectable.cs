using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    CollectableType collectableTypeEnum;

    [SerializeField] private Transform TargetPlatform;
    [SerializeField] private Transform Player;

    [SerializeField] private GameObject CollectableParticle;
    [SerializeField] private int CollValue;


    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        if (Player == null) return;
        if (TargetPlatform == null) TargetPlatform = Player;

        //IF PLATFORM IS STILL MOVING, FOLLOW PLATFORM; ELSE FOLLOW PLAYER X POSITION
        float xPos = Mathf.Lerp(transform.position.x, TargetPlatform.position.x, 2f * Time.deltaTime);
        float yPos = transform.position.y;
        float zPos = transform.position.z;

        transform.position = new Vector3(xPos, yPos, zPos);
    }

    public void SetTarget(Transform target)
    {
        TargetPlatform = target;
    }

    public void Collect()
    {
        Instantiate(CollectableParticle, transform.position, Quaternion.identity);
        GameManager.Instance.data.TotalCoin += CollValue;

        //PLAY SOUND DEPENDS ON COLLECTABLE TYPE
        string sound;
        sound = collectableTypeEnum switch{ CollectableType.Coin => "SoundCoin", CollectableType.Diamond => "SoundDiamond", CollectableType.Star => "SoundStar", _=> ""};
        PlaySound(sound);

        EventManager.Broadcast(GameEvent.OnCollectCoin);
        
        Destroy(gameObject);
    }

    void PlaySound(string sound)
    {
        EventManager.Broadcast(GameEvent.OnPlaySound, sound);
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                Collect();
                break;
        }
    }
}
