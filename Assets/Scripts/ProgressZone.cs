using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressZone : MonoBehaviour
{
    [Header("UI Ayarlarý")]
    public Image progressCircle;   // Çemberin Image bileþeni
    public GameObject circleUI;    // Çemberin UI objesi (aktif/pasif kontrolü)
    public float fillTime = 5f;    // Dolma süresi (saniye)

    [Header("Görseller")]

    public GameObject imageE, imageE2;
    public GameObject imageA, imageA2;
    public GameObject imageS, imageS2;
    public GameObject imageD, imageD2;
    public GameObject imageEnter, imageEnter2;

    private bool playerInside = false;
    private float currentFill = 0f;
    private Coroutine fillCoroutine;
    private string currentTag = "";
    private GameObject currentObject; // Etkileþilen objeyi tutar

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
        currentObject = other.gameObject; // Þu an etkileþilen objeyi sakla
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

        // Ýlgili görseli aktif et
        switch (tag)
        {
            case "E":
                imageE.SetActive(true);
                imageE2.SetActive(true);
                Debug.Log("E tamamlandý ve obje yok edildi!");
                break;
            case "A":
                imageA.SetActive(true);
                imageA2.SetActive(true);
                Debug.Log("A tamamlandý ve obje yok edildi!");
                break;
            case "S":
                imageS.SetActive(true);
                imageS2.SetActive(true);
                Debug.Log("S tamamlandý ve obje yok edildi!");
                break;
            case "D":
                imageD.SetActive(true);
                imageD2.SetActive(true);
                Debug.Log("D tamamlandý ve obje yok edildi!");
                break;
            case "enter":
                imageEnter.SetActive(true);
                imageEnter2.SetActive(true);
                Debug.Log("Enter tamamlandý ve obje yok edildi!");
                break;
        }
    }

    bool IsValidTag(string tag)
    {
        return tag == "E" || tag == "A" || tag == "S" || tag == "D" || tag == "enter";
    }
}
