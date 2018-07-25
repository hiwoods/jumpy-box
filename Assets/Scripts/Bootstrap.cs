using UnityEngine;
using Unity.Entities;

public sealed class Bootstrap : MonoBehaviour
{
    public static ISettings Settings;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void InitializeAfterSceneLoad()
    {
        var settingsObject = GameObject.Find("Settings");
        
        if (!settingsObject)
        {
            Debug.LogError("Settings object does not exist or cannot be found");
            return;
        }

        ISettings settingComponent = settingsObject.GetComponent<ISettings>();

        if (settingComponent == null)
        {
            Debug.LogError("Settings Object doesn't contain ISettings Component.");
            return;
        }

        Settings = settingComponent;
        GetSystem<UpdateHUDSystem>().SetupGameObjects();
    }

    public static void NewGame()
    {
        //clear all removable objs
        GetSystem<RemoveObjectSystem>().ClearAllObjects();

        //populate scene
        GameObject playerObj = Instantiate(Settings.PlayerPrefab, new Vector3(0, -2.4f, 0), Quaternion.identity);
        GameObject baseObj = Instantiate(Settings.BasePrefab, new Vector3(0, -3f, 0), Quaternion.identity);

        playerObj.name = Settings.PlayerPrefab.name;
        baseObj.name = Settings.BasePrefab.name;

        GetSystem<UpdateHUDSystem>().UpdateReset();
    }

    public static T GetSystem<T>() where T : ComponentSystem
    {
        return World.Active.GetExistingManager<T>();
    }
}
