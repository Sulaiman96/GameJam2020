using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
   public IEnumerator Shake(float duration, float magnitude)
   {
      Vector3 originalPosition = transform.localPosition;
      float timeElapsed = 0.0f;
      while (timeElapsed < duration)
      {
         float x = Random.Range(-1, 1f) * magnitude;
         float y = Random.Range(-1, 1f) * magnitude;
         
         transform.localPosition = new Vector3(x,y,originalPosition.z);

         timeElapsed += Time.deltaTime; //update timeElapsed in each frame.
         yield return null; //wait until the frame finishes before going on to the next iteration of while loop.
      }

      transform.localPosition = originalPosition;
   }
}
