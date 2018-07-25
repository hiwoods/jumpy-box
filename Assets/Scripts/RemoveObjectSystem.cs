using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System.Linq;

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

        var transformList = new List<Transform>(ScrollableEntities.Tranforms.ToArray())
            .Where(transform => transform.position.y < settings.Deadline)
            .ToList();

        DestroyTransforms(transformList);
    }

    public void ClearAllObjects()
    {
        var transforms = new List<Transform>(ScrollableEntities.Tranforms.ToArray());
        DestroyTransforms(transforms);
    }

    private void DestroyTransforms(List<Transform> list)
    {
        list.ForEach(transform => Object.Destroy(transform.gameObject));
    }
}
