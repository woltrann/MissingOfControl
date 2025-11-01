using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ProgressZone : MonoBehaviour
{
    [Header("UI Ayarlar�")]
    public Image progressCircle;   // �emberin Image bile�eni
    public GameObject circleUI;    // �emberin UI objesi (aktif/pasif kontrol�)
    public float fillTime = 5f;    // Dolma s�resi (saniye)
    public GameObject infoPanel;
    public TextMeshProUGUI infoText;

    [Header("G�rseller")]
    public GameObject imageE, imageE2;
    public GameObject imageA, imageA2;
    public GameObject imageS, imageS2;
    public GameObject imageD, imageD2;
    public GameObject imageEnter, imageEnter2;

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
        infoPanel.SetActive(false);
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
        if (target != null) Destroy(target);

        string collectedText = "";

        switch (tag)
        {
            case "E":
                imageE.SetActive(true);
                imageE2.SetActive(true);
                GameManager.instance.hasE = true;
                collectedText = "E harfi al�nd�! (Can basma aktif)";
                break;
            case "A":
                imageA.SetActive(true);
                imageA2.SetActive(true);
                GameManager.instance.hasA = true;
                collectedText = "A harfi al�nd�! (Sola hareket aktif)";
                break;
            case "S":
                imageS.SetActive(true);
                imageS2.SetActive(true);
                GameManager.instance.hasS = true;
                collectedText = "S harfi al�nd�! (Geri hareket aktif)";
                break;
            case "D":
                imageD.SetActive(true);
                imageD2.SetActive(true);
                GameManager.instance.hasD = true;
                collectedText = "D harfi al�nd�! (Sa�a hareket aktif)";
                break;
            case "enter":
                imageEnter.SetActive(true);
                imageEnter2.SetActive(true);
                collectedText = "Enter tu�u al�nd�!";
                break;
        }

        StartCoroutine(ShowInfo(collectedText));
    }


    IEnumerator ShowInfo(string message)
    {
        infoPanel.SetActive(true);
        infoText.text = message;

        yield return new WaitForSeconds(3f);

        infoPanel.SetActive(false);
    }

    bool IsValidTag(string tag)
    {
        return tag == "E" || tag == "A" || tag == "S" || tag == "D" || tag == "enter";
    }
}
