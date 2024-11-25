using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Coffee.UIEffects;
using UnityEngine.EventSystems;

public enum CardStatus
{
    Lock = 1,
    Active = 2,
    Select = 3,

}

public delegate void CardTouchDelegate(GameObject gameObj, int id);



public class DemoCard : MonoBehaviour
{
    public Image imgBg;

    public Image imgCard;

    public Button btnItem;

    public CardTouchDelegate OnTouch;

    private UIEffect bgEffect;

    private UIEffect cardEffect;

    public CardStatus status = CardStatus.Lock;

    public Vector2 anchorPos;

    private void Awake()
    {

        imgBg = transform.Find("img_cardbg").GetComponent<Image>();

        imgCard = transform.Find("img_card").GetComponent<Image>();

        btnItem = transform.Find("btn_item").GetComponent<Button>();

        bgEffect = imgBg.GetComponent<UIEffect>();

        cardEffect = imgCard.GetComponent<UIEffect>();

        btnItem.gameObject.AddComponent<PointerTrigger>().OnTouch = OnItemClick;

        anchorPos = transform.GetComponent<RectTransform>().anchoredPosition;
    }

    // Use this for initialization
    void Start()
    {
  
    }

    private OptionsItem _data;

    public int idx = 0;

    public void SetData(OptionsItem data)
    {
        _data = data;

        idx = int.Parse(_data.id);

        string imagePath = string.Format("image/{0}", _data.image.sha1);

        imgCard.sprite = Resources.Load<Sprite>(imagePath);
    }

    void OnItemClick(GameObject gameObj, PointerEventData eventData)
    {
        if(status == CardStatus.Active)
        {
            if (OnTouch != null)
            {
                OnTouch(gameObject, idx);
            }
        }
    }

    public void SetStatus(CardStatus cardstatus, bool isRefreshUI = false)
    {
        status = cardstatus;

        if(isRefreshUI)
        {
            switch (status)
            {
                case CardStatus.Lock:
                    bgEffect.transitionRate = 0;
                    cardEffect.shadowFade = 0;
                    imgCard.color = new Color(1, 1, 1, 0.5f);
                    imgCard.transform.localScale = Vector3.one;
                    break;
                case CardStatus.Active:
                    bgEffect.transitionRate = 0;
                    cardEffect.shadowFade = 0.5f;
                    cardEffect.color = Color.black;
                    imgCard.color = Color.white;
                    cardEffect.shadowMode = ShadowMode.Shadow;
                    cardEffect.shadowDistance = new Vector2(0f, -10f);
                    imgCard.transform.localScale = Vector3.one * 1.1f;
                    break;
                case CardStatus.Select:
                    bgEffect.transitionRate = 1;
                    cardEffect.shadowFade = 1;
                    cardEffect.color = Color.white;
                    imgCard.color = Color.white;
                    cardEffect.shadowMode = ShadowMode.Outline8;
                    cardEffect.shadowDistance = new Vector2(4f, -4f);
                    imgCard.transform.localScale = Vector3.one;
                    break;

            }
        }
    }






    private Sequence seqAnim;

    public float ShakeCard()
    {
        if(seqAnim != null)
        {
            seqAnim.Kill(true);

            seqAnim = null;
        }

        seqAnim = DOTween.Sequence();

        Tweener tw1 = imgCard.transform.DOPunchRotation(new Vector3(0, 0, 10), 0.1f);

        Tweener tw2 = imgCard.transform.DOPunchRotation(new Vector3(0, 0, -10), 0.1f);

        seqAnim.Append(tw1);

        seqAnim.Append(tw2);

        seqAnim.SetLoops(3);

        return 0.6f;
    }

    public float TransitionToSelect()
    {
        if (seqAnim != null)
        {
            seqAnim.Kill(true);

            seqAnim = null;
        }

        status = CardStatus.Select;

        cardEffect.shadowFade = 0;

        bgEffect.transitionRate = 0;

        cardEffect.shadowMode = ShadowMode.Outline8;

        cardEffect.shadowDistance = new Vector2(4f, -4f);

        cardEffect.color = Color.white;

        seqAnim = DOTween.Sequence();

        Tweener tw1 = DOTween.To(() => cardEffect.shadowFade, x => cardEffect.shadowFade = x, 1, 0.2f);

        Tweener tw2 = imgCard.transform.DOScale(1, 0.2f);

        Tweener tw3 = DOTween.To(() => bgEffect.transitionRate, x => bgEffect.transitionRate = x, 1, 0.5f);

        Tweener tw4 = imgCard.DOFade(1, 0.2f);

        seqAnim.Append(tw3);

        seqAnim.Insert(0, tw1);

        seqAnim.Insert(0, tw2);

        seqAnim.Insert(0, tw4);

        return 0.5f;
    }

    public float TransitionToActive()
    {

        if (seqAnim != null)
        {
            seqAnim.Kill(true);

            seqAnim = null;
        }

        cardEffect.shadowFade = 0;

        bgEffect.transitionRate = 0;

        cardEffect.shadowMode = ShadowMode.Shadow;

        cardEffect.shadowDistance = new Vector2(0f, -10f);

        cardEffect.color = Color.black;

        imgCard.color = Color.white;

        seqAnim = DOTween.Sequence();

        Tweener tw1 = DOTween.To(() => cardEffect.shadowFade, x => cardEffect.shadowFade = x, 0.5f, 0.5f);

        Tweener tw2 = imgCard.transform.DOScale(1.1f, 0.2f);

        Tweener tw3 = imgCard.DOFade(1, 0.2f);

        seqAnim.Append(tw3);

        seqAnim.Insert(0, tw1);

        seqAnim.Insert(0, tw2);

        seqAnim.AppendCallback(()=> status = CardStatus.Active);

        return 0.5f;
    }


    public float TransitionToLock()
    {

        if (seqAnim != null)
        {
            seqAnim.Kill(true);

            seqAnim = null;
        }

        status = CardStatus.Lock;

        cardEffect.shadowMode = ShadowMode.Shadow;

        seqAnim = DOTween.Sequence();

        Tweener tw1 = DOTween.To(() => cardEffect.shadowFade, x => cardEffect.shadowFade = x, 0f, 0.5f);

        Tweener tw2 = imgCard.transform.DOScale(1f, 0.2f);

        Tweener tw3 = imgCard.DOFade(0.5f, 0.2f);

        Tweener tw4 = DOTween.To(() => bgEffect.transitionRate, x => bgEffect.transitionRate = x, 0, 0.5f);

        seqAnim.Append(tw4);

        seqAnim.Insert(0, tw1);

        seqAnim.Insert(0, tw2);

        seqAnim.Insert(0, tw3);

        return 0.5f;
    }



    public void RefreshUI()
    {
        



    }

        // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TransitionToLock();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            TransitionToActive();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            TransitionToSelect();
        }
    }
}