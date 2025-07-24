using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BGImages : MonoBehaviour
{
    [SerializeField] Image Ground1Image;
    [SerializeField] Image Ground2Image;
    [SerializeField] List<Image> WindImages;
    [SerializeField] List<Image> BgMoveImages;
    [SerializeField] Image BgImages;
    [SerializeField] Image BgMainMenuImages;

    [SerializeField] List<Sprite> BgMoveSprites;
    [SerializeField] List<Sprite> BSprites;
    public RectTransform ground1;
    public RectTransform ground2;
    private float scrollSpeed = 500;

    private float imageWidth;

    public bool RunBool = true;

    MainGameDatasSO mainGameDatasSO;
    void Start()
    {
        ActiveScrollImages(false);
        imageWidth = ground1.rect.width;
        // ground1.anchoredPosition = new Vector2(ground2.anchoredPosition.x + imageWidth, ground1.anchoredPosition.y);
        ground2.anchoredPosition = new Vector2(ground1.anchoredPosition.x + imageWidth, ground2.anchoredPosition.y);

        LevelAreaImages();
    }

    private void LevelAreaImages()
    {
        int lvl = SaveDataService.CurrentLevel;

        mainGameDatasSO = GameDatas.Instance.mainGameDatasSO;
        BSprites = mainGameDatasSO.GroundSprites;
        BgMoveSprites = mainGameDatasSO.MovedGroundSprites;

        int characterIndex = (lvl / 3) % BSprites.Count;

        BgImages.sprite = mainGameDatasSO.GroundSprites[characterIndex];
        BgMainMenuImages.sprite = mainGameDatasSO.MainMenuGroundSprites[characterIndex];


        foreach (var bgImage in BgMoveImages)
        {
            bgImage.sprite = BgMoveSprites[characterIndex];
        }
    }

    public void GetImages(Sprite image1, Sprite image2)
    {
        Ground1Image.sprite = image1;
        Ground2Image.sprite = image2;
    }
    public void ActiveScrollImages(bool active)
    {
        RunBool = active;
        WindImages.ForEach(image => image.DOFade(active ? 1 : 0, .2f).SetEase(Ease.InOutSine));
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
