using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System.Collections.Generic;

public class WeaponMaker : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler{

    public Sprite baseSprite;

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    public EventSystem m_EventSystem;

    public Image img;
    private Texture2D tex;
    private Sprite spr;
    private Rect bound;

    bool draw = false;

    private string SaveLocation = "/Resources/Sprites/PlayerMade/";

    // Use this for initialization
    void Start ()
    {
        tex = new Texture2D(baseSprite.texture.width, baseSprite.texture.height);
        tex.SetPixels32(baseSprite.texture.GetPixels32());
        tex.Apply();
        spr = Sprite.Create(tex, baseSprite.rect, Vector2.zero);
        img.sprite = spr;

        m_Raycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene

    }
    // Update is called once per frame
    void Update ()
    {


        if (draw)
        {

            Vector2 temp = Input.mousePosition;
            Vector2 size = img.rectTransform.rect.size/2;
            Vector2 max = img.rectTransform.position;
            max = max + size;
            Vector2 r = new Vector2(Input.mousePosition.x / max.x, Input.mousePosition.y / max.y);

            r.x = (r.x) * img.sprite.texture.width;
            r.y = (r.y)* img.sprite.texture.height;
            img.sprite.texture.SetPixel((int)(r.x), (int)(r.y), Color.black);
            img.sprite.texture.Apply();
        }
	}

    public void SaveImage(string name)
    {
        Texture2D texture =  baseSprite.texture;
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                Color color = Color.white;
                texture.SetPixel(x, y, color);
            }
        }
        byte[] bytes = baseSprite.texture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + SaveLocation+name+".png", bytes);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        draw = true;
        WeaponRecognition.Instance.ScanTexture(img.sprite.texture, WeaponRecognition.oneHanderLabel);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        draw = false;
    }
}
