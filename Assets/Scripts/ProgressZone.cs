using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressZone : MonoBehaviour
{
    [Header("UI Ayarlar�")]
    public Image progressCircle;   // �emberin Image bile�eni
    public GameObject circleUI;    // �emberin UI objesi (aktif/pasif kontrol�)
    public float fillTime = 5f;    // Dolma s�resi (saniye)

    [Header("G�rseller")]
    public GameObject imageE;
    public GameObject imageA;
    public GameObject imageS;
    public GameObject imageD;
    public GameObject imageEnter;

    private bool playerInside = false;
    private float currentFill = 0f;
    private Coroutine fillCoroutine;
    private string currentTag = "";
    private GameObject currentObject; // Etkile�ilen objeyi tutar

    void Start()
    {
        progressCircle.fillAmount = 0f;
        circleUI.SetActive(false);
        imageE.SetActive(false);
        imageA.SetActive(false);
        imageS.SetActive(false);
        imageD.SetActive(false);
        imageEnter.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!IsValidTag(other.tag)) return;

        currentTag = other.tag;
        currentObject = other.gameObject; // �u an etkile�ilen objeyi sakla
        playerInside = true;
        circleUI.SetActive(true);

        if (fillCoroutine != null) StopCoroutine(fillCoroutine);
        fillCoroutine = StartCoroutine(FillCircle());
    }

    void OnTriggerExit(Collider other)
    {
        if (!IsValidTag(other.tag)) return;

        playerInside = false;

        if (fillCoroutine != null) StopCoroutine(fillCoroutine);
        fillCoroutine = StartCoroutine(EmptyCircle());
    }

    IEnumerator FillCircle()
    {
        while (playerInside && currentFill < 1f)
        {
            currentFill += Time.deltaTime / fillTime;
            progressCircle.fillAmount = currentFill;

            if (currentFill >= 1f)
            {
                OnFillComplete(currentTag, currentObject);
                yield break;
            }

            yield return null;
        }
    }

    IEnumerator EmptyCircle()
    {
        while (!playerInside && currentFill > 0f)
        {
            currentFill -= Time.deltaTime / fillTime;
            progressCircle.fillAmount = currentFill;
            yield return null;
        }

        if (currentFill <= 0f)
        {
            circleUI.SetActive(false);
        }
    }

    void OnFillComplete(string tag, GameObject target)
    {
        circleUI.SetActive(false);

        // Objeyi yok et
        if (target != null)
        {
            Destroy(target);
        }

        // �lgili g�rseli aktif et
        switch (tag)
        {
            case "E":
                imageE.SetActive(true);
                Debug.Log("E tamamland� ve obje yok edildi!");
                break;
            case "A":
                imageA.SetActive(true);
                Debug.Log("A tamamland� ve obje yok edildi!");
                break;
            case "S":
                imageS.SetActive(true);
                Debug.Log("S tamamland� ve obje yok edildi!");
                break;
            case "D":
                imageD.SetActive(true);
                Debug.Log("D tamamland� ve obje yok edildi!");
                break;
            case "enter":
                imageEnter.SetActive(true);
                Debug.Log("Enter tamamland� ve obje yok edildi!");
                break;
        }
    }

    bool IsValidTag(string tag)
    {
        return tag == "E" || tag == "A" || tag == "S" || tag == "D" || tag == "enter";
    }
}
