using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System.Collections.Generic;

public class WeaponMaker : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler{

    [System.Serializable]
    public enum Brush
    {
        Square,
        Circle,
        Rings
    }
    public Sprite baseSprite;

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    public EventSystem m_EventSystem;
    public Canvas canv;
    public Image image;
    public InputField nameField;
    private Texture2D currentTex;
    private Texture2D tempTex;
    private Sprite spr;
    private Rect bound;

    private EssenceItem cEssence;
    public EssenceItem currentEssence
    {
        set
        {
            cEssence = value;
            UpdateEssence();
        }
        get
        {
            if (cEssence == null && AssetManager.Instance != null)
            {
                cEssence = AssetManager.Instance.GetEssenceOfType(AffType.dark);
                UpdateEssence();
            }
            return cEssence;
        }
    }

    public Text numberOfEssence;

    public float AlphaCorrection = 0.3f;

    public Brush brush;
    public int b_width = 2;
    public int b_height = 2;
    public int b_radius = 2;

    bool draw = false;

    Vector2 oldPos;
    // Use this for initialization
    void Start ()
    {

        ResetSprite();

        m_Raycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
    }

    private void OnEnable()
    {
        ResetSprite();
        if (WeaponRecognition.Instance != null)
        {
            WeaponRecognition.Instance.LoadWeapon(image.sprite, "1h");
        }
        UpdateEssence();
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        if (draw)
        {
            Vector2 loc = GetLocationOnSprite();
            int x = (int)loc.x;
            int y = (int)loc.y;


            if (!IsBrushOverflow(x, y, b_width, b_height))
            {
                switch (brush)
                {
                    case Brush.Square:
                        {
                            SquareBrushDraw(x, y);
                            break;
                        }
                    case Brush.Rings:
                        {
                           RingsBrushDraw(x, y);
                            break;
                        }
                    case Brush.Circle:
                        {
                            CircleDraw(x, y);
                            break;
                        }
                }
            }
            image.sprite.texture.Apply();
            loc = oldPos;
        }
	}

    public void SaveImage()
    {
        MessageManager.Instance.NewQuestion(Save, null, "Are you sure you have finished?");
    }

    public void Save()
    {
        if (nameField.text.Length <= 2)
        {
            MessageManager.Instance.NewMessage("Come on, you need a longer name that that...");
            return;
        }
        WeaponData wd = new WeaponData(nameField.text, null, nameField.text, 1, new AffinityData() );

        SaveLoadHanlder.SaveWeapon(image.sprite.texture, nameField.text, wd);

        SaveLoadHanlder.LoadSprite(nameField.text);

        WeaponManager.Instance.AddWeapon(wd, WeaponSortType.playerMade);

        MessageManager.Instance.NewMessage("You have made a weapon... Good for you.");

        UIManager.Instance.ResetCanvas();
        //To Do: Generate weapon stuffs.
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        draw = true;
        tempTex = CloneTexture(currentTex);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        draw = false;
        if (!WeaponRecognition.Instance.CheckTexture(image.sprite.texture))
        {
            image.sprite.texture.SetPixels(tempTex.GetPixels());
            image.sprite.texture.Apply();
        };
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (draw)
        {
            draw = false;
            if (!WeaponRecognition.Instance.CheckTexture(image.sprite.texture))
            {
                image.sprite.texture.SetPixels(tempTex.GetPixels());
                image.sprite.texture.Apply();
            };
        }
    }

    public void SquareBrushDraw(int x, int y)
    {
        for (int i = 0; i < b_width; i++)
        {
            for (int j = 0; j < b_height; j++)
            {
                DrawPixel(x + i, j + y);
            }

        }
    }

    public void CircleDraw(int oX, int oY)
    {
        for (int y = -b_radius; y <= b_radius; y++)
            for (int x = -b_radius; x <= b_radius; x++)
                if (x * x + y * y <= b_radius * b_radius)
                    DrawPixel(oX + x, oY + y);
    }

