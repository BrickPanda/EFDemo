﻿using System;
using System.Collections.Generic;
using Coffee.UIEffectInternal;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
using UnityEngine.UI;
#if TMP_ENABLE
using TMPro;
#endif

namespace Coffee.UIEffects
{
    public class UIEffectContext
    {
        private const float k_MaxEffectDistance = 600f;
        private static readonly UIEffectContext s_DefaultContext = new UIEffectContext();
        private static readonly List<UIVertex> s_WorkingVertices = new List<UIVertex>();
        private static readonly int s_SrcBlend = Shader.PropertyToID("_SrcBlend");
        private static readonly int s_DstBlend = Shader.PropertyToID("_DstBlend");
        private static readonly int s_ToneIntensity = Shader.PropertyToID("_ToneIntensity");
        private static readonly int s_ToneParams = Shader.PropertyToID("_ToneParams");
        private static readonly int s_ColorValue = Shader.PropertyToID("_ColorValue");
        private static readonly int s_ColorIntensity = Shader.PropertyToID("_ColorIntensity");
        private static readonly int s_SamplingIntensity = Shader.PropertyToID("_SamplingIntensity");
        private static readonly int s_TransitionRate = Shader.PropertyToID("_TransitionRate");
        private static readonly int s_TransitionReverse = Shader.PropertyToID("_TransitionReverse");
        private static readonly int s_TransitionTex = Shader.PropertyToID("_TransitionTex");
        private static readonly int s_TransitionTex_ST = Shader.PropertyToID("_TransitionTex_ST");
        private static readonly int s_TransitionWidth = Shader.PropertyToID("_TransitionWidth");
        private static readonly int s_TransitionSoftness = Shader.PropertyToID("_TransitionSoftness");
        private static readonly int s_TransitionColor = Shader.PropertyToID("_TransitionColor");
        private static readonly int s_TransitionColorFilter = Shader.PropertyToID("_TransitionColorFilter");
        private static readonly int s_TargetColor = Shader.PropertyToID("_TargetColor");
        private static readonly int s_TargetRange = Shader.PropertyToID("_TargetRange");
        private static readonly int s_TargetSoftness = Shader.PropertyToID("_TargetSoftness");

        private static readonly string[] s_ToneKeywords =
        {
            "",
            "TONE_GRAYSCALE",
            "TONE_SEPIA",
            "TONE_NEGATIVE",
            "TONE_RETRO",
            "TONE_POSTERIZE"
        };

        private static readonly string[] s_ColorKeywords =
        {
            "",
            "COLOR_MULTIPLY",
            "COLOR_ADDITIVE",
            "COLOR_SUBTRACTIVE",
            "COLOR_REPLACE",
            "COLOR_MULTIPLY_LUMINANCE",
            "COLOR_MULTIPLY_ADDITIVE",
            "COLOR_HSV_MODIFIER",
            "COLOR_CONTRAST"
        };

        private static readonly string[] s_SamplingKeywords =
        {
            "",
            "SAMPLING_BLUR_FAST",
            "SAMPLING_BLUR_MEDIUM",
            "SAMPLING_BLUR_DETAIL",
            "SAMPLING_PIXELATION",
            "SAMPLING_RGB_SHIFT",
            "SAMPLING_EDGE_LUMINANCE",
            "SAMPLING_EDGE_ALPHA"
        };

        private static readonly string[] s_TransitionKeywords =
        {
            "",
            "TRANSITION_FADE",
            "TRANSITION_CUTOFF",
            "TRANSITION_DISSOLVE",
            "TRANSITION_SHINY",
            "TRANSITION_MASK",
            "TRANSITION_MELT",
            "TRANSITION_BURN"
        };

