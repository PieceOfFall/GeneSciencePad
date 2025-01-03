using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateVideo : MonoBehaviour
{
    public string TargetName;

    public Image UnloopImg;
    public Image LoopImg;

    public Sprite ActivePattrenSprite;
    public Sprite InActivePattrenSprite;

    public Image MuteImg;

    public Color MuteColor;
    public Color UnMuteColor;

    public MQTTPublisher getStatePublisher;
    
    public List<Image> operationImages;
    public Sprite defaultOperationSprite;
    public Sprite BannedOperationSprite;
   
    public void SetMute(bool isMute)
    {
        MuteImg.color = isMute ? MuteColor : UnMuteColor;
    }

    public void SetLoop(bool isLoop)
    {
        UnloopImg.sprite = isLoop ? InActivePattrenSprite : ActivePattrenSprite;
        LoopImg.sprite = isLoop ? ActivePattrenSprite : InActivePattrenSprite;

        Sprite targetSprite = isLoop ? BannedOperationSprite : defaultOperationSprite;
        bool shouldBanBtn = isLoop ? true : false;
        foreach (var operationImg in operationImages)
        {
            operationImg.sprite = targetSprite;
            operationImg.GetComponent<Button>().enabled = !shouldBanBtn;
        }
    }

    public void SetState(State state)
    {
        if (TargetName != state.Target) return;
        SetMute(state.IsMute);
        SetLoop(state.IsLoop);
    }

    public void OnEnable()
    {
        getStatePublisher.Publish("");
    }
}
