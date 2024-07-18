using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using Newtonsoft.Json;

namespace Character
{
    public class CharacterCustomizationManager : MonoBehaviour
    {
        // 모든 리소스의 ID와 모델을 저장하고 있는 데이터
        public CharacterCustomizationCategory[] categories;

        // 현재 선택된 모델의 ID들을 저장하고 있는 데이터
        public CharacterCustomization customization;

        public Animator animator;

        [ContextMenu("Set Names")]
        void SetNames()
        {
            foreach (CharacterCustomizationCategory category in categories)
            {
                int optionCounter = 1;
                foreach (CharacterCustomizationOption option in category.options)
                {
                    option.name = option.model.name.Replace("_", " ");
                    option.id = category.name + "_" + optionCounter;
                    option.description = option.model.name.Replace("_", " ");
                    optionCounter++;
                }
            }
        }

        [ContextMenu("Set Icons")]
        void SetIcons()
        {
            foreach (CharacterCustomizationCategory category in categories)
            {
                foreach (CharacterCustomizationOption option in category.options)
                {
                    option.icon = Resources.Load<Sprite>("Sprites/" + option.model.name);
                }
            }
        }

        // 모델 아이콘이 클릭되었을때 모델을 그에 맞게 변경 하는 코드
        public void SelectOption(string categoryID, string optionID)
        {
            if (customization.selectedOptions.ContainsKey(categoryID))
            {
                customization.selectedOptions[categoryID] = optionID;
            }
            else
            {
                customization.selectedOptions.Add(categoryID, optionID);
            }

            ApplyCustomization();
        }

        private void OnDisable()
        {
            customization.Save();
        }

        private void Start()
        {
            customization = new CharacterCustomization();

            SetDefault();

            foreach (CharacterCustomizationCategory category in categories)
            {
                foreach (CharacterCustomizationOption option in category.options)
                {
                    option.model.SetActive(false);
                }
            }

            ApplyCustomization();
            UI_CusomizationManager.Instance.Initialize(this);
        }

        [ContextMenu("Apply Customization")]
        public void ApplyCustomization()
        {
            // 아바타에 붙어 있는 모든 아이템을 Active False 시킨다.
            foreach (CharacterCustomizationCategory category in categories)
            {
                foreach (CharacterCustomizationOption option in category.options)
                {
                    option.model.SetActive(false);
                }
            }

            // 세팅되어 있는 아이템만 Active True로 변경해준다
            foreach (CharacterCustomizationCategory category in categories)
            {
                if (customization.selectedOptions.ContainsKey(category.id))
                {
                    string optionID = customization.selectedOptions[category.id];
                    foreach (CharacterCustomizationOption option in category.options)
                    {
                        if (option.id == optionID)
                        {
                            option.model.SetActive(true);
                        }
                    }
                }
            }
        }

        void SetDefault()
        {
            foreach (var category in categories)
            {
                SelectOption(category.id, category.options[0].id);
            }
        }

        public void PlayAnimation()
        {
            animator.SetTrigger("Play");            
        }

        // 랜덤으로 캐릭터 외형 설정하기
        public void MakeRandom()
        {
            foreach (var category in categories)
            {
                int randomIndex = UnityEngine.Random.Range(0, category.options.Length);
                SelectOption(category.id, category.options[randomIndex].id);
            }
        }

        public void Save()
        {
            customization.Save();
        }

        public void Load()
        {
            customization.Load();
            ApplyCustomization();
        }
    }

    [Serializable]
    public class CharacterCustomization
    {
        public Dictionary<string, string> selectedOptions;

        public CharacterCustomization()
        {
            selectedOptions = new Dictionary<string, string>();
        }

        void ToJson()
        {
            string json = JsonUtility.ToJson(this);
            Debug.Log(json);
        }

        void FromJson(string json)
        {
            CharacterCustomization customization = JsonUtility.FromJson<CharacterCustomization>(json);
            selectedOptions = customization.selectedOptions;
        }

        public void Save()
        {
            //string json = JsonUtility.ToJson(this);
            string json = JsonConvert.SerializeObject(this);
            PlayerPrefs.SetString("CharacterCustomization", json);
            Debug.Log(json);
        }

        public void Load()
        {
            string json = PlayerPrefs.GetString("CharacterCustomization");
            Debug.Log(json);

            //CharacterCustomization customization = JsonUtility.FromJson<CharacterCustomization>(json);
            CharacterCustomization customization = JsonConvert.DeserializeObject<CharacterCustomization>(json);
            selectedOptions = customization.selectedOptions;
        }
    }

    [Serializable]
    public class Data
    {
        public string name;
        public string id;
        public string description;
    }

    [Serializable]
    public class CharacterCustomizationCategory : Data
    {
        public CharacterCustomizationOption[] options;
    }

    [Serializable]
    public class CharacterCustomizationOption : Data
    {
        public Sprite icon;
        public GameObject model;
    }
}