        private static readonly string[] s_TransitionColorKeywords =
        {
            "",
            "TRANSITION_COLOR_MULTIPLY",
            "TRANSITION_COLOR_ADDITIVE",
            "TRANSITION_COLOR_SUBTRACTIVE",
            "TRANSITION_COLOR_REPLACE",
            "TRANSITION_COLOR_MULTIPLY_LUMINANCE",
            "TRANSITION_COLOR_MULTIPLY_ADDITIVE",
            "TRANSITION_COLOR_HSV_MODIFIER",
            "TRANSITION_COLOR_CONTRAST"
        };

        private static readonly string[] s_TargetKeywords =
        {
            "",
            "TARGET_HUE",
            "TARGET_LUMINANCE"
        };

        public ToneFilter toneFilter = ToneFilter.None;
        public float toneIntensity = 1;
        public Vector4 toneParams = new Vector4(0, 0, 0, 0);
        public ColorFilter colorFilter = ColorFilter.None;
        public float colorIntensity = 1;
        public Color color = Color.white;
        public SamplingFilter samplingFilter = SamplingFilter.None;
        public float samplingIntensity = 0.5f;
        public TransitionFilter transitionFilter = TransitionFilter.None;
        public float transitionRate = 0.5f;
        public bool transitionReverse;
        public Texture transitionTex;
        public Vector2 transitionTexScale = new Vector2(1, 1);
        public Vector2 transitionTexOffset = new Vector2(0, 0);
        public float transitionRotation = 0;
        public bool transitionKeepAspectRatio = true;
        public float transitionWidth = 0.2f;
        public float transitionSoftness = 0.2f;
        public ColorFilter transitionColorFilter = ColorFilter.MultiplyAdditive;
        public Color transitionColor = new Color(0f, 0.5f, 1.0f, 1.0f);
        public TargetMode targetMode = TargetMode.None;
        public Color targetColor = Color.white;
        public float targetRange = 1;
        public float targetSoftness = 0.5f;
        public BlendMode srcBlendMode = BlendMode.One;
        public BlendMode dstBlendMode = BlendMode.OneMinusSrcAlpha;
        public ShadowMode shadowMode = ShadowMode.None;
        public Vector2 shadowDistance = new Vector2(1f, -1f);
        public int shadowIteration = 1;
        public float shadowFade = 0.9f;
        public bool shadowEffectOnOrigin = false;
        public float shadowMirrorScale = 0.5f;
        // public RectTransform transitionRoot;

        public bool willModifyMaterial => samplingFilter != SamplingFilter.None
                                          || transitionFilter != TransitionFilter.None
                                          || toneFilter != ToneFilter.None
                                          || colorFilter != ColorFilter.None
                                          || srcBlendMode != BlendMode.One
                                          || dstBlendMode != BlendMode.OneMinusSrcAlpha;

        public void Reset()
        {
            CopyFrom(s_DefaultContext);
        }

        public void CopyFrom(UIEffectContext preset)
        {
            toneFilter = preset.toneFilter;
            toneIntensity = preset.toneIntensity;
            toneParams = preset.toneParams;

            colorFilter = preset.colorFilter;
            color = preset.color;
            colorIntensity = preset.colorIntensity;

            samplingFilter = preset.samplingFilter;
            samplingIntensity = preset.samplingIntensity;

            transitionFilter = preset.transitionFilter;
            transitionRate = preset.transitionRate;
            transitionReverse = preset.transitionReverse;
            transitionTex = preset.transitionTex;
            transitionTexScale = preset.transitionTexScale;
            transitionTexOffset = preset.transitionTexOffset;
            transitionKeepAspectRatio = preset.transitionKeepAspectRatio;
            transitionRotation = preset.transitionRotation;
            transitionWidth = preset.transitionWidth;
            transitionSoftness = preset.transitionSoftness;
            transitionColor = preset.transitionColor;
            transitionColorFilter = preset.transitionColorFilter;

            targetMode = preset.targetMode;
            targetColor = preset.targetColor;
            targetRange = preset.targetRange;
            targetSoftness = preset.targetSoftness;

            srcBlendMode = preset.srcBlendMode;
            dstBlendMode = preset.dstBlendMode;

            shadowMode = preset.shadowMode;
            shadowDistance = preset.shadowDistance;
            shadowIteration = preset.shadowIteration;
            shadowFade = preset.shadowFade;
            shadowEffectOnOrigin = preset.shadowEffectOnOrigin;
        }

