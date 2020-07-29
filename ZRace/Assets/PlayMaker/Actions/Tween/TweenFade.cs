// (c) Copyright HutongGames, LLC. All rights reserved.
// See also: EasingFunctionLicense.txt

using System;
using UnityEngine;
using UnityEngine.UI;
using HutongGames.PlayMaker.TweenEnums;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Tween)]
    [Tooltip("Fades a GameObject with a Material, Sprite, Image, Text, Light, AudioSource, or CanvasGroup component.")]
    public class TweenFade : TweenActionBase
    {
        private const string supportedComponents = "Material, Sprite, Image, Text, Light, AudioSource, or CanvasGroup component.";

        public enum TargetType { None, Material, Sprite, Image, Text, Light, AudioSource, CanvasGroup}

        [Tooltip("A GameObject with a " + supportedComponents)]
        public FsmOwnerDefault gameObject;

        [Tooltip("Fade To or From value.")]
        public TweenDirection tweenDirection;

        [Tooltip("Value to fade to. E.g., alpha if fading an image, volume if fading audio...")]
        public FsmFloat value;

        public GameObject cachedGameObject;
        public Component cachedComponent;

        private TargetType targetType;
        private Material material;
        private SpriteRenderer spriteRenderer;
        private Text text;
        private Image image;
        private Light light;
        private CanvasGroup canvasGroup;
        private AudioSource audioSource;

        private float StartValue;
        private float EndValue;

        public override void Reset()
        {
            base.Reset();

            tweenDirection = TweenDirection.To;
            value = null;

            gameObject = null;
            cachedGameObject = null;
            cachedComponent = null;
        }

        private void UpdateCache(GameObject go)
        {
            cachedGameObject = go;
            if (go == null)
            {
                cachedComponent = null;
                return;
            }

            FindComponent(typeof(MeshRenderer), typeof(Image), typeof(Text), 
                typeof(Light), typeof(SpriteRenderer), typeof(AudioSource), typeof(CanvasGroup));
        }

        private void FindComponent(params Type[] components)
        {
            foreach (var component in components)
            {
                cachedComponent = cachedGameObject.GetComponent(component);
                if (cachedComponent != null) return;
            }
        }

        private void CheckCache()
        {
            var go = Fsm.GetOwnerDefaultTarget(gameObject);
            if (cachedGameObject != go)
            {
                UpdateCache(go);
            }

            InitTarget();
        }

        private void InitTarget()
        {
            targetType = TargetType.None;

            var renderer = cachedComponent as MeshRenderer;
            if (renderer != null)
            {
                targetType = TargetType.Material;
                material = renderer.material;
                return;
            }

            image = cachedComponent as Image;
            if (image != null)
            {
                targetType = TargetType.Image;
                return;
            }

            spriteRenderer = cachedComponent as SpriteRenderer;
            if (spriteRenderer != null)
            {
                targetType = TargetType.Sprite;
                return;
            }

            text = cachedComponent as Text;
            if (text != null)
            {
                targetType = TargetType.Text;
                return;
            }

            light = cachedComponent as Light;
            if (light != null)
            {
                targetType = TargetType.Light;
                return;
            }

            audioSource = cachedComponent as AudioSource;
            if (audioSource != null)
            {
                targetType = TargetType.AudioSource;
                return;
            }

            canvasGroup = cachedComponent as CanvasGroup;
            if (canvasGroup != null)
            {
                targetType = TargetType.CanvasGroup;
            }
        }

        public override void OnEnter()
        {
            CheckCache();

            if (targetType == TargetType.None)
            {
                LogWarning("GameObject needs a " + supportedComponents);
                Enabled = false;
                return;
            }

            if (tweenDirection == TweenDirection.From)
            {
                StartValue = value.Value;
                EndValue = GetTargetFade();
            }
            else
            {
                StartValue = GetTargetFade();
                EndValue = value.Value;
            }

            base.OnEnter();

            DoTween();
        }
     
        private float GetTargetFade()
        {
            switch (targetType)
            {
                case TargetType.None: return 1f;
                case TargetType.Material: return material.color.a;
                case TargetType.Sprite: return spriteRenderer.color.a;
                case TargetType.Image: return image.color.a;
                case TargetType.Text: return text.color.a;
                case TargetType.Light: return light.intensity;
                case TargetType.CanvasGroup: return canvasGroup.alpha;
                case TargetType.AudioSource: return audioSource.volume;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetTargetFade(float fade)
        {
            Color color;

            switch (targetType)
            {
                case TargetType.None: 
                    break;
                case TargetType.Material:
                    color = material.color;
                    color.a = fade;
                    material.color = color;
                    break;
                case TargetType.Sprite:
                    color = spriteRenderer.color;
                    color.a = fade;
                    spriteRenderer.color = color;
                    break;
                case TargetType.Image:
                    color = image.color;
                    color.a = fade;
                    image.color = color;
                    break;
                case TargetType.Text:
                    color = text.color;
                    color.a = fade;
                    text.color = color;
                    break;
                case TargetType.Light:
                    light.intensity = fade;
                    break;
                case TargetType.AudioSource:
                    audioSource.volume = fade;
                    break;
                case TargetType.CanvasGroup:
                    canvasGroup.alpha = fade;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void DoTween()
        {
            var lerp = easingFunction(0, 1, normalizedTime);
            SetTargetFade( Mathf.Lerp(StartValue, EndValue, lerp));
        }

#if UNITY_EDITOR

        public override string ErrorCheck()
        {
            CheckCache();

            if (targetType == TargetType.None)
            {
                return "GameObject needs a " + supportedComponents;
            }

            return "";
        }

        public override string AutoName()
        {
            return "TweenFade: " + ActionHelpers.GetValueLabel(Fsm, gameObject) + " " + tweenDirection + " " + ActionHelpers.GetValueLabel(value);
        }
#endif

    }

}
