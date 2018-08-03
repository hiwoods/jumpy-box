using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System.Linq;

/*
 * Clear all objects that are out of screen (lower than minimum Y)
 */
[AlwaysUpdateSystem]
public class RemoveObjectSystem : ComponentSystem {

    struct ScrollableObjectData
    {
        public int Length;
        public ComponentArray<Transform> Tranforms;
        public ComponentArray<ScrollData> ScrollDataArray;
    }

    [Inject] private ScrollableObjectData ScrollableEntities;

    protected override void OnUpdate()
    {
        if (ScrollableEntities.Length == 0)
            return;

        ISettings settings = Bootstrap.Settings;

        var transformList = ScrollableEntities.Tranforms.ToArray()
            .Where(transform => transform.position.y < settings.Deadline)
            .ToArray();

        DestroyTransforms(transformList);
    }

    public void ClearAllObjects()
    {
        DestroyTransforms(ScrollableEntities.Tranforms.ToArray());
    }
    
    private void DestroyTransforms(Transform[] list)
    {
        System.Array.ForEach(list, transform => Object.Destroy(transform.gameObject));
    }
}
