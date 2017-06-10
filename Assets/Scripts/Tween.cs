using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tween from one value to another
/// </summary>
public class Tween: MonoBehaviour {

    public delegate void NewValueCallback(float newValue);
    public delegate void TweenCompletionCallback();

    private bool isTweening;

    private float elapsedTweenTime;
    private float tweenDuration;

    private float startValue;
    private float targetValue;
    private float tweenRate {
        get {
            return (targetValue - startValue) / tweenDuration;
        }
    }

    private NewValueCallback setValueBeingTweened;
    private TweenCompletionCallback onTweenComplete;

    private void Update() {
        if(isTweening) {
            elapsedTweenTime += Time.deltaTime;
            float totalDistance = tweenRate * elapsedTweenTime;

            setValueBeingTweened(startValue + totalDistance);

            if(elapsedTweenTime >= tweenDuration)
                finishTween();
        }
    }

    private void finishTween() {
        isTweening = false;
        onTweenComplete();
    }

    public void startTween(float startVal, float targetVal, float tweenDurationSeconds, NewValueCallback setValue, TweenCompletionCallback tweenCompleteCallback) {

        isTweening = true;
        tweenDuration = tweenDurationSeconds;
        elapsedTweenTime = 0;
        startValue = startVal;
        targetValue = targetVal;
        setValueBeingTweened = setValue;
        onTweenComplete = tweenCompleteCallback;
    }
}
