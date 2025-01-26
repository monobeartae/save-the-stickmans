using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    public POPUP_ID ID;
    public POPUP_MODE Mode;

    public float PopUpTimer = 0.0f;
    public float FlyInSpeed = 0.2f;
    public float FlyOutSpeed = 0.2f;
    public Vector3 FlyInDirection;
    public Vector3 FlyOutDirection;

    RectTransform parentRect;
    RectTransform popupRect;

    bool to_clear = false;
    float timer;
    Vector3 targetPos;
    POPUP_ANIM_STATE state = POPUP_ANIM_STATE.FLYIN;


    void OnEnable()
    {
        parentRect = transform.parent.parent.gameObject.GetComponent<RectTransform>();
        popupRect = GetComponent<RectTransform>();
        transform.localPosition = new Vector3(0.0f - (FlyInDirection.x * (parentRect.rect.width + popupRect.rect.width) * 0.5f), 0.0f - (FlyInDirection.y * (parentRect.rect.height + popupRect.rect.height) * 0.5f), 0);

        FlyInDirection.Normalize();
        FlyOutDirection.Normalize();


        state = POPUP_ANIM_STATE.FLYIN;
        targetPos = new Vector3(0.0f, 0.0f, 0.0f);
        to_clear = false;
        timer = PopUpTimer;

    }


    void Update()
    {

        switch (state)
        {
            case POPUP_ANIM_STATE.FLYIN:
                transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, FlyInSpeed);
                if (Vector3.Distance(transform.localPosition, targetPos) < 1.0f)
                {
                    transform.localPosition = targetPos;
                    state = POPUP_ANIM_STATE.WAIT;
                }
                break;
            case POPUP_ANIM_STATE.WAIT:
                if (CheckForInput())
                {
                    targetPos = new Vector3((parentRect.rect.width + popupRect.rect.width) * 0.5f, (parentRect.rect.height + popupRect.rect.height) * 0.5f, 0);
                    if (FlyOutDirection.x == 0)
                        targetPos.x = 0;
                    if (FlyOutDirection.y == 0)
                        targetPos.y = 0;
                    state = POPUP_ANIM_STATE.FLYOUT;
                }
                break;
            case POPUP_ANIM_STATE.FLYOUT:
                transform.localPosition = InverseLerp(transform.localPosition, targetPos, FlyOutSpeed);
                if (Vector3.Distance(transform.localPosition, targetPos) < 1.0f)
                {
                    transform.localPosition = targetPos;
                    state = POPUP_ANIM_STATE.COMPLETE;
                }
                break;
            default:
                break;
        }
    }

    bool CheckForInput()
    {
        if (to_clear)
            return true;

        switch (Mode)
        {
            case POPUP_MODE.MOUSECLICK:
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    GameHandler.instance.sfxScript.PlayPickupItem();
                    return true;
                }
                return false;
            case POPUP_MODE.TIMER:
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    GameHandler.instance.sfxScript.PlayPickupItem();
                    return true;
                }
                else if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    GameHandler.instance.sfxScript.PlayPickupItem();
                    return true;
                }
                return false;
            default:
                return false;


        }

    }

    public POPUP_ANIM_STATE GetState()
    {
        return state;
    }

    public void Deactivate()
    {
        to_clear = true;
    }

    private Vector3 InverseLerp(Vector3 a, Vector3 b, float t)
    {
        return a + (b - a) * t;
    }

}

public enum POPUP_ID
{
    DIALOGUE_INTRO,
    MOVEMENT_INTRO,
    INTERACTIONS_INTRO,
    WATER_INTRO,

    UNLOCK_CHECKPOINT,
    SAVE_NPC,
    PASS_WEAPON,

    DIALOGUE_NPC,
    INVEN_INTRO,
    NPC_WEAPON_INTRO,

    MERCHANT_NPC,

    PORTAL,
    ITEM,

    NUM_TOTAL

}

public enum POPUP_ANIM_STATE
{
    FLYIN,
    WAIT,
    FLYOUT,
    COMPLETE,

    NUM_TOTAL
}

public enum POPUP_MODE
{
    CUSTOM,
    MOUSECLICK,
    TIMER,

    NUM_TOTAL
}
