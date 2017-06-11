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
            elapsedTweenTime = Mathf.Clamp(elapsedTweenTime + Time.deltaTime, 0, tweenDuration);
            float totalDistance = tweenRate * elapsedTweenTime;

            setValueBeingTweened(startValue + totalDistance);

            if(elapsedTweenTime >= tweenDuration)
                finishTween();
        }
    }

    private void finishTween() {
        isTweening = false;

        if(onTweenComplete != null)
            onTweenComplete();
    }

    /// <summary>
    /// Begin tweening from the given value to the target value in the given duration.
    /// the setvalue function is used to update the variable every loop. This is to allow updating something like transform.position.y since you can't update the y position directly
    /// the callback function is executed when the tweening has finished.
    /// </summary>
    /// <param name="startVal">value to start the tween from</param>
    /// <param name="targetVal">value to tween to</param>
    /// <param name="tweenDurationSeconds">how long the tween should last</param>
    /// <param name="setValue">function used to update the variable being tweened</param>
    /// <param name="tweenCompleteCallback">callback to execute when the tween is finished</param>
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