        public void ApplyToMaterial(Material material)
        {
            if (!material) return;

            Profiler.BeginSample("(UIE)[UIEffect] GetModifiedMaterial");

            material.SetInt(s_SrcBlend, (int)srcBlendMode);
            material.SetInt(s_DstBlend, (int)dstBlendMode);

            material.SetFloat(s_ToneIntensity, Mathf.Clamp01(toneIntensity));
            material.SetVector(s_ToneParams, toneParams);

            material.SetColor(s_ColorValue, color);
            material.SetFloat(s_ColorIntensity, Mathf.Clamp01(colorIntensity));

            material.SetFloat(s_SamplingIntensity, Mathf.Clamp01(samplingIntensity));

            material.SetFloat(s_TransitionRate, Mathf.Clamp01(transitionRate));
            material.SetInt(s_TransitionReverse, transitionReverse ? 1 : 0);
            material.SetTexture(s_TransitionTex, transitionTex);
            material.SetVector(s_TransitionTex_ST,
                new Vector4(transitionTexScale.x, transitionTexScale.y,
                    transitionTexOffset.x, transitionTexOffset.y));
            material.SetFloat(s_TransitionWidth, Mathf.Clamp01(transitionWidth));
            material.SetFloat(s_TransitionSoftness, Mathf.Clamp01(transitionSoftness));
            material.SetColor(s_TransitionColor, transitionColor);
            material.SetInt(s_TransitionColorFilter, (int)transitionColorFilter);

            material.SetColor(s_TargetColor, targetColor);
            material.SetFloat(s_TargetRange, Mathf.Clamp01(targetRange));
            material.SetFloat(s_TargetSoftness, Mathf.Clamp01(targetSoftness));

            SetKeyword(material, s_ToneKeywords, (int)toneFilter);
            SetKeyword(material, s_ColorKeywords, (int)colorFilter);
            SetKeyword(material, s_SamplingKeywords, (int)samplingFilter);
            SetKeyword(material, s_TransitionKeywords, (int)transitionFilter);
            switch (transitionFilter)
            {
                case TransitionFilter.None:
                case TransitionFilter.Fade:
                case TransitionFilter.Cutoff:
                    SetKeyword(material, s_TransitionColorKeywords, (int)ColorFilter.None);
                    break;
                case TransitionFilter.Dissolve:
                case TransitionFilter.Shiny:
                case TransitionFilter.Mask:
                case TransitionFilter.Melt:
                case TransitionFilter.Burn:
                    SetKeyword(material, s_TransitionColorKeywords, (int)transitionColorFilter);
                    break;
            }

            SetKeyword(material, s_TargetKeywords, (int)targetMode);

            Profiler.EndSample();
        }

        private static void SetKeyword(Material material, string[] keywords, int index)
        {
            for (var i = 0; i < keywords.Length; i++)
            {
                if (i != index)
                {
                    material.DisableKeyword(keywords[i]);
                }
                else if (!string.IsNullOrEmpty(keywords[i]))
                {
                    material.EnableKeyword(keywords[i]);
                }
            }
        }

