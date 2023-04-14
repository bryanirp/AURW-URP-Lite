/*
    ########################################################
    ######                                           #######
    ###### ABKaspo's Ultra Realistic Water (A.U.R.W.)#######
    ######                                           #######
    ########################################################
    This script is competly yours you can modify how you want
    it's very basic, we know but it's the intention that counts, right? XD

 */
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace ABKaspo.Assets.AURW.Lite.OptionsManager
{
    public class AURW_OceanLiteOptionManager : MonoBehaviour
    {
        // Here Public Values
        public Material waterMaterial;
        public Slider smoothnessSlider;
        public Slider nomralStrengthSlider;
        public Slider depthSlider;
        public Slider depthStrengthSlider;
        public Slider fresnelPowerSlider;
        public TextMeshProUGUI smoothnessValue;
        public TextMeshProUGUI normalStrengthValue;
        public TextMeshProUGUI depthValue;
        public TextMeshProUGUI depthStrenghtValue;
        public TextMeshProUGUI fresnelEffectValue;
        
        // Here Local Values
        float _EasyAlbedo = 0.0f;
        float _Fresnel = 1.0f;
        float _Alpha = 1.0f;
        float _SecondNormalRender = 1.0f;
        float _NormalMapping = 1.0f;
        float _Animation = 1.0f;
        void Start()
        {
            //Debug.Log("Thanks For Purchase AURW Lite URP Edition");
            Debug.Log("AURW Script Manager: OceanLiteOptionManager is working!");
        }
        void Update()
        {
            waterMaterial.SetFloat("_Smoothness", smoothnessSlider.value / 100);
            waterMaterial.SetFloat("_Normal_Strength", nomralStrengthSlider.value / 4);
            waterMaterial.SetFloat("_Depth", depthSlider.value/100);
            waterMaterial.SetFloat("_Depth_Strength", depthStrengthSlider.value/100);
            waterMaterial.SetFloat("_FresnelPower", fresnelPowerSlider.value/100);
            waterMaterial.SetFloat("_Easy_Albedo", _EasyAlbedo);
            waterMaterial.SetFloat("_Fresnel", _Fresnel);
            waterMaterial.SetFloat("_Alpha", _Alpha);
            waterMaterial.SetFloat("_Second_Normal_Render", _SecondNormalRender);
            waterMaterial.SetFloat("_Normal_Mapping", _NormalMapping);
            waterMaterial.SetFloat("_Animation", _Animation);
            smoothnessValue.text = waterMaterial.GetFloat("_Smoothness").ToString();
            normalStrengthValue.text = waterMaterial.GetFloat("_Normal_Strength").ToString();
            depthValue.text = waterMaterial.GetFloat("_Depth").ToString();
            depthStrenghtValue.text = waterMaterial.GetFloat("_Depth_Strength").ToString();
            fresnelEffectValue.text = waterMaterial.GetFloat("_FresnelPower").ToString();
        }
        public void AddFloat(float a, float b)
        {
            // a + b
            a += b;
        }
        public void EasyAlbedoManager()
        {
            _EasyAlbedo += 1.0f;
            if (_EasyAlbedo == 2.0f)
            {
                _EasyAlbedo = 0.0f;
            }
        }
        public void FresnelManager()
        {
            _Fresnel += 1.0f;
            if (_Fresnel == 2.0f)
            {
                _Fresnel = 0.0f;
            }
        }
        public void AlphaManager()
        {
            _Alpha += 1.0f;
            if (_Alpha == 2.0f)
            {
                _Alpha = 0.0f;
            }
        }
        public void SecondNormalManager()
        {
            _SecondNormalRender += 1.0f;
            if (_SecondNormalRender == 2.0f)
            {
                _SecondNormalRender = 0.0f;
            }
        }
        public void NomralMappingManager()
        {
            _NormalMapping += 1.0f;
            if (_NormalMapping == 2.0f)
            {
                _NormalMapping = 0.0f;
            }
        }
        public void AnimationManager()
        {
            _Animation += 1.0f;
            if (_Animation == 2.0f)
            {
                _Animation = 0.0f;
            }
        }
        public void ResetMaterialValues()
        {
            smoothnessSlider.value = 95;
            nomralStrengthSlider.value = 3;
            depthStrengthSlider.value = 100;
            depthSlider.value = 100;
            fresnelPowerSlider.value = 100;
            _EasyAlbedo = 0.0f;
            _Fresnel = 1.0f;
            _Alpha = 1.0f;
            _SecondNormalRender = 1.0f;
            _NormalMapping = 1.0f;
            _Animation = 1.0f;
        }
    }
}