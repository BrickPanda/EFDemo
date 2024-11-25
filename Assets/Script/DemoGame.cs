using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Coffee.UIEffects;
using UnityEngine.UI;
using LitJson;
using UnityEngine.EventSystems;
using TMPro;

public class DemoGame : MonoBehaviour
{

    private Button resetBtn;

    private Button submitBtn;

    private TextMeshProUGUI txtProgress;

    private CanvasGroup cgReset;

    private CanvasGroup cgProgress;

    private CanvasGroup cgSubmit;

    private List<DemoCard> cards = new List<DemoCard>();

    private List<HollowOutMask> lines = new List<HollowOutMask>();

    private QuestionsItem question;

    private List<int> answerIds = new List<int>();

    private List<int> selectIds = new List<int>();

    private List<OptionsItem> optionsItems;

    private int cols = 0;

    private int rows = 0;

    private int submitCnt = 0;

    private float lineSize = 180f;

    private void Awake()
    {
        TextAsset t = Resources.Load<TextAsset>("data");

        List<ProductData> data = JsonMapper.ToObject<List<ProductData>>(t.text);

        question = data[0].Activity.Questions[0];

        TagsItem tag = question.Body.tags[0];

        rows = tag.rows;

        cols = tag.cols;

        List<string> answers = question.Body.answers[0];

        answerIds.Clear();

        for (int i = 0; i < answers.Count; i++ )
        {
            int aId = int.Parse(answers[i]);

            answerIds.Add(aId);
        }

        selectIds.Clear();

        selectIds.Add(answerIds[0]);

        optionsItems = question.Body.options;

        resetBtn = transform.Find("buttons/btn_reset").GetComponent<Button>();

        resetBtn.gameObject.AddComponent<PointerTrigger>().OnTouch = OnResetClick;

        submitBtn = transform.Find("buttons/btn_submit").GetComponent<Button>();

        submitBtn.gameObject.AddComponent<PointerTrigger>().OnTouch = OnSubmitClick;

        txtProgress = transform.Find("buttons/number/txt_count").GetComponent<TextMeshProUGUI>();

        cgReset = transform.Find("buttons/btn_reset").GetComponent<CanvasGroup>();

        cgSubmit = transform.Find("buttons/btn_submit").GetComponent<CanvasGroup>();

        cgProgress = transform.Find("buttons/number").GetComponent<CanvasGroup>();

        ChangeSubmitButton(false);

        Transform transMasks = transform.Find("masks");

        int masksNum = transMasks.childCount;

        for (int i = 0; i < masksNum; i++)
        {
            HollowOutMask line = transMasks.GetChild(i).GetComponent<HollowOutMask>();

            lines.Add(line);
        }





            /*        cards.Clear();

                    Transform transCards = transform.Find("cards");

                    int cardNum = transCards.childCount;

                    for(int i = 0; i< cardNum; i++)
                    {
                        DemoCard card = transCards.GetChild(i).GetComponent<DemoCard>();

                        cards.Add(card);

                        OptionsItem option = optionsItems[i];

                        card.SetData(option);

                        card.SetStatus(GetCardStatus(i), true);

                        card.OnTouch = OnCardClick;
                    }*/
        }

