using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracingPanelUI : MonoBehaviour
{
    public static TracingPanelUI o;

    [SerializeField] Vector2 cameraOffset = new Vector2(0, .7f);
    [SerializeField] float cameraZoom = 4;
    [SerializeField] FlowList<PlayPatternButton> playPatternButtons = new FlowList<PlayPatternButton>();
    [SerializeField] Transform indicatingArrow;

    GameObject panelGO => transform.GetChild(0).gameObject;


    bool isActive;

    private void Awake()
    {
        o = this;
    }
    public void show()
    {
        panelGO.SetActive(true);
        playPatternButtons.iterate(playPatternButtons.count, x => x.gameObject.SetActive(false));
        playPatternButtons.iterate(TracingPhase.o.playTracingOptionsCount, x =>
        {
            x.gameObject.SetActive(true);
            x.component.init(x.iterationIndex);
        });
        CameraControl.o.move(TracingPhase.o.argLetter.transform.position + cameraOffset.toVector3());
        CameraControl.o.zoom(cameraZoom);
        refresh();
        hideIndicating();
    }



    public void refresh()
    {
        playPatternButtons.iterate(playPatternButtons.count, x => x.component.refresh());
    }


    public void hide()
    {
        panelGO.SetActive(false);
    }


    public void setIndicatingPosition(Vector2 position, Vector2? direction = default)
    {
        indicatingArrow.transform.DOMove(position, .5f);
        if (!direction.HasValue)
            direction = Vector2.down;
        indicatingArrow.transform.up = -direction.Value;
    }
    public void showIndicating()
    {
        if (!indicatingArrow.gameObject.activeSelf)
        {
            indicatingArrow.gameObject.SetActive(true);
            indicatingArrow.DOScale(1, .25f);
        }
    }
    public void hideIndicating()
    {
        indicatingArrow.DOScale(0, .25f).OnComplete(() =>
        {
            indicatingArrow.gameObject.SetActive(false);
        });

    }


}
