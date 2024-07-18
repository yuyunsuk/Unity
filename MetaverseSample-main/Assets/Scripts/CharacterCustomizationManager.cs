using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using Newtonsoft.Json;

namespace Character
{
    public class CharacterCustomizationManager : MonoBehaviour
    {
        // ��� ���ҽ��� ID�� ���� �����ϰ� �ִ� ������
        public CharacterCustomizationCategory[] categories;

        // ���� ���õ� ���� ID���� �����ϰ� �ִ� ������
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

        // �� �������� Ŭ���Ǿ����� ���� �׿� �°� ���� �ϴ� �ڵ�
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
            // �ƹ�Ÿ�� �پ� �ִ� ��� �������� Active False ��Ų��.
            foreach (CharacterCustomizationCategory category in categories)
            {
                foreach (CharacterCustomizationOption option in category.options)
                {
                    option.model.SetActive(false);
                }
            }

            // ���õǾ� �ִ� �����۸� Active True�� �������ش�
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

        // �������� ĳ���� ���� �����ϱ�
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