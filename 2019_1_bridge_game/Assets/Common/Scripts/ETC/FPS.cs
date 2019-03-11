using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class FPS : MonoBehaviour
{
	private Text text;
	private float fps = 60;

	void Awake()
	{
		text = GetComponent<Text>();
		StartCoroutine(ForFPS());
	}

	IEnumerator ForFPS()
	{
		while (true)
		{
			yield return new WaitForSecondsRealtime(0.5f);
			float interp = Time.deltaTime / (0.5f + Time.deltaTime);
			float currentFPS = 1.0f / Time.deltaTime;
			fps = Mathf.Lerp(fps, currentFPS, interp);
			text.text = Mathf.RoundToInt(fps) + "fps"; 
		}
	}
	void OnDestroy()
	{
		StopCoroutine(ForFPS());
	}
}
