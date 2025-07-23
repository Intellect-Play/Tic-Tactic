using UnityEngine;
using UnityEngine.UI;

public class HorizontalInfiniteScroll : MonoBehaviour
{
    [SerializeField] Image Ground1Image;
    [SerializeField] Image Ground2Image;
    public RectTransform ground1;
    public RectTransform ground2;
    private float scrollSpeed = 500;

    private float imageWidth;

    public bool RunBool = true;
    void Start()
    {
        RunBool = false;
        imageWidth = ground1.rect.width;
       // ground1.anchoredPosition = new Vector2(ground2.anchoredPosition.x + imageWidth, ground1.anchoredPosition.y);
        ground2.anchoredPosition = new Vector2(ground1.anchoredPosition.x + imageWidth, ground2.anchoredPosition.y);       
    }

    public void GetImages(Sprite image1, Sprite image2)
    {
        Ground1Image.sprite = image1;
        Ground2Image.sprite = image2;
    }
    void Update()
    {
        if(!RunBool) return;
        float movement = scrollSpeed * Time.deltaTime;

        // İmage-ləri sola hərəkət etdir
        ground1.anchoredPosition -= new Vector2(movement, 0);
        ground2.anchoredPosition -= new Vector2(movement, 0);

        // Əgər image ekranın tam soluna çıxıbsa, onu digərin arxasına qoy
        if (ground1.anchoredPosition.x <= -imageWidth)
        {
            ground1.anchoredPosition = new Vector2(ground2.anchoredPosition.x + imageWidth, ground1.anchoredPosition.y);
        }

        if (ground2.anchoredPosition.x <= -imageWidth)
        {
            ground2.anchoredPosition = new Vector2(ground1.anchoredPosition.x + imageWidth, ground2.anchoredPosition.y);
        }
    }
}
