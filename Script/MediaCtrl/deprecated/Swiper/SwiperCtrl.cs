using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwiperCtrl : MonoBehaviour, IEndDragHandler, IBeginDragHandler
{
    public ScrollRect SwiperScrollRect;
    public MQTT Mqtt;
    public float Duration = 1f;

    public float TimeToStartDrag = 0.3f;

    private readonly List<SwiperItem> swiperItems = new();
    private SwiperItem currentItem;

    private float beginDragTime;
    private bool isDragging = false;
    private Vector2 startDragPosition;

    void Start()
    {
        Application.targetFrameRate = 280;
        swiperItems.AddRange(SwiperScrollRect.content.GetComponentsInChildren<SwiperItem>().ToList());
        swiperItems.Sort((x, y) => x.Pos.CompareTo(y.Pos));
        DragItem();
    }

    private IEnumerator MoveHorizontalNormalized(float targetNormalized)
    {
        SwiperScrollRect.horizontal = false;
        float elapsedTime = 0f;
        while (elapsedTime < Duration)
        {
            elapsedTime += Time.deltaTime;
            float nextNormalized = Mathf.SmoothStep(SwiperScrollRect.horizontalNormalizedPosition, targetNormalized, elapsedTime / Duration);
            SwiperScrollRect.horizontalNormalizedPosition = nextNormalized;
            yield return null;
        }

        SwiperScrollRect.horizontalNormalizedPosition = targetNormalized;
    }

    private void SelectNearestItem(PointerEventData eventData)
    {
        float endDragTime = Time.time;

        if (endDragTime - beginDragTime < TimeToStartDrag)
        {
            Vector2 endDragPosition = eventData.position;
            float horizontalDelta = endDragPosition.x - startDragPosition.x;
            int swipeStep = horizontalDelta < 0 ? 1 : -1;
            SwipeItem(swipeStep);
        }
        else
        {
            SwiperScrollRect.horizontal = true;
            DragItem();
        }
    }

    public void DragItem()
    {
        if (!SwiperScrollRect.horizontal) return;
        SwiperItem selectedItem = swiperItems.OrderBy(x => Math.Abs(x.Pos - SwiperScrollRect.horizontalNormalizedPosition)).First();
        StartCoroutine(MoveHorizontalNormalized(selectedItem.Pos));
        currentItem?.SelectItem(false);
        selectedItem.SelectItem(true);
        currentItem = selectedItem;
    }

    public void SwipeItem(int step)
    {
        int nextIndex = currentItem == null ? 1 : (swiperItems.IndexOf(currentItem) + step);
        nextIndex = nextIndex >= swiperItems.Count ? swiperItems.Count - 1 : nextIndex;
        nextIndex = nextIndex < 0 ? 0 : nextIndex;


        SwiperItem nextItem = swiperItems[nextIndex];
        StartCoroutine(MoveHorizontalNormalized(nextItem.Pos));

        currentItem?.SelectItem(false);
        nextItem.SelectItem(true);

        SwiperScrollRect.horizontal = false;
        currentItem = nextItem;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startDragPosition = eventData.position;
        isDragging = true;
        beginDragTime = Time.time;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        isDragging = false;
        SelectNearestItem(eventData);
        Mqtt.Publish("getVideoState", "");
    }

    private void Update()
    {
        //0.1831065
        //0.2268832
        //0.2706598
        //0.3144364
        //0.3580079
        //0.4011694
        //0.4450486
        //0.4886201
        //0.5321917
        //0.5753532
        //0.6192323
        //0.6623938
        //0.7062729
        //0.7498445
        //0.7937236
        //0.8368851
        //
        //Debug.Log(SwiperScrollRect.horizontalNormalizedPosition);
        if (isDragging
            && Time.time - beginDragTime > TimeToStartDrag)
        {
            SwiperScrollRect.horizontal = true;
        }
    }
}