    public void RingsBrushDraw(int x, int y)
    {
        float radSqr = b_radius * b_radius;
        float radius = b_radius;
        for (int j = 1; j < radSqr; j++)
        {
            radius = (j + 1) * b_radius;
            for (double i = 0.0; i < 360.0; i += 0.1)

            {
                double angle = i * System.Math.PI / 180;

                int x1 = (int)(x+radius * System.Math.Cos(angle));

                int y1 = (int)(y+radius * System.Math.Sin(angle));

                DrawPixel(x1, y1);
            }
        }
    }

    private void DrawPixel(int x, int y)
    {

        Color c = baseSprite.texture.GetPixel(x, y);
        if (c.a < AlphaCorrection)
        {
            image.sprite.texture.SetPixel(x, y , currentEssence.EditCol(3, 1));
        }
        else
        {

            image.sprite.texture.SetPixel(x , y ,c*currentEssence.color);
        }
    }

    public void UpdateEssence()
    {
        if (numberOfEssence.text == null || currentEssence == null) return; 
        numberOfEssence.text = currentEssence.quantity.ToString() ;
    }

    public void TryQuit()
    {
        MessageManager.Instance.NewQuestion(Quit, null, "Are you sure you want to Exit?");
    }

    public void Quit()
    {
        UIManager.Instance.ResetCanvas();
        ResetSprite();
        nameField.text = "";
        nameField.textComponent.text = "";
        GUI.SetNextControlName("Quit");
        GUI.FocusControl("Quit");
    }
    private bool IsBrushOverflow(int x, int y, int xOff, int yOff)
    {
        int w = image.sprite.texture.width;
        int h = image.sprite.texture.height;
        return (x >= w - xOff || x <= xOff || y >= h - yOff || y <= yOff);
    }

    public void TryReset()
    {
        MessageManager.Instance.NewQuestion(ResetSprite, null, "Are you sure you want to reset?");
    }

    public void ResetSprite()
    {
        currentTex = new Texture2D(baseSprite.texture.width, baseSprite.texture.height, TextureFormat.ARGB32, false);
        currentTex.SetPixels32(baseSprite.texture.GetPixels32());
        currentTex.Apply();
        spr = Sprite.Create(currentTex, baseSprite.rect, Vector2.zero);
        image.sprite = spr;
        image.rectTransform.sizeDelta = new Vector2(spr.texture.width, spr.texture.height);
    }

    private Vector2 GetLocationOnSprite()
    {
        Vector2 temp = Input.mousePosition;
        Vector2 size = image.rectTransform.rect.size / 2;
        Vector2 max = image.rectTransform.position;
        max = max - size;
        temp = temp - max;
        Vector2 r = new Vector2(temp.x / (size.x * 2), temp.y / (size.y * 2));

        r.x = (r.x) * image.sprite.texture.width;
        r.y = (r.y) * image.sprite.texture.height;
        return r;
    }

    public Texture2D CloneTexture(Texture2D basespr)
    {
        Texture2D tex2D = new Texture2D(basespr.width, basespr.height, TextureFormat.ARGB32, false);
        tex2D.SetPixels32(basespr.GetPixels32());
        tex2D.Apply();
        return tex2D;
    }

    public void SetBrushSize(int size) { b_width = size; b_height = size; b_radius = size; }
    public void SetSquare() { brush = Brush.Square; }
    public void SetCircle() { brush = Brush.Circle; }
    public void SetEssenceLight() { currentEssence = AssetManager.Instance.GetEssenceOfType(AffType.light); }
    public void SetEssenceDark() { currentEssence = AssetManager.Instance.GetEssenceOfType(AffType.dark); }
    public void SetEssenceMagic() { currentEssence = AssetManager.Instance.GetEssenceOfType(AffType.magic); }
    public void SetEssenceSpirtual() { currentEssence = AssetManager.Instance.GetEssenceOfType(AffType.spirtual); }
    public void SetEssenceFrost() { currentEssence = AssetManager.Instance.GetEssenceOfType(AffType.frost); }
    public void SetEssenceFire() { currentEssence = AssetManager.Instance.GetEssenceOfType(AffType.fire); }
    public void SetEssenceEarth() { currentEssence = AssetManager.Instance.GetEssenceOfType(AffType.earth); }
    public void SetEssenceAir() { currentEssence = AssetManager.Instance.GetEssenceOfType(AffType.air); }
}
