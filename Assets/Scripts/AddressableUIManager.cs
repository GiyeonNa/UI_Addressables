using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableUIManager : MonoSingleton<AddressableUIManager>
{
    private Dictionary<string, GameObject> cachedUIs = new Dictionary<string, GameObject>();

    // Add: Dictionary to track instantiated UI instances
    private Dictionary<string, GameObject> uiInstances = new Dictionary<string, GameObject>();

    private void Awake()
    {
        LoadPreloadUIKeysByLabel("UI");
    }

    private void LoadPreloadUIKeysByLabel(string label)
    {
        Addressables.LoadResourceLocationsAsync(label).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var location in handle.Result)
                {
                    string uiKey = location.PrimaryKey;

                    Addressables.LoadAssetAsync<GameObject>(uiKey).Completed += assetHandle =>
                    {
                        if (assetHandle.Status == AsyncOperationStatus.Succeeded)
                        {
                            cachedUIs[uiKey] = assetHandle.Result;
                        }
                        else
                        {
                            Debug.LogError($"Failed to load UI asset for key: {uiKey}");
                        }
                    };
                }
            }
            else
            {
                Debug.LogError($"Failed to load resource locations with label: {label}");
            }
        };
    }

    public void ShowUI(string uiKey)
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("No Canvas found in the scene.");
            return;
        }

        // Check if an instance already exists and is parented to the manager (hidden)
        if (uiInstances.TryGetValue(uiKey, out GameObject uiInstance) && uiInstance != null)
        {
            // Reparent to canvas and activate
            uiInstance.transform.SetParent(canvas.transform, false);
            uiInstance.SetActive(true);
            uiInstance.GetComponent<UIPopup>()?.Open();
            return;
        }

        if (cachedUIs.TryGetValue(uiKey, out GameObject uiPrefab))
        {
            // Instantiate, parent to canvas, and track instance
            GameObject newInstance = Instantiate(uiPrefab, canvas.transform, false);
            uiInstances[uiKey] = newInstance;
            newInstance.SetActive(true);
            newInstance.GetComponent<UIPopup>()?.Open();
        }
        else
        {
            Addressables.LoadAssetAsync<GameObject>(uiKey).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject newInstance = Instantiate(handle.Result, canvas.transform, false);
                    cachedUIs[uiKey] = handle.Result;
                    uiInstances[uiKey] = newInstance;
                    newInstance.SetActive(true);
                    newInstance.GetComponent<UIPopup>()?.Open();
                }
                else
                {
                    Debug.LogError($"Failed to load UI: {uiKey}");
                }
            };
        }
    }

    // HideUI now takes a key, not an instance
    public void HideUI(string uiKey)
    {
        if (uiInstances.TryGetValue(uiKey, out GameObject uiInstance) && uiInstance != null)
        {
            // Reparent to manager and deactivate
            uiInstance.transform.SetParent(this.transform, false);
            uiInstance.SetActive(false);
        }
    }
}