        public void ModifyMesh(Graphic graphic, RectTransform transitionRoot, VertexHelper vh)
        {
            // var graphic = uiEffect.graphic;
            // var transitionRoot = uiEffect.transitionRoot;
#if TMP_ENABLE
            var isTextMeshPro = graphic is TextMeshProUGUI || graphic is TMP_SubMeshUI;
            var isText = isTextMeshPro || graphic is Text;

            // Sprite mode.
            if (graphic is TMP_SubMeshUI sub && sub.spriteAsset && sub.sharedMaterial == sub.spriteAsset.material)
            {
                isTextMeshPro = false;
            }
#else
            var isTextMeshPro = false;
            var isText = graphic is Text;
#endif

            var expandSize = GetExpandSize();
            var useExpand = expandSize != Vector2.zero;

            // Update capacity of workingVertices if needed.
            var count = vh.currentIndexCount;
            var neededCapacity = Mathf.NextPowerOfTwo(count * GetVertexCountMultiply());
            if (s_WorkingVertices.Capacity < neededCapacity)
            {
                s_WorkingVertices.Capacity = neededCapacity;
            }

            // Get the rectangle to calculate the normalized position.
            vh.GetUIVertexStream(s_WorkingVertices);
            var bundleSize = isText ? 6 : count;
            var rot = Matrix4x4.Rotate(Quaternion.Euler(0, 0, transitionRotation));
            var v1 = rot.MultiplyPoint3x4(new Vector3(1, 1, 0));
            var multiplier = Mathf.Max(Mathf.Abs(v1.x), Mathf.Abs(v1.y));

            Rect rect;
            if (transitionRoot)
            {
                rect = transitionRoot.rect;
                rot *= Matrix4x4.Scale(new Vector3(1 / multiplier, 1 / multiplier, 1))
                       * transitionRoot.worldToLocalMatrix
                       * graphic.rectTransform.localToWorldMatrix;
            }
            else
            {
                rect = graphic.rectTransform.rect;
                rot *= Matrix4x4.Scale(new Vector3(multiplier, multiplier, 1));
            }

            if (transitionKeepAspectRatio && transitionTex)
            {
                var center = rect.center;
                var aspectRatio = (float)transitionTex.width / transitionTex.height;
                if (rect.width < rect.height)
                {
                    rect.width = rect.height * aspectRatio;
                }
                else
                {
                    rect.height = rect.width / aspectRatio;
                }

                rect.center = center;
            }

            // Modify vertices for each bundled-quad.
            for (var i = 0; i < count; i += bundleSize)
            {
                // min/max for bundled-quad
                GetBounds(s_WorkingVertices, i, bundleSize, out var bounds, out var uvMask);

                // Quad (6 vertices)
                for (var j = 0; j < bundleSize; j += 6)
                {
                    var size = default(Vector3);
                    var extendPos = default(Vector3);
                    var extendUV = default(Vector3);
                    var posLB = s_WorkingVertices[i + j + 1].position;
                    var posRT = s_WorkingVertices[i + j + 4].position;
                    var willExpand = useExpand
                                     && (bundleSize == 6 // Text or simple quad
                                         || !bounds.Contains(posLB) ||
                                         !bounds.Contains(posRT)); // Outer 9-sliced quad
                    if (willExpand)
                    {
                        var uvLB = s_WorkingVertices[i + j + 1].uv0;
                        var uvRT = s_WorkingVertices[i + j + 4].uv0;
                        var posCenter = (posLB + posRT) / 2;
                        var uvCenter = (uvLB + uvRT) / 2;
                        size = posLB - posRT;
                        size.x = 1 + expandSize.x / Mathf.Abs(size.x);
                        size.y = 1 + expandSize.y / Mathf.Abs(size.y);
                        size.z = 1;
                        extendPos = posCenter - Vector3.Scale(size, posCenter);
                        extendUV = uvCenter - Vector4.Scale(size, uvCenter);
                    }

                    // Set vertex position, uv, uvMask and local normalized position.
                    for (var k = 0; k < 6; k++)
                    {
                        var vt = s_WorkingVertices[i + j + k];
                        var pos = vt.position;
                        var uv0 = vt.uv0;

                        // Expand edge vertex
                        if (willExpand)
                        {
                            if (pos.x < bounds.xMin || bounds.xMax < pos.x)
                            {
                                pos.x = pos.x * size.x + extendPos.x;
                                uv0.x = uv0.x * size.x + extendUV.x;
                            }

                            if (pos.y < bounds.yMin || bounds.yMax < pos.y)
                            {
                                pos.y = pos.y * size.y + extendPos.y;
                                uv0.y = uv0.y * size.y + extendUV.y;
                            }
                        }

                        ModifyVertex(isTextMeshPro, ref vt, pos, uv0, uvMask, rect, rot);
                        s_WorkingVertices[i + j + k] = vt;
                    }
                }
            }

            if (shadowMode != ShadowMode.None)
            {
                var start = 0;
                var end = count;
                if (shadowMode == ShadowMode.Mirror)
                {
                    var rect2 = transitionRoot.rect;
                    var pivot = transitionRoot.pivot.y;
                    var height = rect2.height;
                    var rate = shadowDistance.x;
                    var scale = shadowMirrorScale;
                    var offset = shadowDistance.y - (scale + 1) * pivot * height;
                    var range = new Vector2(rect2.yMin, rect2.yMax);
                    ApplyMirror(s_WorkingVertices, count, rate, range, scale, offset, shadowFade);
                }
                else
                {
                    var d = Vector2.zero;
                    var a = 1f;
                    var distance = new Vector2(Mathf.Clamp(shadowDistance.x, -k_MaxEffectDistance, k_MaxEffectDistance),
                        Mathf.Clamp(shadowDistance.y, -k_MaxEffectDistance, k_MaxEffectDistance));
                    for (var i = 0; i < shadowIteration; i++)
                    {
                        d += distance / (i + 1);
                        a *= shadowFade;
                        ApplyShadow(s_WorkingVertices, ref start, ref end, d, shadowMode, a);
                    }
                }

                // Mark as origin vertices.
                if (!shadowEffectOnOrigin)
                {
                    for (var i = s_WorkingVertices.Count - count; i < s_WorkingVertices.Count; i++)
                    {
                        var vt = s_WorkingVertices[i];
                        if (isTextMeshPro)
                        {
                            vt.uv1.z -= 8;
                            vt.uv1.w -= 8;
                        }
                        else
                        {
                            vt.uv0.z -= 8;
                            vt.uv0.w -= 8;
                        }

                        s_WorkingVertices[i] = vt;
                    }
                }
            }

            vh.Clear();
            vh.AddUIVertexTriangleStream(s_WorkingVertices);
        }

