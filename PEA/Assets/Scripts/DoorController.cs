using UnityEngine;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    [SerializeField] Material MDisabledDoor;
    [SerializeField] Material MEnabledDoor;

    [SerializeField] Image RewardImage;

    bool IsEnabled = false;

    public RoomRewardType NextRoomRewardType { get; private set; }

    public void SetRewardForNextRoom(RoomRewardType rewardType)
    {
        NextRoomRewardType = rewardType;

        switch (NextRoomRewardType)
        {
            case RoomRewardType.Money:
                RewardImage.sprite = Resources.Load<Sprite>("Sprites/money-reward-icon");
                break;
            case RoomRewardType.SpecialMoney:
                RewardImage.sprite = Resources.Load<Sprite>("Sprites/special-money-reward-icon");
                break;
            case RoomRewardType.Gear:
                RewardImage.sprite = Resources.Load<Sprite>("Sprites/gear-reward-icon");
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsEnabled && ((1 << other.gameObject.layer) & LayerMask.GetMask("Player")) != 0)
        {
            transform.parent.GetComponent<RoomController>()?.CreateNewRoom(NextRoomRewardType, transform.position * 0.9f);
        }
    }

    public void EnableDoor()
    {
        GetComponent<MeshRenderer>().material = MEnabledDoor;

        RewardImage.enabled = true;
        IsEnabled = true;
    }

    public void DisableDoor()
    {
        GetComponent<MeshRenderer>().material = MDisabledDoor;
        
        RewardImage.enabled = false;
        IsEnabled = false;
    }
}
