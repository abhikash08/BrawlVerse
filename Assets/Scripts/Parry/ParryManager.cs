using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParryManager : MonoBehaviour
{
    public enum Stance {Parry, Block, Open};

    private Stance currentStance;

    private bool canParry = false;
    private float parryTime = 0.5f;

    public static Action OpenParryWindow;
    public static Action CloseParryWindow;

    [SerializeField] private Animator anim;

    private void Awake() {
        OpenParryWindow += ParryWindowOpened;
        CloseParryWindow += ParryWindowClosed;
        currentStance = Stance.Open;
    }
    private void Update() {
        if (Input.GetMouseButtonDown(1)) {
            if (canParry && currentStance == Stance.Open) {
                StartCoroutine(Parrying());
            }
            else {
                currentStance = Stance.Block;
            }
        }
        if (Input.GetMouseButtonUp(1) && currentStance != Stance.Parry) {
            currentStance = Stance.Open;
        }
        //Debug.Log(canParry);
        Debug.Log(currentStance);
        anim.SetBool("defended",currentStance != Stance.Open);
    }

    private IEnumerator Parrying() {
        currentStance = Stance.Parry;
        yield return new WaitForSeconds(parryTime);
        currentStance = Input.GetMouseButton(1)? Stance.Block : Stance.Open;
    }
    public Stance GetCurrentStance() {
        return currentStance;
    }
    private void ParryWindowOpened() {
        canParry = true;
    }
    private void ParryWindowClosed() {
        canParry = false;
    }
}