        private int GetVertexCountMultiply()
        {
            switch (shadowMode)
            {
                case ShadowMode.Shadow: return 1 + shadowIteration * 1;
                case ShadowMode.Shadow3: return 1 + shadowIteration * 3;
                case ShadowMode.Outline: return 1 + shadowIteration * 4;
                case ShadowMode.Outline8: return 1 + shadowIteration * 8;
                case ShadowMode.Mirror: return 1 + 1;
                default: return 1;
            }
        }

        private Vector2 GetExpandSize()
        {
            var expandSize = Vector2.zero;
            switch (samplingFilter)
            {
                case SamplingFilter.BlurFast:
                    expandSize.x += 10;
                    expandSize.y += 10;
                    break;
                case SamplingFilter.BlurMedium:
                    expandSize.x += 15;
                    expandSize.y += 15;
                    break;
                case SamplingFilter.BlurDetail:
                    expandSize.x += 20;
                    expandSize.y += 20;
                    break;
                case SamplingFilter.RgbShift:
                    expandSize.x += 40;
                    expandSize.y += 40;
                    break;
            }

            switch (transitionFilter)
            {
                case TransitionFilter.Melt:
                case TransitionFilter.Burn:
                    expandSize.y += 40;
                    break;
            }

            return expandSize;
        }

        private static void ModifyVertex(bool isTextMeshPro, ref UIVertex vt, Vector2 pos, Vector2 uv, Rect uvMask,
            Rect rect, Matrix4x4 m)
        {
            vt.position = pos;
            pos = m.MultiplyPoint3x4(pos);
            var normalizedX = Mathf.InverseLerp(rect.xMin, rect.xMax, pos.x);
            var normalizedY = Mathf.InverseLerp(rect.yMin, rect.yMax, pos.y);

            if (isTextMeshPro)
            {
                vt.uv0.x = uv.x;
                vt.uv0.y = uv.y;
                vt.uv1.z = normalizedX;
                vt.uv1.w = normalizedY;
                vt.uv2 = new Vector4(uvMask.xMin, uvMask.yMin, uvMask.xMax, uvMask.yMax);
            }
            else
            {
                vt.uv0 = new Vector4(uv.x, uv.y, normalizedX, normalizedY);
                vt.uv1 = new Vector4(uvMask.xMin, uvMask.yMin, uvMask.xMax, uvMask.yMax);
            }
        }

