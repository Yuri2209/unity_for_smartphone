using UnityEngine;
using UnityEngine.UI;

public class UIActions : MonoBehaviour
{
    // ★ GameObject じゃなくて Image を直接参照（ミスが減る）
    [SerializeField] private Image targetImage;

    // ★ 表示したいSprite（greenapple_0）をここに入れる
    [SerializeField] private Sprite spriteToShow;

    public void OnButtonClick()
    {
        Debug.Log("ボタン押下！");

        if (targetImage == null)
        {
            Debug.LogError("targetImage(Image) が設定されてないよ！");
            return;
        }

        // 1) 表示するSpriteを確実に入れる
        if (spriteToShow != null)
        {
            targetImage.sprite = spriteToShow;
        }
        else
        {
            Debug.LogWarning("spriteToShow が未設定（でも表示自体は試す）");
        }

        // 2) GameObjectとImageコンポーネントを確実に有効化
        targetImage.gameObject.SetActive(true);
        targetImage.enabled = true;

        // 3) 透明になってても見えるようにする（Alphaだけ1にする）
        var c = targetImage.color;
        c.a = 1f;
        targetImage.color = c;

        // 4) 前面に出す（描画順の最後）
        targetImage.transform.SetAsLastSibling();

        // 5) サイズが0や極小ならネイティブサイズを試す
        if (targetImage.rectTransform.rect.width < 2f || targetImage.rectTransform.rect.height < 2f)
        {
            targetImage.SetNativeSize();
        }

        // 6) 念のためUI再描画
        Canvas.ForceUpdateCanvases();

        Debug.Log($"activeInHierarchy={targetImage.gameObject.activeInHierarchy}, enabled={targetImage.enabled}, alpha={targetImage.color.a}, sprite={(targetImage.sprite ? targetImage.sprite.name : "null")}, size={targetImage.rectTransform.rect.size}");
    }
}
