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

    private OreParent currentOre = new OreParent(OreType.Steel, 0, Color.gray);

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
        WeaponRecognition.Instance.LoadWeapon(image.sprite, "1h");
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
            currentOre.col.a = 1;
            loc = oldPos;
        }
	}

    public void SaveImage()
    {
        if (nameField.text.Length <= 2)
        {
            MessageManager.Instance.NewMessage("Come on, you need a longer name that that...");
            return;
        }
        WeaponData wd = new WeaponData(nameField.text, image.sprite, nameField.text, 0.1f, new AffinityData() );

        SaveLoadClass.SaveWeapon(image.sprite.texture, nameField.text, wd);

        SaveLoadClass.LoadSprite(nameField.text);

        LootFactory.Instance.AddWeaponsToLoot(wd);

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
            currentOre.col.a = 0.9f;
            image.sprite.texture.SetPixel(x, y , currentOre.col);
        }
        else
        {

            image.sprite.texture.SetPixel(x , y ,c*currentOre.col);
        }
    }

    private bool IsBrushOverflow(int x, int y, int xOff, int yOff)
    {
        int w = image.sprite.texture.width;
        int h = image.sprite.texture.height;
        return (x >= w - xOff || x <= xOff || y >= h - yOff || y <= yOff);
    }

    public void ResetSprite()
    {
        currentTex = new Texture2D(baseSprite.texture.width, baseSprite.texture.height, TextureFormat.ARGB32, false);
        currentTex.SetPixels32(baseSprite.texture.GetPixels32());
        currentTex.Apply();
        spr = Sprite.Create(currentTex, baseSprite.rect, Vector2.zero);
        image.sprite = spr;
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
    public void SetOreSteel() { currentOre = AssetManager.Instance.GetOreOfType(OreType.Steel); }
    public void SetOreDark() { currentOre = AssetManager.Instance.GetOreOfType(OreType.Dark); }
    public void SetOreMagic() { currentOre = AssetManager.Instance.GetOreOfType(OreType.Magic); }
    public void SetOreOrc() { currentOre = AssetManager.Instance.GetOreOfType(OreType.Orc); }
    public void SetOreFrost() { currentOre = AssetManager.Instance.GetOreOfType(OreType.Frost); }
    public void SetOreFire() { currentOre = AssetManager.Instance.GetOreOfType(OreType.Fire); }
}