        private static void GetBounds(List<UIVertex> verts, int start, int count, out Rect posBounds,
            out Rect uvBounds)
        {
            var minPos = new Vector2(float.MaxValue, float.MaxValue);
            var maxPos = new Vector2(float.MinValue, float.MinValue);
            var minUV = new Vector2(float.MaxValue, float.MaxValue);
            var maxUV = new Vector2(float.MinValue, float.MinValue);
            for (var i = start; i < start + count; i++)
            {
                var vt = verts[i];
                var uv = vt.uv0;
                var pos = vt.position;

                // Left-Bottom
                if (minPos.x >= pos.x && minPos.y >= pos.y)
                {
                    minPos = pos;
                }
                // Right-Top
                else if (maxPos.x <= pos.x && maxPos.y <= pos.y)
                {
                    maxPos = pos;
                }

                // Left-Bottom
                if (minUV.x >= uv.x && minUV.y >= uv.y)
                {
                    minUV = uv;
                }
                // Right-Top
                else if (maxUV.x <= uv.x && maxUV.y <= uv.y)
                {
                    maxUV = uv;
                }
            }

            // Shrink coordinate to avoid uv edge
            posBounds = new Rect(minPos.x + 0.001f, minPos.y + 0.001f,
                maxPos.x - minPos.x - 0.002f, maxPos.y - minPos.y - 0.002f);
            uvBounds = new Rect(minUV.x, minUV.y, maxUV.x - minUV.x, maxUV.y - minUV.y);
        }

        private static void ApplyMirror(List<UIVertex> verts, int count, float rate, Vector2 range, float scale,
            float offset, float alpha)
        {
            rate = Mathf.Clamp01(rate);
            var start = 0;
            var end = count;
            ApplyShadowZeroAlloc(verts, ref start, ref end, 0, 0, alpha);

            for (var i = 0; i < count; i += 6)
            {
                var lb = s_WorkingVertices[i];
                var lbRate = Mathf.InverseLerp(range.x, range.y, lb.position.y);
                var lt = s_WorkingVertices[i + 1];
                var ltRate = Mathf.InverseLerp(range.x, range.y, lt.position.y);
                var rt = s_WorkingVertices[i + 2];
                var rtRate = Mathf.InverseLerp(range.x, range.y, rt.position.y);
                var rb = s_WorkingVertices[i + 4];
                var rbRate = Mathf.InverseLerp(range.x, range.y, rb.position.y);

                lb.color.a = (byte)(Mathf.InverseLerp(rate, 0, lbRate) * lb.color.a);
                lt.color.a = (byte)(Mathf.InverseLerp(rate, 0, ltRate) * lt.color.a);
                rt.color.a = (byte)(Mathf.InverseLerp(rate, 0, rtRate) * rt.color.a);
                rb.color.a = (byte)(Mathf.InverseLerp(rate, 0, rbRate) * rb.color.a);

                if (lbRate < rate && rate < ltRate)
                {
                    var t = (rate - lbRate) / (ltRate - lbRate);
                    lt.position = Vector3.Lerp(lb.position, lt.position, t);
                    lt.uv0 = Vector4.Lerp(lb.uv0, lt.uv0, t);
                    lt.uv1 = Vector4.Lerp(lb.uv1, lt.uv1, t);
                    lt.uv2 = Vector4.Lerp(lb.uv2, lt.uv2, t);
                    lt.color = Color.Lerp(lb.color, lt.color, t);
                }

                if (rbRate < rate && rate < rtRate)
                {
                    var t = (rate - rbRate) / (rtRate - rbRate);
                    rt.position = Vector3.Lerp(rb.position, rt.position, t);
                    rt.uv0 = Vector4.Lerp(rb.uv0, rt.uv0, t);
                    rt.uv1 = Vector4.Lerp(rb.uv1, rt.uv1, t);
                    rt.uv2 = Vector4.Lerp(rb.uv2, rt.uv2, t);
                    rt.color = Color.Lerp(rb.color, rt.color, t);
                }


                lb.position.y = -lb.position.y * scale + offset;
                lt.position.y = -lt.position.y * scale + offset;
                rt.position.y = -rt.position.y * scale + offset;
                rb.position.y = -rb.position.y * scale + offset;

                s_WorkingVertices[i] = s_WorkingVertices[i + 5] = lb;
                s_WorkingVertices[i + 1] = lt;
                s_WorkingVertices[i + 2] = s_WorkingVertices[i + 3] = rt;
                s_WorkingVertices[i + 4] = rb;
            }
        }