    private void Start()
    {
        cards.Clear();

        Transform transCards = transform.Find("cards");

        int cardNum = transCards.childCount;

        for (int i = 0; i < cardNum; i++)
        {
            DemoCard card = transCards.GetChild(i).GetComponent<DemoCard>();

            cards.Add(card);

            OptionsItem option = optionsItems[i];

            card.SetData(option);

            card.SetStatus(GetCardStatus(i), true);

            card.OnTouch = OnCardClick;
        }

        for (int i = 0; i < lines.Count; i++)
        {
            HollowOutMask line = lines[i];

            line.RefreshMaterial(Vector3.zero, 0, 0);
        }

        int preSelectIdx = 0;
        for(int i = 0; i < selectIds.Count; i++)
        {
            int curSelectIdx = selectIds[i];

            DemoCard curCard = cards[curSelectIdx];

            HollowOutMask line = lines[i];

            if (i == 0)
            {
                line.RefreshMaterial(curCard.anchorPos, lineSize, lineSize);
            } 
            else
            {
                DemoCard preCard = cards[preSelectIdx];

                Vector2 Center = (curCard.anchorPos + preCard.anchorPos) / 2;

                float addWidth = Mathf.Abs(curCard.anchorPos.x - preCard.anchorPos.x);

                float addHeight = Mathf.Abs(curCard.anchorPos.y - preCard.anchorPos.y);

                line.RefreshMaterial(Center, lineSize + addWidth, lineSize + addHeight);
            }
            preSelectIdx = curSelectIdx;

        }
    }

    List<int> GetCardNeighborIds(int idx)
    {
        List<int> neighborIds = new List<int>();
        int cardRow = idx / cols;
        int cardCol = idx % cols;
        if(cardRow - 1 >= 0)
        {
            int idx1 = (cardRow - 1) * cols + cardCol;
            neighborIds.Add(idx1);
        }

        if (cardRow + 1 < rows)
        {
            int idx2 = (cardRow + 1) * cols + cardCol;
            neighborIds.Add(idx2);
        }

        if (cardCol - 1 >= 0)
        {
            int idx3 = cardRow * cols + cardCol - 1;
            neighborIds.Add(idx3);
        }

        if (cardCol + 1 < cols )
        {
            int idx4 = cardRow * cols + cardCol + 1;
            neighborIds.Add(idx4);
        }
        return neighborIds;

    }

    CardStatus GetCardStatus(int idx)
    {
        CardStatus status = CardStatus.Lock;

        if(selectIds.Contains(idx))
        {
            status = CardStatus.Select;
        }
        else if(selectIds.Count > 0)
        {
            int selectEndIdx = selectIds[selectIds.Count - 1];
            List<int> neighborIds = GetCardNeighborIds(selectEndIdx);
            if (neighborIds.Contains(idx))
            {
                status = CardStatus.Active;
            }
        }
        return status;
    }

    void OnResetClick(GameObject gameObj, PointerEventData eventData)
    {
        Debug.Log("OnResetClick");

        PlayBack(1);
    }

    void OnSubmitClick(GameObject gameObj, PointerEventData eventData)
    {
        Debug.Log("OnSubmitClick");
        if(selectIds.Count == answerIds.Count)
        {
            submitCnt += 1;

            int backCnt = answerIds.Count;

            for(int i = 0; i < selectIds.Count; i++)
            {
                if(selectIds[i] != answerIds[i])
                {
                    backCnt = i;
                    break;   
                }
            }

            if (backCnt < answerIds.Count)
            {
                PlayErrorShake(backCnt);

                Sequence wait = DOTween.Sequence();

                wait.AppendInterval(0.6f);

                wait.AppendCallback(() => PlayBack(backCnt));
            }
            else
            {
                ChangeResetButton(false, 0.5f);
                ChangeSubmitButton(true, 0.5f);

            }
        }
    }

    void PlayErrorShake(int backCnt)
    {
        for (int i = backCnt; i < selectIds.Count; i++)
        {
            int selectId = selectIds[i];
            DemoCard card = cards[selectId];
            card.ShakeCard();
        }
    }

