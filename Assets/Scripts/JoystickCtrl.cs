using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickCtrl : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler

{
    public PlayerCtrl playerCtrl;
    public Transform centerTransform, stickTransform;

    float joystickRadius = 100f;
    float stickToCenter;
    bool isMoving;

    
    Vector3 joystickDir;
    Vector3 targetPosition;

    private void FixedUpdate()
    {
        if (isMoving)
        {
            stickToCenter = Vector3.Distance(targetPosition, centerTransform.position);

            if (stickToCenter < joystickRadius)
            {
                stickTransform.position = centerTransform.position + joystickDir * stickToCenter;
            }
            else
            {
                stickTransform.position = centerTransform.position + joystickDir * joystickRadius;
            }

            playerCtrl.MoveMobile(joystickDir);
        }
        else
        {
            stickTransform.position = Vector3.Lerp(stickTransform.position, centerTransform.position, 0.5f);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isMoving = true;
    }

    
    public void OnDrag(PointerEventData eventData)
    {
        targetPosition = eventData.position;
        joystickDir = (targetPosition - centerTransform.position).normalized;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        playerCtrl.StopMobile();
        isMoving = false;
        joystickDir = Vector3.zero;
    }

    
    
}