        /// <summary>
        /// Append shadow vertices.
        /// * It is similar to Shadow component implementation.
        /// </summary>
        private static void ApplyShadow(List<UIVertex> verts, ref int start, ref int end, Vector2 distance,
            ShadowMode mode, float alpha)
        {
            if (mode == ShadowMode.None) return;

            var x = distance.x;
            var y = distance.y;
            switch (mode)
            {
                case ShadowMode.Shadow:
                    ApplyShadowZeroAlloc(verts, ref start, ref end, x, y, alpha);
                    break;
                case ShadowMode.Shadow3:
                    ApplyShadowZeroAlloc(verts, ref start, ref end, x, y, alpha);
                    ApplyShadowZeroAlloc(verts, ref start, ref end, x, 0, alpha);
                    ApplyShadowZeroAlloc(verts, ref start, ref end, 0, y, alpha);
                    break;
                case ShadowMode.Outline:
                    ApplyShadowZeroAlloc(verts, ref start, ref end, x, y, alpha);
                    ApplyShadowZeroAlloc(verts, ref start, ref end, x, -y, alpha);
                    ApplyShadowZeroAlloc(verts, ref start, ref end, -x, y, alpha);
                    ApplyShadowZeroAlloc(verts, ref start, ref end, -x, -y, alpha);
                    break;
                case ShadowMode.Outline8:
                    ApplyShadowZeroAlloc(verts, ref start, ref end, x, y, alpha);
                    ApplyShadowZeroAlloc(verts, ref start, ref end, x, -y, alpha);
                    ApplyShadowZeroAlloc(verts, ref start, ref end, -x, y, alpha);
                    ApplyShadowZeroAlloc(verts, ref start, ref end, -x, -y, alpha);
                    ApplyShadowZeroAlloc(verts, ref start, ref end, x, 0, alpha);
                    ApplyShadowZeroAlloc(verts, ref start, ref end, 0, y, alpha);
                    ApplyShadowZeroAlloc(verts, ref start, ref end, -x, 0, alpha);
                    ApplyShadowZeroAlloc(verts, ref start, ref end, 0, -y, alpha);
                    break;
            }
        }

        /// <summary>
        /// Append shadow vertices.
        /// * It is similar to Shadow component implementation.
        /// </summary>
        private static void ApplyShadowZeroAlloc(List<UIVertex> verts, ref int start, ref int end, float x, float y,
            float alpha)
        {
            var count = end - start;
            for (var i = 0; i < count; i++)
            {
                // The original vertices is pushed backward.
                verts.Add(verts[end - count + i]);

                // Set shadow vertex.
                var vt = verts[start + i];
                vt.position.x += x;
                vt.position.y += y;
                vt.color.a = (byte)(alpha * vt.color.a);
                verts[start + i] = vt;
            }

            // Update next shadow offset.
            start = end;
            end = verts.Count;
        }
    }
}
