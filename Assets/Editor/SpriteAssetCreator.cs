using System.Linq; // Убедитесь, что это пространство имен подключено
using UnityEngine;
using UnityEditor;
using TMPro;
public class SpriteAssetCreator : MonoBehaviour
{
 [MenuItem("Tools/Create Sprite Asset")]
    public static void CreateSpriteAsset()
    {
        // Path where the sprite asset will be created
        string path = "Assets/Resources/SpriteAsset.asset";

        // Create new sprite asset
        TMP_SpriteAsset spriteAsset = ScriptableObject.CreateInstance<TMP_SpriteAsset>();

        // Load the texture
        Texture2D spriteSheet = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/1_icon.png");
        if (spriteSheet == null)
        {
            Debug.LogError("Sprite sheet not found at specified path.");
            return;
        }

        // Create sprite asset
        spriteAsset.spriteSheet = spriteSheet;
        AssetDatabase.CreateAsset(spriteAsset, path);

        // Import the texture
        Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(spriteSheet)).OfType<Sprite>().ToArray();
        if (sprites.Length == 0)
        {
            Debug.LogError("No sprites found in the sprite sheet.");
            return;
        }

        // Add sprites to the sprite asset
        foreach (var sprite in sprites)
        {
            TMP_SpriteGlyph spriteGlyph = new TMP_SpriteGlyph();
            spriteGlyph.sprite = sprite;
            spriteAsset.spriteGlyphTable.Add(spriteGlyph);

            TMP_SpriteCharacter spriteCharacter = new TMP_SpriteCharacter((uint)sprite.name.GetHashCode(), spriteGlyph);
            spriteCharacter.name = sprite.name;
            spriteAsset.spriteCharacterTable.Add(spriteCharacter);
        }

        // Save the asset
        AssetDatabase.SaveAssets();
        Debug.Log("Sprite asset created successfully at " + path);
    }
}