    void PlayBack(int num)
    {
        int selectCnt = selectIds.Count;
        int curSelectId = selectIds[selectCnt - 1];
        if (selectIds.Count < answerIds.Count)
        {
            List<int> neighborIds = GetCardNeighborIds(curSelectId);
            for (int i = 0; i < neighborIds.Count; i++)
            {
                int neighborId = neighborIds[i];
                if (neighborId != curSelectId)
                {
                    DemoCard nCard = cards[neighborId];
                    if (nCard.status == CardStatus.Active)
                    {
                        nCard.TransitionToLock();
                    }
                }
            }
        }

        Sequence wait = DOTween.Sequence();

        ChangeResetButton(false, 0f);

        for (int j = selectCnt - 1; j > num - 1; j--)
        {
            int idx = selectIds[j];

            int preIdx = selectIds[j - 1];

            int lineIdx = j;

            wait.AppendCallback(() =>
            {
                DemoCard nCard = cards[idx];

                nCard.TransitionToLock();

                DemoCard preCard = cards[preIdx];

                HollowOutMask line = lines[lineIdx];

                float rate = 0;

                Tweener tw1 = DOTween.To(() => rate, x =>
                {

                    rate = x;

                    Vector2 toPos = Vector2.Lerp(nCard.anchorPos, preCard.anchorPos, rate);

                    Vector2 Center = (preCard.anchorPos + toPos) / 2;

                    float addWidth = Mathf.Abs(preCard.anchorPos.x - toPos.x);

                    float addHeight = Mathf.Abs(preCard.anchorPos.y - toPos.y);

                    if(rate < 1)
                    {
                        line.RefreshMaterial(Center, lineSize + addWidth, lineSize + addHeight);
                    } 
                    else
                    {
                        line.RefreshMaterial(Center, 1, 1);
                    }

                }, 1, 0.5f);
            });

            wait.AppendInterval(0.5f);

            wait.AppendCallback(() =>
            {
                selectIds.Remove(idx);
                ChangeSubmitButton(false, 0f);
            });
        }

        wait.AppendInterval(0.5f);


        wait.AppendCallback(() =>
        {


            if (selectIds.Count < answerIds.Count)
            {
                if (submitCnt >= 2)
                {
                    GoToCorrectCard();
                } 
                else
                {
                    if (selectIds.Count > 1)
                    {
                        ChangeResetButton(true, 0.5f);
                    }

                    //ChangeSubmitButton(false, 0.5f);

                    int selectCnt = selectIds.Count;
                    int curSelectId = selectIds[selectCnt - 1];
                    List<int> neighborIds = GetCardNeighborIds(curSelectId);
                    for (int i = 0; i < neighborIds.Count; i++)
                    {
                        int neighborId = neighborIds[i];
                        if (neighborId != curSelectId)
                        {
                            DemoCard nCard = cards[neighborId];
                            if (nCard.status == CardStatus.Lock)
                            {
                                nCard.TransitionToActive();
                            }
                        }
                    }
                }
            }
        });
    }

    void GoToCorrectCard()
    {
        int selectCnt = selectIds.Count;
        Sequence wait = DOTween.Sequence();
        for (int i = selectCnt; i < answerIds.Count; i++)
        {
            int answerId = answerIds[i];

            int preSelectId = answerIds[i - 1];

            int lineIdx = i;

            selectIds.Add(answerId);

            wait.AppendCallback(() =>
            {
                DemoCard nCard = cards[answerId];
                nCard.TransitionToSelect();

                DemoCard preCard = cards[preSelectId];

                HollowOutMask line = lines[lineIdx];

                float rate = 0;

                Tweener tw1 = DOTween.To(() => rate, x => {

                    rate = x;

                    Vector2 toPos = Vector2.Lerp(preCard.anchorPos, nCard.anchorPos, rate);

                    Vector2 Center = (preCard.anchorPos + toPos) / 2;

                    float addWidth = Mathf.Abs(preCard.anchorPos.x - toPos.x);

                    float addHeight = Mathf.Abs(preCard.anchorPos.y - toPos.y);

                    line.RefreshMaterial(Center, lineSize + addWidth, lineSize + addHeight);

                }, 1, 0.5f);
            });

            wait.AppendInterval(0.5f);
        }

        wait.AppendCallback(() =>
        {
            ChangeResetButton(false, 0.5f);
            ChangeSubmitButton(true, 0.5f);
        });
    }

