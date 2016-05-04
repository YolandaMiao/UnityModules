﻿using UnityEngine;
using System.Collections;
using Leap;
using Leap.Unity;

namespace Leap.Unity.DetectionUtilities {

  public class FingerPointingDetector : BinaryDetector {
    public IHandModel handModel = null;
  
    public Finger.FingerType FingerName = Finger.FingerType.TYPE_INDEX;
    public Directions PointingDirection = Directions.Forward;
    public Vector3 CustomDirection = Vector3.forward;
    public float OnAngle = 45f; //degrees
    public float OffAngle = 65f; //degrees
  
    void Start () {
      if(handModel == null){
        handModel = gameObject.GetComponentInParent<IHandModel>();
      }
    }

    void OnEnable () {
      StartCoroutine(fingerPointingWatcher());
    }
  
    void OnDisable () {
      StopCoroutine(fingerPointingWatcher());
      Deactivate();
    }
  
    IEnumerator fingerPointingWatcher() {
      Hand hand;
      Vector3 fingerDirection;
      Vector3 targetDirection = selectedDirection();
      int selectedFinger = selectedFingerOrdinal();
      while(true){
        if(handModel != null){
          hand = handModel.GetLeapHand();
          if(hand != null){
            fingerDirection = hand.Fingers[selectedFinger].Direction.ToVector3();
            float angleTo = Vector3.Angle(fingerDirection, targetDirection);
            if(handModel.IsTracked && angleTo <= OnAngle){
              Activate();
            } else if (!handModel.IsTracked || angleTo >= OffAngle) {
              Deactivate();
            }
          }
        }
        yield return new WaitForSeconds(Period);
      }
    }
  
    private Vector3 selectedDirection(){
      switch(PointingDirection){
        case Directions.Backward:
          return -Camera.main.transform.forward;
        case Directions.Down:
          return -Camera.main.transform.up;
        case Directions.Forward:
          return Camera.main.transform.forward;
        case Directions.Left:
          return -Camera.main.transform.right;
        case Directions.Right:
          return Camera.main.transform.right;
        case Directions.Up:
          return Camera.main.transform.up;
        case Directions.CustomVector:
          return CustomDirection;
        default:
          return Camera.main.transform.forward;
      }
    }
  
    private int selectedFingerOrdinal(){
      switch(FingerName){
        case Finger.FingerType.TYPE_INDEX:
          return 1;
        case Finger.FingerType.TYPE_MIDDLE:
          return 2;
        case Finger.FingerType.TYPE_PINKY:
          return 4;
        case Finger.FingerType.TYPE_RING:
          return 3;
        case Finger.FingerType.TYPE_THUMB:
          return 0;
        default:
          return 1;
      }
    }
  }
  
}