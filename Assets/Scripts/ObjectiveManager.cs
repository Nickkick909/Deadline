using System.Collections;
using TMPro;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{

    public GameObject objectiveUI;
    public TMP_Text objectiveText;

    public static ObjectiveManager objectiveManager;

    public float textAnimationTime = 1f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objectiveManager = this;
    }

    public void UpdateObjectiveText(string text, bool animate=true)
    {
        objectiveText.text = text;

        if (animate)
        {
            StartCoroutine(PlayTextAnimation());
        }
    }

    IEnumerator PlayTextAnimation()
    {
        float t = 0;
        while (t < textAnimationTime)
        {
            t += Time.deltaTime;
            gameObject.transform.localScale = Vector3.Lerp(new Vector3(2,2,2), new Vector3(1, 1, 1), t / textAnimationTime);

            objectiveText.color = Color.Lerp(new Color(153, 153, 153), Color.white, t / textAnimationTime);
            //playerCamera.rotation = Quaternion.Slerp(startLocalRot,
            //    targetLocalRot,
            //    t / 0.25f);
            yield return null;
        }


    }
}
