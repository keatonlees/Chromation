using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour {
    
    public Image image;  // whatever loading image
    public AnimationCurve curve;  // for control over the curve
    float fadeSpeed = 2f;  // higher number for faster, lower number for slower fade

    void Start() {
        StartCoroutine(FadeIn());
    }

    public void FadeTo(string scene) {
        StartCoroutine(FadeOut(scene));
    }

    IEnumerator FadeIn() {
        float t = 1f;
        while (t > 0f) {
            t -= Time.deltaTime * fadeSpeed;
            float a = curve.Evaluate(t);
            image.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }
    }

    IEnumerator FadeOut(string scene) {
        float t = 0f;
        while (t < 1f) {
            t += Time.deltaTime * fadeSpeed;
            float a = curve.Evaluate(t);
            image.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }
        SceneManager.LoadScene(scene);
    }

}
