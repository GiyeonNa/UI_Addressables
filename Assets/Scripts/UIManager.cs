using System.Collections.Generic;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UIManager : MonoSingleton<UIManager>
{
    private Dictionary<string, GameObject> cachedUIs = new Dictionary<string, GameObject>();

    [SerializeField]
    private List<string> preloadUIKeys;

    private void Awake()
    {
        LoadPreloadUIKeysByLabel("UI");
        LoadPreloadUIKeysByLabel("Image");
        PreloadUIs();
    }

    private void LoadPreloadUIKeysByLabel(string label)
    {
        preloadUIKeys = new List<string>();

        // Automatically fetch Addressables keys based on the provided label
        Addressables.LoadResourceLocationsAsync(label).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var location in handle.Result)
                {
                    preloadUIKeys.Add(location.PrimaryKey);
                }
            }
            else
            {
                Debug.LogError($"Failed to load keys with label: {label}");
            }
        };
    }

    private void PreloadUIs()
    {
        foreach (string uiKey in preloadUIKeys)
        {
            // Check if the UI is already cached
            if (cachedUIs.ContainsKey(uiKey))
            {
                Debug.Log($"UI already cached: {uiKey}, skipping preload.");
                continue;
            }

            // Addressables�� UI ������ �ε� (GameObject�� �������� ����)
            Addressables.LoadAssetAsync<GameObject>(uiKey).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    // �ε�� UI �����͸� ĳ��
                    cachedUIs[uiKey] = handle.Result;
                }
                else
                {
                    Debug.LogError($"Failed to preload UI: {uiKey}");
                }
            };
        }
    }

    public void ShowUI(string uiKey)
    {
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("No Canvas found in the scene.");
            return;
        }

        if (cachedUIs.ContainsKey(uiKey))
        {
            // �̹� �ε�� UI�� �ִٸ� Ȱ��ȭ
            GameObject uiPrefab = cachedUIs[uiKey];
            GameObject uiInstance = Instantiate(uiPrefab);
            uiInstance.transform.SetParent(canvas.transform, false); // Set as child of canvas
            uiInstance.GetComponent<UIPopup>().Open();
        }
        else
        {
            // Addressables�� UI �ε�
            Addressables.LoadAssetAsync<GameObject>(uiKey).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject uiInstance = Instantiate(handle.Result);
                    uiInstance.transform.SetParent(canvas.transform, false); // Set as child of canvas
                    cachedUIs[uiKey] = handle.Result; // ĳ�̵� ������ ������Ʈ
                    uiInstance.SetActive(true);
                }
                else
                {
                    Debug.LogError($"Failed to load UI: {uiKey}");
                }
            };
        }
    }

    public void HideUI(string uiKey, GameObject uiInstance)
    {
        if (cachedUIs.ContainsKey(uiKey))
        {
            cachedUIs[uiKey] = uiInstance;
            uiInstance.transform.SetParent(transform, false);
        }
    }
}
