using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSetActive : MonoBehaviour
{
    public bool IsOpen = false;
    [SerializeField]
    private float Speed = 1f;
    [Header("Sliding Configs")]
    [SerializeField]
    private Vector2 SlideDirection = Vector2.down;
    [SerializeField]
    private float SlideAmount = 1.9f;

    private Vector2 StartPosition;

    private Coroutine AnimationCoroutine;

    private void Awake()
    {
        StartPosition = transform.position;
    }

    public void OpenDoor()
    {
        if (!IsOpen)
        {
            if(AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }

            AnimationCoroutine = StartCoroutine(DoSlidingOpen());
            //gameObject.SetActive(false);
        }
    }

    private IEnumerator DoSlidingOpen()
    {
        Vector2 endPosition = StartPosition + SlideAmount * SlideDirection;
        Vector2 startPosition = transform.position;

        float time = 0;
        IsOpen = true;
        while(time < 1)
        {
            transform.position = Vector2.Lerp(startPosition, endPosition, time);
            yield return null;
            time += Time.deltaTime * Speed;
        }
    }

    public void CloseDoor()
    {
        if (IsOpen)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }

            AnimationCoroutine = StartCoroutine(DoSlidingClose());
            //gameObject.SetActive(true);
        }
    }

    private IEnumerator DoSlidingClose()
    {
        Vector2 endPosition = StartPosition;
        Vector2 startPosition = transform.position;

        float time = 0;
        IsOpen = false;
        while (time < 1)
        {
            transform.position = Vector2.Lerp(startPosition, endPosition, time);
            yield return null;
            time += Time.deltaTime * Speed;
        }

    }
}
