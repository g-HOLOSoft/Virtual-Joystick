using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {
    RectTransform joystick;
    RectTransform lever;

    Transform player;
    Animator animator;

    float fRadius;
    float fSpeed = 2.0f;

    Vector3 vecMove;

    Vector2 vecNormal;

    bool bTouch = false;


    void Start() {
        joystick = transform.Find("Joystick").GetComponent<RectTransform>();
        lever = transform.Find("Joystick/Lever").GetComponent<RectTransform>();

        player = GameObject.Find("Player").transform;
        animator = GameObject.Find("Player").GetComponent<Animator>();

        fRadius = joystick.rect.width * 0.5f;
    }

    void Update() {
        animator.SetBool("isMoving", bTouch);

        if(bTouch) {
            player.position += vecMove;
        }
    }

    void OnTouch(Vector2 vecTouch) {
        Vector2 vec = new Vector2(vecTouch.x - joystick.position.x, vecTouch.y - joystick.position.y);

        vec = Vector2.ClampMagnitude(vec, fRadius);
        lever.localPosition = vec;

        float fSqr = (joystick.position - lever.position).sqrMagnitude / (fRadius * fRadius);

        Vector2 vecNormal = vec.normalized;

        vecMove = new Vector3(vecNormal.x * fSpeed * Time.deltaTime * fSqr, 0f, vecNormal.y * fSpeed * Time.deltaTime * fSqr);
        player.eulerAngles = new Vector3(0f, Mathf.Atan2(vecNormal.x, vecNormal.y) * Mathf.Rad2Deg, 0f);
    }

    public void OnDrag(PointerEventData eventData) {
        OnTouch(eventData.position);
        bTouch = true;
    }

    public void OnPointerDown(PointerEventData eventData) {
        OnTouch(eventData.position);
        bTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        lever.localPosition = Vector2.zero;
        bTouch = false;
    }
}
