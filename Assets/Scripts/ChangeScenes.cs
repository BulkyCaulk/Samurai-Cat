using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    [Header("Scene & Exit-Point")]
    public string SceneToChange;
    [Tooltip("Name of the empty GameObject in the next scene")]
    public string exitPointName;

    [Header("Transition")]
    public Animator transition;
    public float transitionTime = 1f;

    [Header("Transition Info Prefab")]
    public GameObject transitionInfoPrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Spawn carrier, set its field, persist it
        var info = Instantiate(transitionInfoPrefab);
        var ti   = info.GetComponent<TransitionInfo>();
        ti.entryPointName = exitPointName;
        DontDestroyOnLoad(info);

        StartCoroutine(DoTransition());
    }

    private IEnumerator DoTransition()
    {
        if (transition != null)
            transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(SceneToChange);
    }
}