    void PlayAddCard()
    {
        int selectCnt = selectIds.Count;
        int curSelectId = selectIds[selectCnt - 1];
        if (selectCnt > 1)
        {
            int preSelectIdx = selectIds[selectCnt - 2];
            List<int> neighborIds = GetCardNeighborIds(preSelectIdx);
            for (int i = 0; i < neighborIds.Count; i++)
            {
                int neighborId = neighborIds[i];
                if (neighborId != curSelectId)
                {
                    DemoCard nCard = cards[neighborId];
                    if(nCard.status == CardStatus.Active)
                    {
                        nCard.TransitionToLock();
                    }
                }
            }

       

            DemoCard addCard = cards[curSelectId];

            addCard.TransitionToSelect();

            int preSelectId = selectIds[selectCnt - 2];

            DemoCard preCard = cards[preSelectId];

            HollowOutMask line = lines[selectCnt - 1];

            float rate = 0;

            Tweener tw1 = DOTween.To(() => rate, x => {
                 
                rate = x;

                Vector2 toPos = Vector2.Lerp(preCard.anchorPos, addCard.anchorPos, rate);

                Vector2 Center = (preCard.anchorPos + toPos) / 2;

                float addWidth = Mathf.Abs(preCard.anchorPos.x - toPos.x);

                float addHeight = Mathf.Abs(preCard.anchorPos.y - toPos.y);

                line.RefreshMaterial(Center, lineSize + addWidth, lineSize + addHeight);

            }, 1, 0.5f);

            cgReset.alpha = 0.5f;

            cgReset.blocksRaycasts = false;

            txtProgress.text = string.Format("{0}/{1}", selectIds.Count, answerIds.Count);

            Sequence wait = DOTween.Sequence();

            wait.AppendInterval(0.5f);

            wait.AppendCallback(() =>
            {
                ChangeResetButton(true, 0.5f);

                if (selectIds.Count < answerIds.Count)
                {
                    List<int> neighborIds = GetCardNeighborIds(curSelectId);
                    for (int i = 0; i < neighborIds.Count; i++)
                    {
                        int neighborId = neighborIds[i];
                        if (neighborId != curSelectId)
                        {
                            DemoCard nCard = cards[neighborId];
                            if (nCard.status == CardStatus.Lock)
                            {
                                nCard.TransitionToActive();
                            }
                        }
                    }
                } 
                else
                {
                    ChangeSubmitButton(true, 0.5f);
                }
            });
        }
    }

    void ChangeSubmitButton(bool active, float duration = 0)
    {
        float alpha = active ? 1.0f : 0f;

        txtProgress.text = string.Format("{0}/{1}", selectIds.Count, answerIds.Count);

        if (duration <= 0)
        {
            cgProgress.alpha = 1 - alpha;

            cgSubmit.alpha = alpha;

            cgSubmit.blocksRaycasts = active;

            cgSubmit.interactable = active;
        }
        else
        {
            cgSubmit.blocksRaycasts = false;

            cgSubmit.interactable = false;

            cgProgress.DOFade(1 - alpha, duration);

            cgSubmit.DOFade(alpha, duration).OnComplete(() =>
            {
                cgSubmit.blocksRaycasts = active;

                cgSubmit.interactable = active;

            });
        }
    }


    void ChangeResetButton(bool active, float duration = 0)
    {
        float alpha = active ? 1.0f : 0.5f;

        if(duration <= 0)
        {
            cgReset.alpha = alpha;

            cgReset.blocksRaycasts = active;

            cgReset.interactable = active;
        }
        else
        {
            cgReset.blocksRaycasts = false;

            cgReset.interactable = false;

            cgReset.DOFade(alpha, duration).OnComplete(() =>
            {
                cgReset.blocksRaycasts = active;

                cgReset.interactable = active;

            });
        }
    }



    void OnCardClick(GameObject gameObj, int idx)
    {
        Debug.Log($"OnCardClick {idx}");

        selectIds.Add(idx);

        PlayAddCard();

    }

    void RefreshUI()
    {

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
