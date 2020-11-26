using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowimagePreview : MonoBehaviour
{
    public GameObject imagePanel;
    public Transform canvas;
    private bool Pointed, HelpingBool = true;
    private RaycastHit2D hit;
    private Vector2 mousepos;
    private GameObject gb;
    [HideInInspector] public Texture PreviewTexture;
    // Start is called before the first frame update
    void Start()
    {
        HelpingBool = true;
    }

    private void OnEnable()
    {
        HelpingBool = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 screenpt = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousepos = new Vector2(screenpt.x, screenpt.y);
            hit = Physics2D.Raycast(mousepos, Vector2.zero);
            if (hit != null && hit.collider != null && hit.collider.gameObject.name == this.gameObject.name && HelpingBool)
            {
                HelpingBool = false;
                StartCoroutine(LargetView());
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (hit != null && hit.collider != null && hit.collider.gameObject.name == this.gameObject.name && !HelpingBool)
            {
                HelpingBool = true;
                StartCoroutine(SmallView());
            }
        }
    }


    IEnumerator LargetView()
    {
        gb = Instantiate(imagePanel, canvas, false);
        gb.transform.GetChild(0).gameObject.GetComponent<RawImage>().texture = PreviewTexture;
        gb.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(PreviewTexture.width, PreviewTexture.height);
        gb.SetActive(true);
        iTween.ScaleTo(gb, Vector3.one, 0.3f);
        yield return new WaitForSeconds(0.3f);
    }

    IEnumerator SmallView()
    {
        iTween.ScaleTo(gb, Vector3.zero, 0.3f);
        yield return new WaitForSeconds(0.3f);
        gb.SetActive(false);
        Destroy(gb);
    }
}
