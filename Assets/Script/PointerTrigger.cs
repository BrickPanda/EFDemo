using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;

public class PointerTrigger : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler,ICancelHandler
{
        /// <summary>
        /// 代理
        /// </summary>
        /// <param name="go"></param>
        #region 
        public delegate void PointerEventDelegate (GameObject gameObj, PointerEventData eventData);
        public PointerEventDelegate OnTouch;
        public PointerEventDelegate OnTouchDown; 
        public PointerEventDelegate OnTouchUp;
        public PointerEventDelegate OnLongTouch;
        public PointerEventDelegate OnTouchEnter;
        public PointerEventDelegate OnTouchExit;
        public PointerEventDelegate OnTouchCancel;

        

        public delegate void PointerEventExDelegate (GameObject gameObj,PointerEventData eventData);
        static public  PointerEventExDelegate OnGlobalTouch = null ;
        #endregion
        
        public bool ShakeEnable {get {return shakeAni ;} set {shakeAni = value;}} 
        public float TouchTime { get {return touchTime;} set {touchTime = value;} }

        public bool IsPassEvent { get { return isPassEvent; } set { isPassEvent = value; } }

        float touchTime = 1.05f;
        
        float pressTime = 0f;

        float eventTime =  1.0f;

        bool shakeAni = false;

        bool isPassEvent = false;

        Vector3 defScale = Vector3.one;


        public float TouchEventTime 
        {
            get 
            {
                return eventTime;
            }

            set 
            {
                eventTime = value;
            }
        }

        bool longClickTag = false;

        /// <summary>
        /// PointerTrigger:Init
        /// </summary>
        /// <param name="gameObj"></param>
        /// <returns></returns>
        public static PointerTrigger Setup(GameObject gameObj)
        {
            PointerTrigger trigger = gameObj.GetComponent<PointerTrigger>();
            if (trigger == null) 
            {
                trigger = gameObj.AddComponent<PointerTrigger>();
            }
            return trigger;
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            float checkTime = Time.realtimeSinceStartup - pressTime;
            if (checkTime < TouchEventTime)
            {
                if(OnGlobalTouch != null) 
                {
                    OnGlobalTouch(gameObject,eventData);
                }

                if (OnTouch != null)
                {
                    OnTouch(gameObject, eventData);
                }
            }

            if (isPassEvent)
            {
                PassEvent(eventData, ExecuteEvents.pointerClickHandler);
            }
        }

        public void OnPointerDown (PointerEventData eventData)
        {
            pressTime = Time.realtimeSinceStartup;
            longClickTag = true;
            if (OnTouchDown != null) 
            {
                OnTouchDown(gameObject, eventData);
                OnAnimation(true);
            }

            if (isPassEvent)
            {
                PassEvent(eventData, ExecuteEvents.pointerDownHandler);
            }
        }

        public void OnPointerUp (PointerEventData eventData)
        {
            longClickTag = false;

            if (OnTouchUp != null) 
            {
                OnTouchUp(eventData.pointerCurrentRaycast.gameObject, eventData);
            }

            if(isPassEvent)
            {
                PassEvent(eventData, ExecuteEvents.pointerUpHandler);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (OnTouchEnter != null)
            {
                OnTouchEnter(gameObject, eventData);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (OnTouchExit != null)
            {
                OnTouchExit(gameObject, eventData);
            }
        }

        public virtual void OnCancel(BaseEventData eventData)
        {
            if (OnTouchCancel != null)
            {
                OnTouchCancel(gameObject, (PointerEventData)eventData);
            }
        }

        #region Unity Event
        void Start()
        {
            defScale = transform.localScale;    
        }

        void Update()
        {
            if (longClickTag)
            {
                float time = Time.realtimeSinceStartup - pressTime;
                if (time >= touchTime)
                {
                    if (OnLongTouch != null)
                        OnLongTouch(gameObject, null);

                    longClickTag = false;
                }
            }
        }

        void OnDestroy()
        {
            OnAnimation(false);
            OnTouch = null;
            OnLongTouch = null;
            OnTouchDown = null;
            OnTouchUp = null;
            OnTouchEnter = null;
            OnTouchExit = null;
        }

        void OnDisable()
        {
            OnAnimation(false);
        }

        #endregion
       

        void OnAnimation(bool play)
        {
            if(shakeAni)
            {
                if(play) 
                {
                    ResetState();
                    transform.DOShakeScale(2f,0.15f,2,10);
                }else 
                {
                    ResetState();
                }
            }
        }

        void ResetState()
        {
            transform.localScale = defScale;
            transform.DOKill();
        }

        public void PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function) where T : IEventSystemHandler
        {
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, results);
            GameObject current = data.pointerCurrentRaycast.gameObject;
            for (int i = 0; i < results.Count; i++)
            {
                if (current != results[i].gameObject)
                {
                    ExecuteEvents.Execute(results[i].gameObject, data, function);
                    break;
                }
            }
        }
    }
